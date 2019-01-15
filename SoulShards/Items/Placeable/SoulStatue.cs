using Terraria.ID;
using Terraria.ModLoader;

namespace SoulShards.Items.Placeable
{
	public class SoulStatue : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Statue");
			Tooltip.SetDefault("Spawn your Souls. Shift-Rightclick to remove again.");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 32;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.value = 500;
			item.createTile = mod.TileType<Tiles.SoulStatue>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			//recipe.AddIngredient(ItemID.KingStatue);
			//recipe.AddIngredient(ItemID.QueenStatue);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}