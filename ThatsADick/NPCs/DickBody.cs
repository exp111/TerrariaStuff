using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ThatsADick.NPCs
{
	public abstract class DickSubPart : DickPart
	{
		public override void SetDefaults()
		{
			base.SetDefaults();

			npc.dontCountMe = true;
		}

		public override void AI()
		{
			base.AI();

			if (!Main.npc[(int)npc.ai[1]].active)
			{
				npc.life = 0;
				npc.HitEffect(0, 10.0);
				npc.checkDead();
				npc.active = false;
				NetMessage.SendData(28, -1, -1, null, npc.whoAmI, -1f, 0.0f, 0.0f, 0, 0, 0);
			}
		}

		public override bool CheckActive()
		{
			return false;
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return false;
		}
	}

	class DickBody : DickSubPart
	{
		public override void SetDefaults()
		{
			base.SetDefaults();

			npc.damage = 13;
			npc.defense = 4;
			npc.lifeMax = 150;
		}
	}
}
