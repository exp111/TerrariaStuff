using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NPCPrisonBuilder
{
	public class NPCPrisonBuilder : Mod
	{
		internal static NPCPrisonBuilder Instance { get; private set; }

		public NPCPrisonBuilder()
		{
		}

		public override void Load()
		{
			Instance = this;
		}

		public override void Unload()
		{
			Instance = null;
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			switch (reader.ReadInt32())
			{
				case 0:
					Items.PrisonBuilder.HandleBuilding(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadBoolean());
					return;
				default:
					return;
			}
		}
	}

	internal static class TileChecks
	{
		internal static void ClearEverything(int x, int y)
		{
			TileChecks.FindChestTopLeft(x, y, true);
			TileChecks.ClearTile(x, y);
			TileChecks.ClearWall(x, y);
			TileChecks.ClearLiquid(x, y);
		}

		internal static Point16 FindChestTopLeft(int x, int y, bool destroy)
		{
			if (!TileID.Sets.BasicChest[(int)Main.tile[x, y].type])
			{
				return Point16.NegativeOne;
			}
			Tile tile = Main.tile[x, y];
			if (((tile != null) ? new short?(tile.frameX) : null) % 36 != 0)
			{
				x--;
			}
			Tile tile2 = Main.tile[x, y];
			if (((tile2 != null) ? new short?(tile2.frameY) : null) % 36 != 0)
			{
				y--;
			}
			if (!destroy)
			{
				return new Point16(x, y);
			}
			TileChecks.DestroyChest(x, y);
			return new Point16(x, y);
		}

		internal static void DestroyChest(int x, int y)
		{
			int num = Chest.FindChest(x, y);
			int number = 1;
			if (num != -1)
			{
				for (int i = 0; i < 40; i++)
				{
					Main.chest[num].item[i] = new Item();
				}
				Main.chest[num] = null;
				if (Main.tile[x, y].type == 467)
				{
					number = 5;
				}
				if (Main.tile[x, y].type >= 470)
				{
					number = 101;
				}
			}
			for (int j = x; j < x + 2; j++)
			{
				for (int k = y; k < y + 2; k++)
				{
					TileChecks.ClearTile(j, k);
				}
			}
			if (Main.netMode != 0)
			{
				if (num != -1)
				{
					NetMessage.SendData(34, -1, -1, null, number, (float)x, (float)y, 0f, num, (int)Main.tile[x, y].type, 0);
				}
				NetMessage.SendTileSquare(-1, x, y, 3, TileChangeType.None);
			}
		}

		internal static void ClearLiquid(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			tile.liquid = 0;
			tile.lava(false);
			tile.honey(false);
			if (Main.netMode == 2)
			{
				NetMessage.sendWater(x, y);
			}
		}

		internal static void ClearTile(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			tile.type = 0;
			tile.sTileHeader = 0;
			tile.frameX = 0;
			tile.frameY = 0;
		}

		internal static void ClearWall(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			tile.wall = 0;
			tile.bTileHeader = 0;
			tile.bTileHeader2 = 0;
			tile.bTileHeader3 = 0;
		}

		internal static bool NoOrbOrAltar(int x, int y)
		{
			return Main.tile[x, y].type != 31 && Main.tile[x, y].type != 26;
		}

		internal static bool NoTempleOrGolemIsDead(int x, int y)
		{
			return TileChecks.NoTemple(x, y) || NPC.downedGolemBoss;
		}

		internal static bool NoTemple(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			return tile.wall != 87 && tile.type != 226 && (tile.type != 10 || tile.frameY < 594 || tile.frameY > 646);
		}

		internal static bool InGameWorld(int x, int y)
		{
			return x > 39 && x < Main.maxTilesX - 39 && y > 39 && y < Main.maxTilesY - 39;
		}

		internal static void TileSafe(int x, int y)
		{
			if (Main.tile[x, y] == null)
			{
				Main.tile[x, y] = new Tile();
			}
		}

		internal static void SquareUpdate(int x, int y)
		{
			if (Main.netMode != 0)
			{
				NetMessage.SendTileSquare(-1, x, y, 1, TileChangeType.None);
			}
		}
	}
}