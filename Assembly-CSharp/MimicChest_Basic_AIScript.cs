using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using UnityEngine;

// Token: 0x020000FA RID: 250
public class MimicChest_Basic_AIScript : BaseAIScript
{
	// Token: 0x060007A6 RID: 1958 RVA: 0x0001AAC4 File Offset: 0x00018CC4
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

	// Token: 0x17000420 RID: 1056
	// (get) Token: 0x060007A7 RID: 1959 RVA: 0x0001AAF2 File Offset: 0x00018CF2
	public override bool ForceDeathAnimation
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000421 RID: 1057
	// (get) Token: 0x060007A8 RID: 1960 RVA: 0x0001AAF5 File Offset: 0x00018CF5
	protected virtual string JumpAudioEventPath
	{
		get
		{
			return "event:/SFX/Enemies/sfx_enemy_mimicChest_jump";
		}
	}

	// Token: 0x17000422 RID: 1058
	// (get) Token: 0x060007A9 RID: 1961 RVA: 0x0001AAFC File Offset: 0x00018CFC
	protected virtual string LandAudioEventPath
	{
		get
		{
			return "event:/SFX/Enemies/sfx_enemy_mimicChest_land";
		}
	}

	// Token: 0x17000423 RID: 1059
	// (get) Token: 0x060007AA RID: 1962 RVA: 0x0001AB03 File Offset: 0x00018D03
	protected virtual string DeathHitAudioEventPath
	{
		get
		{
			return "event:/SFX/Enemies/sfx_enemy_mimicChest_death_killingBlow";
		}
	}

	// Token: 0x17000424 RID: 1060
	// (get) Token: 0x060007AB RID: 1963 RVA: 0x0001AB0A File Offset: 0x00018D0A
	private string DashLoopAudioEventPath
	{
		get
		{
			return "event:/SFX/Enemies/sfx_enemy_mimicChest_dash_start_loop";
		}
	}

	// Token: 0x17000425 RID: 1061
	// (get) Token: 0x060007AC RID: 1964 RVA: 0x0001AB11 File Offset: 0x00018D11
	private string ChangeDirectionAudioEventPath
	{
		get
		{
			return "event:/SFX/Enemies/sfx_enemy_mimicChest_dash_changeDirection";
		}
	}

	// Token: 0x17000426 RID: 1062
	// (get) Token: 0x060007AD RID: 1965 RVA: 0x0001AB18 File Offset: 0x00018D18
	protected virtual Vector2 JumpHeight
	{
		get
		{
			return new Vector2(17f, 17f);
		}
	}

	// Token: 0x17000427 RID: 1063
	// (get) Token: 0x060007AE RID: 1966 RVA: 0x0001AB29 File Offset: 0x00018D29
	protected virtual int NumCoinsFiredOnHit
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x17000428 RID: 1064
	// (get) Token: 0x060007AF RID: 1967 RVA: 0x0001AB2C File Offset: 0x00018D2C
	protected virtual Vector2 CoinFireAngle_OnHit
	{
		get
		{
			return new Vector2(80f, 100f);
		}
	}

	// Token: 0x17000429 RID: 1065
	// (get) Token: 0x060007B0 RID: 1968 RVA: 0x0001AB3D File Offset: 0x00018D3D
	protected virtual Vector2 CoinFireSpeed_OnHit
	{
		get
		{
			return new Vector2(1.25f, 1.65f);
		}
	}

	// Token: 0x1700042A RID: 1066
	// (get) Token: 0x060007B1 RID: 1969 RVA: 0x0001AB4E File Offset: 0x00018D4E
	protected virtual int NumCoinsFiredOnLanding
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x1700042B RID: 1067
	// (get) Token: 0x060007B2 RID: 1970 RVA: 0x0001AB51 File Offset: 0x00018D51
	protected virtual Vector2 CoinFireAngle_OnLand
	{
		get
		{
			return new Vector2(80f, 100f);
		}
	}

	// Token: 0x1700042C RID: 1068
	// (get) Token: 0x060007B3 RID: 1971 RVA: 0x0001AB62 File Offset: 0x00018D62
	protected virtual Vector2 CoinFireSpeed_OnLand
	{
		get
		{
			return new Vector2(1.25f, 1.65f);
		}
	}

