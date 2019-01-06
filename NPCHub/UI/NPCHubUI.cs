using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace NPCHub.UI
{
	internal class NPCHubUI : UIState
	{
		private const int padding = 4;
		private const int numColumns = 10;
		public const float inventoryScale = 0.85f;

		private static UIPanel basePanel;
		private static float panelTop;
		private static float panelLeft;
		private static float panelWidth;
		private static float panelHeight;
		public static bool Visible;

		private static UIHeads heads = new UIHeads();
		public static MouseState curMouse;


		public override void OnInitialize()
		{
			float itemSlotWidth = Main.inventoryBackTexture.Width * inventoryScale;
			float itemSlotHeight = Main.inventoryBackTexture.Height * inventoryScale;

			panelTop = Main.instance.invBottom + 60;
			panelLeft = 80f;
			basePanel = new UIPanel();
			float innerPanelLeft = panelLeft + basePanel.PaddingLeft;
			float innerPanelWidth = numColumns * (itemSlotWidth + padding) + 20f + padding;
			panelWidth = basePanel.PaddingLeft + innerPanelWidth + basePanel.PaddingRight;
			panelHeight = Main.screenHeight - panelTop - 40f;
			basePanel.Left.Set(panelLeft, 0f);
			basePanel.Top.Set(panelTop, 0f);
			basePanel.Width.Set(panelWidth, 0f);
			basePanel.Height.Set(panelHeight, 0f);
			basePanel.Recalculate();

			heads.Width.Set(0f, 1f);
			heads.Top.Set(76f, 0f);
			heads.Height.Set(-116f, 1f);
			basePanel.Append(heads);

			Append(basePanel);
		}

		public override void Update(GameTime gameTime)
		{
			// Don't delete this or the UIElements attached to this UIState will cease to function.
			base.Update(gameTime);

			curMouse = Mouse.GetState();

			if (!Main.playerInventory)
			{
				Visible = false;
			}
		}

		/*protected override void DrawSelf(SpriteBatch spriteBatch)
		{

		}*/
	}

	public class UIHeads : UIElement
	{
		private const int padding = 4;
		private int numColumns = 10;
		private int numRows = 4;
		private int hoverSlot = -1;

		private float inventoryScale;

		public UIHeads()
		{

		}

		public override void Update(GameTime gameTime)
		{
			hoverSlot = -1;
			Vector2 origin = GetDimensions().Position();
			MouseState curMouse = NPCHubUI.curMouse;
			if (curMouse.X <= origin.X || curMouse.Y <= origin.Y)
			{
				return;
			}
			int slotWidth = (int)(Main.inventoryBackTexture.Width * inventoryScale * Main.UIScale);
			int slotHeight = (int)(Main.inventoryBackTexture.Height * inventoryScale * Main.UIScale);
			int slotX = (curMouse.X - (int)origin.X) / (slotWidth + padding);
			int slotY = (curMouse.Y - (int)origin.Y) / (slotHeight + padding);
			if (slotX < 0 || slotX >= numColumns || slotY < 0 || slotY >= numRows)
			{
				return;
			}
			Vector2 slotPos = origin + new Vector2(slotX * (slotWidth + padding * Main.UIScale), slotY * (slotHeight + padding * Main.UIScale));
			if (curMouse.X > slotPos.X && curMouse.X < slotPos.X + slotWidth && curMouse.Y > slotPos.Y && curMouse.Y < slotPos.Y + slotHeight)
			{
				//onHover(slotX + numColumns * slotY, ref hoverSlot);
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			float slotWidth = Main.inventoryBackTexture.Width * inventoryScale;
			float slotHeight = Main.inventoryBackTexture.Height * inventoryScale;
			Vector2 origin = GetDimensions().Position();
			float oldScale = Main.inventoryScale;
			Main.inventoryScale = inventoryScale;
			int i = 0;
			for (int k = 0; k < numColumns * numRows; k++)
			{
				Vector2 drawPos = origin + new Vector2((slotWidth + padding) * (k % numColumns), (slotHeight + padding) * (k / numColumns));

				//Background
				Texture2D texture = Main.inventoryBack7Texture;
				Color white2 = Main.inventoryBack;
				Main.spriteBatch.Draw(texture, drawPos, new Rectangle(0, 0, Main.inventoryBackTexture.Width, Main.inventoryBackTexture.Height), white2, 0f, default(Vector2), Main.inventoryScale, SpriteEffects.None, 0f);
				// Head
				Main.spriteBatch.Draw(Main.npcHeadTexture[i],
					new Vector2(drawPos.X + 26f * Main.inventoryScale, drawPos.Y + 26f * Main.inventoryScale),
					new Rectangle(0, 0, Main.npcHeadTexture[i].Width, Main.npcHeadTexture[i].Height), Color.White);
			}
			Main.inventoryScale = oldScale;
		}

	}
}