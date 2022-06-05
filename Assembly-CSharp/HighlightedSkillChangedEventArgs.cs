using System;

// Token: 0x020007D0 RID: 2000
public class HighlightedSkillChangedEventArgs : EventArgs
{
	// Token: 0x060042F2 RID: 17138 RVA: 0x000EC278 File Offset: 0x000EA478
	public HighlightedSkillChangedEventArgs(SkillTreeType skillTreeType)
	{
		this.Initialize(skillTreeType);
	}

	// Token: 0x060042F3 RID: 17139 RVA: 0x000EC287 File Offset: 0x000EA487
	public void Initialize(SkillTreeType skillTreeType)
	{
		this.SkillTreeType = skillTreeType;
	}

	// Token: 0x170016AB RID: 5803
	// (get) Token: 0x060042F4 RID: 17140 RVA: 0x000EC290 File Offset: 0x000EA490
	// (set) Token: 0x060042F5 RID: 17141 RVA: 0x000EC298 File Offset: 0x000EA498
	public SkillTreeType SkillTreeType { get; private set; }
}
