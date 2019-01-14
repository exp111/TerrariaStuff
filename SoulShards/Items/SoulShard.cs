using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SoulShards.Items
{
	public class SoulShard : ModItem
	{
		public Soul killed = new Soul();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Shard");
			Tooltip.SetDefault("Kill a enemy with this equipped.");
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

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override ModItem Clone(Item item)
		{
			SoulShard myClone = (SoulShard)base.Clone(item);
			myClone.killed = killed;
			return myClone;
		}

		public override void Load(TagCompound tag)
		{
			killed = Soul.Load(tag);
		}

		public override TagCompound Save()
		{
			return killed.Serialize();
		}

		public override void NetSend(BinaryWriter writer)
		{
			killed.NetSend(writer);
		}

		public override void NetRecieve(BinaryReader reader)
		{
			killed = Soul.NetReceive(reader);
		}

		public override void OnCraft(Recipe recipe)
		{
			killed = new Soul(); // Reset
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (!String.IsNullOrEmpty(killed.name) && killed.kills > 0)
			{
				tooltips.Add(
					new TooltipLine(mod, "SoulShard", String.Format("Bound to {0}. Kills: {1}", killed.name, killed.kills)));
			}
		}
	}
}
