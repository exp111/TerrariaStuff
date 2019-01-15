using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SoulShards.TileEntities
{
	class TESoulStatue : ModTileEntity
	{
		public Soul soul = null;

		public override bool ValidTile(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			return tile.active() && tile.type == mod.TileType<Tiles.SoulStatue>();
		}

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
		{
			j--; //cuz it's two wide
			if (Main.netMode == 1)
			{
				NetMessage.SendTileRange(Main.myPlayer, i, j, 2, 2);
				NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, type);
				return -1;
			}
			int id = Place(i, j);
			return id;
		}
	}
}
