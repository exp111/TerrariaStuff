using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ThatsADick.NPCs
{
	public abstract class DickPart : ModNPC
	{
		public const int Segments = 25;
		public bool JustSpawned = true;

		public const string bossName = "Eater of Dicks";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(bossName);
		}

		public override void SetDefaults()
		{
			npc.width = 38; //TODO: check if width & height is ok (copied from eow)
			npc.height = 38;
			npc.aiStyle = 6;
			npc.netAlways = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.knockBackResist = 0.0f;
			npc.behindTiles = true;
			npc.value = 300f;
			npc.scale = 1f;
			npc.buffImmune[20] = true;
			npc.buffImmune[24] = true;
			npc.buffImmune[39] = true;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.6f);
		}
	}

	[AutoloadBossHead]
	class DickHead : DickPart
	{
		public override void SetDefaults()
		{
			base.SetDefaults();

			npc.boss = true;

			npc.npcSlots = 5f; // what's this for?

			npc.damage = 22; // TODO: Change dmg, defense & life
			npc.defense = 2;
			npc.lifeMax = 65;
		}

		public override void NPCLoot()
		{
			MWorld.downedDick = true;
			//TODO: drop loot in NPCLoot?
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.HealingPotion;
			//TODO: drop loot in BossLoot?
		}

		public override void AI()
		{
			base.AI();

			//TODO: if destroyer style: check if every segment is ded -> die
			if (JustSpawned)
			{
				OnSpawn();
			}
			ShootProjectile();
		}

		private void ShootProjectile()
		{
			//TODO: shoot projectile (onhit: slowed debuff)
		}

		private void OnSpawn()
		{
			int previous = npc.whoAmI;

			for (int i = 0; i < Segments; i++)
			{
				int type = i < Segments - 1 // Last part?
						? mod.NPCType<DickBody>()
						: mod.NPCType<DickTail>();

				// Create new Segment
				int segmentWhoAmI = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, type, ai1: previous, ai2: npc.whoAmI);
				// Set npc.whoAmI
				NPC segment = Main.npc[segmentWhoAmI];
				segment.whoAmI = segmentWhoAmI;
				segment.realLife = npc.whoAmI;
				segment.active = true;
				// Update previous so we know which one we follow
				previous = segmentWhoAmI;
			}

			JustSpawned = false;
		}
	}
}
