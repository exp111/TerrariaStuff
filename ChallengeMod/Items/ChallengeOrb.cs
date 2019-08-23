using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChallengeMod.Items
{
	public class ChallengeOrb : ModItem
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("Use to change challenges");
		}

		public override void SetDefaults() 
		{
			item.width = 26;
			item.height = 26;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 4;
			item.value = 0;
			item.rare = 11;
		}

		public override bool UseItem(Player player)
		{
			ChallengeMod.Instance.ToggleUIVisible();

			return true;
		}
	}
}