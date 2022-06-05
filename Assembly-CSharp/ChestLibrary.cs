using System;
using UnityEngine;

// Token: 0x020003D6 RID: 982
[CreateAssetMenu(menuName = "Custom/Libraries/Chest Library")]
public class ChestLibrary : ScriptableObject
{
	// Token: 0x06002007 RID: 8199 RVA: 0x000A430C File Offset: 0x000A250C
	public static ChestObj GetChestPrefab(ChestType chestType)
	{
		ChestObj result = null;
		if (ChestLibrary.Instance.m_chestLibrary.TryGetValue(chestType, out result))
		{
			return result;
		}
		Debug.Log("<color=red>Could not find Chest type: " + chestType.ToString() + " in Chest Library.</color>");
		return null;
	}

	// Token: 0x17000E46 RID: 3654
	// (get) Token: 0x06002008 RID: 8200 RVA: 0x00010F52 File Offset: 0x0000F152
	public static ChestLibrary Instance
	{
		get
		{
			if (ChestLibrary.m_instance == null)
			{
				ChestLibrary.m_instance = CDGResources.Load<ChestLibrary>("Scriptable Objects/Libraries/ChestLibrary", "", true);
			}
			return ChestLibrary.m_instance;
		}
	}

	// Token: 0x04001CB2 RID: 7346
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/ChestLibrary";

	// Token: 0x04001CB3 RID: 7347
	[SerializeField]
	private ChestTypeChestObjDictionary m_chestLibrary;

	// Token: 0x04001CB4 RID: 7348
	private static ChestLibrary m_instance;
}
