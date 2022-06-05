using System;

// Token: 0x02000C97 RID: 3223
public class SkillLevelChangedEventArgs : EventArgs
{
	// Token: 0x06005C7F RID: 23679 RVA: 0x00032CB7 File Offset: 0x00030EB7
	public SkillLevelChangedEventArgs(SkillTreeType skillTreeType, int prevLevel, int newLevel)
	{
		this.Initialize(skillTreeType, prevLevel, newLevel);
	}

	// Token: 0x06005C80 RID: 23680 RVA: 0x00032CC8 File Offset: 0x00030EC8
	public void Initialize(SkillTreeType skillTreeType, int prevLevel, int newLevel)
	{
		this.SkillTreeType = skillTreeType;
		this.NewLevel = newLevel;
		this.PrevLevel = prevLevel;
	}

	// Token: 0x17001EAA RID: 7850
	// (get) Token: 0x06005C81 RID: 23681 RVA: 0x00032CDF File Offset: 0x00030EDF
	// (set) Token: 0x06005C82 RID: 23682 RVA: 0x00032CE7 File Offset: 0x00030EE7
	public SkillTreeType SkillTreeType { get; private set; }

	// Token: 0x17001EAB RID: 7851
	// (get) Token: 0x06005C83 RID: 23683 RVA: 0x00032CF0 File Offset: 0x00030EF0
	// (set) Token: 0x06005C84 RID: 23684 RVA: 0x00032CF8 File Offset: 0x00030EF8
	public int NewLevel { get; private set; }

	// Token: 0x17001EAC RID: 7852
	// (get) Token: 0x06005C85 RID: 23685 RVA: 0x00032D01 File Offset: 0x00030F01
	// (set) Token: 0x06005C86 RID: 23686 RVA: 0x00032D09 File Offset: 0x00030F09
	public int PrevLevel { get; private set; }
}
