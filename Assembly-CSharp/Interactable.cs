using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using Rewired;
using TMPro;
using UnityEngine;

// Token: 0x020003BF RID: 959
public class Interactable : MonoBehaviour, ITerrainOnEnterHitResponse, IHitResponse, ITerrainOnStayHitResponse, ITerrainOnExitHitResponse, IInteractable
{
	// Token: 0x17000E27 RID: 3623
	// (get) Token: 0x06001F91 RID: 8081 RVA: 0x00010A04 File Offset: 0x0000EC04
	public static bool PlayerIsInteracting
	{
		get
		{
			return Interactable.m_closestInteractable_STATIC != null;
		}
	}

	// Token: 0x17000E28 RID: 3624
	// (get) Token: 0x06001F92 RID: 8082 RVA: 0x00010A11 File Offset: 0x0000EC11
	public GameObject InteractIconPositionGO
	{
		get
		{
			return this.m_interactIconPositionGO;
		}
	}

	// Token: 0x17000E29 RID: 3625
	// (get) Token: 0x06001F93 RID: 8083 RVA: 0x00010A19 File Offset: 0x0000EC19
	// (set) Token: 0x06001F94 RID: 8084 RVA: 0x00010A21 File Offset: 0x0000EC21
	public bool UseParentScaleForInteractIcon { get; set; }

	// Token: 0x17000E2A RID: 3626
	// (get) Token: 0x06001F95 RID: 8085 RVA: 0x00010A2A File Offset: 0x0000EC2A
	// (set) Token: 0x06001F96 RID: 8086 RVA: 0x00010A32 File Offset: 0x0000EC32
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

	// Token: 0x17000E2B RID: 3627
	// (get) Token: 0x06001F97 RID: 8087 RVA: 0x00010A3B File Offset: 0x0000EC3B
	public bool IsInteractableActive
	{
		get
		{
			return this.m_isInteractableActive;
		}
	}

	// Token: 0x17000E2C RID: 3628
	// (get) Token: 0x06001F98 RID: 8088 RVA: 0x00010A43 File Offset: 0x0000EC43
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

	// Token: 0x17000E2D RID: 3629
	// (get) Token: 0x06001F99 RID: 8089 RVA: 0x00010A53 File Offset: 0x0000EC53
	public bool IsPlayerGrounded
	{
		get
		{
			return this.PlayerController && this.PlayerController.IsGrounded;
		}
	}

	// Token: 0x17000E2E RID: 3630
	// (get) Token: 0x06001F9A RID: 8090 RVA: 0x00010A6F File Offset: 0x0000EC6F
	// (set) Token: 0x06001F9B RID: 8091 RVA: 0x00010A77 File Offset: 0x0000EC77
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

	// Token: 0x17000E2F RID: 3631
	// (get) Token: 0x06001F9C RID: 8092 RVA: 0x000A3218 File Offset: 0x000A1418
	public bool IsPlayerFacing
	{
		get
		{
			return this.PlayerController != null && ((this.PlayerController.IsFacingRight && this.PlayerController.transform.position.x < base.gameObject.transform.position.x) || (!this.PlayerController.IsFacingRight && this.PlayerController.transform.position.x > base.gameObject.transform.position.x));
		}
	}

	// Token: 0x17000E30 RID: 3632
	// (get) Token: 0x06001F9D RID: 8093 RVA: 0x00010A80 File Offset: 0x0000EC80
	// (set) Token: 0x06001F9E RID: 8094 RVA: 0x00010A88 File Offset: 0x0000EC88
	public bool IsInteractable { get; private set; }

	// Token: 0x17000E31 RID: 3633
	// (get) Token: 0x06001F9F RID: 8095 RVA: 0x00010A91 File Offset: 0x0000EC91
	public UnityEvent_GameObject TriggerOnEnterEvent
	{
		get
		{
			return this.m_triggerOnEnterEvent;
		}
	}

	// Token: 0x17000E32 RID: 3634
	// (get) Token: 0x06001FA0 RID: 8096 RVA: 0x00010A99 File Offset: 0x0000EC99
	public UnityEvent_GameObject TriggerOnExitEvent
	{
		get
		{
			return this.m_triggerOnExitEvent;
		}
	}

	// Token: 0x17000E33 RID: 3635
	// (get) Token: 0x06001FA1 RID: 8097 RVA: 0x00010AA1 File Offset: 0x0000ECA1
	public bool TriggerOnTouch
	{
		get
		{
			return this.m_triggerOnTouch;
		}
	}

	// Token: 0x17000E34 RID: 3636
	// (get) Token: 0x06001FA2 RID: 8098 RVA: 0x00010AA9 File Offset: 0x0000ECA9
	public TextPopupObj InteractIcon
	{
		get
		{
			return this.m_pooledInteractIcon;
		}
	}

	// Token: 0x06001FA3 RID: 8099 RVA: 0x00010AB1 File Offset: 0x0000ECB1
	private void Awake()
	{
		this.m_onInteractButtonPressed = new Action<InputActionEventData>(this.OnInteractButtonPressed);
	}

	// Token: 0x06001FA4 RID: 8100 RVA: 0x000A32AC File Offset: 0x000A14AC
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

	// Token: 0x06001FA5 RID: 8101 RVA: 0x000A32EC File Offset: 0x000A14EC
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

