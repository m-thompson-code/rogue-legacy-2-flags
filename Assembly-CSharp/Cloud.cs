using System;
using System.Collections;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000447 RID: 1095
public class Cloud : MonoBehaviour, IDamageObj, IRoomConsumer, ITerrainOnEnterHitResponse, IHitResponse, ITerrainOnStayHitResponse
{
	// Token: 0x17000FCC RID: 4044
	// (get) Token: 0x0600282B RID: 10283 RVA: 0x0008530A File Offset: 0x0008350A
	public static bool HittingCloud
	{
		get
		{
			return Cloud.m_alreadyHitCloud;
		}
	}

	// Token: 0x17000FCD RID: 4045
	// (get) Token: 0x0600282C RID: 10284 RVA: 0x00085311 File Offset: 0x00083511
	public string RelicDamageTypeString
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000FCE RID: 4046
	// (get) Token: 0x0600282D RID: 10285 RVA: 0x00085314 File Offset: 0x00083514
	public StatusEffectType[] StatusEffectTypes
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000FCF RID: 4047
	// (get) Token: 0x0600282E RID: 10286 RVA: 0x00085317 File Offset: 0x00083517
	public float[] StatusEffectDurations
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000FD0 RID: 4048
	// (get) Token: 0x0600282F RID: 10287 RVA: 0x0008531A File Offset: 0x0008351A
	public bool IsDotDamage
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000FD1 RID: 4049
	// (get) Token: 0x06002830 RID: 10288 RVA: 0x0008531D File Offset: 0x0008351D
	public float ActualCritChance
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000FD2 RID: 4050
	// (get) Token: 0x06002831 RID: 10289 RVA: 0x00085324 File Offset: 0x00083524
	public float ActualCritDamage
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000FD3 RID: 4051
	// (get) Token: 0x06002832 RID: 10290 RVA: 0x0008532C File Offset: 0x0008352C
	public virtual float ActualDamage
	{
		get
		{
			if (!PlayerManager.IsInstantiated)
			{
				return 0f;
			}
			PlayerController playerController = PlayerManager.GetPlayerController();
			if (this.m_disableDamage)
			{
				return 0f;
			}
			Room room = this.Room as Room;
			if (room != null && room.GridPointManager.RoomMetaData.SpecialRoomType == SpecialRoomType.Heirloom)
			{
				float num = 0f;
				return (float)playerController.BaseMaxHealth * num;
			}
			return Hazard_EV.GetDamageAmount(this.Room);
		}
	}

	// Token: 0x17000FD4 RID: 4052
	// (get) Token: 0x06002833 RID: 10291 RVA: 0x000853A4 File Offset: 0x000835A4
	public float ActualKnockbackStrength
	{
		get
		{
			return this.BaseKnockbackStrength;
		}
	}

	// Token: 0x17000FD5 RID: 4053
	// (get) Token: 0x06002834 RID: 10292 RVA: 0x000853AC File Offset: 0x000835AC
	public float ActualStunStrength
	{
		get
		{
			return this.BaseStunStrength;
		}
	}

	// Token: 0x17000FD6 RID: 4054
	// (get) Token: 0x06002835 RID: 10293 RVA: 0x000853B4 File Offset: 0x000835B4
	public virtual float BaseDamage
	{
		get
		{
			return this.ActualDamage;
		}
	}

	// Token: 0x17000FD7 RID: 4055
	// (get) Token: 0x06002836 RID: 10294 RVA: 0x000853BC File Offset: 0x000835BC
	// (set) Token: 0x06002837 RID: 10295 RVA: 0x000853C4 File Offset: 0x000835C4
	public float BaseStunStrength
	{
		get
		{
			return this.m_baseStunStrength;
		}
		set
		{
			this.m_baseStunStrength = value;
		}
	}

	// Token: 0x17000FD8 RID: 4056
	// (get) Token: 0x06002838 RID: 10296 RVA: 0x000853CD File Offset: 0x000835CD
	// (set) Token: 0x06002839 RID: 10297 RVA: 0x000853D5 File Offset: 0x000835D5
	public float BaseKnockbackStrength
	{
		get
		{
			return this.m_knockbackStrength;
		}
		set
		{
			this.m_knockbackStrength = value;
		}
	}

	// Token: 0x17000FD9 RID: 4057
	// (get) Token: 0x0600283A RID: 10298 RVA: 0x000853DE File Offset: 0x000835DE
	public Vector2 ExternalKnockbackMod
	{
		get
		{
			return this.m_knockbackMod;
		}
	}

