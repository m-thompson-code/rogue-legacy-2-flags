using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000140 RID: 320
public class TopShotHazard_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000A33 RID: 2611 RVA: 0x000200E1 File Offset: 0x0001E2E1
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"TopShotBoltProjectile",
			"TopShotBoltExplosionProjectile",
			"TopShotBoltMinibossProjectile"
		};
	}

	// Token: 0x17000588 RID: 1416
	// (get) Token: 0x06000A34 RID: 2612 RVA: 0x00020107 File Offset: 0x0001E307
	protected virtual float m_hookToWall_MaxDistance
	{
		get
		{
			return 20f;
		}
	}

	// Token: 0x17000589 RID: 1417
	// (get) Token: 0x06000A35 RID: 2613 RVA: 0x0002010E File Offset: 0x0001E30E
	protected virtual float SHOT_TRIGGER_WIDTH
	{
		get
		{
			return 8f;
		}
	}

	// Token: 0x1700058A RID: 1418
	// (get) Token: 0x06000A36 RID: 2614 RVA: 0x00020115 File Offset: 0x0001E315
	protected virtual bool m_snapToFloor
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700058B RID: 1419
	// (get) Token: 0x06000A37 RID: 2615 RVA: 0x00020118 File Offset: 0x0001E318
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x1700058C RID: 1420
	// (get) Token: 0x06000A38 RID: 2616 RVA: 0x00020129 File Offset: 0x0001E329
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x1700058D RID: 1421
	// (get) Token: 0x06000A39 RID: 2617 RVA: 0x0002013A File Offset: 0x0001E33A
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x06000A3A RID: 2618 RVA: 0x0002014B File Offset: 0x0001E34B
	private void Awake()
	{
		this.m_onResetPosition = new Action<object, EventArgs>(this.OnResetPosition);
		this.m_onPositionedForSummoning = new Action<object, EventArgs>(this.OnPositionedForSummoning);
	}

	// Token: 0x06000A3B RID: 2619 RVA: 0x00020174 File Offset: 0x0001E374
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
		base.EnemyController.ControllerCorgi.DefaultParameters.Gravity = 0f;
		base.LogicController.ExecuteLogicInAir = true;
		base.EnemyController.PreserveRotationWhenDeactivated = true;
		base.EnemyController.ForceDisableSummonOffset = true;
		base.LogicController.IsInRange = new Func<float, bool>(this.IsInRange);
		this.HookToWall();
		this.m_triggerShotMask = 256;
		base.EnemyController.OnResetPositionRelay.AddListener(this.m_onResetPosition, false);
		base.EnemyController.OnPositionedForSummoningRelay.AddListener(this.m_onPositionedForSummoning, false);
		this.m_aimFollowTarget = false;
		this.m_headAimAngle = -90f;
	}

	// Token: 0x06000A3C RID: 2620 RVA: 0x00020242 File Offset: 0x0001E442
	private void OnResetPosition(object sender, EventArgs args)
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06000A3D RID: 2621 RVA: 0x00020250 File Offset: 0x0001E450
	private void OnPositionedForSummoning(object sender, EventArgs args)
	{
		this.HookToWall();
	}

	// Token: 0x06000A3E RID: 2622 RVA: 0x00020258 File Offset: 0x0001E458
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		this.HookToWall();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06000A3F RID: 2623 RVA: 0x0002026C File Offset: 0x0001E46C
	protected override void OnDisable()
	{
		base.OnDisable();
		this.m_aimFollowTarget = false;
		this.m_headAimAngle = -90f;
	}

	// Token: 0x06000A40 RID: 2624 RVA: 0x00020286 File Offset: 0x0001E486
	public override void OnEnemyActivated()
	{
		this.m_reaimStartTime = Time.time;
		this.m_aimFollowTarget = true;
		base.OnEnemyActivated();
	}

	// Token: 0x06000A41 RID: 2625 RVA: 0x000202A0 File Offset: 0x0001E4A0
	public override void Unpause()
	{
		this.m_reaimStartTime = Time.time;
		base.Unpause();
	}

	// Token: 0x06000A42 RID: 2626 RVA: 0x000202B3 File Offset: 0x0001E4B3
	private void OnDestroy()
	{
		if (base.EnemyController)
		{
			base.EnemyController.OnResetPositionRelay.RemoveListener(this.m_onResetPosition);
			base.EnemyController.OnPositionedForSummoningRelay.RemoveListener(this.m_onPositionedForSummoning);
		}
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x000202F0 File Offset: 0x0001E4F0
	private void HookToWall()
	{
		LayerMask mask = 256;
		if (this.m_hookToOneWays)
		{
			mask |= 2048;
		}
		Vector2 direction = (!this.m_snapToFloor) ? Vector2.up : Vector2.down;
		RaycastHit2D hit = Physics2D.Raycast(base.EnemyController.Midpoint, direction, this.m_hookToWall_MaxDistance, mask);
		bool flag = hit.collider && hit.collider.CompareTag("Hazard");
		bool flag2 = hit.collider && hit.collider.CompareTag("OneWay");
		base.EnemyController.transform.localEulerAngles = Vector3.zero;
		if (hit && !flag && !flag2)
		{
			Vector3 localEulerAngles = base.EnemyController.Pivot.transform.localEulerAngles;
			localEulerAngles.z = (float)((!this.m_snapToFloor) ? 180 : 0);
			base.EnemyController.Pivot.transform.localEulerAngles = localEulerAngles;
			Vector3 position = base.EnemyController.transform.position;
			position.y = hit.point.y;
			if (!this.m_snapToFloor)
			{
				position.y -= base.EnemyController.VisualBounds.size.y;
			}
			base.EnemyController.transform.position = position;
			float num = 270f - CDGHelper.VectorToAngle(hit.normal);
			Vector3 zero = Vector3.zero;
			zero.z = -num;
			base.EnemyController.transform.localEulerAngles = zero;
		}
		else
		{
			EnemySpawnController enemySpawnController = base.EnemyController.EnemySpawnController;
			if (enemySpawnController && !this.m_snapToFloor)
			{
				enemySpawnController.RemoveListeners();
				base.EnemyController.gameObject.SetActive(false);
				enemySpawnController.Override = false;
				enemySpawnController.SetEnemy(EnemyType.Eyeball, base.EnemyController.EnemyRank, base.EnemyController.Level);
				enemySpawnController.InitializeEnemyInstance();
			}
		}
		Vector3 relativeSpawnPositionAtIndex = base.GetRelativeSpawnPositionAtIndex(2, false);
		relativeSpawnPositionAtIndex.z = -1f;
		base.EnemyController.StatusBarController.gameObject.transform.localPosition = relativeSpawnPositionAtIndex;
		base.EnemyController.ControllerCorgi.SetRaysParameters();
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x00020558 File Offset: 0x0001E758
	private bool IsInRange(float rangeRadius)
	{
		Rect rect = default(Rect);
		rect.x = base.EnemyController.Midpoint.x - 9f;
		rect.y = base.EnemyController.Midpoint.y;
		if (!this.m_snapToFloor)
		{
			rect.y -= rangeRadius;
		}
		rect.width = 18f;
		rect.height = rangeRadius;
		Vector2 vector = PlayerManager.GetPlayerController().Midpoint;
		if (rect.Contains(vector))
		{
			float distance = CDGHelper.DistanceBetweenPts(base.EnemyController.Midpoint, vector);
			Vector2 direction = CDGHelper.VectorBetweenPts(base.EnemyController.Midpoint, vector);
			direction.Normalize();
			if (!Physics2D.Raycast(base.EnemyController.Midpoint, direction, distance, this.m_triggerShotMask))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x1700058E RID: 1422
	// (get) Token: 0x06000A45 RID: 2629 RVA: 0x00020642 File Offset: 0x0001E842
	protected virtual float m_fireBullet_Exit_ForceIdle
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700058F RID: 1423
	// (get) Token: 0x06000A46 RID: 2630 RVA: 0x00020649 File Offset: 0x0001E849
	protected virtual float m_fireBullet_Exit_AttackCD
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000590 RID: 1424
	// (get) Token: 0x06000A47 RID: 2631 RVA: 0x00020650 File Offset: 0x0001E850
	protected virtual bool m_fireBullet_TargetPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000591 RID: 1425
	// (get) Token: 0x06000A48 RID: 2632 RVA: 0x00020653 File Offset: 0x0001E853
	protected virtual float m_fireBullet_InitialAngle
	{
		get
		{
			return 90f;
		}
	}

	// Token: 0x17000592 RID: 1426
	// (get) Token: 0x06000A49 RID: 2633 RVA: 0x0002065A File Offset: 0x0001E85A
	protected virtual float m_fireBullet_SpreadAngle
	{
		get
		{
			return 15f;
		}
	}

	// Token: 0x17000593 RID: 1427
	// (get) Token: 0x06000A4A RID: 2634 RVA: 0x00020661 File Offset: 0x0001E861
	protected virtual float m_fireBullet_AdditionalSpreadBullets
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000594 RID: 1428
	// (get) Token: 0x06000A4B RID: 2635 RVA: 0x00020668 File Offset: 0x0001E868
	protected virtual float m_fireBullet_ShotLoop
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000595 RID: 1429
	// (get) Token: 0x06000A4C RID: 2636 RVA: 0x0002066F File Offset: 0x0001E86F
	protected virtual float m_fireBullet_ShotLoopDelay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x06000A4D RID: 2637 RVA: 0x00020676 File Offset: 0x0001E876
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[WanderLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ShootFireball()
	{
		string tellIntroName = "SingleShot_Tell_Intro";
		string tellHoldName = "SingleShot_Tell_Hold";
		string attackIntro = "SingleShot_Attack_Intro";
		string attackHold = "SingleShot_Attack_Hold";
		string attackExit = "SingleShot_Exit";
		if (this.m_fireBullet_ShotLoop > 1f)
		{
			tellIntroName = "ContinuousShot_Tell_Intro";
			tellHoldName = "ContinuousShot_Tell_Hold";
			attackIntro = "ContinuousShot_Attack_Intro";
			attackHold = "ContinuousShot_Attack_Hold";
			attackExit = "ContinuousShot_Exit";
		}
		yield return this.Default_TellIntroAndLoop(tellIntroName, this.m_fireBullet_TellIntro_AnimationSpeed, tellHoldName, this.m_fireBullet_TellHold_AnimationSpeed, 0.6f);
		yield return this.Default_Animation(attackIntro, this.m_fireBullet_AttackIntro_AnimationSpeed, this.m_fireBullet_AttackIntro_Delay, true);
		yield return this.Default_Animation(attackHold, this.m_fireBullet_AttackHold_AnimationSpeed, 0f, false);
		float initialFireAngle;
		if (this.m_fireBullet_TargetPlayer)
		{
			initialFireAngle = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint);
		}
		else
		{
			initialFireAngle = this.m_fireBullet_InitialAngle;
		}
		if (base.EnemyController.Pivot.transform.localEulerAngles.z == 180f && !this.m_fireBullet_TargetPlayer)
		{
			initialFireAngle = -initialFireAngle;
		}
		int projectileSpawnPoint = 1;
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			projectileSpawnPoint = 0;
		}
		int i = 0;
		while ((float)i < this.m_fireBullet_ShotLoop)
		{
			EffectManager.PlayEffect(base.EnemyController.gameObject, base.Animator, "PoisonProjectileHit_Effect", base.EnemyController.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None).transform.localEulerAngles = Vector3.forward * (-90f + this.m_headAimAngle);
			if (this.m_fireBullet_TargetPlayer)
			{
				initialFireAngle = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint);
			}
			Vector2 vector = base.GetAbsoluteSpawnPositionAtIndex(projectileSpawnPoint, false);
			vector = CDGHelper.RotatedPoint(vector, base.EnemyController.Midpoint, 90f + this.m_headAimAngle);
			if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
			{
				this.FireProjectileAbsPos("TopShotBoltMinibossProjectile", vector, false, initialFireAngle, 1f, true, true, true);
			}
			else
			{
				this.FireProjectileAbsPos("TopShotBoltProjectile", vector, false, initialFireAngle, 1f, true, true, true);
			}
			int num = 1;
			while ((float)num < this.m_fireBullet_AdditionalSpreadBullets + 1f)
			{
				if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
				{
					this.FireProjectileAbsPos("TopShotBoltMinibossProjectile", vector, false, initialFireAngle + this.m_fireBullet_SpreadAngle * (float)num, 1f, true, true, true);
					this.FireProjectileAbsPos("TopShotBoltMinibossProjectile", vector, false, initialFireAngle - this.m_fireBullet_SpreadAngle * (float)num, 1f, true, true, true);
				}
				else
				{
					this.FireProjectileAbsPos("TopShotBoltProjectile", vector, false, initialFireAngle + this.m_fireBullet_SpreadAngle * (float)num, 1f, true, true, true);
					this.FireProjectileAbsPos("TopShotBoltProjectile", vector, false, initialFireAngle - this.m_fireBullet_SpreadAngle * (float)num, 1f, true, true, true);
				}
				num++;
			}
			if (this.m_fireBullet_ShotLoopDelay > 0f)
			{
				yield return base.Wait(this.m_fireBullet_ShotLoopDelay, false);
			}
			int num2 = i;
			i = num2 + 1;
		}
		yield return this.Default_Animation(attackExit, this.m_fireBullet_Exit_AnimationSpeed, this.m_fireBullet_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_fireBullet_Exit_ForceIdle, this.m_fireBullet_Exit_AttackCD);
		yield break;
	}

	// Token: 0x17000596 RID: 1430
	// (get) Token: 0x06000A4E RID: 2638 RVA: 0x00020685 File Offset: 0x0001E885
	protected virtual float m_spreadShot_Exit_ForceIdle
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000597 RID: 1431
	// (get) Token: 0x06000A4F RID: 2639 RVA: 0x0002068C File Offset: 0x0001E88C
	protected virtual float m_spreadShot_Exit_AttackCD
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000598 RID: 1432
	// (get) Token: 0x06000A50 RID: 2640 RVA: 0x00020693 File Offset: 0x0001E893
	protected virtual bool m_spreadShot_TargetPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000599 RID: 1433
	// (get) Token: 0x06000A51 RID: 2641 RVA: 0x00020696 File Offset: 0x0001E896
	protected virtual float m_spreadShot_InitialAngle
	{
		get
		{
			return 90f;
		}
	}

	// Token: 0x1700059A RID: 1434
	// (get) Token: 0x06000A52 RID: 2642 RVA: 0x0002069D File Offset: 0x0001E89D
	protected virtual float m_spreadShot_SpreadAngle
	{
		get
		{
			return 15f;
		}
	}

	// Token: 0x1700059B RID: 1435
	// (get) Token: 0x06000A53 RID: 2643 RVA: 0x000206A4 File Offset: 0x0001E8A4
	protected virtual float m_spreadShot_AdditionalSpreadBullets
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700059C RID: 1436
	// (get) Token: 0x06000A54 RID: 2644 RVA: 0x000206AB File Offset: 0x0001E8AB
	protected virtual float m_spreadShot_ShotLoop
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700059D RID: 1437
	// (get) Token: 0x06000A55 RID: 2645 RVA: 0x000206B2 File Offset: 0x0001E8B2
	protected virtual float m_spreadShot_ShotLoopDelay
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x06000A56 RID: 2646 RVA: 0x000206B9 File Offset: 0x0001E8B9
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[WanderLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator SpreadShot()
	{
		string tellIntroName = "SingleShot_Tell_Intro";
		string tellHoldName = "SingleShot_Tell_Hold";
		string attackIntro = "SingleShot_Attack_Intro";
		string attackHold = "SingleShot_Attack_Hold";
		string attackExit = "SingleShot_Exit";
		if (this.m_spreadShot_ShotLoop > 1f)
		{
			tellIntroName = "ContinuousShot_Tell_Intro";
			tellHoldName = "ContinuousShot_Tell_Hold";
			attackIntro = "ContinuousShot_Attack_Intro";
			attackHold = "ContinuousShot_Attack_Hold";
			attackExit = "ContinuousShot_Exit";
		}
		yield return this.Default_TellIntroAndLoop(tellIntroName, this.m_spreadShot_TellIntro_AnimationSpeed, tellHoldName, this.m_spreadShot_TellHold_AnimationSpeed, 0.6f);
		yield return this.Default_Animation(attackIntro, this.m_spreadShot_AttackIntro_AnimationSpeed, this.m_spreadShot_AttackIntro_Delay, true);
		yield return this.Default_Animation(attackHold, this.m_spreadShot_AttackHold_AnimationSpeed, 0f, false);
		float initialFireAngle;
		if (this.m_spreadShot_TargetPlayer)
		{
			initialFireAngle = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint);
		}
		else
		{
			initialFireAngle = this.m_spreadShot_InitialAngle;
		}
		if (base.EnemyController.Pivot.transform.localEulerAngles.z == 180f && !this.m_spreadShot_TargetPlayer)
		{
			initialFireAngle = -initialFireAngle;
		}
		int projectileSpawnPoint = 1;
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			projectileSpawnPoint = 0;
		}
		int i = 0;
		while ((float)i < this.m_spreadShot_ShotLoop)
		{
			EffectManager.PlayEffect(base.EnemyController.gameObject, base.Animator, "PoisonProjectileHit_Effect", base.EnemyController.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None).transform.localEulerAngles = Vector3.forward * (-90f + this.m_headAimAngle);
			if (this.m_spreadShot_TargetPlayer)
			{
				initialFireAngle = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint);
			}
			Vector2 vector = base.GetAbsoluteSpawnPositionAtIndex(projectileSpawnPoint, false);
			vector = CDGHelper.RotatedPoint(vector, base.EnemyController.Midpoint, 90f + this.m_headAimAngle);
			if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
			{
				this.FireProjectileAbsPos("TopShotBoltMinibossProjectile", vector, false, initialFireAngle, 1f, true, true, true);
			}
			else
			{
				this.FireProjectileAbsPos("TopShotBoltProjectile", vector, false, initialFireAngle, 1f, true, true, true);
			}
			int num = 0;
			while ((float)num < this.m_spreadShot_AdditionalSpreadBullets)
			{
				if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
				{
					this.FireProjectileAbsPos("TopShotBoltMinibossProjectile", vector, false, initialFireAngle + this.m_spreadShot_SpreadAngle * (float)num, 1f, true, true, true);
					this.FireProjectileAbsPos("TopShotBoltMinibossProjectile", vector, false, initialFireAngle - this.m_spreadShot_SpreadAngle * (float)num, 1f, true, true, true);
				}
				else
				{
					this.FireProjectileAbsPos("TopShotBoltProjectile", vector, false, initialFireAngle + this.m_spreadShot_SpreadAngle * (float)num, 1f, true, true, true);
					this.FireProjectileAbsPos("TopShotBoltProjectile", vector, false, initialFireAngle - this.m_spreadShot_SpreadAngle * (float)num, 1f, true, true, true);
				}
				num++;
			}
			if (this.m_spreadShot_ShotLoopDelay > 0f)
			{
				yield return base.Wait(this.m_spreadShot_ShotLoopDelay, false);
			}
			int num2 = i;
			i = num2 + 1;
		}
		yield return this.Default_Animation(attackExit, this.m_spreadShot_Exit_AnimationSpeed, this.m_spreadShot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_spreadShot_Exit_ForceIdle, this.m_spreadShot_Exit_AttackCD);
		yield break;
	}

	// Token: 0x06000A57 RID: 2647 RVA: 0x000206C8 File Offset: 0x0001E8C8
	private void FixedUpdate()
	{
		if (!base.IsInitialized)
		{
			return;
		}
		if (base.IsPaused)
		{
			return;
		}
		if (this.m_aimFollowTarget && base.EnemyController)
		{
			float num = CDGHelper.WrapAngleDegrees(CDGHelper.AngleBetweenPts(base.EnemyController.Midpoint, base.EnemyController.TargetController.Midpoint), true);
			float num2 = 1f;
			float num3 = this.m_reaimStartTime + num2;
			if (Time.time < num3)
			{
				this.m_headAimAngle = Mathf.Abs(Mathf.LerpAngle(this.m_headAimAngle, num, 1f - (num3 - Time.time / num2))) * -1f;
			}
			else
			{
				this.m_headAimAngle = num;
			}
			this.m_headAimAngle = Mathf.Clamp(this.m_headAimAngle, -135f, -45f);
			float t = (this.m_headAimAngle + 45f) / -90f;
			float value = Mathf.Lerp(1f, -1f, t);
			base.EnemyController.Animator.SetFloat("LookAngle", value);
		}
	}

	// Token: 0x04000EAC RID: 3756
	[SerializeField]
	private bool m_hookToOneWays;

	// Token: 0x04000EAD RID: 3757
	private int m_triggerShotMask;

	// Token: 0x04000EAE RID: 3758
	private bool m_aimFollowTarget = true;

	// Token: 0x04000EAF RID: 3759
	private float m_headAimAngle;

	// Token: 0x04000EB0 RID: 3760
	private float m_reaimStartTime;

	// Token: 0x04000EB1 RID: 3761
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x04000EB2 RID: 3762
	private Action<object, EventArgs> m_onResetPosition;

	// Token: 0x04000EB3 RID: 3763
	private Action<object, EventArgs> m_onPositionedForSummoning;

	// Token: 0x04000EB4 RID: 3764
	private const float RANGE_WIDTH = 18f;

	// Token: 0x04000EB5 RID: 3765
	protected const string SINGLE_SHOT_TELL_INTRO = "SingleShot_Tell_Intro";

	// Token: 0x04000EB6 RID: 3766
	protected const string SINGLE_SHOT_TELL_HOLD = "SingleShot_Tell_Hold";

	// Token: 0x04000EB7 RID: 3767
	protected const string SINGLE_SHOT_ATTACK_INTRO = "SingleShot_Attack_Intro";

	// Token: 0x04000EB8 RID: 3768
	protected const string SINGLE_SHOT_ATTACK_HOLD = "SingleShot_Attack_Hold";

	// Token: 0x04000EB9 RID: 3769
	protected const string SINGLE_SHOT_EXIT = "SingleShot_Exit";

	// Token: 0x04000EBA RID: 3770
	protected const string CONTINUOUS_SHOT_TELL_INTRO = "ContinuousShot_Tell_Intro";

	// Token: 0x04000EBB RID: 3771
	protected const string CONTINUOUS_SHOT_TELL_HOLD = "ContinuousShot_Tell_Hold";

	// Token: 0x04000EBC RID: 3772
	protected const string CONTINUOUS_SHOT_ATTACK_INTRO = "ContinuousShot_Attack_Intro";

	// Token: 0x04000EBD RID: 3773
	protected const string CONTINUOUS_SHOT_ATTACK_HOLD = "ContinuousShot_Attack_Hold";

	// Token: 0x04000EBE RID: 3774
	protected const string CONTINUOUS_SHOT_EXIT = "ContinuousShot_Exit";

	// Token: 0x04000EBF RID: 3775
	protected const string SINGLE_SHOT_PROJECTILE = "TopShotBoltProjectile";

	// Token: 0x04000EC0 RID: 3776
	protected const string SINGLE_SHOT_PROJECTILE_EXPLOSION = "TopShotBoltExplosionProjectile";

	// Token: 0x04000EC1 RID: 3777
	protected const string SINGLE_SHOT_PROJECTILE_MINIBOSS = "TopShotBoltMinibossProjectile";

	// Token: 0x04000EC2 RID: 3778
	protected float m_fireBullet_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000EC3 RID: 3779
	protected float m_fireBullet_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000EC4 RID: 3780
	protected const float m_fireBullet_TellIntroAndHold_Delay = 0.6f;

	// Token: 0x04000EC5 RID: 3781
	protected float m_fireBullet_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000EC6 RID: 3782
	protected float m_fireBullet_AttackIntro_Delay;

	// Token: 0x04000EC7 RID: 3783
	protected float m_fireBullet_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000EC8 RID: 3784
	protected const float m_fireBullet_AttackHold_Delay = 0f;

	// Token: 0x04000EC9 RID: 3785
	protected float m_fireBullet_Exit_AnimationSpeed = 0.65f;

	// Token: 0x04000ECA RID: 3786
	protected float m_fireBullet_Exit_Delay = 0.15f;

	// Token: 0x04000ECB RID: 3787
	protected float m_spreadShot_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000ECC RID: 3788
	protected float m_spreadShot_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000ECD RID: 3789
	protected const float m_spreadShot_TellIntroAndHold_Delay = 0.6f;

	// Token: 0x04000ECE RID: 3790
	protected float m_spreadShot_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000ECF RID: 3791
	protected float m_spreadShot_AttackIntro_Delay;

	// Token: 0x04000ED0 RID: 3792
	protected float m_spreadShot_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000ED1 RID: 3793
	protected const float m_spreadShot_AttackHold_Delay = 0f;

	// Token: 0x04000ED2 RID: 3794
	protected float m_spreadShot_Exit_AnimationSpeed = 0.65f;

	// Token: 0x04000ED3 RID: 3795
	protected float m_spreadShot_Exit_Delay = 0.15f;
}
