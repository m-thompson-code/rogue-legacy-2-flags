using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003F1 RID: 1009
[CreateAssetMenu(menuName = "Custom/Libraries/Hazard Library")]
public class HazardLibrary : ScriptableObject
{
	// Token: 0x17000E60 RID: 3680
	// (get) Token: 0x06002072 RID: 8306 RVA: 0x00011335 File Offset: 0x0000F535
	public static HazardLibrary Instance
	{
		get
		{
			if (HazardLibrary.m_instance == null)
			{
				HazardLibrary.m_instance = CDGResources.Load<HazardLibrary>("Scriptable Objects/Libraries/HazardLibrary", "", true);
			}
			return HazardLibrary.m_instance;
		}
	}

	// Token: 0x17000E61 RID: 3681
	// (get) Token: 0x06002073 RID: 8307 RVA: 0x0001135E File Offset: 0x0000F55E
	// (set) Token: 0x06002074 RID: 8308 RVA: 0x0001136A File Offset: 0x0000F56A
	public static HazardLibraryEntry[] HazardPrefabs
	{
		get
		{
			return HazardLibrary.Instance.m_hazardPrefabs;
		}
		set
		{
			HazardLibrary.Instance.m_hazardPrefabs = value;
		}
	}

	// Token: 0x06002075 RID: 8309 RVA: 0x000A4FF0 File Offset: 0x000A31F0
	public static Hazards GetHazards(HazardCategory hazardCategory)
	{
		if (hazardCategory <= HazardCategory.Line)
		{
			if (hazardCategory == HazardCategory.Point)
			{
				return HazardLibrary.Instance.m_pointHazards;
			}
			if (hazardCategory == HazardCategory.Line)
			{
				return HazardLibrary.Instance.m_lineHazards;
			}
		}
		else
		{
			if (hazardCategory == HazardCategory.Turret)
			{
				return HazardLibrary.Instance.m_turretHazards;
			}
			if (hazardCategory == HazardCategory.Ferr2D)
			{
				return HazardLibrary.Instance.m_spikeHazards;
			}
		}
		Debug.LogFormat("<color=red>| HazardLibrary | No entry found matching given Hazard category ({0})", new object[]
		{
			hazardCategory
		});
		return null;
	}

	// Token: 0x06002076 RID: 8310 RVA: 0x000A5060 File Offset: 0x000A3260
	public static Dictionary<BiomeType, HazardType[]> GetHazardsInBiomeTable(HazardCategory hazardCategory)
	{
		if (hazardCategory <= HazardCategory.Line)
		{
			if (hazardCategory == HazardCategory.Point)
			{
				return HazardLibrary.Instance.m_pointHazards.HazardsInBiomeTable;
			}
			if (hazardCategory == HazardCategory.Line)
			{
				return HazardLibrary.Instance.m_lineHazards.HazardsInBiomeTable;
			}
		}
		else
		{
			if (hazardCategory == HazardCategory.Turret)
			{
				return HazardLibrary.Instance.m_turretHazards.HazardsInBiomeTable;
			}
			if (hazardCategory == HazardCategory.Ferr2D)
			{
				return HazardLibrary.Instance.m_spikeHazards.HazardsInBiomeTable;
			}
		}
		Debug.LogFormat("<color=red>| HazardLibrary | No entry found matching given Hazard category ({0})", new object[]
		{
			hazardCategory
		});
		return null;
	}

	// Token: 0x06002077 RID: 8311 RVA: 0x000A50E4 File Offset: 0x000A32E4
	public static HazardType[] GetDefaultHazardTypes(HazardCategory hazardCategory)
	{
		if (hazardCategory <= HazardCategory.Line)
		{
			if (hazardCategory == HazardCategory.Point)
			{
				return HazardLibrary.Instance.m_pointHazards.DefaultHazardsInBiomes;
			}
			if (hazardCategory == HazardCategory.Line)
			{
				return HazardLibrary.Instance.m_lineHazards.DefaultHazardsInBiomes;
			}
		}
		else
		{
			if (hazardCategory == HazardCategory.Turret)
			{
				return HazardLibrary.Instance.m_turretHazards.DefaultHazardsInBiomes;
			}
			if (hazardCategory == HazardCategory.Ferr2D)
			{
				return HazardLibrary.Instance.m_spikeHazards.DefaultHazardsInBiomes;
			}
		}
		Debug.LogFormat("<color=red>| HazardLibrary | No entry found matching given Hazard category ({0})", new object[]
		{
			hazardCategory
		});
		return null;
	}

