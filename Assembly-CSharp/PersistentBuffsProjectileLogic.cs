using System;

// Token: 0x020004A6 RID: 1190
public class PersistentBuffsProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06002B87 RID: 11143 RVA: 0x00093C26 File Offset: 0x00091E26
	private void OnEnable()
	{
		this.m_playerController = null;
		if (base.SourceProjectile.OwnerController)
		{
			this.m_playerController = (base.SourceProjectile.OwnerController as PlayerController);
		}
		this.UpdatePersistentDamageBuffs();
	}

	// Token: 0x06002B88 RID: 11144 RVA: 0x00093C5D File Offset: 0x00091E5D
	private void OnDisable()
	{
		this.m_danceApplied = false;
		this.m_comboApplied = false;
		this.m_invulnDamageBuffApplied = false;
		this.m_groundDamageBonusApplied = false;
		this.m_previousComboCount = 0;
	}

	// Token: 0x06002B89 RID: 11145 RVA: 0x00093C84 File Offset: 0x00091E84
	private void UpdatePersistentDamageBuffs()
	{
		if (this.m_playerController)
		{
			bool flag = false;
			if (this.m_danceApplied)
			{
				if (!this.m_playerController.StatusEffectController.HasStatusEffect(StatusEffectType.Player_Dance))
				{
					this.m_danceApplied = false;
					flag = true;
				}
			}
			else if (this.m_playerController.StatusEffectController.HasStatusEffect(StatusEffectType.Player_Dance))
			{
				this.m_danceApplied = true;
				flag = true;
			}
			if (this.m_comboApplied)
			{
				if (!this.m_playerController.StatusEffectController.HasStatusEffect(StatusEffectType.Player_Combo))
				{
					this.m_comboApplied = false;
					flag = true;
					this.m_previousComboCount = 0;
				}
				else
				{
					int timesStacked = this.m_playerController.StatusEffectController.GetStatusEffect(StatusEffectType.Player_Combo).TimesStacked;
					if (this.m_previousComboCount != timesStacked)
					{
						this.m_previousComboCount = timesStacked;
						flag = true;
					}
				}
			}
			else if (this.m_playerController.StatusEffectController.HasStatusEffect(StatusEffectType.Player_Combo))
			{
				this.m_comboApplied = true;
				flag = true;
			}
			if (this.m_invulnDamageBuffApplied)
			{
				if (!this.m_playerController.IsInvincible)
				{
					this.m_invulnDamageBuffApplied = false;
					flag = true;
				}
			}
			else if (this.m_playerController.IsInvincible && SaveManager.PlayerSaveData.GetRelic(RelicType.InvulnDamageBuff).Level > 0)
			{
				this.m_invulnDamageBuffApplied = true;
				flag = true;
			}
			if (this.m_groundDamageBonusApplied)
			{
				if (this.m_groundDamageBonusApplied && !this.m_playerController.IsGrounded)
				{
					this.m_groundDamageBonusApplied = false;
					flag = true;
				}
			}
			else if (this.m_playerController.IsGrounded && SaveManager.PlayerSaveData.GetRelic(RelicType.GroundDamageBonus).Level > 0)
			{
				this.m_groundDamageBonusApplied = true;
				flag = true;
			}
			if (flag)
			{
				base.SourceProjectile.RelicDamageTypeString = null;
				base.SourceProjectile.DamageMod = 0f;
				ProjectileManager.ApplyProjectileDamage(this.m_playerController, base.SourceProjectile);
			}
		}
	}

	// Token: 0x06002B8A RID: 11146 RVA: 0x00093E3F File Offset: 0x0009203F
	private void FixedUpdate()
	{
		this.UpdatePersistentDamageBuffs();
	}

	// Token: 0x04002362 RID: 9058
	private PlayerController m_playerController;

	// Token: 0x04002363 RID: 9059
	private bool m_danceApplied;

	// Token: 0x04002364 RID: 9060
	private bool m_comboApplied;

	// Token: 0x04002365 RID: 9061
	private bool m_invulnDamageBuffApplied;

	// Token: 0x04002366 RID: 9062
	private bool m_groundDamageBonusApplied;

	// Token: 0x04002367 RID: 9063
	private int m_previousComboCount;
}
