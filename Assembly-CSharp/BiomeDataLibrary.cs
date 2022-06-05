using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200021E RID: 542
[CreateAssetMenu(menuName = "Custom/Libraries/Biome Library")]
public class BiomeDataLibrary : ScriptableObject
{
	// Token: 0x17000B1C RID: 2844
	// (get) Token: 0x0600165E RID: 5726 RVA: 0x00045D1E File Offset: 0x00043F1E
	private static BiomeDataLibrary Instance
	{
		get
		{
			if (BiomeDataLibrary.m_instance == null)
			{
				BiomeDataLibrary.m_instance = CDGResources.Load<BiomeDataLibrary>("Scriptable Objects/Libraries/BiomeDataLibrary", "", true);
				BiomeDataLibrary.m_instance.Initialize();
			}
			return BiomeDataLibrary.m_instance;
		}
	}

	// Token: 0x0600165F RID: 5727 RVA: 0x00045D54 File Offset: 0x00043F54
	private void Initialize()
	{
		BiomeDataLibrary.Instance.m_dataDictionary = new Dictionary<BiomeType, BiomeData>();
		foreach (BiomeDataEntry biomeDataEntry in this.m_dataTable)
		{
			BiomeDataLibrary.Instance.m_dataDictionary.Add(biomeDataEntry.BiomeType, biomeDataEntry.BiomeData);
		}
	}

	// Token: 0x06001660 RID: 5728 RVA: 0x00045DCC File Offset: 0x00043FCC
	public static BiomeData GetData(BiomeType biomeType)
	{
		return BiomeDataLibrary.Instance.m_dataDictionary[biomeType];
	}

	// Token: 0x0400159C RID: 5532
	private const string RESOURCES_PATH = "Scriptable Objects/Libraries/BiomeDataLibrary";

	// Token: 0x0400159D RID: 5533
	[SerializeField]
	private List<BiomeDataEntry> m_dataTable;

	// Token: 0x0400159E RID: 5534
	private Dictionary<BiomeType, BiomeData> m_dataDictionary;

	// Token: 0x0400159F RID: 5535
	private static BiomeDataLibrary m_instance;
}
