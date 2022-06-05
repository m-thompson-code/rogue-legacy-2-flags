using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using Rewired;
using TMPro;
using UnityEngine;

// Token: 0x0200020E RID: 526
public class Interactable : MonoBehaviour, ITerrainOnEnterHitResponse, IHitResponse, ITerrainOnStayHitResponse, ITerrainOnExitHitResponse, IInteractable
{
	// Token: 0x17000B04 RID: 2820
	// (get) Token: 0x06001601 RID: 5633 RVA: 0x00044A33 File Offset: 0x00042C33
	public static bool PlayerIsInteracting
	{
		get
		{
			return Interactable.m_closestInteractable_STATIC != null;
		}
	}

	// Token: 0x17000B05 RID: 2821
	// (get) Token: 0x06001602 RID: 5634 RVA: 0x00044A40 File Offset: 0x00042C40
	public GameObject InteractIconPositionGO
	{
		get
		{
			return this.m_interactIconPositionGO;
		}
	}

	// Token: 0x17000B06 RID: 2822
	// (get) Token: 0x06001603 RID: 5635 RVA: 0x00044A48 File Offset: 0x00042C48
	// (set) Token: 0x06001604 RID: 5636 RVA: 0x00044A50 File Offset: 0x00042C50
	public bool UseParentScaleForInteractIcon { get; set; }

	// Token: 0x17000B07 RID: 2823
	// (get) Token: 0x06001605 RID: 5637 RVA: 0x00044A59 File Offset: 0x00042C59
	// (set) Token: 0x06001606 RID: 5638 RVA: 0x00044A61 File Offset: 0x00042C61
	public SpeechBubbleController SpeechBubble
	{
		get
		{
			return this.m_speechBubble;
		}
		set
		{
			this.m_speechBubble = value;
		}
	}

	// Token: 0x17000B08 RID: 2824
	// (get) Token: 0x06001607 RID: 5639 RVA: 0x00044A6A File Offset: 0x00042C6A
	public bool IsInteractableActive
	{
		get
		{
			return this.m_isInteractableActive;
		}
	}

	// Token: 0x17000B09 RID: 2825
	// (get) Token: 0x06001608 RID: 5640 RVA: 0x00044A72 File Offset: 0x00042C72
	public global::PlayerController PlayerController
	{
		get
		{
			if (PlayerManager.IsInstantiated)
			{
				return PlayerManager.GetPlayerController();
			}
			return null;
		}
	}

	// Token: 0x17000B0A RID: 2826
	// (get) Token: 0x06001609 RID: 5641 RVA: 0x00044A82 File Offset: 0x00042C82
	public bool IsPlayerGrounded
	{
		get
		{
			return this.PlayerController && this.PlayerController.IsGrounded;
		}
	}

	// Token: 0x17000B0B RID: 2827
	// (get) Token: 0x0600160A RID: 5642 RVA: 0x00044A9E File Offset: 0x00042C9E
	// (set) Token: 0x0600160B RID: 5643 RVA: 0x00044AA6 File Offset: 0x00042CA6
	public bool IsPlayerInTrigger
	{
		get
		{
			return this.m_isPlayerInTrigger;
		}
		private set
		{
			this.m_isPlayerInTrigger = value;
		}
	}

	// Token: 0x17000B0C RID: 2828
	// (get) Token: 0x0600160C RID: 5644 RVA: 0x00044AB0 File Offset: 0x00042CB0
	public bool IsPlayerFacing
	{
		get
		{
			return this.PlayerController != null && ((this.PlayerController.IsFacingRight && this.PlayerController.transform.position.x < base.gameObject.transform.position.x) || (!this.PlayerController.IsFacingRight && this.PlayerController.transform.position.x > base.gameObject.transform.position.x));
		}
	}

	// Token: 0x17000B0D RID: 2829
	// (get) Token: 0x0600160D RID: 5645 RVA: 0x00044B42 File Offset: 0x00042D42
	// (set) Token: 0x0600160E RID: 5646 RVA: 0x00044B4A File Offset: 0x00042D4A
	public bool IsInteractable { get; private set; }

	// Token: 0x17000B0E RID: 2830
	// (get) Token: 0x0600160F RID: 5647 RVA: 0x00044B53 File Offset: 0x00042D53
	public UnityEvent_GameObject TriggerOnEnterEvent
	{
		get
		{
			return this.m_triggerOnEnterEvent;
		}
	}

	// Token: 0x17000B0F RID: 2831
	// (get) Token: 0x06001610 RID: 5648 RVA: 0x00044B5B File Offset: 0x00042D5B
	public UnityEvent_GameObject TriggerOnExitEvent
	{
		get
		{
			return this.m_triggerOnExitEvent;
		}
	}

