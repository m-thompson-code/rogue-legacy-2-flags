using System;
using UnityEngine;

// Token: 0x020001CE RID: 462
public class ChangeAnimUpdateMode : MonoBehaviour
{
	// Token: 0x0600129D RID: 4765 RVA: 0x00036BD0 File Offset: 0x00034DD0
	private void Awake()
	{
		this.m_animators = base.GetComponentsInChildren<Animator>(true);
		this.m_particleSystems = base.GetComponentsInChildren<ParticleSystem>(true);
		this.m_storedAnimatorModes = new AnimatorUpdateMode[this.m_animators.Length];
		this.m_storedPartSysModes = new bool[this.m_particleSystems.Length];
		for (int i = 0; i < this.m_animators.Length; i++)
		{
			this.m_storedAnimatorModes[i] = this.m_animators[i].updateMode;
		}
		for (int j = 0; j < this.m_particleSystems.Length; j++)
		{
			this.m_storedPartSysModes[j] = this.m_particleSystems[j].main.useUnscaledTime;
		}
	}

	// Token: 0x0600129E RID: 4766 RVA: 0x00036C78 File Offset: 0x00034E78
	private void OnEnable()
	{
		AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal;
		bool useUnscaledTime = this.m_updateMode == ChangeAnimUpdateMode.UpdateModeType.UnscaledTime;
		if (this.m_updateMode == ChangeAnimUpdateMode.UpdateModeType.UnscaledTime)
		{
			updateMode = AnimatorUpdateMode.UnscaledTime;
		}
		Animator[] animators = this.m_animators;
		for (int i = 0; i < animators.Length; i++)
		{
			animators[i].updateMode = updateMode;
		}
		ParticleSystem[] particleSystems = this.m_particleSystems;
		for (int i = 0; i < particleSystems.Length; i++)
		{
			particleSystems[i].main.useUnscaledTime = useUnscaledTime;
		}
	}

	// Token: 0x0600129F RID: 4767 RVA: 0x00036CE4 File Offset: 0x00034EE4
	private void OnDisable()
	{
		for (int i = 0; i < this.m_animators.Length; i++)
		{
			this.m_animators[i].updateMode = this.m_storedAnimatorModes[i];
		}
		for (int j = 0; j < this.m_particleSystems.Length; j++)
		{
			this.m_particleSystems[j].main.useUnscaledTime = this.m_storedPartSysModes[j];
		}
	}

	// Token: 0x04001301 RID: 4865
	[SerializeField]
	private ChangeAnimUpdateMode.UpdateModeType m_updateMode;

	// Token: 0x04001302 RID: 4866
	[SerializeField]
	private bool m_changeAllAnimators;

	// Token: 0x04001303 RID: 4867
	[SerializeField]
	private bool m_changeAllParticleSystems;

	// Token: 0x04001304 RID: 4868
	private Animator[] m_animators;

	// Token: 0x04001305 RID: 4869
	private ParticleSystem[] m_particleSystems;

	// Token: 0x04001306 RID: 4870
	private AnimatorUpdateMode[] m_storedAnimatorModes;

	// Token: 0x04001307 RID: 4871
	private bool[] m_storedPartSysModes;

	// Token: 0x02000AF3 RID: 2803
	private enum UpdateModeType
	{
		// Token: 0x04004AC9 RID: 19145
		ScaledTime,
		// Token: 0x04004ACA RID: 19146
		UnscaledTime
	}
}
