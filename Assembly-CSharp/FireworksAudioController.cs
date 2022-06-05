using System;
using RLAudio;
using UnityEngine;

// Token: 0x020001BC RID: 444
public class FireworksAudioController : MonoBehaviour
{
	// Token: 0x0600114D RID: 4429 RVA: 0x00032296 File Offset: 0x00030496
	private void Awake()
	{
		this.m_partSys = base.GetComponent<ParticleSystem>();
	}

	// Token: 0x0600114E RID: 4430 RVA: 0x000322A4 File Offset: 0x000304A4
	private void OnEnable()
	{
		this.m_particleCount = this.m_partSys.particleCount;
	}

	// Token: 0x0600114F RID: 4431 RVA: 0x000322B8 File Offset: 0x000304B8
	private void FixedUpdate()
	{
		if (this.m_particleCount != this.m_partSys.particleCount)
		{
			if (this.m_partSys.particleCount < this.m_particleCount)
			{
				AudioManager.Play(null, "event:/SFX/Interactables/sfx_env_prop_fireworks_explode", base.transform.position);
			}
			if (this.m_partSys.particleCount > this.m_particleCount)
			{
				AudioManager.Play(null, "event:/SFX/Interactables/sfx_env_prop_fireworks_launch", base.transform.position);
			}
			this.m_particleCount = this.m_partSys.particleCount;
		}
	}

	// Token: 0x0400123C RID: 4668
	private ParticleSystem m_partSys;

	// Token: 0x0400123D RID: 4669
	private int m_particleCount;
}
