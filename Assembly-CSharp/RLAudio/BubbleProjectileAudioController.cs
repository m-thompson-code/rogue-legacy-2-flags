using System;
using FMOD.Studio;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008E2 RID: 2274
	public class BubbleProjectileAudioController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001849 RID: 6217
		// (get) Token: 0x06004AC1 RID: 19137 RVA: 0x0010CB44 File Offset: 0x0010AD44
		public string Description
		{
			get
			{
				return "Bubble Projectile Audio Controller";
			}
		}

		// Token: 0x06004AC2 RID: 19138 RVA: 0x0010CB4C File Offset: 0x0010AD4C
		public void PlaySpawnAudio(float pitch)
		{
			if (!this.m_spawnEventInstance.isValid())
			{
				this.m_spawnEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_enemy_blobfish_bubble_spawn", base.transform);
			}
			if (!this.m_popEventInstance.isValid())
			{
				this.m_popEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_enemy_blobfish_bubble_pop", base.transform);
			}
			this.m_spawnEventInstance.setParameterByName("enemyBlobfishBubbleSpawn", pitch, false);
			this.m_popEventInstance.setParameterByName("enemyBlobfishBubbleSpawn", pitch, false);
			AudioManager.Play(this, this.m_spawnEventInstance);
		}

		// Token: 0x06004AC3 RID: 19139 RVA: 0x0010CBD1 File Offset: 0x0010ADD1
		private void OnDisable()
		{
			AudioManager.Play(this, this.m_popEventInstance);
		}

		// Token: 0x06004AC4 RID: 19140 RVA: 0x0010CBDF File Offset: 0x0010ADDF
		private void OnDestroy()
		{
			if (this.m_spawnEventInstance.isValid())
			{
				this.m_spawnEventInstance.release();
			}
			if (this.m_popEventInstance.isValid())
			{
				this.m_popEventInstance.release();
			}
		}

		// Token: 0x04003EB7 RID: 16055
		private EventInstance m_spawnEventInstance;

		// Token: 0x04003EB8 RID: 16056
		private EventInstance m_popEventInstance;
	}
}
