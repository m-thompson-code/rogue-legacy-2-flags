using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E6D RID: 3693
	public class ItemDropManagerAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700214C RID: 8524
		// (get) Token: 0x0600682F RID: 26671 RVA: 0x00039A51 File Offset: 0x00037C51
		public string Description
		{
			get
			{
				if (string.IsNullOrEmpty(this.m_description))
				{
					this.m_description = this.ToString();
				}
				return this.m_description;
			}
		}

		// Token: 0x06006830 RID: 26672 RVA: 0x0017F050 File Offset: 0x0017D250
		private void Awake()
		{
			this.m_dropManager = base.GetComponent<ItemDropManager>();
			if (this.m_dropManager != null)
			{
				this.m_dropManager.DropItemRelay.AddListener(new Action<ItemDropType, Vector2>(this.OnItemDropped), false);
				this.m_dropManager.DropGoldRelay.AddListener(new Action<ItemDropType, Vector2, int>(this.OnMultipleGoldDropped), false);
			}
		}

		// Token: 0x06006831 RID: 26673 RVA: 0x0017F0B4 File Offset: 0x0017D2B4
		private void OnItemDropped(ItemDropType itemDrop, Vector2 position)
		{
			string spawnSingleEventPath = ItemDropAudioLibrary.GetItemDropAudioLibraryEntry(itemDrop).SpawnSingleEventPath;
			if (!string.IsNullOrEmpty(spawnSingleEventPath))
			{
				AudioManager.PlayOneShot(this, spawnSingleEventPath, position);
			}
		}

		// Token: 0x06006832 RID: 26674 RVA: 0x0017F0E4 File Offset: 0x0017D2E4
		private void OnMultipleGoldDropped(ItemDropType itemDrop, Vector2 position, int count)
		{
			ItemDropAudioLibraryEntry itemDropAudioLibraryEntry = ItemDropAudioLibrary.GetItemDropAudioLibraryEntry(itemDrop);
			string text = itemDropAudioLibraryEntry.SpawnSingleEventPath;
			if (count >= this.m_minMultipleGoldCount && itemDropAudioLibraryEntry.UseManyAudio)
			{
				text = itemDropAudioLibraryEntry.SpawnManyEventPath;
			}
			if (!string.IsNullOrEmpty(text))
			{
				AudioManager.PlayOneShot(this, text, position);
			}
		}

		// Token: 0x040054A2 RID: 21666
		[SerializeField]
		private int m_minMultipleGoldCount = 3;

		// Token: 0x040054A3 RID: 21667
		private ItemDropManager m_dropManager;

		// Token: 0x040054A4 RID: 21668
		private string m_description = string.Empty;
	}
}
