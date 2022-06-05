using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008E4 RID: 2276
	public class CastleBossEntranceAudioController : BossEntranceAudioController
	{
		// Token: 0x06004AC9 RID: 19145 RVA: 0x0010CC4F File Offset: 0x0010AE4F
		protected override void Awake()
		{
			base.Awake();
			(this.m_bossEntranceRoomController as CastleBossEntranceRoomController).DoorUnlockedRelay.AddListener(new Action(this.OnDoorUnlocked), false);
		}

		// Token: 0x06004ACA RID: 19146 RVA: 0x0010CC7C File Offset: 0x0010AE7C
		private void OnDoorUnlocked()
		{
			AudioManager.PlayOneShot(this, this.m_doorUnlockedAudioPath, default(Vector3));
		}

		// Token: 0x04003EBC RID: 16060
		[SerializeField]
		[EventRef]
		private string m_doorUnlockedAudioPath;
	}
}
