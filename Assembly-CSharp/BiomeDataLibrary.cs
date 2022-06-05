using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003D1 RID: 977
[CreateAssetMenu(menuName = "Custom/Libraries/Biome Library")]
public class BiomeDataLibrary : ScriptableObject
{
	// Token: 0x17000E43 RID: 3651
	// (get) Token: 0x06001FFA RID: 8186 RVA: 0x00010EBB File Offset: 0x0000F0BB
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

	// Token: 0x06001FFB RID: 8187 RVA: 0x000A4204 File Offset: 0x000A2404
	private void Initialize()
	{
		BiomeDataLibrary.Instance.m_dataDictionary = new Dictionary<BiomeType, BiomeData>();
		foreach (BiomeDataEntry biomeDataEntry in this.m_dataTable)
		{
			BiomeDataLibrary.Instance.m_dataDictionary.Add(biomeDataEntry.BiomeType, biomeDataEntry.BiomeData);
		}
	}

	// Token: 0x06001FFC RID: 8188 RVA: 0x00010EEE File Offset: 0x0000F0EE
	public static BiomeData GetData(BiomeType biomeType)
	{
		return BiomeDataLibrary.Instance.m_dataDictionary[biomeType];
	}

	// Token: 0x04001C9F RID: 7327
	private const string RESOURCES_PATH = "Scriptable Objects/Libraries/BiomeDataLibrary";

	// Token: 0x04001CA0 RID: 7328
	[SerializeField]
	private List<BiomeDataEntry> m_dataTable;

	// Token: 0x04001CA1 RID: 7329
	private Dictionary<BiomeType, BiomeData> m_dataDictionary;

	// Token: 0x04001CA2 RID: 7330
	private static BiomeDataLibrary m_instance;
}
