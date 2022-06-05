using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E83 RID: 3715
	public class PlayerManaAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17002161 RID: 8545
		// (get) Token: 0x060068C5 RID: 26821 RVA: 0x0003A037 File Offset: 0x00038237
		public string Description
		{
			get
			{
				if (this.m_description == string.Empty)
				{
					this.m_description = this.ToString();
				}
				return this.m_description;
			}
		}

		// Token: 0x060068C6 RID: 26822 RVA: 0x0003A05D File Offset: 0x0003825D
		private IEnumerator Start()
		{
			while (!PlayerManager.IsInstantiated)
			{
				yield return null;
			}
			PlayerManager.GetPlayerController().ManaChangeRelay.AddListener(new Action<ManaChangeEventArgs>(this.OnManaChange), false);
			yield break;
		}

		// Token: 0x060068C7 RID: 26823 RVA: 0x00180CB0 File Offset: 0x0017EEB0
		private void OnManaChange(ManaChangeEventArgs manaChangeEventArgs)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			if (playerController.CurrentManaAsInt < playerController.ActualMaxMana && this.m_hasMaxManaAudioPlayed)
			{
				this.m_hasMaxManaAudioPlayed = false;
				return;
			}
			if (playerController.CurrentManaAsInt >= playerController.ActualMaxMana && !this.m_hasMaxManaAudioPlayed)
			{
				AudioManager.PlayOneShot(this, this.m_maxManaAudioEventPath, default(Vector3));
				this.m_hasMaxManaAudioPlayed = true;
			}
		}

		// Token: 0x060068C8 RID: 26824 RVA: 0x0003A06C File Offset: 0x0003826C
		private void OnDestroy()
		{
			if (!GameManager.IsApplicationClosing && PlayerManager.IsInstantiated && PlayerManager.GetPlayerController().ManaChangeRelay != null)
			{
				PlayerManager.GetPlayerController().ManaChangeRelay.RemoveListener(new Action<ManaChangeEventArgs>(this.OnManaChange));
			}
		}

		// Token: 0x0400553D RID: 21821
		[SerializeField]
		[EventRef]
		private string m_maxManaAudioEventPath;

		// Token: 0x0400553E RID: 21822
		private string m_description = string.Empty;

		// Token: 0x0400553F RID: 21823
		private bool m_hasMaxManaAudioPlayed = true;
	}
}
