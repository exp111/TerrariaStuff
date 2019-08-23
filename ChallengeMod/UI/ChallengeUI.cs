using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace ChallengeMod.UI
{
	internal class ChallengeUI : UIState
	{
		internal class Checkbox
		{
			public UICheckbox checkbox;
			public string field;
		}
		UIPanel basePanel;

		public List<Checkbox> checkboxes = new List<Checkbox>();

		public override void OnInitialize()
		{
			basePanel = new UIPanel();
			basePanel.Width.Set(300, 0f);
			basePanel.HAlign = basePanel.VAlign = 0.5f;

			UIImageButton closeButton = new UIImageButton(ChallengeMod.Instance.GetTexture("UI/closeButton"));
			closeButton.OnClick += (e, el) => ChallengeMod.Instance.SetUIVisible(false);
			closeButton.Left.Set(250f, 0f);
			closeButton.Top.Set(1f, 0f);
			basePanel.Append(closeButton);

			UIText header = new UIText("Challenges");
			header.HAlign = 0.5f;
			header.Top.Set(10f, 0f);
			basePanel.Append(header);

			int top = 30;
			addCheckbox(ref top, "No Melee Dmg", "Toggle Melee Damage", nameof(MPlayer.noMeleeDmg));
			addCheckbox(ref top, "No Summon Dmg", "Toggle Summon Damage", nameof(MPlayer.noSummonDmg));
			addCheckbox(ref top, "No Magic Dmg", "Toggle Magic Damage", nameof(MPlayer.noMagicDmg));
			addCheckbox(ref top, "No Ranged Dmg", "Toggle Ranged Damage", nameof(MPlayer.noRangedDmg));
			addCheckbox(ref top, "No Thrown Dmg", "Toggle Thrown Damage", nameof(MPlayer.noThrownDmg));
			if (ChallengeMod.thoriumLoaded)
			{
				addCheckbox(ref top, "No Symphonic Dmg", "Toggle Symphonic Damage", nameof(MPlayer.noSymphonicDmg));
				addCheckbox(ref top, "No Radiant Dmg", "Toggle Radiant Damage", nameof(MPlayer.noRadiantDmg));
			}
			top += 20;

			addCheckbox(ref top, "Upside Down", "Toggle Australian Mode (Always gravity potion)", nameof(MPlayer.upsideDown));
			addCheckbox(ref top, "Merfolk", "Toggle Merfolk Mode (Always a merfolk. Can't breathe air)", nameof(MPlayer.merfolk));
			addCheckbox(ref top, "No Armor", "Toggle no Armor Mode (Can't equip Armor)", nameof(MPlayer.noArmor));
			addCheckbox(ref top, "No Accessories", "Toggle no Accessories Mode (Can't equip Accessories)", nameof(MPlayer.noAccessories));
			addCheckbox(ref top, "1 HP", "Toggle 1HP Mode (Always at 1 HP)", nameof(MPlayer.oneHp));
			addCheckbox(ref top, "Mineless", "Toggle Mineless Mode (Can't mine any blocks)", nameof(MPlayer.mineless));

			basePanel.Height.Set(top + 20, 0f);
			Append(basePanel);
		}

		private void addCheckbox(ref int y, string name, string tooltip, string field)
		{
			UICheckbox checkbox = new UICheckbox(name, tooltip);
			checkbox.Top.Set(y, 0f);
			y += 20; //increase height
			checkbox.Left.Set(12f, 0f);
			checkbox.OnSelectedChanged += () => SetModPlayerField(field, checkbox);
			basePanel.Append(checkbox);

			checkboxes.Add(new Checkbox { checkbox = checkbox, field = field });
		}

		public void Load()
		{
			MPlayer modPlayer = Main.LocalPlayer.GetModPlayer<MPlayer>();
			if (modPlayer == null)
				return;

			checkboxes.ForEach((Checkbox checkbox) =>
			{
				checkbox.checkbox.Selected = (bool)typeof(MPlayer).GetField(checkbox.field).GetValue(modPlayer);
			});
		}

		public void SetModPlayerField(string field, UICheckbox checkbox)
		{
			MPlayer modPlayer = Main.LocalPlayer.GetModPlayer<MPlayer>();
			if (modPlayer == null)
				return;

			modPlayer.SetField(field, checkbox.Selected);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (basePanel.ContainsPoint(Main.MouseScreen))
			{
				Main.LocalPlayer.mouseInterface = true;
			}
		}
	}
}
