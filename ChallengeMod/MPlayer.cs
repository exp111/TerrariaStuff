using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ChallengeMod
{
	public class MPlayer : ModPlayer
	{
		public bool summonDmg = true;
		public bool meleeDmg = true;
		public bool magicDmg = true;
		public bool rangedDmg = true;
		public bool thrownDmg = true;

		public bool upsideDown = false;
		public bool noArmor = false;

		bool hadGravControl = false;

		public override void ResetEffects()
		{
			//reset upsidedown cuz we don't reset it //TODO: maybe move to upsidedown toggle as this cancels other mods
			if (!upsideDown && !player.gravControl && !player.gravControl2)
			{
				player.gravDir = 1f;
			}
		}

		public override TagCompound Save()
		{
			return new TagCompound()
			{
				{nameof(summonDmg), summonDmg},
				{nameof(meleeDmg), meleeDmg},
				{nameof(magicDmg), magicDmg},
				{nameof(rangedDmg), rangedDmg},
				{nameof(thrownDmg), thrownDmg},
				{nameof(upsideDown), upsideDown},
				{nameof(noArmor), noArmor}
			};
		}

		public override void Load(TagCompound tag)
		{
			summonDmg = tag.GetBool(nameof(summonDmg));
			meleeDmg = tag.GetBool(nameof(meleeDmg));
			magicDmg = tag.GetBool(nameof(magicDmg));
			rangedDmg = tag.GetBool(nameof(rangedDmg));
			thrownDmg = tag.GetBool(nameof(thrownDmg));
			upsideDown = tag.GetBool(nameof(upsideDown));
			noArmor = tag.GetBool(nameof(noArmor));
		}

		public override void PostUpdate()
		{
			if (upsideDown)
			{
				player.gravDir = -1f;
				player.gravControl = hadGravControl; //reset to previous state so we can't press up
			}
		}

		public override void PostUpdateBuffs()
		{
			if (upsideDown)
			{
				player.gravDir = -1f;
				hadGravControl = player.gravControl; //save gravcontrol
				player.gravControl = true; //set to true so we fall down
			}
		}

		public override void PreUpdate()
		{
			if (!noArmor)
				return;
			
			for (var i = 0; i < 3; i++)
			{
				var item = player.armor[i];
				if (item.netID != 0) // Drop if valid item
				{
					player.QuickSpawnClonedItem(item); //clone & give it back
					player.armor[i] = new Item();
				}
			}
		}

		public override bool? CanHitNPC(Item item, NPC target)
		{
			return CanHit(item, target);
		}

		public override bool? CanHitNPCWithProj(Projectile proj, NPC target)
		{
			return CanHitWithProj(proj, target);
		}

		public bool? CanHit(Item item, NPC target)
		{
			if (item.melee && !meleeDmg)
				return false;

			if (item.summon && !summonDmg)
				return false;

			if (item.ranged && !rangedDmg)
				return false;

			if (item.magic && !magicDmg)
				return false;

			if (item.thrown && !thrownDmg)
				return false;

			return null;
		}

		public bool? CanHitWithProj(Projectile proj, NPC target)
		{
			if (proj.melee && !meleeDmg)
				return false;

			if (proj.minion && !summonDmg)
				return false;

			if (proj.ranged && !rangedDmg)
				return false;

			if (proj.magic && !magicDmg)
				return false;

			if (proj.thrown && !thrownDmg)
				return false;

			return null;
		}
	}
}