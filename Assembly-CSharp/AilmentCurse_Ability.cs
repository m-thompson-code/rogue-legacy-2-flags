using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

// Token: 0x020002A6 RID: 678
public class AilmentCurse_Ability : BaseAbility_RL, ISpell, IAbility
{
	// Token: 0x17000923 RID: 2339
	// (get) Token: 0x060013BD RID: 5053 RVA: 0x00004AB5 File Offset: 0x00002CB5
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x17000924 RID: 2340
	// (get) Token: 0x060013BE RID: 5054 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000925 RID: 2341
	// (get) Token: 0x060013BF RID: 5055 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float TellAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000926 RID: 2342
	// (get) Token: 0x060013C0 RID: 5056 RVA: 0x00008A96 File Offset: 0x00006C96
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0.375f;
		}
	}

	// Token: 0x17000927 RID: 2343
	// (get) Token: 0x060013C1 RID: 5057 RVA: 0x00004FFB File Offset: 0x000031FB
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000928 RID: 2344
	// (get) Token: 0x060013C2 RID: 5058 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000929 RID: 2345
	// (get) Token: 0x060013C3 RID: 5059 RVA: 0x00004FFB File Offset: 0x000031FB
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x1700092A RID: 2346
	// (get) Token: 0x060013C4 RID: 5060 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700092B RID: 2347
	// (get) Token: 0x060013C5 RID: 5061 RVA: 0x00004536 File Offset: 0x00002736
	protected override float ExitAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700092C RID: 2348
	// (get) Token: 0x060013C6 RID: 5062 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060013C7 RID: 5063 RVA: 0x0000A127 File Offset: 0x00008327
	public override void PreCastAbility()
	{
		base.PreCastAbility();
		this.m_chargeEmitter.Play();
	}

	// Token: 0x060013C8 RID: 5064 RVA: 0x0000A13A File Offset: 0x0000833A
	public override IEnumerator CastAbility()
	{
		this.m_abilityController.PlayerController.PauseGravity(true, true);
		yield return base.CastAbility();
		yield break;
	}

	// Token: 0x060013C9 RID: 5065 RVA: 0x0000A149 File Offset: 0x00008349
	protected override void FireProjectile()
	{
		this.m_chargeEmitter.Stop();
		base.FireProjectile();
		this.m_abilityController.PlayerController.StartCoroutine(this.TimeStop());
	}

	// Token: 0x060013CA RID: 5066 RVA: 0x0000A173 File Offset: 0x00008373
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

	// Token: 0x060013CB RID: 5067 RVA: 0x0000A17B File Offset: 0x0000837B
	public override void StopAbility(bool abilityInterrupted)
	{
		this.m_chargeEmitter.Stop();
		this.m_abilityController.PlayerController.ResumeGravity();
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x040015E1 RID: 5601
	[SerializeField]
	private StudioEventEmitter m_chargeEmitter;
}
