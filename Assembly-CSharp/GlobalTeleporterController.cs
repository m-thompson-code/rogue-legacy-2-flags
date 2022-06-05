using System;
using System.Collections;
using RLWorldCreation;
using RL_Windows;
using SceneManagement_RL;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000204 RID: 516
public class GlobalTeleporterController : MonoBehaviour, IRootObj
{
	// Token: 0x17000AFF RID: 2815
	// (get) Token: 0x060015C6 RID: 5574 RVA: 0x00043EC0 File Offset: 0x000420C0
	// (set) Token: 0x060015C7 RID: 5575 RVA: 0x00043EC7 File Offset: 0x000420C7
	public static bool StayInCutsceneAfterTeleporting { get; set; }

	// Token: 0x17000B00 RID: 2816
	// (get) Token: 0x060015C8 RID: 5576 RVA: 0x00043ECF File Offset: 0x000420CF
	// (set) Token: 0x060015C9 RID: 5577 RVA: 0x00043ED6 File Offset: 0x000420D6
	public static bool IsTeleporting { get; private set; }

	// Token: 0x17000B01 RID: 2817
	// (get) Token: 0x060015CA RID: 5578 RVA: 0x00043EDE File Offset: 0x000420DE
	public GlobalTeleporterType TeleporterType
	{
		get
		{
			return this.m_teleporterType;
		}
	}

	// Token: 0x17000B02 RID: 2818
	// (get) Token: 0x060015CB RID: 5579 RVA: 0x00043EE6 File Offset: 0x000420E6
	// (set) Token: 0x060015CC RID: 5580 RVA: 0x00043EEE File Offset: 0x000420EE
	public Room Room { get; set; }

	// Token: 0x060015CD RID: 5581 RVA: 0x00043EF7 File Offset: 0x000420F7
	private void OnDisable()
	{
		if (this.m_interactable != null)
		{
			this.m_interactable.TriggerOnEnterEvent.RemoveListener(new UnityAction<GameObject>(this.OnInteract));
		}
	}

	// Token: 0x060015CE RID: 5582 RVA: 0x00043F24 File Offset: 0x00042124
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

	// Token: 0x060015CF RID: 5583 RVA: 0x00043F7C File Offset: 0x0004217C
	private void Awake()
	{
		this.Room = base.GetComponentInParent<Room>();
		this.m_interactable = base.GetComponent<Interactable>();
		this.m_hitboxController = base.GetComponentInChildren<IHitboxController>();
		this.m_animWaitUntilLandedYield = new WaitUntil(() => PlayerManager.GetPlayerController().IsGrounded);
		this.m_animWaitUntilTransitionYield = new WaitUntil(() => !SceneLoader_RL.IsRunningTransitionWithLogic);
		this.m_animWaitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x060015D0 RID: 5584 RVA: 0x00044012 File Offset: 0x00042212
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

	// Token: 0x060015D1 RID: 5585 RVA: 0x00044021 File Offset: 0x00042221
	private void OnInteract(GameObject otherObj)
	{
		GlobalTeleporterController.ActiveTeleporter = this;
		if (this.m_playerInteractedWithEvent != null)
		{
			this.m_playerInteractedWithEvent.Invoke();
		}
		WindowManager.SetWindowIsOpen(WindowID.Teleporter, true);
	}

	// Token: 0x060015D2 RID: 5586 RVA: 0x00044044 File Offset: 0x00042244
	public void OnEnterTeleportPlayer_V2(BiomeType biomeType, GridPointManager gridPointManager)
	{
		base.StartCoroutine(this.RunOnEnterTeleporterCoroutine_V2(biomeType, gridPointManager));
	}

	// Token: 0x060015D3 RID: 5587 RVA: 0x00044055 File Offset: 0x00042255
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

	// Token: 0x060015D4 RID: 5588 RVA: 0x00044072 File Offset: 0x00042272
	private IEnumerator TeleportPlayer_V2(BiomeType biomeType, GridPointManager gridPointManager)
	{
		yield return MapController.TeleportPlayerToRoom_Coroutine(biomeType, gridPointManager, true);
		PlayerManager.GetPlayerController().Visuals.gameObject.SetActive(true);
		this.m_interactable.SetIsInteractableActive(true);
		yield break;
	}

	// Token: 0x060015D5 RID: 5589 RVA: 0x0004408F File Offset: 0x0004228F
	public void OnExitTeleportPlayer()
	{
		GlobalTeleporterController.ActiveTeleporter = null;
		base.StartCoroutine(this.RunOnExitTeleporterCoroutine());
	}

	// Token: 0x060015D6 RID: 5590 RVA: 0x000440A4 File Offset: 0x000422A4
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

	// Token: 0x060015D9 RID: 5593 RVA: 0x000440BD File Offset: 0x000422BD
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040014F4 RID: 5364
	public static GlobalTeleporterController ActiveTeleporter;

	// Token: 0x040014F7 RID: 5367
	[SerializeField]
	private ParticleSystem m_onActivateParticleSystem;

	// Token: 0x040014F8 RID: 5368
	[SerializeField]
	private GlobalTeleporterType m_teleporterType;

	// Token: 0x040014F9 RID: 5369
	[SerializeField]
	private GameObject m_distortPosition;

	// Token: 0x040014FA RID: 5370
	[SerializeField]
	private UnityEvent m_playerInteractedWithEvent;

	// Token: 0x040014FB RID: 5371
	[SerializeField]
	private UnityEvent m_teleportStart;

	// Token: 0x040014FC RID: 5372
	[SerializeField]
	private UnityEvent m_playerJumpedOnPlatformEvent;

	// Token: 0x040014FD RID: 5373
	[SerializeField]
	private UnityEvent m_dematerializeStartEvent;

	// Token: 0x040014FE RID: 5374
	[SerializeField]
	private UnityEvent m_dematerializeCompleteEvent;

	// Token: 0x040014FF RID: 5375
	[SerializeField]
	private UnityEvent m_rematerializeStartEvent;

	// Token: 0x04001500 RID: 5376
	[SerializeField]
	private UnityEvent m_transitionStartEvent;

	// Token: 0x04001501 RID: 5377
	[SerializeField]
	private UnityEvent m_transitionCompleteEvent;

	// Token: 0x04001502 RID: 5378
	private Interactable m_interactable;

	// Token: 0x04001503 RID: 5379
	private IHitboxController m_hitboxController;

	// Token: 0x04001504 RID: 5380
	private WaitRL_Yield m_animWaitYield;

	// Token: 0x04001505 RID: 5381
	private WaitUntil m_animWaitUntilLandedYield;

	// Token: 0x04001506 RID: 5382
	private WaitUntil m_animWaitUntilTransitionYield;

	// Token: 0x04001507 RID: 5383
	private HeirloomWarp_Effect m_warpEffect;
}
