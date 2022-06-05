using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x02000095 RID: 149
public class Blobfish_Basic_AIScript : BaseAIScript
{
	// Token: 0x0600025A RID: 602 RVA: 0x00012B6C File Offset: 0x00010D6C
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"BlobFishWaterProjectile",
			"BlobFishTailProjectile",
			"BlobFishTailLongProjectile",
			"BlobFishWaterSlamProjectile"
		};
	}

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x0600025B RID: 603 RVA: 0x00012B9A File Offset: 0x00010D9A
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x0600025C RID: 604 RVA: 0x00012BAB File Offset: 0x00010DAB
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x0600025D RID: 605 RVA: 0x00012BBC File Offset: 0x00010DBC
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x0600025E RID: 606 RVA: 0x00012BCD File Offset: 0x00010DCD
	protected virtual bool HasTail
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600025F RID: 607 RVA: 0x00012BD0 File Offset: 0x00010DD0
	private void Awake()
	{
		this.m_onHitReenableGravity = new Action<object, CharacterHitEventArgs>(this.OnHitReenableGravity);
	}

	// Token: 0x06000260 RID: 608 RVA: 0x00012BE4 File Offset: 0x00010DE4
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		this.HideAimIndicator();
	}

	// Token: 0x06000261 RID: 609 RVA: 0x00012BF3 File Offset: 0x00010DF3
	protected override void OnDisable()
	{
		base.OnDisable();
		this.HideAimIndicator();
	}

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x06000262 RID: 610 RVA: 0x00012C01 File Offset: 0x00010E01
	protected virtual float m_jump_Tell_Delay
	{
		get
		{
			return 0.3f;
		}
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x06000263 RID: 611 RVA: 0x00012C08 File Offset: 0x00010E08
	protected virtual float m_jump_Tell_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x06000264 RID: 612 RVA: 0x00012C0F File Offset: 0x00010E0F
	protected virtual float m_jump_Exit_ForceIdle
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x06000265 RID: 613 RVA: 0x00012C16 File Offset: 0x00010E16
	protected virtual bool m_jumpLandingShootsProjectiles
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x06000266 RID: 614 RVA: 0x00012C19 File Offset: 0x00010E19
	protected virtual bool m_jumpLandingShootsManyProjectiles
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x06000267 RID: 615 RVA: 0x00012C1C File Offset: 0x00010E1C
	protected virtual int m_jumpLandingProjectileAngle
	{
		get
		{
			return 80;
		}
	}

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x06000268 RID: 616 RVA: 0x00012C20 File Offset: 0x00010E20
	protected virtual int m_jumpLandingProjectileAngleMany
	{
		get
		{
			return 70;
		}
	}

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x06000269 RID: 617 RVA: 0x00012C24 File Offset: 0x00010E24
	protected virtual float m_jumpProjectileFireDelay
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x0600026A RID: 618 RVA: 0x00012C2B File Offset: 0x00010E2B
	protected virtual float m_aim_Indicator_Offset
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x0600026B RID: 619 RVA: 0x00012C32 File Offset: 0x00010E32
	protected virtual Vector2Int m_jump_Attack_Min_And_Max_Angle
	{
		get
		{
			return new Vector2Int(65, 125);
		}
	}

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x0600026C RID: 620 RVA: 0x00012C3D File Offset: 0x00010E3D
	protected virtual float m_jump_Power
	{
		get
		{
			return 27.5f;
		}
	}

	// Token: 0x0600026D RID: 621 RVA: 0x00012C44 File Offset: 0x00010E44
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Jump_Attack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		this.SetRandomJumpDirection();
		base.Animator.SetBool("BellyFlop", false);
		yield return this.Default_Animation("Jump_Tell_Intro", this.m_jump_Tell_AnimationSpeed, this.m_jump_Tell_Delay, true);
		yield return this.Default_Animation("Jump_Tell_Hold", this.m_jump_Tell_AnimationSpeed, this.m_jump_Tell_Delay, true);
		base.SetAttackingWithContactDamage(true, 0.1f);
		this.Jump(this.m_jump_Direction.x, this.m_jump_Direction.y);
		this.HideAimIndicator();
		yield return base.Wait(0.05f, false);
		if (this.HasTail)
		{
			this.RunPersistentCoroutine(this.FireJumpProjectiles(this.m_jumpProjectileFireDelay));
		}
		yield return this.Default_Animation("Jump_Attack_Intro", this.m_jump_Attack_Intro_AnimationSpeed, this.m_jump_Attack_Intro_Delay, true);
		yield return this.Default_Animation("Jump_Attack_Hold", this.m_jump_Attack_Hold_AnimationSpeed, this.m_jump_Attack_Hold_Delay, true);
		yield return base.Wait(0.05f, false);
		yield return base.WaitUntilIsGrounded();
		base.SetVelocity(Vector2.zero, false);
		base.EnemyController.JumpHorizontalVelocity = 0f;
		if (this.m_jumpLandingShootsProjectiles)
		{
			this.FireProjectile("BlobFishWaterProjectile", 2, true, (float)this.m_jumpLandingProjectileAngle, 1f, true, true, true);
			this.FireProjectile("BlobFishWaterProjectile", 1, true, (float)(180 - this.m_jumpLandingProjectileAngle), 1f, true, true, true);
			if (this.m_jumpLandingShootsManyProjectiles)
			{
				this.FireProjectile("BlobFishWaterProjectile", 2, true, (float)this.m_jumpLandingProjectileAngleMany, 1f, true, true, true);
				this.FireProjectile("BlobFishWaterProjectile", 1, true, (float)(180 - this.m_jumpLandingProjectileAngleMany), 1f, true, true, true);
			}
		}
		yield return this.Default_Animation("Jump_Exit", this.m_jump_Exit_AnimationSpeed, this.m_jump_Exit_Delay, true);
		base.SetAttackingWithContactDamage(false, 0.1f);
		if (this.m_jump_Exit_Delay > 0f)
		{
			base.Wait(this.m_jump_Exit_Delay, false);
		}
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_jump_Exit_ForceIdle, this.m_jump_Exit_AttackCD);
		yield break;
	}

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x0600026E RID: 622 RVA: 0x00012C53 File Offset: 0x00010E53
	protected virtual float GroundSlam_Jump_Power
	{
		get
		{
			return 27.5f;
		}
	}

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x0600026F RID: 623 RVA: 0x00012C5A File Offset: 0x00010E5A
	protected virtual float GroundSlam_Land_Power
	{
		get
		{
			return -50f;
		}
	}

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x06000270 RID: 624 RVA: 0x00012C61 File Offset: 0x00010E61
	protected virtual float GroundSlam_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x06000271 RID: 625 RVA: 0x00012C68 File Offset: 0x00010E68
	protected virtual float GroundSlam_TellHold_AnimSpeed
	{
		get
		{
			return 1.75f;
		}
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x06000272 RID: 626 RVA: 0x00012C6F File Offset: 0x00010E6F
	protected virtual float GroundSlam_TellHold_Duration
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x06000273 RID: 627 RVA: 0x00012C76 File Offset: 0x00010E76
	protected virtual float GroundSlam_AttackIntro_AnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x06000274 RID: 628 RVA: 0x00012C7D File Offset: 0x00010E7D
	protected virtual float GroundSlam_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x06000275 RID: 629 RVA: 0x00012C84 File Offset: 0x00010E84
	protected virtual float GroundSlam_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x06000276 RID: 630 RVA: 0x00012C8B File Offset: 0x00010E8B
	protected virtual float GroundSlam_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x06000277 RID: 631 RVA: 0x00012C92 File Offset: 0x00010E92
	protected virtual float GroundSlam_Exit_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x06000278 RID: 632 RVA: 0x00012C99 File Offset: 0x00010E99
	protected virtual float GroundSlam_Exit_Duration
	{
		get
		{
			return 0.2f;
		}
	}

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x06000279 RID: 633 RVA: 0x00012CA0 File Offset: 0x00010EA0
	protected virtual float GroundSlam_Exit_ForceIdle
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x0600027A RID: 634 RVA: 0x00012CA7 File Offset: 0x00010EA7
	protected virtual float GroundSlam_Exit_AttackCD
	{
		get
		{
			return 2.75f;
		}
	}

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x0600027B RID: 635 RVA: 0x00012CAE File Offset: 0x00010EAE
	protected float GroundSlam_Land_Delay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x0600027C RID: 636 RVA: 0x00012CB5 File Offset: 0x00010EB5
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Ground_Slam()
	{
		base.EnemyController.AlwaysFacing = false;
		base.SetVelocityX(0f, false);
		float totalDuration = this.GroundSlam_TellHold_Duration * 0.3f;
		float blinkDuration = this.GroundSlam_TellHold_Duration * 0.7f;
		base.Animator.SetBool("BellyFlop", true);
		yield return this.Default_TellIntroAndLoop("Jump_Tell_Intro", this.GroundSlam_TellIntro_AnimSpeed, "Jump_Tell_Hold", this.GroundSlam_TellHold_AnimSpeed, totalDuration);
		Color red = Color.red;
		red.a = 0.5f;
		this.m_groundSlamBlinkCoroutine = this.RunPersistentCoroutine(this.GroundSlamBlinkCoroutine(blinkDuration, red));
		yield return base.Wait(blinkDuration, false);
		base.SetAttackingWithContactDamage(true, 0.1f);
		this.Jump(0f, this.GroundSlam_Jump_Power);
		yield return base.Wait(0.05f, false);
		if (this.HasTail)
		{
			this.RunPersistentCoroutine(this.FireJumpProjectiles(this.m_jumpProjectileFireDelay));
		}
		yield return this.Default_Animation("Jump_Attack_Intro", 1f, 0f, true);
		yield return this.Default_Animation("Jump_Attack_Hold", 1f, 0f, true);
		yield return base.Wait(0.05f, false);
		while (base.EnemyController.Velocity.y > 0f)
		{
			yield return null;
		}
		base.EnemyController.SetVelocity(0f, 0f, false);
		base.EnemyController.ControllerCorgi.GravityActive(false);
		base.EnemyController.CharacterHitResponse.OnCharacterHitRelay.AddListener(this.m_onHitReenableGravity, false);
		yield return this.Default_Animation("Flop_Attack_Intro", this.GroundSlam_AttackIntro_AnimSpeed, this.GroundSlam_AttackIntro_Delay, true);
		yield return this.Default_Animation("Flop_Attack_Hold", this.GroundSlam_AttackHold_AnimSpeed, this.GroundSlam_AttackHold_Delay, false);
		yield return base.Wait(this.GroundSlam_Land_Delay, false);
		base.EnemyController.ControllerCorgi.GravityActive(true);
		base.SetVelocityY(this.GroundSlam_Land_Power, false);
		base.EnemyController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onHitReenableGravity);
		yield return base.WaitUntilIsGrounded();
		this.StopPersistentCoroutine(this.m_groundSlamBlinkCoroutine);
		base.SetVelocity(Vector2.zero, false);
		base.EnemyController.JumpHorizontalVelocity = 0f;
		for (int i = 0; i < 2; i++)
		{
			float speedMod = UnityEngine.Random.Range(this.GroundSlam_Land_ThrowPower.x, this.GroundSlam_Land_ThrowPower.y);
			speedMod = 1f;
			int num = UnityEngine.Random.Range((int)this.GroundSlam_Land_ThrowAngle.x, (int)this.GroundSlam_Land_ThrowAngle.y);
			this.FireProjectile("BlobFishWaterSlamProjectile", 5, false, (float)num, speedMod, true, true, true);
			this.FireProjectile("BlobFishWaterSlamProjectile", 5, false, (float)(180 - num), speedMod, true, true, true);
			this.FireProjectile("BlobFishWaterSlamProjectile", 6, false, (float)num, speedMod, true, true, true);
			this.FireProjectile("BlobFishWaterSlamProjectile", 6, false, (float)(180 - num), speedMod, true, true, true);
		}
		yield return this.Default_Animation("Flop_Exit", this.GroundSlam_Exit_AnimSpeed, this.GroundSlam_Exit_Duration, true);
		base.SetAttackingWithContactDamage(false, 0.1f);
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.GroundSlam_Exit_ForceIdle, this.GroundSlam_Exit_AttackCD);
		yield break;
	}

	// Token: 0x0600027D RID: 637 RVA: 0x00012CC4 File Offset: 0x00010EC4
	private void OnHitReenableGravity(object sender, EventArgs args)
	{
		base.EnemyController.ControllerCorgi.GravityActive(true);
	}

	// Token: 0x0600027E RID: 638 RVA: 0x00012CD7 File Offset: 0x00010ED7
	private IEnumerator GroundSlamBlinkCoroutine(float duration, Color color)
	{
		for (;;)
		{
			float blinkDuration = Time.time + base.EnemyController.BlinkPulseEffect.SingleBlinkDuration * 2f;
			base.EnemyController.BlinkPulseEffect.StartSingleBlinkEffect(color);
			while (Time.time < blinkDuration)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x0600027F RID: 639 RVA: 0x00012CED File Offset: 0x00010EED
	private void Jump(float x, float y)
	{
		base.SetVelocityX(x, true);
		base.EnemyController.JumpHorizontalVelocity = x;
		base.SetVelocityY(y, false);
	}

	// Token: 0x06000280 RID: 640 RVA: 0x00012D0C File Offset: 0x00010F0C
	private void SetRandomJumpDirection()
	{
		Vector2 vector = CDGHelper.AngleToVector((float)UnityEngine.Random.Range(this.m_jump_Attack_Min_And_Max_Angle.x, this.m_jump_Attack_Min_And_Max_Angle.y + 1));
		this.m_jump_Direction = this.m_jump_Power * vector;
		this.DrawAimIndicator(vector);
	}

	// Token: 0x06000281 RID: 641 RVA: 0x00012D5C File Offset: 0x00010F5C
	private IEnumerator FireJumpProjectiles(float delayBetweenProjectiles)
	{
		float pitch = 0f;
		while (!base.EnemyController.IsGrounded)
		{
			float delay = Time.time + delayBetweenProjectiles;
			while (Time.time < delay || base.IsPaused)
			{
				yield return null;
			}
			Projectile_RL projectile_RL;
			if (base.EnemyController.EnemyRank < EnemyRank.Expert)
			{
				projectile_RL = this.FireProjectile("BlobFishTailProjectile", Vector2.zero, false, 0f, 1f, true, true, true);
			}
			else
			{
				projectile_RL = this.FireProjectile("BlobFishTailLongProjectile", Vector2.zero, false, 0f, 1f, true, true, true);
			}
			projectile_RL.GetComponent<BubbleProjectileAudioController>().PlaySpawnAudio(pitch);
			pitch = Mathf.Min(pitch + 0.05f, 1f);
		}
		yield break;
	}

	// Token: 0x06000282 RID: 642 RVA: 0x00012D74 File Offset: 0x00010F74
	private void DrawAimIndicator(Vector2 direction)
	{
		this.m_aimIndicator.transform.rotation = Quaternion.LookRotation(Vector3.forward, this.m_jump_Direction);
		Vector3 localEulerAngles = this.m_aimIndicator.transform.localEulerAngles;
		localEulerAngles.z += 90f;
		this.m_aimIndicator.transform.localEulerAngles = localEulerAngles;
		this.m_aimIndicator.transform.localPosition = this.m_aim_Indicator_Offset * direction;
		this.m_aimIndicator.gameObject.SetActive(true);
	}

	// Token: 0x06000283 RID: 643 RVA: 0x00012E0A File Offset: 0x0001100A
	private void HideAimIndicator()
	{
		this.m_aimIndicator.gameObject.SetActive(false);
	}

	// Token: 0x06000284 RID: 644 RVA: 0x00012E20 File Offset: 0x00011020
	public override void OnLBCompleteOrCancelled()
	{
		base.EnemyController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onHitReenableGravity);
		base.EnemyController.ControllerCorgi.GravityActive(true);
		this.StopPersistentCoroutine(this.m_groundSlamBlinkCoroutine);
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x0400066C RID: 1644
	[SerializeField]
	private SpriteRenderer m_aimIndicator;

	// Token: 0x0400066D RID: 1645
	private Action<object, CharacterHitEventArgs> m_onHitReenableGravity;

	// Token: 0x0400066E RID: 1646
	protected const string JUMP_ATTACK_TELL_INTRO = "Jump_Tell_Intro";

	// Token: 0x0400066F RID: 1647
	protected const string JUMP_ATTACK_TELL_HOLD = "Jump_Tell_Hold";

	// Token: 0x04000670 RID: 1648
	protected const string JUMP_ATTACK_ATTACK_INTRO = "Jump_Attack_Intro";

	// Token: 0x04000671 RID: 1649
	protected const string JUMP_ATTACK_ATTACK_HOLD = "Jump_Attack_Hold";

	// Token: 0x04000672 RID: 1650
	protected const string JUMP_ATTACK_EXIT = "Jump_Exit";

	// Token: 0x04000673 RID: 1651
	protected const string WATER_PROJECTILE = "BlobFishWaterProjectile";

	// Token: 0x04000674 RID: 1652
	protected const string TAIL_PROJECTILE = "BlobFishTailProjectile";

	// Token: 0x04000675 RID: 1653
	protected const string TAIL_LONG_PROJECTILE = "BlobFishTailLongProjectile";

	// Token: 0x04000676 RID: 1654
	protected Vector2 m_jump_Direction;

	// Token: 0x04000677 RID: 1655
	protected float m_jump_Attack_Intro_AnimationSpeed = 1f;

	// Token: 0x04000678 RID: 1656
	protected float m_jump_Attack_Intro_Delay;

	// Token: 0x04000679 RID: 1657
	protected float m_jump_Attack_Hold_AnimationSpeed = 1f;

	// Token: 0x0400067A RID: 1658
	protected float m_jump_Attack_Hold_Delay;

	// Token: 0x0400067B RID: 1659
	protected float m_jump_Exit_AnimationSpeed = 1f;

	// Token: 0x0400067C RID: 1660
	protected float m_jump_Exit_Delay;

	// Token: 0x0400067D RID: 1661
	protected float m_jump_Exit_AttackCD;

	// Token: 0x0400067E RID: 1662
	protected const string GROUND_SLAM_TELL_INTRO = "Flop_Tell_Intro";

	// Token: 0x0400067F RID: 1663
	protected const string GROUND_SLAM_TELL_HOLD = "Flop_Tell_Hold";

	// Token: 0x04000680 RID: 1664
	protected const string GROUND_SLAM_ATTACK_INTRO = "Flop_Attack_Intro";

	// Token: 0x04000681 RID: 1665
	protected const string GROUND_SLAM_ATTACK_HOLD = "Flop_Attack_Hold";

	// Token: 0x04000682 RID: 1666
	protected const string GROUND_SLAM_EXIT = "Flop_Exit";

	// Token: 0x04000683 RID: 1667
	protected const string SPLASH_PROJECTILE = "BlobFishWaterSlamProjectile";

	// Token: 0x04000684 RID: 1668
	protected Vector2 GroundSlam_Land_ThrowAngle = new Vector2(15f, 7f);

	// Token: 0x04000685 RID: 1669
	protected Vector2 GroundSlam_Land_ThrowPower = new Vector2(0.75f, 1f);

	// Token: 0x04000686 RID: 1670
	private Coroutine m_groundSlamBlinkCoroutine;
}
