using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000239 RID: 569
[CreateAssetMenu(menuName = "Custom/Libraries/Item Drop Library")]
public class ItemDropLibrary : ScriptableObject
{
	// Token: 0x17000B37 RID: 2871
	// (get) Token: 0x060016EA RID: 5866 RVA: 0x00047AFB File Offset: 0x00045CFB
	public static List<ItemDropEntry> ItemDropEntryList
	{
		get
		{
			return ItemDropLibrary.Instance.m_itemDropEntryList;
		}
	}

	// Token: 0x17000B38 RID: 2872
	// (get) Token: 0x060016EB RID: 5867 RVA: 0x00047B07 File Offset: 0x00045D07
	public static ItemDropLibrary Instance
	{
		get
		{
			if (ItemDropLibrary.m_instance == null)
			{
				ItemDropLibrary.m_instance = CDGResources.Load<ItemDropLibrary>("Scriptable Objects/Libraries/ItemDropLibrary", "", true);
			}
			return ItemDropLibrary.m_instance;
		}
	}

	// Token: 0x04001672 RID: 5746
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/ItemDropLibrary";

	// Token: 0x04001673 RID: 5747
	[SerializeField]
	private List<ItemDropEntry> m_itemDropEntryList = new List<ItemDropEntry>();

	// Token: 0x04001674 RID: 5748
	private static ItemDropLibrary m_instance;
}
