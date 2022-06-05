using System;

namespace RLAudio
{
	// Token: 0x02000E6E RID: 3694
	public class KineticBowProjectileAudioEventEmitter : ProjectileEventEmitter
	{
		// Token: 0x06006834 RID: 26676 RVA: 0x00039A8C File Offset: 0x00037C8C
		public void SetPitch(float pitch)
		{
			this.m_lifeTimeEventInstance.setParameterByName("bardShootNote", pitch, false);
		}
	}
}
