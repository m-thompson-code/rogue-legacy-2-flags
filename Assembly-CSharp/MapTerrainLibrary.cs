using System;
using UnityEngine;

// Token: 0x020003F9 RID: 1017
[CreateAssetMenu(menuName = "Custom/Libraries/MapTerrain Library")]
public class MapTerrainLibrary : ScriptableObject
{
	// Token: 0x17000E6D RID: 3693
	// (get) Token: 0x060020BC RID: 8380 RVA: 0x000115B0 File Offset: 0x0000F7B0
	private static MapTerrainLibrary Instance
	{
		get
		{
			if (MapTerrainLibrary.m_instance == null && Application.isPlaying)
			{
				MapTerrainLibrary.m_instance = CDGResources.Load<MapTerrainLibrary>("Scriptable Objects/Libraries/MapTerrainLibrary", "", true);
			}
			return MapTerrainLibrary.m_instance;
		}
	}

	// Token: 0x060020BD RID: 8381 RVA: 0x000A5BA0 File Offset: 0x000A3DA0
	public static Ferr2DT_PathTerrain GetMapTerrain(int width, int height)
	{
		Ferr2DT_PathTerrain[] array = null;
		switch (width)
		{
		case 1:
			array = MapTerrainLibrary.Instance.m_oneWidthMapTerrains;
			break;
		case 2:
			array = MapTerrainLibrary.Instance.m_twoWidthMapTerrains;
			break;
		case 3:
			array = MapTerrainLibrary.Instance.m_threeWidthMapTerrains;
			break;
		}
		if (array != null && array.Length > height - 1)
		{
			return array[height - 1];
		}
		return null;
	}

	// Token: 0x060020BE RID: 8382 RVA: 0x000115E0 File Offset: 0x0000F7E0
	public static Ferr2DT_PathTerrain GetDefaultMapTerrain()
	{
		return MapTerrainLibrary.Instance.m_defaultMapTerrain;
	}

	// Token: 0x04001D9C RID: 7580
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/MapTerrainLibrary";

	// Token: 0x04001D9D RID: 7581
	private const string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/MapTerrainLibrary.asset";

	// Token: 0x04001D9E RID: 7582
	[HelpBox("The elements must be in ascending Y size order. E.g. Element 0 must have a Y size of 1, element 1 must have a Y size of 2, etc.", HelpBoxMessageType.None)]
	[SerializeField]
	private Ferr2DT_PathTerrain[] m_oneWidthMapTerrains;

	// Token: 0x04001D9F RID: 7583
	[SerializeField]
	private Ferr2DT_PathTerrain[] m_twoWidthMapTerrains;

	// Token: 0x04001DA0 RID: 7584
	[SerializeField]
	private Ferr2DT_PathTerrain[] m_threeWidthMapTerrains;

	// Token: 0x04001DA1 RID: 7585
	[SerializeField]
	private Ferr2DT_PathTerrain m_defaultMapTerrain;

	// Token: 0x04001DA2 RID: 7586
	private static MapTerrainLibrary m_instance;
}
