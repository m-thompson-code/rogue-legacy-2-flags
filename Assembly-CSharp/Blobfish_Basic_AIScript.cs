using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x020000B6 RID: 182
public class Blobfish_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000304 RID: 772 RVA: 0x000044C1 File Offset: 0x000026C1
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

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x06000305 RID: 773 RVA: 0x00003DEF File Offset: 0x00001FEF
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x06000306 RID: 774 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x06000307 RID: 775 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x06000308 RID: 776 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool HasTail
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000309 RID: 777 RVA: 0x000044EF File Offset: 0x000026EF
	private void Awake()
	{
		this.m_onHitReenableGravity = new Action<object, CharacterHitEventArgs>(this.OnHitReenableGravity);
	}

	// Token: 0x0600030A RID: 778 RVA: 0x00004503 File Offset: 0x00002703
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		this.HideAimIndicator();
	}

	// Token: 0x0600030B RID: 779 RVA: 0x00004512 File Offset: 0x00002712
	protected override void OnDisable()
	{
		base.OnDisable();
		this.HideAimIndicator();
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x0600030C RID: 780 RVA: 0x00004520 File Offset: 0x00002720
	protected virtual float m_jump_Tell_Delay
	{
		get
		{
			return 0.3f;
		}
	}

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x0600030D RID: 781 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_jump_Tell_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x0600030E RID: 782 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float m_jump_Exit_ForceIdle
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x0600030F RID: 783 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_jumpLandingShootsProjectiles
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x06000310 RID: 784 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_jumpLandingShootsManyProjectiles
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x06000311 RID: 785 RVA: 0x00004527 File Offset: 0x00002727
	protected virtual int m_jumpLandingProjectileAngle
	{
		get
		{
			return 80;
		}
	}

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x06000312 RID: 786 RVA: 0x0000452B File Offset: 0x0000272B
	protected virtual int m_jumpLandingProjectileAngleMany
	{
		get
		{
			return 70;
		}
	}

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x06000313 RID: 787 RVA: 0x0000452F File Offset: 0x0000272F
	protected virtual float m_jumpProjectileFireDelay
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x06000314 RID: 788 RVA: 0x00004536 File Offset: 0x00002736
	protected virtual float m_aim_Indicator_Offset
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x06000315 RID: 789 RVA: 0x0000453D File Offset: 0x0000273D
	protected virtual Vector2Int m_jump_Attack_Min_And_Max_Angle
	{
		get
		{
			return new Vector2Int(65, 125);
		}
	}

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x06000316 RID: 790 RVA: 0x00004548 File Offset: 0x00002748
	protected virtual float m_jump_Power
	{
		get
		{
			return 27.5f;
		}
	}

	// Token: 0x06000317 RID: 791 RVA: 0x0000454F File Offset: 0x0000274F
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

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x06000318 RID: 792 RVA: 0x00004548 File Offset: 0x00002748
	protected virtual float GroundSlam_Jump_Power
	{
		get
		{
			return 27.5f;
		}
	}

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x06000319 RID: 793 RVA: 0x0000455E File Offset: 0x0000275E
	protected virtual float GroundSlam_Land_Power
	{
		get
		{
			return -50f;
		}
	}

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x0600031A RID: 794 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float GroundSlam_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x0600031B RID: 795 RVA: 0x00004565 File Offset: 0x00002765
	protected virtual float GroundSlam_TellHold_AnimSpeed
	{
		get
		{
			return 1.75f;
		}
	}

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x0600031C RID: 796 RVA: 0x00004536 File Offset: 0x00002736
	protected virtual float GroundSlam_TellHold_Duration
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x0600031D RID: 797 RVA: 0x00004536 File Offset: 0x00002736
	protected virtual float GroundSlam_AttackIntro_AnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x0600031E RID: 798 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float GroundSlam_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x0600031F RID: 799 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float GroundSlam_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x06000320 RID: 800 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float GroundSlam_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x06000321 RID: 801 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float GroundSlam_Exit_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170000CC RID: 204
	// (get) Token: 0x06000322 RID: 802 RVA: 0x0000456C File Offset: 0x0000276C
	protected virtual float GroundSlam_Exit_Duration
	{
		get
		{
			return 0.2f;
		}
	}

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x06000323 RID: 803 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float GroundSlam_Exit_ForceIdle
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x06000324 RID: 804 RVA: 0x00004573 File Offset: 0x00002773
	protected virtual float GroundSlam_Exit_AttackCD
	{
		get
		{
			return 2.75f;
		}
	}

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x06000325 RID: 805 RVA: 0x0000457A File Offset: 0x0000277A
	protected float GroundSlam_Land_Delay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x06000326 RID: 806 RVA: 0x00004581 File Offset: 0x00002781
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

	// Token: 0x06000327 RID: 807 RVA: 0x00004590 File Offset: 0x00002790
	private void OnHitReenableGravity(object sender, EventArgs args)
	{
		base.EnemyController.ControllerCorgi.GravityActive(true);
	}

	// Token: 0x06000328 RID: 808 RVA: 0x000045A3 File Offset: 0x000027A3
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

	// Token: 0x06000329 RID: 809 RVA: 0x000045B9 File Offset: 0x000027B9
	private void Jump(float x, float y)
	{
		base.SetVelocityX(x, true);
		base.EnemyController.JumpHorizontalVelocity = x;
		base.SetVelocityY(y, false);
	}

	// Token: 0x0600032A RID: 810 RVA: 0x000508BC File Offset: 0x0004EABC
	private void SetRandomJumpDirection()
	{
		Vector2 vector = CDGHelper.AngleToVector((float)UnityEngine.Random.Range(this.m_jump_Attack_Min_And_Max_Angle.x, this.m_jump_Attack_Min_And_Max_Angle.y + 1));
		this.m_jump_Direction = this.m_jump_Power * vector;
		this.DrawAimIndicator(vector);
	}

	// Token: 0x0600032B RID: 811 RVA: 0x000045D7 File Offset: 0x000027D7
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

	// Token: 0x0600032C RID: 812 RVA: 0x0005090C File Offset: 0x0004EB0C
	private void DrawAimIndicator(Vector2 direction)
	{
		this.m_aimIndicator.transform.rotation = Quaternion.LookRotation(Vector3.forward, this.m_jump_Direction);
		Vector3 localEulerAngles = this.m_aimIndicator.transform.localEulerAngles;
		localEulerAngles.z += 90f;
		this.m_aimIndicator.transform.localEulerAngles = localEulerAngles;
		this.m_aimIndicator.transform.localPosition = this.m_aim_Indicator_Offset * direction;
		this.m_aimIndicator.gameObject.SetActive(true);
	}

	// Token: 0x0600032D RID: 813 RVA: 0x000045ED File Offset: 0x000027ED
	private void HideAimIndicator()
	{
		this.m_aimIndicator.gameObject.SetActive(false);
	}

	// Token: 0x0600032E RID: 814 RVA: 0x000509A4 File Offset: 0x0004EBA4
	public override void OnLBCompleteOrCancelled()
	{
		base.EnemyController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onHitReenableGravity);
		base.EnemyController.ControllerCorgi.GravityActive(true);
		this.StopPersistentCoroutine(this.m_groundSlamBlinkCoroutine);
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x040006F8 RID: 1784
	[SerializeField]
	private SpriteRenderer m_aimIndicator;

	// Token: 0x040006F9 RID: 1785
	private Action<object, CharacterHitEventArgs> m_onHitReenableGravity;

	// Token: 0x040006FA RID: 1786
	protected const string JUMP_ATTACK_TELL_INTRO = "Jump_Tell_Intro";

	// Token: 0x040006FB RID: 1787
	protected const string JUMP_ATTACK_TELL_HOLD = "Jump_Tell_Hold";

	// Token: 0x040006FC RID: 1788
	protected const string JUMP_ATTACK_ATTACK_INTRO = "Jump_Attack_Intro";

	// Token: 0x040006FD RID: 1789
	protected const string JUMP_ATTACK_ATTACK_HOLD = "Jump_Attack_Hold";

	// Token: 0x040006FE RID: 1790
	protected const string JUMP_ATTACK_EXIT = "Jump_Exit";

	// Token: 0x040006FF RID: 1791
	protected const string WATER_PROJECTILE = "BlobFishWaterProjectile";

	// Token: 0x04000700 RID: 1792
	protected const string TAIL_PROJECTILE = "BlobFishTailProjectile";

	// Token: 0x04000701 RID: 1793
	protected const string TAIL_LONG_PROJECTILE = "BlobFishTailLongProjectile";

	// Token: 0x04000702 RID: 1794
	protected Vector2 m_jump_Direction;

	// Token: 0x04000703 RID: 1795
	protected float m_jump_Attack_Intro_AnimationSpeed = 1f;

	// Token: 0x04000704 RID: 1796
	protected float m_jump_Attack_Intro_Delay;

	// Token: 0x04000705 RID: 1797
	protected float m_jump_Attack_Hold_AnimationSpeed = 1f;

	// Token: 0x04000706 RID: 1798
	protected float m_jump_Attack_Hold_Delay;

	// Token: 0x04000707 RID: 1799
	protected float m_jump_Exit_AnimationSpeed = 1f;

	// Token: 0x04000708 RID: 1800
	protected float m_jump_Exit_Delay;

	// Token: 0x04000709 RID: 1801
	protected float m_jump_Exit_AttackCD;

	// Token: 0x0400070A RID: 1802
	protected const string GROUND_SLAM_TELL_INTRO = "Flop_Tell_Intro";

	// Token: 0x0400070B RID: 1803
	protected const string GROUND_SLAM_TELL_HOLD = "Flop_Tell_Hold";

	// Token: 0x0400070C RID: 1804
	protected const string GROUND_SLAM_ATTACK_INTRO = "Flop_Attack_Intro";

	// Token: 0x0400070D RID: 1805
	protected const string GROUND_SLAM_ATTACK_HOLD = "Flop_Attack_Hold";

	// Token: 0x0400070E RID: 1806
	protected const string GROUND_SLAM_EXIT = "Flop_Exit";

	// Token: 0x0400070F RID: 1807
	protected const string SPLASH_PROJECTILE = "BlobFishWaterSlamProjectile";

	// Token: 0x04000710 RID: 1808
	protected Vector2 GroundSlam_Land_ThrowAngle = new Vector2(15f, 7f);

	// Token: 0x04000711 RID: 1809
	protected Vector2 GroundSlam_Land_ThrowPower = new Vector2(0.75f, 1f);

	// Token: 0x04000712 RID: 1810
	private Coroutine m_groundSlamBlinkCoroutine;
}
