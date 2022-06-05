using System;
using System.Linq;
using UnityEngine;

// Token: 0x020005FC RID: 1532
public class CameraLayerController : MonoBehaviour, ICameraLayerController, IRootObj
{
	// Token: 0x170013A3 RID: 5027
	// (get) Token: 0x0600376C RID: 14188 RVA: 0x000BDF83 File Offset: 0x000BC183
	public bool IsProp
	{
		get
		{
			return this.m_prop;
		}
	}

	// Token: 0x170013A4 RID: 5028
	// (get) Token: 0x0600376D RID: 14189 RVA: 0x000BDF90 File Offset: 0x000BC190
	public bool IsSet
	{
		get
		{
			return this.m_isSet;
		}
	}

	// Token: 0x170013A5 RID: 5029
	// (get) Token: 0x0600376E RID: 14190 RVA: 0x000BDF98 File Offset: 0x000BC198
	// (set) Token: 0x0600376F RID: 14191 RVA: 0x000BDFA0 File Offset: 0x000BC1A0
	public CameraLayer CameraLayer
	{
		get
		{
			return this.m_cameraLayer;
		}
		private set
		{
			this.m_cameraLayer = value;
		}
	}

	// Token: 0x170013A6 RID: 5030
	// (get) Token: 0x06003770 RID: 14192 RVA: 0x000BDFA9 File Offset: 0x000BC1A9
	// (set) Token: 0x06003771 RID: 14193 RVA: 0x000BDFB1 File Offset: 0x000BC1B1
	public bool DisableNonGameLayerCollisions
	{
		get
		{
			return this.m_disableNonGameLayerCollisions;
		}
		set
		{
			if (!Application.isPlaying)
			{
				this.m_disableNonGameLayerCollisions = value;
			}
		}
	}

	// Token: 0x170013A7 RID: 5031
	// (get) Token: 0x06003772 RID: 14194 RVA: 0x000BDFC1 File Offset: 0x000BC1C1
	// (set) Token: 0x06003773 RID: 14195 RVA: 0x000BDFC9 File Offset: 0x000BC1C9
	public bool SetVisualLayers
	{
		get
		{
			return this.m_setVisualLayers;
		}
		set
		{
			this.m_setVisualLayers = value;
		}
	}

	// Token: 0x170013A8 RID: 5032
	// (get) Token: 0x06003774 RID: 14196 RVA: 0x000BDFD2 File Offset: 0x000BC1D2
	// (set) Token: 0x06003775 RID: 14197 RVA: 0x000BDFDA File Offset: 0x000BC1DA
	public int SubLayer
	{
		get
		{
			return this.m_subLayer;
		}
		private set
		{
			this.m_subLayer = value;
		}
	}

	// Token: 0x170013A9 RID: 5033
	// (get) Token: 0x06003776 RID: 14198 RVA: 0x000BDFE3 File Offset: 0x000BC1E3
	// (set) Token: 0x06003777 RID: 14199 RVA: 0x000BDFEB File Offset: 0x000BC1EB
	public GameObject Visuals
	{
		get
		{
			return this.m_visuals;
		}
		set
		{
			this.m_visuals = value;
		}
	}

	// Token: 0x06003778 RID: 14200 RVA: 0x000BDFF4 File Offset: 0x000BC1F4
	private void Awake()
	{
		this.m_prop = base.gameObject.GetComponent<Prop>();
	}

	// Token: 0x06003779 RID: 14201 RVA: 0x000BE007 File Offset: 0x000BC207
	private void Start()
	{
		if (!this.m_isSet)
		{
			this.SetLayerAndSubLayer();
		}
	}

	// Token: 0x0600377A RID: 14202 RVA: 0x000BE017 File Offset: 0x000BC217
	public void SetLayerAndSubLayer()
	{
		this.SetLayer();
		this.SetZPosition(false);
	}

	// Token: 0x0600377B RID: 14203 RVA: 0x000BE026 File Offset: 0x000BC226
	public void SetIsDeco(bool isDeco)
	{
		this.m_isDeco = isDeco;
	}

	// Token: 0x0600377C RID: 14204 RVA: 0x000BE02F File Offset: 0x000BC22F
	public void SetCameraLayer(CameraLayer cameraLayer)
	{
		this.CameraLayer = cameraLayer;
		if (Application.isPlaying)
		{
			this.m_isSet = true;
			this.SetLayer();
		}
	}

