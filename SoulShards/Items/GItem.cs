using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SoulShards.Items
{
	public class GItem : GlobalItem
	{
		public int killedType;
		private const string killedTypeString = "killedType";
		public int kills;
		private const string killsString = "kills";

		public GItem()
		{
			killedType = 0;
			kills = 0;
		}

		public override bool InstancePerEntity
		{
			get
			{
				return true;
			}
		}

		public override GlobalItem Clone(Item item, Item itemClone)
		{
			GItem myClone = (GItem)base.Clone(item, itemClone);
			myClone.killedType = killedType;
			myClone.kills = kills;
			return myClone;
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (item.type != mod.ItemType<SoulShard>())
				return;

			if (killedType > 0 && kills > 0)
			{
				tooltips.Add(
					new TooltipLine(mod, "SoulShard", String.Format("Bound to {0}. Kills: {1}", killedType, kills)));
			}
		}

		public override void Load(Item item, TagCompound tag)
		{
			killedType = tag.GetAsInt(killedTypeString);
			kills = tag.GetAsInt(killsString);
		}

		public override bool NeedsSaving(Item item)
		{
			return killedType > 0 && item.type == mod.ItemType<SoulShard>();
		}

		public override TagCompound Save(Item item)
		{
			return new TagCompound {
				{killedTypeString, killedType},
				{killsString, kills}
			};
		}

		public override void NetSend(Item item, BinaryWriter writer)
		{
			writer.Write(killedType);
			writer.Write(kills);
		}

		public override void NetReceive(Item item, BinaryReader reader)
		{
			killedType = reader.ReadInt32();
			kills = reader.ReadInt32();
		}

		public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
		{
			if (item.type != mod.ItemType<SoulShard>())
				return;

			if (!player.GetModPlayer<MPlayer>().soulShard)
				return;

			if (target.active) //ded?
				return;

			Main.NewText("Killed");
		}
	}
}