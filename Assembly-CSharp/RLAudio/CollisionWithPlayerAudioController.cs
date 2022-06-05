using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E58 RID: 3672
	public class CollisionWithPlayerAudioController : MonoBehaviour, IAudioEventEmitter, IWeaponOnEnterHitResponse, IHitResponse
	{
		// Token: 0x17002130 RID: 8496
		// (get) Token: 0x06006796 RID: 26518 RVA: 0x0003928E File Offset: 0x0003748E
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

		// Token: 0x06006797 RID: 26519 RVA: 0x000392B4 File Offset: 0x000374B4
		private void Awake()
		{
			this.m_defaultPath = this.m_collidedWithPlayerAudioPath;
		}

		// Token: 0x06006798 RID: 26520 RVA: 0x0017D2E0 File Offset: 0x0017B4E0
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

		// Token: 0x06006799 RID: 26521 RVA: 0x000392C2 File Offset: 0x000374C2
		public void SetCollisionWithPlayerAudioOverride(string path)
		{
			if (path != string.Empty)
			{
				this.m_collidedWithPlayerAudioPath = path;
				return;
			}
			this.m_collidedWithPlayerAudioPath = this.m_defaultPath;
		}

		// Token: 0x040053E9 RID: 21481
		[SerializeField]
		[EventRef]
		private string m_collidedWithPlayerAudioPath = string.Empty;

		// Token: 0x040053EA RID: 21482
		private string m_defaultPath = string.Empty;

		// Token: 0x040053EB RID: 21483
		private string m_description = string.Empty;
	}
}