	// Token: 0x1700042D RID: 1069
	// (get) Token: 0x060007B4 RID: 1972 RVA: 0x0001AB73 File Offset: 0x00018D73
	protected virtual int NumCoinsFiredOnDeath
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x1700042E RID: 1070
	// (get) Token: 0x060007B5 RID: 1973 RVA: 0x0001AB76 File Offset: 0x00018D76
	protected virtual Vector2 CoinFireAngle_OnDeath
	{
		get
		{
			return new Vector2(68f, 112f);
		}
	}

	// Token: 0x1700042F RID: 1071
	// (get) Token: 0x060007B6 RID: 1974 RVA: 0x0001AB87 File Offset: 0x00018D87
	protected virtual Vector2 CoinFireSpeed_OnDeath
	{
		get
		{
			return new Vector2(0.75f, 1f);
		}
	}

	// Token: 0x17000430 RID: 1072
	// (get) Token: 0x060007B7 RID: 1975 RVA: 0x0001AB98 File Offset: 0x00018D98
	protected virtual int DeathCoinAttack_CoinSpreadAmount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000431 RID: 1073
	// (get) Token: 0x060007B8 RID: 1976 RVA: 0x0001AB9B File Offset: 0x00018D9B
	protected virtual float DeathCoinAttack_AngleSpread
	{
		get
		{
			return 18f;
		}
	}

