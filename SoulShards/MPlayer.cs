using System;
using Terraria.ModLoader;

namespace SoulShards
{
	public class MPlayer : ModPlayer
	{
		public bool soulShard;

		public override void ResetEffects()
		{
			soulShard = false;
		}
	}
}