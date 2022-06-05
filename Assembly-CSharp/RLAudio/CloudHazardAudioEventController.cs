using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E57 RID: 3671
	public class CloudHazardAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700212F RID: 8495
		// (get) Token: 0x06006791 RID: 26513 RVA: 0x00039221 File Offset: 0x00037421
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

		// Token: 0x06006792 RID: 26514 RVA: 0x0017D26C File Offset: 0x0017B46C
		private void Awake()
		{
			this.m_cloud = base.transform.GetComponent<Cloud>();
			if (this.m_cloud == null)
			{
				throw new MissingComponentException("Cloud");
			}
			this.m_cloud.PlayerHitRelay.AddListener(new Action<Vector2>(this.OnPlayerHit), false);
			this.m_cloud.TeleportStartRelay.AddListener(new Action(this.OnTeleportStart), false);
		}

		// Token: 0x06006793 RID: 26515 RVA: 0x00039247 File Offset: 0x00037447
		private void OnPlayerHit(Vector2 position)
		{
			this.m_playerHitPosition = position;
			AudioManager.PlayOneShot(this, this.m_playerHitAudioEventPath, position);
		}

		// Token: 0x06006794 RID: 26516 RVA: 0x00039262 File Offset: 0x00037462
		private void OnTeleportStart()
		{
			AudioManager.PlayOneShot(this, this.m_teleportStartAudioEventPath, this.m_playerHitPosition);
		}

		// Token: 0x040053E4 RID: 21476
		[SerializeField]
		[EventRef]
		private string m_playerHitAudioEventPath;

		// Token: 0x040053E5 RID: 21477
		[SerializeField]
		[EventRef]
		private string m_teleportStartAudioEventPath;

		// Token: 0x040053E6 RID: 21478
		private Cloud m_cloud;

		// Token: 0x040053E7 RID: 21479
		private string m_description = string.Empty;

		// Token: 0x040053E8 RID: 21480
		private Vector2 m_playerHitPosition;
	}
}