	// Token: 0x17000FDA RID: 4058
	// (get) Token: 0x0600283B RID: 10299 RVA: 0x000853E6 File Offset: 0x000835E6
	public float KnockbackStrength
	{
		get
		{
			return this.m_knockbackStrength;
		}
	}

	// Token: 0x17000FDB RID: 4059
	// (get) Token: 0x0600283C RID: 10300 RVA: 0x000853EE File Offset: 0x000835EE
	public StrikeType StrikeType
	{
		get
		{
			return StrikeType.Blunt;
		}
	}

	// Token: 0x17000FDC RID: 4060
	// (get) Token: 0x0600283D RID: 10301 RVA: 0x000853F2 File Offset: 0x000835F2
	// (set) Token: 0x0600283E RID: 10302 RVA: 0x000853FA File Offset: 0x000835FA
	public BaseRoom Room { get; private set; }

	// Token: 0x0600283F RID: 10303 RVA: 0x00085404 File Offset: 0x00083604
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_hbController = base.gameObject.GetComponentInChildren<IHitboxController>(true);
		base.tag = "Hazard";
		this.m_onPlayerJustHit = new Action<object, CharacterHitEventArgs>(this.OnPlayerJustHit);
	}

	// Token: 0x06002840 RID: 10304 RVA: 0x00085451 File Offset: 0x00083651
	private void OnEnable()
	{
		if (PlayerManager.IsInstantiated)
		{
			PlayerManager.GetPlayerController().CharacterHitResponse.OnCharacterHitRelay.AddListener(this.m_onPlayerJustHit, false);
		}
	}

	// Token: 0x06002841 RID: 10305 RVA: 0x00085478 File Offset: 0x00083678
	private void OnDisable()
	{
		if (PlayerManager.IsInstantiated && !PlayerManager.IsDisposed)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			playerController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onPlayerJustHit);
			if (this.m_teleportTween != null)
			{
				this.m_teleportTween.StopTweenWithConditionChecks(false, playerController.transform, null);
			}
			if (Cloud.m_alreadyHitCloud)
			{
				playerController.Visuals.SetActive(true);
				playerController.ControllerCorgi.GravityActive(true);
				playerController.ControllerCorgi.CollisionsOn();
				playerController.HitboxController.SetHitboxActiveState(HitboxType.Body, true);
			}
			if (this.m_playerInputDisabled)
			{
				if (playerController.CurrentHealth > 0f)
				{
					playerController.CharacterHitResponse.SetInvincibleTime(1f, false, false);
					RewiredMapController.SetMapEnabled(GameInputMode.Game, true);
				}
				this.m_playerInputDisabled = false;
			}
		}
		Cloud.m_alreadyHitCloud = false;
	}

	// Token: 0x06002842 RID: 10306 RVA: 0x0008554A File Offset: 0x0008374A
	private void OnPlayerJustHit(object sender, EventArgs args)
	{
		this.m_charHitFrameCount = Time.frameCount;
	}

	// Token: 0x06002843 RID: 10307 RVA: 0x00085558 File Offset: 0x00083758
	private void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		this.m_playerPositionOnEnterRoom = PlayerManager.GetPlayer().transform.position;
		if (this.m_hbController != null && this.m_hbController.IsInitialized)
		{
			this.m_hbController.RepeatHitDuration = 0f;
		}
	}

	// Token: 0x06002844 RID: 10308 RVA: 0x000855A4 File Offset: 0x000837A4
	private void OnPlayerHitCloud()
	{
		if (Cloud.m_alreadyHitCloud)
		{
			return;
		}
		if (LocalTeleporterController.IsTeleporting)
		{
			return;
		}
		this.m_cloudHitFrameCount = Time.frameCount;
		base.StopAllCoroutines();
		base.StartCoroutine(this.HitCloudCoroutine());
	}

	// Token: 0x06002845 RID: 10309 RVA: 0x000855D4 File Offset: 0x000837D4
	private IEnumerator HitCloudCoroutine()
	{
		Vector3 position = PlayerManager.GetPlayerController().transform.position;
		this.PlayerHitRelay.Dispatch(position);
		Cloud.m_alreadyHitCloud = true;
		this.m_disableDamage = false;
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.StopActiveAbilities(false);
		bool disableInput = true;
		if (this.m_charHitFrameCount != this.m_cloudHitFrameCount && (playerController.CharacterHitResponse.IsInvincible || playerController.CharacterHitResponse.IsStunned))
		{
			this.m_disableDamage = true;
			disableInput = false;
			playerController.CharacterHitResponse.StartHitResponse(this.m_hbController.RootGameObject, this.m_hbController.DamageObj, -1f, false, true);
		}
		yield return null;
		RewiredMapController.SetMapEnabled(GameInputMode.Game, !disableInput);
		this.m_playerInputDisabled = true;
		if (!this.m_disableDamage)
		{
			this.m_waitYield.CreateNew(0.25f, false);
			yield return this.m_waitYield;
		}
		if (LocalTeleporterController.IsTeleporting)
		{
			Cloud.m_alreadyHitCloud = false;
			this.m_playerInputDisabled = false;
			yield break;
		}
		playerController.ControllerCorgi.CollisionsOff();
		playerController.SetVelocity(0f, 0f, false);
		playerController.ControllerCorgi.GravityActive(false);
		playerController.HitboxController.SetHitboxActiveState(HitboxType.Body, false);
		EffectManager.PlayEffect(playerController.gameObject, playerController.Animator, "CloudHazardPoof_Effect", playerController.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		playerController.Visuals.SetActive(false);
		playerController.CharacterHitResponse.SetInvincibleTime(999f, false, false);
		this.TeleportStartRelay.Dispatch();
		BaseEffect trailEffect = EffectManager.PlayEffect(playerController.gameObject, playerController.Animator, "CloudHazardSpark_Effect", playerController.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		trailEffect.transform.SetParent(playerController.transform);
		Vector3 localPosition = playerController.Midpoint - playerController.transform.position;
		localPosition.z = trailEffect.transform.localPosition.z;
		trailEffect.transform.localPosition = localPosition;
		base.StartCoroutine(this.TrailAnimCurveCoroutine(0.25f, 5f, trailEffect.transform));
		Vector3 previousPosition = PlayerManager.GetPlayerController().ControllerCorgi.LastStandingPosition;
		previousPosition = this.FindSaferPosition(previousPosition);
		if (this.m_teleportTween != null)
		{
			this.m_teleportTween.StopTweenWithConditionChecks(false, playerController.transform, null);
		}
		this.m_teleportTween = TweenManager.TweenTo(playerController.transform, 0.25f, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
		{
			"position.x",
			previousPosition.x,
			"position.y",
			previousPosition.y
		});
		yield return this.m_teleportTween.TweenCoroutine;
		this.TeleportCompleteRelay.Dispatch(previousPosition);
		playerController.Visuals.SetActive(true);
		playerController.ControllerCorgi.GravityActive(true);
		playerController.ControllerCorgi.CollisionsOn();
		playerController.HitboxController.SetHitboxActiveState(HitboxType.Body, true);
		playerController.CharacterJump.ResetNumberOfJumps();
		playerController.CharacterDash.ResetNumberOfDashes();
		EffectManager.PlayEffect(playerController.gameObject, playerController.Animator, "CloudHazardPoof_Effect", playerController.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		trailEffect.Stop(EffectStopType.Gracefully);
		Cloud.m_alreadyHitCloud = false;
		this.m_waitYield.CreateNew(0.25f, false);
		yield return this.m_waitYield;
		if (playerController.CurrentHealth > 0f)
		{
			playerController.CharacterHitResponse.SetInvincibleTime(1f, false, false);
			RewiredMapController.SetMapEnabled(GameInputMode.Game, true);
		}
		this.m_playerInputDisabled = false;
		yield break;
	}

	// Token: 0x06002846 RID: 10310 RVA: 0x000855E4 File Offset: 0x000837E4
	private Vector3 FindSaferPosition(Vector3 currentPos)
	{
		Collider2D collider = this.m_hbController.GetCollider(HitboxType.Terrain);
		CorgiController_RL controllerCorgi = PlayerManager.GetPlayerController().ControllerCorgi;
		float boundsWidth = controllerCorgi.BoundsWidth;
		float num = boundsWidth / 2f;
		float num2 = controllerCorgi.BoundsHeight / 2f;
		float distance = num2 + 0.1f;
		Vector2 origin = currentPos;
		origin.y += num2;
		bool flag = false;
		bool flag2 = false;
		LayerMask mask = controllerCorgi.SavedPlatformMask;
		mask |= controllerCorgi.OneWayPlatformMask;
		origin.x = currentPos.x - boundsWidth;
		RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, distance, mask);
		Vector2 v = currentPos;
		if (hit && hit.distance > 0f)
		{
			Vector2 point2;
			Vector2 point = point2 = hit.point;
			point2.x -= num;
			point.x += num;
			if (!collider.OverlapPoint(point2) && !collider.OverlapPoint(point) && Mathf.Abs(Vector2.Angle(hit.normal, base.transform.up)) <= controllerCorgi.Parameters.MaximumSlopeAngle)
			{
				flag2 = true;
				v = hit.point;
			}
		}
		origin.x = currentPos.x + boundsWidth;
		hit = Physics2D.Raycast(origin, Vector2.down, distance, mask);
		Vector2 v2 = currentPos;
		if (hit && hit.distance > 0f)
		{
			Vector2 point2;
			Vector2 point = point2 = hit.point;
			point2.x -= num;
			point.x += num;
			if (!collider.OverlapPoint(point2) && !collider.OverlapPoint(point) && Mathf.Abs(Vector2.Angle(hit.normal, base.transform.up)) <= controllerCorgi.Parameters.MaximumSlopeAngle)
			{
				flag = true;
				v2 = hit.point;
			}
		}
		if (!flag2 && flag)
		{
			currentPos = v2;
		}
		else if (flag2 && !flag)
		{
			currentPos = v;
		}
		return currentPos;
	}

	// Token: 0x06002847 RID: 10311 RVA: 0x0008580C File Offset: 0x00083A0C
	private IEnumerator TrailAnimCurveCoroutine(float duration, float intensity, Transform transform)
	{
		float startTime = Time.time;
		while (Time.time - startTime < duration)
		{
			float time = (Time.time - startTime) / duration;
			Vector3 localPosition = transform.localPosition;
			localPosition.y = this.m_animCurve.Evaluate(time) * intensity;
			transform.localPosition = localPosition;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002848 RID: 10312 RVA: 0x00085830 File Offset: 0x00083A30
	private void OnRoomDestroyed(object sender, EventArgs eventArgs)
	{
		this.ClearEventHandlers();
	}

	// Token: 0x06002849 RID: 10313 RVA: 0x00085838 File Offset: 0x00083A38
	private void ClearEventHandlers()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
			this.Room.RoomDestroyedRelay.RemoveListener(new Action<object, EventArgs>(this.OnRoomDestroyed));
		}
	}

	// Token: 0x0600284A RID: 10314 RVA: 0x0008588C File Offset: 0x00083A8C
	private void OnDestroy()
	{
		this.ClearEventHandlers();
	}

	// Token: 0x0600284B RID: 10315 RVA: 0x00085894 File Offset: 0x00083A94
	public void SetRoom(BaseRoom room)
	{
		this.ClearEventHandlers();
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
		this.Room.RoomDestroyedRelay.AddListener(new Action<object, EventArgs>(this.OnRoomDestroyed), false);
	}

	// Token: 0x0600284C RID: 10316 RVA: 0x000858EA File Offset: 0x00083AEA
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (otherHBController.RootGameObject.CompareTag("Player") || otherHBController.RootGameObject.CompareTag("Player_Dodging"))
		{
			this.OnPlayerHitCloud();
		}
	}

	// Token: 0x0600284D RID: 10317 RVA: 0x00085916 File Offset: 0x00083B16
	public void TerrainOnStayHitResponse(IHitboxController otherHBController)
	{
		this.TerrainOnEnterHitResponse(otherHBController);
	}

	// Token: 0x0600284F RID: 10319 RVA: 0x00085974 File Offset: 0x00083B74
	GameObject IDamageObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400215C RID: 8540
	private static bool m_alreadyHitCloud;

	// Token: 0x0400215D RID: 8541
	[SerializeField]
	private AnimationCurve m_animCurve;

	// Token: 0x0400215E RID: 8542
	private Vector2 m_knockbackMod = new Vector2(1f, 1.8f);

	// Token: 0x0400215F RID: 8543
	private float m_knockbackStrength = 99f;

	// Token: 0x04002160 RID: 8544
	private float m_baseStunStrength;

	// Token: 0x04002161 RID: 8545
	private Vector2 m_playerPositionOnEnterRoom;

	// Token: 0x04002162 RID: 8546
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002163 RID: 8547
	private IHitboxController m_hbController;

	// Token: 0x04002164 RID: 8548
	private int m_charHitFrameCount;

	// Token: 0x04002165 RID: 8549
	private int m_cloudHitFrameCount;

	// Token: 0x04002166 RID: 8550
	private bool m_disableDamage;

	// Token: 0x04002167 RID: 8551
	private Tween m_teleportTween;

	// Token: 0x04002168 RID: 8552
	private bool m_playerInputDisabled;

	// Token: 0x04002169 RID: 8553
	private Action<object, CharacterHitEventArgs> m_onPlayerJustHit;

	// Token: 0x0400216A RID: 8554
	public Relay<Vector2> PlayerHitRelay = new Relay<Vector2>();

	// Token: 0x0400216B RID: 8555
	public Relay TeleportStartRelay = new Relay();

	// Token: 0x0400216C RID: 8556
	public Relay<Vector2> TeleportCompleteRelay = new Relay<Vector2>();
}
