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
using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader.IO;
using System.IO;
using SoulShards.TileEntities;

namespace SoulShards.Tiles
{
	public class SoulStatue : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 18 };
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<TESoulStatue>().Hook_AfterPlacement, -1, 0, false);
			TileObjectData.addTile(Type);

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Soul Statue");
			AddMapEntry(new Color(190, 230, 190), name);
			dustType = mod.DustType("Pixel");
			disableSmartCursor = true;
		}

		public override void RightClick(int i, int j)
		{
			Player player = Main.player[Main.myPlayer];
			if (player == null)
				return;

			// We need to get to frameX == 0 && frameY == 0
			j -= Main.tile[i, j].frameY / 16;
			i -= Main.tile[i, j].frameX / 16;

			Item item = player.inventory[player.selectedItem];
			
			TESoulStatue tileEntity = null;
			SoulShard shard = SoulShard.GetFromItem(item);
			
			if (shard != null)
			{
				player.tileInteractionHappened = true;

				if (shard.soul.type == 0)
				{
					Main.NewText(String.Format("First kill a enemy with the Soul Shard equipped and kill {0} Enemies.", shard.neededKills), Color.Red);
					return;
				}

				if (shard.soul.kills < shard.neededKills)
				{
					Main.NewText(String.Format("You need {0} more kills.", shard.neededKills - shard.soul.kills), Color.Red);
					return;
				}

				//	decrease stack
				if (--item.stack <= 0)
				{
					item.SetDefaults(0);
				}
				if (player.selectedItem == 58) // main.mouseItem == player.inventory[player.selectedItem]
				{
					Main.mouseItem = item.Clone();
				}

				// transfer soul
				tileEntity = (TESoulStatue)TileEntity.ByPosition[new Point16(i, j)];
				if (tileEntity == null)
				{
					Main.NewText("Can't find Tile Entity.", Color.Red);
					return;
				}
				tileEntity.soul = shard.soul;
			}

			tileEntity = (TESoulStatue)TileEntity.ByPosition[new Point16(i, j)];
			if (tileEntity == null)
			{
				Main.NewText("Can't find Tile Entity.", Color.Red);
				return;
			}
			if (tileEntity.soul == null)
			{
				Main.NewText("No Soul.");
			}
			else
			{
				//TODO: maybe check for shift? then drop the soul
				Main.NewText(String.Format("Soul Type: {0}, Kills: {1}.", tileEntity.soul.name, tileEntity.soul.kills));
			}
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.player[Main.myPlayer];
			if (player == null)
				return;

			Item item = player.inventory[player.selectedItem];
			SoulShard shard = SoulShard.GetFromItem(item);
			if (shard != null)
			{
				player.noThrow = 2;
				player.showItemIcon = true;
				player.showItemIcon2 = mod.ItemType<SoulShard>();
			}
		}

		public override void HitWire(int i, int j)
		{
			// We need to get to frameX == 0 && frameY == 0
			j -= Main.tile[i, j].frameY / 16;
			i -= Main.tile[i, j].frameX / 16;
			
			TESoulStatue tileEntity = (TESoulStatue)TileEntity.ByPosition[new Point16(i, j)];

			//TODO: activate/deactive spawning
			if (tileEntity.soul != null)
			{
				Main.NewText("Hit by wire with Soul.");
			}
			else
			{
				Main.NewText("Hit by wire without Soul.");
			}
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			//TODO: drop soul if existing
			Item.NewItem(i * 16, j * 16, 32, 48, mod.ItemType<Items.Placeable.SoulStatue>());
		}
	}
}
