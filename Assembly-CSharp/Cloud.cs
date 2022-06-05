using System;
using System.Collections;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000718 RID: 1816
public class Cloud : MonoBehaviour, IDamageObj, IRoomConsumer, ITerrainOnEnterHitResponse, IHitResponse, ITerrainOnStayHitResponse
{
	// Token: 0x170014BF RID: 5311
	// (get) Token: 0x0600376D RID: 14189 RVA: 0x0001E7C5 File Offset: 0x0001C9C5
	public static bool HittingCloud
	{
		get
		{
			return Cloud.m_alreadyHitCloud;
		}
	}

	// Token: 0x170014C0 RID: 5312
	// (get) Token: 0x0600376E RID: 14190 RVA: 0x0000F49B File Offset: 0x0000D69B
	public string RelicDamageTypeString
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170014C1 RID: 5313
	// (get) Token: 0x0600376F RID: 14191 RVA: 0x0000F49B File Offset: 0x0000D69B
	public StatusEffectType[] StatusEffectTypes
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170014C2 RID: 5314
	// (get) Token: 0x06003770 RID: 14192 RVA: 0x0000F49B File Offset: 0x0000D69B
	public float[] StatusEffectDurations
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170014C3 RID: 5315
	// (get) Token: 0x06003771 RID: 14193 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public bool IsDotDamage
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170014C4 RID: 5316
	// (get) Token: 0x06003772 RID: 14194 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float ActualCritChance
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170014C5 RID: 5317
	// (get) Token: 0x06003773 RID: 14195 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float ActualCritDamage
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170014C6 RID: 5318
	// (get) Token: 0x06003774 RID: 14196 RVA: 0x000E5DD4 File Offset: 0x000E3FD4
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

	// Token: 0x170014C7 RID: 5319
	// (get) Token: 0x06003775 RID: 14197 RVA: 0x0001E7CC File Offset: 0x0001C9CC
	public float ActualKnockbackStrength
	{
		get
		{
			return this.BaseKnockbackStrength;
		}
	}

	// Token: 0x170014C8 RID: 5320
	// (get) Token: 0x06003776 RID: 14198 RVA: 0x0001E7D4 File Offset: 0x0001C9D4
	public float ActualStunStrength
	{
		get
		{
			return this.BaseStunStrength;
		}
	}

	// Token: 0x170014C9 RID: 5321
	// (get) Token: 0x06003777 RID: 14199 RVA: 0x0001E7DC File Offset: 0x0001C9DC
	public virtual float BaseDamage
	{
		get
		{
			return this.ActualDamage;
		}
	}

	// Token: 0x170014CA RID: 5322
	// (get) Token: 0x06003778 RID: 14200 RVA: 0x0001E7E4 File Offset: 0x0001C9E4
	// (set) Token: 0x06003779 RID: 14201 RVA: 0x0001E7EC File Offset: 0x0001C9EC
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

	// Token: 0x170014CB RID: 5323
	// (get) Token: 0x0600377A RID: 14202 RVA: 0x0001E7F5 File Offset: 0x0001C9F5
	// (set) Token: 0x0600377B RID: 14203 RVA: 0x0001E7FD File Offset: 0x0001C9FD
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

	// Token: 0x170014CC RID: 5324
	// (get) Token: 0x0600377C RID: 14204 RVA: 0x0001E806 File Offset: 0x0001CA06
	public Vector2 ExternalKnockbackMod
	{
		get
		{
			return this.m_knockbackMod;
		}
	}

	// Token: 0x170014CD RID: 5325
	// (get) Token: 0x0600377D RID: 14205 RVA: 0x0001E7F5 File Offset: 0x0001C9F5
	public float KnockbackStrength
	{
		get
		{
			return this.m_knockbackStrength;
		}
	}

	// Token: 0x170014CE RID: 5326
	// (get) Token: 0x0600377E RID: 14206 RVA: 0x000046FA File Offset: 0x000028FA
	public StrikeType StrikeType
	{
		get
		{
			return StrikeType.Blunt;
		}
	}

	// Token: 0x170014CF RID: 5327
	// (get) Token: 0x0600377F RID: 14207 RVA: 0x0001E80E File Offset: 0x0001CA0E
	// (set) Token: 0x06003780 RID: 14208 RVA: 0x0001E816 File Offset: 0x0001CA16
	public BaseRoom Room { get; private set; }

