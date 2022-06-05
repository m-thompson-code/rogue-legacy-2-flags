using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200041B RID: 1051
[SelectionBase]
public class LocalTeleporterController : MonoBehaviour, IRootObj
{
	// Token: 0x17000E9E RID: 3742
	// (get) Token: 0x06002163 RID: 8547 RVA: 0x00011CBE File Offset: 0x0000FEBE
	public static bool IsTeleporting
	{
		get
		{
			return LocalTeleporterController.m_isTeleporting;
		}
	}

	// Token: 0x17000E9F RID: 3743
	// (get) Token: 0x06002164 RID: 8548 RVA: 0x00011CC5 File Offset: 0x0000FEC5
	// (set) Token: 0x06002165 RID: 8549 RVA: 0x00011CCD File Offset: 0x0000FECD
	public Color TeleporterColor
	{
		get
		{
			return this.m_teleporterColor;
		}
		set
		{
			if (this.m_teleporterColor != value)
			{
				this.m_teleporterColor = value;
				this.UpdateTeleporterColour();
			}
		}
	}

	// Token: 0x17000EA0 RID: 3744
	// (get) Token: 0x06002166 RID: 8550 RVA: 0x00011CEA File Offset: 0x0000FEEA
	public LocalTeleporterController TeleporterLocation
	{
		get
		{
			return this.m_teleportLocation;
		}
	}

	// Token: 0x17000EA1 RID: 3745
	// (get) Token: 0x06002167 RID: 8551 RVA: 0x00011CF2 File Offset: 0x0000FEF2
	public Animator Animator
	{
		get
		{
			return this.m_animator;
		}
	}

	// Token: 0x17000EA2 RID: 3746
	// (get) Token: 0x06002168 RID: 8552 RVA: 0x00011CFA File Offset: 0x0000FEFA
	// (set) Token: 0x06002169 RID: 8553 RVA: 0x00011D02 File Offset: 0x0000FF02
	public Room Room { get; set; }

	// Token: 0x17000EA3 RID: 3747
	// (get) Token: 0x0600216A RID: 8554 RVA: 0x00011D0B File Offset: 0x0000FF0B
	public Ferr2DT_PathTerrain LeyLines
	{
		get
		{
			return this.m_leyLines;
		}
	}

	// Token: 0x17000EA4 RID: 3748
	// (get) Token: 0x0600216B RID: 8555 RVA: 0x00011D13 File Offset: 0x0000FF13
	// (set) Token: 0x0600216C RID: 8556 RVA: 0x00011D1B File Offset: 0x0000FF1B
	public int CurrentCornerIndex { get; private set; }

	// Token: 0x17000EA5 RID: 3749
	// (get) Token: 0x0600216D RID: 8557 RVA: 0x00011D24 File Offset: 0x0000FF24
	// (set) Token: 0x0600216E RID: 8558 RVA: 0x00011D2C File Offset: 0x0000FF2C
	public int CornerCount { get; private set; }

	// Token: 0x17000EA6 RID: 3750
	// (get) Token: 0x0600216F RID: 8559 RVA: 0x00011D35 File Offset: 0x0000FF35
	public static List<Vector2> LeylinePoints
	{
		get
		{
			return LocalTeleporterController.m_leylineHelperList;
		}
	}

	// Token: 0x06002170 RID: 8560 RVA: 0x000A6E90 File Offset: 0x000A5090
	private void UpdateTeleporterColour()
	{
		if (this.m_teleporterSprite && this.m_teleportLocation)
		{
			this.m_teleporterSprite.color = this.m_teleporterColor;
			if (this.m_teleportLocation != null && this.m_teleportLocation.m_teleporterSprite.color != this.m_teleporterColor)
			{
				this.m_teleportLocation.m_teleporterSprite.color = this.m_teleporterColor;
				this.m_teleportLocation.m_teleporterColor = this.m_teleporterColor;
			}
			if (this.m_leyLines != null)
			{
				this.m_leyLines.vertexColor = this.m_teleporterColor;
				this.m_leyLines.CreateVertColors(true);
			}
		}
	}

	// Token: 0x06002171 RID: 8561 RVA: 0x00011D3C File Offset: 0x0000FF3C
	private void OnDisable()
	{
		if (this.m_interactable)
		{
			this.m_interactable.TriggerOnEnterEvent.RemoveListener(new UnityAction<GameObject>(this.OnInteract));
		}
		LocalTeleporterController.m_isTeleporting = false;
	}

	// Token: 0x06002172 RID: 8562 RVA: 0x00011D6D File Offset: 0x0000FF6D
	private void OnDestroy()
	{
		if (LocalTeleporterController.m_closestTeleporter_STATIC == this)
		{
			LocalTeleporterController.m_closestTeleporter_STATIC = null;
		}
	}

