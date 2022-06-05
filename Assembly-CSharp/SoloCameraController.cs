using System;
using UnityEngine;

// Token: 0x020006AF RID: 1711
public class SoloCameraController : MonoBehaviour
{
	// Token: 0x1700158B RID: 5515
	// (get) Token: 0x06003F06 RID: 16134 RVA: 0x000E03ED File Offset: 0x000DE5ED
	public Camera Camera
	{
		get
		{
			return this.m_camera;
		}
	}

	// Token: 0x06003F07 RID: 16135 RVA: 0x000E03F5 File Offset: 0x000DE5F5
	private void Awake()
	{
		this.Initialize();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06003F08 RID: 16136 RVA: 0x000E040C File Offset: 0x000DE60C
	public void AddToCameraLayer(GameObject obj)
	{
		obj.SetLayerRecursively(28, false);
		foreach (BaseTrait baseTrait in TraitManager.ActiveTraitList)
		{
			if (baseTrait.TraitMask != null)
			{
				baseTrait.TraitMask.gameObject.gameObject.layer = 27;
			}
		}
	}

	// Token: 0x06003F09 RID: 16137 RVA: 0x000E048C File Offset: 0x000DE68C
	private void Initialize()
	{
		this.m_defaultDepth = this.m_camera.depth;
		this.m_isInitialized = true;
	}

	// Token: 0x06003F0A RID: 16138 RVA: 0x000E04A6 File Offset: 0x000DE6A6
	public void ResetController()
	{
		if (!this.m_isInitialized)
		{
			return;
		}
		this.SetCameraDepth(this.m_defaultDepth);
	}

	// Token: 0x06003F0B RID: 16139 RVA: 0x000E04BD File Offset: 0x000DE6BD
	public void SetCameraDepth(float depth)
	{
		if (!this.m_isInitialized)
		{
			this.Initialize();
		}
		this.m_camera.depth = depth;
	}

	// Token: 0x04002ED8 RID: 11992
	[SerializeField]
	private Camera m_camera;

	// Token: 0x04002ED9 RID: 11993
	private bool m_isInitialized;

	// Token: 0x04002EDA RID: 11994
	private float m_defaultDepth;
}
