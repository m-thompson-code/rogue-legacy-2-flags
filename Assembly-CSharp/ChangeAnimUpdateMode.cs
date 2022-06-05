using System;
using UnityEngine;

// Token: 0x02000348 RID: 840
public class ChangeAnimUpdateMode : MonoBehaviour
{
	// Token: 0x06001B1B RID: 6939 RVA: 0x00094270 File Offset: 0x00092470
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

	// Token: 0x06001B1C RID: 6940 RVA: 0x00094318 File Offset: 0x00092518
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

	// Token: 0x06001B1D RID: 6941 RVA: 0x00094384 File Offset: 0x00092584
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

	// Token: 0x04001934 RID: 6452
	[SerializeField]
	private ChangeAnimUpdateMode.UpdateModeType m_updateMode;

	// Token: 0x04001935 RID: 6453
	[SerializeField]
	private bool m_changeAllAnimators;

	// Token: 0x04001936 RID: 6454
	[SerializeField]
	private bool m_changeAllParticleSystems;

	// Token: 0x04001937 RID: 6455
	private Animator[] m_animators;

	// Token: 0x04001938 RID: 6456
	private ParticleSystem[] m_particleSystems;

	// Token: 0x04001939 RID: 6457
	private AnimatorUpdateMode[] m_storedAnimatorModes;

	// Token: 0x0400193A RID: 6458
	private bool[] m_storedPartSysModes;

	// Token: 0x02000349 RID: 841
	private enum UpdateModeType
	{
		// Token: 0x0400193C RID: 6460
		ScaledTime,
		// Token: 0x0400193D RID: 6461
		UnscaledTime
	}
}
