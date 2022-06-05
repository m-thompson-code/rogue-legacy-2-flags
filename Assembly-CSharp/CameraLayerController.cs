using System;
using System.Linq;
using UnityEngine;

// Token: 0x02000A19 RID: 2585
public class CameraLayerController : MonoBehaviour, ICameraLayerController, IRootObj
{
	// Token: 0x17001AF6 RID: 6902
	// (get) Token: 0x06004DF5 RID: 19957 RVA: 0x0002A64C File Offset: 0x0002884C
	public bool IsProp
	{
		get
		{
			return this.m_prop;
		}
	}

	// Token: 0x17001AF7 RID: 6903
	// (get) Token: 0x06004DF6 RID: 19958 RVA: 0x0002A659 File Offset: 0x00028859
	public bool IsSet
	{
		get
		{
			return this.m_isSet;
		}
	}

	// Token: 0x17001AF8 RID: 6904
	// (get) Token: 0x06004DF7 RID: 19959 RVA: 0x0002A661 File Offset: 0x00028861
	// (set) Token: 0x06004DF8 RID: 19960 RVA: 0x0002A669 File Offset: 0x00028869
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

	// Token: 0x17001AF9 RID: 6905
	// (get) Token: 0x06004DF9 RID: 19961 RVA: 0x0002A672 File Offset: 0x00028872
	// (set) Token: 0x06004DFA RID: 19962 RVA: 0x0002A67A File Offset: 0x0002887A
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

	// Token: 0x17001AFA RID: 6906
	// (get) Token: 0x06004DFB RID: 19963 RVA: 0x0002A68A File Offset: 0x0002888A
	// (set) Token: 0x06004DFC RID: 19964 RVA: 0x0002A692 File Offset: 0x00028892
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

	// Token: 0x17001AFB RID: 6907
	// (get) Token: 0x06004DFD RID: 19965 RVA: 0x0002A69B File Offset: 0x0002889B
	// (set) Token: 0x06004DFE RID: 19966 RVA: 0x0002A6A3 File Offset: 0x000288A3
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

	// Token: 0x17001AFC RID: 6908
	// (get) Token: 0x06004DFF RID: 19967 RVA: 0x0002A6AC File Offset: 0x000288AC
	// (set) Token: 0x06004E00 RID: 19968 RVA: 0x0002A6B4 File Offset: 0x000288B4
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

	// Token: 0x06004E01 RID: 19969 RVA: 0x0002A6BD File Offset: 0x000288BD
	private void Awake()
	{
		this.m_prop = base.gameObject.GetComponent<Prop>();
	}

	// Token: 0x06004E02 RID: 19970 RVA: 0x0002A6D0 File Offset: 0x000288D0
	private void Start()
	{
		if (!this.m_isSet)
		{
			this.SetLayerAndSubLayer();
		}
	}

	// Token: 0x06004E03 RID: 19971 RVA: 0x0002A6E0 File Offset: 0x000288E0
	public void SetLayerAndSubLayer()
	{
		this.SetLayer();
		this.SetZPosition(false);
	}

	// Token: 0x06004E04 RID: 19972 RVA: 0x0002A6EF File Offset: 0x000288EF
	public void SetIsDeco(bool isDeco)
	{
		this.m_isDeco = isDeco;
	}

	// Token: 0x06004E05 RID: 19973 RVA: 0x0002A6F8 File Offset: 0x000288F8
	public void SetCameraLayer(CameraLayer cameraLayer)
	{
		this.CameraLayer = cameraLayer;
		if (Application.isPlaying)
		{
			this.m_isSet = true;
			this.SetLayer();
		}
	}

	// Token: 0x06004E06 RID: 19974 RVA: 0x0012CB4C File Offset: 0x0012AD4C
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

	// Token: 0x06004E07 RID: 19975 RVA: 0x0012CEF0 File Offset: 0x0012B0F0
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

	// Token: 0x06004E08 RID: 19976 RVA: 0x0012CF5C File Offset: 0x0012B15C
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

	// Token: 0x06004E09 RID: 19977 RVA: 0x0012CFE8 File Offset: 0x0012B1E8
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

	// Token: 0x06004E0A RID: 19978 RVA: 0x0012D030 File Offset: 0x0012B230
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

	// Token: 0x06004E0C RID: 19980 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04003AE8 RID: 15080
	[SerializeField]
	private GameObject m_visuals;

	// Token: 0x04003AE9 RID: 15081
	[SerializeField]
	private CameraLayer m_cameraLayer = CameraLayer.Game;

	// Token: 0x04003AEA RID: 15082
	[SerializeField]
	private int m_subLayer;

	// Token: 0x04003AEB RID: 15083
	[SerializeField]
	private bool m_setVisualLayers = true;

	// Token: 0x04003AEC RID: 15084
	[SerializeField]
	private bool m_disableNonGameLayerCollisions = true;

	// Token: 0x04003AED RID: 15085
	private const int CHARACTER_DEFAULT_Z_POSITION = 0;

	// Token: 0x04003AEE RID: 15086
	private Prop m_prop;

	// Token: 0x04003AEF RID: 15087
	private bool m_isDeco;

	// Token: 0x04003AF0 RID: 15088
	private bool m_isSet;

	// Token: 0x04003AF1 RID: 15089
	private ZPositionOverride[] m_zPositionOverrides;

	// Token: 0x04003AF2 RID: 15090
	private ICameraLayerController[] m_childCameraLayerControllers;

	// Token: 0x04003AF3 RID: 15091
	private CorgiController_RL[] m_corgiControllers;

	// Token: 0x04003AF4 RID: 15092
	private IHitboxController[] m_hbControllers;

	// Token: 0x04003AF5 RID: 15093
	private Transform[] m_transforms;

	// Token: 0x04003AF6 RID: 15094
	private Collider2D[] m_platformColliders;

	// Token: 0x04003AF7 RID: 15095
	private Collider2D[] m_propColliders;

	// Token: 0x04003AF8 RID: 15096
	private bool m_transparentFXLayersAssigned;

	// Token: 0x04003AF9 RID: 15097
	private int m_transparentFXLayerFlags;
}
