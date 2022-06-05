using System;

// Token: 0x020007B2 RID: 1970
public class PersistentBuffsProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06003BF4 RID: 15348 RVA: 0x00021125 File Offset: 0x0001F325
	private void OnEnable()
	{
		this.m_playerController = null;
		if (base.SourceProjectile.OwnerController)
		{
			this.m_playerController = (base.SourceProjectile.OwnerController as PlayerController);
		}
		this.UpdatePersistentDamageBuffs();
	}

	// Token: 0x06003BF5 RID: 15349 RVA: 0x0002115C File Offset: 0x0001F35C
	private void OnDisable()
	{
		this.m_danceApplied = false;
		this.m_comboApplied = false;
		this.m_invulnDamageBuffApplied = false;
		this.m_groundDamageBonusApplied = false;
		this.m_previousComboCount = 0;
	}

	// Token: 0x06003BF6 RID: 15350 RVA: 0x000F538C File Offset: 0x000F358C
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

	// Token: 0x06003BF7 RID: 15351 RVA: 0x00021181 File Offset: 0x0001F381
	private void FixedUpdate()
	{
		this.UpdatePersistentDamageBuffs();
	}

	// Token: 0x04002F98 RID: 12184
	private PlayerController m_playerController;

	// Token: 0x04002F99 RID: 12185
	private bool m_danceApplied;

	// Token: 0x04002F9A RID: 12186
	private bool m_comboApplied;

	// Token: 0x04002F9B RID: 12187
	private bool m_invulnDamageBuffApplied;

	// Token: 0x04002F9C RID: 12188
	private bool m_groundDamageBonusApplied;

	// Token: 0x04002F9D RID: 12189
	private int m_previousComboCount;
}
