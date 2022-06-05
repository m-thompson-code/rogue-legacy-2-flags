using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x020002E6 RID: 742
public class Bow_Ability : AimedAbility_RL, IAttack, IAbility, IAudioEventEmitter
{
	// Token: 0x17000A9A RID: 2714
	// (get) Token: 0x0600168D RID: 5773 RVA: 0x00004536 File Offset: 0x00002736
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000A9B RID: 2715
	// (get) Token: 0x0600168E RID: 5774 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A9C RID: 2716
	// (get) Token: 0x0600168F RID: 5775 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float TellAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000A9D RID: 2717
	// (get) Token: 0x06001690 RID: 5776 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A9E RID: 2718
	// (get) Token: 0x06001691 RID: 5777 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000A9F RID: 2719
	// (get) Token: 0x06001692 RID: 5778 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000AA0 RID: 2720
	// (get) Token: 0x06001693 RID: 5779 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000AA1 RID: 2721
	// (get) Token: 0x06001694 RID: 5780 RVA: 0x0000B4E9 File Offset: 0x000096E9
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.08f;
		}
	}

	// Token: 0x17000AA2 RID: 2722
	// (get) Token: 0x06001695 RID: 5781 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000AA3 RID: 2723
	// (get) Token: 0x06001696 RID: 5782 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000AA4 RID: 2724
	// (get) Token: 0x06001697 RID: 5783 RVA: 0x00006764 File Offset: 0x00004964
	protected virtual float GravityReduction
	{
		get
		{
			return 0.425f;
		}
	}

	// Token: 0x17000AA5 RID: 2725
	// (get) Token: 0x06001698 RID: 5784 RVA: 0x0000B4F0 File Offset: 0x000096F0
	protected virtual Vector2 BowPushbackAmount
	{
		get
		{
			return new Vector2(4.5f, 13.5f);
		}
	}

	// Token: 0x17000AA6 RID: 2726
	// (get) Token: 0x06001699 RID: 5785 RVA: 0x0000B501 File Offset: 0x00009701
	public override Vector2 PushbackAmount
	{
		get
		{
			return this.BowPushbackAmount;
		}
	}

	// Token: 0x0600169A RID: 5786 RVA: 0x0000B509 File Offset: 0x00009709
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		base.Initialize(abilityController, castAbilityType);
		this.m_gravityReductionModWhenAiming = this.GravityReduction;
	}

	// Token: 0x0600169B RID: 5787 RVA: 0x0000B51F File Offset: 0x0000971F
	public override void PreCastAbility()
	{
		base.PreCastAbility();
		this.m_abilityController.PlayerController.LookController.ArrowGeo.gameObject.SetActive(true);
	}

	// Token: 0x0600169C RID: 5788 RVA: 0x0000B547 File Offset: 0x00009747
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

	// Token: 0x0600169D RID: 5789 RVA: 0x0008B584 File Offset: 0x00089784
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

	// Token: 0x0600169E RID: 5790 RVA: 0x0000B55D File Offset: 0x0000975D
	public override void StopAbility(bool abilityInterrupted)
	{
		this.m_abilityController.PlayerController.LookController.ArrowGeo.gameObject.SetActive(false);
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x0600169F RID: 5791 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void PlayCritWindowAudio()
	{
	}
}
