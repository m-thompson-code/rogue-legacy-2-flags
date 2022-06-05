using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000629 RID: 1577
public class Prop : MonoBehaviour, IGenericPoolObj, IMirror, IPivotObj
{
	// Token: 0x1700140A RID: 5130
	// (get) Token: 0x060038DD RID: 14557 RVA: 0x000C2018 File Offset: 0x000C0218
	// (set) Token: 0x060038DE RID: 14558 RVA: 0x000C2020 File Offset: 0x000C0220
	public bool HitboxesDisabled { get; set; }

	// Token: 0x1700140B RID: 5131
	// (get) Token: 0x060038DF RID: 14559 RVA: 0x000C2029 File Offset: 0x000C0229
	// (set) Token: 0x060038E0 RID: 14560 RVA: 0x000C2031 File Offset: 0x000C0231
	public bool IsCulled { get; set; }

	// Token: 0x1700140C RID: 5132
	// (get) Token: 0x060038E1 RID: 14561 RVA: 0x000C203A File Offset: 0x000C023A
	// (set) Token: 0x060038E2 RID: 14562 RVA: 0x000C2042 File Offset: 0x000C0242
	public IRoomConsumer[] RoomConsumers { get; private set; }

	// Token: 0x1700140D RID: 5133
	// (get) Token: 0x060038E3 RID: 14563 RVA: 0x000C204B File Offset: 0x000C024B
	// (set) Token: 0x060038E4 RID: 14564 RVA: 0x000C2053 File Offset: 0x000C0253
	public bool IsFreePoolObj { get; set; }

	// Token: 0x1700140E RID: 5134
	// (get) Token: 0x060038E5 RID: 14565 RVA: 0x000C205C File Offset: 0x000C025C
	// (set) Token: 0x060038E6 RID: 14566 RVA: 0x000C2064 File Offset: 0x000C0264
	public bool IsAwakeCalled { get; protected set; }

	// Token: 0x1700140F RID: 5135
	// (get) Token: 0x060038E7 RID: 14567 RVA: 0x000C206D File Offset: 0x000C026D
	// (set) Token: 0x060038E8 RID: 14568 RVA: 0x000C2075 File Offset: 0x000C0275
	public Light[] Lights { get; private set; }

	// Token: 0x17001410 RID: 5136
	// (get) Token: 0x060038E9 RID: 14569 RVA: 0x000C207E File Offset: 0x000C027E
	public CorgiController_RL CorgiController
	{
		get
		{
			return this.m_corgiController;
		}
	}

	// Token: 0x17001411 RID: 5137
	// (get) Token: 0x060038EA RID: 14570 RVA: 0x000C2086 File Offset: 0x000C0286
	// (set) Token: 0x060038EB RID: 14571 RVA: 0x000C208E File Offset: 0x000C028E
	public Breakable Breakable { get; private set; }

	// Token: 0x17001412 RID: 5138
	// (get) Token: 0x060038EC RID: 14572 RVA: 0x000C2097 File Offset: 0x000C0297
	// (set) Token: 0x060038ED RID: 14573 RVA: 0x000C209F File Offset: 0x000C029F
	public BaseRoom Room { get; private set; }

	// Token: 0x17001413 RID: 5139
	// (get) Token: 0x060038EE RID: 14574 RVA: 0x000C20A8 File Offset: 0x000C02A8
	// (set) Token: 0x060038EF RID: 14575 RVA: 0x000C20B0 File Offset: 0x000C02B0
	public DecoController[] DecoControllers { get; private set; }

	// Token: 0x17001414 RID: 5140
	// (get) Token: 0x060038F0 RID: 14576 RVA: 0x000C20B9 File Offset: 0x000C02B9
	// (set) Token: 0x060038F1 RID: 14577 RVA: 0x000C20C1 File Offset: 0x000C02C1
	public PropSpawnController PropSpawnController { get; private set; }

	// Token: 0x17001415 RID: 5141
	// (get) Token: 0x060038F2 RID: 14578 RVA: 0x000C20CA File Offset: 0x000C02CA
	// (set) Token: 0x060038F3 RID: 14579 RVA: 0x000C20D2 File Offset: 0x000C02D2
	public ICameraLayerController CameraLayerController { get; private set; }

	// Token: 0x17001416 RID: 5142
	// (get) Token: 0x060038F4 RID: 14580 RVA: 0x000C20DB File Offset: 0x000C02DB
	// (set) Token: 0x060038F5 RID: 14581 RVA: 0x000C20E3 File Offset: 0x000C02E3
	public IHitboxController HitboxController { get; private set; }

