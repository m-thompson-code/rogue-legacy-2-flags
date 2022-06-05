using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x020006B4 RID: 1716
[RequireComponent(typeof(Rigidbody2D))]
public class EarthShiftPlatformController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x17001415 RID: 5141
	// (get) Token: 0x060034E1 RID: 13537 RVA: 0x0001CFDF File Offset: 0x0001B1DF
	public string SFXNameLoopOverride
	{
		get
		{
			return this.m_sfxNameLoopOverride;
		}
	}

	// Token: 0x17001416 RID: 5142
	// (get) Token: 0x060034E2 RID: 13538 RVA: 0x0001CFE7 File Offset: 0x0001B1E7
	public string SFXNameStopOverride
	{
		get
		{
			return this.m_sfxNameStopOverride;
		}
	}

	// Token: 0x17001417 RID: 5143
	// (get) Token: 0x060034E3 RID: 13539 RVA: 0x0001CFEF File Offset: 0x0001B1EF
	public bool MovesThroughWater
	{
		get
		{
			return this.m_movesThroughWater;
		}
	}

	// Token: 0x17001418 RID: 5144
	// (get) Token: 0x060034E4 RID: 13540 RVA: 0x0001CFF7 File Offset: 0x0001B1F7
	public EarthShiftPlatformController.EarthShiftPlatformSize PlatformSize
	{
		get
		{
			return this.m_platformSize;
		}
	}

	// Token: 0x17001419 RID: 5145
	// (get) Token: 0x060034E5 RID: 13541 RVA: 0x0001CFFF File Offset: 0x0001B1FF
	public MeshRenderer Renderer
	{
		get
		{
			return this.m_renderer;
		}
	}

	// Token: 0x1700141A RID: 5146
	// (get) Token: 0x060034E6 RID: 13542 RVA: 0x0001D007 File Offset: 0x0001B207
	public float Progress
	{
		get
		{
			return this.m_progress;
		}
	}

	// Token: 0x1700141B RID: 5147
	// (get) Token: 0x060034E7 RID: 13543 RVA: 0x0001D00F File Offset: 0x0001B20F
	// (set) Token: 0x060034E8 RID: 13544 RVA: 0x0001D017 File Offset: 0x0001B217
	public BaseRoom Room { get; private set; }

	// Token: 0x060034E9 RID: 13545 RVA: 0x0001D020 File Offset: 0x0001B220
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.m_roomRelativeStartingPos = base.transform.position - room.transform.position;
	}

	// Token: 0x060034EA RID: 13546 RVA: 0x000DEB50 File Offset: 0x000DCD50
	private void Awake()
	{
		if (!this.m_triggerSpawnController)
		{
			throw new Exception("No trigger spawn controller was assigned for this EarthShiftPlatformController!");
		}
		if (!this.m_destination)
		{
			throw new Exception("EarthShiftPlatformController is missing a destination!");
		}
		if (!base.GetComponent<Ferr2DT_PathTerrain>().DisableMerging)
		{
			throw new Exception("EarthShiftPlatformController is attached to a platform that does NOT have merging disabled!");
		}
		this.m_collider = base.GetComponent<Collider2D>();
		this.m_renderer = base.GetComponent<MeshRenderer>();
		base.GetComponent<Rigidbody2D>().isKinematic = true;
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x060034EB RID: 13547 RVA: 0x0001D04A File Offset: 0x0001B24A
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x060034EC RID: 13548 RVA: 0x0001D058 File Offset: 0x0001B258
	private void OnPlayerEnterRoom(MonoBehaviour sender, EventArgs args)
	{
		base.gameObject.tag = "MagicPlatform";
		this.ResetToOrigin();
		this.m_triggerSpawnController.PropInstance.GetComponent<EarthShiftTriggerController>().SetPlatform(this);
	}

	// Token: 0x060034ED RID: 13549 RVA: 0x0001D086 File Offset: 0x0001B286
	private void OnDisable()
	{
		this.ResetToOrigin();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x060034EE RID: 13550 RVA: 0x0001D09A File Offset: 0x0001B29A
	private void ResetToOrigin()
	{
		this.m_progress = 0f;
		if (this.Room)
		{
			base.transform.position = this.Room.transform.position + this.m_roomRelativeStartingPos;
		}
	}

	// Token: 0x060034EF RID: 13551 RVA: 0x000DEBEC File Offset: 0x000DCDEC
	public void Move()
	{
		this.m_progress += Time.deltaTime / this.m_timeToReachDestination;
		this.m_progress = Mathf.Clamp(this.m_progress, 0f, 1f);
		base.transform.position = Vector3.Lerp(this.Room.transform.position + this.m_roomRelativeStartingPos, this.m_destination.transform.position, this.m_progress);
	}

	// Token: 0x060034F0 RID: 13552 RVA: 0x000DEC70 File Offset: 0x000DCE70
	public void ResetPosition()
	{
		this.ResetToOrigin();
		bool flag = false;
		ContactFilter2D contactFilter = default(ContactFilter2D);
		contactFilter.NoFilter();
		contactFilter.SetLayerMask(1048576);
		if (Physics2D.OverlapCollider(this.m_collider, contactFilter, this.m_overlappingColliders) > 0 && (this.m_overlappingColliders[0].CompareTag("Player") || this.m_overlappingColliders[0].CompareTag("Player_Dodging")))
		{
			flag = true;
		}
		if (flag)
		{
			base.StartCoroutine(this.TeleportPlayerCoroutine());
		}
	}

	// Token: 0x060034F1 RID: 13553 RVA: 0x0001D0DA File Offset: 0x0001B2DA
	private IEnumerator TeleportPlayerCoroutine()
	{
		RewiredMapController.SetMapEnabled(GameInputMode.Game, false);
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.ControllerCorgi.CollisionsOff();
		playerController.SetVelocity(0f, 0f, false);
		playerController.ControllerCorgi.GravityActive(false);
		playerController.HitboxController.SetHitboxActiveState(HitboxType.Body, false);
		EffectManager.PlayEffect(playerController.gameObject, playerController.Animator, "CloudHazardPoof_Effect", playerController.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		playerController.Visuals.SetActive(false);
		playerController.CharacterHitResponse.SetInvincibleTime(999f, false, false);
		BaseEffect trailEffect = EffectManager.PlayEffect(playerController.gameObject, playerController.Animator, "CloudHazardSpark_Effect", playerController.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		trailEffect.transform.SetParent(playerController.transform);
		Vector3 localPosition = playerController.Midpoint - playerController.transform.position;
		localPosition.z = trailEffect.transform.localPosition.z;
		trailEffect.transform.localPosition = localPosition;
		Vector3 position = this.m_triggerSpawnController.PropInstance.transform.position;
		playerController.ControllerCorgi.SetLastStandingPosition(position);
		AudioManager.PlayOneShot(null, "event:/SFX/Weapons/sfx_player_splash_return", playerController.Midpoint);
		if (this.m_teleportTween != null)
		{
			this.m_teleportTween.StopTweenWithConditionChecks(false, playerController.transform, null);
		}
		this.m_teleportTween = TweenManager.TweenTo(playerController.transform, 0.25f, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
		{
			"position.x",
			position.x,
			"position.y",
			position.y
		});
		yield return this.m_teleportTween.TweenCoroutine;
		playerController.Visuals.SetActive(true);
		playerController.ControllerCorgi.GravityActive(true);
		playerController.ControllerCorgi.CollisionsOn();
		playerController.HitboxController.SetHitboxActiveState(HitboxType.Body, true);
		playerController.CharacterJump.ResetNumberOfJumps();
		playerController.CharacterDash.ResetNumberOfDashes();
		EffectManager.PlayEffect(playerController.gameObject, playerController.Animator, "CloudHazardPoof_Effect", playerController.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		trailEffect.Stop(EffectStopType.Gracefully);
		this.m_waitYield.CreateNew(0.25f, false);
		yield return this.m_waitYield;
		if (playerController.CurrentHealth > 0f)
		{
			playerController.CharacterHitResponse.SetInvincibleTime(1f, false, false);
			RewiredMapController.SetMapEnabled(GameInputMode.Game, true);
		}
		yield break;
	}

	// Token: 0x04002AB3 RID: 10931
	private const string TELEPORT_HIT_SFX_NAME = "event:/SFX/Weapons/sfx_player_splash_return";

	// Token: 0x04002AB4 RID: 10932
	[SerializeField]
	private GameObject m_destination;

	// Token: 0x04002AB5 RID: 10933
	[SerializeField]
	private float m_timeToReachDestination;

	// Token: 0x04002AB6 RID: 10934
	[SerializeField]
	private PropSpawnController m_triggerSpawnController;

	// Token: 0x04002AB7 RID: 10935
	[SerializeField]
	private EarthShiftPlatformController.EarthShiftPlatformSize m_platformSize;

	// Token: 0x04002AB8 RID: 10936
	[SerializeField]
	private string m_sfxNameLoopOverride;

	// Token: 0x04002AB9 RID: 10937
	[SerializeField]
	private string m_sfxNameStopOverride;

	// Token: 0x04002ABA RID: 10938
	[SerializeField]
	private bool m_movesThroughWater;

	// Token: 0x04002ABB RID: 10939
	private Vector3 m_roomRelativeStartingPos;

	// Token: 0x04002ABC RID: 10940
	private float m_progress;

	// Token: 0x04002ABD RID: 10941
	private Collider2D m_collider;

	// Token: 0x04002ABE RID: 10942
	private Collider2D[] m_overlappingColliders = new Collider2D[1];

	// Token: 0x04002ABF RID: 10943
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002AC0 RID: 10944
	private Tween m_teleportTween;

	// Token: 0x04002AC1 RID: 10945
	private MeshRenderer m_renderer;

	// Token: 0x04002AC2 RID: 10946
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x020006B5 RID: 1717
	public enum EarthShiftPlatformSize
	{
		// Token: 0x04002AC5 RID: 10949
		Small,
		// Token: 0x04002AC6 RID: 10950
		Medium,
		// Token: 0x04002AC7 RID: 10951
		Large,
		// Token: 0x04002AC8 RID: 10952
		Final
	}
}
