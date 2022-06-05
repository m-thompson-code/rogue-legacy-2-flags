using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A55 RID: 2645
public class Prop : MonoBehaviour, IGenericPoolObj, IMirror, IPivotObj
{
	// Token: 0x17001B6F RID: 7023
	// (get) Token: 0x06004FB6 RID: 20406 RVA: 0x0002B815 File Offset: 0x00029A15
	// (set) Token: 0x06004FB7 RID: 20407 RVA: 0x0002B81D File Offset: 0x00029A1D
	public bool HitboxesDisabled { get; set; }

	// Token: 0x17001B70 RID: 7024
	// (get) Token: 0x06004FB8 RID: 20408 RVA: 0x0002B826 File Offset: 0x00029A26
	// (set) Token: 0x06004FB9 RID: 20409 RVA: 0x0002B82E File Offset: 0x00029A2E
	public bool IsCulled { get; set; }

	// Token: 0x17001B71 RID: 7025
	// (get) Token: 0x06004FBA RID: 20410 RVA: 0x0002B837 File Offset: 0x00029A37
	// (set) Token: 0x06004FBB RID: 20411 RVA: 0x0002B83F File Offset: 0x00029A3F
	public IRoomConsumer[] RoomConsumers { get; private set; }

	// Token: 0x17001B72 RID: 7026
	// (get) Token: 0x06004FBC RID: 20412 RVA: 0x0002B848 File Offset: 0x00029A48
	// (set) Token: 0x06004FBD RID: 20413 RVA: 0x0002B850 File Offset: 0x00029A50
	public bool IsFreePoolObj { get; set; }

	// Token: 0x17001B73 RID: 7027
	// (get) Token: 0x06004FBE RID: 20414 RVA: 0x0002B859 File Offset: 0x00029A59
	// (set) Token: 0x06004FBF RID: 20415 RVA: 0x0002B861 File Offset: 0x00029A61
	public bool IsAwakeCalled { get; protected set; }

	// Token: 0x17001B74 RID: 7028
	// (get) Token: 0x06004FC0 RID: 20416 RVA: 0x0002B86A File Offset: 0x00029A6A
	// (set) Token: 0x06004FC1 RID: 20417 RVA: 0x0002B872 File Offset: 0x00029A72
	public Light[] Lights { get; private set; }

	// Token: 0x17001B75 RID: 7029
	// (get) Token: 0x06004FC2 RID: 20418 RVA: 0x0002B87B File Offset: 0x00029A7B
	public CorgiController_RL CorgiController
	{
		get
		{
			return this.m_corgiController;
		}
	}

	// Token: 0x17001B76 RID: 7030
	// (get) Token: 0x06004FC3 RID: 20419 RVA: 0x0002B883 File Offset: 0x00029A83
	// (set) Token: 0x06004FC4 RID: 20420 RVA: 0x0002B88B File Offset: 0x00029A8B
	public Breakable Breakable { get; private set; }

	// Token: 0x17001B77 RID: 7031
	// (get) Token: 0x06004FC5 RID: 20421 RVA: 0x0002B894 File Offset: 0x00029A94
	// (set) Token: 0x06004FC6 RID: 20422 RVA: 0x0002B89C File Offset: 0x00029A9C
	public BaseRoom Room { get; private set; }

	// Token: 0x17001B78 RID: 7032
	// (get) Token: 0x06004FC7 RID: 20423 RVA: 0x0002B8A5 File Offset: 0x00029AA5
	// (set) Token: 0x06004FC8 RID: 20424 RVA: 0x0002B8AD File Offset: 0x00029AAD
	public DecoController[] DecoControllers { get; private set; }

	// Token: 0x17001B79 RID: 7033
	// (get) Token: 0x06004FC9 RID: 20425 RVA: 0x0002B8B6 File Offset: 0x00029AB6
	// (set) Token: 0x06004FCA RID: 20426 RVA: 0x0002B8BE File Offset: 0x00029ABE
	public PropSpawnController PropSpawnController { get; private set; }

	// Token: 0x17001B7A RID: 7034
	// (get) Token: 0x06004FCB RID: 20427 RVA: 0x0002B8C7 File Offset: 0x00029AC7
	// (set) Token: 0x06004FCC RID: 20428 RVA: 0x0002B8CF File Offset: 0x00029ACF
	public ICameraLayerController CameraLayerController { get; private set; }

