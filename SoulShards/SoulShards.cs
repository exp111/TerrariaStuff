using SoulShards.Items;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SoulShards
{
	public class Soul
	{
		public int ID = 0;
		public const string typeString = "soulID";
		public string name = "";
		public const string nameString = "soulName";
		public int kills = 0;
		public const string killsString = "soulKills";

		public TagCompound Serialize()
		{
			return new TagCompound {
				{typeString, ID},
				{nameString, name},
				{killsString, kills}
			};
		}

		public static Soul Load(TagCompound tag)
		{
			if (!tag.ContainsKey(nameString))
				return null;

			return new Soul
			{
				ID = tag.GetInt(typeString),
				name = tag.GetString(nameString),
				kills = tag.GetInt(killsString)
			};
		}

		public void NetSend(BinaryWriter writer)
		{
			writer.Write(ID);
			writer.Write(name);
			writer.Write(kills);
		}

		public static Soul NetReceive(BinaryReader reader)
		{
			return new Soul()
			{
				ID = reader.ReadInt32(),
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
