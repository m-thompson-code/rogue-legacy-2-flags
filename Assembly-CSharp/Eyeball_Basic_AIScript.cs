using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000C0 RID: 192
public class Eyeball_Basic_AIScript : BaseAIScript
{
	// Token: 0x060004E6 RID: 1254 RVA: 0x000166AC File Offset: 0x000148AC
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"EyeballBounceBoltProjectile",
			"EyeballBoltProjectile",
			"EyeballBounceBoltProjectile",
			"EyeballBounceBoltProjectile"
		};
	}

	// Token: 0x17000240 RID: 576
	// (get) Token: 0x060004E7 RID: 1255 RVA: 0x000166DA File Offset: 0x000148DA
	protected virtual float SingleShot_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000241 RID: 577
	// (get) Token: 0x060004E8 RID: 1256 RVA: 0x000166E1 File Offset: 0x000148E1
	protected virtual float SingleShot_TellHold_AnimSpeed
	{
		get
		{
			return 1.75f;
		}
	}

	// Token: 0x17000242 RID: 578
	// (get) Token: 0x060004E9 RID: 1257 RVA: 0x000166E8 File Offset: 0x000148E8
	protected virtual float SingleShot_TellHold_Duration
	{
		get
		{
			return 0.8f;
		}
	}

	// Token: 0x17000243 RID: 579
	// (get) Token: 0x060004EA RID: 1258 RVA: 0x000166EF File Offset: 0x000148EF
	protected virtual float SingleShot_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000244 RID: 580
	// (get) Token: 0x060004EB RID: 1259 RVA: 0x000166F6 File Offset: 0x000148F6
	protected virtual float SingleShot_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000245 RID: 581
	// (get) Token: 0x060004EC RID: 1260 RVA: 0x000166FD File Offset: 0x000148FD
	protected virtual float SingleShot_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000246 RID: 582
	// (get) Token: 0x060004ED RID: 1261 RVA: 0x00016704 File Offset: 0x00014904
	protected virtual float SingleShot_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x060004EE RID: 1262 RVA: 0x0001670B File Offset: 0x0001490B
	protected virtual float SingleShot_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000248 RID: 584
	// (get) Token: 0x060004EF RID: 1263 RVA: 0x00016712 File Offset: 0x00014912
	protected virtual float SingleShot_Exit_Duration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000249 RID: 585
	// (get) Token: 0x060004F0 RID: 1264 RVA: 0x00016719 File Offset: 0x00014919
	protected virtual float SingleShot_Exit_ForceIdle
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x1700024A RID: 586
	// (get) Token: 0x060004F1 RID: 1265 RVA: 0x00016720 File Offset: 0x00014920
	protected virtual float SingleShot_Exit_AttackCD
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x1700024B RID: 587
	// (get) Token: 0x060004F2 RID: 1266 RVA: 0x00016727 File Offset: 0x00014927
	protected virtual float MultiShot_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700024C RID: 588
	// (get) Token: 0x060004F3 RID: 1267 RVA: 0x0001672E File Offset: 0x0001492E
	protected virtual float MultiShot_TellHold_AnimSpeed
	{
		get
		{
			return 1.75f;
		}
	}

	// Token: 0x1700024D RID: 589
	// (get) Token: 0x060004F4 RID: 1268 RVA: 0x00016735 File Offset: 0x00014935
	protected virtual float MultiShot_TellHold_Duration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700024E RID: 590
	// (get) Token: 0x060004F5 RID: 1269 RVA: 0x0001673C File Offset: 0x0001493C
	protected virtual float MultiShot_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700024F RID: 591
	// (get) Token: 0x060004F6 RID: 1270 RVA: 0x00016743 File Offset: 0x00014943
	protected virtual float MultiShot_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x060004F7 RID: 1271 RVA: 0x0001674A File Offset: 0x0001494A
	protected virtual float MultiShot_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x060004F8 RID: 1272 RVA: 0x00016751 File Offset: 0x00014951
	protected virtual float MultiShot_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x060004F9 RID: 1273 RVA: 0x00016758 File Offset: 0x00014958
	protected virtual float MultiShot_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000253 RID: 595
	// (get) Token: 0x060004FA RID: 1274 RVA: 0x0001675F File Offset: 0x0001495F
	protected virtual float MultiShot_Exit_Duration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x060004FB RID: 1275 RVA: 0x00016766 File Offset: 0x00014966
	protected virtual float MultiShot_Exit_ForceIdle
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000255 RID: 597
	// (get) Token: 0x060004FC RID: 1276 RVA: 0x0001676D File Offset: 0x0001496D
	protected virtual float MultiShot_Exit_AttackCD
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x17000256 RID: 598
	// (get) Token: 0x060004FD RID: 1277 RVA: 0x00016774 File Offset: 0x00014974
	protected virtual int MultiShot_Attack_Amount
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x060004FE RID: 1278 RVA: 0x00016777 File Offset: 0x00014977
	protected virtual float MultiShot_AttackHold_DelayBetweenShots
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000258 RID: 600
	// (get) Token: 0x060004FF RID: 1279 RVA: 0x0001677E File Offset: 0x0001497E
	protected virtual float MultiShot_Attack_AngleSpread
	{
		get
		{
			return 75f;
		}
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x00016788 File Offset: 0x00014988
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.EnemyController.FollowTarget = true;
		base.EnemyController.Heading = Vector2.zero;
		this.UpdateEyeball();
		if (enemyController.IsBoss)
		{
			base.LogicController.DisableLogicActivationByDistance = true;
			this.m_faceForward = true;
		}
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x000167DC File Offset: 0x000149DC
	public override void OnEnemyActivated()
	{
		base.OnEnemyActivated();
		if (base.EnemyController.TargetController && !base.EnemyController.IsBoss)
		{
			base.EnemyController.Heading = base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint;
		}
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x0001683E File Offset: 0x00014A3E
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ShootSingleFireball()
	{
		yield return this.Default_TellIntroAndLoop("SingleShot_Tell_Intro", this.SingleShot_TellIntro_AnimSpeed, "SingleShot_Tell_Hold", this.SingleShot_TellHold_AnimSpeed, this.SingleShot_TellHold_Duration);
		yield return this.Default_Animation("SingleShot_Attack_Intro", this.SingleShot_AttackIntro_AnimSpeed, this.SingleShot_AttackIntro_Delay, true);
		base.EnemyController.FollowTarget = false;
		float angle = CDGHelper.VectorToAngle(base.EnemyController.Heading);
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
		{
			this.FireProjectile("EyeballBounceBoltProjectile", 1, false, angle, 1f, true, true, true);
		}
		else
		{
			this.FireProjectile("EyeballBoltProjectile", 1, false, angle, 1f, true, true, true);
		}
		yield return this.Default_Animation("SingleShot_Attack_Hold", this.SingleShot_AttackHold_AnimSpeed, this.SingleShot_AttackHold_Delay, true);
		yield return this.Default_Animation("SingleShot_Exit", this.SingleShot_Exit_AnimSpeed, this.SingleShot_Exit_Duration, true);
		base.EnemyController.FollowTarget = true;
		yield return this.Default_Attack_Cooldown(this.SingleShot_Exit_ForceIdle, this.SingleShot_Exit_AttackCD);
		yield break;
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x0001684D File Offset: 0x00014A4D
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ShootMultiFireball()
	{
		yield return this.Default_TellIntroAndLoop("ContinuousShot_Tell_Intro", this.MultiShot_TellIntro_AnimSpeed, "ContinuousShot_Tell_Hold", this.MultiShot_TellHold_AnimSpeed, this.MultiShot_TellHold_Duration);
		yield return this.Default_Animation("ContinuousShot_Attack_Intro", this.MultiShot_AttackIntro_AnimSpeed, this.MultiShot_AttackIntro_Delay, true);
		base.EnemyController.FollowTarget = false;
		float fireAngle = CDGHelper.VectorToAngle(base.EnemyController.Heading);
		yield return this.ChangeAnimationState("ContinuousShot_Attack_Hold");
		this.SetAnimationSpeedMultiplier(this.MultiShot_AttackHold_AnimSpeed);
		int num;
		for (int i = 0; i < this.MultiShot_Attack_Amount; i = num + 1)
		{
			this.FireProjectile("EyeballBoltProjectile", 0, false, fireAngle, 1f, true, true, true);
			if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
			{
				this.FireProjectile("EyeballBoltProjectile", 1, false, fireAngle + this.MultiShot_Attack_AngleSpread, 1f, true, true, true);
				this.FireProjectile("EyeballBoltProjectile", 1, false, fireAngle - this.MultiShot_Attack_AngleSpread, 1f, true, true, true);
			}
			if (this.MultiShot_AttackHold_DelayBetweenShots > 0f)
			{
				yield return base.Wait(this.MultiShot_AttackHold_DelayBetweenShots, false);
			}
			num = i;
		}
		if (this.MultiShot_AttackHold_Delay > 0f)
		{
			yield return base.Wait(this.MultiShot_AttackHold_Delay, false);
		}
		yield return this.Default_Animation("ContinuousShot_Exit", this.MultiShot_Exit_AnimSpeed, this.MultiShot_Exit_Duration, true);
		base.EnemyController.FollowTarget = true;
		yield return this.Default_Attack_Cooldown(this.MultiShot_Exit_ForceIdle, this.MultiShot_Exit_AttackCD);
		yield break;
	}

	// Token: 0x17000259 RID: 601
	// (get) Token: 0x06000504 RID: 1284 RVA: 0x0001685C File Offset: 0x00014A5C
	protected virtual float ExplodingShot_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700025A RID: 602
	// (get) Token: 0x06000505 RID: 1285 RVA: 0x00016863 File Offset: 0x00014A63
	protected virtual float ExplodingShot_TellHold_AnimSpeed
	{
		get
		{
			return 1.75f;
		}
	}

	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06000506 RID: 1286 RVA: 0x0001686A File Offset: 0x00014A6A
	protected virtual float ExplodingShot_TellHold_Duration
	{
		get
		{
			return 1.35f;
		}
	}

	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06000507 RID: 1287 RVA: 0x00016871 File Offset: 0x00014A71
	protected virtual float ExplodingShot_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700025D RID: 605
	// (get) Token: 0x06000508 RID: 1288 RVA: 0x00016878 File Offset: 0x00014A78
	protected virtual float ExplodingShot_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700025E RID: 606
	// (get) Token: 0x06000509 RID: 1289 RVA: 0x0001687F File Offset: 0x00014A7F
	protected virtual float ExplodingShot_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700025F RID: 607
	// (get) Token: 0x0600050A RID: 1290 RVA: 0x00016886 File Offset: 0x00014A86
	protected virtual float ExplodingShot_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000260 RID: 608
	// (get) Token: 0x0600050B RID: 1291 RVA: 0x0001688D File Offset: 0x00014A8D
	protected virtual float ExplodingShot_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x0600050C RID: 1292 RVA: 0x00016894 File Offset: 0x00014A94
	protected virtual float ExplodingShot_Exit_Duration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x0600050D RID: 1293 RVA: 0x0001689B File Offset: 0x00014A9B
	protected virtual float ExplodingShot_Exit_ForceIdle
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x0600050E RID: 1294 RVA: 0x000168A2 File Offset: 0x00014AA2
	protected virtual float ExplodingShot_Exit_AttackCD
	{
		get
		{
			return 7f;
		}
	}

	// Token: 0x0600050F RID: 1295 RVA: 0x000168A9 File Offset: 0x00014AA9
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ExplodingShot()
	{
		yield return this.Default_TellIntroAndLoop("Explosion_Tell_Intro", this.ExplodingShot_TellIntro_AnimSpeed, "Explosion_Tell_Hold", this.ExplodingShot_TellHold_AnimSpeed, this.ExplodingShot_TellHold_Duration);
		yield return this.Default_Animation("Explosion_Attack_Intro", this.ExplodingShot_AttackIntro_AnimSpeed, this.ExplodingShot_AttackIntro_Delay, true);
		float angle = CDGHelper.VectorToAngle(base.EnemyController.Heading);
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
		{
			this.FireProjectile("EyeballBounceBoltProjectile", 1, false, angle, 1f, true, true, true);
			yield return base.Wait(0.45f, false);
			angle = CDGHelper.VectorToAngle(base.EnemyController.Heading);
			this.FireProjectile("EyeballBounceBoltProjectile", 1, false, angle, 1f, true, true, true);
		}
		else
		{
			this.FireProjectile("EyeballBounceBoltProjectile", 1, false, angle, 1f, true, true, true);
		}
		yield return this.Default_Animation("Explosion_Attack_Hold", this.ExplodingShot_Exit_AnimSpeed, this.ExplodingShot_AttackHold_Delay, true);
		yield return this.Default_Animation("Explosion_Exit", this.ExplodingShot_AttackHold_AnimSpeed, this.ExplodingShot_Exit_Duration, true);
		base.EnemyController.FollowTarget = true;
		yield return this.Default_Attack_Cooldown(this.ExplodingShot_Exit_ForceIdle, this.ExplodingShot_Exit_AttackCD);
		yield break;
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x000168B8 File Offset: 0x00014AB8
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[MinibossEnemy]
	public virtual IEnumerator ShootSpinningFireball()
	{
		yield return null;
		yield break;
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x000168C0 File Offset: 0x00014AC0
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[MinibossEnemy]
	public virtual IEnumerator ShootHomingFireball()
	{
		yield return null;
		yield break;
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x000168C8 File Offset: 0x00014AC8
	private void FixedUpdate()
	{
		if (!base.IsInitialized)
		{
			return;
		}
		if (base.EnemyController.Target)
		{
			this.UpdateEyeball();
		}
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x000168EC File Offset: 0x00014AEC
	private void UpdateEyeball()
	{
		if (!this.m_faceForward)
		{
			base.EnemyController.Animator.SetFloat("LookDirectionX", base.EnemyController.HeadingX);
			base.EnemyController.Animator.SetFloat("LookDirectionY", base.EnemyController.HeadingY);
			return;
		}
		base.EnemyController.Animator.SetFloat("LookDirectionX", 0f);
		base.EnemyController.Animator.SetFloat("LookDirectionY", 0f);
	}

	// Token: 0x040008AB RID: 2219
	protected const string EXPLODING_SHOT_TELL_INTRO = "Explosion_Tell_Intro";

	// Token: 0x040008AC RID: 2220
	protected const string EXPLODING_SHOT_TELL_HOLD = "Explosion_Tell_Hold";

	// Token: 0x040008AD RID: 2221
	protected const string EXPLODING_SHOT_ATTACK_INTRO = "Explosion_Attack_Intro";

	// Token: 0x040008AE RID: 2222
	protected const string EXPLODING_SHOT_ATTACK_HOLD = "Explosion_Attack_Hold";

	// Token: 0x040008AF RID: 2223
	protected const string EXPLODING_SHOT_EXIT = "Explosion_Exit";

	// Token: 0x040008B0 RID: 2224
	protected const string EXPLODING_SHOT_PROJECTILE = "EyeballBounceBoltProjectile";

	// Token: 0x040008B1 RID: 2225
	protected const string EXPLODING_SHOT_EXPERT_PROJECTILE = "EyeballBounceBoltProjectile";

	// Token: 0x040008B2 RID: 2226
	protected bool m_faceForward;
}
