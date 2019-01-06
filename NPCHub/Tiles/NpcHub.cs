using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using NPCHub.UI;
using Terraria;
using Terraria.ID;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;

namespace NPCHub.Tiles
{
	public class NpcHub : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 18 };
			//TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<TEElementalPurge>().Hook_AfterPlacement, -1, 0, false);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Npc Hub");
			AddMapEntry(new Color(190, 230, 190), name);
			dustType = mod.DustType("Pixel");
			disableSmartCursor = true;
		}

		public override bool HasSmartInteract()
		{
			return true;
		}

		public override void RightClick(int i, int j)
		{

			Main.playerInventory = true;
			NPCHubUI.Visible = true;

			/*
			Player player = Main.LocalPlayer;

			var npcs = Main.npc.Where(npc => npc.active && npc.townNPC).OrderBy(npc => npc.FullName).ToList();
			Main.NewText(npcs.Count.ToString(), 255, 240, 20);
			foreach (NPC npc in npcs)
			{
				if (NPC.TypeToHeadIndex(npc.type) >= 0) // ignore old man & traveling
				{
					var dist = (npc.position - player.position).Length();

					string x = String.Format("Full Name: {0}, Distance: {1}, WhoAmI: {2}", npc.FullName, dist, npc.whoAmI);
					Main.NewText(x, 255, 240, 20);

					//Get him out of the inventory
					Main.playerInventory = false;
					player.chest = -1;
					//NPC Needs to be in range
					npc.Teleport(player.position);
					//Talk to NPC
					player.talkNPC = npc.whoAmI;
					//Set chat text
					Main.npcChatText = npc.GetChat();
					//Play bubble sound
					Main.PlaySound(24);
				}
			}
			*/

		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = mod.ItemType<Items.Placeable.NpcHub>();
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 48, mod.ItemType<Items.Placeable.NpcHub>());
		}
	}
}
