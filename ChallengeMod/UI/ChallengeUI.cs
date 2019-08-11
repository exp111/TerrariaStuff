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
			basePanel.Height.Set(300, 0f);
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
			addCheckbox(top, "No Melee Dmg", "Toggle Melee Damage", nameof(MPlayer.noMeleeDmg));
			top += 20;
			addCheckbox(top, "No Summon Dmg", "Toggle Summon Damage", nameof(MPlayer.noSummonDmg));
			top += 20;
			addCheckbox(top, "No Magic Dmg", "Toggle Magic Damage", nameof(MPlayer.noMagicDmg));
			top += 20;
			addCheckbox(top, "No Ranged Dmg", "Toggle Ranged Damage", nameof(MPlayer.noRangedDmg));
			top += 20;
			addCheckbox(top, "No Thrown Dmg", "Toggle Thrown Damage", nameof(MPlayer.noThrownDmg));
			top += 40;

			addCheckbox(top, "Upside Down", "Toggle Australian Mode (Always gravity potion)", nameof(MPlayer.upsideDown));
			top += 20;
			addCheckbox(top, "Merfolk", "Toggle Merfolk Mode (Always a merfolk. Can't breathe air)", nameof(MPlayer.merfolk));
			top += 20;
			addCheckbox(top, "No Armor", "Toggle no Armor Mode (Can't equip Armor)", nameof(MPlayer.noArmor));
			top += 20;
			addCheckbox(top, "No Accessories", "Toggle no Accessories Mode (Can't equip Accessories)", nameof(MPlayer.noAccessories));
			top += 20;
			addCheckbox(top, "1 HP", "Toggle 1HP Mode (Always at 1 HP)", nameof(MPlayer.oneHp));
			top += 20;
			addCheckbox(top, "Mineless", "Toggle Mineless Mode (Can't mine any blocks)", nameof(MPlayer.mineless));

			Append(basePanel);
		}

		private void addCheckbox(int y, string name, string tooltip, string field)
		{
			UICheckbox checkbox = new UICheckbox(name, tooltip);
			checkbox.Top.Set(y, 0f);
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
