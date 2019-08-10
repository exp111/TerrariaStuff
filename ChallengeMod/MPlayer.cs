using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ChallengeMod
{
	public class MPlayer : ModPlayer
	{
		public bool noMeleeDmg = false;
		public bool noSummonDmg = false;
		public bool noMagicDmg = false;
		public bool noRangedDmg = false;
		public bool noThrownDmg = false;

		public bool upsideDown = false;
		public bool noArmor = false; //TODO: maybe add a blocked item and put it into armor slot?
		public int accessorySlots = 5;

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
				{nameof(noMeleeDmg), noMeleeDmg},
				{nameof(noSummonDmg), noSummonDmg},
				{nameof(noMagicDmg), noMagicDmg},
				{nameof(noRangedDmg), noRangedDmg},
				{nameof(noThrownDmg), noThrownDmg},
				{nameof(upsideDown), upsideDown},
				{nameof(noArmor), noArmor}
			};
		}

		public override void Load(TagCompound tag)
		{
			noMeleeDmg = tag.GetBool(nameof(noMeleeDmg));
			noSummonDmg = tag.GetBool(nameof(noSummonDmg));
			noMagicDmg = tag.GetBool(nameof(noMagicDmg));
			noRangedDmg = tag.GetBool(nameof(noRangedDmg));
			noThrownDmg = tag.GetBool(nameof(noThrownDmg));
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
			if (item.melee && noMeleeDmg)
				return false;

			if (item.summon && noSummonDmg)
				return false;

			if (item.ranged && noRangedDmg)
				return false;

			if (item.magic && noMagicDmg)
				return false;

			if (item.thrown && noThrownDmg)
				return false;

			return null;
		}

		public bool? CanHitWithProj(Projectile proj, NPC target)
		{
			if (proj.melee && noMeleeDmg)
				return false;

			if (proj.minion && noSummonDmg)
				return false;

			if (proj.ranged && noRangedDmg)
				return false;

			if (proj.magic && noMagicDmg)
				return false;

			if (proj.thrown && noThrownDmg)
				return false;

			return null;
		}
	}
}