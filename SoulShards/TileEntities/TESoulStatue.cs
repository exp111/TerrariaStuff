using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

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

		public override void Load(TagCompound tag)
		{
			soul = Soul.Load(tag);
		}

		public override TagCompound Save()
		{
			if (soul == null)
				return new TagCompound();
			return soul.Serialize();
		}

		public override void NetSend(BinaryWriter writer, bool lightSend)
		{
			soul.NetSend(writer);
		}
		public override void NetReceive(BinaryReader reader, bool lightSend)
		{
			soul = Soul.NetReceive(reader);
		}

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
		{
			// i - 1 and j - 2 come from the fact that the origin of the tile is "new Point16(1, 2);", so we need to pass the coordinates back to the top left tile.
			if (Main.netMode == 1)
			{
				NetMessage.SendTileRange(Main.myPlayer, i - 1, j - 1, 2, 2);
				NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i - 1, j - 2, type);
				return -1;
			}
			int id = Place(i - 1, j - 2);
			return id;
		}
	}
}
