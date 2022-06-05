using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000231 RID: 561
public class TopShotHazard_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000FAE RID: 4014 RVA: 0x00008770 File Offset: 0x00006970
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"TopShotBoltProjectile",
			"TopShotBoltExplosionProjectile",
			"TopShotBoltMinibossProjectile"
		};
	}

	// Token: 0x1700075A RID: 1882
	// (get) Token: 0x06000FAF RID: 4015 RVA: 0x00003CEB File Offset: 0x00001EEB
	protected virtual float m_hookToWall_MaxDistance
	{
		get
		{
			return 20f;
		}
	}

	// Token: 0x1700075B RID: 1883
	// (get) Token: 0x06000FB0 RID: 4016 RVA: 0x000081A4 File Offset: 0x000063A4
	protected virtual float SHOT_TRIGGER_WIDTH
	{
		get
		{
			return 8f;
		}
	}

	// Token: 0x1700075C RID: 1884
	// (get) Token: 0x06000FB1 RID: 4017 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_snapToFloor
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700075D RID: 1885
	// (get) Token: 0x06000FB2 RID: 4018 RVA: 0x00003DEF File Offset: 0x00001FEF
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x1700075E RID: 1886
	// (get) Token: 0x06000FB3 RID: 4019 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x1700075F RID: 1887
	// (get) Token: 0x06000FB4 RID: 4020 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x06000FB5 RID: 4021 RVA: 0x00008796 File Offset: 0x00006996
	private void Awake()
	{
		this.m_onResetPosition = new Action<object, EventArgs>(this.OnResetPosition);
		this.m_onPositionedForSummoning = new Action<object, EventArgs>(this.OnPositionedForSummoning);
	}

	// Token: 0x06000FB6 RID: 4022 RVA: 0x00078980 File Offset: 0x00076B80
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

	// Token: 0x06000FB7 RID: 4023 RVA: 0x000087BC File Offset: 0x000069BC
	private void OnResetPosition(object sender, EventArgs args)
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06000FB8 RID: 4024 RVA: 0x000087CA File Offset: 0x000069CA
	private void OnPositionedForSummoning(object sender, EventArgs args)
	{
		this.HookToWall();
	}

	// Token: 0x06000FB9 RID: 4025 RVA: 0x000087D2 File Offset: 0x000069D2
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		this.HookToWall();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06000FBA RID: 4026 RVA: 0x000087E6 File Offset: 0x000069E6
	protected override void OnDisable()
	{
		base.OnDisable();
		this.m_aimFollowTarget = false;
		this.m_headAimAngle = -90f;
	}

	// Token: 0x06000FBB RID: 4027 RVA: 0x00008800 File Offset: 0x00006A00
	public override void OnEnemyActivated()
	{
		this.m_reaimStartTime = Time.time;
		this.m_aimFollowTarget = true;
		base.OnEnemyActivated();
	}

	// Token: 0x06000FBC RID: 4028 RVA: 0x0000881A File Offset: 0x00006A1A
	public override void Unpause()
	{
		this.m_reaimStartTime = Time.time;
		base.Unpause();
	}

	// Token: 0x06000FBD RID: 4029 RVA: 0x0000882D File Offset: 0x00006A2D
	private void OnDestroy()
	{
		if (base.EnemyController)
		{
			base.EnemyController.OnResetPositionRelay.RemoveListener(this.m_onResetPosition);
			base.EnemyController.OnPositionedForSummoningRelay.RemoveListener(this.m_onPositionedForSummoning);
		}
	}

	// Token: 0x06000FBE RID: 4030 RVA: 0x00078A50 File Offset: 0x00076C50
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

	// Token: 0x06000FBF RID: 4031 RVA: 0x00078CB8 File Offset: 0x00076EB8
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

	// Token: 0x17000760 RID: 1888
	// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_fireBullet_Exit_ForceIdle
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000761 RID: 1889
	// (get) Token: 0x06000FC1 RID: 4033 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float m_fireBullet_Exit_AttackCD
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000762 RID: 1890
	// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_fireBullet_TargetPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000763 RID: 1891
	// (get) Token: 0x06000FC3 RID: 4035 RVA: 0x0000886A File Offset: 0x00006A6A
	protected virtual float m_fireBullet_InitialAngle
	{
		get
		{
			return 90f;
		}
	}

	// Token: 0x17000764 RID: 1892
	// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x00003DA4 File Offset: 0x00001FA4
	protected virtual float m_fireBullet_SpreadAngle
	{
		get
		{
			return 15f;
		}
	}

	// Token: 0x17000765 RID: 1893
	// (get) Token: 0x06000FC5 RID: 4037 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_fireBullet_AdditionalSpreadBullets
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000766 RID: 1894
	// (get) Token: 0x06000FC6 RID: 4038 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_fireBullet_ShotLoop
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000767 RID: 1895
	// (get) Token: 0x06000FC7 RID: 4039 RVA: 0x0000457A File Offset: 0x0000277A
	protected virtual float m_fireBullet_ShotLoopDelay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x06000FC8 RID: 4040 RVA: 0x00008871 File Offset: 0x00006A71
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

	// Token: 0x17000768 RID: 1896
	// (get) Token: 0x06000FC9 RID: 4041 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_spreadShot_Exit_ForceIdle
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000769 RID: 1897
	// (get) Token: 0x06000FCA RID: 4042 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float m_spreadShot_Exit_AttackCD
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x1700076A RID: 1898
	// (get) Token: 0x06000FCB RID: 4043 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_spreadShot_TargetPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700076B RID: 1899
	// (get) Token: 0x06000FCC RID: 4044 RVA: 0x0000886A File Offset: 0x00006A6A
	protected virtual float m_spreadShot_InitialAngle
	{
		get
		{
			return 90f;
		}
	}

	// Token: 0x1700076C RID: 1900
	// (get) Token: 0x06000FCD RID: 4045 RVA: 0x00003DA4 File Offset: 0x00001FA4
	protected virtual float m_spreadShot_SpreadAngle
	{
		get
		{
			return 15f;
		}
	}

	// Token: 0x1700076D RID: 1901
	// (get) Token: 0x06000FCE RID: 4046 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_spreadShot_AdditionalSpreadBullets
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700076E RID: 1902
	// (get) Token: 0x06000FCF RID: 4047 RVA: 0x00004536 File Offset: 0x00002736
	protected virtual float m_spreadShot_ShotLoop
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700076F RID: 1903
	// (get) Token: 0x06000FD0 RID: 4048 RVA: 0x000050CB File Offset: 0x000032CB
	protected virtual float m_spreadShot_ShotLoopDelay
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x06000FD1 RID: 4049 RVA: 0x00008880 File Offset: 0x00006A80
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

	// Token: 0x06000FD2 RID: 4050 RVA: 0x00078DA4 File Offset: 0x00076FA4
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

	// Token: 0x040012B3 RID: 4787
	[SerializeField]
	private bool m_hookToOneWays;

	// Token: 0x040012B4 RID: 4788
	private int m_triggerShotMask;

	// Token: 0x040012B5 RID: 4789
	private bool m_aimFollowTarget = true;

	// Token: 0x040012B6 RID: 4790
	private float m_headAimAngle;

	// Token: 0x040012B7 RID: 4791
	private float m_reaimStartTime;

	// Token: 0x040012B8 RID: 4792
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x040012B9 RID: 4793
	private Action<object, EventArgs> m_onResetPosition;

	// Token: 0x040012BA RID: 4794
	private Action<object, EventArgs> m_onPositionedForSummoning;

	// Token: 0x040012BB RID: 4795
	private const float RANGE_WIDTH = 18f;

	// Token: 0x040012BC RID: 4796
	protected const string SINGLE_SHOT_TELL_INTRO = "SingleShot_Tell_Intro";

	// Token: 0x040012BD RID: 4797
	protected const string SINGLE_SHOT_TELL_HOLD = "SingleShot_Tell_Hold";

	// Token: 0x040012BE RID: 4798
	protected const string SINGLE_SHOT_ATTACK_INTRO = "SingleShot_Attack_Intro";

	// Token: 0x040012BF RID: 4799
	protected const string SINGLE_SHOT_ATTACK_HOLD = "SingleShot_Attack_Hold";

	// Token: 0x040012C0 RID: 4800
	protected const string SINGLE_SHOT_EXIT = "SingleShot_Exit";

	// Token: 0x040012C1 RID: 4801
	protected const string CONTINUOUS_SHOT_TELL_INTRO = "ContinuousShot_Tell_Intro";

	// Token: 0x040012C2 RID: 4802
	protected const string CONTINUOUS_SHOT_TELL_HOLD = "ContinuousShot_Tell_Hold";

	// Token: 0x040012C3 RID: 4803
	protected const string CONTINUOUS_SHOT_ATTACK_INTRO = "ContinuousShot_Attack_Intro";

	// Token: 0x040012C4 RID: 4804
	protected const string CONTINUOUS_SHOT_ATTACK_HOLD = "ContinuousShot_Attack_Hold";

	// Token: 0x040012C5 RID: 4805
	protected const string CONTINUOUS_SHOT_EXIT = "ContinuousShot_Exit";

	// Token: 0x040012C6 RID: 4806
	protected const string SINGLE_SHOT_PROJECTILE = "TopShotBoltProjectile";

	// Token: 0x040012C7 RID: 4807
	protected const string SINGLE_SHOT_PROJECTILE_EXPLOSION = "TopShotBoltExplosionProjectile";

	// Token: 0x040012C8 RID: 4808
	protected const string SINGLE_SHOT_PROJECTILE_MINIBOSS = "TopShotBoltMinibossProjectile";

	// Token: 0x040012C9 RID: 4809
	protected float m_fireBullet_TellIntro_AnimationSpeed = 1f;

	// Token: 0x040012CA RID: 4810
	protected float m_fireBullet_TellHold_AnimationSpeed = 1f;

	// Token: 0x040012CB RID: 4811
	protected const float m_fireBullet_TellIntroAndHold_Delay = 0.6f;

	// Token: 0x040012CC RID: 4812
	protected float m_fireBullet_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x040012CD RID: 4813
	protected float m_fireBullet_AttackIntro_Delay;

	// Token: 0x040012CE RID: 4814
	protected float m_fireBullet_AttackHold_AnimationSpeed = 1f;

	// Token: 0x040012CF RID: 4815
	protected const float m_fireBullet_AttackHold_Delay = 0f;

	// Token: 0x040012D0 RID: 4816
	protected float m_fireBullet_Exit_AnimationSpeed = 0.65f;

	// Token: 0x040012D1 RID: 4817
	protected float m_fireBullet_Exit_Delay = 0.15f;

	// Token: 0x040012D2 RID: 4818
	protected float m_spreadShot_TellIntro_AnimationSpeed = 1f;

	// Token: 0x040012D3 RID: 4819
	protected float m_spreadShot_TellHold_AnimationSpeed = 1f;

	// Token: 0x040012D4 RID: 4820
	protected const float m_spreadShot_TellIntroAndHold_Delay = 0.6f;

	// Token: 0x040012D5 RID: 4821
	protected float m_spreadShot_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x040012D6 RID: 4822
	protected float m_spreadShot_AttackIntro_Delay;

	// Token: 0x040012D7 RID: 4823
	protected float m_spreadShot_AttackHold_AnimationSpeed = 1f;

	// Token: 0x040012D8 RID: 4824
	protected const float m_spreadShot_AttackHold_Delay = 0f;

	// Token: 0x040012D9 RID: 4825
	protected float m_spreadShot_Exit_AnimationSpeed = 0.65f;

	// Token: 0x040012DA RID: 4826
	protected float m_spreadShot_Exit_Delay = 0.15f;
}
