using System;
using UnityEngine;

// Token: 0x02000600 RID: 1536
public class Deco : MonoBehaviour, IGenericPoolObj
{
	// Token: 0x170013BE RID: 5054
	// (get) Token: 0x060037BD RID: 14269 RVA: 0x000BEEA8 File Offset: 0x000BD0A8
	// (set) Token: 0x060037BE RID: 14270 RVA: 0x000BEEB0 File Offset: 0x000BD0B0
	public bool IsFreePoolObj { get; set; }

	// Token: 0x170013BF RID: 5055
	// (get) Token: 0x060037BF RID: 14271 RVA: 0x000BEEB9 File Offset: 0x000BD0B9
	// (set) Token: 0x060037C0 RID: 14272 RVA: 0x000BEEC1 File Offset: 0x000BD0C1
	public bool IsAwakeCalled { get; protected set; }

	// Token: 0x170013C0 RID: 5056
	// (get) Token: 0x060037C1 RID: 14273 RVA: 0x000BEECA File Offset: 0x000BD0CA
	// (set) Token: 0x060037C2 RID: 14274 RVA: 0x000BEEEB File Offset: 0x000BD0EB
	public int NameHash
	{
		get
		{
			if (this.m_nameHash == 0)
			{
				this.m_nameHash = Animator.StringToHash(base.name);
			}
			return this.m_nameHash;
		}
		set
		{
			this.m_nameHash = value;
		}
	}

	// Token: 0x170013C1 RID: 5057
	// (get) Token: 0x060037C3 RID: 14275 RVA: 0x000BEEF4 File Offset: 0x000BD0F4
	public CameraLayerController[] CameraLayerControllers
	{
		get
		{
			if (this.m_cameraLayerControllers == null)
			{
				this.m_cameraLayerControllers = base.gameObject.GetComponentsInChildren<CameraLayerController>(true);
				for (int i = 0; i < this.m_cameraLayerControllers.Length; i++)
				{
					this.m_cameraLayerControllers[i].SetIsDeco(true);
				}
			}
			return this.m_cameraLayerControllers;
		}
	}

	// Token: 0x170013C2 RID: 5058
	// (get) Token: 0x060037C4 RID: 14276 RVA: 0x000BEF42 File Offset: 0x000BD142
	public int PropCount
	{
		get
		{
			return this.Props.Length;
		}
	}

	// Token: 0x170013C3 RID: 5059
	// (get) Token: 0x060037C5 RID: 14277 RVA: 0x000BEF4C File Offset: 0x000BD14C
	// (set) Token: 0x060037C6 RID: 14278 RVA: 0x000BEF68 File Offset: 0x000BD168
	public Prop[] Props
	{
		get
		{
			if (this.m_props == null)
			{
				this.m_props = base.GetComponentsInChildren<Prop>();
			}
			return this.m_props;
		}
		private set
		{
			this.m_props = value;
		}
	}

	// Token: 0x060037C7 RID: 14279 RVA: 0x000BEF71 File Offset: 0x000BD171
	private void Awake()
	{
		this.m_initialScale = base.transform.localScale;
		this.IsAwakeCalled = true;
	}

	// Token: 0x060037C8 RID: 14280 RVA: 0x000BEF8C File Offset: 0x000BD18C
	public void ResetValues()
	{
		foreach (Prop prop in this.Props)
		{
			if (!prop.gameObject.activeSelf)
			{
				prop.gameObject.SetActive(true);
			}
			prop.ResetValues();
		}
		base.transform.localScale = this.m_initialScale;
	}

	// Token: 0x060037C9 RID: 14281 RVA: 0x000BEFE4 File Offset: 0x000BD1E4
	public void SetCameraLayerControllers(Prop ownerProp)
	{
		if (ownerProp)
		{
			foreach (CameraLayerController cameraLayerController in this.CameraLayerControllers)
			{
				cameraLayerController.SetCameraLayer(ownerProp.CameraLayerController.CameraLayer);
				cameraLayerController.SetSubLayer(ownerProp.CameraLayerController.SubLayer, true);
			}
		}
	}

	// Token: 0x060037CA RID: 14282 RVA: 0x000BF033 File Offset: 0x000BD233
	private void OnDisable()
	{
		DisablePooledObjectManager.DisablePooledObject(this, false);
	}

	// Token: 0x060037CC RID: 14284 RVA: 0x000BF044 File Offset: 0x000BD244
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002ABB RID: 10939
	private Prop[] m_props;

	// Token: 0x04002ABC RID: 10940
	private CameraLayerController[] m_cameraLayerControllers;

	// Token: 0x04002ABD RID: 10941
	private Vector3 m_initialScale;

	// Token: 0x04002ABE RID: 10942
	private int m_nameHash;
}
