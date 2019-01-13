using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ThatsADick.Items
{
	class Dick : ModItem
	{
		public static readonly string[] nameList =
		new string[197] {
			"Baby-Maker",
			"Beaver Basher",
			"Bed Snake",
			"Beef Whistle",
			"Best Friend",
			"Bishop",
			"Blue-Vein Sausage",
			"Boner",
			"Braciole",
			"Bratwurst",
			"Burrito",
			"Candle",
			"Captain",
			"Chief",
			"Choad",
			"Chopper",
			"Clarinet",
			"Cracksman",
			"Cranny Axe",
			"Crevice Crawler",
			"Cucumber",
			"Cum Gun",
			"Custard Launcher",
			"Dagger",
			"Danger noodle",
			"Ding Dong",
			"Dingbat",
			"Dingus",
			"Dink",
			"Dinky",
			"Disco Stick",
			"Dong",
			"Donger",
			"Doodad",
			"Dork",
			"Dragon",
			"Drum Stick",
			"Dude Piston",
			"Easy Rider",
			"Eggroll",
			"Excalibur",
			"Fang",
			"Ferret",
			"Fire Hose",
			"Flesh Flute",
			"Flesh Tower",
			"Foto",
			"Fuck Rod",
			"Fuck Stick",
			"Fun Stick",
			"Giggle Stick",
			"Goober",
			"Goofy Goober",
			"Groin Ferret",
			"Hairy Hotdog",
			"Heat-Seeking Moisture Missile",
			"Helmet Head",
			"Hog",
			"Horn",
			"Hose",
			"Hot dog",
			"Hotdogger",
			"Humperdink",
			"Jackhammer",
			"Jimmy",
			"John Thomas",
			"Johnson",
			"Joystick",
			"Junk",
			"Kickstand",
			"King Sebastian",
			"Knob",
			"Krull The Warrior King",
			"Lamb Kebab",
			"Lap Rocket",
			"Leaky Hose",
			"Leather-Stretcher",
			"Lingam",
			"Lipstick",
			"Little Alex",
			"Little Bob",
			"Little Elvis",
			"Little Fish",
			"Lizard",
			"Longfellow",
			"Love Muscle",
			"Love Rod",
			"Love Stick",
			"Love Whistle",
			"Luigi",
			"Man Meat",
			"Man Umbrella",
			"Manhood",
			"Master Sword",
			"Meat And Two Veg",
			"Meat Popsicle",
			"Meat Stick",
			"Meat Sword",
			"Member",
			"Microphone",
			"Middle Stump",
			"Milkman",
			"Millimetre Peter",
			"Mutton",
			"Netherrod",
			"Old Boy",
			"Old Chap",
			"Old Fellow",
			"Old Man",
			"One-Eyed Anaconda",
			"One-Eyed Monster",
			"One-Eyed Trouser-Snake",
			"One-Eyed Willie",
			"One-Eyed Wonder Weasel",
			"One-Eyed Wonder Worm",
			"One-Eyed Yogurt Slinger",
			"Pecker",
			"Pedro",
			"Peepee",
			"Percy",
			"Peter",
			"Pickle",
			"Pied Piper",
			"Pig Skin Bus",
			"Pink Oboe",
			"Pink Torpedo",
			"Piss Weasle",
			"Piston",
			"Pitched Tent",
			"Plug",
			"Pocket Rocket",
			"Pork Sword",
			"Prick",
			"Prince Charming",
			"Princess Sophia",
			"Purple-Headed Yogurt Flinger",
			"Putz",
			"Python",
			"Quiver Bone",
			"Rod",
			"Rod Of Pleasure",
			"Sausage",
			"Schlong",
			"Schlort",
			"Shmuck",
			"Schnitzel",
			"Schwanz",
			"Schwarz",
			"Sea Monster",
			"Sebastianic Sword",
			"Sexcalibur",
			"Shaft",
			"Short Arm",
			"Single Serving Soup Dispenser",
			"Skin Flute",
			"Snake",
			"Soldier",
			"Spawn Hammer",
			"Stick Shift",
			"Surfboard",
			"Tallywhacker",
			"Tent Peg",
			"Third Leg",
			"Thumper",
			"Thunderbird",
			"Thundersword",
			"Tinker",
			"Tiny",
			"Todger",
			"Tonka",
			"Tonsil Tickler",
			"Tool",
			"Trouser Snake",
			"Tubesteak",
			"Twig",
			"Twinkie",
			"Uncle Dick",
			"Vein",
			"Vlad The Impaler",
			"Wand",
			"Wang",
			"Wang Doodle",
			"Wanger",
			"Wedding Tackle",
			"Wee Wee",
			"Whoopie Stick",
			"Wick",
			"Wiener",
			"Wiener Schnitzel",
			"Willy",
			"Winkie",
			"Womb Broom",
			"Woody Womb Pecker",
			"Worm",
			"Yogurt Gun",
			"Yogurt Hose",
			"$5 Footlong"
		};
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(nameList[ThatsADick.rng.Next(nameList.Length)]);
			Tooltip.SetDefault("That's a ... dick?");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.maxStack = 20;
			item.value = 100;
			item.rare = 3;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = 4;
			item.consumable = false; //TODO: item.consumable = true
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DirtBlock);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType<NPCs.DickHead>());
			Main.PlaySound(SoundID.Roar, player.position, 0);
			return true;
		}
	}
}