	// Token: 0x0600377D RID: 14205 RVA: 0x000BE04C File Offset: 0x000BC24C
	private void SetLayer()
	{
		if (!this.SetVisualLayers)
		{
			return;
		}
		if (this.CameraLayer == CameraLayer.None || this.CameraLayer == CameraLayer.Any)
		{
			this.SetCameraLayer(CameraLayer.Game);
		}
		LayerMask layer = CameraLayerUtility.GetLayer(this.CameraLayer);
		if (!this.Visuals)
		{
			this.Visuals = base.gameObject;
		}
		this.InitializeTransforms(this.Visuals);
		this.SetLayersRecursively(this.Visuals, layer);
		if (this.DisableNonGameLayerCollisions && this.CameraLayer != CameraLayer.Game)
		{
			if (this.m_hbControllers == null)
			{
				this.m_hbControllers = base.gameObject.GetComponentsInChildren<IHitboxController>();
			}
			for (int i = 0; i < this.m_hbControllers.Length; i++)
			{
				IHitboxController hitboxController = this.m_hbControllers[i];
				hitboxController.SetHitboxActiveState(HitboxType.Body, false);
				hitboxController.SetHitboxActiveState(HitboxType.Weapon, false);
				hitboxController.SetHitboxActiveState(HitboxType.Terrain, false);
				this.m_hbControllers[i].DisableAllCollisions = true;
			}
			if (this.m_platformColliders == null)
			{
				this.m_platformColliders = (from entry in base.gameObject.GetComponentsInChildren<Collider2D>()
				where entry.gameObject.layer == 8 || entry.gameObject.layer == 10 || entry.gameObject.layer == 9 || entry.gameObject.layer == 11
				select entry).ToArray<Collider2D>();
			}
			for (int j = 0; j < this.m_platformColliders.Length; j++)
			{
				this.m_platformColliders[j].enabled = false;
			}
			if (this.m_propColliders == null)
			{
				this.m_propColliders = (from entry in base.gameObject.GetComponentsInChildren<Collider2D>()
				where entry.gameObject.layer == 12
				select entry).ToArray<Collider2D>();
			}
			for (int k = 0; k < this.m_propColliders.Length; k++)
			{
				this.m_propColliders[k].enabled = false;
			}
			if (this.m_corgiControllers == null)
			{
				this.m_corgiControllers = base.gameObject.GetComponentsInChildren<CorgiController_RL>();
			}
			for (int l = 0; l < this.m_corgiControllers.Length; l++)
			{
				this.m_corgiControllers[l].enabled = false;
			}
		}
		else
		{
			if (this.m_hbControllers == null)
			{
				this.m_hbControllers = base.gameObject.GetComponentsInChildren<IHitboxController>();
			}
			for (int m = 0; m < this.m_hbControllers.Length; m++)
			{
				IHitboxController hitboxController2 = this.m_hbControllers[m];
				hitboxController2.SetHitboxActiveState(HitboxType.Body, true);
				hitboxController2.SetHitboxActiveState(HitboxType.Weapon, true);
				hitboxController2.SetHitboxActiveState(HitboxType.Terrain, true);
				this.m_hbControllers[m].DisableAllCollisions = false;
			}
			if (this.m_platformColliders == null)
			{
				this.m_platformColliders = (from entry in base.gameObject.GetComponentsInChildren<Collider2D>()
				where entry.gameObject.layer == 8 || entry.gameObject.layer == 10 || entry.gameObject.layer == 9 || entry.gameObject.layer == 11
				select entry).ToArray<Collider2D>();
			}
			for (int n = 0; n < this.m_platformColliders.Length; n++)
			{
				this.m_platformColliders[n].enabled = true;
			}
			if (this.m_propColliders == null)
			{
				this.m_propColliders = (from entry in base.gameObject.GetComponentsInChildren<Collider2D>()
				where entry.gameObject.layer == 12
				select entry).ToArray<Collider2D>();
			}
			for (int num = 0; num < this.m_propColliders.Length; num++)
			{
				this.m_propColliders[num].enabled = true;
			}
			if (this.m_corgiControllers == null)
			{
				this.m_corgiControllers = base.gameObject.GetComponentsInChildren<CorgiController_RL>();
			}
			for (int num2 = 0; num2 < this.m_corgiControllers.Length; num2++)
			{
				this.m_corgiControllers[num2].enabled = true;
			}
		}
		if (this.m_childCameraLayerControllers == null)
		{
			ICameraLayerController[] componentsInChildren = base.gameObject.GetComponentsInChildren<CameraLayerController>();
			this.m_childCameraLayerControllers = componentsInChildren;
		}
		for (int num3 = 1; num3 < this.m_childCameraLayerControllers.Length; num3++)
		{
			this.m_childCameraLayerControllers[num3].SetCameraLayer(this.m_childCameraLayerControllers[num3].CameraLayer);
		}
	}

	// Token: 0x0600377E RID: 14206 RVA: 0x000BE3F0 File Offset: 0x000BC5F0
	private void InitializeTransforms(GameObject obj)
	{
		if (this.m_transforms == null)
		{
			this.m_transforms = obj.GetComponentsInChildren<Transform>(true);
			if (!this.m_transparentFXLayersAssigned)
			{
				this.m_transparentFXLayersAssigned = true;
				for (int i = 0; i < this.m_transforms.Length; i++)
				{
					if (this.m_transforms[i].gameObject.layer == 1)
					{
						this.m_transparentFXLayerFlags |= 1 << i;
					}
				}
			}
		}
	}

