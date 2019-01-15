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
