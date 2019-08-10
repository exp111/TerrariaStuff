﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			addCheckbox(top, "No Melee Dmg", "Toggle Melee Damage", "noMeleeDmg");
			top += 20;
			addCheckbox(top, "No Summon Dmg", "Toggle Summon Damage", "noSummonDmg");
			top += 20;
			addCheckbox(top, "No Magic Dmg", "Toggle Magic Damage", "noMagicDmg");
			top += 20;
			addCheckbox(top, "No Ranged Dmg", "Toggle Ranged Damage", "noRangedDmg");
			top += 20;
			addCheckbox(top, "No Thrown Dmg", "Toggle Thrown Damage", "noThrownDmg");
			top += 40;

			addCheckbox(top, "Upside Down", "Toggle Australian Mode", "upsideDown");
			top += 20;
			addCheckbox(top, "No Armor", "Toggle no Armor", "noArmor");

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

			typeof(MPlayer).GetField(field).SetValue(modPlayer, checkbox.Selected);
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
