using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003F6 RID: 1014
[CreateAssetMenu(menuName = "Custom/Libraries/Item Drop Library")]
public class ItemDropLibrary : ScriptableObject
{
	// Token: 0x17000E64 RID: 3684
	// (get) Token: 0x0600209D RID: 8349 RVA: 0x00011428 File Offset: 0x0000F628
	public static List<ItemDropEntry> ItemDropEntryList
	{
		get
		{
			return ItemDropLibrary.Instance.m_itemDropEntryList;
		}
	}

	// Token: 0x17000E65 RID: 3685
	// (get) Token: 0x0600209E RID: 8350 RVA: 0x00011434 File Offset: 0x0000F634
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

	// Token: 0x04001D8A RID: 7562
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/ItemDropLibrary";

	// Token: 0x04001D8B RID: 7563
	[SerializeField]
	private List<ItemDropEntry> m_itemDropEntryList = new List<ItemDropEntry>();

	// Token: 0x04001D8C RID: 7564
	private static ItemDropLibrary m_instance;
}