	// Token: 0x17001417 RID: 5143
	// (get) Token: 0x060038F6 RID: 14582 RVA: 0x000C20EC File Offset: 0x000C02EC
	public GameObject Pivot
	{
		get
		{
			return this.m_pivot;
		}
	}

	// Token: 0x17001418 RID: 5144
	// (get) Token: 0x060038F7 RID: 14583 RVA: 0x000C20F4 File Offset: 0x000C02F4
	// (set) Token: 0x060038F8 RID: 14584 RVA: 0x000C20FC File Offset: 0x000C02FC
	public Animator[] Animators { get; private set; }

	// Token: 0x17001419 RID: 5145
	// (get) Token: 0x060038F9 RID: 14585 RVA: 0x000C2105 File Offset: 0x000C0305
	// (set) Token: 0x060038FA RID: 14586 RVA: 0x000C210D File Offset: 0x000C030D
	public bool IsInitialized { get; private set; }

	// Token: 0x060038FB RID: 14587 RVA: 0x000C2118 File Offset: 0x000C0318
	private void Awake()
	{
		this.m_renderers = base.GetComponentsInChildren<Renderer>(true);
		this.m_materials = new Material[this.m_renderers.Length];
		for (int i = 0; i < this.m_materials.Length; i++)
		{
			this.m_materials[i] = this.m_renderers[i].sharedMaterial;
		}
		this.Breakable = base.GetComponent<Breakable>();
		this.CameraLayerController = base.GetComponent<ICameraLayerController>();
		this.DecoControllers = base.GetComponents<DecoController>();
		this.m_animator = base.GetComponent<Animator>();
		this.m_lightControllers = base.GetComponentsInChildren<PointLightController>();
		this.Lights = base.GetComponentsInChildren<Light>();
		this.m_corgiController = base.GetComponent<CorgiController_RL>();
		if (this.m_corgiController)
		{
			this.m_corgiController.DisableYSlopeOffsetCheck = true;
			this.m_corgiController.DisableCastRaysAbove = true;
			this.m_corgiController.NumberOfHorizontalRays = 0;
			this.m_corgiController.PermanentlyDisableUponTouchingPlatform = true;
		}
		this.Animators = base.GetComponentsInChildren<Animator>();
		this.HitboxController = base.GetComponentInChildren<IHitboxController>();
		this.RoomConsumers = base.GetComponentsInChildren<IRoomConsumer>();
		this.m_hasForegroundSwapper = base.GetComponent<ForegroundPropMeshSpriteSwapper>();
		this.m_hasNPCController = base.GetComponent<NPCController>();
		this.IsAwakeCalled = true;
	}

	// Token: 0x060038FC RID: 14588 RVA: 0x000C224B File Offset: 0x000C044B
	private void OnValidate()
	{
		if (base.gameObject.tag != "Prop")
		{
			base.gameObject.tag = "Prop";
		}
	}

