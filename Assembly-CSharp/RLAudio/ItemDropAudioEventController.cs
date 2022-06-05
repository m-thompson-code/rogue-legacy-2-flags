using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E69 RID: 3689
	public class ItemDropAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17002144 RID: 8516
		// (get) Token: 0x06006819 RID: 26649 RVA: 0x0003992E File Offset: 0x00037B2E
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

		// Token: 0x0600681A RID: 26650 RVA: 0x0017EE34 File Offset: 0x0017D034
		private void Awake()
		{
			this.m_itemDrop = base.GetComponent<BaseItemDrop>();
			if (this.m_itemDrop != null)
			{
				this.m_itemDrop.CollectedRelay.AddListener(new Action(this.OnItemCollected), false);
			}
			this.m_corgiController = base.GetComponent<CorgiController_RL>();
			if (this.m_corgiController != null)
			{
				this.m_corgiController.OnCorgiLandedEnterRelay.AddListener(new Action<CorgiController_RL>(this.OnItemLanded), false);
			}
		}

		// Token: 0x0600681B RID: 26651 RVA: 0x0017EEB4 File Offset: 0x0017D0B4
		private void OnDestroy()
		{
			if (this.m_itemDrop != null)
			{
				this.m_itemDrop.CollectedRelay.RemoveListener(new Action(this.OnItemCollected));
			}
			if (this.m_corgiController != null)
			{
				this.m_corgiController.OnCorgiLandedEnterRelay.RemoveListener(new Action<CorgiController_RL>(this.OnItemLanded));
			}
		}

		// Token: 0x0600681C RID: 26652 RVA: 0x0017EF18 File Offset: 0x0017D118
		private void OnItemLanded(CorgiController_RL controller)
		{
			if (this.m_itemDrop != null)
			{
				string landEventPath = ItemDropAudioLibrary.GetItemDropAudioLibraryEntry(this.m_itemDrop.ItemDropType).LandEventPath;
				this.PlayAudio(landEventPath);
			}
		}

		// Token: 0x0600681D RID: 26653 RVA: 0x0017EF50 File Offset: 0x0017D150
		private void OnItemCollected()
		{
			if (this.m_itemDrop != null)
			{
				string collectEventPath = ItemDropAudioLibrary.GetItemDropAudioLibraryEntry(this.m_itemDrop.ItemDropType).CollectEventPath;
				this.PlayAudio(collectEventPath);
			}
		}

		// Token: 0x0600681E RID: 26654 RVA: 0x0017EF88 File Offset: 0x0017D188
		private void PlayAudio(string audioPath)
		{
			try
			{
				AudioManager.PlayOneShot(this, audioPath, base.transform.position);
			}
			catch (EventNotFoundException)
			{
				Debug.LogFormat("<color=red>| {0} | The FMOD Audio Event path <b>({1})</b> is not valid. Check the ItemDropAudioLibrary's <b>({2})</b> entry and update the path if required.</color>", new object[]
				{
					this,
					audioPath,
					this.m_itemDrop.ItemDropType
				});
			}
		}

		// Token: 0x0400548F RID: 21647
		private BaseItemDrop m_itemDrop;

		// Token: 0x04005490 RID: 21648
		private CorgiController_RL m_corgiController;

		// Token: 0x04005491 RID: 21649
		private string m_description = string.Empty;
	}
}
