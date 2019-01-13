using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ThatsADick.NPCs
{
	class DickTail : DickSubPart
	{
		public override void SetDefaults()
		{
			base.SetDefaults();

			npc.damage = 11;
			npc.defense = 8;
			npc.lifeMax = 220;
		}
	}
}