	// Token: 0x06002173 RID: 8563 RVA: 0x000A6F4C File Offset: 0x000A514C
	private void OnEnable()
	{
		if (this.m_interactable)
		{
			this.m_interactable.TriggerOnEnterEvent.AddListener(new UnityAction<GameObject>(this.OnInteract));
		}
		if (this.m_teleportLocation)
		{
			this.m_animator.SetBool("Open", true);
			return;
		}
		this.m_animator.SetBool("Open", false);
	}

	// Token: 0x06002174 RID: 8564 RVA: 0x000A6FB4 File Offset: 0x000A51B4
	private void Awake()
	{
		this.Room = base.GetComponent<Room>();
		this.m_interactable = base.GetComponent<Interactable>();
		this.m_animator = base.GetComponent<Animator>();
		if (this.m_teleportLocation != null)
		{
			this.m_animator.SetBool("Open", true);
		}
		else
		{
			this.m_animator.SetBool("Open", false);
			this.m_interactable.SetIsInteractableActive(false);
		}
		if (Application.isPlaying && this.TeleporterColor != Color.white)
		{
			this.TeleporterColor = Color.white;
		}
	}

	// Token: 0x06002175 RID: 8565 RVA: 0x000A7048 File Offset: 0x000A5248
	private void OnInteract(GameObject otherObj)
	{
		if (LocalTeleporterController.m_isTeleporting)
		{
			return;
		}
		if (this.m_teleportLocation)
		{
			if (!LocalTeleporterController.m_closestTeleporter_STATIC)
			{
				LocalTeleporterController.m_closestTeleporter_STATIC = this;
				base.StartCoroutine(this.StartTeleporterAtEndOfFrameCoroutine());
				return;
			}
			PlayerController playerController = PlayerManager.GetPlayerController();
			float num = CDGHelper.DistanceBetweenPts(LocalTeleporterController.m_closestTeleporter_STATIC.transform.position, playerController.transform.localPosition);
			if (CDGHelper.DistanceBetweenPts(base.transform.position, playerController.transform.localPosition) < num)
			{
				LocalTeleporterController.m_closestTeleporter_STATIC = this;
				return;
			}
		}
		else
		{
			Debug.Log("<color=red>Could not locally teleport player. Teleport location null.</color>");
		}
	}

