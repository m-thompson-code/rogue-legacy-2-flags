using System;
using System.Collections;
using FMOD.Studio;
using Rewired;
using RLAudio;
using UnityEngine;

// Token: 0x020006B7 RID: 1719
public class EarthShiftTriggerController : MonoBehaviour
{
	// Token: 0x060034F9 RID: 13561 RVA: 0x000DF068 File Offset: 0x000DD268
	private void Awake()
	{
		this.m_interactable = base.GetComponent<Interactable>();
		this.m_animator = base.GetComponent<Animator>();
		this.m_prepWaitYield = new WaitRL_Yield(0f, false);
		this.m_blinkWaitYield = new WaitRL_Yield(0f, false);
		this.m_matPropBlock = new MaterialPropertyBlock();
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
		this.m_onPlayerHit = new Action<MonoBehaviour, EventArgs>(this.OnPlayerHit);
		this.m_onInteractReleased = new Action<InputActionEventData>(this.OnInteractReleased);
	}

	// Token: 0x060034FA RID: 13562 RVA: 0x0001D114 File Offset: 0x0001B314
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x060034FB RID: 13563 RVA: 0x000DF0F0 File Offset: 0x000DD2F0
	private void OnPlayerEnterRoom(MonoBehaviour sender, EventArgs args)
	{
		this.m_largeHasPlayed = false;
		this.m_initialDelayOverride = base.GetComponent<Prop>().PropSpawnController.GetComponent<EarthShiftTrigger_InitialDelayOverrideHelper>().InitialDelayOverride;
		this.m_hasEarthShiftHeirloom = (SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockEarthShift) > 0);
		LayerMask platformMask = PlayerManager.GetPlayerController().ControllerCorgi.PlatformMask;
		RaycastHit2D hit = Physics2D.Raycast(base.transform.position, Vector2.down, 5f, platformMask);
		if (hit)
		{
			base.transform.SetParent(hit.transform);
		}
	}

	// Token: 0x060034FC RID: 13564
	public void OnInteract()
	{
		this.m_state = EarthShiftTriggerController.ShiftState.Prep;
		if (this.m_isLarge)
		{
			this.m_largeHasPlayed = true;
		}
		global::PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.StopActiveAbilities(true);
		playerController.SetVelocity(0f, 0f, false);
		playerController.Animator.SetBool("Kneeling", true);
		this.m_animator.SetBool("SymbolLit", true);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerHit, this.m_onPlayerHit);
		Rewired_RL.Player.AddInputEventDelegate(this.m_onInteractReleased, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Interact");
		this.m_prepCoroutine = base.StartCoroutine(this.PrepCoroutine());
	}

	// Token: 0x060034FD RID: 13565 RVA: 0x000DF224 File Offset: 0x000DD424
	private void StopInteraction()
	{
		EarthShiftTriggerController.ShiftState state = this.m_state;
		this.m_state = EarthShiftTriggerController.ShiftState.Inactive;
		if (this.m_prepCoroutine != null)
		{
			base.StopCoroutine(this.m_prepCoroutine);
			this.m_prepCoroutine = null;
		}
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHit, this.m_onPlayerHit);
		Rewired_RL.Player.RemoveInputEventDelegate(this.m_onInteractReleased, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Interact");
		PlayerManager.GetPlayerController().Animator.SetBool("Kneeling", false);
		this.m_animator.SetBool("SymbolLit", false);
		this.m_animator.SetBool("Floating", false);
		if (this.m_chargeSFXInstance.isValid())
		{
			AudioManager.Stop(this.m_chargeSFXInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		if (this.m_activeLoopSFXInstance.isValid())
		{
			AudioManager.Stop(this.m_activeLoopSFXInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		if (this.m_movePlatformSFXInstance.isValid())
		{
			AudioManager.Stop(this.m_movePlatformSFXInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		if (this.m_platform)
		{
			string path = null;
			string path2 = null;
			switch (this.m_platform.PlatformSize)
			{
			case EarthShiftPlatformController.EarthShiftPlatformSize.Small:
				path = "event:/SFX/Interactables/sfx_bindingStone_small_deactivate";
				path2 = "event:/SFX/Interactables/sfx_bindingStone_small_player_release";
				break;
			case EarthShiftPlatformController.EarthShiftPlatformSize.Medium:
				path = "event:/SFX/Interactables/sfx_bindingStone_med_deactivate";
				path2 = "event:/SFX/Interactables/sfx_bindingStone_med_player_release";
				break;
			case EarthShiftPlatformController.EarthShiftPlatformSize.Large:
			case EarthShiftPlatformController.EarthShiftPlatformSize.Final:
				path = "event:/SFX/Interactables/sfx_bindingStone_large_deactivate";
				path2 = "event:/SFX/Interactables/sfx_bindingStone_large_player_release";
				break;
			}
			if (!this.m_isLarge)
			{
				if (state == EarthShiftTriggerController.ShiftState.Active)
				{
					AudioManager.PlayOneShot(null, path, this.m_platform.transform.position);
				}
				AudioManager.PlayOneShot(null, path2, base.transform.position);
			}
		}
	}

	// Token: 0x060034FE RID: 13566 RVA: 0x0001D122 File Offset: 0x0001B322
	private void OnInteractReleased(InputActionEventData data)
	{
		if (!this.m_isLarge)
		{
			this.StopInteraction();
		}
	}

	// Token: 0x060034FF RID: 13567 RVA: 0x0001D132 File Offset: 0x0001B332
	private void OnPlayerHit(MonoBehaviour sender, EventArgs args)
	{
		this.StopInteraction();
	}

	// Token: 0x06003500 RID: 13568 RVA: 0x0001D13A File Offset: 0x0001B33A
	private IEnumerator PrepCoroutine()
	{
		if (this.m_chargeSFXInstance.isValid())
		{
			AudioManager.PlayAttached(null, this.m_chargeSFXInstance, base.gameObject);
			this.m_chargeSFXInstance.setParameterByName("bindingStoneCharge", 0f, false);
		}
		float waitTime = (this.m_initialDelayOverride == -1f) ? 1.25f : this.m_initialDelayOverride;
		if (this.m_chargeSFXInstance.isValid())
		{
			float waitTimeCounter = Time.time + waitTime;
			while (Time.time < waitTimeCounter)
			{
				yield return null;
				float value = Mathf.Clamp(1f - (waitTimeCounter - Time.time) / waitTime, 0f, 1f);
				this.m_chargeSFXInstance.setParameterByName("bindingStoneCharge", value, false);
			}
		}
		else
		{
			this.m_prepWaitYield.CreateNew(waitTime, false);
			yield return this.m_prepWaitYield;
		}
		if (this.m_chargeSFXInstance.isValid())
		{
			AudioManager.Stop(this.m_chargeSFXInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		this.m_state = EarthShiftTriggerController.ShiftState.Active;
		if (this.m_activeLoopSFXInstance.isValid())
		{
			AudioManager.PlayAttached(null, this.m_activeLoopSFXInstance, base.gameObject);
		}
		if (this.m_movePlatformSFXInstance.isValid())
		{
			AudioManager.PlayAttached(null, this.m_movePlatformSFXInstance, this.m_platform.gameObject);
		}
		if (this.m_animator)
		{
			this.m_animator.SetBool("Floating", true);
		}
		if (this.m_isLarge)
		{
			this.m_prepWaitYield.CreateNew(4f, false);
			yield return this.m_prepWaitYield;
			TunnelSpawnController tunnelSpawnController = PlayerManager.GetCurrentPlayerRoom().gameObject.FindObjectReference("AboveGroundTunnel", false, false);
			if (tunnelSpawnController && tunnelSpawnController.Tunnel)
			{
				tunnelSpawnController.Tunnel.ForceEnterTunnel(true, null);
			}
		}
		this.m_prepCoroutine = null;
		yield break;
	}

	// Token: 0x06003501 RID: 13569 RVA: 0x000DF394 File Offset: 0x000DD594
	private void Update()
	{
		global::PlayerController playerController = PlayerManager.GetPlayerController();
		this.m_interactable.SetIsInteractableActive(this.m_hasEarthShiftHeirloom && this.m_platform.Progress < 1f && !this.m_animator.GetCurrentAnimatorStateInfo(0).IsTag("InteractingWith"));
		bool flag = false;
		if (!this.m_isLarge)
		{
			flag = (this.m_hasEarthShiftHeirloom && this.m_platform.Progress < 1f && CDGHelper.DistanceBetweenPts(base.transform.position, playerController.Midpoint) <= this.m_glowDistance);
			this.m_animator.SetBool("PlayerNear", flag);
		}
		else if (playerController.IsGrounded)
		{
			flag = true;
			this.m_animator.SetBool("PlayerNear", flag);
		}
		if (!this.m_playingIdleProximityLoop && flag && this.m_state != EarthShiftTriggerController.ShiftState.Active)
		{
			if ((!this.m_isLarge || (this.m_isLarge && !this.m_largeHasPlayed)) && this.m_idleProximitySFXEvent.isValid())
			{
				AudioManager.Play(null, this.m_idleProximitySFXEvent, base.transform.position);
			}
			this.m_playingIdleProximityLoop = true;
		}
		else if (this.m_playingIdleProximityLoop && (!flag || this.m_state == EarthShiftTriggerController.ShiftState.Active))
		{
			if (this.m_idleProximitySFXEvent.isValid())
			{
				AudioManager.Stop(this.m_idleProximitySFXEvent, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			this.m_playingIdleProximityLoop = false;
		}
		if (this.m_state != EarthShiftTriggerController.ShiftState.Inactive)
		{
			playerController.StopActiveAbilities(true);
			playerController.SetVelocity(0f, 0f, false);
			if (this.m_state == EarthShiftTriggerController.ShiftState.Active)
			{
				Vector3 b = playerController.transform.position - base.transform.position;
				this.m_platform.Move();
				playerController.transform.position = base.transform.position + b;
				if (this.m_platform.Progress >= 1f)
				{
					if (this.m_platform.MovesThroughWater)
					{
						AudioManager.PlayOneShot(null, "event:/SFX/Interactables/sfx_stonePlatform_moveThroughWater_fast_stop", this.m_platform.transform.position);
					}
					else if (!string.IsNullOrWhiteSpace(this.m_platform.SFXNameStopOverride))
					{
						AudioManager.PlayOneShot(null, this.m_platform.SFXNameStopOverride, this.m_platform.transform.position);
					}
					else
					{
						switch (this.m_platform.PlatformSize)
						{
						case EarthShiftPlatformController.EarthShiftPlatformSize.Small:
							AudioManager.PlayOneShot(null, "event:/SFX/Interactables/sfx_stonePlatform_moveSmall_stop", this.m_platform.transform.position);
							break;
						case EarthShiftPlatformController.EarthShiftPlatformSize.Medium:
							AudioManager.PlayOneShot(null, "event:/SFX/Interactables/sfx_stonePlatform_moveMed_stop", this.m_platform.transform.position);
							break;
						case EarthShiftPlatformController.EarthShiftPlatformSize.Large:
							AudioManager.PlayOneShot(null, "event:/SFX/Interactables/sfx_stonePlatform_moveLarge_stop", this.m_platform.transform.position);
							break;
						}
					}
					this.StopInteraction();
				}
			}
			if (this.m_platform.Progress > 0f)
			{
				this.m_resetTimer = 0f;
				this.StopBlinking();
				return;
			}
		}
		else if (this.m_resetTimer >= 0f)
		{
			this.m_resetTimer += Time.deltaTime;
			if (this.m_blinkingCoroutine == null && this.m_resetTimer >= 3f)
			{
				this.m_blinkingCoroutine = base.StartCoroutine(this.BlinkCoroutine());
			}
			if (this.m_resetTimer >= 5f)
			{
				this.m_resetPositionCoroutine = base.StartCoroutine(this.ResetPositionCoroutine());
			}
		}
	}

	// Token: 0x06003502 RID: 13570
	private IEnumerator BlinkCoroutine()
	{
		if (this.m_platform.PlatformSize == EarthShiftPlatformController.EarthShiftPlatformSize.Final)
		{
			MainMenuWindowController.splitStep = 28;
		}
		string path = null;
		switch (this.m_platform.PlatformSize)
		{
		case EarthShiftPlatformController.EarthShiftPlatformSize.Small:
			path = "event:/SFX/Interactables/sfx_stonePlatform_moveSmall_reset_telegraph";
			break;
		case EarthShiftPlatformController.EarthShiftPlatformSize.Medium:
			path = "event:/SFX/Interactables/sfx_stonePlatform_moveMed_reset_telegraph";
			break;
		case EarthShiftPlatformController.EarthShiftPlatformSize.Large:
		case EarthShiftPlatformController.EarthShiftPlatformSize.Final:
			path = "event:/SFX/Interactables/sfx_stonePlatform_moveLarge_reset_telegraph";
			break;
		}
		AudioManager.PlayOneShot(null, path, this.m_platform.transform.position);
		for (;;)
		{
			this.m_renderer.color = this.m_flickerColor;
			this.m_platform.Renderer.GetPropertyBlock(this.m_matPropBlock);
			this.m_matPropBlock.SetColor(ShaderID_RL._MultiplyColor, this.m_flickerColor);
			this.m_platform.Renderer.SetPropertyBlock(this.m_matPropBlock);
			this.m_blinkWaitYield.CreateNew(0.1f, false);
			yield return this.m_blinkWaitYield;
			this.m_renderer.color = Color.white;
			this.m_platform.Renderer.GetPropertyBlock(this.m_matPropBlock);
			this.m_matPropBlock.SetColor(ShaderID_RL._MultiplyColor, Color.white);
			this.m_platform.Renderer.SetPropertyBlock(this.m_matPropBlock);
			this.m_blinkWaitYield.CreateNew(0.1f, false);
			yield return this.m_blinkWaitYield;
		}
		yield break;
	}

	// Token: 0x06003503 RID: 13571 RVA: 0x0001D158 File Offset: 0x0001B358
	private void StopBlinking()
	{
		if (this.m_blinkingCoroutine != null)
		{
			base.StopCoroutine(this.m_blinkingCoroutine);
			this.m_blinkingCoroutine = null;
			this.ResetColor();
		}
	}

	// Token: 0x06003504 RID: 13572 RVA: 0x0001D17B File Offset: 0x0001B37B
	private IEnumerator ResetPositionCoroutine()
	{
		this.m_resetTimer = -1f;
		this.StopBlinking();
		float startTime = Time.time;
		float endTime = startTime + 0.25f;
		string path = null;
		string appearEventPath = null;
		switch (this.m_platform.PlatformSize)
		{
		case EarthShiftPlatformController.EarthShiftPlatformSize.Small:
			path = "event:/SFX/Interactables/sfx_stonePlatform_moveSmall_reset_vanish";
			appearEventPath = "event:/SFX/Interactables/sfx_stonePlatform_moveSmall_reset_appear";
			break;
		case EarthShiftPlatformController.EarthShiftPlatformSize.Medium:
			path = "event:/SFX/Interactables/sfx_stonePlatform_moveMed_reset_vanish";
			appearEventPath = "event:/SFX/Interactables/sfx_stonePlatform_moveMed_reset_appear";
			break;
		case EarthShiftPlatformController.EarthShiftPlatformSize.Large:
		case EarthShiftPlatformController.EarthShiftPlatformSize.Final:
			path = "event:/SFX/Interactables/sfx_stonePlatform_moveLarge_reset_vanish";
			appearEventPath = "event:/SFX/Interactables/sfx_stonePlatform_moveLarge_reset_appear";
			break;
		}
		AudioManager.PlayOneShot(null, path, this.m_platform.transform.position);
		while (Time.time < endTime)
		{
			float t = (Time.time - startTime) / 0.25f;
			float num = Mathf.Lerp(1f, 0f, t);
			Color color = new Color(num, num, num);
			this.m_renderer.color = color;
			this.m_platform.Renderer.GetPropertyBlock(this.m_matPropBlock);
			this.m_matPropBlock.SetColor(ShaderID_RL._MultiplyColor, color);
			this.m_platform.Renderer.SetPropertyBlock(this.m_matPropBlock);
			yield return null;
		}
		this.m_renderer.color = Color.black;
		this.m_platform.Renderer.GetPropertyBlock(this.m_matPropBlock);
		this.m_matPropBlock.SetColor(ShaderID_RL._MultiplyColor, Color.black);
		this.m_platform.Renderer.SetPropertyBlock(this.m_matPropBlock);
		this.m_platform.ResetPosition();
		startTime = Time.time;
		endTime = startTime + 0.25f;
		while (Time.time < endTime)
		{
			float t2 = (Time.time - startTime) / 0.25f;
			float num2 = Mathf.Lerp(0f, 1f, t2);
			Color color2 = new Color(num2, num2, num2);
			this.m_renderer.color = color2;
			this.m_platform.Renderer.GetPropertyBlock(this.m_matPropBlock);
			this.m_matPropBlock.SetColor(ShaderID_RL._MultiplyColor, color2);
			this.m_platform.Renderer.SetPropertyBlock(this.m_matPropBlock);
			yield return null;
		}
		AudioManager.PlayOneShot(null, appearEventPath, this.m_platform.transform.position);
		this.m_resetPositionCoroutine = null;
		yield break;
	}

	// Token: 0x06003505 RID: 13573 RVA: 0x000DF6E8 File Offset: 0x000DD8E8
	public void SetPlatform(EarthShiftPlatformController platform)
	{
		this.m_platform = platform;
		string eventPath = null;
		string eventPath2 = null;
		string eventPath3 = null;
		string eventPath4 = null;
		switch (this.m_platform.PlatformSize)
		{
		case EarthShiftPlatformController.EarthShiftPlatformSize.Small:
			eventPath = "event:/SFX/Interactables/sfx_stonePlatform_moveSmall_loop";
			eventPath2 = "event:/SFX/Interactables/sfx_bindingStone_small_charging_loop";
			eventPath3 = "event:/SFX/Interactables/sfx_bindingStone_small_inactive_loop";
			eventPath4 = "event:/SFX/Interactables/sfx_bindingStone_small_active_loop";
			break;
		case EarthShiftPlatformController.EarthShiftPlatformSize.Medium:
			eventPath = "event:/SFX/Interactables/sfx_stonePlatform_moveMed_loop";
			eventPath2 = "event:/SFX/Interactables/sfx_bindingStone_med_charging_loop";
			eventPath3 = "event:/SFX/Interactables/sfx_bindingStone_med_inactive_loop";
			eventPath4 = "event:/SFX/Interactables/sfx_bindingStone_med_active_loop";
			break;
		case EarthShiftPlatformController.EarthShiftPlatformSize.Large:
			eventPath = "event:/SFX/Interactables/sfx_stonePlatform_moveLarge_loop";
			eventPath2 = "event:/SFX/Interactables/sfx_bindingStone_large_charging_loop";
			eventPath3 = "event:/SFX/Interactables/sfx_bindingStone_large_inactive_loop";
			eventPath4 = "event:/SFX/Interactables/sfx_bindingStone_large_active_loop";
			break;
		case EarthShiftPlatformController.EarthShiftPlatformSize.Final:
			eventPath = "event:/SFX/Interactables/sfx_stonePlatform_moveLarge_loop";
			eventPath2 = "event:/SFX/Interactables/sfx_bindingStone_final_charging_loop";
			eventPath3 = "event:/SFX/Interactables/sfx_bindingStone_final_inactive_loop";
			eventPath4 = "event:/SFX/Interactables/sfx_bindingStone_final_active_loop";
			break;
		}
		if (!string.IsNullOrWhiteSpace(platform.SFXNameLoopOverride))
		{
			eventPath4 = platform.SFXNameLoopOverride;
		}
		if (this.m_platform.MovesThroughWater)
		{
			eventPath = "event:/SFX/Interactables/sfx_stonePlatform_moveThroughWater_fast_loop";
		}
		if (!this.m_movePlatformSFXInstance.isValid())
		{
			this.m_movePlatformSFXInstance = AudioUtility.GetEventInstance(eventPath, base.transform);
		}
		if (!this.m_chargeSFXInstance.isValid())
		{
			this.m_chargeSFXInstance = AudioUtility.GetEventInstance(eventPath2, base.transform);
		}
		if (!this.m_idleProximitySFXEvent.isValid())
		{
			this.m_idleProximitySFXEvent = AudioUtility.GetEventInstance(eventPath3, base.transform);
		}
		if (!this.m_activeLoopSFXInstance.isValid())
		{
			this.m_activeLoopSFXInstance = AudioUtility.GetEventInstance(eventPath4, base.transform);
		}
	}

	// Token: 0x06003506 RID: 13574 RVA: 0x000DF834 File Offset: 0x000DDA34
	private void ResetColor()
	{
		this.m_renderer.color = Color.white;
		this.m_platform.Renderer.GetPropertyBlock(this.m_matPropBlock);
		this.m_matPropBlock.SetColor(ShaderID_RL._MultiplyColor, Color.white);
		this.m_platform.Renderer.SetPropertyBlock(this.m_matPropBlock);
	}

	// Token: 0x06003507 RID: 13575 RVA: 0x000DF894 File Offset: 0x000DDA94
	private void OnDisable()
	{
		this.m_state = EarthShiftTriggerController.ShiftState.Inactive;
		if (this.m_prepCoroutine != null)
		{
			base.StopCoroutine(this.m_prepCoroutine);
			this.m_prepCoroutine = null;
		}
		this.StopBlinking();
		this.ResetColor();
		if (this.m_resetPositionCoroutine != null)
		{
			base.StopCoroutine(this.m_resetPositionCoroutine);
			this.m_resetPositionCoroutine = null;
		}
		this.m_platform = null;
		this.m_resetTimer = -1f;
		this.m_hasEarthShiftHeirloom = false;
		this.m_playingIdleProximityLoop = false;
		if (!GameManager.IsApplicationClosing)
		{
			this.StopInteraction();
		}
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		if (this.m_movePlatformSFXInstance.isValid())
		{
			AudioManager.Stop(this.m_movePlatformSFXInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		if (this.m_chargeSFXInstance.isValid())
		{
			AudioManager.Stop(this.m_chargeSFXInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		if (this.m_activeLoopSFXInstance.isValid())
		{
			AudioManager.Stop(this.m_activeLoopSFXInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		if (this.m_idleProximitySFXEvent.isValid())
		{
			AudioManager.Stop(this.m_idleProximitySFXEvent, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x06003508 RID: 13576 RVA: 0x000DF988 File Offset: 0x000DDB88
	private void OnDestroy()
	{
		if (this.m_movePlatformSFXInstance.isValid())
		{
			this.m_movePlatformSFXInstance.release();
		}
		if (this.m_chargeSFXInstance.isValid())
		{
			this.m_chargeSFXInstance.release();
		}
		if (this.m_activeLoopSFXInstance.isValid())
		{
			this.m_activeLoopSFXInstance.release();
		}
		if (this.m_idleProximitySFXEvent.isValid())
		{
			this.m_idleProximitySFXEvent.release();
		}
	}

	// Token: 0x04002ACE RID: 10958
	private const string EARTHSHIFT_PLATFORM_MOVE_SMALL_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveSmall_loop";

	// Token: 0x04002ACF RID: 10959
	private const string EARTHSHIFT_PLATFORM_MOVE_MEDIUM_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveMed_loop";

	// Token: 0x04002AD0 RID: 10960
	private const string EARTHSHIFT_PLATFORM_MOVE_LARGE_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveLarge_loop";

	// Token: 0x04002AD1 RID: 10961
	private const string EARTHSHIFT_PLATFORM_STOP_SMALL_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveSmall_stop";

	// Token: 0x04002AD2 RID: 10962
	private const string EARTHSHIFT_PLATFORM_STOP_MEDIUM_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveMed_stop";

	// Token: 0x04002AD3 RID: 10963
	private const string EARTHSHIFT_PLATFORM_STOP_LARGE_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveLarge_stop";

	// Token: 0x04002AD4 RID: 10964
	private const string EARTHSHIFT_PLATFORM_VANISH_SMALL_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveSmall_reset_vanish";

	// Token: 0x04002AD5 RID: 10965
	private const string EARTHSHIFT_PLATFORM_VANISH_MEDIUM_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveMed_reset_vanish";

	// Token: 0x04002AD6 RID: 10966
	private const string EARTHSHIFT_PLATFORM_VANISH_LARGE_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveLarge_reset_vanish";

	// Token: 0x04002AD7 RID: 10967
	private const string EARTHSHIFT_PLATFORM_DEACTIVATE_SMALL_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_small_deactivate";

	// Token: 0x04002AD8 RID: 10968
	private const string EARTHSHIFT_PLATFORM_DEACTIVATE_MEDIUM_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_med_deactivate";

	// Token: 0x04002AD9 RID: 10969
	private const string EARTHSHIFT_PLATFORM_DEACTIVATE_LARGE_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_large_deactivate";

	// Token: 0x04002ADA RID: 10970
	private const string EARTHSHIFT_PLATFORM_APPEAR_SMALL_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveSmall_reset_appear";

	// Token: 0x04002ADB RID: 10971
	private const string EARTHSHIFT_PLATFORM_APPEAR_MEDIUM_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveMed_reset_appear";

	// Token: 0x04002ADC RID: 10972
	private const string EARTHSHIFT_PLATFORM_APPEAR_LARGE_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveLarge_reset_appear";

	// Token: 0x04002ADD RID: 10973
	private const string EARTHSHIFT_STONE_CHARGE_SMALL_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_small_charging_loop";

	// Token: 0x04002ADE RID: 10974
	private const string EARTHSHIFT_STONE_CHARGE_MEDIUM_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_med_charging_loop";

	// Token: 0x04002ADF RID: 10975
	private const string EARTHSHIFT_STONE_CHARGE_LARGE_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_large_charging_loop";

	// Token: 0x04002AE0 RID: 10976
	private const string EARTHSHIFT_STONE_CHARGE_FINAL_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_final_charging_loop";

	// Token: 0x04002AE1 RID: 10977
	private const string EARTHSHIFT_STONE_PLAYER_RELEASE_SMALL_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_small_player_release";

	// Token: 0x04002AE2 RID: 10978
	private const string EARTHSHIFT_STONE_PLAYER_RELEASE_MEDIUM_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_med_player_release";

	// Token: 0x04002AE3 RID: 10979
	private const string EARTHSHIFT_STONE_PLAYER_RELEASE_LARGE_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_large_player_release";

	// Token: 0x04002AE4 RID: 10980
	private const string EARTHSHIFT_RESET_TELEGRAPH_SMALL_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveSmall_reset_telegraph";

	// Token: 0x04002AE5 RID: 10981
	private const string EARTHSHIFT_RESET_TELEGRAPH_MEDIUM_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveMed_reset_telegraph";

	// Token: 0x04002AE6 RID: 10982
	private const string EARTHSHIFT_RESET_TELEGRAPH_LARGE_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveLarge_reset_telegraph";

	// Token: 0x04002AE7 RID: 10983
	private const string EARTHSHIFT_IDLE_PROXIMITY_LOOP_SMALL_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_small_inactive_loop";

	// Token: 0x04002AE8 RID: 10984
	private const string EARTHSHIFT_IDLE_PROXIMITY_LOOP_MEDIUM_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_med_inactive_loop";

	// Token: 0x04002AE9 RID: 10985
	private const string EARTHSHIFT_IDLE_PROXIMITY_LOOP_LARGE_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_large_inactive_loop";

	// Token: 0x04002AEA RID: 10986
	private const string EARTHSHIFT_IDLE_PROXIMITY_LOOP_FINAL_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_final_inactive_loop";

	// Token: 0x04002AEB RID: 10987
	private const string EARTHSHIFT_ACTIVE_LOOP_SMALL_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_small_active_loop";

	// Token: 0x04002AEC RID: 10988
	private const string EARTHSHIFT_ACTIVE_LOOP_MEDIUM_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_med_active_loop";

	// Token: 0x04002AED RID: 10989
	private const string EARTHSHIFT_ACTIVE_LOOP_LARGE_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_large_active_loop";

	// Token: 0x04002AEE RID: 10990
	private const string EARTHSHIFT_ACTIVE_LOOP_FINAL_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_final_active_loop";

	// Token: 0x04002AEF RID: 10991
	private const string EARTHSHIFT_WATER_LOOP_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveThroughWater_fast_loop";

	// Token: 0x04002AF0 RID: 10992
	private const string EARTHSHIFT_WATER_STOP_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveThroughWater_fast_stop";

	// Token: 0x04002AF1 RID: 10993
	[SerializeField]
	private SpriteRenderer m_renderer;

	// Token: 0x04002AF2 RID: 10994
	[SerializeField]
	private bool m_isLarge;

	// Token: 0x04002AF3 RID: 10995
	[SerializeField]
	private float m_glowDistance = 4f;

	// Token: 0x04002AF4 RID: 10996
	[SerializeField]
	private Color m_flickerColor = new Color(0.6f, 0.6f, 0.6f);

	// Token: 0x04002AF5 RID: 10997
	private Interactable m_interactable;

	// Token: 0x04002AF6 RID: 10998
	private Animator m_animator;

	// Token: 0x04002AF7 RID: 10999
	private Coroutine m_prepCoroutine;

	// Token: 0x04002AF8 RID: 11000
	private WaitRL_Yield m_prepWaitYield;

	// Token: 0x04002AF9 RID: 11001
	private Coroutine m_blinkingCoroutine;

	// Token: 0x04002AFA RID: 11002
	private WaitRL_Yield m_blinkWaitYield;

	// Token: 0x04002AFB RID: 11003
	private Coroutine m_resetPositionCoroutine;

	// Token: 0x04002AFC RID: 11004
	private bool m_hasEarthShiftHeirloom;

	// Token: 0x04002AFD RID: 11005
	private EarthShiftTriggerController.ShiftState m_state;

	// Token: 0x04002AFE RID: 11006
	private EarthShiftPlatformController m_platform;

	// Token: 0x04002AFF RID: 11007
	private float m_initialDelayOverride = -1f;

	// Token: 0x04002B00 RID: 11008
	private float m_resetTimer = -1f;

	// Token: 0x04002B01 RID: 11009
	private MaterialPropertyBlock m_matPropBlock;

	// Token: 0x04002B02 RID: 11010
	private bool m_playingIdleProximityLoop;

	// Token: 0x04002B03 RID: 11011
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x04002B04 RID: 11012
	private Action<MonoBehaviour, EventArgs> m_onPlayerHit;

	// Token: 0x04002B05 RID: 11013
	private Action<InputActionEventData> m_onInteractReleased;

	// Token: 0x04002B06 RID: 11014
	private EventInstance m_movePlatformSFXInstance;

	// Token: 0x04002B07 RID: 11015
	private EventInstance m_chargeSFXInstance;

	// Token: 0x04002B08 RID: 11016
	private EventInstance m_idleProximitySFXEvent;

	// Token: 0x04002B09 RID: 11017
	private EventInstance m_activeLoopSFXInstance;

	// Token: 0x04002B0A RID: 11018
	private bool m_largeHasPlayed;

	// Token: 0x020006B8 RID: 1720
	private enum ShiftState
	{
		// Token: 0x04002B0C RID: 11020
		Inactive,
		// Token: 0x04002B0D RID: 11021
		Prep,
		// Token: 0x04002B0E RID: 11022
		Active
	}
}
