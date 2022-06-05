using System;

// Token: 0x020007D1 RID: 2001
public class SkillLevelChangedEventArgs : EventArgs
{
	// Token: 0x060042F6 RID: 17142 RVA: 0x000EC2A1 File Offset: 0x000EA4A1
	public SkillLevelChangedEventArgs(SkillTreeType skillTreeType, int prevLevel, int newLevel)
	{
		this.Initialize(skillTreeType, prevLevel, newLevel);
	}

	// Token: 0x060042F7 RID: 17143 RVA: 0x000EC2B2 File Offset: 0x000EA4B2
	public void Initialize(SkillTreeType skillTreeType, int prevLevel, int newLevel)
	{
		this.SkillTreeType = skillTreeType;
		this.NewLevel = newLevel;
		this.PrevLevel = prevLevel;
	}

	// Token: 0x170016AC RID: 5804
	// (get) Token: 0x060042F8 RID: 17144 RVA: 0x000EC2C9 File Offset: 0x000EA4C9
	// (set) Token: 0x060042F9 RID: 17145 RVA: 0x000EC2D1 File Offset: 0x000EA4D1
	public SkillTreeType SkillTreeType { get; private set; }

	// Token: 0x170016AD RID: 5805
	// (get) Token: 0x060042FA RID: 17146 RVA: 0x000EC2DA File Offset: 0x000EA4DA
	// (set) Token: 0x060042FB RID: 17147 RVA: 0x000EC2E2 File Offset: 0x000EA4E2
	public int NewLevel { get; private set; }

	// Token: 0x170016AE RID: 5806
	// (get) Token: 0x060042FC RID: 17148 RVA: 0x000EC2EB File Offset: 0x000EA4EB
	// (set) Token: 0x060042FD RID: 17149 RVA: 0x000EC2F3 File Offset: 0x000EA4F3
	public int PrevLevel { get; private set; }
}
