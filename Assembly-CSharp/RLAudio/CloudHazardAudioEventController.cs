using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008E7 RID: 2279
	public class CloudHazardAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700184C RID: 6220
		// (get) Token: 0x06004AD8 RID: 19160 RVA: 0x0010CEDA File Offset: 0x0010B0DA
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

		// Token: 0x06004AD9 RID: 19161 RVA: 0x0010CF00 File Offset: 0x0010B100
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

		// Token: 0x06004ADA RID: 19162 RVA: 0x0010CF73 File Offset: 0x0010B173
		private void OnPlayerHit(Vector2 position)
		{
			this.m_playerHitPosition = position;
			AudioManager.PlayOneShot(this, this.m_playerHitAudioEventPath, position);
		}

		// Token: 0x06004ADB RID: 19163 RVA: 0x0010CF8E File Offset: 0x0010B18E
		private void OnTeleportStart()
		{
			AudioManager.PlayOneShot(this, this.m_teleportStartAudioEventPath, this.m_playerHitPosition);
		}

		// Token: 0x04003EC6 RID: 16070
		[SerializeField]
		[EventRef]
		private string m_playerHitAudioEventPath;

		// Token: 0x04003EC7 RID: 16071
		[SerializeField]
		[EventRef]
		private string m_teleportStartAudioEventPath;

		// Token: 0x04003EC8 RID: 16072
		private Cloud m_cloud;

		// Token: 0x04003EC9 RID: 16073
		private string m_description = string.Empty;

		// Token: 0x04003ECA RID: 16074
		private Vector2 m_playerHitPosition;
	}
}
