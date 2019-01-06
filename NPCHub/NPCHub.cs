using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using NPCHub.UI;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace NPCHub
{
	class NPCHub : Mod
	{
		internal static NPCHub Instance;
		private UserInterface _npcHubInterface;
		internal NPCHubUI NpcHubUI;

		public NPCHub()
		{
		}

		public override void Load()
		{
			Instance = this;

			if (!Main.dedServ)
			{
				NpcHubUI = new NPCHubUI();
				NpcHubUI.Activate();
				_npcHubInterface = new UserInterface();
				_npcHubInterface.SetState(NpcHubUI);
			}
		}

		public override void UpdateUI(GameTime gameTime)
		{
			if (_npcHubInterface != null && NPCHubUI.Visible)
			{
				_npcHubInterface.Update(gameTime);
			}
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIndex != -1)
			{
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
					"NPCHub: Stuff",
					delegate
					{
						if (NPCHubUI.Visible)
						{
							_npcHubInterface.Draw(Main.spriteBatch, new GameTime());
						}

						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}
