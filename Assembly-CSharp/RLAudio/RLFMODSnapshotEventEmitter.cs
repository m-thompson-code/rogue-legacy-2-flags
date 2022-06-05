using System;

namespace RLAudio
{
	// Token: 0x0200090E RID: 2318
	public class RLFMODSnapshotEventEmitter : BaseFMODEventEmitter
	{
		// Token: 0x06004C13 RID: 19475 RVA: 0x0011152F File Offset: 0x0010F72F
		public override void Play()
		{
			if (this.m_eventInstance.isValid())
			{
				AudioManager.Play(this, this.m_eventInstance);
			}
		}
	}
}
