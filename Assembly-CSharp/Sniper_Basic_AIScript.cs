using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000122 RID: 290
public class Sniper_Basic_AIScript : BaseAIScript
{
	// Token: 0x060008ED RID: 2285 RVA: 0x0001D7A8 File Offset: 0x0001B9A8
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"SniperBoltProjectile",
			"SniperSlashBoltProjectile"
		};
	}

	// Token: 0x170004C5 RID: 1221
	// (get) Token: 0x060008EE RID: 2286 RVA: 0x0001D7C6 File Offset: 0x0001B9C6
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x170004C6 RID: 1222
	// (get) Token: 0x060008EF RID: 2287 RVA: 0x0001D7D7 File Offset: 0x0001B9D7
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x170004C7 RID: 1223
	// (get) Token: 0x060008F0 RID: 2288 RVA: 0x0001D7E8 File Offset: 0x0001B9E8
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x170004C8 RID: 1224
	// (get) Token: 0x060008F1 RID: 2289 RVA: 0x0001D7F9 File Offset: 0x0001B9F9
	protected virtual float m_shoot_AimDelayDuration
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x170004C9 RID: 1225
	// (get) Token: 0x060008F2 RID: 2290 RVA: 0x0001D800 File Offset: 0x0001BA00
	protected virtual float m_shoot_FireDelayDuration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004CA RID: 1226
	// (get) Token: 0x060008F3 RID: 2291 RVA: 0x0001D807 File Offset: 0x0001BA07
	protected virtual int AimShot_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170004CB RID: 1227
	// (get) Token: 0x060008F4 RID: 2292 RVA: 0x0001D80A File Offset: 0x0001BA0A
	protected virtual float AimShot_MultiShotDelay
	{
		get
		{
			return 0.075f;
		}
	}

	// Token: 0x170004CC RID: 1228
	// (get) Token: 0x060008F5 RID: 2293 RVA: 0x0001D811 File Offset: 0x0001BA11
	protected virtual bool AimShot_FireSideShot
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170004CD RID: 1229
	// (get) Token: 0x060008F6 RID: 2294 RVA: 0x0001D814 File Offset: 0x0001BA14
	protected virtual bool AimShot_FireSlashBolts
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004CE RID: 1230
	// (get) Token: 0x060008F7 RID: 2295 RVA: 0x0001D817 File Offset: 0x0001BA17
	protected virtual float AimShot_SideShotSpread
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x0001D820 File Offset: 0x0001BA20
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		GameObject @object = enemyController.GetComponent<ObjectReferenceFinder>().GetObject("AimIndicator");
		this.m_aimIndicator = @object.GetComponent<SpriteRenderer>();
		this.m_aimIndicator.transform.SetParent(base.EnemyController.Pivot.transform, true);
		this.m_aimIndicator.gameObject.SetActive(false);
		this.m_aimFollowTarget = true;
		this.FacePlayerImmediately();
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x0001D890 File Offset: 0x0001BA90
	private void OnEnable()
	{
		if (this.m_aimIndicator)
		{
			this.m_aimIndicator.gameObject.SetActive(false);
		}
		if (base.EnemyController && base.EnemyController.IsInitialized)
		{
			this.FacePlayerImmediately();
		}
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x0001D8D0 File Offset: 0x0001BAD0
	private void FacePlayerImmediately()
	{
		Vector3 localEulerAngles = base.EnemyController.Pivot.transform.localEulerAngles;
		localEulerAngles.z = CDGHelper.AngleBetweenPts(base.EnemyController.Midpoint, base.EnemyController.TargetController.Midpoint);
		base.EnemyController.Pivot.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x0001D93A File Offset: 0x0001BB3A
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator AimAndFire()
	{
		if (this.m_shoot_AimDelayDuration > 0f)
		{
			yield return base.Wait(this.m_shoot_AimDelayDuration, false);
		}
		this.m_aimFollowTarget = false;
		this.m_aimIndicator.gameObject.SetActive(true);
		base.Animator.SetBool("Armed", true);
		yield return this.Default_TellIntroAndLoop("SingleShot_Tell_Intro", this.m_shoot_TellIntro_AnimationSpeed, "SingleShot_Tell_Hold", this.m_shoot_TellHold_AnimationSpeed, this.m_shoot_TellHold_Delay);
		yield return this.Default_Animation("SingleShot_Attack_Intro", this.m_shoot_AttackIntro_AnimationSpeed, this.m_shoot_AttackIntro_Delay, true);
		if (this.m_shoot_FireDelayDuration > 0f)
		{
			yield return base.Wait(this.m_shoot_FireDelayDuration, false);
		}
		float fireAngle = base.EnemyController.Pivot.transform.localEulerAngles.z;
		int num;
		for (int i = 0; i < this.AimShot_TotalShots; i = num + 1)
		{
			if (this.AimShot_FireSlashBolts)
			{
				this.FireProjectile("SniperSlashBoltProjectile", 0, false, fireAngle, 1f, true, true, true);
				if (this.AimShot_FireSideShot)
				{
					this.FireProjectile("SniperSlashBoltProjectile", 0, false, fireAngle + this.AimShot_SideShotSpread, 1f, true, true, true);
					this.FireProjectile("SniperSlashBoltProjectile", 0, false, fireAngle - this.AimShot_SideShotSpread, 1f, true, true, true);
				}
			}
			else
			{
				this.FireProjectile("SniperBoltProjectile", 0, false, fireAngle, 1f, true, true, true);
				if (this.AimShot_FireSideShot)
				{
					this.FireProjectile("SniperBoltProjectile", 0, false, fireAngle + this.AimShot_SideShotSpread, 1f, true, true, true);
					this.FireProjectile("SniperBoltProjectile", 0, false, fireAngle - this.AimShot_SideShotSpread, 1f, true, true, true);
				}
			}
			if (this.AimShot_MultiShotDelay > 0f)
			{
				yield return base.Wait(this.AimShot_MultiShotDelay, false);
			}
			num = i;
		}
		this.m_aimIndicator.gameObject.SetActive(false);
		base.Animator.SetBool("Armed", false);
		yield return this.Default_Animation("SingleShot_Attack_Hold", this.m_shoot_AttackHold_AnimationSpeed, this.m_shoot_AttackHold_Delay, false);
		yield return this.Default_Animation("SingleShot_Exit", this.m_shoot_Exit_AnimationSpeed, this.m_shoot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		this.m_aimFollowTarget = true;
		this.m_reaimStartTime = Time.time;
		yield return this.Default_Attack_Cooldown(this.m_shoot_Exit_ForceIdle, this.m_shoot_Exit_AttackCD);
		yield break;
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x0001D949 File Offset: 0x0001BB49
	public override void OnLBCompleteOrCancelled()
	{
		base.Animator.SetBool("Armed", false);
		this.m_aimFollowTarget = true;
		base.OnLBCompleteOrCancelled();
		this.m_aimIndicator.gameObject.SetActive(false);
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x0001D97A File Offset: 0x0001BB7A
	public override void Unpause()
	{
		base.Unpause();
		this.m_reaimStartTime = Time.time;
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x0001D990 File Offset: 0x0001BB90
	private void Update()
	{
		if (!base.IsInitialized)
		{
			return;
		}
		if (base.IsPaused)
		{
			return;
		}
		if (this.m_aimFollowTarget)
		{
			float z = CDGHelper.AngleBetweenPts(base.EnemyController.Midpoint, base.EnemyController.TargetController.Midpoint);
			Vector3 vector = new Vector3(0f, 0f, z);
			float num = 1f;
			float num2 = this.m_reaimStartTime + num;
			if (Time.time < num2)
			{
				Vector3 localEulerAngles = base.EnemyController.Pivot.transform.localEulerAngles;
				localEulerAngles.z = Mathf.LerpAngle(localEulerAngles.z, vector.z, 1f - (num2 - Time.time / num));
				base.EnemyController.Pivot.transform.localEulerAngles = localEulerAngles;
			}
			else
			{
				base.EnemyController.Pivot.transform.localEulerAngles = vector;
			}
			float num3 = CDGHelper.DistanceBetweenPts(base.EnemyController.Midpoint, base.EnemyController.TargetController.Midpoint);
			Vector2 direction = (base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint).normalized;
			RaycastHit2D hit = Physics2D.Raycast(base.EnemyController.Midpoint, direction, num3, base.EnemyController.ControllerCorgi.PlatformMask);
			if (hit)
			{
				num3 = hit.distance;
			}
			Vector2 size = this.m_aimIndicator.size;
			size.x = num3 * (1f / this.m_aimIndicator.transform.lossyScale.x);
			this.m_aimIndicator.size = size;
		}
	}

	// Token: 0x04000C66 RID: 3174
	protected float m_shoot_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000C67 RID: 3175
	protected float m_shoot_TellHold_AnimationSpeed = 2f;

	// Token: 0x04000C68 RID: 3176
	protected float m_shoot_TellHold_Delay = 0.5f;

	// Token: 0x04000C69 RID: 3177
	protected float m_shoot_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000C6A RID: 3178
	protected float m_shoot_AttackIntro_Delay;

	// Token: 0x04000C6B RID: 3179
	protected float m_shoot_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000C6C RID: 3180
	protected float m_shoot_AttackHold_Delay;

	// Token: 0x04000C6D RID: 3181
	protected float m_shoot_Exit_AnimationSpeed = 1f;

	// Token: 0x04000C6E RID: 3182
	protected float m_shoot_Exit_Delay;

	// Token: 0x04000C6F RID: 3183
	protected float m_shoot_Exit_ForceIdle = 1.25f;

	// Token: 0x04000C70 RID: 3184
	protected float m_shoot_Exit_AttackCD;

	// Token: 0x04000C71 RID: 3185
	private SpriteRenderer m_aimIndicator;

	// Token: 0x04000C72 RID: 3186
	private bool m_aimFollowTarget = true;

	// Token: 0x04000C73 RID: 3187
	private float m_reaimStartTime;

	// Token: 0x04000C74 RID: 3188
	protected const string SINGLE_SHOT_TELL_INTRO = "SingleShot_Tell_Intro";

	// Token: 0x04000C75 RID: 3189
	protected const string SINGLE_SHOT_TELL_HOLD = "SingleShot_Tell_Hold";

	// Token: 0x04000C76 RID: 3190
	protected const string SINGLE_SHOT_ATTACK_INTRO = "SingleShot_Attack_Intro";

	// Token: 0x04000C77 RID: 3191
	protected const string SINGLE_SHOT_ATTACK_HOLD = "SingleShot_Attack_Hold";

	// Token: 0x04000C78 RID: 3192
	protected const string SINGLE_SHOT_EXIT = "SingleShot_Exit";

	// Token: 0x04000C79 RID: 3193
	protected const string SINGLE_SHOT_PROJECTILE = "SniperBoltProjectile";
}
