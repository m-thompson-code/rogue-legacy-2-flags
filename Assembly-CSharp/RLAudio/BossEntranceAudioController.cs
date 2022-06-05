using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008E1 RID: 2273
	public class BossEntranceAudioController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001848 RID: 6216
		// (get) Token: 0x06004ABC RID: 19132 RVA: 0x0010CA65 File Offset: 0x0010AC65
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

		// Token: 0x06004ABD RID: 19133 RVA: 0x0010CA8C File Offset: 0x0010AC8C
		protected virtual void Awake()
		{
			this.m_bossEntranceRoomController = base.GetComponent<BossEntranceRoomController>();
			this.m_bossEntranceRoomController.DoorPartlyOpenedRelay.AddListener(new Action<bool>(this.OnDoorOpened), false);
			this.m_bossEntranceRoomController.DoorDestroyedRelay.AddListener(new Action<bool>(this.OnDoorDestroyed), false);
		}

		// Token: 0x06004ABE RID: 19134 RVA: 0x0010CAE4 File Offset: 0x0010ACE4
		protected virtual void OnDoorOpened(bool setOnEnter)
		{
			if (!setOnEnter)
			{
				AudioManager.PlayOneShot(this, this.m_doorOpenedAudioPath, default(Vector3));
			}
		}

		// Token: 0x06004ABF RID: 19135 RVA: 0x0010CB0C File Offset: 0x0010AD0C
		protected virtual void OnDoorDestroyed(bool setOnEnter)
		{
			if (!setOnEnter)
			{
				AudioManager.PlayOneShot(this, this.m_doorDestroyedAudioPath, default(Vector3));
			}
		}

		// Token: 0x04003EB3 RID: 16051
		[SerializeField]
		[EventRef]
		protected string m_doorOpenedAudioPath;

		// Token: 0x04003EB4 RID: 16052
		[SerializeField]
		[EventRef]
		protected string m_doorDestroyedAudioPath;

		// Token: 0x04003EB5 RID: 16053
		private string m_description = string.Empty;

		// Token: 0x04003EB6 RID: 16054
		protected BossEntranceRoomController m_bossEntranceRoomController;
	}
}
