using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000907 RID: 2311
	public class PlayerManaAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700186A RID: 6250
		// (get) Token: 0x06004BD0 RID: 19408 RVA: 0x001109EB File Offset: 0x0010EBEB
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

		// Token: 0x06004BD1 RID: 19409 RVA: 0x00110A11 File Offset: 0x0010EC11
		private IEnumerator Start()
		{
			while (!PlayerManager.IsInstantiated)
			{
				yield return null;
			}
			PlayerManager.GetPlayerController().ManaChangeRelay.AddListener(new Action<ManaChangeEventArgs>(this.OnManaChange), false);
			yield break;
		}

		// Token: 0x06004BD2 RID: 19410 RVA: 0x00110A20 File Offset: 0x0010EC20
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

		// Token: 0x06004BD3 RID: 19411 RVA: 0x00110A83 File Offset: 0x0010EC83
		private void OnDestroy()
		{
			if (!GameManager.IsApplicationClosing && PlayerManager.IsInstantiated && PlayerManager.GetPlayerController().ManaChangeRelay != null)
			{
				PlayerManager.GetPlayerController().ManaChangeRelay.RemoveListener(new Action<ManaChangeEventArgs>(this.OnManaChange));
			}
		}

		// Token: 0x04003FE0 RID: 16352
		[SerializeField]
		[EventRef]
		private string m_maxManaAudioEventPath;

		// Token: 0x04003FE1 RID: 16353
		private string m_description = string.Empty;

		// Token: 0x04003FE2 RID: 16354
		private bool m_hasMaxManaAudioPlayed = true;
	}
}
