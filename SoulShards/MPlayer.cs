using SoulShards.Items;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SoulShards
{
	public class MPlayer : ModPlayer
	{
		public ModItem soulShard;

		public override void ResetEffects()
		{
			soulShard = null;
		}

		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
		{
			OnHit(target);
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			OnHit(target);
		}

		public void OnHit(NPC target)
		{
			if (target.active) //ded?
				return;

			if (target.boss)
				return;

			if (player.GetModPlayer<MPlayer>().soulShard == null)
				return;

			SoulShard soulShard = (SoulShard)player.GetModPlayer<MPlayer>().soulShard;
			if (soulShard.killed.type == 0) // unset?
			{
				soulShard.killed.type = target.type;
				soulShard.killed.name = target.GivenOrTypeName;
			}
			else
			{
				if (soulShard.killed.type != target.type) // not our type?
					return;
			}
			soulShard.killed.kills++;
		}
	}
}