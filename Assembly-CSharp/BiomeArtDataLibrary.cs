using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003CF RID: 975
[CreateAssetMenu(menuName = "Custom/Libraries/Art Data Library")]
public class BiomeArtDataLibrary : ScriptableObject
{
	// Token: 0x17000E41 RID: 3649
	// (get) Token: 0x06001FF3 RID: 8179 RVA: 0x00010E5E File Offset: 0x0000F05E
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

	// Token: 0x17000E42 RID: 3650
	// (get) Token: 0x06001FF4 RID: 8180 RVA: 0x00010E91 File Offset: 0x0000F091
	public static List<BiomeArtDataEntry> ArtDataTable
	{
		get
		{
			return BiomeArtDataLibrary.Instance.m_artTable;
		}
	}

	// Token: 0x06001FF5 RID: 8181 RVA: 0x000A418C File Offset: 0x000A238C
	private void Initialize()
	{
		BiomeArtDataLibrary.Instance.m_artDataDictionary = new Dictionary<BiomeType, BiomeArtData>();
		foreach (BiomeArtDataEntry biomeArtDataEntry in this.m_artTable)
		{
			BiomeArtDataLibrary.Instance.m_artDataDictionary.Add(biomeArtDataEntry.BiomeType, biomeArtDataEntry.BiomeArtData);
		}
	}

	// Token: 0x06001FF6 RID: 8182 RVA: 0x00010E9D File Offset: 0x0000F09D
	public static BiomeArtData GetArtData(BiomeType biome)
	{
		return BiomeArtDataLibrary.Instance.m_artDataDictionary[biome];
	}

	// Token: 0x04001C99 RID: 7321
	[SerializeField]
	private List<BiomeArtDataEntry> m_artTable;

	// Token: 0x04001C9A RID: 7322
	public static string RESOURCES_PATH = "Scriptable Objects/Libraries/BiomeArtDataLibrary";

	// Token: 0x04001C9B RID: 7323
	private Dictionary<BiomeType, BiomeArtData> m_artDataDictionary;

	// Token: 0x04001C9C RID: 7324
	private static BiomeArtDataLibrary m_instance;
}
