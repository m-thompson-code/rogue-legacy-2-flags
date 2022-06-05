using System;
using System.Collections;
using RLWorldCreation;
using RL_Windows;
using SceneManagement_RL;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020003B0 RID: 944
public class GlobalTeleporterController : MonoBehaviour, IRootObj
{
	// Token: 0x17000E1A RID: 3610
	// (get) Token: 0x06001F3A RID: 7994 RVA: 0x00010602 File Offset: 0x0000E802
	// (set) Token: 0x06001F3B RID: 7995 RVA: 0x00010609 File Offset: 0x0000E809
	public static bool StayInCutsceneAfterTeleporting { get; set; }

	// Token: 0x17000E1B RID: 3611
	// (get) Token: 0x06001F3C RID: 7996 RVA: 0x00010611 File Offset: 0x0000E811
	// (set) Token: 0x06001F3D RID: 7997 RVA: 0x00010618 File Offset: 0x0000E818
	public static bool IsTeleporting { get; private set; }

	// Token: 0x17000E1C RID: 3612
	// (get) Token: 0x06001F3E RID: 7998 RVA: 0x00010620 File Offset: 0x0000E820
	public GlobalTeleporterType TeleporterType
	{
		get
		{
			return this.m_teleporterType;
		}
	}

	// Token: 0x17000E1D RID: 3613
	// (get) Token: 0x06001F3F RID: 7999 RVA: 0x00010628 File Offset: 0x0000E828
	// (set) Token: 0x06001F40 RID: 8000 RVA: 0x00010630 File Offset: 0x0000E830
	public Room Room { get; set; }

	// Token: 0x06001F41 RID: 8001 RVA: 0x00010639 File Offset: 0x0000E839
	private void OnDisable()
	{
		if (this.m_interactable != null)
		{
			this.m_interactable.TriggerOnEnterEvent.RemoveListener(new UnityAction<GameObject>(this.OnInteract));
		}
	}

	// Token: 0x06001F42 RID: 8002 RVA: 0x000A2444 File Offset: 0x000A0644
	private void OnEnable()
	{
		if (this.m_hitboxController.IsInitialized)
		{
			this.m_hitboxController.GetCollider(HitboxType.Platform).enabled = false;
		}
		if (this.m_interactable != null)
		{
			this.m_interactable.TriggerOnEnterEvent.AddListener(new UnityAction<GameObject>(this.OnInteract));
		}
	}

