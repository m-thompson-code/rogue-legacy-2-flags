using System;
using System.Collections;
using FMOD.Studio;
using Rewired;
using RLAudio;
using UnityEngine;

// Token: 0x02000405 RID: 1029
public class EarthShiftTriggerController : MonoBehaviour
{
	// Token: 0x06002662 RID: 9826 RVA: 0x0007EE90 File Offset: 0x0007D090
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

	// Token: 0x06002663 RID: 9827 RVA: 0x0007EF18 File Offset: 0x0007D118
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06002664 RID: 9828 RVA: 0x0007EF28 File Offset: 0x0007D128
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

	// Token: 0x06002665 RID: 9829 RVA: 0x0007EFBC File Offset: 0x0007D1BC
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

	// Token: 0x06002666 RID: 9830 RVA: 0x0007F054 File Offset: 0x0007D254
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

	// Token: 0x06002667 RID: 9831 RVA: 0x0007F1C4 File Offset: 0x0007D3C4
	private void OnInteractReleased(InputActionEventData data)
	{
		if (!this.m_isLarge)
		{
			this.StopInteraction();
		}
	}

	// Token: 0x06002668 RID: 9832 RVA: 0x0007F1D4 File Offset: 0x0007D3D4
	private void OnPlayerHit(MonoBehaviour sender, EventArgs args)
	{
		this.StopInteraction();
	}

	// Token: 0x06002669 RID: 9833 RVA: 0x0007F1DC File Offset: 0x0007D3DC
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

	// Token: 0x0600266A RID: 9834 RVA: 0x0007F1EC File Offset: 0x0007D3EC
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