	// Token: 0x17000B10 RID: 2832
	// (get) Token: 0x06001611 RID: 5649 RVA: 0x00044B63 File Offset: 0x00042D63
	public bool TriggerOnTouch
	{
		get
		{
			return this.m_triggerOnTouch;
		}
	}

	// Token: 0x17000B11 RID: 2833
	// (get) Token: 0x06001612 RID: 5650 RVA: 0x00044B6B File Offset: 0x00042D6B
	public TextPopupObj InteractIcon
	{
		get
		{
			return this.m_pooledInteractIcon;
		}
	}

	// Token: 0x06001613 RID: 5651 RVA: 0x00044B73 File Offset: 0x00042D73
	private void Awake()
	{
		this.m_onInteractButtonPressed = new Action<InputActionEventData>(this.OnInteractButtonPressed);
	}

	// Token: 0x06001614 RID: 5652 RVA: 0x00044B88 File Offset: 0x00042D88
	private void Start()
	{
		IHitboxController componentInChildren = base.GetComponentInChildren<IHitboxController>();
		if (!componentInChildren.IsNativeNull())
		{
			componentInChildren.RepeatHitDuration = 0f;
		}
		if (this.m_speechBubble)
		{
			this.m_speechBubble.SetSpeechBubbleEnabled(true);
		}
	}

	// Token: 0x06001615 RID: 5653 RVA: 0x00044BC8 File Offset: 0x00042DC8
	private void OnInteractButtonPressed(InputActionEventData obj)
	{
		if (this.IsInteractable)
		{
			if (!Interactable.m_closestInteractable_STATIC)
			{
				Interactable.m_closestInteractable_STATIC = this;
				base.StartCoroutine(this.StartInteractAtEndOfFrameCoroutine());
				return;
			}
			global::PlayerController playerController = PlayerManager.GetPlayerController();
			float num = CDGHelper.DistanceBetweenPts(Interactable.m_closestInteractable_STATIC.transform.position, playerController.transform.localPosition);
			if (CDGHelper.DistanceBetweenPts(base.transform.position, playerController.transform.localPosition) < num)
			{
				Interactable.m_closestInteractable_STATIC = this;
			}
		}
	}

	// Token: 0x06001616 RID: 5654 RVA: 0x00044C5B File Offset: 0x00042E5B
	private void OnInteractBoundsTouched()
	{
		if (this.IsInteractable)
		{
			this.m_hasPlayerInteractedWith = true;
			this.m_triggerOnEnterEvent.Invoke(base.gameObject);
		}
	}

	// Token: 0x06001617 RID: 5655 RVA: 0x00044C80 File Offset: 0x00042E80
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (base.enabled && this.m_isInteractableActive)
		{
			this.IsPlayerInTrigger = true;
			if (!this.m_triggerOnTouch)
			{
				Rewired_RL.Player.AddInputEventDelegate(this.m_onInteractButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Interact");
			}
			base.StopAllCoroutines();
			if (!this.m_canOnlyInteractOnce || (this.m_canOnlyInteractOnce && !this.m_hasPlayerInteractedWith))
			{
				base.StartCoroutine(this.OnPlayerInTriggerCoroutine());
			}
		}
	}

	// Token: 0x06001618 RID: 5656 RVA: 0x00044CEE File Offset: 0x00042EEE
	public void TerrainOnStayHitResponse(IHitboxController otherHBController)
	{
		if (this.m_isInteractableActive && !this.IsPlayerInTrigger)
		{
			this.TerrainOnEnterHitResponse(otherHBController);
		}
	}

