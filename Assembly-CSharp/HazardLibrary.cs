using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000234 RID: 564
[CreateAssetMenu(menuName = "Custom/Libraries/Hazard Library")]
public class HazardLibrary : ScriptableObject
{
	// Token: 0x17000B33 RID: 2867
	// (get) Token: 0x060016BF RID: 5823 RVA: 0x00046F20 File Offset: 0x00045120
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

	// Token: 0x17000B34 RID: 2868
	// (get) Token: 0x060016C0 RID: 5824 RVA: 0x00046F49 File Offset: 0x00045149
	// (set) Token: 0x060016C1 RID: 5825 RVA: 0x00046F55 File Offset: 0x00045155
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

	// Token: 0x060016C2 RID: 5826 RVA: 0x00046F64 File Offset: 0x00045164
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

	// Token: 0x060016C3 RID: 5827 RVA: 0x00046FD4 File Offset: 0x000451D4
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

	// Token: 0x060016C4 RID: 5828 RVA: 0x00047058 File Offset: 0x00045258
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

	// Token: 0x060016C5 RID: 5829 RVA: 0x000470DC File Offset: 0x000452DC
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

	// Token: 0x060016C6 RID: 5830 RVA: 0x00047108 File Offset: 0x00045308
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

	// Token: 0x060016C7 RID: 5831 RVA: 0x0004716C File Offset: 0x0004536C
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

	// Token: 0x04001633 RID: 5683
	[SerializeField]
	private HazardLibraryEntry[] m_hazardPrefabs;

	// Token: 0x04001634 RID: 5684
	[Header("Point Hazards")]
	[SerializeField]
	private Hazards m_pointHazards;

	// Token: 0x04001635 RID: 5685
	[Header("Line Hazards")]
	[SerializeField]
	private Hazards m_lineHazards;

	// Token: 0x04001636 RID: 5686
	[Header("Turret Hazards")]
	[SerializeField]
	private Hazards m_turretHazards;

	// Token: 0x04001637 RID: 5687
	[Header("Turret Hazards")]
	[SerializeField]
	private Hazards m_spikeHazards;

	// Token: 0x04001638 RID: 5688
	private static HazardLibrary m_instance = null;

	// Token: 0x04001639 RID: 5689
	private static Dictionary<HazardType, Hazard> m_hazardTable = null;

	// Token: 0x0400163A RID: 5690
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/HazardLibrary";

	// Token: 0x0400163B RID: 5691
	public const string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/HazardLibrary.asset";

	// Token: 0x0400163C RID: 5692
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

	// Token: 0x0400163D RID: 5693
	private static HazardType[] TURRET_HAZARDS = new HazardType[]
	{
		HazardType.None,
		HazardType.BiomeSpecific,
		HazardType.WallTurret_Standard,
		HazardType.WallTurret_FlameThrower,
		HazardType.RaycastTurret_Arrow,
		HazardType.RaycastTurret_Curse
	};

	// Token: 0x0400163E RID: 5694
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

	// Token: 0x0400163F RID: 5695
	private static HazardType[] FERR2D_HAZARDS = new HazardType[]
	{
		HazardType.None,
		HazardType.BiomeSpecific,
		HazardType.TallSpike
	};
}