	// Token: 0x17001B7B RID: 7035
	// (get) Token: 0x06004FCD RID: 20429 RVA: 0x0002B8D8 File Offset: 0x00029AD8
	// (set) Token: 0x06004FCE RID: 20430 RVA: 0x0002B8E0 File Offset: 0x00029AE0
	public IHitboxController HitboxController { get; private set; }

	// Token: 0x17001B7C RID: 7036
	// (get) Token: 0x06004FCF RID: 20431 RVA: 0x0002B8E9 File Offset: 0x00029AE9
	public GameObject Pivot
	{
		get
		{
			return this.m_pivot;
		}
	}

	// Token: 0x17001B7D RID: 7037
	// (get) Token: 0x06004FD0 RID: 20432 RVA: 0x0002B8F1 File Offset: 0x00029AF1
	// (set) Token: 0x06004FD1 RID: 20433 RVA: 0x0002B8F9 File Offset: 0x00029AF9
	public Animator[] Animators { get; private set; }

	// Token: 0x17001B7E RID: 7038
	// (get) Token: 0x06004FD2 RID: 20434 RVA: 0x0002B902 File Offset: 0x00029B02
	// (set) Token: 0x06004FD3 RID: 20435 RVA: 0x0002B90A File Offset: 0x00029B0A
	public bool IsInitialized { get; private set; }

	// Token: 0x06004FD4 RID: 20436 RVA: 0x00130B74 File Offset: 0x0012ED74
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

	// Token: 0x06004FD5 RID: 20437 RVA: 0x0002B913 File Offset: 0x00029B13
	private void OnValidate()
	{
		if (base.gameObject.tag != "Prop")
		{
			base.gameObject.tag = "Prop";
		}
	}

	// Token: 0x06004FD6 RID: 20438 RVA: 0x00130CA8 File Offset: 0x0012EEA8
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

	// Token: 0x06004FD7 RID: 20439 RVA: 0x0002B93C File Offset: 0x00029B3C
	protected virtual void ResizeSlicedOrTiledSpriteRenderer(SpriteRenderer spriteRenderer, Vector2 newSize)
	{
		spriteRenderer.size = newSize;
	}

	// Token: 0x06004FD8 RID: 20440 RVA: 0x0013101C File Offset: 0x0012F21C
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

	// Token: 0x06004FD9 RID: 20441 RVA: 0x001310C0 File Offset: 0x0012F2C0
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

	// Token: 0x06004FDA RID: 20442 RVA: 0x0002B945 File Offset: 0x00029B45
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

	// Token: 0x06004FDB RID: 20443 RVA: 0x001311C4 File Offset: 0x0012F3C4
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

	// Token: 0x06004FDC RID: 20444 RVA: 0x0002B954 File Offset: 0x00029B54
	public void SetRoom(BaseRoom value)
	{
		this.Room = value;
	}

	// Token: 0x06004FDD RID: 20445 RVA: 0x00131258 File Offset: 0x0012F458
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

	// Token: 0x06004FDE RID: 20446 RVA: 0x0002B95D File Offset: 0x00029B5D
	public void ResetInitialization()
	{
		this.IsInitialized = false;
	}

	// Token: 0x06004FDF RID: 20447 RVA: 0x0002B966 File Offset: 0x00029B66
	private void OnDisable()
	{
		if (this.m_animator)
		{
			this.m_animator.WriteDefaultValues();
		}
		this.IsFreePoolObj = true;
	}

	// Token: 0x06004FE1 RID: 20449 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04003C70 RID: 15472
	[SerializeField]
	private bool m_verticalTuck;

	// Token: 0x04003C71 RID: 15473
	[SerializeField]
	private bool m_flipWhenMirrored = true;

	// Token: 0x04003C72 RID: 15474
	[SerializeField]
	private bool m_maintainUniformScale;

	// Token: 0x04003C73 RID: 15475
	[SerializeField]
	private GameObject m_pivot;

	// Token: 0x04003C74 RID: 15476
	private CorgiController_RL m_corgiController;

	// Token: 0x04003C75 RID: 15477
	private Renderer[] m_renderers;

	// Token: 0x04003C76 RID: 15478
	private PointLightController[] m_lightControllers;

	// Token: 0x04003C77 RID: 15479
	private bool m_isMirrored;

	// Token: 0x04003C78 RID: 15480
	private Animator m_animator;

	// Token: 0x04003C79 RID: 15481
	private Material[] m_materials;

	// Token: 0x04003C7A RID: 15482
	private bool m_hasNPCController;

	// Token: 0x04003C7B RID: 15483
	private bool m_hasForegroundSwapper;

	// Token: 0x04003C7C RID: 15484
	private bool m_isOnForeground;
}
