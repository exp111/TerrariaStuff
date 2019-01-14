using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SoulShards.Items
{
	public class SoulShard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("SoulShard");
			Tooltip.SetDefault("Bound to: None, Kills: 0");
		}
		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 40;
			item.accessory = true;
			item.rare = ItemRarityID.Lime;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<MPlayer>().soulShard = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
