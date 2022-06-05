using System;
using UnityEngine;

// Token: 0x02000B67 RID: 2919
[Serializable]
public class PostProcessingBiomeArtData
{
	// Token: 0x17001DA6 RID: 7590
	// (get) Token: 0x060058BF RID: 22719 RVA: 0x0003045D File Offset: 0x0002E65D
	// (set) Token: 0x060058C0 RID: 22720 RVA: 0x00030465 File Offset: 0x0002E665
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

	// Token: 0x17001DA7 RID: 7591
	// (get) Token: 0x060058C1 RID: 22721 RVA: 0x0003046E File Offset: 0x0002E66E
	// (set) Token: 0x060058C2 RID: 22722 RVA: 0x00030476 File Offset: 0x0002E676
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

	// Token: 0x060058C3 RID: 22723 RVA: 0x0003047F File Offset: 0x0002E67F
	public void SetMainProfile(MobilePostProcessingProfile behaviour)
	{
		if (Application.isPlaying)
		{
			return;
		}
		this.ForegroundProfile = behaviour;
	}

	// Token: 0x060058C4 RID: 22724 RVA: 0x00030490 File Offset: 0x0002E690
	public void SetBackgroundProfile(MobilePostProcessingProfile behaviour)
	{
		if (Application.isPlaying)
		{
			return;
		}
		this.BackgroundProfile = behaviour;
	}

	// Token: 0x04004175 RID: 16757
	[SerializeField]
	private MobilePostProcessingProfile m_foregroundProfile;

	// Token: 0x04004176 RID: 16758
	[SerializeField]
	private MobilePostProcessingProfile m_backgroundProfile;
}
