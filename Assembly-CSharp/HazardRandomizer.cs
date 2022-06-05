using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000617 RID: 1559
public static class HazardRandomizer
{
	// Token: 0x0600384C RID: 14412 RVA: 0x000C0434 File Offset: 0x000BE634
	private static HazardType GetRandomHazardType(HazardCategory category)
	{
		HazardType result = HazardType.None;
		if (!HazardRandomizer.m_hazardTypeTable.ContainsKey(category))
		{
			HazardRandomizer.m_hazardTypeTable.Add(category, (from entry in HazardLibrary.GetHazardTypesInHazardCategory(category)
			where entry > HazardType.BiomeSpecific
			select entry).ToList<HazardType>());
		}
		else if (HazardRandomizer.m_hazardTypeTable[category].Count == 0)
		{
			HazardRandomizer.m_hazardTypeTable[category] = (from entry in HazardLibrary.GetHazardTypesInHazardCategory(category)
			where entry > HazardType.BiomeSpecific
			select entry).ToList<HazardType>();
		}
		if (HazardRandomizer.m_hazardTypeTable[category].Count > 0)
		{
			int index = 0;
			if (HazardRandomizer.m_hazardTypeTable[category].Count > 1)
			{
				index = UnityEngine.Random.Range(0, HazardRandomizer.m_hazardTypeTable[category].Count);
			}
			result = HazardRandomizer.m_hazardTypeTable[category][index];
			HazardRandomizer.m_hazardTypeTable[category].RemoveAt(index);
		}
		return result;
	}

	// Token: 0x0600384D RID: 14413 RVA: 0x000C0540 File Offset: 0x000BE740
	public static void RandomizeHazards(IEnumerable<IHazardSpawnController> spawnControllers)
	{
		IEnumerable<IHazardSpawnController> spawnControllers2 = from entry in spawnControllers
		where entry is PointHazardSpawnController
		select entry;
		IEnumerable<IHazardSpawnController> spawnControllers3 = from entry in spawnControllers
		where entry is LineHazardSpawnController
		select entry;
		IEnumerable<IHazardSpawnController> spawnControllers4 = from entry in spawnControllers
		where entry is TurretHazardSpawnController
		select entry;
		HazardRandomizer.SetRandomHazardType(HazardCategory.Point, spawnControllers2);
		HazardRandomizer.SetRandomHazardType(HazardCategory.Line, spawnControllers3);
		HazardRandomizer.SetRandomHazardType(HazardCategory.Turret, spawnControllers4);
	}

	// Token: 0x0600384E RID: 14414 RVA: 0x000C05D8 File Offset: 0x000BE7D8
	private static void SetRandomHazardType(HazardCategory category, IEnumerable<IHazardSpawnController> spawnControllers)
	{
		if (spawnControllers.Count<IHazardSpawnController>() == 0)
		{
			return;
		}
		HazardType randomHazardType = HazardRandomizer.GetRandomHazardType(category);
		if (randomHazardType != HazardType.None)
		{
			foreach (IHazardSpawnController hazardSpawnController in spawnControllers)
			{
				hazardSpawnController.SetType(randomHazardType);
			}
		}
	}

	// Token: 0x04002B92 RID: 11154
	private static Dictionary<HazardCategory, List<HazardType>> m_hazardTypeTable = new Dictionary<HazardCategory, List<HazardType>>();
}