	// Token: 0x0600377F RID: 14207 RVA: 0x000BE45C File Offset: 0x000BC65C
	private void SetLayersRecursively(GameObject obj, LayerMask layer)
	{
		if (this.CameraLayer == CameraLayer.Game)
		{
			for (int i = 0; i < this.m_transforms.Length; i++)
			{
				Transform transform = this.m_transforms[i];
				if ((this.m_transparentFXLayerFlags & 1 << i) != 0)
				{
					transform.gameObject.layer = 1;
				}
				else
				{
					transform.gameObject.layer = layer;
				}
			}
			return;
		}
		Transform[] transforms = this.m_transforms;
		for (int j = 0; j < transforms.Length; j++)
		{
			transforms[j].gameObject.layer = layer;
		}
	}

	// Token: 0x06003780 RID: 14208 RVA: 0x000BE4E8 File Offset: 0x000BC6E8
	public void SetSubLayer(int subLayer, bool isDeco = false)
	{
		Vector2Int subLayerRange = CameraLayerUtility.GetSubLayerRange(this.CameraLayer);
		this.SubLayer = Mathf.Clamp(subLayer, subLayerRange.x, subLayerRange.y);
		if (Application.isPlaying)
		{
			this.m_isSet = true;
			this.SetZPosition(isDeco);
		}
	}

	// Token: 0x06003781 RID: 14209 RVA: 0x000BE530 File Offset: 0x000BC730
	private void SetZPosition(bool isDeco = false)
	{
		if (!this.Visuals)
		{
			this.Visuals = base.gameObject;
		}
		float num;
		if (this.IsProp && this.m_prop.PropSpawnController && this.m_prop.PropSpawnController.IsOverrideZPosition)
		{
			num = this.m_prop.PropSpawnController.ZPositionOverride;
		}
		else
		{
			num = CameraLayerUtility.GetZDepth(this.CameraLayer, this.SubLayer, this.IsProp, isDeco);
		}
		if (base.CompareTag("Enemy") || base.CompareTag("Player"))
		{
			num = 0f;
		}
		if (base.CompareTag("Enemy") && this.Visuals.transform.lossyScale.z > 1f)
		{
			num *= this.Visuals.transform.lossyScale.z;
		}
		this.Visuals.transform.position = new Vector3(this.Visuals.transform.position.x, this.Visuals.transform.position.y, num);
		if (this.m_zPositionOverrides == null)
		{
			this.m_zPositionOverrides = base.gameObject.GetComponentsInChildren<ZPositionOverride>();
		}
		for (int i = 0; i < this.m_zPositionOverrides.Length; i++)
		{
			this.m_zPositionOverrides[i].SetZPosition();
		}
		if (this.m_childCameraLayerControllers == null)
		{
			ICameraLayerController[] componentsInChildren = base.gameObject.GetComponentsInChildren<CameraLayerController>();
			this.m_childCameraLayerControllers = componentsInChildren;
		}
		for (int j = 1; j < this.m_childCameraLayerControllers.Length; j++)
		{
			this.m_childCameraLayerControllers[j].SetSubLayer(this.m_childCameraLayerControllers[j].SubLayer, false);
		}
	}

	// Token: 0x06003783 RID: 14211 RVA: 0x000BE6EE File Offset: 0x000BC8EE
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002A96 RID: 10902
	[SerializeField]
	private GameObject m_visuals;

	// Token: 0x04002A97 RID: 10903
	[SerializeField]
	private CameraLayer m_cameraLayer = CameraLayer.Game;

	// Token: 0x04002A98 RID: 10904
	[SerializeField]
	private int m_subLayer;

	// Token: 0x04002A99 RID: 10905
	[SerializeField]
	private bool m_setVisualLayers = true;

	// Token: 0x04002A9A RID: 10906
	[SerializeField]
	private bool m_disableNonGameLayerCollisions = true;

	// Token: 0x04002A9B RID: 10907
	private const int CHARACTER_DEFAULT_Z_POSITION = 0;

	// Token: 0x04002A9C RID: 10908
	private Prop m_prop;

	// Token: 0x04002A9D RID: 10909
	private bool m_isDeco;

	// Token: 0x04002A9E RID: 10910
	private bool m_isSet;

	// Token: 0x04002A9F RID: 10911
	private ZPositionOverride[] m_zPositionOverrides;

	// Token: 0x04002AA0 RID: 10912
	private ICameraLayerController[] m_childCameraLayerControllers;

	// Token: 0x04002AA1 RID: 10913
	private CorgiController_RL[] m_corgiControllers;

	// Token: 0x04002AA2 RID: 10914
	private IHitboxController[] m_hbControllers;

	// Token: 0x04002AA3 RID: 10915
	private Transform[] m_transforms;

	// Token: 0x04002AA4 RID: 10916
	private Collider2D[] m_platformColliders;

	// Token: 0x04002AA5 RID: 10917
	private Collider2D[] m_propColliders;

	// Token: 0x04002AA6 RID: 10918
	private bool m_transparentFXLayersAssigned;

	// Token: 0x04002AA7 RID: 10919
	private int m_transparentFXLayerFlags;
}
