using System.IO;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ModLoader;

namespace FasterOldOnesArmy
{
	class FasterOldOnesArmy : Mod
	{
		public FasterOldOnesArmy()
		{
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			ModNetHandler.HandlePacket(reader, whoAmI);
		}
	}

	internal class ModNetHandler
	{
		public enum MessageType : byte
		{
			TimeLeft = 1,
			LaneSpawnRate
		}

		public static void HandlePacket(BinaryReader r, int fromWho)
		{
			switch (r.ReadByte())
			{
				case (byte)MessageType.TimeLeft:
					DD2Event.TimeLeftBetweenWaves = r.ReadInt32();
					break;
				case (byte)MessageType.LaneSpawnRate:
					DD2Event.LaneSpawnRate = r.ReadInt32();
					break;
			}
		}
	}
}
