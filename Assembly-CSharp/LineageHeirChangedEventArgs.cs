using System;

// Token: 0x020007D2 RID: 2002
public class LineageHeirChangedEventArgs : EventArgs
{
	// Token: 0x060042FE RID: 17150 RVA: 0x000EC2FC File Offset: 0x000EA4FC
	public LineageHeirChangedEventArgs(CharacterData charData, bool classLocked, bool spellLocked)
	{
		this.Initialize(charData, classLocked, spellLocked);
	}

	// Token: 0x060042FF RID: 17151 RVA: 0x000EC30D File Offset: 0x000EA50D
	public void Initialize(CharacterData charData, bool classLocked, bool spellLocked)
	{
		this.CharacterData = charData;
		this.ClassLocked = classLocked;
		this.SpellLocked = spellLocked;
	}

	// Token: 0x170016AF RID: 5807
	// (get) Token: 0x06004300 RID: 17152 RVA: 0x000EC324 File Offset: 0x000EA524
	// (set) Token: 0x06004301 RID: 17153 RVA: 0x000EC32C File Offset: 0x000EA52C
	public CharacterData CharacterData { get; private set; }

	// Token: 0x170016B0 RID: 5808
	// (get) Token: 0x06004302 RID: 17154 RVA: 0x000EC335 File Offset: 0x000EA535
	// (set) Token: 0x06004303 RID: 17155 RVA: 0x000EC33D File Offset: 0x000EA53D
	public bool ClassLocked { get; private set; }

	// Token: 0x170016B1 RID: 5809
	// (get) Token: 0x06004304 RID: 17156 RVA: 0x000EC346 File Offset: 0x000EA546
	// (set) Token: 0x06004305 RID: 17157 RVA: 0x000EC34E File Offset: 0x000EA54E
	public bool SpellLocked { get; private set; }
}