	// Token: 0x060038FD RID: 14589 RVA: 0x000C2274 File Offset: 0x000C0474
	public virtual void Initialize(BaseRoom room, PropSpawnController propSpawnController)
	{
		this.SetRoom(room);
		this.PropSpawnController = propSpawnController;
		if (this.m_hasForegroundSwapper)
		{
			ForegroundPropMeshSpriteSwapper component = base.GetComponent<ForegroundPropMeshSpriteSwapper>();
			if (this.PropSpawnController.CameraLayer == CameraLayer.Foreground_PERSP)
			{
				component.MeshRenderer.gameObject.SetActive(false);
				component.SpriteRenderer.gameObject.SetActive(true);
			}
			else
			{
				component.MeshRenderer.gameObject.SetActive(true);
				component.SpriteRenderer.gameObject.SetActive(false);
			}
		}
		bool flag = propSpawnController.CameraLayer == CameraLayer.Foreground_PERSP && !this.m_hasNPCController;
		if (this.m_isOnForeground || flag)
		{
			this.m_isOnForeground = flag;
			BiomeArtData biomeArtData = this.Room.BiomeArtDataOverride;
			if (!biomeArtData)
			{
				biomeArtData = BiomeArtDataLibrary.GetArtData(this.Room.AppearanceBiomeType);
			}
			int num = this.m_renderers.Length;
			for (int i = 0; i < num; i++)
			{
				SpriteRenderer spriteRenderer = this.m_renderers[i] as SpriteRenderer;
				if (spriteRenderer)
				{
					if (this.m_isOnForeground)
					{
						spriteRenderer.sharedMaterial = biomeArtData.ForegroundData.ForegroundSpriteMaterial;
					}
					else
					{
						spriteRenderer.sharedMaterial = this.m_materials[i];
					}
				}
			}
		}
		SpriteDrawMode spriteRendererDrawMode = propSpawnController.SpriteRendererDrawMode;
		Vector2 spriteRendererSize = propSpawnController.SpriteRendererSize;
		Vector3 position = propSpawnController.gameObject.transform.position;
		float num2 = position.y;
		if (spriteRendererDrawMode == SpriteDrawMode.Sliced || spriteRendererDrawMode == SpriteDrawMode.Tiled)
		{
			float num3 = spriteRendererSize.y;
			float x = spriteRendererSize.x;
			if (this.m_verticalTuck && propSpawnController.CameraLayer == CameraLayer.Background_Near_PERSP)
			{
				float tuckYOffset = CameraLayerUtility.GetTuckYOffset(propSpawnController.CameraLayer, propSpawnController.SubLayer);
				num2 += tuckYOffset;
				num3 += 2f * Mathf.Abs(tuckYOffset);
			}
			for (int j = 0; j < this.m_renderers.Length; j++)
			{
				SpriteRenderer spriteRenderer2 = this.m_renderers[j] as SpriteRenderer;
				if (spriteRenderer2 && (spriteRenderer2.drawMode == SpriteDrawMode.Sliced || spriteRenderer2.drawMode == SpriteDrawMode.Tiled))
				{
					this.ResizeSlicedOrTiledSpriteRenderer(spriteRenderer2, new Vector2(x, num3));
				}
			}
		}
		base.transform.position = new Vector3(position.x, num2, 0f);
		if (this.m_corgiController && this.Pivot)
		{
			this.Pivot.transform.localScale = propSpawnController.gameObject.transform.localScale;
			this.Pivot.transform.rotation = propSpawnController.gameObject.transform.rotation;
			if (this.m_maintainUniformScale)
			{
				this.MaintainUniformScale(this.Pivot.transform);
			}
		}
		else
		{
			base.transform.rotation = propSpawnController.gameObject.transform.rotation;
			base.transform.localScale = propSpawnController.gameObject.transform.localScale;
			if (this.m_maintainUniformScale)
			{
				this.MaintainUniformScale(base.transform);
			}
		}
		if (propSpawnController.IsMirrored)
		{
			this.Mirror();
		}
		if (!this.CameraLayerController.IsNativeNull())
		{
			this.CameraLayerController.SetCameraLayer(propSpawnController.CameraLayer);
			this.CameraLayerController.SetSubLayer(propSpawnController.ActualSubLayer, false);
		}
		for (int k = 0; k < this.m_lightControllers.Length; k++)
		{
			this.m_lightControllers[k].UpdateLocation(propSpawnController.CameraLayer);
		}
		if (this.m_corgiController)
		{
			base.StartCoroutine(this.ResetCorgiControllerRayParameters(position));
		}
		this.IsInitialized = true;
	}

	// Token: 0x060038FE RID: 14590 RVA: 0x000C25E7 File Offset: 0x000C07E7
	protected virtual void ResizeSlicedOrTiledSpriteRenderer(SpriteRenderer spriteRenderer, Vector2 newSize)
	{
		spriteRenderer.size = newSize;
	}

	// Token: 0x060038FF RID: 14591 RVA: 0x000C25F0 File Offset: 0x000C07F0
	private void MaintainUniformScale(Transform propTransform)
	{
		Vector3 localScale = propTransform.localScale;
		float num = Mathf.Abs(localScale.x);
		float num2 = Mathf.Abs(localScale.y);
		if (num > num2)
		{
			bool flag = localScale.y < 0f;
			localScale.y = num;
			if (flag && localScale.y > 0f)
			{
				localScale.y *= -1f;
			}
		}
		else
		{
			bool flag2 = localScale.x < 0f;
			localScale.x = num2;
			if (flag2 && localScale.x > 0f)
			{
				localScale.x *= -1f;
			}
		}
		propTransform.localScale = localScale;
	}

