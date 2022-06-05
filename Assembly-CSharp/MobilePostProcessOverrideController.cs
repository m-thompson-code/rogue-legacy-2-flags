using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class MobilePostProcessOverrideController : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000013 RID: 19 RVA: 0x00002B87 File Offset: 0x00000D87
	public MobilePostProcessingProfile Profile
	{
		get
		{
			return this.m_profileClone;
		}
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002B8F File Offset: 0x00000D8F
	private void Awake()
	{
		this.m_profileClone = this.m_profile.Clone();
	}

	// Token: 0x06000015 RID: 21 RVA: 0x0003FD08 File Offset: 0x0003DF08
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

	// Token: 0x06000016 RID: 22 RVA: 0x0003FD60 File Offset: 0x0003DF60
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

	// Token: 0x04000010 RID: 16
	[SerializeField]
	private MobilePostProcessingProfile m_profile;

	// Token: 0x04000011 RID: 17
	[SerializeField]
	private bool m_isTraitEffect = true;

	// Token: 0x04000012 RID: 18
	private MobilePostProcessingProfile m_profileClone;
}
