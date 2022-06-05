using System;
using UnityEngine;

// Token: 0x020002CD RID: 717
public class Knockout_Ability : AimedAbility_RL, ITalent, IAbility
{
	// Token: 0x170009F0 RID: 2544
	// (get) Token: 0x0600155A RID: 5466 RVA: 0x00004A8D File Offset: 0x00002C8D
	protected override AbilityAnimState StateToHoldAttackOn
	{
		get
		{
			return AbilityAnimState.Attack_Intro;
		}
	}

	// Token: 0x170009F1 RID: 2545
	// (get) Token: 0x0600155B RID: 5467 RVA: 0x00004536 File Offset: 0x00002736
	protected override float TellAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170009F2 RID: 2546
	// (get) Token: 0x0600155C RID: 5468 RVA: 0x00004536 File Offset: 0x00002736
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x0600155D RID: 5469 RVA: 0x0000A9EB File Offset: 0x00008BEB
	protected override void Awake()
	{
		base.Awake();
		this.m_onCollision = new Action<Projectile_RL, GameObject>(this.OnCollision);
	}

	// Token: 0x0600155E RID: 5470 RVA: 0x00089CA4 File Offset: 0x00087EA4
	protected override void FireProjectile()
	{
		base.FireProjectile();
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.OnCollisionRelay.AddListener(this.m_onCollision, false);
			this.m_firedProjectile.ExternalKnockbackMod = CDGHelper.AngleToVector(this.m_unmoddedAngle);
		}
	}

	// Token: 0x0600155F RID: 5471 RVA: 0x00089CF4 File Offset: 0x00087EF4
	protected override void UpdateArrowAim(bool doNotUpdatePlayerAnims = false)
	{
		base.UpdateArrowAim(doNotUpdatePlayerAnims);
		if (this.m_abilityController && this.m_abilityController.PlayerController.IsGrounded && this.m_abilityController.PlayerController.ControllerCorgi.StandingOnCollider && this.m_abilityController.PlayerController.ControllerCorgi.StandingOnCollider.CompareTag("Platform"))
		{
			Vector2 knockout_MIN_MAX_ANGLE = Ability_EV.KNOCKOUT_MIN_MAX_ANGLE;
			this.m_aimAngle = Mathf.Clamp(this.m_aimAngle, knockout_MIN_MAX_ANGLE.x, knockout_MIN_MAX_ANGLE.y);
			this.m_unmoddedAngle = Mathf.Clamp(this.m_unmoddedAngle, knockout_MIN_MAX_ANGLE.x, knockout_MIN_MAX_ANGLE.y);
			if (this.m_unmoddedAngle == knockout_MIN_MAX_ANGLE.x || this.m_unmoddedAngle == knockout_MIN_MAX_ANGLE.y)
			{
				if (this.m_abilityController.PlayerController.IsFacingRight)
				{
					this.m_unmoddedAngle = knockout_MIN_MAX_ANGLE.x;
					return;
				}
				this.m_unmoddedAngle = knockout_MIN_MAX_ANGLE.y;
			}
		}
	}

	// Token: 0x06001560 RID: 5472 RVA: 0x0000AA05 File Offset: 0x00008C05
	public override void StopAbility(bool abilityInterrupted)
	{
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.OnCollisionRelay.RemoveListener(this.m_onCollision);
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x06001561 RID: 5473 RVA: 0x00089DF8 File Offset: 0x00087FF8
	protected void OnCollision(Projectile_RL projectile, GameObject colliderObj)
	{
		if (colliderObj.CompareTag("Enemy"))
		{
			EffectManager.SetEffectParams("SlowTime_Effect", new object[]
			{
				"m_timeScaleValue",
				0.05f
			});
			EffectManager.PlayEffect(this.m_abilityController.PlayerController.gameObject, this.m_abilityController.PlayerController.Animator, "SlowTime_Effect", Vector3.zero, 0.175f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		}
	}

	// Token: 0x04001675 RID: 5749
	protected float gravityMod;

	// Token: 0x04001676 RID: 5750
	private Action<Projectile_RL, GameObject> m_onCollision;
}
