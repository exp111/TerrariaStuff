using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ShittyTerrariaHax
{
	public class MPlayer : ModPlayer
	{
		public bool toggle = false;
		public bool unhittable = false;

		public void SetField(string field, bool value) //TODO: make generic
		{
			var info = GetType().GetField(field);
			if (info == null)
				return;

			info.SetValue(this, value);

			switch (field)
			{
				default:
					break;
			}
		}

		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (ShittyTerrariaHax.UIKey.JustPressed)
			{
				ShittyTerrariaHax.Instance.ToggleUIVisible();
			}
		}

		public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
		{
			  return !unhittable || base.CanBeHitByNPC(npc, ref cooldownSlot);
		}

		public override bool CanBeHitByProjectile(Projectile proj)
		{
			return !unhittable || base.CanBeHitByProjectile(proj);
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
			
		}

		public override void UpdateVanityAccessories()
		{
			
		}
		

		public override void SetControls()
		{
		}


		public override void ResetEffects()
		{
			
		}

		public override void PostUpdateBuffs()
		{

		}

		public override void PostUpdateRunSpeeds()
		{

		}

		public override void PreUpdateMovement()
		{

		}

		public override void PostUpdate()
		{

		}
		#endregion
	}
}