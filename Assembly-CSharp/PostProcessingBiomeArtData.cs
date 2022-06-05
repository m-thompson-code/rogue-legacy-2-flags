using System;
using UnityEngine;

// Token: 0x020006BC RID: 1724
[Serializable]
public class PostProcessingBiomeArtData
{
	// Token: 0x170015AE RID: 5550
	// (get) Token: 0x06003F88 RID: 16264 RVA: 0x000E2340 File Offset: 0x000E0540
	// (set) Token: 0x06003F89 RID: 16265 RVA: 0x000E2348 File Offset: 0x000E0548
	public MobilePostProcessingProfile ForegroundProfile
	{
		get
		{
			return this.m_foregroundProfile;
		}
		private set
		{
			this.m_foregroundProfile = value;
		}
	}

	// Token: 0x170015AF RID: 5551
	// (get) Token: 0x06003F8A RID: 16266 RVA: 0x000E2351 File Offset: 0x000E0551
	// (set) Token: 0x06003F8B RID: 16267 RVA: 0x000E2359 File Offset: 0x000E0559
	public MobilePostProcessingProfile BackgroundProfile
	{
		get
		{
			return this.m_backgroundProfile;
		}
		private set
		{
			this.m_backgroundProfile = value;
		}
	}

	// Token: 0x06003F8C RID: 16268 RVA: 0x000E2362 File Offset: 0x000E0562
	public void SetMainProfile(MobilePostProcessingProfile behaviour)
	{
		if (Application.isPlaying)
		{
			return;
		}
		this.ForegroundProfile = behaviour;
	}

	// Token: 0x06003F8D RID: 16269 RVA: 0x000E2373 File Offset: 0x000E0573
	public void SetBackgroundProfile(MobilePostProcessingProfile behaviour)
	{
		if (Application.isPlaying)
		{
			return;
		}
		this.BackgroundProfile = behaviour;
	}

	// Token: 0x04002F26 RID: 12070
	[SerializeField]
	private MobilePostProcessingProfile m_foregroundProfile;

	// Token: 0x04002F27 RID: 12071
	[SerializeField]
	private MobilePostProcessingProfile m_backgroundProfile;
}
