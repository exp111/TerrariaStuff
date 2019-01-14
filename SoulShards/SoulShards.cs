using System;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SoulShards
{
	public class Soul
	{
		public int type = 0;
		public const string typeString = "soulType";
		public string name = "";
		public const string nameString = "soulName";
		public int kills = 0;
		public const string killsString = "soulKills";
	}

	class SoulShards : Mod
	{
		public SoulShards()
		{
		}
	}
}
