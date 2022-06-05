using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x02000538 RID: 1336
public class KnockoutStatusEffect : BaseStatusEffect
{
	// Token: 0x17001153 RID: 4435
	// (get) Token: 0x06002AEC RID: 10988 RVA: 0x00017F99 File Offset: 0x00016199
	public override string[] ProjectileNameArray
	{
		get
		{
			return KnockoutStatusEffect.m_projectileNameArray;
		}
	}

	// Token: 0x17001154 RID: 4436
	// (get) Token: 0x06002AED RID: 10989 RVA: 0x00017FA0 File Offset: 0x000161A0
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Knockout;
		}
	}

	// Token: 0x17001155 RID: 4437
	// (get) Token: 0x06002AEE RID: 10990 RVA: 0x00005FB8 File Offset: 0x000041B8
	public override float StartingDurationOverride
	{
		get
		{
			return 0.6f;
		}
	}

	// Token: 0x06002AEF RID: 10991 RVA: 0x00017FA4 File Offset: 0x000161A4
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

	// Token: 0x06002AF0 RID: 10992 RVA: 0x00017FBA File Offset: 0x000161BA
	private void RemovePlayerCombo()
	{
		PlayerManager.GetPlayerController().StatusEffectController.StopStatusEffect(StatusEffectType.Player_Combo, true);
	}

	// Token: 0x06002AF1 RID: 10993 RVA: 0x000C2CD8 File Offset: 0x000C0ED8
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

	// Token: 0x040024A8 RID: 9384
	private static string[] m_projectileNameArray = new string[]
	{
		"KnockoutStatusEffectProjectile"
	};

	// Token: 0x040024A9 RID: 9385
	private Projectile_RL m_knockbackProjectile;

	// Token: 0x040024AA RID: 9386
	private bool m_isKnockedBack;

	// Token: 0x040024AB RID: 9387
	private bool m_oneWayCollisionDisabled;
}
