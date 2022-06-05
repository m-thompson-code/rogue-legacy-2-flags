using System;

// Token: 0x02000C96 RID: 3222
public class HighlightedSkillChangedEventArgs : EventArgs
{
	// Token: 0x06005C7B RID: 23675 RVA: 0x00032C8E File Offset: 0x00030E8E
	public HighlightedSkillChangedEventArgs(SkillTreeType skillTreeType)
	{
		this.Initialize(skillTreeType);
	}

	// Token: 0x06005C7C RID: 23676 RVA: 0x00032C9D File Offset: 0x00030E9D
	public void Initialize(SkillTreeType skillTreeType)
	{
		this.SkillTreeType = skillTreeType;
	}

	// Token: 0x17001EA9 RID: 7849
	// (get) Token: 0x06005C7D RID: 23677 RVA: 0x00032CA6 File Offset: 0x00030EA6
	// (set) Token: 0x06005C7E RID: 23678 RVA: 0x00032CAE File Offset: 0x00030EAE
	public SkillTreeType SkillTreeType { get; private set; }
}