	// Token: 0x17000432 RID: 1074
	// (get) Token: 0x060007B9 RID: 1977 RVA: 0x0001ABA2 File Offset: 0x00018DA2
	protected virtual bool m_advancedBoss
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000433 RID: 1075
	// (get) Token: 0x060007BA RID: 1978 RVA: 0x0001ABA5 File Offset: 0x00018DA5
	protected virtual float m_airborneKO_AnimationDelay
	{
		get
		{
			return 0.65f;
		}
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x0001ABAC File Offset: 0x00018DAC
	private void Awake()
	{
		this.m_onPlayerHit = new Action<object, CharacterHitEventArgs>(this.OnPlayerHit);
	}

	// Token: 0x060007BC RID: 1980 RVA: 0x0001ABC0 File Offset: 0x00018DC0
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

	// Token: 0x060007BD RID: 1981 RVA: 0x0001AC9E File Offset: 0x00018E9E
	private void OnEnable()
	{
		if (PlayerManager.IsInstantiated && !PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().CharacterHitResponse.OnCharacterHitRelay.AddListener(this.m_onPlayerHit, false);
		}
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x0001ACCC File Offset: 0x00018ECC
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

	// Token: 0x060007BF RID: 1983 RVA: 0x0001ADC4 File Offset: 0x00018FC4
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

	// Token: 0x060007C0 RID: 1984 RVA: 0x0001AE6C File Offset: 0x0001906C
	private void OnEnemyHit(object sender, CharacterHitEventArgs args)
	{
		base.EnemyController.StatusEffectController.SetAllStatusEffectsHidden(false);
		base.EnemyController.DisableOffscreenWarnings = false;
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x0001AE8C File Offset: 0x0001908C
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

	// Token: 0x060007C2 RID: 1986 RVA: 0x0001AF08 File Offset: 0x00019108
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

	// Token: 0x060007C3 RID: 1987 RVA: 0x0001AF7C File Offset: 0x0001917C
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

	// Token: 0x17000434 RID: 1076
	// (get) Token: 0x060007C4 RID: 1988 RVA: 0x0001B003 File Offset: 0x00019203
	protected virtual string m_verticalShotProjectileName
	{
		get
		{
			return "MimicCoinProjectile";
		}
	}

	// Token: 0x17000435 RID: 1077
	// (get) Token: 0x060007C5 RID: 1989 RVA: 0x0001B00A File Offset: 0x0001920A
	protected virtual int m_verticalShot_TotalShotSpread
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x17000436 RID: 1078
	// (get) Token: 0x060007C6 RID: 1990 RVA: 0x0001B00D File Offset: 0x0001920D
	protected virtual int m_verticalShot_TotalLoops
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x17000437 RID: 1079
	// (get) Token: 0x060007C7 RID: 1991 RVA: 0x0001B010 File Offset: 0x00019210
	protected virtual int m_verticalShot_InitialAngle
	{
		get
		{
			return 90;
		}
	}

	// Token: 0x17000438 RID: 1080
	// (get) Token: 0x060007C8 RID: 1992 RVA: 0x0001B014 File Offset: 0x00019214
	protected virtual Vector2 m_verticalShot_RandomAngleAngleOffset
	{
		get
		{
			return new Vector2(-21f, 3f);
		}
	}

	// Token: 0x17000439 RID: 1081
	// (get) Token: 0x060007C9 RID: 1993 RVA: 0x0001B025 File Offset: 0x00019225
	protected virtual float m_verticalShot_LoopDelay
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x1700043A RID: 1082
	// (get) Token: 0x060007CA RID: 1994 RVA: 0x0001B02C File Offset: 0x0001922C
	protected virtual float m_verticalShot_SpeedMod
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x0001B033 File Offset: 0x00019233
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

	// Token: 0x1700043B RID: 1083
	// (get) Token: 0x060007CC RID: 1996 RVA: 0x0001B042 File Offset: 0x00019242
	protected virtual float m_dashAttack_Exit_AttackCD
	{
		get
		{
			return 6f;
		}
	}

	// Token: 0x1700043C RID: 1084
	// (get) Token: 0x060007CD RID: 1997 RVA: 0x0001B049 File Offset: 0x00019249
	protected virtual float m_dashAttackSpeed
	{
		get
		{
			return 22f;
		}
	}

	// Token: 0x1700043D RID: 1085
	// (get) Token: 0x060007CE RID: 1998 RVA: 0x0001B050 File Offset: 0x00019250
	protected virtual Vector2 m_dashAttackDuration
	{
		get
		{
			return new Vector2(2f, 2f);
		}
	}

	// Token: 0x1700043E RID: 1086
	// (get) Token: 0x060007CF RID: 1999 RVA: 0x0001B061 File Offset: 0x00019261
	protected virtual bool m_canDashWake
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700043F RID: 1087
	// (get) Token: 0x060007D0 RID: 2000 RVA: 0x0001B064 File Offset: 0x00019264
	protected virtual float m_dashWakeActivationRange
	{
		get
		{
			return 5.5f;
		}
	}

	// Token: 0x17000440 RID: 1088
	// (get) Token: 0x060007D1 RID: 2001 RVA: 0x0001B06B File Offset: 0x0001926B
	protected virtual float m_dashWakeAttackOdds
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x0001B072 File Offset: 0x00019272
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

	// Token: 0x060007D3 RID: 2003 RVA: 0x0001B081 File Offset: 0x00019281
	protected virtual void OnStartDashing()
	{
		AudioManager.PlayAttached(this, this.m_dashEventInstance, base.gameObject);
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x0001B095 File Offset: 0x00019295
	protected virtual void OnStopDashing()
	{
		AudioManager.Stop(this.m_dashEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x0001B0A3 File Offset: 0x000192A3
	private void OnJump()
	{
		AudioManager.PlayOneShotAttached(this, this.JumpAudioEventPath, base.gameObject);
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x0001B0B7 File Offset: 0x000192B7
	private void OnGrounded()
	{
		AudioManager.PlayOneShotAttached(this, this.LandAudioEventPath, base.gameObject);
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x0001B0CB File Offset: 0x000192CB
	private IEnumerator OnBeginDeathAnim()
	{
		AudioManager.PlayOneShotAttached(this, this.DeathHitAudioEventPath, base.gameObject);
		if (base.EnemyController.IsBoss)
		{
			yield return base.DeathAnim();
		}
		yield break;
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x0001B0DA File Offset: 0x000192DA
	public override void OnLBCompleteOrCancelled()
	{
		base.EnemyController.DisableFriction = false;
		base.EnemyController.LockFlip = false;
		this.m_isDashAttacking = false;
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x0001B101 File Offset: 0x00019301
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

	// Token: 0x060007DA RID: 2010 RVA: 0x0001B110 File Offset: 0x00019310
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

	// Token: 0x060007DB RID: 2011 RVA: 0x0001B120 File Offset: 0x00019320
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

	// Token: 0x060007DC RID: 2012 RVA: 0x0001B300 File Offset: 0x00019500
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

	// Token: 0x04000B2B RID: 2859
	protected float m_death_AnimationSpeed = 1f;

	// Token: 0x04000B2C RID: 2860
	protected float m_death_AnimationDelay = 0.5f;

	// Token: 0x04000B2D RID: 2861
	protected float m_airborneKO_AnimationSpeed = 1f;

	// Token: 0x04000B2E RID: 2862
	protected float m_groundedKOAnimationSpeed = 1f;

	// Token: 0x04000B2F RID: 2863
	protected float m_groundedKOAnimationDelay = 0.5f;

	// Token: 0x04000B30 RID: 2864
	protected bool m_playDeathParticlesOnZeroHP = true;

	// Token: 0x04000B31 RID: 2865
	protected float m_deathParticleDuration = 0.5f;

	// Token: 0x04000B32 RID: 2866
	private const string DEATH_POOF_EFFECT = "EnemyGenericDeathPuff_Small_Effect_MediumVariant";

	// Token: 0x04000B33 RID: 2867
	private Color m_deathTintColor = new Color(0f, 0f, 0f, 0.5f);

	// Token: 0x04000B34 RID: 2868
	protected bool m_isDashAttacking;

	// Token: 0x04000B35 RID: 2869
	private MaterialPropertyBlock m_matPropertyBlock;

	// Token: 0x04000B36 RID: 2870
	private bool m_playerWasInDashWakeRange;

	// Token: 0x04000B37 RID: 2871
	private bool m_wasGrounded = true;

	// Token: 0x04000B38 RID: 2872
	private EventInstance m_dashEventInstance;

	// Token: 0x04000B39 RID: 2873
	private Action<object, CharacterHitEventArgs> m_onPlayerHit;

	// Token: 0x04000B3A RID: 2874
	protected float m_verticalShot_TellHold_AnimationSpeed = 1.25f;

	// Token: 0x04000B3B RID: 2875
	protected float m_verticalShot_TellHold_Delay = 1.25f;

	// Token: 0x04000B3C RID: 2876
	protected float m_verticalShot_AttackIntro_AnimationSpeed = 1.25f;

	// Token: 0x04000B3D RID: 2877
	protected float m_verticalShot_AttackIntro_Delay;

	// Token: 0x04000B3E RID: 2878
	protected float m_verticalShot_AttackHold_AnimationSpeed = 1.25f;

	// Token: 0x04000B3F RID: 2879
	protected float m_verticalShot_AttackHold_Delay = 0.25f;

	// Token: 0x04000B40 RID: 2880
	protected float m_verticalShot_Exit_AnimationSpeed = 0.65f;

	// Token: 0x04000B41 RID: 2881
	protected float m_verticalShot_Exit_Delay;

	// Token: 0x04000B42 RID: 2882
	protected float m_verticalShot_Exit_ForceIdle = 0.15f;

	// Token: 0x04000B43 RID: 2883
	protected float m_verticalShot_Exit_AttackCD = 10f;

	// Token: 0x04000B44 RID: 2884
	protected float m_dashAttack_TellIntro_AnimationSpeed = 1.25f;

	// Token: 0x04000B45 RID: 2885
	protected float m_dashAttack_TellHold_AnimationSpeed = 1.25f;

	// Token: 0x04000B46 RID: 2886
	protected float m_dashAttack_TellIntroAndHold_Delay = 0.75f;

	// Token: 0x04000B47 RID: 2887
	protected float m_dashAttack_AttackIntro_AnimationSpeed = 1.25f;

	// Token: 0x04000B48 RID: 2888
	protected float m_dashAttack_AttackIntro_Delay;

	// Token: 0x04000B49 RID: 2889
	protected float m_dashAttack_AttackHold_AnimationSpeed = 1.25f;

	// Token: 0x04000B4A RID: 2890
	protected float m_dashAttack_AttackHold_Delay = 0.15f;

	// Token: 0x04000B4B RID: 2891
	protected float m_dashAttack_Exit_AnimationSpeed = 1.25f;

	// Token: 0x04000B4C RID: 2892
	protected float m_dashAttack_Exit_Delay;

	// Token: 0x04000B4D RID: 2893
	protected float m_dashAttack_Exit_ForceIdle = 0.25f;
}
