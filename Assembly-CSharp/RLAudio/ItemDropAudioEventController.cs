using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008F2 RID: 2290
	public class ItemDropAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001857 RID: 6231
		// (get) Token: 0x06004B42 RID: 19266 RVA: 0x0010EBCD File Offset: 0x0010CDCD
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

		// Token: 0x06004B43 RID: 19267 RVA: 0x0010EBF0 File Offset: 0x0010CDF0
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

		// Token: 0x06004B44 RID: 19268 RVA: 0x0010EC70 File Offset: 0x0010CE70
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

		// Token: 0x06004B45 RID: 19269 RVA: 0x0010ECD4 File Offset: 0x0010CED4
		private void OnItemLanded(CorgiController_RL controller)
		{
			if (this.m_itemDrop != null)
			{
				string landEventPath = ItemDropAudioLibrary.GetItemDropAudioLibraryEntry(this.m_itemDrop.ItemDropType).LandEventPath;
				this.PlayAudio(landEventPath);
			}
		}

		// Token: 0x06004B46 RID: 19270 RVA: 0x0010ED0C File Offset: 0x0010CF0C
		private void OnItemCollected()
		{
			if (this.m_itemDrop != null)
			{
				string collectEventPath = ItemDropAudioLibrary.GetItemDropAudioLibraryEntry(this.m_itemDrop.ItemDropType).CollectEventPath;
				this.PlayAudio(collectEventPath);
			}
		}

		// Token: 0x06004B47 RID: 19271 RVA: 0x0010ED44 File Offset: 0x0010CF44
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

		// Token: 0x04003F4B RID: 16203
		private BaseItemDrop m_itemDrop;

		// Token: 0x04003F4C RID: 16204
		private CorgiController_RL m_corgiController;

		// Token: 0x04003F4D RID: 16205
		private string m_description = string.Empty;
	}
}
