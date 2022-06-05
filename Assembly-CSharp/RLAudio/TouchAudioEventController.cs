using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x0200091D RID: 2333
	public class TouchAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700188E RID: 6286
		// (get) Token: 0x06004C64 RID: 19556 RVA: 0x00112711 File Offset: 0x00110911
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

		// Token: 0x06004C65 RID: 19557 RVA: 0x00112738 File Offset: 0x00110938
		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (CollisionType_RL.IsProjectile(collision.gameObject))
			{
				return;
			}
			if (collision.GetRoot(false).CompareTag("Player"))
			{
				Vector2 v = PlayerManager.GetPlayerController().transform.position;
				AudioManager.PlayOneShot(this, this.m_touchEnterAudioPath, v);
				if (!string.IsNullOrEmpty(this.m_touchStaySnapshotPath))
				{
					if (!this.m_snapshotEventInstance.isValid())
					{
						this.m_snapshotEventInstance = AudioUtility.GetEventInstance(this.m_touchStaySnapshotPath, base.transform);
					}
					AmbientSoundController.SetSnapshotOverride(this.m_snapshotEventInstance);
				}
			}
		}

		// Token: 0x06004C66 RID: 19558 RVA: 0x001127CC File Offset: 0x001109CC
		private void OnTriggerExit2D(Collider2D collision)
		{
			if (CollisionType_RL.IsProjectile(collision.gameObject))
			{
				return;
			}
			if (collision.GetRoot(false).CompareTag("Player"))
			{
				Vector2 v = PlayerManager.GetPlayerController().transform.position;
				AudioManager.PlayOneShot(this, this.m_touchExitAudioPath, v);
				if (!string.IsNullOrEmpty(this.m_touchStaySnapshotPath))
				{
					AmbientSoundController.ClearSnapshotOverride();
				}
			}
		}

		// Token: 0x06004C67 RID: 19559 RVA: 0x00112833 File Offset: 0x00110A33
		private void OnDestroy()
		{
			if (this.m_snapshotEventInstance.isValid())
			{
				this.m_snapshotEventInstance.release();
			}
		}

		// Token: 0x04004060 RID: 16480
		[SerializeField]
		[EventRef]
		private string m_touchEnterAudioPath = string.Empty;

		// Token: 0x04004061 RID: 16481
		[SerializeField]
		[EventRef]
		private string m_touchExitAudioPath = string.Empty;

		// Token: 0x04004062 RID: 16482
		[SerializeField]
		[EventRef]
		private string m_touchStaySnapshotPath = string.Empty;

		// Token: 0x04004063 RID: 16483
		private string m_description = string.Empty;

		// Token: 0x04004064 RID: 16484
		private EventInstance m_snapshotEventInstance;
	}
}