	// Token: 0x06001FA6 RID: 8102 RVA: 0x00010AC5 File Offset: 0x0000ECC5
	private void OnInteractBoundsTouched()
	{
		if (this.IsInteractable)
		{
			this.m_hasPlayerInteractedWith = true;
			this.m_triggerOnEnterEvent.Invoke(base.gameObject);
		}
	}

	// Token: 0x06001FA7 RID: 8103 RVA: 0x000A3380 File Offset: 0x000A1580
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

	// Token: 0x06001FA8 RID: 8104 RVA: 0x00010AE7 File Offset: 0x0000ECE7
	public void TerrainOnStayHitResponse(IHitboxController otherHBController)
	{
		if (this.m_isInteractableActive && !this.IsPlayerInTrigger)
		{
			this.TerrainOnEnterHitResponse(otherHBController);
		}
	}

	// Token: 0x06001FA9 RID: 8105 RVA: 0x00010B00 File Offset: 0x0000ED00
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

	// Token: 0x06001FAA RID: 8106 RVA: 0x000A33F0 File Offset: 0x000A15F0
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

	// Token: 0x06001FAB RID: 8107 RVA: 0x00010B0F File Offset: 0x0000ED0F
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

	// Token: 0x06001FAC RID: 8108 RVA: 0x00010B4E File Offset: 0x0000ED4E
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

	// Token: 0x06001FAD RID: 8109 RVA: 0x000A35D0 File Offset: 0x000A17D0
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

	// Token: 0x06001FAE RID: 8110 RVA: 0x000A3624 File Offset: 0x000A1824
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

	// Token: 0x06001FAF RID: 8111 RVA: 0x00010B56 File Offset: 0x0000ED56
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

	// Token: 0x06001FB0 RID: 8112 RVA: 0x00010B87 File Offset: 0x0000ED87
	public void ForceUpdateSpeechBubbleState()
	{
		if (this.m_speechBubble)
		{
			this.m_speechBubble.SetSpeechBubbleEnabled(true);
		}
	}

	// Token: 0x06001FB1 RID: 8113 RVA: 0x00010BA2 File Offset: 0x0000EDA2
	public void ForceDisableInteractPrompt(bool forceDisable)
	{
		this.m_forceDisableInteractPrompt = forceDisable;
	}

	// Token: 0x06001FB2 RID: 8114 RVA: 0x00010BAB File Offset: 0x0000EDAB
	public void ForceResetInteractedState()
	{
		this.m_hasPlayerInteractedWith = false;
	}

	// Token: 0x06001FB3 RID: 8115 RVA: 0x00010BB4 File Offset: 0x0000EDB4
	private void OnDisable()
	{
		if (Interactable.m_closestInteractable_STATIC == this)
		{
			Interactable.m_closestInteractable_STATIC = null;
		}
		this.Disable();
	}

	// Token: 0x04001C30 RID: 7216
	private const string m_interactString_STATIC = "[Interact]";

	// Token: 0x04001C31 RID: 7217
	private static Interactable m_closestInteractable_STATIC;

	// Token: 0x04001C32 RID: 7218
	private static WaitForEndOfFrame m_endOfFrameYield_STATIC = new WaitForEndOfFrame();

	// Token: 0x04001C33 RID: 7219
	[SerializeField]
	private GameObject m_interactIconPositionGO;

	// Token: 0x04001C34 RID: 7220
	[SerializeField]
	private SpeechBubbleController m_speechBubble;

	// Token: 0x04001C35 RID: 7221
	[SerializeField]
	private bool m_triggerOnTouch;

	// Token: 0x04001C36 RID: 7222
	[SerializeField]
	private bool m_onlyTriggerWhenPlayerIsGrounded;

	// Token: 0x04001C37 RID: 7223
	[SerializeField]
	private bool m_canOnlyInteractOnce = true;

	// Token: 0x04001C38 RID: 7224
	[Tooltip("If false, will reset the trigger state of the interactable only once the player has exited the trigger box. If true, will reset even the player has not yet exited first. Useful for things like spring traps that always need to check for triggers in case the player is standing on it while it is disabled.")]
	[SerializeField]
	[ConditionalHide("m_canOnlyInteractOnce", true, Inverse = true)]
	private bool m_resetInteractionImmediately;

	// Token: 0x04001C39 RID: 7225
	[SerializeField]
	private bool m_playerMustBeFacing;

	// Token: 0x04001C3A RID: 7226
	[SerializeField]
	private bool m_disableAbilitiesOnInteract = true;

	// Token: 0x04001C3B RID: 7227
	[Space(10f)]
	public UnityEvent_GameObject m_triggerOnEnterEvent;

	// Token: 0x04001C3C RID: 7228
	public UnityEvent_GameObject m_triggerOnExitEvent;

	// Token: 0x04001C3D RID: 7229
	[Header("Debug")]
	[SerializeField]
	[ReadOnly]
	private bool m_isPlayerInTrigger;

	// Token: 0x04001C3E RID: 7230
	[SerializeField]
	[ReadOnly]
	private bool m_hasPlayerInteractedWith;

	// Token: 0x04001C3F RID: 7231
	[SerializeField]
	[ReadOnly]
	private bool m_isInteractableActive = true;

	// Token: 0x04001C40 RID: 7232
	private TextPopupObj m_pooledInteractIcon;

	// Token: 0x04001C41 RID: 7233
	private bool m_interactPromptDisplayed;

	// Token: 0x04001C42 RID: 7234
	private bool m_forceDisableInteractPrompt;

	// Token: 0x04001C43 RID: 7235
	private Action<InputActionEventData> m_onInteractButtonPressed;
}
