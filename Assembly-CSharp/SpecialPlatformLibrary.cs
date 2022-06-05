using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200040F RID: 1039
[CreateAssetMenu(menuName = "Custom/Rogue Legacy 2/Special Platform Library")]
public class SpecialPlatformLibrary : ScriptableObject
{
	// Token: 0x17000E8F RID: 3727
	// (get) Token: 0x0600212A RID: 8490 RVA: 0x000A6A40 File Offset: 0x000A4C40
	private static SpecialPlatformLibrary Instance
	{
		get
		{
			if (SpecialPlatformLibrary.m_instance == null)
			{
				if (Application.isPlaying)
				{
					SpecialPlatformLibrary.m_instance = CDGResources.Load<SpecialPlatformLibrary>("Scriptable Objects/Libraries/SpecialPlatformLibrary", "", true);
				}
				if (SpecialPlatformLibrary.m_instance == null)
				{
					Debug.LogFormat("<color=red>| SpecialPlatformLibrary | Failed to find SpecialPlatformLibrary SO at Path ({0})</color>", new object[]
					{
						"Scriptable Objects/Libraries/SpecialPlatformLibrary"
					});
				}
			}
			return SpecialPlatformLibrary.m_instance;
		}
	}

	// Token: 0x17000E90 RID: 3728
	// (get) Token: 0x0600212B RID: 8491 RVA: 0x00011A5C File Offset: 0x0000FC5C
	// (set) Token: 0x0600212C RID: 8492 RVA: 0x00011A68 File Offset: 0x0000FC68
	public static BiomeSpecialPlatformEntry[] SpecialPlatformsInBiomes
	{
		get
		{
			return SpecialPlatformLibrary.Instance.m_specialPlatformsInBiomes;
		}
		set
		{
			if (!Application.isPlaying)
			{
				SpecialPlatformLibrary.Instance.m_specialPlatformsInBiomes = value;
			}
		}
	}

	// Token: 0x17000E91 RID: 3729
	// (get) Token: 0x0600212D RID: 8493 RVA: 0x00011A7C File Offset: 0x0000FC7C
	// (set) Token: 0x0600212E RID: 8494 RVA: 0x00011A88 File Offset: 0x0000FC88
	public static SpecialPlatformType[] DefaultSpecialPlatformsInBiomes
	{
		get
		{
			return SpecialPlatformLibrary.Instance.m_defaultSpecialPlatformsInBiomes;
		}
		set
		{
			if (!Application.isPlaying)
			{
				SpecialPlatformLibrary.Instance.m_defaultSpecialPlatformsInBiomes = value;
			}
		}
	}

	// Token: 0x0600212F RID: 8495 RVA: 0x000A6AA0 File Offset: 0x000A4CA0
	private static void CreateEntryTable()
	{
		SpecialPlatformLibrary.m_entryTable = new Dictionary<SpecialPlatformType, SpecialPlatformLibrary.SpecialPlatformEntry>();
		foreach (SpecialPlatformLibrary.SpecialPlatformEntry specialPlatformEntry in SpecialPlatformLibrary.Instance.m_entries)
		{
			SpecialPlatformLibrary.m_entryTable.Add(specialPlatformEntry.Type, specialPlatformEntry);
		}
	}

	// Token: 0x06002130 RID: 8496 RVA: 0x000A6AE8 File Offset: 0x000A4CE8
	public static SpecialPlatform CreatePlatformInstance(SpecialPlatformType type, bool isActive = true)
	{
		if (SpecialPlatformLibrary.m_entryTable == null)
		{
			SpecialPlatformLibrary.CreateEntryTable();
		}
		if (SpecialPlatformLibrary.m_entryTable.ContainsKey(type))
		{
			SpecialPlatform specialPlatform = UnityEngine.Object.Instantiate<SpecialPlatform>(SpecialPlatformLibrary.m_entryTable[type].Prefab);
			specialPlatform.gameObject.SetActive(isActive);
			return specialPlatform;
		}
		Debug.LogFormat("<color=red>| {0} | Table doesn't contain an entry matching Type ({1})</color>", new object[]
		{
			SpecialPlatformLibrary.Instance,
			type
		});
		return null;
	}

	// Token: 0x06002131 RID: 8497 RVA: 0x000A6B54 File Offset: 0x000A4D54
	public static SpecialPlatformType[] GetPlatformTypesInBiome(BiomeType biome)
	{
		if (SpecialPlatformLibrary.m_specialPlatformsInBiomeTable == null)
		{
			SpecialPlatformLibrary.InitialiseSpecialPlatformTable();
		}
		SpecialPlatformType[] result = SpecialPlatformLibrary.DefaultSpecialPlatformsInBiomes;
		if (SpecialPlatformLibrary.m_specialPlatformsInBiomeTable.ContainsKey(biome))
		{
			result = SpecialPlatformLibrary.m_specialPlatformsInBiomeTable[biome];
		}
		return result;
	}

	// Token: 0x06002132 RID: 8498 RVA: 0x000A6B90 File Offset: 0x000A4D90
	private static void InitialiseSpecialPlatformTable()
	{
		SpecialPlatformLibrary.m_specialPlatformsInBiomeTable = new Dictionary<BiomeType, SpecialPlatformType[]>();
		foreach (BiomeSpecialPlatformEntry biomeSpecialPlatformEntry in SpecialPlatformLibrary.SpecialPlatformsInBiomes)
		{
			SpecialPlatformLibrary.m_specialPlatformsInBiomeTable.Add(biomeSpecialPlatformEntry.Biome, biomeSpecialPlatformEntry.SpecialPlatforms);
		}
	}

	// Token: 0x04001E20 RID: 7712
	[SerializeField]
	private SpecialPlatformLibrary.SpecialPlatformEntry[] m_entries;

	// Token: 0x04001E21 RID: 7713
	[SerializeField]
	private BiomeSpecialPlatformEntry[] m_specialPlatformsInBiomes;

	// Token: 0x04001E22 RID: 7714
	[SerializeField]
	private SpecialPlatformType[] m_defaultSpecialPlatformsInBiomes;

	// Token: 0x04001E23 RID: 7715
	private const string RESOURCES_PATH = "Scriptable Objects/Libraries/SpecialPlatformLibrary";

	// Token: 0x04001E24 RID: 7716
	private const string EDITOR_PATH = "Assets/Content/Scriptable Objects/Libraries/SpecialPlatformLibrary.asset";

	// Token: 0x04001E25 RID: 7717
	private static Dictionary<SpecialPlatformType, SpecialPlatformLibrary.SpecialPlatformEntry> m_entryTable;

	// Token: 0x04001E26 RID: 7718
	private static SpecialPlatformLibrary m_instance;

	// Token: 0x04001E27 RID: 7719
	private static Dictionary<BiomeType, SpecialPlatformType[]> m_specialPlatformsInBiomeTable;

	// Token: 0x02000410 RID: 1040
	[Serializable]
	private class SpecialPlatformEntry
	{
		// Token: 0x04001E28 RID: 7720
		public SpecialPlatformType Type;

		// Token: 0x04001E29 RID: 7721
		public SpecialPlatform Prefab;
	}
}
