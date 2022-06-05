using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x02000404 RID: 1028
[RequireComponent(typeof(Rigidbody2D))]
public class EarthShiftPlatformController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x17000F5C RID: 3932
	// (get) Token: 0x06002650 RID: 9808 RVA: 0x0007EBCC File Offset: 0x0007CDCC
	public string SFXNameLoopOverride
	{
		get
		{
			return this.m_sfxNameLoopOverride;
		}
	}

	// Token: 0x17000F5D RID: 3933
	// (get) Token: 0x06002651 RID: 9809 RVA: 0x0007EBD4 File Offset: 0x0007CDD4
	public string SFXNameStopOverride
	{
		get
		{
			return this.m_sfxNameStopOverride;
		}
	}

	// Token: 0x17000F5E RID: 3934
	// (get) Token: 0x06002652 RID: 9810 RVA: 0x0007EBDC File Offset: 0x0007CDDC
	public bool MovesThroughWater
	{
		get
		{
			return this.m_movesThroughWater;
		}
	}

	// Token: 0x17000F5F RID: 3935
	// (get) Token: 0x06002653 RID: 9811 RVA: 0x0007EBE4 File Offset: 0x0007CDE4
	public EarthShiftPlatformController.EarthShiftPlatformSize PlatformSize
	{
		get
		{
			return this.m_platformSize;
		}
	}

	// Token: 0x17000F60 RID: 3936
	// (get) Token: 0x06002654 RID: 9812 RVA: 0x0007EBEC File Offset: 0x0007CDEC
	public MeshRenderer Renderer
	{
		get
		{
			return this.m_renderer;
		}
	}

	// Token: 0x17000F61 RID: 3937
	// (get) Token: 0x06002655 RID: 9813 RVA: 0x0007EBF4 File Offset: 0x0007CDF4
	public float Progress
	{
		get
		{
			return this.m_progress;
		}
	}

	// Token: 0x17000F62 RID: 3938
	// (get) Token: 0x06002656 RID: 9814 RVA: 0x0007EBFC File Offset: 0x0007CDFC
	// (set) Token: 0x06002657 RID: 9815 RVA: 0x0007EC04 File Offset: 0x0007CE04
	public BaseRoom Room { get; private set; }

	// Token: 0x06002658 RID: 9816 RVA: 0x0007EC0D File Offset: 0x0007CE0D
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.m_roomRelativeStartingPos = base.transform.position - room.transform.position;
	}

	// Token: 0x06002659 RID: 9817 RVA: 0x0007EC38 File Offset: 0x0007CE38
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

	// Token: 0x0600265A RID: 9818 RVA: 0x0007ECD4 File Offset: 0x0007CED4
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x0600265B RID: 9819 RVA: 0x0007ECE2 File Offset: 0x0007CEE2
	private void OnPlayerEnterRoom(MonoBehaviour sender, EventArgs args)
	{
		base.gameObject.tag = "MagicPlatform";
		this.ResetToOrigin();
		this.m_triggerSpawnController.PropInstance.GetComponent<EarthShiftTriggerController>().SetPlatform(this);
	}

	// Token: 0x0600265C RID: 9820 RVA: 0x0007ED10 File Offset: 0x0007CF10
	private void OnDisable()
	{
		this.ResetToOrigin();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x0600265D RID: 9821 RVA: 0x0007ED24 File Offset: 0x0007CF24
	private void ResetToOrigin()
	{
		this.m_progress = 0f;
		if (this.Room)
		{
			base.transform.position = this.Room.transform.position + this.m_roomRelativeStartingPos;
		}
	}

	// Token: 0x0600265E RID: 9822 RVA: 0x0007ED64 File Offset: 0x0007CF64
	public void Move()
	{
		this.m_progress += Time.deltaTime / this.m_timeToReachDestination;
		this.m_progress = Mathf.Clamp(this.m_progress, 0f, 1f);
		base.transform.position = Vector3.Lerp(this.Room.transform.position + this.m_roomRelativeStartingPos, this.m_destination.transform.position, this.m_progress);
	}

	// Token: 0x0600265F RID: 9823 RVA: 0x0007EDE8 File Offset: 0x0007CFE8
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

	// Token: 0x06002660 RID: 9824 RVA: 0x0007EE6D File Offset: 0x0007D06D
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

	// Token: 0x04001FF5 RID: 8181
	private const string TELEPORT_HIT_SFX_NAME = "event:/SFX/Weapons/sfx_player_splash_return";

	// Token: 0x04001FF6 RID: 8182
	[SerializeField]
	private GameObject m_destination;

	// Token: 0x04001FF7 RID: 8183
	[SerializeField]
	private float m_timeToReachDestination;

	// Token: 0x04001FF8 RID: 8184
	[SerializeField]
	private PropSpawnController m_triggerSpawnController;

	// Token: 0x04001FF9 RID: 8185
	[SerializeField]
	private EarthShiftPlatformController.EarthShiftPlatformSize m_platformSize;

	// Token: 0x04001FFA RID: 8186
	[SerializeField]
	private string m_sfxNameLoopOverride;

	// Token: 0x04001FFB RID: 8187
	[SerializeField]
	private string m_sfxNameStopOverride;

	// Token: 0x04001FFC RID: 8188
	[SerializeField]
	private bool m_movesThroughWater;

	// Token: 0x04001FFD RID: 8189
	private Vector3 m_roomRelativeStartingPos;

	// Token: 0x04001FFE RID: 8190
	private float m_progress;

	// Token: 0x04001FFF RID: 8191
	private Collider2D m_collider;

	// Token: 0x04002000 RID: 8192
	private Collider2D[] m_overlappingColliders = new Collider2D[1];

	// Token: 0x04002001 RID: 8193
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002002 RID: 8194
	private Tween m_teleportTween;

	// Token: 0x04002003 RID: 8195
	private MeshRenderer m_renderer;

	// Token: 0x04002004 RID: 8196
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x02000C29 RID: 3113
	public enum EarthShiftPlatformSize
	{
		// Token: 0x04004F53 RID: 20307
		Small,
		// Token: 0x04004F54 RID: 20308
		Medium,
		// Token: 0x04004F55 RID: 20309
		Large,
		// Token: 0x04004F56 RID: 20310
		Final
	}
}
