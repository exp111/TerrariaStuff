using System;
using System.IO;
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

		public TagCompound Serialize()
		{
			return new TagCompound {
				{typeString, type},
				{nameString, name},
				{killsString, kills}
			};
		}

		public static Soul Load(TagCompound tag)
		{
			if (!tag.ContainsKey(nameString))
				return new Soul();

			return new Soul
			{
				type = tag.GetInt(typeString),
				name = tag.GetString(nameString),
				kills = tag.GetInt(killsString)
			};
		}

		public void NetSend(BinaryWriter writer)
		{
			writer.Write(type);
			writer.Write(name);
			writer.Write(kills);
		}

		public static Soul NetReceive(BinaryReader reader)
		{
			return new Soul()
			{
				type = reader.ReadInt32(),
				name = reader.ReadString(),
				kills = reader.ReadInt32()
			};
		}
	}

	class SoulShards : Mod
	{
		public SoulShards()
		{
		}
	}
}
