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
		public override bool Autoload(ref string name)
		{
			On.Terraria.Player.PickTile += HookPickTile;
			return base.Autoload(ref name);
		}

		private void HookPickTile(On.Terraria.Player.orig_PickTile orig, Player self, int x, int y, int pickPower)
		{
			MPlayer modPlayer = self.GetModPlayer<MPlayer>();
			if (modPlayer != null && modPlayer.mineless)
				return;

			orig(self, x, y, pickPower);
		}

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
	}
}
