using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using UnityEngine;

// Token: 0x020002C4 RID: 708
public class Comet_Ability : BaseAbility_RL, ITalent, IAbility, IPersistentAbility
{
	// Token: 0x170009BF RID: 2495
	// (get) Token: 0x060014E6 RID: 5350 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public override bool IgnoreStuckCheck
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170009C0 RID: 2496
	// (get) Token: 0x060014E7 RID: 5351 RVA: 0x0000A726 File Offset: 0x00008926
	public bool IsPersistentActive
	{
		get
		{
			return this.m_isComet;
		}
	}

	// Token: 0x060014E8 RID: 5352 RVA: 0x0000A72E File Offset: 0x0000892E
	protected override void Awake()
	{
		this.m_onPlayerJump = new Action<MonoBehaviour, EventArgs>(this.OnPlayerJump);
		this.m_waitYield = new WaitRL_Yield(0f, false);
		base.Awake();
	}

	// Token: 0x060014E9 RID: 5353 RVA: 0x0000A759 File Offset: 0x00008959
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		base.Initialize(abilityController, castAbilityType);
		this.m_manaRequiredForNextCharge = 1000f;
		if (!this.m_cometSFXInstance.isValid())
		{
			this.m_cometSFXInstance = AudioUtility.GetEventInstance("event:/SFX/Spells/sfx_spell_astromancer_cometForm_loop", base.transform);
		}
	}

	// Token: 0x060014EA RID: 5354 RVA: 0x0000A689 File Offset: 0x00008889
	public override void PreCastAbility()
	{
		this.m_abilityController.StopAllQueueCoroutines();
		base.PreCastAbility();
	}

	// Token: 0x060014EB RID: 5355 RVA: 0x0000A791 File Offset: 0x00008991
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerJump, this.m_onPlayerJump);
		if (this.m_cometSFXInstance.isValid())
		{
			this.m_cometSFXInstance.release();
		}
	}

	// Token: 0x060014EC RID: 5356 RVA: 0x0000A7BF File Offset: 0x000089BF
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

	// Token: 0x060014ED RID: 5357 RVA: 0x0000A63F File Offset: 0x0000883F
	private void OnPlayerJump(MonoBehaviour sender, EventArgs args)
	{
		this.StopAbility(true);
	}

	// Token: 0x060014EE RID: 5358 RVA: 0x000884C4 File Offset: 0x000866C4
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

	// Token: 0x060014EF RID: 5359 RVA: 0x00088594 File Offset: 0x00086794
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

	// Token: 0x060014F0 RID: 5360 RVA: 0x00088638 File Offset: 0x00086838
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

	// Token: 0x060014F1 RID: 5361 RVA: 0x0000A63F File Offset: 0x0000883F
	public void StopPersistentAbility()
	{
		this.StopAbility(true);
	}

	// Token: 0x060014F2 RID: 5362 RVA: 0x0000A7CE File Offset: 0x000089CE
	private void ResumeCooldown(Projectile_RL proj, GameObject obj)
	{
		base.DecreaseCooldownOverTime = true;
		base.DisplayPausedAbilityCooldown = false;
	}

	// Token: 0x04001646 RID: 5702
	private const string SFX_COMET_LOOP_NAME = "event:/SFX/Spells/sfx_spell_astromancer_cometForm_loop";

	// Token: 0x04001647 RID: 5703
	private bool m_isComet;

	// Token: 0x04001648 RID: 5704
	private WaitRL_Yield m_waitYield;

	// Token: 0x04001649 RID: 5705
	private Action<MonoBehaviour, EventArgs> m_onPlayerJump;

	// Token: 0x0400164A RID: 5706
	private Action<MonoBehaviour, EventArgs> m_onPlayerManaChange;

	// Token: 0x0400164B RID: 5707
	private float m_manaRequiredForNextCharge;

	// Token: 0x0400164C RID: 5708
	private EventInstance m_cometSFXInstance;

	// Token: 0x0400164D RID: 5709
	private bool m_cometSFXParamIsMoving;
}
