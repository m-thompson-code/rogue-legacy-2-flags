using System;
using UnityEngine;

// Token: 0x0200049F RID: 1183
public class HPAmountCheckSkillCritProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06002B4F RID: 11087 RVA: 0x00092E2E File Offset: 0x0009102E
	protected override void Awake()
	{
		base.Awake();
		this.m_onProjectileCollision = new Action<Projectile_RL, GameObject>(this.OnProjectileCollision);
	}

	// Token: 0x06002B50 RID: 11088 RVA: 0x00092E48 File Offset: 0x00091048
	private void OnEnable()
	{
		base.SourceProjectile.OnCollisionRelay.AddListener(this.m_onProjectileCollision, false);
		this.m_critWasApplied = false;
	}

	// Token: 0x06002B51 RID: 11089 RVA: 0x00092E69 File Offset: 0x00091069
	private void OnDisable()
	{
		base.SourceProjectile.OnCollisionRelay.RemoveListener(this.m_onProjectileCollision);
	}

	// Token: 0x06002B52 RID: 11090 RVA: 0x00092E84 File Offset: 0x00091084
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

	// Token: 0x06002B53 RID: 11091 RVA: 0x00092F24 File Offset: 0x00091124
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

	// Token: 0x0400233F RID: 9023
	[SerializeField]
	private HPAmountCheckSkillCritProjectileLogic.AmountCheckType m_amountCheckType;

	// Token: 0x04002340 RID: 9024
	[SerializeField]
	private float m_hpAmount;

	// Token: 0x04002341 RID: 9025
	private bool m_critWasApplied;

	// Token: 0x04002342 RID: 9026
	private Action<Projectile_RL, GameObject> m_onProjectileCollision;

	// Token: 0x02000C80 RID: 3200
	private enum AmountCheckType
	{
		// Token: 0x040050B6 RID: 20662
		None,
		// Token: 0x040050B7 RID: 20663
		Greater,
		// Token: 0x040050B8 RID: 20664
		GreaterOrEqual,
		// Token: 0x040050B9 RID: 20665
		Less,
		// Token: 0x040050BA RID: 20666
		LessOrEqual
	}
}
