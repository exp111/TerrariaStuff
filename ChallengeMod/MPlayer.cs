using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
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
		public bool noThrownDmg = false; //TODO: add cross mod compability with Thorium (bard & healer)

		public bool upsideDown = false; //FIXME: no fall dmg if australian?
		public bool merfolk = false; //TODO: maybe make the fish bowl "useful"?
		public bool noArmor = false; //TODO: maybe add a blocked item and put it into armor slot?
		public bool noAccessories = false;
		public bool oneHp = false;
		public bool mineless = false;

		bool gravControlNeedsReset = false;
		int previousType = 0;

		public void SetField(string field, bool value) //TODO: make generic
		{
			var info = GetType().GetField(field);
			if (info == null)
				return;

			info.SetValue(this, value);

			switch (field)
			{
				case nameof(noAccessories):
					UnqeuipAccessories();
					break;
				case nameof(upsideDown):
					SpawnBlocks();
					SetGravDirOnToggle();
					break;
				case nameof(oneHp):
					player.statLife = 1;
					break;
				default:
					break;
			}
		}

		public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
		{
			Item orb = new Item();
			orb.SetDefaults(mod.ItemType<Items.ChallengeOrb>());
			orb.stack = 1;
			items.Add(orb);
		}

		#region TagSaveLoadEvents
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
		#endregion

		#region UpdateEvents
		public override void PreUpdate()
		{
			CheckAndUnequipArmor();
		}

		public override void UpdateVanityAccessories()
		{
			if (merfolk) //Force merfolk look
				player.forceMerman = true;
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

		public override void ResetEffects()
		{
			
		}

		public override void PostUpdateBuffs()
		{
			if (upsideDown && !player.gravControl && !player.gravControl2) //don't do shit if we have a grav potion
			{
				player.gravDir = -1f;
				gravControlNeedsReset = true; //save gravcontrol
				player.gravControl = true; //set to true so we fall down
			}

			if (merfolk)
			{
				player.accMerman = true;
			}
		}

		public override void PostUpdate()
		{
			if (upsideDown && gravControlNeedsReset) //only reset if we changed smth
			{
				player.gravDir = -1f;
				player.gravControl = false; //reset to false state so we can't press up
				gravControlNeedsReset = false;
			}

			if (oneHp)
			{
				player.statLifeMax2 = 1;
			}
		}
		#endregion

		#region NoArmor
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
		#endregion

		#region NoAccessories
		public void UnqeuipAccessories()
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
		#endregion

		#region UpsideDown
		public void SpawnBlocks()
		{
			if (!upsideDown)
				return;

			var pos = player.position.ToTileCoordinates();
			int x = pos.X;
			int y = pos.Y - 1;
			for (var i = 0; i < 2; i++)
			{
				WorldGen.PlaceTile(x + i, y, TileID.Dirt);
			}
		}

		public void SetGravDirOnToggle()
		{
			if (!upsideDown)
			{
				player.gravDir = 1f; //somehow it doesn't reset gravDir so we need to do it manually
			}
			else
			{
				player.gravDir = -1f;
			}
		}
		#endregion

		#region CanHitNPCEvents
		public override bool? CanHitNPC(Item item, NPC target)
		{
			return CanHit(item, target);
		}

		public override bool? CanHitNPCWithProj(Projectile proj, NPC target)
		{
			return CanHitWithProj(proj, target);
		}
		#endregion

		#region CanHitEventHandlers
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
		#endregion
	}
}