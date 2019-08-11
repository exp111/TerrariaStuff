using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ChallengeMod.Tiles
{
	class ChallengeGlobalTile : GlobalTile
	{
		public override bool CanExplode(int i, int j, int type)
		{
			MPlayer modPlayer = Main.LocalPlayer.GetModPlayer<MPlayer>();
			if (modPlayer != null)
			{
				if (modPlayer.mineless)
				{
					return false;
				}
			}

			return base.CanExplode(i, j, type);
		}

		public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
		{
			MPlayer modPlayer = Main.LocalPlayer.GetModPlayer<MPlayer>();
			if (modPlayer != null)
			{
				if (modPlayer.mineless && modPlayer.player.HeldItem.pick > 0) //only if its a pickaxe //FIXME: can't use pick & axe multitools
				{
					return false;
				}
			}

			return base.CanKillTile(i, j, type, ref blockDamaged);
		}
	}
}
