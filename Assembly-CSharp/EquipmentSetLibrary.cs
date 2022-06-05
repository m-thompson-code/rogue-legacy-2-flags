using System;
using UnityEngine;

// Token: 0x0200022D RID: 557
[CreateAssetMenu(menuName = "Custom/Libraries/Equipment Set Library")]
public class EquipmentSetLibrary : ScriptableObject
{
	// Token: 0x17000B30 RID: 2864
	// (get) Token: 0x060016AF RID: 5807 RVA: 0x00046C25 File Offset: 0x00044E25
	private static EquipmentSetLibrary Instance
	{
		get
		{
			if (EquipmentSetLibrary.m_instance == null)
			{
				EquipmentSetLibrary.m_instance = CDGResources.Load<EquipmentSetLibrary>("Scriptable Objects/Libraries/EquipmentSetLibrary", "", true);
			}
			return EquipmentSetLibrary.m_instance;
		}
	}

	// Token: 0x060016B0 RID: 5808 RVA: 0x00046C50 File Offset: 0x00044E50
	public static EquipmentSetData GetEquipmentSetData(EquipmentType equipType)
	{
		EquipmentSetData result;
		if (EquipmentSetLibrary.Instance.m_equipmentSetLibrary.TryGetValue(equipType, out result))
		{
			return result;
		}
		Debug.LogWarningFormat("<color=red>{0}: ({1}) Could not find EquipmentSetData ({2}) in EquipmentSetData Library.</color>", new object[]
		{
			Time.frameCount,
			EquipmentSetLibrary.Instance,
			equipType
		});
		return null;
	}

	// Token: 0x060016B1 RID: 5809 RVA: 0x00046CA4 File Offset: 0x00044EA4
	public static string GetEquipmentSetBonusLocID(EquipmentSetBonusType bonusType)
	{
		string result;
		if (EquipmentSetLibrary.Instance.m_equipmentSetBonusLocIDLibrary.TryGetValue(bonusType, out result))
		{
			return result;
		}
		Debug.LogWarningFormat("<color=red>{0}: ({1}) Could not find EquipmentSetBonus LocID ({2}) in EquipmentSetData Library.</color>", new object[]
		{
			Time.frameCount,
			EquipmentSetLibrary.Instance,
			bonusType
		});
		return null;
	}

	// Token: 0x040015F3 RID: 5619
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/EquipmentSetLibrary";

	// Token: 0x040015F4 RID: 5620
	[SerializeField]
	private EquipmentTypeEquipmentSetDataDictionary m_equipmentSetLibrary;

	// Token: 0x040015F5 RID: 5621
	[SerializeField]
	private EquipmentSetBonusTypeStringDictionary m_equipmentSetBonusLocIDLibrary;

	// Token: 0x040015F6 RID: 5622
	private static EquipmentSetLibrary m_instance;
}
