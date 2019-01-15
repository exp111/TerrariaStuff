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
		public double lastActivated = 0;
		public const int delay = 1;

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

			// Get the Tile Entity
			TESoulStatue tileEntity = (TESoulStatue)TileEntity.ByPosition[new Point16(i, j)];
			if (tileEntity == null)
			{
				Main.NewText("Can't find Tile Entity.", Color.Red);
				return;
			}

			// Try to get Soul Shard from item
			Item item = player.inventory[player.selectedItem];
			SoulShard shard = SoulShard.GetFromItem(item);
			
			if (shard != null) // shard in hand
			{
				player.tileInteractionHappened = true;

				if (shard.soul.ID == 0) // shard not bound to an enemy
				{
					Main.NewText(String.Format("First kill a enemy with the Soul Shard equipped and kill {0} Enemies.", shard.neededKills), Color.Red);
					return;
				}

				if (shard.soul.kills < shard.neededKills) // shard has not enough kills
				{
					Main.NewText(String.Format("You need {0} more kills.", shard.neededKills - shard.soul.kills), Color.Red);
					return;
				}

				if (tileEntity.soul != null) // already got a soul
				{
					Main.NewText("Statue already has a soul.", Color.Red);
					return;
				}

				// decrease stack
				if (--item.stack <= 0)
				{
					item.SetDefaults(0);
				}
				if (player.selectedItem == 58) // main.mouseItem == player.inventory[player.selectedItem]
				{
					Main.mouseItem = item.Clone();
				}

				// transfer soul
				tileEntity.soul = shard.soul;
			}

			// Print info about the statue
			if (tileEntity.soul == null)
			{
				Main.NewText("No Soul.");
			}
			else
			{
				//TODO: maybe check for shift? then drop the soul
				KeyboardState keyboard = Keyboard.GetState();
				if (keyboard.IsKeyDown(Keys.LeftShift)) // drop soul
				{
					Vector2 worldCord = new Vector2(i, j).ToWorldCoordinates();
					//TODO: we need to save tier
					int newItem = Item.NewItem((int)Math.Round(worldCord.X), (int)Math.Round(worldCord.Y), 16, 16, mod.ItemType<SoulShard1>());
					((SoulShard)Main.item[newItem].modItem).soul = tileEntity.soul;

					tileEntity.soul = null;
				}
				else // print info
				{
					Main.NewText(String.Format("Soul ID: {0}, Kills: {1}.", tileEntity.soul.name, tileEntity.soul.kills));
				}
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
				if (Main.time - lastActivated >= delay)
				{
					Vector2 worldCord = new Vector2(i, j).ToWorldCoordinates();
					NPC.NewNPC((int)Math.Round(worldCord.X), (int)Math.Round(worldCord.Y), tileEntity.soul.ID);
					lastActivated = Main.time;
				}
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
