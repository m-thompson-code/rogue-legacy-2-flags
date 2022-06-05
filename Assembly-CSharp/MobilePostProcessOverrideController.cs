using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class MobilePostProcessOverrideController : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000012 RID: 18 RVA: 0x00003B21 File Offset: 0x00001D21
	public MobilePostProcessingProfile Profile
	{
		get
		{
			return this.m_profileClone;
		}
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00003B29 File Offset: 0x00001D29
	private void Awake()
	{
		this.m_profileClone = this.m_profile.Clone();
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00003B3C File Offset: 0x00001D3C
	private void OnEnable()
	{
		if (CameraController.IsInstantiated && CameraController.ForegroundPostProcessing && this.Profile != null)
		{
			if (this.m_isTraitEffect)
			{
				CameraController.ForegroundPostProcessing.AddTraitOverride(this.Profile);
				return;
			}
			CameraController.ForegroundPostProcessing.AddDimensionOverride(this.Profile);
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00003B94 File Offset: 0x00001D94
	private void OnDisable()
	{
		if (CameraController.IsInstantiated && CameraController.ForegroundPostProcessing && this.Profile != null)
		{
			if (this.m_isTraitEffect)
			{
				CameraController.ForegroundPostProcessing.RemoveTraitOverride(this.Profile);
				return;
			}
			CameraController.ForegroundPostProcessing.RemoveDimensionOverride(this.Profile);
		}
	}

	// Token: 0x0400000E RID: 14
	[SerializeField]
	private MobilePostProcessingProfile m_profile;

	// Token: 0x0400000F RID: 15
	[SerializeField]
	private bool m_isTraitEffect = true;

	// Token: 0x04000010 RID: 16
	private MobilePostProcessingProfile m_profileClone;
}
