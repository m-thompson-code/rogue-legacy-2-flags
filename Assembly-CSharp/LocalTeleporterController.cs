using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200025C RID: 604
[SelectionBase]
public class LocalTeleporterController : MonoBehaviour, IRootObj
{
	// Token: 0x17000B6F RID: 2927
	// (get) Token: 0x060017AA RID: 6058 RVA: 0x00049813 File Offset: 0x00047A13
	public static bool IsTeleporting
	{
		get
		{
			return LocalTeleporterController.m_isTeleporting;
		}
	}

	// Token: 0x17000B70 RID: 2928
	// (get) Token: 0x060017AB RID: 6059 RVA: 0x0004981A File Offset: 0x00047A1A
	// (set) Token: 0x060017AC RID: 6060 RVA: 0x00049822 File Offset: 0x00047A22
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

	// Token: 0x17000B71 RID: 2929
	// (get) Token: 0x060017AD RID: 6061 RVA: 0x0004983F File Offset: 0x00047A3F
	public LocalTeleporterController TeleporterLocation
	{
		get
		{
			return this.m_teleportLocation;
		}
	}

	// Token: 0x17000B72 RID: 2930
	// (get) Token: 0x060017AE RID: 6062 RVA: 0x00049847 File Offset: 0x00047A47
	public Animator Animator
	{
		get
		{
			return this.m_animator;
		}
	}

	// Token: 0x17000B73 RID: 2931
	// (get) Token: 0x060017AF RID: 6063 RVA: 0x0004984F File Offset: 0x00047A4F
	// (set) Token: 0x060017B0 RID: 6064 RVA: 0x00049857 File Offset: 0x00047A57
	public Room Room { get; set; }

	// Token: 0x17000B74 RID: 2932
	// (get) Token: 0x060017B1 RID: 6065 RVA: 0x00049860 File Offset: 0x00047A60
	public Ferr2DT_PathTerrain LeyLines
	{
		get
		{
			return this.m_leyLines;
		}
	}

	// Token: 0x17000B75 RID: 2933
	// (get) Token: 0x060017B2 RID: 6066 RVA: 0x00049868 File Offset: 0x00047A68
	// (set) Token: 0x060017B3 RID: 6067 RVA: 0x00049870 File Offset: 0x00047A70
	public int CurrentCornerIndex { get; private set; }

	// Token: 0x17000B76 RID: 2934
	// (get) Token: 0x060017B4 RID: 6068 RVA: 0x00049879 File Offset: 0x00047A79
	// (set) Token: 0x060017B5 RID: 6069 RVA: 0x00049881 File Offset: 0x00047A81
	public int CornerCount { get; private set; }

	// Token: 0x17000B77 RID: 2935
	// (get) Token: 0x060017B6 RID: 6070 RVA: 0x0004988A File Offset: 0x00047A8A
	public static List<Vector2> LeylinePoints
	{
		get
		{
			return LocalTeleporterController.m_leylineHelperList;
		}
	}

	// Token: 0x060017B7 RID: 6071 RVA: 0x00049894 File Offset: 0x00047A94
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

	// Token: 0x060017B8 RID: 6072 RVA: 0x0004994F File Offset: 0x00047B4F
	private void OnDisable()
	{
		if (this.m_interactable)
		{
			this.m_interactable.TriggerOnEnterEvent.RemoveListener(new UnityAction<GameObject>(this.OnInteract));
		}
		LocalTeleporterController.m_isTeleporting = false;
	}

	// Token: 0x060017B9 RID: 6073 RVA: 0x00049980 File Offset: 0x00047B80
	private void OnDestroy()
	{
		if (LocalTeleporterController.m_closestTeleporter_STATIC == this)
		{
			LocalTeleporterController.m_closestTeleporter_STATIC = null;
		}
	}

	// Token: 0x060017BA RID: 6074 RVA: 0x00049998 File Offset: 0x00047B98
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

	// Token: 0x060017BB RID: 6075 RVA: 0x00049A00 File Offset: 0x00047C00
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

	// Token: 0x060017BC RID: 6076 RVA: 0x00049A94 File Offset: 0x00047C94
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

	// Token: 0x060017BD RID: 6077 RVA: 0x00049B3F File Offset: 0x00047D3F
	private void StartTeleporterCoroutine()
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.TeleportPlayerCoroutine());
	}

	// Token: 0x060017BE RID: 6078 RVA: 0x00049B54 File Offset: 0x00047D54
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

	// Token: 0x060017BF RID: 6079 RVA: 0x00049B5C File Offset: 0x00047D5C
	public static void StopTeleportPlayer()
	{
		if (LocalTeleporterController.m_closestTeleporter_STATIC)
		{
			LocalTeleporterController.m_closestTeleporter_STATIC.StopTeleportPlayer_Internal();
		}
	}

	// Token: 0x060017C0 RID: 6080 RVA: 0x00049B74 File Offset: 0x00047D74
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

	// Token: 0x060017C1 RID: 6081 RVA: 0x00049C1E File Offset: 0x00047E1E
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

	// Token: 0x060017C4 RID: 6084 RVA: 0x00049C56 File Offset: 0x00047E56
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400172C RID: 5932
	private const float TELEPORTER_SPEED = 40f;

	// Token: 0x0400172D RID: 5933
	private static LocalTeleporterController m_closestTeleporter_STATIC;

	// Token: 0x0400172E RID: 5934
	private static WaitForEndOfFrame m_endOfFrameYield_STATIC = new WaitForEndOfFrame();

	// Token: 0x0400172F RID: 5935
	[SerializeField]
	private LocalTeleporterController m_teleportLocation;

	// Token: 0x04001730 RID: 5936
	[SerializeField]
	private Ferr2DT_PathTerrain m_leyLines;

	// Token: 0x04001731 RID: 5937
	[SerializeField]
	private SpriteRenderer m_teleporterSprite;

	// Token: 0x04001732 RID: 5938
	[SerializeField]
	private Color m_teleporterColor = Color.white;

	// Token: 0x04001733 RID: 5939
	[SerializeField]
	public UnityEvent_Vector2 OnHitLeyCornerEvent;

	// Token: 0x04001734 RID: 5940
	[SerializeField]
	public UnityEvent OnEnterEvent;

	// Token: 0x04001735 RID: 5941
	[SerializeField]
	public UnityEvent OnExitEvent;

	// Token: 0x04001736 RID: 5942
	[SerializeField]
	private GameObject m_spark;

	// Token: 0x04001737 RID: 5943
	private Interactable m_interactable;

	// Token: 0x04001738 RID: 5944
	private Animator m_animator;

	// Token: 0x04001739 RID: 5945
	private static bool m_isTeleporting;

	// Token: 0x0400173D RID: 5949
	private static List<Vector2> m_leylineHelperList = new List<Vector2>();
}
