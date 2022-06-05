using System;
using UnityEngine;

// Token: 0x0200023C RID: 572
[CreateAssetMenu(menuName = "Custom/Libraries/MapTerrain Library")]
public class MapTerrainLibrary : ScriptableObject
{
	// Token: 0x17000B40 RID: 2880
	// (get) Token: 0x06001709 RID: 5897 RVA: 0x00047D80 File Offset: 0x00045F80
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

	// Token: 0x0600170A RID: 5898 RVA: 0x00047DB0 File Offset: 0x00045FB0
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

	// Token: 0x0600170B RID: 5899 RVA: 0x00047E0C File Offset: 0x0004600C
	public static Ferr2DT_PathTerrain GetDefaultMapTerrain()
	{
		return MapTerrainLibrary.Instance.m_defaultMapTerrain;
	}

	// Token: 0x04001684 RID: 5764
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/MapTerrainLibrary";

	// Token: 0x04001685 RID: 5765
	private const string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/MapTerrainLibrary.asset";

	// Token: 0x04001686 RID: 5766
	[HelpBox("The elements must be in ascending Y size order. E.g. Element 0 must have a Y size of 1, element 1 must have a Y size of 2, etc.", HelpBoxMessageType.None)]
	[SerializeField]
	private Ferr2DT_PathTerrain[] m_oneWidthMapTerrains;

	// Token: 0x04001687 RID: 5767
	[SerializeField]
	private Ferr2DT_PathTerrain[] m_twoWidthMapTerrains;

	// Token: 0x04001688 RID: 5768
	[SerializeField]
	private Ferr2DT_PathTerrain[] m_threeWidthMapTerrains;

	// Token: 0x04001689 RID: 5769
	[SerializeField]
	private Ferr2DT_PathTerrain m_defaultMapTerrain;

	// Token: 0x0400168A RID: 5770
	private static MapTerrainLibrary m_instance;
}
