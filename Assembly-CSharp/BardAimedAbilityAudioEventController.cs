using System;
using FMOD.Studio;
using RLAudio;
using UnityEngine;

// Token: 0x020001BA RID: 442
public class BardAimedAbilityAudioEventController : AimedAbilityAudioEventController
{
	// Token: 0x06001147 RID: 4423 RVA: 0x0003217F File Offset: 0x0003037F
	private void Awake()
	{
		this.m_ability = base.GetComponent<AimedAbility_RL>();
	}

	// Token: 0x06001148 RID: 4424 RVA: 0x00032190 File Offset: 0x00030390
	protected override void OnAbilityFired(Projectile_RL projectile)
	{
		this.m_isAbilityStarted = false;
		int num = Mathf.RoundToInt((this.m_ability.AimAngle + 90f) / 22.5f);
		if (this.m_abilityFiredEventInstance.isValid())
		{
			this.m_abilityFiredEventInstance.setParameterByName("bardShootNote", (float)num, false);
			AudioManager.PlayAttached(this, this.m_abilityFiredEventInstance, base.gameObject);
		}
		if (this.m_isElectricLute)
		{
			projectile.GetComponent<KineticBowProjectileAudioEventEmitter>().SetPitch((float)num);
		}
		AudioManager.Stop(this.m_aimEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x04001236 RID: 4662
	[SerializeField]
	private bool m_isElectricLute;

	// Token: 0x04001237 RID: 4663
	private AimedAbility_RL m_ability;

	// Token: 0x04001238 RID: 4664
	private const float LUTE_PITCH_DIFF = 22.5f;
}