	// Token: 0x06003900 RID: 14592 RVA: 0x000C2694 File Offset: 0x000C0894
	private void PerformRaycastReposition(Vector3 spawnPos)
	{
		Vector2 vector = spawnPos;
		float num = this.m_corgiController.Width() / 2f;
		vector.y += 0.1f;
		RaycastHit2D hit = Physics2D.Raycast(vector, Vector2.left, num, this.m_corgiController.PlatformMask);
		if (hit)
		{
			float num2 = hit.point.x - (vector.x - num) + 0.01f;
			base.transform.position = new Vector3(spawnPos.x + num2, spawnPos.y, spawnPos.z);
			return;
		}
		hit = Physics2D.Raycast(vector, Vector2.right, num, this.m_corgiController.PlatformMask);
		if (hit)
		{
			float num3 = vector.x + num - hit.point.x + 0.01f;
			base.transform.position = new Vector3(spawnPos.x - num3, spawnPos.y, spawnPos.z);
			return;
		}
	}

	// Token: 0x06003901 RID: 14593 RVA: 0x000C2798 File Offset: 0x000C0998
	private IEnumerator ResetCorgiControllerRayParameters(Vector3 spawnPos)
	{
		while (!this.m_corgiController.IsInitialized)
		{
			yield return null;
		}
		this.m_corgiController.ResetState();
		this.m_corgiController.SetRaysParameters();
		this.m_corgiController.SetHorizontalForce(0f);
		yield break;
	}

	// Token: 0x06003902 RID: 14594 RVA: 0x000C27A8 File Offset: 0x000C09A8
	public virtual void Mirror()
	{
		this.m_isMirrored = true;
		if (this.m_flipWhenMirrored)
		{
			Vector3 localScale = base.transform.localScale;
			localScale.x *= -1f;
			base.transform.localScale = localScale;
		}
		float num = base.transform.rotation.eulerAngles.z;
		if (num < -0.1f || num > 0.1f)
		{
			num *= -1f;
			Quaternion rotation = Quaternion.Euler(0f, 0f, num);
			base.transform.rotation = rotation;
		}
	}

	// Token: 0x06003903 RID: 14595 RVA: 0x000C283A File Offset: 0x000C0A3A
	public void SetRoom(BaseRoom value)
	{
		this.Room = value;
	}

	// Token: 0x06003904 RID: 14596 RVA: 0x000C2844 File Offset: 0x000C0A44
	public void ResetValues()
	{
		if (this.m_corgiController)
		{
			this.m_corgiController.ResetPermanentDisable();
		}
		if (this.m_isMirrored)
		{
			this.m_isMirrored = false;
			if (this.m_flipWhenMirrored)
			{
				base.transform.localScale = new Vector3(-1f * base.transform.localScale.x, base.transform.localScale.y, base.transform.localScale.z);
			}
			float num = base.transform.rotation.eulerAngles.z;
			if (num < -0.1f || num > 0.1f)
			{
				num *= -1f;
				Quaternion rotation = Quaternion.Euler(0f, 0f, num);
				base.transform.rotation = rotation;
			}
		}
		if (this.Breakable)
		{
			this.Breakable.ForceBrokenState(false);
		}
	}

	// Token: 0x06003905 RID: 14597 RVA: 0x000C2930 File Offset: 0x000C0B30
	public void ResetInitialization()
	{
		this.IsInitialized = false;
	}

	// Token: 0x06003906 RID: 14598 RVA: 0x000C2939 File Offset: 0x000C0B39
	private void OnDisable()
	{
		if (this.m_animator)
		{
			this.m_animator.WriteDefaultValues();
		}
		this.IsFreePoolObj = true;
	}

	// Token: 0x06003908 RID: 14600 RVA: 0x000C2969 File Offset: 0x000C0B69
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002BE1 RID: 11233
	[SerializeField]
	private bool m_verticalTuck;

	// Token: 0x04002BE2 RID: 11234
	[SerializeField]
	private bool m_flipWhenMirrored = true;

	// Token: 0x04002BE3 RID: 11235
	[SerializeField]
	private bool m_maintainUniformScale;

	// Token: 0x04002BE4 RID: 11236
	[SerializeField]
	private GameObject m_pivot;

	// Token: 0x04002BE5 RID: 11237
	private CorgiController_RL m_corgiController;

	// Token: 0x04002BE6 RID: 11238
	private Renderer[] m_renderers;

	// Token: 0x04002BE7 RID: 11239
	private PointLightController[] m_lightControllers;

	// Token: 0x04002BE8 RID: 11240
	private bool m_isMirrored;

	// Token: 0x04002BE9 RID: 11241
	private Animator m_animator;

	// Token: 0x04002BEA RID: 11242
	private Material[] m_materials;

	// Token: 0x04002BEB RID: 11243
	private bool m_hasNPCController;

	// Token: 0x04002BEC RID: 11244
	private bool m_hasForegroundSwapper;

	// Token: 0x04002BED RID: 11245
	private bool m_isOnForeground;
}
