using System;
using UnityEngine;

// Token: 0x02000A1F RID: 2591
public class Deco : MonoBehaviour, IGenericPoolObj
{
	// Token: 0x17001B11 RID: 6929
	// (get) Token: 0x06004E4E RID: 20046 RVA: 0x0002A9C1 File Offset: 0x00028BC1
	// (set) Token: 0x06004E4F RID: 20047 RVA: 0x0002A9C9 File Offset: 0x00028BC9
	public bool IsFreePoolObj { get; set; }

	// Token: 0x17001B12 RID: 6930
	// (get) Token: 0x06004E50 RID: 20048 RVA: 0x0002A9D2 File Offset: 0x00028BD2
	// (set) Token: 0x06004E51 RID: 20049 RVA: 0x0002A9DA File Offset: 0x00028BDA
	public bool IsAwakeCalled { get; protected set; }

	// Token: 0x17001B13 RID: 6931
	// (get) Token: 0x06004E52 RID: 20050 RVA: 0x0002A9E3 File Offset: 0x00028BE3
	// (set) Token: 0x06004E53 RID: 20051 RVA: 0x0002AA04 File Offset: 0x00028C04
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

	// Token: 0x17001B14 RID: 6932
	// (get) Token: 0x06004E54 RID: 20052 RVA: 0x0012D764 File Offset: 0x0012B964
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

	// Token: 0x17001B15 RID: 6933
	// (get) Token: 0x06004E55 RID: 20053 RVA: 0x0002AA0D File Offset: 0x00028C0D
	public int PropCount
	{
		get
		{
			return this.Props.Length;
		}
	}

	// Token: 0x17001B16 RID: 6934
	// (get) Token: 0x06004E56 RID: 20054 RVA: 0x0002AA17 File Offset: 0x00028C17
	// (set) Token: 0x06004E57 RID: 20055 RVA: 0x0002AA33 File Offset: 0x00028C33
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

	// Token: 0x06004E58 RID: 20056 RVA: 0x0002AA3C File Offset: 0x00028C3C
	private void Awake()
	{
		this.m_initialScale = base.transform.localScale;
		this.IsAwakeCalled = true;
	}

	// Token: 0x06004E59 RID: 20057 RVA: 0x0012D7B4 File Offset: 0x0012B9B4
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

	// Token: 0x06004E5A RID: 20058 RVA: 0x0012D80C File Offset: 0x0012BA0C
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

	// Token: 0x06004E5B RID: 20059 RVA: 0x0001BE85 File Offset: 0x0001A085
	private void OnDisable()
	{
		DisablePooledObjectManager.DisablePooledObject(this, false);
	}

	// Token: 0x06004E5D RID: 20061 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04003B13 RID: 15123
	private Prop[] m_props;

	// Token: 0x04003B14 RID: 15124
	private CameraLayerController[] m_cameraLayerControllers;

	// Token: 0x04003B15 RID: 15125
	private Vector3 m_initialScale;

	// Token: 0x04003B16 RID: 15126
	private int m_nameHash;
}
