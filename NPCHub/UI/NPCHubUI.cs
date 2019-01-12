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
using Terraria.ModLoader;
using Terraria.UI;

namespace NPCHub.UI
{
	public class NPCWithIndex
	{
		public NPC npc;
		public int headIndex;
	}

	internal class NPCHubUI : UIState
	{
		private const int padding = 4;
		private const int numColumns = 5;
		private const int numRows = 5;
		public const float inventoryScale = 0.85f;

		private static UIPanel basePanel;
		private static float panelTop;
		private static float panelLeft;
		private static float panelWidth;
		private static float panelHeight;
		public static bool Visible;

		private static UIHeads heads = new UIHeads(inventoryScale, numColumns, numRows);

		public static MouseState oldMouse;
		public static MouseState curMouse;
		public static bool MouseClicked
		{
			get
			{
				return curMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released;
			}
		}

		public static List<NPCWithIndex> npcs = new List<NPCWithIndex>();
		private static bool searched;

		public override void OnInitialize()
		{
			float itemSlotWidth = Main.inventoryBackTexture.Width * inventoryScale;
			float itemSlotHeight = Main.inventoryBackTexture.Height * inventoryScale;

			panelTop = Main.instance.invBottom + 60;
			panelLeft = 80f;
			basePanel = new UIPanel();

			float innerPanelWidth = numColumns * (itemSlotWidth + padding) + padding;
			panelWidth = basePanel.PaddingLeft + innerPanelWidth + basePanel.PaddingRight;

			float innerPanelHeight = numRows * (itemSlotWidth + padding);
			panelHeight = Main.screenHeight - panelTop - innerPanelHeight - 600;

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
			if (!searched)
			{
				npcs = Main.npc.Where(npc => npc.active && npc.townNPC && NPC.TypeToHeadIndex(npc.type) != -1).OrderBy(npc => npc.TypeName).Select(n => new NPCWithIndex{npc = n, headIndex = NPC.TypeToHeadIndex(n.type)}).ToList();
				searched = true;
			}
			// Don't delete this or the UIElements attached to this UIState will cease to function.
			base.Update(gameTime);

			oldMouse = curMouse;
			curMouse = Mouse.GetState();

			if (!Main.playerInventory)
			{
				Visible = false;
				npcs.Clear();
				searched = false;
			}
		}

		/*protected override void DrawSelf(SpriteBatch spriteBatch)
		{

		}*/
	}

	public class UIHeads : UIElement
	{
		private const int padding = 4;
		private int numColumns;
		private int numRows = 4;
		private int hoverSlot = -1;

		private float inventoryScale;

		public UIHeads(float inventoryScale, int numColumns, int numRows)
		{
			this.inventoryScale = inventoryScale;
			this.numColumns = numColumns;
			this.numRows = numRows;
		}

		public static Rectangle GetFullRectangle(UIElement element)
		{
			Vector2 vector = new Vector2(element.GetDimensions().X, element.GetDimensions().Y);
			Vector2 position = new Vector2(element.GetDimensions().Width, element.GetDimensions().Height) + vector;
			vector = Vector2.Transform(vector, Main.UIScaleMatrix);
			position = Vector2.Transform(position, Main.UIScaleMatrix);
			Rectangle result = new Rectangle((int)vector.X, (int)vector.Y, (int)(position.X - vector.X), (int)(position.Y - vector.Y));
			int width = Main.spriteBatch.GraphicsDevice.Viewport.Width;
			int height = Main.spriteBatch.GraphicsDevice.Viewport.Height;
			result.X = Utils.Clamp<int>(result.X, 0, width);
			result.Y = Utils.Clamp<int>(result.Y, 0, height);
			result.Width = Utils.Clamp<int>(result.Width, 0, width - result.X);
			result.Height = Utils.Clamp<int>(result.Height, 0, height - result.Y);
			return result;
		}

		public override void Update(GameTime gameTime)
		{
			Vector2 origin = GetFullRectangle(this).TopLeft();
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
				onHover(slotX + numColumns * slotY);
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			float slotWidth = Main.inventoryBackTexture.Width * inventoryScale;
			float slotHeight = Main.inventoryBackTexture.Height * inventoryScale;
			Vector2 origin = GetDimensions().Position();
			float oldScale = Main.inventoryScale;
			Main.inventoryScale = inventoryScale;

			for (int k = 0; k < numColumns * numRows/* && k < NPCHubUI.npcs.Count*/; k++)
			{
				Vector2 drawPos = origin + new Vector2((slotWidth + padding) * (k % numColumns), (slotHeight + padding) * (k / numColumns));

				//Background
				Texture2D texture = Main.inventoryBack7Texture;
				Color clrBack = Main.inventoryBack;
				Main.spriteBatch.Draw(texture, drawPos, new Rectangle(0, 0, Main.inventoryBackTexture.Width, Main.inventoryBackTexture.Height), clrBack);
				// Head
				if (k < NPCHubUI.npcs.Count)
				{
					int i = NPCHubUI.npcs[k].headIndex;
					float offX = Main.inventoryBackTexture.Width / 2 - Main.npcHeadTexture[i].Width / 2;
					float offY = Main.inventoryBackTexture.Height / 2 - Main.npcHeadTexture[i].Height / 2;
					Main.spriteBatch.Draw(Main.npcHeadTexture[i],
						new Vector2(drawPos.X + offX * Main.inventoryScale, drawPos.Y + offY * Main.inventoryScale),
						new Rectangle(0, 0, Main.npcHeadTexture[i].Width, Main.npcHeadTexture[i].Height), Color.White);
				}
			}
			Main.inventoryScale = oldScale;
		}

		private static void onHover(int slot)
		{
			Player player = Main.player[Main.myPlayer];
			int visualSlot = slot;
			//slot += numColumns * (int)Math.Round(scrollBar.ViewPosition);
			if (NPCHubUI.MouseClicked && slot < NPCHubUI.npcs.Count)
			{
				NPC npc = NPCHubUI.npcs[slot].npc;

				var dist = (npc.position - player.position).Length();
				string x = String.Format("Full Name: {0}, Distance: {1}, WhoAmI: {2}", npc.FullName, dist, npc.whoAmI);
				Main.NewText(x, 255, 240, 20);

				//Get him out of the inventory
				Main.playerInventory = false;
				player.chest = -1;
				//NPC Needs to be in range
				npc.Teleport(player.position);
				//Talk to NPC
				player.talkNPC = npc.whoAmI;
				//Set chat text
				Main.npcChatText = npc.GetChat();
				//Play bubble sound
				Main.PlaySound(24);
			}
		}
	}
}