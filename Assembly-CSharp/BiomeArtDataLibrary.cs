using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200021C RID: 540
[CreateAssetMenu(menuName = "Custom/Libraries/Art Data Library")]
public class BiomeArtDataLibrary : ScriptableObject
{
	// Token: 0x17000B1A RID: 2842
	// (get) Token: 0x06001657 RID: 5719 RVA: 0x00045C36 File Offset: 0x00043E36
	private static BiomeArtDataLibrary Instance
	{
		get
		{
			if (BiomeArtDataLibrary.m_instance == null)
			{
				BiomeArtDataLibrary.m_instance = CDGResources.Load<BiomeArtDataLibrary>(BiomeArtDataLibrary.RESOURCES_PATH, "", true);
				BiomeArtDataLibrary.m_instance.Initialize();
			}
			return BiomeArtDataLibrary.m_instance;
		}
	}

	// Token: 0x17000B1B RID: 2843
	// (get) Token: 0x06001658 RID: 5720 RVA: 0x00045C69 File Offset: 0x00043E69
	public static List<BiomeArtDataEntry> ArtDataTable
	{
		get
		{
			return BiomeArtDataLibrary.Instance.m_artTable;
		}
	}

	// Token: 0x06001659 RID: 5721 RVA: 0x00045C78 File Offset: 0x00043E78
	private void Initialize()
	{
		BiomeArtDataLibrary.Instance.m_artDataDictionary = new Dictionary<BiomeType, BiomeArtData>();
		foreach (BiomeArtDataEntry biomeArtDataEntry in this.m_artTable)
		{
			BiomeArtDataLibrary.Instance.m_artDataDictionary.Add(biomeArtDataEntry.BiomeType, biomeArtDataEntry.BiomeArtData);
		}
	}

	// Token: 0x0600165A RID: 5722 RVA: 0x00045CF0 File Offset: 0x00043EF0
	public static BiomeArtData GetArtData(BiomeType biome)
	{
		return BiomeArtDataLibrary.Instance.m_artDataDictionary[biome];
	}

	// Token: 0x04001596 RID: 5526
	[SerializeField]
	private List<BiomeArtDataEntry> m_artTable;

	// Token: 0x04001597 RID: 5527
	public static string RESOURCES_PATH = "Scriptable Objects/Libraries/BiomeArtDataLibrary";

	// Token: 0x04001598 RID: 5528
	private Dictionary<BiomeType, BiomeArtData> m_artDataDictionary;

	// Token: 0x04001599 RID: 5529
	private static BiomeArtDataLibrary m_instance;
}
