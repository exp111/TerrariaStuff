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
		public bool JustSpawned = true;

		public const string bossName = "Eater of Dicks";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(bossName);
		}

		public override void SetDefaults()
		{
			npc.width = 38;
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

			npc.npcSlots = 5f;

			npc.damage = 22;
			npc.defense = 2;
			npc.lifeMax = 65;
		}

		public override void NPCLoot()
		{
			MWorld.downedDick = true;
			Main.NewText("NPC Loot");
		}

		public override void AI()
		{
			base.AI();

			if (JustSpawned)
			{
				OnSpawn();
			}
			ShootProjectile();
		}

		private void ShootProjectile()
		{
			//TODO: shoot projectile
		}

		private void OnSpawn()
		{
			int previous = npc.whoAmI;
			const int segments = 25;

			for (int i = 0; i < segments; i++)
			{
				int type = i < segments - 1 // Last part?
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
