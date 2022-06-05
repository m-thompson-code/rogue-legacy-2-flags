using System;
using System.Collections.Generic;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E6A RID: 3690
	[CreateAssetMenu(menuName = "Custom/Libraries/Item Drop Audio Library")]
	public class ItemDropAudioLibrary : AudioLibrary<ItemDropAudioLibraryEntry>
	{
		// Token: 0x17002145 RID: 8517
		// (get) Token: 0x06006820 RID: 26656 RVA: 0x00039962 File Offset: 0x00037B62
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

		// Token: 0x06006821 RID: 26657 RVA: 0x0003998B File Offset: 0x00037B8B
		public static ItemDropAudioLibraryEntry GetItemDropAudioLibraryEntry(ItemDropType itemDrop)
		{
			if (!ItemDropAudioLibrary.m_toStringTable.ContainsKey(itemDrop))
			{
				ItemDropAudioLibrary.m_toStringTable.Add(itemDrop, itemDrop.ToString());
			}
			return ItemDropAudioLibrary.Instance.GetAudioLibraryEntry(ItemDropAudioLibrary.m_toStringTable[itemDrop]);
		}

		// Token: 0x04005492 RID: 21650
		private static ItemDropAudioLibrary m_instance = null;

		// Token: 0x04005493 RID: 21651
		private static Dictionary<ItemDropType, string> m_toStringTable = new Dictionary<ItemDropType, string>();

		// Token: 0x04005494 RID: 21652
		public const string RESOURCES_PATH = "Scriptable Objects/Libraries/ItemDropAudioLibrary";

		// Token: 0x04005495 RID: 21653
		public const string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/ItemDropAudioLibrary.asset";
	}
}
