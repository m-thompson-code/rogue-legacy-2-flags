using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using UnityEngine;

// Token: 0x02000194 RID: 404
public class MimicChest_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000B17 RID: 2839 RVA: 0x00006ED8 File Offset: 0x000050D8
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"MimicCoinProjectile",
			"MimicCoinMinibossProjectile",
			"MimicBossBounceBoltProjectile",
			"MimicBossJumpProjectile"
		};
	}

	// Token: 0x17000544 RID: 1348
	// (get) Token: 0x06000B18 RID: 2840 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public override bool ForceDeathAnimation
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000545 RID: 1349
	// (get) Token: 0x06000B19 RID: 2841 RVA: 0x00006F06 File Offset: 0x00005106
	protected virtual string JumpAudioEventPath
	{
		get
		{
			return "event:/SFX/Enemies/sfx_enemy_mimicChest_jump";
		}
	}

	// Token: 0x17000546 RID: 1350
	// (get) Token: 0x06000B1A RID: 2842 RVA: 0x00006F0D File Offset: 0x0000510D
	protected virtual string LandAudioEventPath
	{
		get
		{
			return "event:/SFX/Enemies/sfx_enemy_mimicChest_land";
		}
	}

	// Token: 0x17000547 RID: 1351
	// (get) Token: 0x06000B1B RID: 2843 RVA: 0x00006F14 File Offset: 0x00005114
	protected virtual string DeathHitAudioEventPath
	{
		get
		{
			return "event:/SFX/Enemies/sfx_enemy_mimicChest_death_killingBlow";
		}
	}

	// Token: 0x17000548 RID: 1352
	// (get) Token: 0x06000B1C RID: 2844 RVA: 0x00006F1B File Offset: 0x0000511B
	private string DashLoopAudioEventPath
	{
		get
		{
			return "event:/SFX/Enemies/sfx_enemy_mimicChest_dash_start_loop";
		}
	}

	// Token: 0x17000549 RID: 1353
	// (get) Token: 0x06000B1D RID: 2845 RVA: 0x00006F22 File Offset: 0x00005122
	private string ChangeDirectionAudioEventPath
	{
		get
		{
			return "event:/SFX/Enemies/sfx_enemy_mimicChest_dash_changeDirection";
		}
	}

	// Token: 0x1700054A RID: 1354
	// (get) Token: 0x06000B1E RID: 2846 RVA: 0x00006F29 File Offset: 0x00005129
	protected virtual Vector2 JumpHeight
	{
		get
		{
			return new Vector2(17f, 17f);
		}
	}

	// Token: 0x1700054B RID: 1355
	// (get) Token: 0x06000B1F RID: 2847 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual int NumCoinsFiredOnHit
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x1700054C RID: 1356
	// (get) Token: 0x06000B20 RID: 2848 RVA: 0x00006F3A File Offset: 0x0000513A
	protected virtual Vector2 CoinFireAngle_OnHit
	{
		get
		{
			return new Vector2(80f, 100f);
		}
	}

	// Token: 0x1700054D RID: 1357
	// (get) Token: 0x06000B21 RID: 2849 RVA: 0x00006F4B File Offset: 0x0000514B
	protected virtual Vector2 CoinFireSpeed_OnHit
	{
		get
		{
			return new Vector2(1.25f, 1.65f);
		}
	}

	// Token: 0x1700054E RID: 1358
	// (get) Token: 0x06000B22 RID: 2850 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual int NumCoinsFiredOnLanding
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x1700054F RID: 1359
	// (get) Token: 0x06000B23 RID: 2851 RVA: 0x00006F3A File Offset: 0x0000513A
	protected virtual Vector2 CoinFireAngle_OnLand
	{
		get
		{
			return new Vector2(80f, 100f);
		}
	}

	// Token: 0x17000550 RID: 1360
	// (get) Token: 0x06000B24 RID: 2852 RVA: 0x00006F4B File Offset: 0x0000514B
	protected virtual Vector2 CoinFireSpeed_OnLand
	{
		get
		{
			return new Vector2(1.25f, 1.65f);
		}
	}

	// Token: 0x17000551 RID: 1361
	// (get) Token: 0x06000B25 RID: 2853 RVA: 0x00004762 File Offset: 0x00002962
	protected virtual int NumCoinsFiredOnDeath
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x17000552 RID: 1362
	// (get) Token: 0x06000B26 RID: 2854 RVA: 0x00006F5C File Offset: 0x0000515C
	protected virtual Vector2 CoinFireAngle_OnDeath
	{
		get
		{
			return new Vector2(68f, 112f);
		}
	}

	// Token: 0x17000553 RID: 1363
	// (get) Token: 0x06000B27 RID: 2855 RVA: 0x00006F6D File Offset: 0x0000516D
	protected virtual Vector2 CoinFireSpeed_OnDeath
	{
		get
		{
			return new Vector2(0.75f, 1f);
		}
	}

	// Token: 0x17000554 RID: 1364
	// (get) Token: 0x06000B28 RID: 2856 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int DeathCoinAttack_CoinSpreadAmount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000555 RID: 1365
	// (get) Token: 0x06000B29 RID: 2857 RVA: 0x000052B0 File Offset: 0x000034B0
	protected virtual float DeathCoinAttack_AngleSpread
	{
		get
		{
			return 18f;
		}
	}

	// Token: 0x17000556 RID: 1366
	// (get) Token: 0x06000B2A RID: 2858 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_advancedBoss
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000557 RID: 1367
	// (get) Token: 0x06000B2B RID: 2859 RVA: 0x00006780 File Offset: 0x00004980
	protected virtual float m_airborneKO_AnimationDelay
	{
		get
		{
			return 0.65f;
		}
	}

	// Token: 0x06000B2C RID: 2860 RVA: 0x00006F7E File Offset: 0x0000517E
	private void Awake()
	{
		this.m_onPlayerHit = new Action<object, CharacterHitEventArgs>(this.OnPlayerHit);
	}

	// Token: 0x06000B2D RID: 2861 RVA: 0x000696FC File Offset: 0x000678FC
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.EnemyController.HealthChangeRelay.AddListener(new Action<object, HealthChangeEventArgs>(this.SpewCoins), false);
		base.EnemyController.LogicController.DisableLogicActivationByDistance = true;
		base.EnemyController.LogicController.OverrideLogicDelay(0f);
		base.EnemyController.LockFlip = true;
		base.EnemyController.CharacterHitResponse.OnCharacterHitRelay.AddListener(new Action<object, CharacterHitEventArgs>(this.OnEnemyHit), false);
		base.EnemyController.LogicController.DisableRestLogicInterrupt = true;
		base.EnemyController.StatusEffectController.SetAllStatusEffectsHidden(true);
		base.EnemyController.DisableOffscreenWarnings = true;
		this.m_matPropertyBlock = new MaterialPropertyBlock();
		this.m_playerWasInDashWakeRange = false;
		this.m_dashEventInstance = AudioUtility.GetEventInstance(this.DashLoopAudioEventPath, base.transform);
	}

	// Token: 0x06000B2E RID: 2862 RVA: 0x00006F92 File Offset: 0x00005192
	private void OnEnable()
	{
		if (PlayerManager.IsInstantiated && !PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().CharacterHitResponse.OnCharacterHitRelay.AddListener(this.m_onPlayerHit, false);
		}
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x000697DC File Offset: 0x000679DC
	protected override void OnDisable()
	{
		base.OnDisable();
		if (PlayerManager.IsInstantiated && !PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onPlayerHit);
		}
		if (base.IsInitialized)
		{
			base.EnemyController.StatusEffectController.SetAllStatusEffectsHidden(true);
			base.EnemyController.DisableOffscreenWarnings = true;
			base.EnemyController.LogicController.DisableLogicActivationByDistance = true;
			for (int i = 0; i < base.EnemyController.RendererArray.Count; i++)
			{
				Renderer renderer = base.EnemyController.RendererArray[i];
				renderer.GetPropertyBlock(this.m_matPropertyBlock);
				this.m_matPropertyBlock.SetColor(ShaderID_RL._MultiplyColor, base.EnemyController.RendererArrayDefaultTint[i].DefaultMultiplyColor);
				renderer.SetPropertyBlock(this.m_matPropertyBlock);
			}
			if (this.m_dashEventInstance.isValid())
			{
				AudioManager.Stop(this.m_dashEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
		}
	}

	// Token: 0x06000B30 RID: 2864 RVA: 0x000698D4 File Offset: 0x00067AD4
	public override void OnEnemyActivated()
	{
		base.OnEnemyActivated();
		if (base.EnemyController.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Invuln))
		{
			foreach (Renderer renderer in base.EnemyController.RendererArray)
			{
				if (renderer.sharedMaterial.HasProperty(ShaderID_RL._ShieldToggle))
				{
					renderer.GetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
					BaseStatusEffect.m_matBlockHelper_STATIC.SetInt(ShaderID_RL._ShieldToggle, 1);
					renderer.SetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
				}
			}
		}
	}

	// Token: 0x06000B31 RID: 2865 RVA: 0x00006FBE File Offset: 0x000051BE
	private void OnEnemyHit(object sender, CharacterHitEventArgs args)
	{
		base.EnemyController.StatusEffectController.SetAllStatusEffectsHidden(false);
		base.EnemyController.DisableOffscreenWarnings = false;
	}

	// Token: 0x06000B32 RID: 2866 RVA: 0x0006997C File Offset: 0x00067B7C
	private void OnPlayerHit(object sender, CharacterHitEventArgs args)
	{
		if (args.Attacker == base.EnemyController)
		{
			base.EnemyController.LogicController.TriggerAggro(null, null);
			base.EnemyController.StatusEffectController.SetAllStatusEffectsHidden(false);
			base.EnemyController.DisableOffscreenWarnings = false;
			if (PlayerManager.IsInstantiated && !PlayerManager.IsDisposed)
			{
				PlayerManager.GetPlayerController().CharacterHitResponse.OnCharacterHitRelay.RemoveListener(new Action<object, CharacterHitEventArgs>(this.OnPlayerHit));
			}
		}
	}

	// Token: 0x06000B33 RID: 2867 RVA: 0x000699F8 File Offset: 0x00067BF8
	private void OnDestroy()
	{
		if (this.m_dashEventInstance.isValid())
		{
			this.m_dashEventInstance.release();
		}
		if (base.EnemyController)
		{
			base.EnemyController.HealthChangeRelay.RemoveListener(new Action<object, HealthChangeEventArgs>(this.SpewCoins));
			base.EnemyController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(new Action<object, CharacterHitEventArgs>(this.OnEnemyHit));
		}
	}

	// Token: 0x06000B34 RID: 2868 RVA: 0x00069A6C File Offset: 0x00067C6C
	protected void SpewCoins(object sender, HealthChangeEventArgs args)
	{
		if (args.PrevHealthValue <= args.NewHealthValue)
		{
			return;
		}
		if (args.NewHealthValue <= 0f)
		{
			return;
		}
		for (int i = 0; i < this.NumCoinsFiredOnHit; i++)
		{
			float angle = UnityEngine.Random.Range(this.CoinFireAngle_OnHit.x, this.CoinFireAngle_OnHit.y);
			float speedMod = UnityEngine.Random.Range(this.CoinFireSpeed_OnHit.x, this.CoinFireSpeed_OnHit.y);
			this.FireProjectile(this.m_verticalShotProjectileName, 0, false, angle, speedMod, true, true, true);
		}
	}

	// Token: 0x17000558 RID: 1368
	// (get) Token: 0x06000B35 RID: 2869 RVA: 0x00006FDD File Offset: 0x000051DD
	protected virtual string m_verticalShotProjectileName
	{
		get
		{
			return "MimicCoinProjectile";
		}
	}

	// Token: 0x17000559 RID: 1369
	// (get) Token: 0x06000B36 RID: 2870 RVA: 0x00004A8D File Offset: 0x00002C8D
	protected virtual int m_verticalShot_TotalShotSpread
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x1700055A RID: 1370
	// (get) Token: 0x06000B37 RID: 2871 RVA: 0x00004762 File Offset: 0x00002962
	protected virtual int m_verticalShot_TotalLoops
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x1700055B RID: 1371
	// (get) Token: 0x06000B38 RID: 2872 RVA: 0x00006CB3 File Offset: 0x00004EB3
	protected virtual int m_verticalShot_InitialAngle
	{
		get
		{
			return 90;
		}
	}

	// Token: 0x1700055C RID: 1372
	// (get) Token: 0x06000B39 RID: 2873 RVA: 0x00006FE4 File Offset: 0x000051E4
	protected virtual Vector2 m_verticalShot_RandomAngleAngleOffset
	{
		get
		{
			return new Vector2(-21f, 3f);
		}
	}

	// Token: 0x1700055D RID: 1373
	// (get) Token: 0x06000B3A RID: 2874 RVA: 0x00004A90 File Offset: 0x00002C90
	protected virtual float m_verticalShot_LoopDelay
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x1700055E RID: 1374
	// (get) Token: 0x06000B3B RID: 2875 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_verticalShot_SpeedMod
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x06000B3C RID: 2876 RVA: 0x00006FF5 File Offset: 0x000051F5
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator VerticalShot_Attack()
	{
		this.StopAndFaceTarget();
		yield return this.Default_Animation("VerticalShot_Tell_Hold", this.m_verticalShot_TellHold_AnimationSpeed, this.m_verticalShot_TellHold_Delay, true);
		yield return this.Default_Animation("HopIntro", this.m_verticalShot_AttackIntro_AnimationSpeed, this.m_verticalShot_AttackIntro_Delay, true);
		int num2;
		for (int i = 0; i < this.m_verticalShot_TotalLoops; i = num2 + 1)
		{
			for (int j = 0; j < this.m_verticalShot_TotalShotSpread; j++)
			{
				float num = (float)UnityEngine.Random.Range((int)this.m_verticalShot_RandomAngleAngleOffset.x, (int)this.m_verticalShot_RandomAngleAngleOffset.y);
				this.FireProjectile(this.m_verticalShotProjectileName, 1, true, (float)this.m_verticalShot_InitialAngle + num * (float)(j + 1), this.m_verticalShot_SpeedMod, true, true, true);
			}
			yield return base.Wait(this.m_verticalShot_LoopDelay, false);
			num2 = i;
		}
		yield return this.Default_Animation("Hop", this.m_verticalShot_AttackHold_AnimationSpeed, this.m_verticalShot_AttackHold_Delay, false);
		yield return this.Default_Animation("HopLand", this.m_verticalShot_Exit_AnimationSpeed, this.m_verticalShot_Exit_Delay, true);
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_verticalShot_Exit_ForceIdle, this.m_verticalShot_Exit_AttackCD);
		yield break;
	}

	// Token: 0x1700055F RID: 1375
	// (get) Token: 0x06000B3D RID: 2877 RVA: 0x00003D93 File Offset: 0x00001F93
	protected virtual float m_dashAttack_Exit_AttackCD
	{
		get
		{
			return 6f;
		}
	}

	// Token: 0x17000560 RID: 1376
	// (get) Token: 0x06000B3E RID: 2878 RVA: 0x0000521E File Offset: 0x0000341E
	protected virtual float m_dashAttackSpeed
	{
		get
		{
			return 22f;
		}
	}

	// Token: 0x17000561 RID: 1377
	// (get) Token: 0x06000B3F RID: 2879 RVA: 0x00006E9D File Offset: 0x0000509D
	protected virtual Vector2 m_dashAttackDuration
	{
		get
		{
			return new Vector2(2f, 2f);
		}
	}

	// Token: 0x17000562 RID: 1378
	// (get) Token: 0x06000B40 RID: 2880 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_canDashWake
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000563 RID: 1379
	// (get) Token: 0x06000B41 RID: 2881 RVA: 0x00005FB1 File Offset: 0x000041B1
	protected virtual float m_dashWakeActivationRange
	{
		get
		{
			return 5.5f;
		}
	}

	// Token: 0x17000564 RID: 1380
	// (get) Token: 0x06000B42 RID: 2882 RVA: 0x0000457A File Offset: 0x0000277A
	protected virtual float m_dashWakeAttackOdds
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x06000B43 RID: 2883 RVA: 0x00007004 File Offset: 0x00005204
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Dash_Attack()
	{
		this.StopAndFaceTarget();
		yield return this.Default_TellIntroAndLoop("Dash_Tell_Intro", this.m_dashAttack_TellIntro_AnimationSpeed, "Dash_Tell_Hold", this.m_dashAttack_TellHold_AnimationSpeed, this.m_dashAttack_TellIntroAndHold_Delay);
		yield return this.Default_Animation("Dash_Attack_Intro", this.m_dashAttack_AttackIntro_AnimationSpeed, this.m_dashAttack_AttackIntro_Delay, true);
		yield return this.Default_Animation("Dash_Attack_Hold", this.m_dashAttack_AttackHold_AnimationSpeed, this.m_dashAttack_AttackHold_Delay, false);
		base.EnemyController.DisableFriction = true;
		if (base.EnemyController.IsFacingRight)
		{
			base.SetVelocityX(this.m_dashAttackSpeed, false);
		}
		else
		{
			base.SetVelocityX(-this.m_dashAttackSpeed, false);
		}
		base.EnemyController.LockFlip = true;
		this.m_isDashAttacking = true;
		this.OnStartDashing();
		yield return base.Wait(this.m_dashAttackDuration.x, this.m_dashAttackDuration.y, false);
		this.OnStopDashing();
		this.m_isDashAttacking = false;
		if (base.EnemyController.IsFacingRight)
		{
			base.SetVelocityX(this.m_dashAttackSpeed * 0.5f, false);
		}
		else
		{
			base.SetVelocityX(this.m_dashAttackSpeed * -0.5f, false);
		}
		base.EnemyController.DisableFriction = true;
		yield return this.Default_Animation("Dash_Exit", this.m_dashAttack_Exit_AnimationSpeed, this.m_dashAttack_Exit_Delay, true);
		base.SetVelocityX(0f, false);
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_dashAttack_Exit_ForceIdle, this.m_dashAttack_Exit_AttackCD);
		base.EnemyController.LockFlip = false;
		yield break;
	}

	// Token: 0x06000B44 RID: 2884 RVA: 0x00007013 File Offset: 0x00005213
	protected virtual void OnStartDashing()
	{
		AudioManager.PlayAttached(this, this.m_dashEventInstance, base.gameObject);
	}

	// Token: 0x06000B45 RID: 2885 RVA: 0x00007027 File Offset: 0x00005227
	protected virtual void OnStopDashing()
	{
		AudioManager.Stop(this.m_dashEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x06000B46 RID: 2886 RVA: 0x00007035 File Offset: 0x00005235
	private void OnJump()
	{
		AudioManager.PlayOneShotAttached(this, this.JumpAudioEventPath, base.gameObject);
	}

	// Token: 0x06000B47 RID: 2887 RVA: 0x00007049 File Offset: 0x00005249
	private void OnGrounded()
	{
		AudioManager.PlayOneShotAttached(this, this.LandAudioEventPath, base.gameObject);
	}

	// Token: 0x06000B48 RID: 2888 RVA: 0x0000705D File Offset: 0x0000525D
	private IEnumerator OnBeginDeathAnim()
	{
		AudioManager.PlayOneShotAttached(this, this.DeathHitAudioEventPath, base.gameObject);
		if (base.EnemyController.IsBoss)
		{
			yield return base.DeathAnim();
		}
		yield break;
	}

	// Token: 0x06000B49 RID: 2889 RVA: 0x0000706C File Offset: 0x0000526C
	public override void OnLBCompleteOrCancelled()
	{
		base.EnemyController.DisableFriction = false;
		base.EnemyController.LockFlip = false;
		this.m_isDashAttacking = false;
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x06000B4A RID: 2890 RVA: 0x00007093 File Offset: 0x00005293
	public override IEnumerator WalkTowards()
	{
		while (base.EnemyController.KnockedIntoAir)
		{
			yield return null;
		}
		base.EnemyController.LockFlip = false;
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		this.SetAnimationSpeedMultiplier(this.WalkAnimSpeedMod);
		float y = UnityEngine.Random.Range(this.JumpHeight.x, this.JumpHeight.y);
		if (base.EnemyController.IsTargetToMyRight)
		{
			base.SetVelocity(base.EnemyController.ActualSpeed, y, false);
		}
		else
		{
			base.SetVelocity(-base.EnemyController.ActualSpeed, y, false);
		}
		this.OnJump();
		yield return base.Wait(0.25f, false);
		yield return base.WaitUntilIsGrounded();
		for (int i = 0; i < this.NumCoinsFiredOnLanding; i++)
		{
			float angle = UnityEngine.Random.Range(this.CoinFireAngle_OnLand.x, this.CoinFireAngle_OnLand.y);
			float speedMod = UnityEngine.Random.Range(this.CoinFireSpeed_OnLand.x, this.CoinFireSpeed_OnLand.y);
			if (this.m_advancedBoss)
			{
				this.FireProjectile("MimicBossBounceBoltProjectile", 2, false, 0f, 1f, true, true, true);
				this.FireProjectile("MimicBossBounceBoltProjectile", 2, false, 180f, 1f, true, true, true);
			}
			else
			{
				this.FireProjectile(this.m_verticalShotProjectileName, 0, false, angle, speedMod, true, true, true);
			}
		}
		this.SetAnimationSpeedMultiplier(1f);
		base.EnemyController.LockFlip = false;
		yield break;
	}

	// Token: 0x06000B4B RID: 2891 RVA: 0x000070A2 File Offset: 0x000052A2
	public override IEnumerator DeathAnim()
	{
		if (base.EnemyController && base.EnemyController.EnemySpawnController)
		{
			base.EnemyController.EnemySpawnController.ForceEnemyDead(false);
		}
		if (this.m_playDeathParticlesOnZeroHP)
		{
			EffectManager.PlayEffect(base.EnemyController.gameObject, base.EnemyController.Animator, "EnemyGenericDeathPuff_Small_Effect_MediumVariant", base.EnemyController.transform.position, this.m_deathParticleDuration, EffectStopType.Gracefully, EffectTriggerDirection.None);
		}
		yield return this.OnBeginDeathAnim();
		foreach (Renderer renderer in base.EnemyController.RendererArray)
		{
			renderer.GetPropertyBlock(this.m_matPropertyBlock);
			this.m_matPropertyBlock.SetColor(ShaderID_RL._MultiplyColor, this.m_deathTintColor);
			renderer.SetPropertyBlock(this.m_matPropertyBlock);
		}
		if (!base.EnemyController.ControllerCorgi.State.IsGrounded)
		{
			yield return this.Default_Animation("KOAirborne", this.m_airborneKO_AnimationSpeed, this.m_airborneKO_AnimationDelay, false);
			while (!base.EnemyController.ControllerCorgi.State.IsGrounded)
			{
				yield return null;
			}
		}
		base.SetVelocity(0f, 0f, false);
		yield return this.Default_Animation("MimicChest_KOGrounded", this.m_groundedKOAnimationSpeed, this.m_groundedKOAnimationDelay, true);
		yield return this.Default_Animation("MimicChest_KOShake", this.m_death_AnimationSpeed, this.m_death_AnimationDelay, true);
		this.FireProjectile(this.m_verticalShotProjectileName, 0, false, 90f, 1f, true, true, true);
		for (int i = 0; i < this.DeathCoinAttack_CoinSpreadAmount; i++)
		{
			float angle = 90f + (float)(i + 1) * this.DeathCoinAttack_AngleSpread;
			this.FireProjectile(this.m_verticalShotProjectileName, 0, false, angle, 1f, true, true, true);
			angle = 90f - (float)(i + 1) * this.DeathCoinAttack_AngleSpread;
			this.FireProjectile(this.m_verticalShotProjectileName, 0, false, angle, 1f, true, true, true);
		}
		yield break;
	}

	// Token: 0x06000B4C RID: 2892 RVA: 0x00069AF4 File Offset: 0x00067CF4
	protected void LateUpdate()
	{
		if (!base.IsInitialized)
		{
			return;
		}
		if (this.m_canDashWake && !base.LogicController.LogicIsActivated)
		{
			bool flag = CDGHelper.DistanceBetweenPts(base.LogicController.PlayerController.Midpoint, base.EnemyController.Midpoint) < this.m_dashWakeActivationRange;
			if (!LocalTeleporterController.IsTeleporting && flag && !this.m_playerWasInDashWakeRange && UnityEngine.Random.Range(0f, 1f) < this.m_dashWakeAttackOdds)
			{
				base.LogicController.TriggerAggro(null, null);
				base.EnemyController.StatusEffectController.SetAllStatusEffectsHidden(false);
				base.EnemyController.DisableOffscreenWarnings = false;
				if (PlayerManager.IsInstantiated && !PlayerManager.IsDisposed)
				{
					PlayerManager.GetPlayerController().CharacterHitResponse.OnCharacterHitRelay.RemoveListener(new Action<object, CharacterHitEventArgs>(this.OnPlayerHit));
				}
				base.LogicController.ForceExecuteLogicBlockName_OnceOnly = "Dash_Attack";
			}
			this.m_playerWasInDashWakeRange = flag;
		}
		if (this.m_isDashAttacking)
		{
			bool flag2 = base.EnemyController.ControllerCorgi.State.IsCollidingLeft;
			if (base.EnemyController.IsFacingRight)
			{
				flag2 = base.EnemyController.ControllerCorgi.State.IsCollidingRight;
			}
			if (flag2)
			{
				base.EnemyController.LockFlip = false;
				base.EnemyController.CharacterCorgi.Flip(false, false);
				base.EnemyController.LockFlip = true;
				if (base.EnemyController.IsFacingRight)
				{
					base.SetVelocityX(this.m_dashAttackSpeed, false);
				}
				else
				{
					base.SetVelocityX(-this.m_dashAttackSpeed, false);
				}
				AudioManager.PlayOneShotAttached(this, this.ChangeDirectionAudioEventPath, base.gameObject);
			}
		}
		if (base.EnemyController.IsGrounded && !this.m_wasGrounded)
		{
			this.OnGrounded();
		}
		this.m_wasGrounded = base.EnemyController.IsGrounded;
	}

	// Token: 0x06000B4D RID: 2893 RVA: 0x00069CD4 File Offset: 0x00067ED4
	public override void ResetScript()
	{
		string forceExecuteLogicBlockName_OnceOnly = base.LogicController.ForceExecuteLogicBlockName_OnceOnly;
		base.ResetScript();
		base.LogicController.ForceExecuteLogicBlockName_OnceOnly = forceExecuteLogicBlockName_OnceOnly;
		if (!base.EnemyController.IsFacingRight)
		{
			base.EnemyController.LockFlip = false;
			base.EnemyController.CharacterCorgi.Flip(false, false);
		}
		base.EnemyController.LockFlip = true;
	}

	// Token: 0x04000DDD RID: 3549
	protected float m_death_AnimationSpeed = 1f;

	// Token: 0x04000DDE RID: 3550
	protected float m_death_AnimationDelay = 0.5f;

	// Token: 0x04000DDF RID: 3551
	protected float m_airborneKO_AnimationSpeed = 1f;

	// Token: 0x04000DE0 RID: 3552
	protected float m_groundedKOAnimationSpeed = 1f;

	// Token: 0x04000DE1 RID: 3553
	protected float m_groundedKOAnimationDelay = 0.5f;

	// Token: 0x04000DE2 RID: 3554
	protected bool m_playDeathParticlesOnZeroHP = true;

	// Token: 0x04000DE3 RID: 3555
	protected float m_deathParticleDuration = 0.5f;

	// Token: 0x04000DE4 RID: 3556
	private const string DEATH_POOF_EFFECT = "EnemyGenericDeathPuff_Small_Effect_MediumVariant";

	// Token: 0x04000DE5 RID: 3557
	private Color m_deathTintColor = new Color(0f, 0f, 0f, 0.5f);

	// Token: 0x04000DE6 RID: 3558
	protected bool m_isDashAttacking;

	// Token: 0x04000DE7 RID: 3559
	private MaterialPropertyBlock m_matPropertyBlock;

	// Token: 0x04000DE8 RID: 3560
	private bool m_playerWasInDashWakeRange;

	// Token: 0x04000DE9 RID: 3561
	private bool m_wasGrounded = true;

	// Token: 0x04000DEA RID: 3562
	private EventInstance m_dashEventInstance;

	// Token: 0x04000DEB RID: 3563
	private Action<object, CharacterHitEventArgs> m_onPlayerHit;

	// Token: 0x04000DEC RID: 3564
	protected float m_verticalShot_TellHold_AnimationSpeed = 1.25f;

	// Token: 0x04000DED RID: 3565
	protected float m_verticalShot_TellHold_Delay = 1.25f;

	// Token: 0x04000DEE RID: 3566
	protected float m_verticalShot_AttackIntro_AnimationSpeed = 1.25f;

	// Token: 0x04000DEF RID: 3567
	protected float m_verticalShot_AttackIntro_Delay;

	// Token: 0x04000DF0 RID: 3568
	protected float m_verticalShot_AttackHold_AnimationSpeed = 1.25f;

	// Token: 0x04000DF1 RID: 3569
	protected float m_verticalShot_AttackHold_Delay = 0.25f;

	// Token: 0x04000DF2 RID: 3570
	protected float m_verticalShot_Exit_AnimationSpeed = 0.65f;

	// Token: 0x04000DF3 RID: 3571
	protected float m_verticalShot_Exit_Delay;

	// Token: 0x04000DF4 RID: 3572
	protected float m_verticalShot_Exit_ForceIdle = 0.15f;

	// Token: 0x04000DF5 RID: 3573
	protected float m_verticalShot_Exit_AttackCD = 10f;

	// Token: 0x04000DF6 RID: 3574
	protected float m_dashAttack_TellIntro_AnimationSpeed = 1.25f;

	// Token: 0x04000DF7 RID: 3575
	protected float m_dashAttack_TellHold_AnimationSpeed = 1.25f;

	// Token: 0x04000DF8 RID: 3576
	protected float m_dashAttack_TellIntroAndHold_Delay = 0.75f;

	// Token: 0x04000DF9 RID: 3577
	protected float m_dashAttack_AttackIntro_AnimationSpeed = 1.25f;

	// Token: 0x04000DFA RID: 3578
	protected float m_dashAttack_AttackIntro_Delay;

	// Token: 0x04000DFB RID: 3579
	protected float m_dashAttack_AttackHold_AnimationSpeed = 1.25f;

	// Token: 0x04000DFC RID: 3580
	protected float m_dashAttack_AttackHold_Delay = 0.15f;

	// Token: 0x04000DFD RID: 3581
	protected float m_dashAttack_Exit_AnimationSpeed = 1.25f;

	// Token: 0x04000DFE RID: 3582
	protected float m_dashAttack_Exit_Delay;

	// Token: 0x04000DFF RID: 3583
	protected float m_dashAttack_Exit_ForceIdle = 0.25f;
}
