using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent.Events;

namespace FasterOldOnesArmy.Tiles
{
	public class FOOAGlobalTile : GlobalTile
	{
		public override void RightClick(int i, int j, int type)
		{
			if (type != TileID.ElderCrystalStand)
				return;

			if (!DD2Event.Ongoing)
				return;

			if (DD2Event.TimeLeftBetweenWaves == 300) //Just started? TODO: find a better way to check
				return;

			if (DD2Event.EnemySpawningIsOnHold)
			{
				Main.NewText("Skipped!", Colors.CoinGold);
				DD2Event.TimeLeftBetweenWaves = 1;
				return;
			}

			if (DD2Event.LaneSpawnRate >= 90)
			{
				DD2Event.LaneSpawnRate = 10;
			}
			else
			{
				DD2Event.LaneSpawnRate += 10;
			}
			Main.NewText(string.Format("Spawn rate set to {0}", DD2Event.LaneSpawnRate), Colors.CoinGold);
		}
	}
}