	// Token: 0x06002078 RID: 8312 RVA: 0x000A5168 File Offset: 0x000A3368
	public static int GetHazardTypeIndexInHazardCategory(HazardType hazardType, HazardCategory hazardCategory)
	{
		HazardType[] hazardTypesInHazardCategory = HazardLibrary.GetHazardTypesInHazardCategory(hazardCategory);
		for (int i = 0; i < hazardTypesInHazardCategory.Length; i++)
		{
			if (hazardTypesInHazardCategory[i] == hazardType)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06002079 RID: 8313 RVA: 0x000A5194 File Offset: 0x000A3394
	public static HazardType[] GetHazardTypesInHazardCategory(HazardCategory hazardCategory)
	{
		if (hazardCategory <= HazardCategory.Line)
		{
			if (hazardCategory == HazardCategory.Point)
			{
				return HazardLibrary.POINT_HAZARDS;
			}
			if (hazardCategory == HazardCategory.Line)
			{
				return HazardLibrary.LINE_HAZARDS;
			}
		}
		else
		{
			if (hazardCategory == HazardCategory.Turret)
			{
				return HazardLibrary.TURRET_HAZARDS;
			}
			if (hazardCategory == HazardCategory.Ferr2D)
			{
				return HazardLibrary.FERR2D_HAZARDS;
			}
		}
		Debug.LogFormat("<color=red>| HazardLibrary | No entry for Hazard Category ({0})</color>", new object[]
		{
			hazardCategory
		});
		return new HazardType[0];
	}

	// Token: 0x0600207A RID: 8314 RVA: 0x000A51F8 File Offset: 0x000A33F8
	public static Hazard GetPrefab(HazardType hazardType)
	{
		if (HazardLibrary.m_hazardTable == null)
		{
			HazardLibrary.m_hazardTable = new Dictionary<HazardType, Hazard>();
			foreach (HazardLibraryEntry hazardLibraryEntry in HazardLibrary.HazardPrefabs)
			{
				HazardLibrary.m_hazardTable.Add(hazardLibraryEntry.Hazard, hazardLibraryEntry.Prefab);
			}
		}
		if (HazardLibrary.m_hazardTable.ContainsKey(hazardType))
		{
			return HazardLibrary.m_hazardTable[hazardType];
		}
		Debug.LogFormat("<color=red>| HazardLibrary | Hazard Table does not contain an entry for Hazard Type ({0})</color>", new object[]
		{
			hazardType
		});
		return null;
	}

	// Token: 0x04001D4B RID: 7499
	[SerializeField]
	private HazardLibraryEntry[] m_hazardPrefabs;

	// Token: 0x04001D4C RID: 7500
	[Header("Point Hazards")]
	[SerializeField]
	private Hazards m_pointHazards;

	// Token: 0x04001D4D RID: 7501
	[Header("Line Hazards")]
	[SerializeField]
	private Hazards m_lineHazards;

	// Token: 0x04001D4E RID: 7502
	[Header("Turret Hazards")]
	[SerializeField]
	private Hazards m_turretHazards;

	// Token: 0x04001D4F RID: 7503
	[Header("Turret Hazards")]
	[SerializeField]
	private Hazards m_spikeHazards;

	// Token: 0x04001D50 RID: 7504
	private static HazardLibrary m_instance = null;

	// Token: 0x04001D51 RID: 7505
	private static Dictionary<HazardType, Hazard> m_hazardTable = null;

	// Token: 0x04001D52 RID: 7506
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/HazardLibrary";

	// Token: 0x04001D53 RID: 7507
	public const string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/HazardLibrary.asset";

	// Token: 0x04001D54 RID: 7508
	private static HazardType[] LINE_HAZARDS = new HazardType[]
	{
		HazardType.None,
		HazardType.BiomeSpecific,
		HazardType.SpikeTrap,
		HazardType.Buzzsaw,
		HazardType.BreakableSpike,
		HazardType.PressurePlate,
		HazardType.SpringTrap,
		HazardType.Conveyor,
		HazardType.SnowMound,
		HazardType.Bodies,
		HazardType.BreakableSpikeTall
	};

	// Token: 0x04001D55 RID: 7509
	private static HazardType[] TURRET_HAZARDS = new HazardType[]
	{
		HazardType.None,
		HazardType.BiomeSpecific,
		HazardType.WallTurret_Standard,
		HazardType.WallTurret_FlameThrower,
		HazardType.RaycastTurret_Arrow,
		HazardType.RaycastTurret_Curse
	};

	// Token: 0x04001D56 RID: 7510
	private static HazardType[] POINT_HAZARDS = new HazardType[]
	{
		HazardType.None,
		HazardType.BiomeSpecific,
		HazardType.Orbiter,
		HazardType.Triple_Orbiter,
		HazardType.Windmill,
		HazardType.ShrinkOnDash,
		HazardType.Sentry,
		HazardType.ProximityMine,
		HazardType.VoidTrap,
		HazardType.ExhaustPoint,
		HazardType.HomingVine,
		HazardType.ProximityProjectile,
		HazardType.IceCrystal,
		HazardType.SentryWithIce,
		HazardType.RisingWater
	};

	// Token: 0x04001D57 RID: 7511
	private static HazardType[] FERR2D_HAZARDS = new HazardType[]
	{
		HazardType.None,
		HazardType.BiomeSpecific,
		HazardType.TallSpike
	};
}