	// Token: 0x06002176 RID: 8566 RVA: 0x00011D82 File Offset: 0x0000FF82
	private void StartTeleporterCoroutine()
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.TeleportPlayerCoroutine());
	}

	// Token: 0x06002177 RID: 8567 RVA: 0x00011D97 File Offset: 0x0000FF97
	private IEnumerator StartTeleporterAtEndOfFrameCoroutine()
	{
		if (LocalTeleporterController.m_closestTeleporter_STATIC)
		{
			yield return LocalTeleporterController.m_endOfFrameYield_STATIC;
			if (!PlayerManager.GetPlayerController().IsDead)
			{
				LocalTeleporterController.m_closestTeleporter_STATIC.StartTeleporterCoroutine();
			}
		}
		yield break;
	}

	// Token: 0x06002178 RID: 8568 RVA: 0x00011D9F File Offset: 0x0000FF9F
	public static void StopTeleportPlayer()
	{
		if (LocalTeleporterController.m_closestTeleporter_STATIC)
		{
			LocalTeleporterController.m_closestTeleporter_STATIC.StopTeleportPlayer_Internal();
		}
	}

	// Token: 0x06002179 RID: 8569 RVA: 0x000A70F4 File Offset: 0x000A52F4
	private void StopTeleportPlayer_Internal()
	{
		base.StopAllCoroutines();
		TweenManager.StopAllTweensContaining("LocalTeleporter", false);
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.Visuals.SetActive(true);
		playerController.ControllerCorgi.GravityActive(true);
		playerController.ControllerCorgi.CollisionsOn();
		playerController.HitboxController.SetHitboxActiveState(HitboxType.Body, true);
		playerController.HitboxController.SetHitboxActiveState(HitboxType.Terrain, true);
		this.m_spark.transform.SetParent(base.transform, false);
		this.m_spark.gameObject.SetActive(false);
		playerController.CharacterHitResponse.StopInvincibleTime();
		this.m_interactable.SetIsInteractableActive(true);
		LocalTeleporterController.m_closestTeleporter_STATIC = null;
		LocalTeleporterController.m_isTeleporting = false;
	}

	// Token: 0x0600217A RID: 8570 RVA: 0x00011DB7 File Offset: 0x0000FFB7
	private IEnumerator TeleportPlayerCoroutine()
	{
		if (this.OnEnterEvent != null)
		{
			this.OnEnterEvent.Invoke();
		}
		LocalTeleporterController.m_isTeleporting = true;
		this.m_interactable.SetIsInteractableActive(false);
		RumbleManager.StartRumble(true, true, 0.5f, 0.2f, true);
		RewiredMapController.SetCurrentMapEnabled(false);
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.Visuals.SetActive(false);
		playerController.StopActiveAbilities(false);
		playerController.ControllerCorgi.GravityActive(false);
		playerController.ControllerCorgi.CollisionsOff();
		playerController.SetVelocity(0f, 0f, false);
		playerController.CharacterHitResponse.SetInvincibleTime(999f, false, false);
		playerController.HitboxController.SetHitboxActiveState(HitboxType.Body, false);
		playerController.HitboxController.SetHitboxActiveState(HitboxType.Terrain, false);
		this.m_spark.gameObject.SetActive(true);
		this.m_spark.transform.SetParent(playerController.transform, false);
		if (this.m_leyLines)
		{
			LeylineController leylineController = this.m_leyLines.GetComponent<LeylineController>();
			LocalTeleporterController.m_leylineHelperList.Clear();
			LocalTeleporterController.m_leylineHelperList.AddRange(this.m_leyLines.PathData.GetFinalPath());
			this.CornerCount = LocalTeleporterController.m_leylineHelperList.Count - 2;
			bool reversed = false;
			if (LocalTeleporterController.m_leylineHelperList[0] + this.m_leyLines.transform.position != base.transform.position)
			{
				LocalTeleporterController.m_leylineHelperList.Reverse();
				reversed = true;
			}
			bool flag = LocalTeleporterController.m_leylineHelperList[0] + this.m_leyLines.transform.position == base.transform.position;
			bool flag2 = LocalTeleporterController.m_leylineHelperList[LocalTeleporterController.m_leylineHelperList.Count - 1] + this.m_leyLines.transform.position == this.m_teleportLocation.transform.position;
			if (!flag || !flag2)
			{
				if (!flag)
				{
					Debug.LogWarning("<color=yellow>Could not activate leyline animation.  No starting leyline attached to the entry teleporter.");
				}
				if (!flag2)
				{
					Debug.LogWarning("<color=yellow>Could not activate leyline animation.  No ending leyline attached to the exit teleporter.");
				}
				Vector3 position = this.m_teleportLocation.transform.position;
				position.y += playerController.Midpoint.y - playerController.transform.position.y;
				playerController.transform.position = position;
			}
			else
			{
				Tween tween = TweenManager.TweenTo(playerController.transform, 0.1f, new EaseDelegate(Ease.None), new object[]
				{
					"position.x",
					base.transform.position.x,
					"position.y",
					base.transform.position.y
				});
				tween.ID = "LocalTeleporter";
				yield return tween.TweenCoroutine;
				int rawPointIndex = (!reversed) ? 0 : (this.m_leyLines.PathData.Count - 1);
				int leypointCount = LocalTeleporterController.m_leylineHelperList.Count;
				int num2;
				for (int i = 0; i < leypointCount - 1; i = num2 + 1)
				{
					this.CurrentCornerIndex = i;
					Vector2 vector = LocalTeleporterController.m_leylineHelperList[i];
					Vector2 vector2 = LocalTeleporterController.m_leylineHelperList[i + 1];
					Vector2 vector3 = vector2 - vector;
					Vector3 v = this.m_leyLines.transform.TransformPoint(vector);
					Vector3 vector4 = this.m_leyLines.transform.TransformPoint(vector2);
					int smoothIndex = this.m_leyLines.PathData.GetSmoothIndex(rawPointIndex);
					int num = (!reversed) ? i : (leypointCount - 1 - i);
					bool flag3 = smoothIndex == num;
					if (flag3)
					{
						if (!reversed)
						{
							num2 = rawPointIndex;
							rawPointIndex = num2 + 1;
						}
						else
						{
							num2 = rawPointIndex;
							rawPointIndex = num2 - 1;
						}
					}
					if (flag3 && rawPointIndex >= 0 && rawPointIndex < this.m_leyLines.PathData.Count && this.OnHitLeyCornerEvent != null && i > 0 && i < leypointCount - 1)
					{
						this.OnHitLeyCornerEvent.Invoke(v);
					}
					EaseDelegate ease = new EaseDelegate(Ease.None);
					if (leypointCount > 2)
					{
						if (i == 0)
						{
							ease = new EaseDelegate(Ease.Cubic.EaseIn);
						}
						else if (i == leypointCount - 2)
						{
							ease = new EaseDelegate(Ease.Cubic.EaseOut);
						}
					}
					else
					{
						ease = new EaseDelegate(Ease.Cubic.EaseInOut);
					}
					float num3 = CDGHelper.DistanceBetweenPts(vector, vector2) / 40f;
					if (leypointCount > 2 && (i == 0 || i == leypointCount - 2))
					{
						num3 *= 2f;
					}
					float num4 = Time.deltaTime;
					num4 *= 2f;
					if (num3 < num4)
					{
						playerController.transform.localPosition = new Vector3(vector4.x, vector4.y, playerController.transform.localPosition.z);
						yield return null;
					}
					else
					{
						if (leylineController.UseCircleCorners)
						{
							tween = TweenManager.TweenBy(playerController.transform, num3, ease, new object[]
							{
								"subtract",
								Time.deltaTime,
								"localPosition.x",
								vector3.x,
								"localPosition.y",
								vector3.y
							});
						}
						else
						{
							tween = TweenManager.TweenBy(playerController.transform, num3, ease, new object[]
							{
								"localPosition.x",
								vector3.x,
								"localPosition.y",
								vector3.y
							});
						}
						tween.ID = "LocalTeleporter";
						yield return tween.TweenCoroutine;
					}
					num2 = i;
				}
				this.m_teleportLocation.Animator.SetTrigger("Expel");
				Vector3 position2 = this.m_teleportLocation.transform.position;
				position2.y -= playerController.Midpoint.y - playerController.transform.position.y;
				tween = TweenManager.TweenTo(playerController.transform, 0.1f, new EaseDelegate(Ease.None), new object[]
				{
					"position.x",
					position2.x,
					"position.y",
					position2.y
				});
				tween.ID = "LocalTeleporter";
				yield return tween.TweenCoroutine;
			}
			leylineController = null;
		}
		else
		{
			this.m_teleportLocation.Animator.SetTrigger("Expel");
			Vector3 position3 = this.m_teleportLocation.transform.position;
			position3.y += playerController.Midpoint.y - playerController.transform.position.y;
			playerController.transform.position = position3;
		}
		RewiredMapController.SetCurrentMapEnabled(true);
		playerController.Visuals.SetActive(true);
		playerController.ControllerCorgi.GravityActive(true);
		playerController.ControllerCorgi.CollisionsOn();
		playerController.HitboxController.SetHitboxActiveState(HitboxType.Body, true);
		playerController.HitboxController.SetHitboxActiveState(HitboxType.Terrain, true);
		this.m_spark.transform.SetParent(base.transform, false);
		this.m_spark.gameObject.SetActive(false);
		RumbleManager.StartRumble(true, true, 0.5f, 0.2f, true);
		if (this.OnExitEvent != null)
		{
			this.OnExitEvent.Invoke();
		}
		playerController.CharacterHitResponse.SetInvincibleTime(0.2f, false, false);
		this.m_interactable.SetIsInteractableActive(true);
		LocalTeleporterController.m_closestTeleporter_STATIC = null;
		LocalTeleporterController.m_isTeleporting = false;
		yield break;
	}

	// Token: 0x0600217D RID: 8573 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04001E48 RID: 7752
	private const float TELEPORTER_SPEED = 40f;

	// Token: 0x04001E49 RID: 7753
	private static LocalTeleporterController m_closestTeleporter_STATIC;

	// Token: 0x04001E4A RID: 7754
	private static WaitForEndOfFrame m_endOfFrameYield_STATIC = new WaitForEndOfFrame();

	// Token: 0x04001E4B RID: 7755
	[SerializeField]
	private LocalTeleporterController m_teleportLocation;

	// Token: 0x04001E4C RID: 7756
	[SerializeField]
	private Ferr2DT_PathTerrain m_leyLines;

	// Token: 0x04001E4D RID: 7757
	[SerializeField]
	private SpriteRenderer m_teleporterSprite;

	// Token: 0x04001E4E RID: 7758
	[SerializeField]
	private Color m_teleporterColor = Color.white;

	// Token: 0x04001E4F RID: 7759
	[SerializeField]
	public UnityEvent_Vector2 OnHitLeyCornerEvent;

	// Token: 0x04001E50 RID: 7760
	[SerializeField]
	public UnityEvent OnEnterEvent;

	// Token: 0x04001E51 RID: 7761
	[SerializeField]
	public UnityEvent OnExitEvent;

	// Token: 0x04001E52 RID: 7762
	[SerializeField]
	private GameObject m_spark;

	// Token: 0x04001E53 RID: 7763
	private Interactable m_interactable;

	// Token: 0x04001E54 RID: 7764
	private Animator m_animator;

	// Token: 0x04001E55 RID: 7765
	private static bool m_isTeleporting;

	// Token: 0x04001E59 RID: 7769
	private static List<Vector2> m_leylineHelperList = new List<Vector2>();
}
