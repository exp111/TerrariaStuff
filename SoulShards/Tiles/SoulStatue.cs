using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using SoulShards.Items;

namespace SoulShards.Tiles
{
	public class SoulStatue : ModTile
	{
		public Soul soul = null;

		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 18 };
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Soul Statue");
			AddMapEntry(new Color(190, 230, 190), name);
			dustType = mod.DustType("Pixel");
			disableSmartCursor = true;
		}

		public override void RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;
			if (player == null)
				return;

			if (player.HeldItem.type == mod.ItemType<SoulShard>())
			{
				SoulShard shard = (SoulShard)player.HeldItem.modItem;
				if (shard.killed.type == 0)
				{
					Main.NewText(String.Format("First kill a enemy with the Soul Shard equipped and kill {0} Enemies.", Soul.neededKills), Color.Red);
					return;
				}

				if (shard.killed.kills < Soul.neededKills)
				{
					Main.NewText(String.Format("You need {0} more kills.", Soul.neededKills - shard.killed.kills), Color.Red);
					return;
				}

				//TODO: transfer soul & remove item from inventory
			}
		}

		public override void HitWire(int i, int j)
		{
			//TODO: activate/deactive spawning
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			if (player == null)
				return;

			if (player.HeldItem.type == mod.ItemType<SoulShard>())
			{
				player.noThrow = 2;
				player.showItemIcon = true;
				player.showItemIcon2 = mod.ItemType<SoulShard>();
			}
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 48, mod.ItemType<Items.Placeable.SoulStatue>());
		}
	}
}