	// Token: 0x06003781 RID: 14209 RVA: 0x000E5E4C File Offset: 0x000E404C
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_hbController = base.gameObject.GetComponentInChildren<IHitboxController>(true);
		base.tag = "Hazard";
		this.m_onPlayerJustHit = new Action<object, CharacterHitEventArgs>(this.OnPlayerJustHit);
	}

	// Token: 0x06003782 RID: 14210 RVA: 0x0001E81F File Offset: 0x0001CA1F
	private void OnEnable()
	{
		if (PlayerManager.IsInstantiated)
		{
			PlayerManager.GetPlayerController().CharacterHitResponse.OnCharacterHitRelay.AddListener(this.m_onPlayerJustHit, false);
		}
	}

	// Token: 0x06003783 RID: 14211 RVA: 0x000E5E9C File Offset: 0x000E409C
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

	// Token: 0x06003784 RID: 14212 RVA: 0x0001E844 File Offset: 0x0001CA44
	private void OnPlayerJustHit(object sender, EventArgs args)
	{
		this.m_charHitFrameCount = Time.frameCount;
	}

	// Token: 0x06003785 RID: 14213 RVA: 0x000E5F70 File Offset: 0x000E4170
	private void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		this.m_playerPositionOnEnterRoom = PlayerManager.GetPlayer().transform.position;
		if (this.m_hbController != null && this.m_hbController.IsInitialized)
		{
			this.m_hbController.RepeatHitDuration = 0f;
		}
	}

	// Token: 0x06003786 RID: 14214 RVA: 0x0001E851 File Offset: 0x0001CA51
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

	// Token: 0x06003787 RID: 14215 RVA: 0x0001E881 File Offset: 0x0001CA81
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

	// Token: 0x06003788 RID: 14216 RVA: 0x000E5FBC File Offset: 0x000E41BC
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

	// Token: 0x06003789 RID: 14217 RVA: 0x0001E890 File Offset: 0x0001CA90
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

	// Token: 0x0600378A RID: 14218 RVA: 0x0001E8B4 File Offset: 0x0001CAB4
	private void OnRoomDestroyed(object sender, EventArgs eventArgs)
	{
		this.ClearEventHandlers();
	}

	// Token: 0x0600378B RID: 14219 RVA: 0x000E61E4 File Offset: 0x000E43E4
	private void ClearEventHandlers()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
			this.Room.RoomDestroyedRelay.RemoveListener(new Action<object, EventArgs>(this.OnRoomDestroyed));
		}
	}

	// Token: 0x0600378C RID: 14220 RVA: 0x0001E8B4 File Offset: 0x0001CAB4
	private void OnDestroy()
	{
		this.ClearEventHandlers();
	}

	// Token: 0x0600378D RID: 14221 RVA: 0x000E6238 File Offset: 0x000E4438
	public void SetRoom(BaseRoom room)
	{
		this.ClearEventHandlers();
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
		this.Room.RoomDestroyedRelay.AddListener(new Action<object, EventArgs>(this.OnRoomDestroyed), false);
	}

	// Token: 0x0600378E RID: 14222 RVA: 0x0001E8BC File Offset: 0x0001CABC
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (otherHBController.RootGameObject.CompareTag("Player") || otherHBController.RootGameObject.CompareTag("Player_Dodging"))
		{
			this.OnPlayerHitCloud();
		}
	}

	// Token: 0x0600378F RID: 14223 RVA: 0x0001E8E8 File Offset: 0x0001CAE8
	public void TerrainOnStayHitResponse(IHitboxController otherHBController)
	{
		this.TerrainOnEnterHitResponse(otherHBController);
	}

	// Token: 0x06003791 RID: 14225 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IDamageObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002CAA RID: 11434
	private static bool m_alreadyHitCloud;

	// Token: 0x04002CAB RID: 11435
	[SerializeField]
	private AnimationCurve m_animCurve;

	// Token: 0x04002CAC RID: 11436
	private Vector2 m_knockbackMod = new Vector2(1f, 1.8f);

	// Token: 0x04002CAD RID: 11437
	private float m_knockbackStrength = 99f;

	// Token: 0x04002CAE RID: 11438
	private float m_baseStunStrength;

	// Token: 0x04002CAF RID: 11439
	private Vector2 m_playerPositionOnEnterRoom;

	// Token: 0x04002CB0 RID: 11440
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002CB1 RID: 11441
	private IHitboxController m_hbController;

	// Token: 0x04002CB2 RID: 11442
	private int m_charHitFrameCount;

	// Token: 0x04002CB3 RID: 11443
	private int m_cloudHitFrameCount;

	// Token: 0x04002CB4 RID: 11444
	private bool m_disableDamage;

	// Token: 0x04002CB5 RID: 11445
	private Tween m_teleportTween;

	// Token: 0x04002CB6 RID: 11446
	private bool m_playerInputDisabled;

	// Token: 0x04002CB7 RID: 11447
	private Action<object, CharacterHitEventArgs> m_onPlayerJustHit;

	// Token: 0x04002CB8 RID: 11448
	public Relay<Vector2> PlayerHitRelay = new Relay<Vector2>();

	// Token: 0x04002CB9 RID: 11449
	public Relay TeleportStartRelay = new Relay();

	// Token: 0x04002CBA RID: 11450
	public Relay<Vector2> TeleportCompleteRelay = new Relay<Vector2>();
}
