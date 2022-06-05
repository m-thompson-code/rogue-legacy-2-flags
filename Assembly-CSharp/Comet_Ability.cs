using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using UnityEngine;

// Token: 0x0200017E RID: 382
public class Comet_Ability : BaseAbility_RL, ITalent, IAbility, IPersistentAbility
{
	// Token: 0x17000747 RID: 1863
	// (get) Token: 0x06000D77 RID: 3447 RVA: 0x000291A7 File Offset: 0x000273A7
	public override bool IgnoreStuckCheck
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000748 RID: 1864
	// (get) Token: 0x06000D78 RID: 3448 RVA: 0x000291AA File Offset: 0x000273AA
	public bool IsPersistentActive
	{
		get
		{
			return this.m_isComet;
		}
	}

	// Token: 0x06000D79 RID: 3449 RVA: 0x000291B2 File Offset: 0x000273B2
	protected override void Awake()
	{
		this.m_onPlayerJump = new Action<MonoBehaviour, EventArgs>(this.OnPlayerJump);
		this.m_waitYield = new WaitRL_Yield(0f, false);
		base.Awake();
	}

	// Token: 0x06000D7A RID: 3450 RVA: 0x000291DD File Offset: 0x000273DD
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		base.Initialize(abilityController, castAbilityType);
		this.m_manaRequiredForNextCharge = 1000f;
		if (!this.m_cometSFXInstance.isValid())
		{
			this.m_cometSFXInstance = AudioUtility.GetEventInstance("event:/SFX/Spells/sfx_spell_astromancer_cometForm_loop", base.transform);
		}
	}

	// Token: 0x06000D7B RID: 3451 RVA: 0x00029215 File Offset: 0x00027415
	public override void PreCastAbility()
	{
		this.m_abilityController.StopAllQueueCoroutines();
		base.PreCastAbility();
	}

	// Token: 0x06000D7C RID: 3452 RVA: 0x00029228 File Offset: 0x00027428
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerJump, this.m_onPlayerJump);
		if (this.m_cometSFXInstance.isValid())
		{
			this.m_cometSFXInstance.release();
		}
	}

	// Token: 0x06000D7D RID: 3453 RVA: 0x00029256 File Offset: 0x00027456
	public override IEnumerator CastAbility()
	{
		this.m_beginCastingRelay.Dispatch();
		PlayerController playerController = this.m_abilityController.PlayerController;
		if (!this.m_isComet)
		{
			this.m_isComet = true;
			this.m_abilityController.StopAbility(CastAbilityType.Weapon, true);
			this.m_abilityController.StopAbility(CastAbilityType.Spell, true);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerJump, this.m_onPlayerJump);
			float num = 1.5f;
			playerController.CharacterHitResponse.StopInvincibleTime();
			playerController.CharacterHitResponse.SetInvincibleTime(num, false, false);
			playerController.HitboxController.SetHitboxActiveState(HitboxType.Body, false);
			playerController.Visuals.SetActive(false);
			this.FireProjectile();
			this.m_abilityController.BroadcastAbilityCastEvents(base.CastAbilityType);
			if (!playerController.CharacterFlight.IsFlying)
			{
				playerController.CharacterFlight.StartFlight(num, 1f);
			}
			else
			{
				playerController.CharacterFlight.MovementSpeedMultiplier += 1f;
				playerController.CharacterFlight.ResetIsAssistFlying();
			}
			playerController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Cloak, num);
			this.m_waitYield.CreateNew(1.5f, false);
			yield return this.m_waitYield;
			bool isComet = this.m_isComet;
			yield break;
		}
		yield break;
	}

	// Token: 0x06000D7E RID: 3454 RVA: 0x00029265 File Offset: 0x00027465
	private void OnPlayerJump(MonoBehaviour sender, EventArgs args)
	{
		this.StopAbility(true);
	}

	// Token: 0x06000D7F RID: 3455 RVA: 0x00029270 File Offset: 0x00027470
	public override void StopAbility(bool abilityInterrupted)
	{
		if (this.m_isComet)
		{
			PlayerController playerController = this.m_abilityController.PlayerController;
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerJump, this.m_onPlayerJump);
			playerController.CharacterHitResponse.SetInvincibleTime(0.1f, false, false);
			playerController.HitboxController.SetHitboxActiveState(HitboxType.Body, true);
			playerController.Visuals.SetActive(true);
			if (this.m_firedProjectile)
			{
				this.m_firedProjectile.FlagForDestruction(null);
			}
			playerController.CharacterFlight.StopFlight();
			playerController.CharacterFlight.MovementSpeedMultiplier -= 1f;
			playerController.StatusBarController.StopUIEffect(StatusBarEntryType.Cloak);
		}
		this.m_isComet = false;
		this.m_cometSFXParamIsMoving = false;
		if (this.m_cometSFXInstance.isValid())
		{
			AudioManager.Stop(this.m_cometSFXInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x06000D80 RID: 3456 RVA: 0x00029340 File Offset: 0x00027540
	protected override void FireProjectile()
	{
		base.FireProjectile();
		if (this.m_cometSFXInstance.isValid())
		{
			AudioManager.PlayAttached(null, this.m_cometSFXInstance, this.m_abilityController.PlayerController.gameObject);
			this.m_cometSFXInstance.setParameterByName("cometFormSpeed", 0f, false);
			this.m_cometSFXParamIsMoving = false;
		}
		if (base.DecreaseCooldownOverTime)
		{
			this.StartCooldownTimer();
			base.DecreaseCooldownOverTime = false;
			base.DisplayPausedAbilityCooldown = true;
			if (this.m_firedProjectile)
			{
				this.m_firedProjectile.OnDeathRelay.AddOnce(new Action<Projectile_RL, GameObject>(this.ResumeCooldown), false);
			}
		}
	}

	// Token: 0x06000D81 RID: 3457 RVA: 0x000293E4 File Offset: 0x000275E4
	protected override void Update()
	{
		base.Update();
		if (base.AbilityActive && this.m_cometSFXInstance.isValid())
		{
			PLAYBACK_STATE playback_STATE;
			this.m_cometSFXInstance.getPlaybackState(out playback_STATE);
			if (playback_STATE == PLAYBACK_STATE.PLAYING)
			{
				if (this.m_cometSFXParamIsMoving && this.m_abilityController.PlayerController.Velocity.x == 0f && this.m_abilityController.PlayerController.Velocity.y == 0f)
				{
					this.m_cometSFXParamIsMoving = false;
					this.m_cometSFXInstance.setParameterByName("cometFormSpeed", 0f, false);
					return;
				}
				if (!this.m_cometSFXParamIsMoving && (this.m_abilityController.PlayerController.Velocity.x != 0f || this.m_abilityController.PlayerController.Velocity.y != 0f))
				{
					this.m_cometSFXParamIsMoving = true;
					this.m_cometSFXInstance.setParameterByName("cometFormSpeed", 1f, false);
				}
			}
		}
	}

	// Token: 0x06000D82 RID: 3458 RVA: 0x000294E3 File Offset: 0x000276E3
	public void StopPersistentAbility()
	{
		this.StopAbility(true);
	}

	// Token: 0x06000D83 RID: 3459 RVA: 0x000294EC File Offset: 0x000276EC
	private void ResumeCooldown(Projectile_RL proj, GameObject obj)
	{
		base.DecreaseCooldownOverTime = true;
		base.DisplayPausedAbilityCooldown = false;
	}

	// Token: 0x040010DA RID: 4314
	private const string SFX_COMET_LOOP_NAME = "event:/SFX/Spells/sfx_spell_astromancer_cometForm_loop";

	// Token: 0x040010DB RID: 4315
	private bool m_isComet;

	// Token: 0x040010DC RID: 4316
	private WaitRL_Yield m_waitYield;

	// Token: 0x040010DD RID: 4317
	private Action<MonoBehaviour, EventArgs> m_onPlayerJump;

	// Token: 0x040010DE RID: 4318
	private Action<MonoBehaviour, EventArgs> m_onPlayerManaChange;

	// Token: 0x040010DF RID: 4319
	private float m_manaRequiredForNextCharge;

	// Token: 0x040010E0 RID: 4320
	private EventInstance m_cometSFXInstance;

	// Token: 0x040010E1 RID: 4321
	private bool m_cometSFXParamIsMoving;
}
