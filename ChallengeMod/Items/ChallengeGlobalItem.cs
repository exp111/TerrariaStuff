using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ChallengeMod.Items
{
	class ChallengeGlobalItem : GlobalItem
	{
		public override bool CanEquipAccessory(Item item, Player player, int slot)
		{
			MPlayer modPlayer = Main.LocalPlayer.GetModPlayer<MPlayer>();
			if (modPlayer != null)
			{
				if (modPlayer.noAccessories && item.accessory)
				{
					return false;
				}
			}

			return base.CanEquipAccessory(item, player, slot);
		}
	}
}
