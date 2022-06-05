using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E51 RID: 3665
	public class BossEntranceAudioController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700212B RID: 8491
		// (get) Token: 0x06006775 RID: 26485 RVA: 0x0003907A File Offset: 0x0003727A
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

		// Token: 0x06006776 RID: 26486 RVA: 0x0017CFAC File Offset: 0x0017B1AC
		protected virtual void Awake()
		{
			this.m_bossEntranceRoomController = base.GetComponent<BossEntranceRoomController>();
			this.m_bossEntranceRoomController.DoorPartlyOpenedRelay.AddListener(new Action<bool>(this.OnDoorOpened), false);
			this.m_bossEntranceRoomController.DoorDestroyedRelay.AddListener(new Action<bool>(this.OnDoorDestroyed), false);
		}

		// Token: 0x06006777 RID: 26487 RVA: 0x0017D004 File Offset: 0x0017B204
		protected virtual void OnDoorOpened(bool setOnEnter)
		{
			if (!setOnEnter)
			{
				AudioManager.PlayOneShot(this, this.m_doorOpenedAudioPath, default(Vector3));
			}
		}

		// Token: 0x06006778 RID: 26488 RVA: 0x0017D02C File Offset: 0x0017B22C
		protected virtual void OnDoorDestroyed(bool setOnEnter)
		{
			if (!setOnEnter)
			{
				AudioManager.PlayOneShot(this, this.m_doorDestroyedAudioPath, default(Vector3));
			}
		}

		// Token: 0x040053D1 RID: 21457
		[SerializeField]
		[EventRef]
		protected string m_doorOpenedAudioPath;

		// Token: 0x040053D2 RID: 21458
		[SerializeField]
		[EventRef]
		protected string m_doorDestroyedAudioPath;

		// Token: 0x040053D3 RID: 21459
		private string m_description = string.Empty;

		// Token: 0x040053D4 RID: 21460
		protected BossEntranceRoomController m_bossEntranceRoomController;
	}
}
