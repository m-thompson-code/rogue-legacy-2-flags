using System;
using UnityEngine;

// Token: 0x02000B50 RID: 2896
public class SoloCameraController : MonoBehaviour
{
	// Token: 0x17001D77 RID: 7543
	// (get) Token: 0x0600580E RID: 22542 RVA: 0x0002FDFB File Offset: 0x0002DFFB
	public Camera Camera
	{
		get
		{
			return this.m_camera;
		}
	}

	// Token: 0x0600580F RID: 22543 RVA: 0x0002FE03 File Offset: 0x0002E003
	private void Awake()
	{
		this.Initialize();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06005810 RID: 22544 RVA: 0x0015072C File Offset: 0x0014E92C
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

	// Token: 0x06005811 RID: 22545 RVA: 0x0002FE17 File Offset: 0x0002E017
	private void Initialize()
	{
		this.m_defaultDepth = this.m_camera.depth;
		this.m_isInitialized = true;
	}

	// Token: 0x06005812 RID: 22546 RVA: 0x0002FE31 File Offset: 0x0002E031
	public void ResetController()
	{
		if (!this.m_isInitialized)
		{
			return;
		}
		this.SetCameraDepth(this.m_defaultDepth);
	}

	// Token: 0x06005813 RID: 22547 RVA: 0x0002FE48 File Offset: 0x0002E048
	public void SetCameraDepth(float depth)
	{
		if (!this.m_isInitialized)
		{
			this.Initialize();
		}
		this.m_camera.depth = depth;
	}

	// Token: 0x04004111 RID: 16657
	[SerializeField]
	private Camera m_camera;

	// Token: 0x04004112 RID: 16658
	private bool m_isInitialized;

	// Token: 0x04004113 RID: 16659
	private float m_defaultDepth;
}
