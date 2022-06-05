using System;
using UnityEngine;

// Token: 0x020007A6 RID: 1958
public class HPAmountCheckSkillCritProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06003BAA RID: 15274 RVA: 0x00020CCE File Offset: 0x0001EECE
	protected override void Awake()
	{
		base.Awake();
		this.m_onProjectileCollision = new Action<Projectile_RL, GameObject>(this.OnProjectileCollision);
	}

	// Token: 0x06003BAB RID: 15275 RVA: 0x00020CE8 File Offset: 0x0001EEE8
	private void OnEnable()
	{
		base.SourceProjectile.OnCollisionRelay.AddListener(this.m_onProjectileCollision, false);
		this.m_critWasApplied = false;
	}

	// Token: 0x06003BAC RID: 15276 RVA: 0x00020D09 File Offset: 0x0001EF09
	private void OnDisable()
	{
		base.SourceProjectile.OnCollisionRelay.RemoveListener(this.m_onProjectileCollision);
	}

	// Token: 0x06003BAD RID: 15277 RVA: 0x000F47AC File Offset: 0x000F29AC
	private void OnProjectileCollision(Projectile_RL projectile, GameObject obj)
	{
		if (!obj.CompareTag("Enemy"))
		{
			return;
		}
		EnemyController component = obj.GetComponent<EnemyController>();
		if (!component)
		{
			return;
		}
		if (this.HPConditionMet(component))
		{
			if (base.SourceProjectile.ActualCritChance < 100f)
			{
				base.SourceProjectile.ActualCritChance += 100f;
				this.m_critWasApplied = true;
				return;
			}
		}
		else if (this.m_critWasApplied && base.SourceProjectile.ActualCritChance >= 100f)
		{
			base.SourceProjectile.ActualCritChance -= 100f;
			this.m_critWasApplied = false;
		}
	}

	// Token: 0x06003BAE RID: 15278 RVA: 0x000F484C File Offset: 0x000F2A4C
	private bool HPConditionMet(BaseCharacterController character)
	{
		switch (this.m_amountCheckType)
		{
		case HPAmountCheckSkillCritProjectileLogic.AmountCheckType.Greater:
			return character.CurrentHealth > (float)character.ActualMaxHealth * this.m_hpAmount;
		case HPAmountCheckSkillCritProjectileLogic.AmountCheckType.GreaterOrEqual:
			return character.CurrentHealth >= (float)character.ActualMaxHealth * this.m_hpAmount;
		case HPAmountCheckSkillCritProjectileLogic.AmountCheckType.Less:
			return character.CurrentHealth < (float)character.ActualMaxHealth * this.m_hpAmount;
		case HPAmountCheckSkillCritProjectileLogic.AmountCheckType.LessOrEqual:
			return character.CurrentHealth <= (float)character.ActualMaxHealth * this.m_hpAmount;
		default:
			return true;
		}
	}

	// Token: 0x04002F60 RID: 12128
	[SerializeField]
	private HPAmountCheckSkillCritProjectileLogic.AmountCheckType m_amountCheckType;

	// Token: 0x04002F61 RID: 12129
	[SerializeField]
	private float m_hpAmount;

	// Token: 0x04002F62 RID: 12130
	private bool m_critWasApplied;

	// Token: 0x04002F63 RID: 12131
	private Action<Projectile_RL, GameObject> m_onProjectileCollision;

	// Token: 0x020007A7 RID: 1959
	private enum AmountCheckType
	{
		// Token: 0x04002F65 RID: 12133
		None,
		// Token: 0x04002F66 RID: 12134
		Greater,
		// Token: 0x04002F67 RID: 12135
		GreaterOrEqual,
		// Token: 0x04002F68 RID: 12136
		Less,
		// Token: 0x04002F69 RID: 12137
		LessOrEqual
	}
}
