using System;

namespace RLAudio
{
	// Token: 0x02000E8B RID: 3723
	public class RLFMODSnapshotEventEmitter : BaseFMODEventEmitter
	{
		// Token: 0x0600690E RID: 26894 RVA: 0x0003A35A File Offset: 0x0003855A
		public override void Play()
		{
			if (this.m_eventInstance.isValid())
			{
				AudioManager.Play(this, this.m_eventInstance);
			}
		}
	}
}
