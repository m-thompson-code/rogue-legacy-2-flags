using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x02000192 RID: 402
public class Bow_Ability : AimedAbility_RL, IAttack, IAbility, IAudioEventEmitter
{
	// Token: 0x17000806 RID: 2054
	// (get) Token: 0x06000ECA RID: 3786 RVA: 0x0002C6FD File Offset: 0x0002A8FD
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000807 RID: 2055
	// (get) Token: 0x06000ECB RID: 3787 RVA: 0x0002C704 File Offset: 0x0002A904
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000808 RID: 2056
	// (get) Token: 0x06000ECC RID: 3788 RVA: 0x0002C70B File Offset: 0x0002A90B
	protected override float TellAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000809 RID: 2057
	// (get) Token: 0x06000ECD RID: 3789 RVA: 0x0002C712 File Offset: 0x0002A912
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700080A RID: 2058
	// (get) Token: 0x06000ECE RID: 3790 RVA: 0x0002C719 File Offset: 0x0002A919
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700080B RID: 2059
	// (get) Token: 0x06000ECF RID: 3791 RVA: 0x0002C720 File Offset: 0x0002A920
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700080C RID: 2060
	// (get) Token: 0x06000ED0 RID: 3792 RVA: 0x0002C727 File Offset: 0x0002A927
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700080D RID: 2061
	// (get) Token: 0x06000ED1 RID: 3793 RVA: 0x0002C72E File Offset: 0x0002A92E
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.08f;
		}
	}

	// Token: 0x1700080E RID: 2062
	// (get) Token: 0x06000ED2 RID: 3794 RVA: 0x0002C735 File Offset: 0x0002A935
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700080F RID: 2063
	// (get) Token: 0x06000ED3 RID: 3795 RVA: 0x0002C73C File Offset: 0x0002A93C
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000810 RID: 2064
	// (get) Token: 0x06000ED4 RID: 3796 RVA: 0x0002C743 File Offset: 0x0002A943
	protected virtual float GravityReduction
	{
		get
		{
			return 0.425f;
		}
	}

	// Token: 0x17000811 RID: 2065
	// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x0002C74A File Offset: 0x0002A94A
	protected virtual Vector2 BowPushbackAmount
	{
		get
		{
			return new Vector2(4.5f, 13.5f);
		}
	}

	// Token: 0x17000812 RID: 2066
	// (get) Token: 0x06000ED6 RID: 3798 RVA: 0x0002C75B File Offset: 0x0002A95B
	public override Vector2 PushbackAmount
	{
		get
		{
			return this.BowPushbackAmount;
		}
	}

	// Token: 0x06000ED7 RID: 3799 RVA: 0x0002C763 File Offset: 0x0002A963
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		base.Initialize(abilityController, castAbilityType);
		this.m_gravityReductionModWhenAiming = this.GravityReduction;
	}

	// Token: 0x06000ED8 RID: 3800 RVA: 0x0002C779 File Offset: 0x0002A979
	public override void PreCastAbility()
	{
		base.PreCastAbility();
		this.m_abilityController.PlayerController.LookController.ArrowGeo.gameObject.SetActive(true);
	}

	// Token: 0x06000ED9 RID: 3801 RVA: 0x0002C7A1 File Offset: 0x0002A9A1
	protected override IEnumerator ChangeAnim(float duration)
	{
		if (base.CurrentAbilityAnimState == AbilityAnimState.Attack_Intro)
		{
			Color startColor = new Color(1f, 1f, 1f, 1f);
			this.m_aimLine.startColor = startColor;
		}
		yield return base.ChangeAnim(duration);
		yield break;
	}

	// Token: 0x06000EDA RID: 3802 RVA: 0x0002C7B8 File Offset: 0x0002A9B8
	protected override void Update()
	{
		base.Update();
		if (this.m_critEffectGO)
		{
			Vector3 localEulerAngles = this.m_critEffectGO.transform.localEulerAngles;
			localEulerAngles.z = this.m_aimAngle;
			this.m_critEffectGO.transform.localEulerAngles = localEulerAngles;
		}
	}

	// Token: 0x06000EDB RID: 3803 RVA: 0x0002C807 File Offset: 0x0002AA07
	public override void StopAbility(bool abilityInterrupted)
	{
		this.m_abilityController.PlayerController.LookController.ArrowGeo.gameObject.SetActive(false);
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x06000EDC RID: 3804 RVA: 0x0002C830 File Offset: 0x0002AA30
	protected override void PlayCritWindowAudio()
	{
	}
}
