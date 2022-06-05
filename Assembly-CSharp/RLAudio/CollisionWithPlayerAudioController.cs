using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008E8 RID: 2280
	public class CollisionWithPlayerAudioController : MonoBehaviour, IAudioEventEmitter, IWeaponOnEnterHitResponse, IHitResponse
	{
		// Token: 0x1700184D RID: 6221
		// (get) Token: 0x06004ADD RID: 19165 RVA: 0x0010CFBA File Offset: 0x0010B1BA
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

		// Token: 0x06004ADE RID: 19166 RVA: 0x0010CFE0 File Offset: 0x0010B1E0
		private void Awake()
		{
			this.m_defaultPath = this.m_collidedWithPlayerAudioPath;
		}

		// Token: 0x06004ADF RID: 19167 RVA: 0x0010CFF0 File Offset: 0x0010B1F0
		public void WeaponOnEnterHitResponse(IHitboxController otherHBController)
		{
			if (!otherHBController.RootGameObject.CompareTag("Player"))
			{
				return;
			}
			if (PlayerManager.GetPlayerController().IsInvincible)
			{
				return;
			}
			if (this.m_collidedWithPlayerAudioPath != string.Empty)
			{
				AudioManager.PlayOneShotAttached(this, this.m_collidedWithPlayerAudioPath, base.gameObject);
			}
		}

		// Token: 0x06004AE0 RID: 19168 RVA: 0x0010D041 File Offset: 0x0010B241
		public void SetCollisionWithPlayerAudioOverride(string path)
		{
			if (path != string.Empty)
			{
				this.m_collidedWithPlayerAudioPath = path;
				return;
			}
			this.m_collidedWithPlayerAudioPath = this.m_defaultPath;
		}

		// Token: 0x04003ECB RID: 16075
		[SerializeField]
		[EventRef]
		private string m_collidedWithPlayerAudioPath = string.Empty;

		// Token: 0x04003ECC RID: 16076
		private string m_defaultPath = string.Empty;

		// Token: 0x04003ECD RID: 16077
		private string m_description = string.Empty;
	}
}
