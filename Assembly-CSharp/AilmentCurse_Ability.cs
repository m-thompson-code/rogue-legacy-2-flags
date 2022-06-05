using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

// Token: 0x0200016F RID: 367
public class AilmentCurse_Ability : BaseAbility_RL, ISpell, IAbility
{
	// Token: 0x170006C7 RID: 1735
	// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x000270F4 File Offset: 0x000252F4
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x170006C8 RID: 1736
	// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x000270FB File Offset: 0x000252FB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006C9 RID: 1737
	// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x00027102 File Offset: 0x00025302
	protected override float TellAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170006CA RID: 1738
	// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x00027109 File Offset: 0x00025309
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0.375f;
		}
	}

	// Token: 0x170006CB RID: 1739
	// (get) Token: 0x06000CA8 RID: 3240 RVA: 0x00027110 File Offset: 0x00025310
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x170006CC RID: 1740
	// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x00027117 File Offset: 0x00025317
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006CD RID: 1741
	// (get) Token: 0x06000CAA RID: 3242 RVA: 0x0002711E File Offset: 0x0002531E
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x170006CE RID: 1742
	// (get) Token: 0x06000CAB RID: 3243 RVA: 0x00027125 File Offset: 0x00025325
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006CF RID: 1743
	// (get) Token: 0x06000CAC RID: 3244 RVA: 0x0002712C File Offset: 0x0002532C
	protected override float ExitAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170006D0 RID: 1744
	// (get) Token: 0x06000CAD RID: 3245 RVA: 0x00027133 File Offset: 0x00025333
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000CAE RID: 3246 RVA: 0x0002713A File Offset: 0x0002533A
	public override void PreCastAbility()
	{
		base.PreCastAbility();
		this.m_chargeEmitter.Play();
	}

	// Token: 0x06000CAF RID: 3247 RVA: 0x0002714D File Offset: 0x0002534D
	public override IEnumerator CastAbility()
	{
		this.m_abilityController.PlayerController.PauseGravity(true, true);
		yield return base.CastAbility();
		yield break;
	}

	// Token: 0x06000CB0 RID: 3248 RVA: 0x0002715C File Offset: 0x0002535C
	protected override void FireProjectile()
	{
		this.m_chargeEmitter.Stop();
		base.FireProjectile();
		this.m_abilityController.PlayerController.StartCoroutine(this.TimeStop());
	}

	// Token: 0x06000CB1 RID: 3249 RVA: 0x00027186 File Offset: 0x00025386
	private IEnumerator TimeStop()
	{
		TimeScaleType timeScaleType = RLTimeScale.GetAvailableSlowTimeStack();
		RLTimeScale.SetTimeScale(timeScaleType, 0.1f);
		float timeDuration = Time.unscaledTime + 0.1f;
		while (Time.unscaledTime < timeDuration)
		{
			yield return null;
		}
		RLTimeScale.SetTimeScale(timeScaleType, 1f);
		yield break;
	}

	// Token: 0x06000CB2 RID: 3250 RVA: 0x0002718E File Offset: 0x0002538E
	public override void StopAbility(bool abilityInterrupted)
	{
		this.m_chargeEmitter.Stop();
		this.m_abilityController.PlayerController.ResumeGravity();
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x040010A9 RID: 4265
	[SerializeField]
	private StudioEventEmitter m_chargeEmitter;
}