	// Token: 0x06001F43 RID: 8003 RVA: 0x000A249C File Offset: 0x000A069C
	private void Awake()
	{
		this.Room = base.GetComponentInParent<Room>();
		this.m_interactable = base.GetComponent<Interactable>();
		this.m_hitboxController = base.GetComponentInChildren<IHitboxController>();
		this.m_animWaitUntilLandedYield = new WaitUntil(() => PlayerManager.GetPlayerController().IsGrounded);
		this.m_animWaitUntilTransitionYield = new WaitUntil(() => !SceneLoader_RL.IsRunningTransitionWithLogic);
		this.m_animWaitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06001F44 RID: 8004 RVA: 0x00010665 File Offset: 0x0000E865
	private IEnumerator Start()
	{
		while (!this.m_hitboxController.IsInitialized)
		{
			yield return null;
		}
		this.m_hitboxController.GetCollider(HitboxType.Platform).enabled = false;
		this.m_warpEffect = CameraController.ForegroundPerspCam.GetComponent<HeirloomWarp_Effect>();
		yield break;
	}

	// Token: 0x06001F45 RID: 8005 RVA: 0x00010674 File Offset: 0x0000E874
	private void OnInteract(GameObject otherObj)
	{
		GlobalTeleporterController.ActiveTeleporter = this;
		if (this.m_playerInteractedWithEvent != null)
		{
			this.m_playerInteractedWithEvent.Invoke();
		}
		WindowManager.SetWindowIsOpen(WindowID.Teleporter, true);
	}

	// Token: 0x06001F46 RID: 8006 RVA: 0x00010697 File Offset: 0x0000E897
	public void OnEnterTeleportPlayer_V2(BiomeType biomeType, GridPointManager gridPointManager)
	{
		base.StartCoroutine(this.RunOnEnterTeleporterCoroutine_V2(biomeType, gridPointManager));
	}

	// Token: 0x06001F47 RID: 8007 RVA: 0x000106A8 File Offset: 0x0000E8A8
	private IEnumerator RunOnEnterTeleporterCoroutine_V2(BiomeType biomeType, GridPointManager gridPointManager)
	{
		GlobalTeleporterController.IsTeleporting = true;
		this.m_interactable.SetIsInteractableActive(false);
		if (this.m_teleportStart != null)
		{
			this.m_teleportStart.Invoke();
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.StopActiveAbilities(true);
		playerController.CharacterHitResponse.SetInvincibleTime(9999f, false, false);
		RewiredMapController.SetIsInCutscene(true);
		RewiredMapController.SetCurrentMapEnabled(false);
		this.m_hitboxController.GetCollider(HitboxType.Platform).enabled = true;
		float storedJump = playerController.CharacterJump.JumpHeight;
		playerController.CharacterJump.JumpHeight = 8.75f;
		yield return PlayerMovementHelper.JumpPlayer(0f);
		playerController.CharacterJump.JumpHeight = storedJump;
		yield return PlayerMovementHelper.MoveTo(base.transform.position, true);
		this.m_animWaitYield.CreateNew(0.2f, false);
		yield return this.m_animWaitYield;
		yield return this.m_animWaitUntilLandedYield;
		if (this.m_playerJumpedOnPlatformEvent != null)
		{
			this.m_playerJumpedOnPlatformEvent.Invoke();
		}
		yield return this.m_animWaitYield;
		playerController.Animator.SetBool("Victory", true);
		Vector3 vector = CameraController.GameCamera.WorldToViewportPoint(playerController.Midpoint);
		this.m_warpEffect.WarpCenterX = vector.x;
		this.m_warpEffect.WarpCenterY = vector.y;
		this.m_warpEffect.DistortionAmount = 0f;
		this.m_warpEffect.enabled = true;
		yield return TweenManager.TweenTo(this.m_warpEffect, 0.5f, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"DistortionAmount",
			0.025f
		}).TweenCoroutine;
		yield return TweenManager.TweenTo(this.m_warpEffect, 0.1f, new EaseDelegate(Ease.None), new object[]
		{
			"DistortionAmount",
			0.1f
		}).TweenCoroutine;
		EffectManager.PlayEffect(base.gameObject, null, "GlobalTeleporterPoof_Effect", playerController.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		playerController.Visuals.gameObject.SetActive(false);
		RumbleManager.StartRumble(true, true, 0.5f, 0.2f, true);
		if (this.m_dematerializeStartEvent != null)
		{
			this.m_dematerializeStartEvent.Invoke();
		}
		yield return TweenManager.TweenTo(this.m_warpEffect, 0.25f, new EaseDelegate(Ease.Back.EaseOut), new object[]
		{
			"DistortionAmount",
			0
		}).TweenCoroutine;
		this.m_warpEffect.enabled = false;
		this.m_animWaitYield.CreateNew(1f, false);
		yield return this.m_animWaitYield;
		if (this.m_dematerializeCompleteEvent != null)
		{
			this.m_dematerializeCompleteEvent.Invoke();
		}
		if (this.m_transitionStartEvent != null)
		{
			this.m_transitionStartEvent.Invoke();
		}
		if (this.Room.BiomeType != biomeType)
		{
			BiomeTransitionController.TransitionStartRelay.Dispatch(this.Room.BiomeType, biomeType);
		}
		SceneLoader_RL.RunTransitionWithLogic(this.TeleportPlayer_V2(biomeType, gridPointManager), TransitionID.FadeToBlackWithLoading, true);
		yield break;
	}

	// Token: 0x06001F48 RID: 8008 RVA: 0x000106C5 File Offset: 0x0000E8C5
	private IEnumerator TeleportPlayer_V2(BiomeType biomeType, GridPointManager gridPointManager)
	{
		yield return MapController.TeleportPlayerToRoom_Coroutine(biomeType, gridPointManager, true);
		PlayerManager.GetPlayerController().Visuals.gameObject.SetActive(true);
		this.m_interactable.SetIsInteractableActive(true);
		yield break;
	}

	// Token: 0x06001F49 RID: 8009 RVA: 0x000106E2 File Offset: 0x0000E8E2
	public void OnExitTeleportPlayer()
	{
		GlobalTeleporterController.ActiveTeleporter = null;
		base.StartCoroutine(this.RunOnExitTeleporterCoroutine());
	}

	// Token: 0x06001F4A RID: 8010 RVA: 0x000106F7 File Offset: 0x0000E8F7
	private IEnumerator RunOnExitTeleporterCoroutine()
	{
		yield return null;
		yield return this.m_animWaitUntilTransitionYield;
		if (this.m_transitionCompleteEvent != null)
		{
			this.m_transitionCompleteEvent.Invoke();
		}
		RewiredMapController.SetCurrentMapEnabled(false);
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.Animator.SetBool("Victory", false);
		this.m_animWaitYield.CreateNew(0.2f, false);
		yield return this.m_animWaitYield;
		if (this.m_rematerializeStartEvent != null)
		{
			this.m_rematerializeStartEvent.Invoke();
		}
		RewiredMapController.SetCurrentMapEnabled(true);
		playerController.CharacterHitResponse.StopInvincibleTime();
		GlobalTeleporterController.IsTeleporting = false;
		if (!GlobalTeleporterController.StayInCutsceneAfterTeleporting)
		{
			RewiredMapController.SetIsInCutscene(false);
		}
		yield break;
	}

	// Token: 0x06001F4D RID: 8013 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04001BDB RID: 7131
	public static GlobalTeleporterController ActiveTeleporter;

	// Token: 0x04001BDE RID: 7134
	[SerializeField]
	private ParticleSystem m_onActivateParticleSystem;

	// Token: 0x04001BDF RID: 7135
	[SerializeField]
	private GlobalTeleporterType m_teleporterType;

	// Token: 0x04001BE0 RID: 7136
	[SerializeField]
	private GameObject m_distortPosition;

	// Token: 0x04001BE1 RID: 7137
	[SerializeField]
	private UnityEvent m_playerInteractedWithEvent;

	// Token: 0x04001BE2 RID: 7138
	[SerializeField]
	private UnityEvent m_teleportStart;

	// Token: 0x04001BE3 RID: 7139
	[SerializeField]
	private UnityEvent m_playerJumpedOnPlatformEvent;

	// Token: 0x04001BE4 RID: 7140
	[SerializeField]
	private UnityEvent m_dematerializeStartEvent;

	// Token: 0x04001BE5 RID: 7141
	[SerializeField]
	private UnityEvent m_dematerializeCompleteEvent;

	// Token: 0x04001BE6 RID: 7142
	[SerializeField]
	private UnityEvent m_rematerializeStartEvent;

	// Token: 0x04001BE7 RID: 7143
	[SerializeField]
	private UnityEvent m_transitionStartEvent;

	// Token: 0x04001BE8 RID: 7144
	[SerializeField]
	private UnityEvent m_transitionCompleteEvent;

	// Token: 0x04001BE9 RID: 7145
	private Interactable m_interactable;

	// Token: 0x04001BEA RID: 7146
	private IHitboxController m_hitboxController;

	// Token: 0x04001BEB RID: 7147
	private WaitRL_Yield m_animWaitYield;

	// Token: 0x04001BEC RID: 7148
	private WaitUntil m_animWaitUntilLandedYield;

	// Token: 0x04001BED RID: 7149
	private WaitUntil m_animWaitUntilTransitionYield;

	// Token: 0x04001BEE RID: 7150
	private HeirloomWarp_Effect m_warpEffect;
}
