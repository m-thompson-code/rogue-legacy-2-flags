using System;
using FMOD.Studio;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E52 RID: 3666
	public class BubbleProjectileAudioController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700212C RID: 8492
		// (get) Token: 0x0600677A RID: 26490 RVA: 0x000390B3 File Offset: 0x000372B3
		public string Description
		{
			get
			{
				return "Bubble Projectile Audio Controller";
			}
		}

		// Token: 0x0600677B RID: 26491 RVA: 0x0017D054 File Offset: 0x0017B254
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

		// Token: 0x0600677C RID: 26492 RVA: 0x000390BA File Offset: 0x000372BA
		private void OnDisable()
		{
			AudioManager.Play(this, this.m_popEventInstance);
		}

		// Token: 0x0600677D RID: 26493 RVA: 0x000390C8 File Offset: 0x000372C8
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

		// Token: 0x040053D5 RID: 21461
		private EventInstance m_spawnEventInstance;

		// Token: 0x040053D6 RID: 21462
		private EventInstance m_popEventInstance;
	}
}
