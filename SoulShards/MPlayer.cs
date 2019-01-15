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

			if (player.GetModPlayer<MPlayer>().soulShard == null) // no accessoire equipped?
				return;

			SoulShard soulShard = (SoulShard)player.GetModPlayer<MPlayer>().soulShard;
			if (soulShard.soul.type == 0) // unset?
			{
				soulShard.soul.type = target.type;
				soulShard.soul.name = target.GivenOrTypeName;
			}
			else
			{
				if (soulShard.soul.type != target.type) // not our type?
					return;
			}
			soulShard.soul.kills += soulShard.addPerKill; //TODO: stop adding after full?
		}
	}
}