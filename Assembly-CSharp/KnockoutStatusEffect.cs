using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x0200030A RID: 778
public class KnockoutStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D5C RID: 3420
	// (get) Token: 0x06001ED5 RID: 7893 RVA: 0x000639B3 File Offset: 0x00061BB3
	public override string[] ProjectileNameArray
	{
		get
		{
			return KnockoutStatusEffect.m_projectileNameArray;
		}
	}

	// Token: 0x17000D5D RID: 3421
	// (get) Token: 0x06001ED6 RID: 7894 RVA: 0x000639BA File Offset: 0x00061BBA
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Knockout;
		}
	}

	// Token: 0x17000D5E RID: 3422
	// (get) Token: 0x06001ED7 RID: 7895 RVA: 0x000639BE File Offset: 0x00061BBE
	public override float StartingDurationOverride
	{
		get
		{
			return 0.6f;
		}
	}

	// Token: 0x06001ED8 RID: 7896 RVA: 0x000639C5 File Offset: 0x00061BC5
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_statusEffectController.StartStatusEffect(StatusEffectType.Enemy_DeathDelay, base.Duration, caster);
		this.m_statusEffectController.StartStatusEffect(StatusEffectType.Enemy_Phased, base.Duration, caster);
		yield return null;
		if (this.m_knockbackProjectile && !this.m_knockbackProjectile.IsFreePoolObj)
		{
			this.m_knockbackProjectile.FlagForDestruction(null);
			this.m_knockbackProjectile = null;
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		int num = 0;
		if (this.m_charController.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Combo))
		{
			BaseStatusEffect statusEffect = this.m_charController.StatusEffectController.GetStatusEffect(StatusEffectType.Enemy_Combo);
			num += statusEffect.TimesStacked;
			this.m_charController.StatusEffectController.StopStatusEffect(StatusEffectType.Enemy_Combo, true);
		}
		if (playerController.StatusEffectController.HasStatusEffect(StatusEffectType.Player_Combo))
		{
			BaseStatusEffect statusEffect2 = playerController.StatusEffectController.GetStatusEffect(StatusEffectType.Player_Combo);
			num += statusEffect2.TimesStacked;
			base.Invoke("RemovePlayerCombo", 0f);
		}
		Vector2 vector = this.m_charController.Midpoint - this.m_charController.transform.position;
		float d = this.m_charController.BaseScaleToOffsetWith / this.m_charController.transform.localScale.x;
		vector *= d;
		this.m_knockbackProjectile = ProjectileManager.FireProjectile(this.m_charController.gameObject, "KnockoutStatusEffectProjectile", vector, false, 0f, 1f, false, true, true, true);
		this.m_knockbackProjectile.CastAbilityType = CastAbilityType.Talent;
		this.m_knockbackProjectile.Magic = playerController.ActualMagic;
		this.m_knockbackProjectile.Strength = playerController.ActualStrength;
		this.m_knockbackProjectile.StrengthScale = this.m_knockbackProjectile.ProjectileData.StrengthScale;
		this.m_knockbackProjectile.MagicScale = this.m_knockbackProjectile.ProjectileData.MagicScale;
		if (this.m_knockbackProjectile.ActualDamage > 0f)
		{
			this.m_knockbackProjectile.DamageMod = (float)num * 0.3f * this.m_knockbackProjectile.ActualDamage / this.m_knockbackProjectile.ActualDamage;
		}
		this.m_knockbackProjectile.GetComponent<OnProjectileDeathProjectileLogic>().enabled = true;
		EnemyController enemyController = this.m_charController as EnemyController;
		Vector2 a = caster.gameObject.transform.position + caster.ExternalKnockbackMod * 32f;
		Vector2 pushbackVelocity = a - enemyController.Midpoint;
		pushbackVelocity = pushbackVelocity.normalized;
		pushbackVelocity *= 33f;
		float explosionDelayTime = Time.time + 0.1f;
		Projectile_RL projectile_RL = caster as Projectile_RL;
		this.m_isKnockedBack = false;
		if (projectile_RL && projectile_RL.ActualKnockbackStrength > enemyController.ActualKnockbackDefense)
		{
			if (enemyController.CurrentHealth > 0f)
			{
				enemyController.LogicController.StopAllLogic(true);
				enemyController.ConditionState = CharacterStates.CharacterConditions.Stunned;
				if (global::AnimatorUtility.HasParameter(enemyController.Animator, "Stunned"))
				{
					enemyController.Animator.SetBool("Stunned", true);
				}
				else
				{
					enemyController.Animator.Play("Neutral");
				}
			}
			this.m_isKnockedBack = true;
			if (!enemyController.ControllerCorgi.DisableOneWayCollision)
			{
				this.m_oneWayCollisionDisabled = true;
				enemyController.ControllerCorgi.DisableOneWayCollision = true;
			}
			float oneWayCollisionDuration = Time.time + 0.25f;
			while (Time.time < base.EndTime)
			{
				if (this.m_oneWayCollisionDisabled && Time.time > oneWayCollisionDuration)
				{
					this.m_oneWayCollisionDisabled = false;
					enemyController.ControllerCorgi.DisableOneWayCollision = false;
				}
				this.m_charController.SetVelocity(pushbackVelocity.x, pushbackVelocity.y, false);
				if ((enemyController.TouchingBottomRoomEdge || enemyController.TouchingLeftRoomEdge || enemyController.TouchingRightRoomEdge || enemyController.TouchingTopRoomEdge || this.m_charController.ControllerCorgi.ContactList.Count > 0) && Time.time > explosionDelayTime)
				{
					this.StopEffect(false);
					yield break;
				}
				yield return null;
			}
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06001ED9 RID: 7897 RVA: 0x000639DB File Offset: 0x00061BDB
	private void RemovePlayerCombo()
	{
		PlayerManager.GetPlayerController().StatusEffectController.StopStatusEffect(StatusEffectType.Player_Combo, true);
	}

	// Token: 0x06001EDA RID: 7898 RVA: 0x000639F4 File Offset: 0x00061BF4
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		EnemyController enemyController = this.m_charController as EnemyController;
		enemyController.ConditionState = CharacterStates.CharacterConditions.Normal;
		if (global::AnimatorUtility.HasParameter(enemyController.Animator, "Stunned"))
		{
			enemyController.Animator.SetBool("Stunned", false);
		}
		if (this.m_knockbackProjectile && !this.m_knockbackProjectile.IsFreePoolObj)
		{
			if (interrupted)
			{
				this.m_knockbackProjectile.gameObject.SetActive(false);
			}
			else
			{
				this.m_knockbackProjectile.transform.SetParent(null, true);
				this.m_knockbackProjectile.FlagForDestruction(null);
			}
			this.m_knockbackProjectile = null;
			if (this.m_isKnockedBack)
			{
				this.m_charController.SetVelocity(0f, 0f, false);
				this.m_isKnockedBack = false;
			}
		}
		if (this.m_oneWayCollisionDisabled)
		{
			this.m_oneWayCollisionDisabled = false;
			enemyController.ControllerCorgi.DisableOneWayCollision = false;
		}
		this.m_statusEffectController.StopStatusEffect(StatusEffectType.Enemy_Phased, interrupted);
		this.m_statusEffectController.StopStatusEffect(StatusEffectType.Enemy_DeathDelay, interrupted);
	}

	// Token: 0x04001BDE RID: 7134
	private static string[] m_projectileNameArray = new string[]
	{
		"KnockoutStatusEffectProjectile"
	};

	// Token: 0x04001BDF RID: 7135
	private Projectile_RL m_knockbackProjectile;

	// Token: 0x04001BE0 RID: 7136
	private bool m_isKnockedBack;

	// Token: 0x04001BE1 RID: 7137
	private bool m_oneWayCollisionDisabled;
}
