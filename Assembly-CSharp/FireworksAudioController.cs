using System;
using RLAudio;
using UnityEngine;

// Token: 0x0200032B RID: 811
public class FireworksAudioController : MonoBehaviour
{
	// Token: 0x06001996 RID: 6550 RVA: 0x0000CED4 File Offset: 0x0000B0D4
	private void Awake()
	{
		this.m_partSys = base.GetComponent<ParticleSystem>();
	}

	// Token: 0x06001997 RID: 6551 RVA: 0x0000CEE2 File Offset: 0x0000B0E2
	private void OnEnable()
	{
		this.m_particleCount = this.m_partSys.particleCount;
	}

	// Token: 0x06001998 RID: 6552 RVA: 0x00090554 File Offset: 0x0008E754
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

	// Token: 0x04001845 RID: 6213
	private ParticleSystem m_partSys;

	// Token: 0x04001846 RID: 6214
	private int m_particleCount;
}