	// Token: 0x0600266B RID: 9835 RVA: 0x0007F540 File Offset: 0x0007D740
	private IEnumerator BlinkCoroutine()
	{
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

	// Token: 0x0600266C RID: 9836 RVA: 0x0007F54F File Offset: 0x0007D74F
	private void StopBlinking()
	{
		if (this.m_blinkingCoroutine != null)
		{
			base.StopCoroutine(this.m_blinkingCoroutine);
			this.m_blinkingCoroutine = null;
			this.ResetColor();
		}
	}

	// Token: 0x0600266D RID: 9837 RVA: 0x0007F572 File Offset: 0x0007D772
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

	// Token: 0x0600266E RID: 9838 RVA: 0x0007F584 File Offset: 0x0007D784
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

	// Token: 0x0600266F RID: 9839 RVA: 0x0007F6D0 File Offset: 0x0007D8D0
	private void ResetColor()
	{
		this.m_renderer.color = Color.white;
		this.m_platform.Renderer.GetPropertyBlock(this.m_matPropBlock);
		this.m_matPropBlock.SetColor(ShaderID_RL._MultiplyColor, Color.white);
		this.m_platform.Renderer.SetPropertyBlock(this.m_matPropBlock);
	}

	// Token: 0x06002670 RID: 9840 RVA: 0x0007F730 File Offset: 0x0007D930
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

	// Token: 0x06002671 RID: 9841 RVA: 0x0007F824 File Offset: 0x0007DA24
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

	// Token: 0x04002006 RID: 8198
	private const string EARTHSHIFT_PLATFORM_MOVE_SMALL_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveSmall_loop";

	// Token: 0x04002007 RID: 8199
	private const string EARTHSHIFT_PLATFORM_MOVE_MEDIUM_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveMed_loop";

	// Token: 0x04002008 RID: 8200
	private const string EARTHSHIFT_PLATFORM_MOVE_LARGE_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveLarge_loop";

	// Token: 0x04002009 RID: 8201
	private const string EARTHSHIFT_PLATFORM_STOP_SMALL_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveSmall_stop";

	// Token: 0x0400200A RID: 8202
	private const string EARTHSHIFT_PLATFORM_STOP_MEDIUM_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveMed_stop";

	// Token: 0x0400200B RID: 8203
	private const string EARTHSHIFT_PLATFORM_STOP_LARGE_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveLarge_stop";

	// Token: 0x0400200C RID: 8204
	private const string EARTHSHIFT_PLATFORM_VANISH_SMALL_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveSmall_reset_vanish";

	// Token: 0x0400200D RID: 8205
	private const string EARTHSHIFT_PLATFORM_VANISH_MEDIUM_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveMed_reset_vanish";

	// Token: 0x0400200E RID: 8206
	private const string EARTHSHIFT_PLATFORM_VANISH_LARGE_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveLarge_reset_vanish";

	// Token: 0x0400200F RID: 8207
	private const string EARTHSHIFT_PLATFORM_DEACTIVATE_SMALL_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_small_deactivate";

	// Token: 0x04002010 RID: 8208
	private const string EARTHSHIFT_PLATFORM_DEACTIVATE_MEDIUM_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_med_deactivate";

	// Token: 0x04002011 RID: 8209
	private const string EARTHSHIFT_PLATFORM_DEACTIVATE_LARGE_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_large_deactivate";

	// Token: 0x04002012 RID: 8210
	private const string EARTHSHIFT_PLATFORM_APPEAR_SMALL_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveSmall_reset_appear";

	// Token: 0x04002013 RID: 8211
	private const string EARTHSHIFT_PLATFORM_APPEAR_MEDIUM_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveMed_reset_appear";

	// Token: 0x04002014 RID: 8212
	private const string EARTHSHIFT_PLATFORM_APPEAR_LARGE_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveLarge_reset_appear";

	// Token: 0x04002015 RID: 8213
	private const string EARTHSHIFT_STONE_CHARGE_SMALL_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_small_charging_loop";

	// Token: 0x04002016 RID: 8214
	private const string EARTHSHIFT_STONE_CHARGE_MEDIUM_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_med_charging_loop";

	// Token: 0x04002017 RID: 8215
	private const string EARTHSHIFT_STONE_CHARGE_LARGE_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_large_charging_loop";

	// Token: 0x04002018 RID: 8216
	private const string EARTHSHIFT_STONE_CHARGE_FINAL_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_final_charging_loop";

	// Token: 0x04002019 RID: 8217
	private const string EARTHSHIFT_STONE_PLAYER_RELEASE_SMALL_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_small_player_release";

	// Token: 0x0400201A RID: 8218
	private const string EARTHSHIFT_STONE_PLAYER_RELEASE_MEDIUM_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_med_player_release";

	// Token: 0x0400201B RID: 8219
	private const string EARTHSHIFT_STONE_PLAYER_RELEASE_LARGE_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_large_player_release";

	// Token: 0x0400201C RID: 8220
	private const string EARTHSHIFT_RESET_TELEGRAPH_SMALL_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveSmall_reset_telegraph";

	// Token: 0x0400201D RID: 8221
	private const string EARTHSHIFT_RESET_TELEGRAPH_MEDIUM_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveMed_reset_telegraph";

	// Token: 0x0400201E RID: 8222
	private const string EARTHSHIFT_RESET_TELEGRAPH_LARGE_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveLarge_reset_telegraph";

	// Token: 0x0400201F RID: 8223
	private const string EARTHSHIFT_IDLE_PROXIMITY_LOOP_SMALL_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_small_inactive_loop";

	// Token: 0x04002020 RID: 8224
	private const string EARTHSHIFT_IDLE_PROXIMITY_LOOP_MEDIUM_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_med_inactive_loop";

	// Token: 0x04002021 RID: 8225
	private const string EARTHSHIFT_IDLE_PROXIMITY_LOOP_LARGE_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_large_inactive_loop";

	// Token: 0x04002022 RID: 8226
	private const string EARTHSHIFT_IDLE_PROXIMITY_LOOP_FINAL_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_final_inactive_loop";

	// Token: 0x04002023 RID: 8227
	private const string EARTHSHIFT_ACTIVE_LOOP_SMALL_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_small_active_loop";

	// Token: 0x04002024 RID: 8228
	private const string EARTHSHIFT_ACTIVE_LOOP_MEDIUM_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_med_active_loop";

	// Token: 0x04002025 RID: 8229
	private const string EARTHSHIFT_ACTIVE_LOOP_LARGE_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_large_active_loop";

	// Token: 0x04002026 RID: 8230
	private const string EARTHSHIFT_ACTIVE_LOOP_FINAL_SFX_NAME = "event:/SFX/Interactables/sfx_bindingStone_final_active_loop";

	// Token: 0x04002027 RID: 8231
	private const string EARTHSHIFT_WATER_LOOP_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveThroughWater_fast_loop";

	// Token: 0x04002028 RID: 8232
	private const string EARTHSHIFT_WATER_STOP_SFX_NAME = "event:/SFX/Interactables/sfx_stonePlatform_moveThroughWater_fast_stop";

	// Token: 0x04002029 RID: 8233
	[SerializeField]
	private SpriteRenderer m_renderer;

	// Token: 0x0400202A RID: 8234
	[SerializeField]
	private bool m_isLarge;

	// Token: 0x0400202B RID: 8235
	[SerializeField]
	private float m_glowDistance = 4f;

	// Token: 0x0400202C RID: 8236
	[SerializeField]
	private Color m_flickerColor = new Color(0.6f, 0.6f, 0.6f);

	// Token: 0x0400202D RID: 8237
	private Interactable m_interactable;

	// Token: 0x0400202E RID: 8238
	private Animator m_animator;

	// Token: 0x0400202F RID: 8239
	private Coroutine m_prepCoroutine;

	// Token: 0x04002030 RID: 8240
	private WaitRL_Yield m_prepWaitYield;

	// Token: 0x04002031 RID: 8241
	private Coroutine m_blinkingCoroutine;

	// Token: 0x04002032 RID: 8242
	private WaitRL_Yield m_blinkWaitYield;

	// Token: 0x04002033 RID: 8243
	private Coroutine m_resetPositionCoroutine;

	// Token: 0x04002034 RID: 8244
	private bool m_hasEarthShiftHeirloom;

	// Token: 0x04002035 RID: 8245
	private EarthShiftTriggerController.ShiftState m_state;

	// Token: 0x04002036 RID: 8246
	private EarthShiftPlatformController m_platform;

	// Token: 0x04002037 RID: 8247
	private float m_initialDelayOverride = -1f;

	// Token: 0x04002038 RID: 8248
	private float m_resetTimer = -1f;

	// Token: 0x04002039 RID: 8249
	private MaterialPropertyBlock m_matPropBlock;

	// Token: 0x0400203A RID: 8250
	private bool m_playingIdleProximityLoop;

	// Token: 0x0400203B RID: 8251
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x0400203C RID: 8252
	private Action<MonoBehaviour, EventArgs> m_onPlayerHit;

	// Token: 0x0400203D RID: 8253
	private Action<InputActionEventData> m_onInteractReleased;

	// Token: 0x0400203E RID: 8254
	private EventInstance m_movePlatformSFXInstance;

	// Token: 0x0400203F RID: 8255
	private EventInstance m_chargeSFXInstance;

	// Token: 0x04002040 RID: 8256
	private EventInstance m_idleProximitySFXEvent;

	// Token: 0x04002041 RID: 8257
	private EventInstance m_activeLoopSFXInstance;

	// Token: 0x04002042 RID: 8258
	private bool m_largeHasPlayed;

	// Token: 0x02000C2B RID: 3115
	private enum ShiftState
	{
		// Token: 0x04004F5D RID: 20317
		Inactive,
		// Token: 0x04004F5E RID: 20318
		Prep,
		// Token: 0x04004F5F RID: 20319
		Active
	}
}
