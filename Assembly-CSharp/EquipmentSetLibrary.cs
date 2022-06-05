using System;
using UnityEngine;

// Token: 0x020003E8 RID: 1000
[CreateAssetMenu(menuName = "Custom/Libraries/Equipment Set Library")]
public class EquipmentSetLibrary : ScriptableObject
{
	// Token: 0x17000E59 RID: 3673
	// (get) Token: 0x0600205C RID: 8284 RVA: 0x000112C2 File Offset: 0x0000F4C2
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

	// Token: 0x0600205D RID: 8285 RVA: 0x000A4D6C File Offset: 0x000A2F6C
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

	// Token: 0x0600205E RID: 8286 RVA: 0x000A4DC0 File Offset: 0x000A2FC0
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

	// Token: 0x04001D07 RID: 7431
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/EquipmentSetLibrary";

	// Token: 0x04001D08 RID: 7432
	[SerializeField]
	private EquipmentTypeEquipmentSetDataDictionary m_equipmentSetLibrary;

	// Token: 0x04001D09 RID: 7433
	[SerializeField]
	private EquipmentSetBonusTypeStringDictionary m_equipmentSetBonusLocIDLibrary;

	// Token: 0x04001D0A RID: 7434
	private static EquipmentSetLibrary m_instance;
}
