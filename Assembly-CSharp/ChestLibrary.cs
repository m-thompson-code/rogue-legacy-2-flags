using System;
using UnityEngine;

// Token: 0x02000221 RID: 545
[CreateAssetMenu(menuName = "Custom/Libraries/Chest Library")]
public class ChestLibrary : ScriptableObject
{
	// Token: 0x0600166A RID: 5738 RVA: 0x00045EE0 File Offset: 0x000440E0
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

	// Token: 0x17000B1F RID: 2847
	// (get) Token: 0x0600166B RID: 5739 RVA: 0x00045F27 File Offset: 0x00044127
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

	// Token: 0x040015A6 RID: 5542
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/ChestLibrary";

	// Token: 0x040015A7 RID: 5543
	[SerializeField]
	private ChestTypeChestObjDictionary m_chestLibrary;

	// Token: 0x040015A8 RID: 5544
	private static ChestLibrary m_instance;
}
