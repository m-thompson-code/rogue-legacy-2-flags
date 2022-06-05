using System;
using UnityEngine;

// Token: 0x02000184 RID: 388
public class Knockout_Ability : AimedAbility_RL, ITalent, IAbility
{
	// Token: 0x17000772 RID: 1906
	// (get) Token: 0x06000DD9 RID: 3545 RVA: 0x0002AAE5 File Offset: 0x00028CE5
	protected override AbilityAnimState StateToHoldAttackOn
	{
		get
		{
			return AbilityAnimState.Attack_Intro;
		}
	}

	// Token: 0x17000773 RID: 1907
	// (get) Token: 0x06000DDA RID: 3546 RVA: 0x0002AAE8 File Offset: 0x00028CE8
	protected override float TellAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000774 RID: 1908
	// (get) Token: 0x06000DDB RID: 3547 RVA: 0x0002AAEF File Offset: 0x00028CEF
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x06000DDC RID: 3548 RVA: 0x0002AAF6 File Offset: 0x00028CF6
	protected override void Awake()
	{
		base.Awake();
		this.m_onCollision = new Action<Projectile_RL, GameObject>(this.OnCollision);
	}

	// Token: 0x06000DDD RID: 3549 RVA: 0x0002AB10 File Offset: 0x00028D10
	protected override void FireProjectile()
	{
		base.FireProjectile();
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.OnCollisionRelay.AddListener(this.m_onCollision, false);
			this.m_firedProjectile.ExternalKnockbackMod = CDGHelper.AngleToVector(this.m_unmoddedAngle);
		}
	}

	// Token: 0x06000DDE RID: 3550 RVA: 0x0002AB60 File Offset: 0x00028D60
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

	// Token: 0x06000DDF RID: 3551 RVA: 0x0002AC63 File Offset: 0x00028E63
	public override void StopAbility(bool abilityInterrupted)
	{
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.OnCollisionRelay.RemoveListener(this.m_onCollision);
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x06000DE0 RID: 3552 RVA: 0x0002AC90 File Offset: 0x00028E90
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

	// Token: 0x04001100 RID: 4352
	protected float gravityMod;

	// Token: 0x04001101 RID: 4353
	private Action<Projectile_RL, GameObject> m_onCollision;
}
