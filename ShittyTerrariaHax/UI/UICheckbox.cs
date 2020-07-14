using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace ShittyTerrariaHax.UI
{
	class UICheckbox : UIText
	{
		internal static Texture2D checkboxTexture;
		internal static Texture2D checkmarkTexture;
		public event Action OnSelectedChanged;

		public float order = 0;

		private bool clickable = false;
		public bool Clickable
		{
			get { return clickable; }
			set
			{
				if (value != clickable)
				{
					clickable = value;
				}
				TextColor = clickable ? Color.White : Color.Gray;
			}
		}

		string tooltip = "";

		private bool selected = false;
		public bool Selected
		{
			get { return selected; }
			set
			{
				if (value != selected)
				{
					selected = value;
					OnSelectedChanged?.Invoke();
				}
			}
		}

		public UICheckbox(string text, string tooltip, bool clickable = true, float textScale = 1, bool large = false) : base(text, textScale, large)
		{
			this.tooltip = tooltip;
			this.clickable = clickable;
			text = "   " + text;
			SetText(text);
			Recalculate();
		}

		public override void Click(UIMouseEvent evt)
		{
			if (clickable)
			{
				Selected = !Selected;
			}
			Recalculate();
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			//Vector2 pos = new Vector2(innerDimensions.X - 20, innerDimensions.Y - 5);
			Vector2 pos = new Vector2(innerDimensions.X, innerDimensions.Y - 5);

			spriteBatch.Draw(checkboxTexture, pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			if (Selected)
				spriteBatch.Draw(checkmarkTexture, pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			base.DrawSelf(spriteBatch);

			if (IsMouseHovering && tooltip.Length > 0)
			{
				Main.HoverItem = new Item();
				Main.hoverItemName = tooltip;
			}
		}

		public override int CompareTo(object obj)
		{
			UICheckbox other = obj as UICheckbox;
			return order.CompareTo(other.order);
		}
	}
}
