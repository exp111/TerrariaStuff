using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChallengeMod.Items
{
	public class DebugItem : ModItem
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("Use to change challenge");
		}

		public override void SetDefaults() 
		{
			item.width = 40;
			item.height = 40;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 4;
			item.value = 0;
			item.rare = 11;
		}

		public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool UseItem(Player player)
		{
			ChallengeMod.Instance.ToggleUIVisible();

			return true;
		}
	}
}