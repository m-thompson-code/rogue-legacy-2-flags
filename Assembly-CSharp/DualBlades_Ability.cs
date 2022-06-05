using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002EE RID: 750
public class DualBlades_Ability : BaseAbility_RL, IAttack, IAbility
{
	// Token: 0x06001721 RID: 5921 RVA: 0x0000BBC4 File Offset: 0x00009DC4
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_projectileNameRight,
			this.m_secondProjectileName,
			this.m_thirdProjectileName
		};
	}

	// Token: 0x17000AFC RID: 2812
	// (get) Token: 0x06001722 RID: 5922 RVA: 0x0000A305 File Offset: 0x00008505
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1.45f;
		}
	}

	// Token: 0x17000AFD RID: 2813
	// (get) Token: 0x06001723 RID: 5923 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000AFE RID: 2814
	// (get) Token: 0x06001724 RID: 5924 RVA: 0x00003C70 File Offset: 0x00001E70
	protected override float TellAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000AFF RID: 2815
	// (get) Token: 0x06001725 RID: 5925 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B00 RID: 2816
	// (get) Token: 0x06001726 RID: 5926 RVA: 0x00003C70 File Offset: 0x00001E70
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000B01 RID: 2817
	// (get) Token: 0x06001727 RID: 5927 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B02 RID: 2818
	// (get) Token: 0x06001728 RID: 5928 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000B03 RID: 2819
	// (get) Token: 0x06001729 RID: 5929 RVA: 0x00006772 File Offset: 0x00004972
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.05f;
		}
	}

	// Token: 0x17000B04 RID: 2820
	// (get) Token: 0x0600172A RID: 5930 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000B05 RID: 2821
	// (get) Token: 0x0600172B RID: 5931 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B06 RID: 2822
	// (get) Token: 0x0600172C RID: 5932 RVA: 0x0008BEE0 File Offset: 0x0008A0E0
	public override string ProjectileName
	{
		get
		{
			switch (this.m_attackCount)
			{
			default:
				return base.ProjectileName;
			case 1:
				return this.m_secondProjectileName;
			case 2:
				return this.m_thirdProjectileName;
			}
		}
	}

	// Token: 0x17000B07 RID: 2823
	// (get) Token: 0x0600172D RID: 5933 RVA: 0x0008BF1C File Offset: 0x0008A11C
	public override Vector2 ProjectileOffset
	{
		get
		{
			switch (this.m_attackCount)
			{
			default:
				return this.m_projectileOffset;
			case 1:
				return this.m_secondProjectileOffset;
			case 2:
				return this.m_thirdProjectileOffset;
			}
		}
	}

	// Token: 0x0600172E RID: 5934 RVA: 0x0000BBF6 File Offset: 0x00009DF6
	protected override void Awake()
	{
		base.Awake();
		if (this.m_abilityTellIntroName != null)
		{
			this.m_abilityTellIntroRight = this.m_abilityTellIntroName.Replace("Left", "Right");
		}
	}

	// Token: 0x0600172F RID: 5935 RVA: 0x0000BC21 File Offset: 0x00009E21
	public override void PreCastAbility()
	{
		this.m_attackCount = 0;
		base.PreCastAbility();
	}

	// Token: 0x06001730 RID: 5936 RVA: 0x0000BC30 File Offset: 0x00009E30
	protected override IEnumerator ChangeAnim(float duration)
	{
		while (duration > 0f)
		{
			duration -= Time.deltaTime;
			yield return null;
		}
		if (base.CurrentAbilityAnimState == AbilityAnimState.Attack)
		{
			if (this.m_attackCount < 3)
			{
				this.m_animator.SetTrigger("ContinueCombo");
			}
			else
			{
				this.m_animator.SetTrigger("Change_Ability_Anim");
			}
		}
		else
		{
			this.m_animator.SetTrigger("Change_Ability_Anim");
		}
		base.PerformTurnAnimCheck();
		yield break;
	}

	// Token: 0x06001731 RID: 5937 RVA: 0x0008BF58 File Offset: 0x0008A158
	protected override void FireProjectile()
	{
		base.FireProjectile();
		this.m_attackCount++;
		if (this.m_attackCount >= 3 && this.m_firedProjectile.ActualCritChance < 100f)
		{
			this.m_firedProjectile.ActualCritChance += 100f;
		}
	}

	// Token: 0x06001732 RID: 5938 RVA: 0x0000BC46 File Offset: 0x00009E46
	public override void StopAbility(bool abilityInterrupted)
	{
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.gameObject.SetActive(false);
			this.m_firedProjectile = null;
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x04001712 RID: 5906
	[SerializeField]
	private string m_projectileNameRight;

	// Token: 0x04001713 RID: 5907
	[Space(10f)]
	[SerializeField]
	private string m_secondProjectileName;

	// Token: 0x04001714 RID: 5908
	[SerializeField]
	private Vector2 m_secondProjectileOffset;

	// Token: 0x04001715 RID: 5909
	[SerializeField]
	private string m_thirdProjectileName;

	// Token: 0x04001716 RID: 5910
	[SerializeField]
	private Vector2 m_thirdProjectileOffset;

	// Token: 0x04001717 RID: 5911
	private const int NUM_ATTACKS = 3;

	// Token: 0x04001718 RID: 5912
	private string m_abilityTellIntroRight;

	// Token: 0x04001719 RID: 5913
	private float m_canAttackAgainCounter;

	// Token: 0x0400171A RID: 5914
	private int m_attackCount;
}
