using System;

namespace RLAudio
{
	// Token: 0x020008F7 RID: 2295
	public class KineticBowProjectileAudioEventEmitter : ProjectileEventEmitter
	{
		// Token: 0x06004B5D RID: 19293 RVA: 0x0010F03B File Offset: 0x0010D23B
		public void SetPitch(float pitch)
		{
			this.m_lifeTimeEventInstance.setParameterByName("bardShootNote", pitch, false);
		}
	}
}
