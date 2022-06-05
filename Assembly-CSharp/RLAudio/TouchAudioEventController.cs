using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E9B RID: 3739
	public class TouchAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17002189 RID: 8585
		// (get) Token: 0x06006965 RID: 26981 RVA: 0x0003A753 File Offset: 0x00038953
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

		// Token: 0x06006966 RID: 26982 RVA: 0x00182464 File Offset: 0x00180664
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

		// Token: 0x06006967 RID: 26983 RVA: 0x001824F8 File Offset: 0x001806F8
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

		// Token: 0x06006968 RID: 26984 RVA: 0x0003A779 File Offset: 0x00038979
		private void OnDestroy()
		{
			if (this.m_snapshotEventInstance.isValid())
			{
				this.m_snapshotEventInstance.release();
			}
		}

		// Token: 0x040055C8 RID: 21960
		[SerializeField]
		[EventRef]
		private string m_touchEnterAudioPath = string.Empty;

		// Token: 0x040055C9 RID: 21961
		[SerializeField]
		[EventRef]
		private string m_touchExitAudioPath = string.Empty;

		// Token: 0x040055CA RID: 21962
		[SerializeField]
		[EventRef]
		private string m_touchStaySnapshotPath = string.Empty;

		// Token: 0x040055CB RID: 21963
		private string m_description = string.Empty;

		// Token: 0x040055CC RID: 21964
		private EventInstance m_snapshotEventInstance;
	}
}