	// Token: 0x06001619 RID: 5657 RVA: 0x00044D07 File Offset: 0x00042F07
	private IEnumerator OnPlayerInTriggerCoroutine()
	{
		global::PlayerController playerController = PlayerManager.GetPlayerController();
		while (this.IsPlayerInTrigger)
		{
			bool isInteractable = true;
			if (playerController && ((playerController.ConditionState != CharacterStates.CharacterConditions.Normal && playerController.ConditionState != CharacterStates.CharacterConditions.DisableHorizontalMovement) || RewiredMapController.CurrentGameInputMode != GameInputMode.Game || !RewiredMapController.IsCurrentMapEnabled || GameManager.IsGamePaused || Cloud.HittingCloud))
			{
				isInteractable = false;
			}
			this.IsInteractable = isInteractable;
			if (!this.m_triggerOnTouch)
			{
				if (this.m_canOnlyInteractOnce)
				{
					if (this.m_hasPlayerInteractedWith)
					{
						this.IsInteractable = false;
						this.SetIsInteractableActive(false);
					}
				}
				else
				{
					if (this.m_playerMustBeFacing && !this.IsPlayerFacing)
					{
						this.IsInteractable = false;
					}
					if (this.m_onlyTriggerWhenPlayerIsGrounded && !this.IsPlayerGrounded)
					{
						this.IsInteractable = false;
					}
				}
			}
			else
			{
				if (this.m_onlyTriggerWhenPlayerIsGrounded && !this.IsPlayerGrounded)
				{
					this.IsInteractable = false;
				}
				if (!this.m_hasPlayerInteractedWith)
				{
					this.OnInteractBoundsTouched();
				}
				if (this.m_resetInteractionImmediately)
				{
					this.m_hasPlayerInteractedWith = false;
				}
			}
			if (this.m_interactPromptDisplayed && (!this.IsInteractable || !this.IsInteractableActive || this.m_forceDisableInteractPrompt))
			{
				this.SetInteractIconIsEnabled(false);
			}
			else if (!this.m_interactPromptDisplayed && this.IsInteractable && this.IsInteractableActive && !this.m_forceDisableInteractPrompt)
			{
				this.SetInteractIconIsEnabled(true);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600161A RID: 5658 RVA: 0x00044D18 File Offset: 0x00042F18
	private void SetInteractIconIsEnabled(bool isEnabled)
	{
		if (this.m_interactPromptDisplayed != isEnabled)
		{
			this.m_interactPromptDisplayed = isEnabled;
			if (!isEnabled)
			{
				if (this.m_pooledInteractIcon && this.m_pooledInteractIcon.isActiveAndEnabled)
				{
					this.m_pooledInteractIcon.gameObject.SetActive(false);
				}
				if (this.m_speechBubble)
				{
					if (this.IsInteractableActive && (!this.IsPlayerInTrigger || (this.IsPlayerInTrigger && this.m_onlyTriggerWhenPlayerIsGrounded && !this.IsPlayerGrounded)))
					{
						this.m_speechBubble.SetSpeechBubbleEnabled(true);
						return;
					}
					this.m_speechBubble.SetSpeechBubbleEnabled(false);
					return;
				}
			}
			else
			{
				if (this.m_interactIconPositionGO)
				{
					Vector2 vector = this.m_interactIconPositionGO.transform.position;
					this.m_pooledInteractIcon = TextPopupManager.DisplayTextAtAbsPos(TextPopupType.Interact, "[Interact]", vector, null, TextAlignmentOptions.Center);
					if (this.UseParentScaleForInteractIcon)
					{
						this.m_pooledInteractIcon.StopAllCoroutines();
						Vector3 lossyScale = base.transform.lossyScale;
						if (lossyScale.x < 0f)
						{
							lossyScale.x *= -1f;
						}
						this.m_pooledInteractIcon.transform.localScale = lossyScale;
						this.m_pooledInteractIcon.Spawn();
					}
					CameraLayerController component = base.GetComponent<CameraLayerController>();
					if (component && (component.CameraLayer == CameraLayer.Foreground_PERSP || component.CameraLayer == CameraLayer.ForegroundLights))
					{
						this.m_pooledInteractIcon.transform.SetParent(component.Visuals.transform);
						this.m_pooledInteractIcon.gameObject.SetLayerRecursively(25, false);
						this.m_pooledInteractIcon.transform.position = new Vector3(vector.x, vector.y, component.Visuals.transform.position.z);
					}
				}
				if (this.m_speechBubble)
				{
					this.m_speechBubble.SetSpeechBubbleEnabled(false);
				}
			}
		}
	}

	// Token: 0x0600161B RID: 5659 RVA: 0x00044EF5 File Offset: 0x000430F5
	public void TerrainOnExitHitResponse(IHitboxController otherHBController)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (base.enabled && this.m_isInteractableActive)
		{
			if (this.m_triggerOnExitEvent != null)
			{
				this.m_triggerOnExitEvent.Invoke(base.gameObject);
			}
			this.Disable();
		}
	}

	// Token: 0x0600161C RID: 5660 RVA: 0x00044F34 File Offset: 0x00043134
	private IEnumerator StartInteractAtEndOfFrameCoroutine()
	{
		if (Interactable.m_closestInteractable_STATIC)
		{
			yield return Interactable.m_endOfFrameYield_STATIC;
			Interactable.m_closestInteractable_STATIC.TriggerInteractable();
		}
		Interactable.m_closestInteractable_STATIC = null;
		yield break;
	}

	// Token: 0x0600161D RID: 5661 RVA: 0x00044F3C File Offset: 0x0004313C
	public void TriggerInteractable()
	{
		if (!this.IsInteractable)
		{
			return;
		}
		if (this.m_disableAbilitiesOnInteract)
		{
			global::PlayerController playerController = PlayerManager.GetPlayerController();
			playerController.StopActiveAbilities(true);
			playerController.SetVelocity(0f, 0f, false);
		}
		this.m_hasPlayerInteractedWith = true;
		this.m_triggerOnEnterEvent.Invoke(base.gameObject);
	}

	// Token: 0x0600161E RID: 5662 RVA: 0x00044F90 File Offset: 0x00043190
	private void Disable()
	{
		this.IsPlayerInTrigger = false;
		if (!this.m_triggerOnTouch)
		{
			if (ReInput.isReady)
			{
				Rewired_RL.Player.RemoveInputEventDelegate(this.m_onInteractButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Interact");
			}
			this.SetInteractIconIsEnabled(false);
			base.StopAllCoroutines();
			return;
		}
		if (!this.m_canOnlyInteractOnce)
		{
			this.m_hasPlayerInteractedWith = false;
		}
	}

	// Token: 0x0600161F RID: 5663 RVA: 0x00044FE7 File Offset: 0x000431E7
	public void SetIsInteractableActive(bool isActive)
	{
		this.m_isInteractableActive = isActive;
		if (!this.m_isInteractableActive)
		{
			this.Disable();
			return;
		}
		if (this.m_speechBubble)
		{
			this.m_speechBubble.SetSpeechBubbleEnabled(true);
		}
	}

	// Token: 0x06001620 RID: 5664 RVA: 0x00045018 File Offset: 0x00043218
	public void ForceUpdateSpeechBubbleState()
	{
		if (this.m_speechBubble)
		{
			this.m_speechBubble.SetSpeechBubbleEnabled(true);
		}
	}

	// Token: 0x06001621 RID: 5665 RVA: 0x00045033 File Offset: 0x00043233
	public void ForceDisableInteractPrompt(bool forceDisable)
	{
		this.m_forceDisableInteractPrompt = forceDisable;
	}

	// Token: 0x06001622 RID: 5666 RVA: 0x0004503C File Offset: 0x0004323C
	public void ForceResetInteractedState()
	{
		this.m_hasPlayerInteractedWith = false;
	}

	// Token: 0x06001623 RID: 5667 RVA: 0x00045045 File Offset: 0x00043245
	private void OnDisable()
	{
		if (Interactable.m_closestInteractable_STATIC == this)
		{
			Interactable.m_closestInteractable_STATIC = null;
		}
		this.Disable();
	}

	// Token: 0x04001533 RID: 5427
	private const string m_interactString_STATIC = "[Interact]";

	// Token: 0x04001534 RID: 5428
	private static Interactable m_closestInteractable_STATIC;

	// Token: 0x04001535 RID: 5429
	private static WaitForEndOfFrame m_endOfFrameYield_STATIC = new WaitForEndOfFrame();

	// Token: 0x04001536 RID: 5430
	[SerializeField]
	private GameObject m_interactIconPositionGO;

	// Token: 0x04001537 RID: 5431
	[SerializeField]
	private SpeechBubbleController m_speechBubble;

	// Token: 0x04001538 RID: 5432
	[SerializeField]
	private bool m_triggerOnTouch;

	// Token: 0x04001539 RID: 5433
	[SerializeField]
	private bool m_onlyTriggerWhenPlayerIsGrounded;

	// Token: 0x0400153A RID: 5434
	[SerializeField]
	private bool m_canOnlyInteractOnce = true;

	// Token: 0x0400153B RID: 5435
	[Tooltip("If false, will reset the trigger state of the interactable only once the player has exited the trigger box. If true, will reset even the player has not yet exited first. Useful for things like spring traps that always need to check for triggers in case the player is standing on it while it is disabled.")]
	[SerializeField]
	[ConditionalHide("m_canOnlyInteractOnce", true, Inverse = true)]
	private bool m_resetInteractionImmediately;

	// Token: 0x0400153C RID: 5436
	[SerializeField]
	private bool m_playerMustBeFacing;

	// Token: 0x0400153D RID: 5437
	[SerializeField]
	private bool m_disableAbilitiesOnInteract = true;

	// Token: 0x0400153E RID: 5438
	[Space(10f)]
	public UnityEvent_GameObject m_triggerOnEnterEvent;

	// Token: 0x0400153F RID: 5439
	public UnityEvent_GameObject m_triggerOnExitEvent;

	// Token: 0x04001540 RID: 5440
	[Header("Debug")]
	[SerializeField]
	[ReadOnly]
	private bool m_isPlayerInTrigger;

	// Token: 0x04001541 RID: 5441
	[SerializeField]
	[ReadOnly]
	private bool m_hasPlayerInteractedWith;

	// Token: 0x04001542 RID: 5442
	[SerializeField]
	[ReadOnly]
	private bool m_isInteractableActive = true;

	// Token: 0x04001543 RID: 5443
	private TextPopupObj m_pooledInteractIcon;

	// Token: 0x04001544 RID: 5444
	private bool m_interactPromptDisplayed;

	// Token: 0x04001545 RID: 5445
	private bool m_forceDisableInteractPrompt;

	// Token: 0x04001546 RID: 5446
	private Action<InputActionEventData> m_onInteractButtonPressed;
}
