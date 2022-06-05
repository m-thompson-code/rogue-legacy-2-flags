using System;
using FMOD.Studio;
using RLAudio;
using UnityEngine;

// Token: 0x02000329 RID: 809
public class BardAimedAbilityAudioEventController : AimedAbilityAudioEventController
{
	// Token: 0x06001990 RID: 6544 RVA: 0x0000CE8A File Offset: 0x0000B08A
	private void Awake()
	{
		this.m_ability = base.GetComponent<AimedAbility_RL>();
	}

	// Token: 0x06001991 RID: 6545 RVA: 0x00090488 File Offset: 0x0008E688
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

	// Token: 0x0400183F RID: 6207
	[SerializeField]
	private bool m_isElectricLute;

	// Token: 0x04001840 RID: 6208
	private AimedAbility_RL m_ability;

	// Token: 0x04001841 RID: 6209
	private const float LUTE_PITCH_DIFF = 22.5f;
}
