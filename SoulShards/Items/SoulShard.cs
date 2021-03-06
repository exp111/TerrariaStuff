using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SoulShards.Items
{
	public abstract class SoulShard : ModItem
	{
		public Soul soul = new Soul();
		public abstract int neededKills { get; }
		public abstract int addPerKill { get; }
		public abstract String name { get; }

		public override string Texture { get { return (GetType().Namespace + "." + "SoulShard").Replace('.', '/'); } }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(name);
			Tooltip.SetDefault("Kill a enemy with this equipped or right click a statue.");
		}
		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 40;
			item.accessory = true;
			item.rare = ItemRarityID.Lime;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<MPlayer>().soulShard = this;
		}

		public abstract void GetRecipes();

		public override void AddRecipes()
		{
			GetRecipes();

			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override ModItem Clone(Item item)
		{
			SoulShard myClone = (SoulShard)base.Clone(item);
			myClone.soul = soul;
			return myClone;
		}

		public override void Load(TagCompound tag)
		{
			soul = Soul.Load(tag);
			if (soul == null)
				soul = new Soul();
		}

		public static SoulShard GetFromItem(Item item)
		{
			return item.modItem is SoulShard ? (SoulShard)item.modItem : null;
		}

		public override TagCompound Save()
		{
			if (soul == null)
				return new TagCompound();
			return soul.Serialize();
		}

		public override void NetSend(BinaryWriter writer)
		{
			soul.NetSend(writer);
		}

		public override void NetRecieve(BinaryReader reader)
		{
			soul = Soul.NetReceive(reader);
		}

		public override void OnCraft(Recipe recipe)
		{
			soul = new Soul(); // Reset //TODO: only if required == result
			item.prefix = 0;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (!String.IsNullOrEmpty(soul.name) && soul.kills > 0)
			{
				tooltips.Add(
					new TooltipLine(mod, 
					"SoulShard", 
					String.Format("Bound to {0}. Kills: {1}", soul.name, soul.kills)));

				//TODO: change tooltip if soul is full
			}
		}
	}

	public class SoulShard1 : SoulShard
	{
		public override int neededKills { get { return 50; } }
		public override int addPerKill { get { return 1; } }
		public override String name { get { return "Soul Shard 1"; } }

		public override void GetRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class SoulShardMax : SoulShard
	{
		public override int neededKills { get { return 50; } }
		public override int addPerKill { get { return neededKills; } }
		public override String name { get { return "Soul Shard Max"; } }

		public override void GetRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.GetItem<SoulShard1>());
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
