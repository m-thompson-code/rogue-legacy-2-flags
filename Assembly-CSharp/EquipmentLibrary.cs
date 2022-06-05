using System;
using UnityEngine;

// Token: 0x020003E2 RID: 994
[CreateAssetMenu(menuName = "Custom/Libraries/Equipment Library")]
public class EquipmentLibrary : ScriptableObject
{
	// Token: 0x17000E56 RID: 3670
	// (get) Token: 0x06002043 RID: 8259 RVA: 0x000111F1 File Offset: 0x0000F3F1
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

	// Token: 0x06002044 RID: 8260 RVA: 0x000A49F0 File Offset: 0x000A2BF0
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

	// Token: 0x04001CE2 RID: 7394
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/EquipmentLibrary";

	// Token: 0x04001CE3 RID: 7395
	[Space(10f)]
	[SerializeField]
	private EquipmentTypeEquipmentDataDictionary m_weaponEquipmentLibrary;

	// Token: 0x04001CE4 RID: 7396
	[Space(10f)]
	[SerializeField]
	private EquipmentTypeEquipmentDataDictionary m_headEquipmentLibrary;

	// Token: 0x04001CE5 RID: 7397
	[Space(10f)]
	[SerializeField]
	private EquipmentTypeEquipmentDataDictionary m_chestEquipmentLibrary;

	// Token: 0x04001CE6 RID: 7398
	[Space(10f)]
	[SerializeField]
	private EquipmentTypeEquipmentDataDictionary m_capeEquipmentLibrary;

	// Token: 0x04001CE7 RID: 7399
	[Space(10f)]
	[SerializeField]
	private EquipmentTypeEquipmentDataDictionary m_trinketEquipmentLibrary;

	// Token: 0x04001CE8 RID: 7400
	private static EquipmentLibrary m_instance;
}
