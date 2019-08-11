using System.Collections.Generic;
using System.Linq;
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

		public bool upsideDown = false; //TODO: spawn blocks under/above player after toggle
		public bool merfolk = false; //TODO: maybe make the fish bowl "useful"?
		public bool noArmor = false; //TODO: maybe add a blocked item and put it into armor slot?
		public bool noAccessories = false;
		public bool oneHp = false;
		public bool mineless = false;

		bool hadGravControl = false;
		int previousType = 0;

		public override bool Autoload(ref string name)
		{
			//On.Terraria.Player.PickTile += HookPickTile;
			return base.Autoload(ref name);
		}

		private void HookPickTile(On.Terraria.Player.orig_PickTile orig, Player self, int x, int y, int pickPower)
		{
			MPlayer modPlayer = self.GetModPlayer<MPlayer>();
			if (modPlayer != null && modPlayer.mineless)
				return;

			orig(self, x, y, pickPower);
		}

		public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
		{
			Item orb = new Item();
			orb.SetDefaults(mod.ItemType<Items.ChallengeOrb>());
			orb.stack = 1;
			items.Add(orb);
		}

		public override void ResetEffects()
		{
			//reset upsidedown cuz we don't reset it //TODO: maybe move to upsidedown toggle as this cancels other mods
			if (!upsideDown && !player.gravControl && !player.gravControl2)
			{
				player.gravDir = 1f;
			}
		}

		public override void UpdateVanityAccessories()
		{
			if (merfolk)
				player.forceMerman = true;
		}

		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound();
			
			// Add all public fields
			foreach (var field in GetType().GetFields().Where(field => field.IsPublic))
			{
				tag.Add(field.Name, field.GetValue(this));
			}

			return tag;
		}

		public override void Load(TagCompound tag)
		{
			foreach (var t in tag)
			{
				var field = typeof(MPlayer).GetField(t.Key);

				if (field != null)
					field.SetValue(this, tag.GetBool(t.Key));
			}
		}

		public override bool PreItemCheck()
		{
			if (true && player.HeldItem.pick > 0)
			{
				return true;
			}
			return true;
		}

		public override void PreUpdate()
		{
			CheckAndUnequipArmor();
			CheckAndUnqeuipAccessories();
		}

		public override void PostUpdateRunSpeeds()
		{
			if (merfolk)
			{
				previousType = player.armor[0].type;
				player.armor[0].type = 250; //force fish bowl
			}
		}

		public override void PreUpdateMovement()
		{
			if (merfolk)
			{
				player.armor[0].type = previousType; // reset hat again (so we get the other buffs)
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

			if (merfolk)
			{
				player.accMerman = true;
			}
		}

		public override void PostUpdate()
		{
			if (upsideDown)
			{
				player.gravDir = -1f;
				player.gravControl = hadGravControl; //reset to previous state so we can't press up
			}

			if (oneHp)
			{
				player.statLifeMax2 = 1;
				player.statLife = 1;
			}
		}

		public void CheckAndUnequipArmor()
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

		public void CheckAndUnqeuipAccessories()
		{
			if (!noAccessories)
				return;

			for (var i = 3; i < 8 + player.extraAccessorySlots; i++)
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