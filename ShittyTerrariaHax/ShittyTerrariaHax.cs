using Microsoft.Xna.Framework;
using ShittyTerrariaHax.UI;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace ShittyTerrariaHax
{
	public class ShittyTerrariaHax : Mod
	{
		internal static ShittyTerrariaHax Instance;

		internal static ModHotKey UIKey;

		private UserInterface _cheatInterface;
		internal CheatUI CheatUI;


		public ShittyTerrariaHax()
		{
		}

		public override void Load()
		{
			Instance = this;

			UIKey = RegisterHotKey("Toggle Menu", "Insert");

			_cheatInterface = new UserInterface();

			CheatUI = new CheatUI();
			CheatUI.Activate();

			UICheckbox.checkboxTexture = GetTexture("UI/checkBox");
			UICheckbox.checkmarkTexture = GetTexture("UI/checkMark");
		}

		public override void Unload()
		{
			Instance = null;
			_cheatInterface = null;
			CheatUI = null;
		}

		public void SetUIVisible(bool visible)
		{
			_cheatInterface.SetState(visible ? CheatUI : null);
			if (visible)
			{
				CheatUI.Load();
			}
		}

		public void ToggleUIVisible()
		{
			SetUIVisible(_cheatInterface.CurrentState == null);
		}

		public override void UpdateUI(GameTime gameTime)
		{
			if (_cheatInterface?.CurrentState != null)
			{
				_cheatInterface.Update(gameTime);
			}
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIndex != -1)
			{
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
					"Unconspicious Mod: CheatUI",
					delegate
					{
						if (_cheatInterface?.CurrentState != null)
						{
							_cheatInterface.Draw(Main.spriteBatch, new GameTime());
						}

						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}