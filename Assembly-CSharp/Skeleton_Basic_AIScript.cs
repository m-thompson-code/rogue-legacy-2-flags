using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x0200011A RID: 282
public class Skeleton_Basic_AIScript : BaseAIScript
{
	// Token: 0x0600089F RID: 2207 RVA: 0x0001CD34 File Offset: 0x0001AF34
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"SkeletonBoneProjectile",
			"SkeletonBoneMinibossSmallProjectile",
			"SkeletonRibProjectile",
			"SkeletonBoneBigSmallProjectile",
			"SkeletonRibMinibossProjectile",
			"SkeletonBossCurseProjectile",
			"SkeletonCannonBallWarningProjectile",
			"SkeletonCannonBallProjectile"
		};
	}

	// Token: 0x1700049C RID: 1180
	// (get) Token: 0x060008A0 RID: 2208 RVA: 0x0001CD8D File Offset: 0x0001AF8D
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x1700049D RID: 1181
	// (get) Token: 0x060008A1 RID: 2209 RVA: 0x0001CD9E File Offset: 0x0001AF9E
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x1700049E RID: 1182
	// (get) Token: 0x060008A2 RID: 2210 RVA: 0x0001CDAF File Offset: 0x0001AFAF
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x1700049F RID: 1183
	// (get) Token: 0x060008A3 RID: 2211 RVA: 0x0001CDC0 File Offset: 0x0001AFC0
	protected virtual bool m_jumpLandingMakesBones
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170004A0 RID: 1184
	// (get) Token: 0x060008A4 RID: 2212 RVA: 0x0001CDC3 File Offset: 0x0001AFC3
	protected virtual bool m_jump_tweak_X
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x0001CDC6 File Offset: 0x0001AFC6
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Throw_Bone_Near_Attack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		yield return this.Single_Attack_Throw_Bone(this.m_BoneNear_Angle);
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_throwBone_Exit_ForceIdle, this.m_throwBone_AttackCD);
		yield break;
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x0001CDD5 File Offset: 0x0001AFD5
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	public IEnumerator Throw_Bone_Far_Attack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		yield return this.Single_Attack_Throw_Bone(this.m_FarBoneAngle);
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_throwBone_Exit_ForceIdle, this.m_throwBone_AttackCD);
		yield break;
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x0001CDE4 File Offset: 0x0001AFE4
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Throw_Rib_Attack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		yield return this.Single_Attack_Throw_Rib();
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_throwRib_Exit_ForceIdle, this.m_throwRib_Exit_AttackCD);
		yield break;
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x0001CDF3 File Offset: 0x0001AFF3
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Jump_Attack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		if (base.EnemyController.EnemyRank == EnemyRank.Miniboss)
		{
			base.EnemyController.DisableOffscreenWarnings = false;
			ProjectileManager.AttachOffscreenIcon(base.EnemyController, true);
		}
		yield return this.Default_Animation("Jump_Tell", this.m_jump_Tell_AnimationSpeed, this.m_jump_Tell_Delay, true);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Single_Action_Jump();
		yield return base.WaitUntilIsGrounded();
		base.SetAttackingWithContactDamage(false, 0.1f);
		base.SetVelocityX(0f, false);
		if (this.m_jumpLandingMakesBones)
		{
			for (int i = 0; i < 2; i++)
			{
				float speedMod = UnityEngine.Random.Range(this.m_miniBoss_Land_ThrowPower.x, this.m_miniBoss_Land_ThrowPower.y);
				int num = UnityEngine.Random.Range((int)this.m_miniBoss_Land_ThrowAngle.x, (int)this.m_miniBoss_Land_ThrowAngle.y);
				this.FireProjectile("SkeletonBoneMinibossSmallProjectile", 1, true, (float)num, speedMod, true, true, true);
				this.FireProjectile("SkeletonBoneMinibossSmallProjectile", 1, true, (float)(180 - num), speedMod, true, true, true);
				this.FireProjectile("SkeletonBoneMinibossSmallProjectile", 2, true, (float)num, speedMod, true, true, true);
				this.FireProjectile("SkeletonBoneMinibossSmallProjectile", 2, true, (float)(180 - num), speedMod, true, true, true);
				this.FireProjectile("SkeletonBoneMinibossSmallProjectile", 2, true, (float)num, speedMod, true, true, true);
				this.FireProjectile("SkeletonBoneMinibossSmallProjectile", 2, true, (float)(180 - num), speedMod, true, true, true);
			}
		}
		base.EnemyController.DisableOffscreenWarnings = true;
		if (this.m_jump_Exit_Delay > 0f)
		{
			yield return base.Wait(this.m_jump_Exit_Delay, false);
		}
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_jump_Exit_ForceIdle, this.m_jump_Exit_AttackCD);
		yield break;
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x0001CE02 File Offset: 0x0001B002
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[AdvancedEnemy]
	[ExpertEnemy]
	public IEnumerator Jump_And_Throw_Bone_Attack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("Jump_Tell", this.m_jump_Tell_AnimationSpeed, this.m_jump_Tell_Delay, true);
		yield return this.Single_Action_Jump();
		float num = UnityEngine.Random.Range(this.m_jumpAndThrowBone_ProjectileSpawnDelay.x, this.m_jumpAndThrowBone_ProjectileSpawnDelay.y);
		if (num > 0f)
		{
			yield return base.Wait(num, false);
		}
		if (UnityEngine.Random.Range(0, 2) == 0)
		{
			yield return this.Single_Attack_Throw_Bone(this.m_BoneNear_Angle);
		}
		else
		{
			yield return this.Single_Attack_Throw_Bone(this.m_FarBoneAngle);
		}
		yield return base.WaitUntilIsGrounded();
		base.SetAttackingWithContactDamage(false, 0.1f);
		if (this.m_jump_Exit_Delay > 0f)
		{
			yield return base.Wait(this.m_jump_Exit_Delay, false);
		}
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_jumpAndThrowBone_Exit_ForceIdle, this.m_jumpAndThrowBone_Exit_AttackCD);
		yield break;
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x0001CE11 File Offset: 0x0001B011
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[AdvancedEnemy]
	[ExpertEnemy]
	public IEnumerator Jump_And_Throw_Rib_Attack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("Jump_Tell", this.m_jump_Tell_AnimationSpeed, this.m_jump_Tell_Delay, true);
		yield return this.Single_Action_Jump();
		float num = UnityEngine.Random.Range(this.m_jumpAndThrowBone_ProjectileSpawnDelay.x, this.m_jumpAndThrowBone_ProjectileSpawnDelay.y);
		if (num > 0f)
		{
			yield return base.Wait(num, false);
		}
		yield return this.Single_Attack_Throw_Rib();
		yield return base.WaitUntilIsGrounded();
		base.SetAttackingWithContactDamage(false, 0.1f);
		if (this.m_jump_Exit_Delay > 0f)
		{
			yield return base.Wait(this.m_jump_Exit_Delay, false);
		}
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_jumpAndThrowBone_Exit_ForceIdle, this.m_jumpAndThrowBone_Exit_AttackCD);
		yield break;
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x0001CE20 File Offset: 0x0001B020
	public IEnumerator Single_Attack_Throw_Bone(int throwAngle)
	{
		yield return this.Default_TellIntroAndLoop("ThrowBone_Tell_Intro", this.m_throwBone_TellIntro_AnimationSpeed, "ThrowBone_Tell_Hold", this.m_throwBone_TellHold_AnimationSpeed, 0.5f);
		yield return this.Default_Animation("ThrowBone_Attack_Intro", this.m_throwBone_AttackIntro_AnimationSpeed, this.m_throwBone_AttackIntro_Delay, true);
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			for (int i = 0; i < this.m_miniBoss_Bone_ThrowAmount; i++)
			{
				int num = 5 + i;
				if (num >= 9)
				{
					num = 5;
				}
				throwAngle = UnityEngine.Random.Range((int)this.m_miniBoss_Bone_ThrowAngle.x, (int)this.m_miniBoss_Bone_ThrowAngle.y);
				this.FireProjectile("SkeletonBoneMinibossSmallProjectile", num, true, (float)throwAngle, 1f, true, true, true);
			}
		}
		else if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
		{
			this.FireProjectile("SkeletonBoneProjectile", 0, true, (float)this.m_FarBoneAngle, 1f, true, true, true);
			this.FireProjectile("SkeletonBoneProjectile", 0, true, (float)this.m_BoneNear_Angle, 1f, true, true, true);
			yield return base.Wait(0.1f, false);
			this.FireProjectile("SkeletonBoneProjectile", 0, true, (float)this.m_FarBoneAngle, 1f, true, true, true);
			this.FireProjectile("SkeletonBoneProjectile", 0, true, (float)this.m_BoneNear_Angle, 1f, true, true, true);
		}
		else
		{
			this.FireProjectile("SkeletonBoneProjectile", 0, true, (float)throwAngle, 1f, true, true, true);
		}
		yield return this.Default_Animation("ThrowBone_Attack_Hold", this.m_throwBone_AttackHold_AnimationSpeed, 0f, false);
		yield return base.Wait(0.6f, false);
		yield return base.WaitUntilIsGrounded();
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("ThrowBone_Exit", this.m_throwBone_Exit_AnimationSpeed, this.m_throwBone_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield break;
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x0001CE36 File Offset: 0x0001B036
	public IEnumerator Single_Attack_Throw_Rib()
	{
		yield return this.Default_TellIntroAndLoop("ChestBone_Tell_Intro", this.m_throwRib_TellIntro_AnimationSpeed, "ChestBone_Tell_Hold", this.m_throwRib_TellHold_AnimationSpeed, 0.75f);
		yield return this.Default_Animation("ChestBone_Attack_Intro", this.m_throwRib_AttackIntro_AnimationSpeed, this.m_throwRib_AttackIntro_Delay, true);
		yield return this.Default_Animation("ChestBone_Attack_Hold", this.m_throwRib_AttackHold_AnimationSpeed, 0f, false);
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			int i = 0;
			while ((float)i < this.m_miniBoss_Rib_ThrowAmount)
			{
				float miniBoss_Rib_ThrowPower = this.m_miniBoss_Rib_ThrowPower;
				UnityEngine.Random.Range(this.m_miniBoss_Rib_ThrowHeight.x, this.m_miniBoss_Rib_ThrowHeight.y);
				this.FireProjectile("SkeletonBossCurseProjectile", 4, true, (float)this.m_throwRib_Angle, miniBoss_Rib_ThrowPower, true, true, true);
				if (this.m_miniBoss_Rib_LoopDelay > 0f)
				{
					yield return base.Wait(this.m_miniBoss_Rib_LoopDelay, false);
				}
				int num = i;
				i = num + 1;
			}
		}
		else
		{
			this.FireProjectile("SkeletonRibProjectile", 4, true, (float)this.m_throwRib_Angle, 1f, true, true, true);
			if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
			{
				this.FireProjectile("SkeletonRibProjectile", 4, true, (float)this.m_throwRib_Angle, this.m_throwRib_secondRibSpeedMod, true, true, true);
			}
		}
		yield return base.Wait(this.m_throwRib_AttackHold_Delay, false);
		yield return base.WaitUntilIsGrounded();
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("ChestBone_Exit", this.m_throwRib_Exit_AnimationSpeed, this.m_throwRib_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield break;
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x0001CE45 File Offset: 0x0001B045
	public IEnumerator Single_Action_Jump()
	{
		float num = this.m_jump_Power.x;
		if (this.m_jump_tweak_X)
		{
			num += UnityEngine.Random.Range(this.m_jump_Tweaker_Add.x, this.m_jump_Tweaker_Add.y);
		}
		if (!base.EnemyController.IsFacingRight)
		{
			base.SetVelocityX(num, true);
		}
		else
		{
			base.SetVelocityX(-num, true);
		}
		base.SetVelocityY(this.m_jump_Power.y, false);
		yield return this.ChangeAnimationState("JumpUp");
		yield return base.Wait(0.05f, false);
		yield break;
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x0001CE54 File Offset: 0x0001B054
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[MinibossEnemy]
	public IEnumerator Simple_Dance_Attack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		yield return this.Default_Animation("Victory", this.m_dance_AttackIntro_AnimationSpeed, this.m_dance_AttackIntro_Delay, false);
		float y = base.EnemyController.Room.Bounds.max.y + 10f;
		float num = 5f;
		Vector2 vector = new Vector2(PlayerManager.GetPlayerController().Midpoint.x, y);
		Vector2 vector4;
		Vector2 vector3;
		Vector2 pos;
		Vector2 vector2 = pos = (vector3 = (vector4 = vector));
		pos.y = y;
		vector2.x += num;
		vector3.x += -num;
		vector4.x += num * 2f;
		vector.x += -num * 2f;
		AudioManager.PlayOneShot(this, "event:/SFX/Enemies/sfx_enemy_skeletonBoss_portal_single", PlayerManager.GetPlayerController().Midpoint);
		this.FireProjectileAbsPos("SkeletonCannonBallWarningProjectile", pos, false, 270f, 1f, true, true, true);
		Projectile_RL projectile_RL = this.FireProjectileAbsPos("SkeletonCannonBallProjectile", pos, true, -90f, 1f, true, true, true);
		AudioManager.PlayOneShotAttached(this, "event:/SFX/Enemies/sfx_enemy_skeletonBoss_small_cannon_projectile_loop", projectile_RL.gameObject);
		yield return base.Wait(0.1f, false);
		yield return base.Wait(this.m_dance_AttackHold_Delay, false);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		base.EnemyController.AlwaysFacing = true;
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Attack_Cooldown(this.m_dance_Exit_ForceIdle, this.m_dance_Exit_AttackCD);
		yield break;
	}

	// Token: 0x170004A1 RID: 1185
	// (get) Token: 0x060008AF RID: 2223 RVA: 0x0001CE63 File Offset: 0x0001B063
	protected virtual float m_modeShift_Damage_Mod
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x0001CE6A File Offset: 0x0001B06A
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Mode_Shift()
	{
		yield return base.DeathAnim();
		base.SetVelocity(0f, 0f, false);
		base.EnemyController.LockFlip = true;
		base.EnemyController.ModeshiftDamageMod = this.m_modeShift_Damage_Mod;
		yield return this.Default_Animation("ModeShift_Intro", this.m_modeShift_Tell_Intro_AnimSpeed, this.m_modeShift_TellIntro_Delay, true);
		yield return this.Default_Animation("ModeShift_Scream_Intro", this.m_modeShift_Attack_Intro_AnimSpeed, this.m_modeShift_Attack_Intro_Delay, true);
		yield return this.Default_Animation("ModeShift_Scream_Hold", this.m_modeShift_Attack_Hold_AnimSpeed, this.m_modeShift_Attack_Hold_Delay, true);
		this.ModeShiftComplete_SpawnSecondBoss = true;
		yield return this.Default_Animation("ModeShift_Scream_Exit", this.m_modeShift_Exit_AnimSpeed, this.m_modeShift_Exit_Delay, true);
		yield return this.Default_Animation(BaseAIScript.NEUTRAL_STATE, 1f, 1f, false);
		base.EnemyController.ModeshiftDamageMod = 1f;
		base.EnemyController.LockFlip = false;
		yield break;
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x0001CE79 File Offset: 0x0001B079
	public override IEnumerator SpawnAnim()
	{
		base.EnemyController.Visuals.SetActive(false);
		yield return base.Wait(1.5f, false);
		yield return this.ChangeAnimationState("Fall");
		Vector3 position = base.EnemyController.transform.position;
		position.y += 18f;
		base.EnemyController.transform.position = position;
		base.EnemyController.Visuals.SetActive(true);
		yield return base.WaitUntilIsGrounded();
		yield return base.Wait(0.5f, false);
		yield return this.Default_Animation("Intro_Idle", this.m_spawn_Idle_AnimSpeed, this.m_spawn_Idle_Delay, true);
		yield return this.Default_Animation("Intro", this.m_spawn_Intro_AnimSpeed, this.m_spawn_Intro_Delay, true);
		yield return this.Default_Animation(BaseAIScript.NEUTRAL_STATE, 1f, 1f, false);
		yield break;
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x0001CE88 File Offset: 0x0001B088
	public override IEnumerator DeathAnim()
	{
		BossRoomController component = base.EnemyController.Room.gameObject.GetComponent<BossRoomController>();
		bool flag;
		if (component)
		{
			flag = component.AllBossesDeadOrDying;
		}
		else
		{
			flag = (EnemyManager.NumActiveEnemies == 0 && EnemyManager.NumActiveSummonedEnemies == 0);
		}
		if (flag)
		{
			yield return base.DeathAnim();
		}
		yield break;
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x0001CE97 File Offset: 0x0001B097
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		this.ModeShiftComplete_SpawnSecondBoss = false;
		if (enemyController.IsBoss)
		{
			base.LogicController.DisableLogicActivationByDistance = true;
		}
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x0001CEBB File Offset: 0x0001B0BB
	public override void OnLBCompleteOrCancelled()
	{
		base.EnemyController.DisableOffscreenWarnings = true;
		base.EnemyController.AlwaysFacing = true;
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x04000C0B RID: 3083
	public UnityEvent_GameObject JumpEvent;

	// Token: 0x04000C0C RID: 3084
	protected float m_throwBone_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x04000C0D RID: 3085
	protected float m_throwBone_TellHold_AnimationSpeed = 1.3f;

	// Token: 0x04000C0E RID: 3086
	protected const float m_throwBone_TellIntroAndHold_Delay = 0.5f;

	// Token: 0x04000C0F RID: 3087
	protected float m_throwBone_AttackIntro_AnimationSpeed = 0.8f;

	// Token: 0x04000C10 RID: 3088
	protected float m_throwBone_AttackIntro_Delay;

	// Token: 0x04000C11 RID: 3089
	protected float m_throwBone_AttackHold_AnimationSpeed = 0.8f;

	// Token: 0x04000C12 RID: 3090
	protected const float m_throwBone_AttackHold_Delay = 0.6f;

	// Token: 0x04000C13 RID: 3091
	protected float m_throwBone_Exit_AnimationSpeed = 0.65f;

	// Token: 0x04000C14 RID: 3092
	protected float m_throwBone_Exit_Delay = 0.15f;

	// Token: 0x04000C15 RID: 3093
	protected int m_BoneNear_Angle = 83;

	// Token: 0x04000C16 RID: 3094
	protected int m_FarBoneAngle = 70;

	// Token: 0x04000C17 RID: 3095
	protected int m_miniBoss_Bone_ThrowAmount = 8;

	// Token: 0x04000C18 RID: 3096
	protected Vector2 m_miniBoss_Bone_ThrowAngle = new Vector2(55f, 85f);

	// Token: 0x04000C19 RID: 3097
	protected float m_throwBone_Exit_ForceIdle = 0.15f;

	// Token: 0x04000C1A RID: 3098
	protected float m_throwBone_AttackCD;

	// Token: 0x04000C1B RID: 3099
	protected float m_throwRib_TellIntro_AnimationSpeed = 0.75f;

	// Token: 0x04000C1C RID: 3100
	protected float m_throwRib_TellHold_AnimationSpeed = 1.35f;

	// Token: 0x04000C1D RID: 3101
	protected const float m_throwRib_TellIntroAndHold_Delay = 0.75f;

	// Token: 0x04000C1E RID: 3102
	protected float m_throwRib_AttackIntro_AnimationSpeed = 1.1f;

	// Token: 0x04000C1F RID: 3103
	protected float m_throwRib_AttackIntro_Delay;

	// Token: 0x04000C20 RID: 3104
	protected float m_throwRib_AttackHold_AnimationSpeed = 1.1f;

	// Token: 0x04000C21 RID: 3105
	protected float m_throwRib_AttackHold_Delay = 1f;

	// Token: 0x04000C22 RID: 3106
	protected float m_throwRib_Exit_AnimationSpeed = 1f;

	// Token: 0x04000C23 RID: 3107
	protected float m_throwRib_Exit_Delay;

	// Token: 0x04000C24 RID: 3108
	protected int m_throwRib_Angle;

	// Token: 0x04000C25 RID: 3109
	protected float m_throwRib_secondRibSpeedMod = 1.35f;

	// Token: 0x04000C26 RID: 3110
	protected float m_throwRib_Exit_ForceIdle = 0.15f;

	// Token: 0x04000C27 RID: 3111
	protected float m_throwRib_Exit_AttackCD = 2f;

	// Token: 0x04000C28 RID: 3112
	protected float m_miniBoss_Rib_ThrowAmount = 1f;

	// Token: 0x04000C29 RID: 3113
	protected float m_miniBoss_Rib_ThrowPower = 1f;

	// Token: 0x04000C2A RID: 3114
	protected Vector2 m_miniBoss_Rib_ThrowHeight = new Vector2(6f, 6f);

	// Token: 0x04000C2B RID: 3115
	protected float m_miniBoss_Rib_LoopDelay = 0.9f;

	// Token: 0x04000C2C RID: 3116
	protected float m_jump_Tell_Delay = 0.4f;

	// Token: 0x04000C2D RID: 3117
	protected float m_jump_Tell_AnimationSpeed = 0.75f;

	// Token: 0x04000C2E RID: 3118
	protected Vector2 m_jump_Power = new Vector2(0f, 27f);

	// Token: 0x04000C2F RID: 3119
	protected int m_miniBoss_Bone_JumpAmount = 5;

	// Token: 0x04000C30 RID: 3120
	protected float m_jump_Exit_Delay;

	// Token: 0x04000C31 RID: 3121
	protected float m_jump_Exit_ForceIdle = 0.15f;

	// Token: 0x04000C32 RID: 3122
	protected float m_jump_Exit_AttackCD;

	// Token: 0x04000C33 RID: 3123
	protected Vector2 m_jumpAndThrowBone_ProjectileSpawnDelay = new Vector2(0f, 0f);

	// Token: 0x04000C34 RID: 3124
	protected float m_jumpAndThrowBone_Exit_ForceIdle = 0.15f;

	// Token: 0x04000C35 RID: 3125
	protected float m_jumpAndThrowBone_Exit_AttackCD = 1.25f;

	// Token: 0x04000C36 RID: 3126
	protected Vector2 m_jump_Tweaker_Add = new Vector2(-12f, 0f);

	// Token: 0x04000C37 RID: 3127
	protected Vector2 m_miniBoss_Land_ThrowAngle = new Vector2(25f, 30f);

	// Token: 0x04000C38 RID: 3128
	protected Vector2 m_miniBoss_Land_ThrowPower = new Vector2(0.75f, 1f);

	// Token: 0x04000C39 RID: 3129
	protected float m_dance_AttackIntro_AnimationSpeed = 1.1f;

	// Token: 0x04000C3A RID: 3130
	protected float m_dance_AttackIntro_Delay = 1f;

	// Token: 0x04000C3B RID: 3131
	protected float m_dance_AttackHold_Delay = 1f;

	// Token: 0x04000C3C RID: 3132
	protected float m_dance_Exit_ForceIdle = 1f;

	// Token: 0x04000C3D RID: 3133
	protected float m_dance_Exit_AttackCD = 5f;

	// Token: 0x04000C3E RID: 3134
	protected const string MODE_SHIFT_TELL_INTRO = "ModeShift_Intro";

	// Token: 0x04000C3F RID: 3135
	protected const string MODE_SHIFT_ATTACK_INTRO = "ModeShift_Scream_Intro";

	// Token: 0x04000C40 RID: 3136
	protected const string MODE_SHIFT_ATTACK_HOLD = "ModeShift_Scream_Hold";

	// Token: 0x04000C41 RID: 3137
	protected const string MODE_SHIFT_EXIT = "ModeShift_Scream_Exit";

	// Token: 0x04000C42 RID: 3138
	protected float m_modeShift_Tell_Intro_AnimSpeed = 1f;

	// Token: 0x04000C43 RID: 3139
	protected float m_modeShift_TellIntro_Delay = 0.5f;

	// Token: 0x04000C44 RID: 3140
	protected float m_modeShift_Attack_Intro_AnimSpeed = 1f;

	// Token: 0x04000C45 RID: 3141
	protected float m_modeShift_Attack_Intro_Delay;

	// Token: 0x04000C46 RID: 3142
	protected float m_modeShift_Attack_Hold_AnimSpeed = 1f;

	// Token: 0x04000C47 RID: 3143
	protected float m_modeShift_Attack_Hold_Delay = 2f;

	// Token: 0x04000C48 RID: 3144
	protected float m_modeShift_Exit_AnimSpeed = 1f;

	// Token: 0x04000C49 RID: 3145
	protected float m_modeShift_Exit_Delay;

	// Token: 0x04000C4A RID: 3146
	[NonSerialized]
	public bool ModeShiftComplete_SpawnSecondBoss;

	// Token: 0x04000C4B RID: 3147
	protected const string SPAWN_IDLE = "Intro_Idle";

	// Token: 0x04000C4C RID: 3148
	protected const string SPAWN_INTRO = "Intro";

	// Token: 0x04000C4D RID: 3149
	protected float m_spawn_Idle_AnimSpeed = 1f;

	// Token: 0x04000C4E RID: 3150
	protected float m_spawn_Idle_Delay;

	// Token: 0x04000C4F RID: 3151
	protected float m_spawn_Intro_AnimSpeed = 1f;

	// Token: 0x04000C50 RID: 3152
	protected float m_spawn_Intro_Delay;

	// Token: 0x04000C51 RID: 3153
	protected const string DEATH_INTRO = "Death_Intro";

	// Token: 0x04000C52 RID: 3154
	protected const string DEATH_HOLD = "Death_Loop";

	// Token: 0x04000C53 RID: 3155
	protected float m_death_Intro_AnimSpeed = 1f;

	// Token: 0x04000C54 RID: 3156
	protected float m_death_Intro_Delay;

	// Token: 0x04000C55 RID: 3157
	protected float m_death_Hold_AnimSpeed = 1f;

	// Token: 0x04000C56 RID: 3158
	protected float m_death_Hold_Delay = 4.5f;
}
