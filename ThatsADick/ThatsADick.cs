using System;
using Terraria;
using Terraria.ModLoader;

namespace ThatsADick
{
	class ThatsADick : Mod
	{
		public static Random rng;
		public ThatsADick()
		{
			rng = new Random();
		}

		public override void UpdateMusic(ref int music, ref MusicPriority priority)
		{
			Player player = Main.LocalPlayer;

			if (player.active && NPC.AnyNPCs(NPCType<NPCs.DickHead>()))
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/whipit");
				priority = MusicPriority.BossHigh;
			}
		}

		public override void PostSetupContent()
		{
			Mod bossChecklist = ModLoader.GetMod("BossChecklist");
			if (bossChecklist != null)
			{
				bossChecklist.Call("AddBossWithInfo", NPCs.DickPart.bossName, 2.7f, (Func<bool>)(() => MWorld.downedDick), "Spawn it");
			}
		}
	}
}
