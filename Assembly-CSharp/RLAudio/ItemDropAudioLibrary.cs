using System;
using System.Collections.Generic;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008F3 RID: 2291
	[CreateAssetMenu(menuName = "Custom/Libraries/Item Drop Audio Library")]
	public class ItemDropAudioLibrary : AudioLibrary<ItemDropAudioLibraryEntry>
	{
		// Token: 0x17001858 RID: 6232
		// (get) Token: 0x06004B49 RID: 19273 RVA: 0x0010EDB7 File Offset: 0x0010CFB7
		private static ItemDropAudioLibrary Instance
		{
			get
			{
				if (ItemDropAudioLibrary.m_instance == null)
				{
					ItemDropAudioLibrary.m_instance = CDGResources.Load<ItemDropAudioLibrary>("Scriptable Objects/Libraries/ItemDropAudioLibrary", "", true);
				}
				return ItemDropAudioLibrary.m_instance;
			}
		}

		// Token: 0x06004B4A RID: 19274 RVA: 0x0010EDE0 File Offset: 0x0010CFE0
		public static ItemDropAudioLibraryEntry GetItemDropAudioLibraryEntry(ItemDropType itemDrop)
		{
			if (!ItemDropAudioLibrary.m_toStringTable.ContainsKey(itemDrop))
			{
				ItemDropAudioLibrary.m_toStringTable.Add(itemDrop, itemDrop.ToString());
			}
			return ItemDropAudioLibrary.Instance.GetAudioLibraryEntry(ItemDropAudioLibrary.m_toStringTable[itemDrop]);
		}

		// Token: 0x04003F4E RID: 16206
		private static ItemDropAudioLibrary m_instance = null;

		// Token: 0x04003F4F RID: 16207
		private static Dictionary<ItemDropType, string> m_toStringTable = new Dictionary<ItemDropType, string>();

		// Token: 0x04003F50 RID: 16208
		public const string RESOURCES_PATH = "Scriptable Objects/Libraries/ItemDropAudioLibrary";

		// Token: 0x04003F51 RID: 16209
		public const string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/ItemDropAudioLibrary.asset";
	}
}
