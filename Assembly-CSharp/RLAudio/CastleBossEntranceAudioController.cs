using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E54 RID: 3668
	public class CastleBossEntranceAudioController : BossEntranceAudioController
	{
		// Token: 0x06006782 RID: 26498 RVA: 0x00039128 File Offset: 0x00037328
		protected override void Awake()
		{
			base.Awake();
			(this.m_bossEntranceRoomController as CastleBossEntranceRoomController).DoorUnlockedRelay.AddListener(new Action(this.OnDoorUnlocked), false);
		}

		// Token: 0x06006783 RID: 26499 RVA: 0x0017D0DC File Offset: 0x0017B2DC
		private void OnDoorUnlocked()
		{
			AudioManager.PlayOneShot(this, this.m_doorUnlockedAudioPath, default(Vector3));
		}

		// Token: 0x040053DA RID: 21466
		[SerializeField]
		[EventRef]
		private string m_doorUnlockedAudioPath;
	}
}
