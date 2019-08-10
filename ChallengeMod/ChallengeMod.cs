using ChallengeMod.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace ChallengeMod
{
	public class ChallengeMod : Mod
	{
		internal static ChallengeMod Instance;
		private UserInterface _challengeInterface;
		internal ChallengeUI ChallengeUI;

		public ChallengeMod()
		{
		}

		public override void Load()
		{
			Instance = this;

			if (!Main.dedServ)
			{
				_challengeInterface = new UserInterface();

				ChallengeUI = new ChallengeUI();
				ChallengeUI.Activate();

				UICheckbox.checkboxTexture = GetTexture("UI/checkBox");
				UICheckbox.checkmarkTexture = GetTexture("UI/checkMark");
			}
		}

		public override void Unload()
		{
			Instance = null;
			_challengeInterface = null;
			ChallengeUI = null;
		}

		public void SetUIVisible(bool visible)
		{
			_challengeInterface.SetState(visible ? ChallengeUI : null);
			if (visible)
			{
				ChallengeUI.Load();
			}
		}

		public void ToggleUIVisible()
		{
			SetUIVisible(_challengeInterface.CurrentState == null);
		}

		public override void UpdateUI(GameTime gameTime)
		{
			if (_challengeInterface?.CurrentState != null)
			{
				_challengeInterface.Update(gameTime);
			}
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIndex != -1)
			{
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
					"ChallengeMod: UI",
					delegate
					{
						if (_challengeInterface?.CurrentState != null)
						{
							_challengeInterface.Draw(Main.spriteBatch, new GameTime());
						}

						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}