using System;
using UnityEngine;

// Token: 0x0200022B RID: 555
[CreateAssetMenu(menuName = "Custom/Libraries/Equipment Library")]
public class EquipmentLibrary : ScriptableObject
{
	// Token: 0x17000B2D RID: 2861
	// (get) Token: 0x0600169E RID: 5790 RVA: 0x00046805 File Offset: 0x00044A05
	private static EquipmentLibrary Instance
	{
		get
		{
			if (EquipmentLibrary.m_instance == null)
			{
				EquipmentLibrary.m_instance = CDGResources.Load<EquipmentLibrary>("Scriptable Objects/Libraries/EquipmentLibrary", "", true);
			}
			return EquipmentLibrary.m_instance;
		}
	}

	// Token: 0x0600169F RID: 5791 RVA: 0x00046830 File Offset: 0x00044A30
	public static EquipmentData GetEquipmentData(EquipmentCategoryType categoryType, EquipmentType equipType)
	{
		if (equipType == EquipmentType.None || categoryType == EquipmentCategoryType.None)
		{
			return null;
		}
		EquipmentTypeEquipmentDataDictionary equipmentTypeEquipmentDataDictionary = null;
		switch (categoryType)
		{
		case EquipmentCategoryType.Weapon:
			equipmentTypeEquipmentDataDictionary = EquipmentLibrary.Instance.m_weaponEquipmentLibrary;
			break;
		case EquipmentCategoryType.Head:
			equipmentTypeEquipmentDataDictionary = EquipmentLibrary.Instance.m_headEquipmentLibrary;
			break;
		case EquipmentCategoryType.Chest:
			equipmentTypeEquipmentDataDictionary = EquipmentLibrary.Instance.m_chestEquipmentLibrary;
			break;
		case EquipmentCategoryType.Cape:
			equipmentTypeEquipmentDataDictionary = EquipmentLibrary.Instance.m_capeEquipmentLibrary;
			break;
		case EquipmentCategoryType.Trinket:
			equipmentTypeEquipmentDataDictionary = EquipmentLibrary.Instance.m_trinketEquipmentLibrary;
			break;
		}
		EquipmentData result = null;
		if (equipmentTypeEquipmentDataDictionary != null)
		{
			equipmentTypeEquipmentDataDictionary.TryGetValue(equipType, out result);
			return result;
		}
		throw new Exception("Equipment Library is null.");
	}

	// Token: 0x040015D2 RID: 5586
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/EquipmentLibrary";

	// Token: 0x040015D3 RID: 5587
	[Space(10f)]
	[SerializeField]
	private EquipmentTypeEquipmentDataDictionary m_weaponEquipmentLibrary;

	// Token: 0x040015D4 RID: 5588
	[Space(10f)]
	[SerializeField]
	private EquipmentTypeEquipmentDataDictionary m_headEquipmentLibrary;

	// Token: 0x040015D5 RID: 5589
	[Space(10f)]
	[SerializeField]
	private EquipmentTypeEquipmentDataDictionary m_chestEquipmentLibrary;

	// Token: 0x040015D6 RID: 5590
	[Space(10f)]
	[SerializeField]
	private EquipmentTypeEquipmentDataDictionary m_capeEquipmentLibrary;

	// Token: 0x040015D7 RID: 5591
	[Space(10f)]
	[SerializeField]
	private EquipmentTypeEquipmentDataDictionary m_trinketEquipmentLibrary;

	// Token: 0x040015D8 RID: 5592
	private static EquipmentLibrary m_instance;
}
