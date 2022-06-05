using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000252 RID: 594
[CreateAssetMenu(menuName = "Custom/Rogue Legacy 2/Special Platform Library")]
public class SpecialPlatformLibrary : ScriptableObject
{
	// Token: 0x17000B62 RID: 2914
	// (get) Token: 0x06001777 RID: 6007 RVA: 0x00049154 File Offset: 0x00047354
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

	// Token: 0x17000B63 RID: 2915
	// (get) Token: 0x06001778 RID: 6008 RVA: 0x000491B4 File Offset: 0x000473B4
	// (set) Token: 0x06001779 RID: 6009 RVA: 0x000491C0 File Offset: 0x000473C0
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

	// Token: 0x17000B64 RID: 2916
	// (get) Token: 0x0600177A RID: 6010 RVA: 0x000491D4 File Offset: 0x000473D4
	// (set) Token: 0x0600177B RID: 6011 RVA: 0x000491E0 File Offset: 0x000473E0
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

	// Token: 0x0600177C RID: 6012 RVA: 0x000491F4 File Offset: 0x000473F4
	private static void CreateEntryTable()
	{
		SpecialPlatformLibrary.m_entryTable = new Dictionary<SpecialPlatformType, SpecialPlatformLibrary.SpecialPlatformEntry>();
		foreach (SpecialPlatformLibrary.SpecialPlatformEntry specialPlatformEntry in SpecialPlatformLibrary.Instance.m_entries)
		{
			SpecialPlatformLibrary.m_entryTable.Add(specialPlatformEntry.Type, specialPlatformEntry);
		}
	}

	// Token: 0x0600177D RID: 6013 RVA: 0x0004923C File Offset: 0x0004743C
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

	// Token: 0x0600177E RID: 6014 RVA: 0x000492A8 File Offset: 0x000474A8
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

	// Token: 0x0600177F RID: 6015 RVA: 0x000492E4 File Offset: 0x000474E4
	private static void InitialiseSpecialPlatformTable()
	{
		SpecialPlatformLibrary.m_specialPlatformsInBiomeTable = new Dictionary<BiomeType, SpecialPlatformType[]>();
		foreach (BiomeSpecialPlatformEntry biomeSpecialPlatformEntry in SpecialPlatformLibrary.SpecialPlatformsInBiomes)
		{
			SpecialPlatformLibrary.m_specialPlatformsInBiomeTable.Add(biomeSpecialPlatformEntry.Biome, biomeSpecialPlatformEntry.SpecialPlatforms);
		}
	}

	// Token: 0x04001708 RID: 5896
	[SerializeField]
	private SpecialPlatformLibrary.SpecialPlatformEntry[] m_entries;

	// Token: 0x04001709 RID: 5897
	[SerializeField]
	private BiomeSpecialPlatformEntry[] m_specialPlatformsInBiomes;

	// Token: 0x0400170A RID: 5898
	[SerializeField]
	private SpecialPlatformType[] m_defaultSpecialPlatformsInBiomes;

	// Token: 0x0400170B RID: 5899
	private const string RESOURCES_PATH = "Scriptable Objects/Libraries/SpecialPlatformLibrary";

	// Token: 0x0400170C RID: 5900
	private const string EDITOR_PATH = "Assets/Content/Scriptable Objects/Libraries/SpecialPlatformLibrary.asset";

	// Token: 0x0400170D RID: 5901
	private static Dictionary<SpecialPlatformType, SpecialPlatformLibrary.SpecialPlatformEntry> m_entryTable;

	// Token: 0x0400170E RID: 5902
	private static SpecialPlatformLibrary m_instance;

	// Token: 0x0400170F RID: 5903
	private static Dictionary<BiomeType, SpecialPlatformType[]> m_specialPlatformsInBiomeTable;

	// Token: 0x02000B36 RID: 2870
	[Serializable]
	private class SpecialPlatformEntry
	{
		// Token: 0x04004BAD RID: 19373
		public SpecialPlatformType Type;

		// Token: 0x04004BAE RID: 19374
		public SpecialPlatform Prefab;
	}
}
