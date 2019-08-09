using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NPCPrisonBuilder.Items
{
	enum BlockType : int
	{
		WALL = 0,
		TILE = 1,
		LIGHT = 2,
		WORKBENCH = 3,
		CHAIR = 4,
		PLATFORM = 5
	}

	public abstract class PrisonBuilder : ModItem
	{
		public abstract int tileType { get; }
		public abstract short ingredient { get; }
		public abstract string name { get; }

		bool lights = true;

		static ushort[] wallByType = { WallID.Wood, WallID.Stone };
		static int[] tileByType = { TileID.WoodBlock, TileID.Stone };

		public override string Texture { get { return (GetType().Namespace + "." + "PrisonBuilder").Replace('.', '/'); } }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(name);
			Tooltip.SetDefault("Builds an NPC prison.\nClick where you want the bottom right block.\nPrison is 5 tiles wide, 12 tiles high.");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 999;
			item.value = 1;
			item.rare = 0;
			item.consumable = true;

			item.useStyle = 4;
			item.useAnimation = 30;
			item.useTime = 30;
			item.autoReuse = false;
		}

		public override void HoldItem(Player player)
		{
			player.rulerLine = true;
		}

		public override bool UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer && !player.noBuilding)
			{
				int tileTargetX = Player.tileTargetX;
				int tileTargetY = Player.tileTargetY;
				if (Main.netMode == 0)
				{
					Item.NewItem(player.getRect(), ItemID.WorkBench);
					Item.NewItem(player.getRect(), ItemID.WoodenChair);
					HandleBuilding(tileTargetX, tileTargetY, tileType, lights);
				}
				else
				{
					PrisonPacket(tileTargetX, tileTargetY, tileType, lights);
				}
			}
			return true;
		}

		private static void PrisonPacket(int x, int y, int tileType, bool lights)
		{
			ModPacket packet = NPCPrisonBuilder.Instance.GetPacket(256);
			packet.Write(1);
			packet.Write(x);
			packet.Write(y);
			packet.Write(tileType);
			packet.Write(lights);
			packet.Send(-1, -1);
		}

		internal static void HandleBuilding(int x, int y, int tileType, bool lights)
		{
			int[,] array = new int[,]
			{
				{
					1,
					5,
					1,
					5,
					1
				},
				{
					1,
					0,
					3,
					4,
					1
				},
				{
					1,
					0,
					0,
					0,
					1
				},
				{
					1,
					0,
					0,
					0,
					1
				},
				{
					1,
					0,
					0,
					0,
					1
				},
				{
					1,
					0,
					2,
					0,
					1
				},
				{
					1,
					0,
					0,
					0,
					1
				},
				{
					1,
					0,
					0,
					0,
					1
				},
				{
					1,
					0,
					0,
					0,
					1
				},
				{
					1,
					0,
					0,
					0,
					1
				},
				{
					1,
					0,
					0,
					0,
					1
				},
				{
					1,
					1,
					1,
					1,
					1
				}
			};
			int light = TileID.Torches;
			int wall = wallByType[tileType];
			int tile = tileByType[tileType];
			ushort workbench = 0;
			ushort chair = 0;
			ushort platform = 0;
			for (int i = array.GetLength(0) - 1; i >= 0; i--)
			{
				for (int j = 0; j < 5; j++)
				{
					int num = x - j;
					int num2 = y - i;
					TileChecks.TileSafe(num, num2);
					if (TileChecks.InGameWorld(num, num2) && TileChecks.NoTempleOrGolemIsDead(num, num2) && TileChecks.NoOrbOrAltar(num, num2))
					{
						TileChecks.ClearEverything(num, num2);
						if (i > 0 && i < array.GetLength(0) - 1 && j > 0 && j < array.GetLength(1) - 1)
						{
							WorldGen.PlaceWall(num, num2, wall, true);
						}
						switch ((BlockType)array[i, j])
						{
							case BlockType.TILE:
								WorldGen.PlaceTile(num, num2, tile, true, false, -1, 0);
								break;
							case BlockType.LIGHT:
								if (lights)
									WorldGen.PlaceTile(num, num2, light, true, false, -1, 0);
								break;
							case BlockType.WORKBENCH:
								WorldGen.PlaceObject(num, num2, TileID.WorkBenches, true, workbench);
								break;
							case BlockType.CHAIR:
								WorldGen.PlaceObject(num, num2, TileID.Chairs, true, chair);
								break;
							case BlockType.PLATFORM:
								WorldGen.PlaceTile(num, num2, TileID.Platforms, true, false, -1, platform);
								break;
							default:
								break;
						}

						TileChecks.SquareUpdate(num, num2);
					}
				}
			}
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(base.mod);
			modRecipe.AddIngredient(ingredient, 36); // blocks + walls
			modRecipe.AddRecipeGroup("Wood", 16); //platform + workbench + chair + torch
			modRecipe.AddIngredient(ItemID.Gel); // torch
			modRecipe.AddTile(TileID.WorkBenches);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}

	public class WoodPrisonBuilder : PrisonBuilder
	{
		public override int tileType { get => 0; }
		public override short ingredient { get => ItemID.Wood; }
		public override string name { get => "Wood Prison Builder"; }
	}

	public class StonePrisonBuilder : PrisonBuilder
	{
		public override int tileType { get => 1; }
		public override short ingredient { get => ItemID.StoneBlock; }
		public override string name { get => "Stone Prison Builder"; }
	}
}