using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008F6 RID: 2294
	public class ItemDropManagerAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700185F RID: 6239
		// (get) Token: 0x06004B58 RID: 19288 RVA: 0x0010EF1F File Offset: 0x0010D11F
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

		// Token: 0x06004B59 RID: 19289 RVA: 0x0010EF40 File Offset: 0x0010D140
		private void Awake()
		{
			this.m_dropManager = base.GetComponent<ItemDropManager>();
			if (this.m_dropManager != null)
			{
				this.m_dropManager.DropItemRelay.AddListener(new Action<ItemDropType, Vector2>(this.OnItemDropped), false);
				this.m_dropManager.DropGoldRelay.AddListener(new Action<ItemDropType, Vector2, int>(this.OnMultipleGoldDropped), false);
			}
		}

		// Token: 0x06004B5A RID: 19290 RVA: 0x0010EFA4 File Offset: 0x0010D1A4
		private void OnItemDropped(ItemDropType itemDrop, Vector2 position)
		{
			string spawnSingleEventPath = ItemDropAudioLibrary.GetItemDropAudioLibraryEntry(itemDrop).SpawnSingleEventPath;
			if (!string.IsNullOrEmpty(spawnSingleEventPath))
			{
				AudioManager.PlayOneShot(this, spawnSingleEventPath, position);
			}
		}

		// Token: 0x06004B5B RID: 19291 RVA: 0x0010EFD4 File Offset: 0x0010D1D4
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

		// Token: 0x04003F5E RID: 16222
		[SerializeField]
		private int m_minMultipleGoldCount = 3;

		// Token: 0x04003F5F RID: 16223
		private ItemDropManager m_dropManager;

		// Token: 0x04003F60 RID: 16224
		private string m_description = string.Empty;
	}
}
