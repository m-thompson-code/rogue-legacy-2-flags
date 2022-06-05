using System;

// Token: 0x02000C98 RID: 3224
public class LineageHeirChangedEventArgs : EventArgs
{
	// Token: 0x06005C87 RID: 23687 RVA: 0x00032D12 File Offset: 0x00030F12
	public LineageHeirChangedEventArgs(CharacterData charData, bool classLocked, bool spellLocked)
	{
		this.Initialize(charData, classLocked, spellLocked);
	}

	// Token: 0x06005C88 RID: 23688 RVA: 0x00032D23 File Offset: 0x00030F23
	public void Initialize(CharacterData charData, bool classLocked, bool spellLocked)
	{
		this.CharacterData = charData;
		this.ClassLocked = classLocked;
		this.SpellLocked = spellLocked;
	}

	// Token: 0x17001EAD RID: 7853
	// (get) Token: 0x06005C89 RID: 23689 RVA: 0x00032D3A File Offset: 0x00030F3A
	// (set) Token: 0x06005C8A RID: 23690 RVA: 0x00032D42 File Offset: 0x00030F42
	public CharacterData CharacterData { get; private set; }

	// Token: 0x17001EAE RID: 7854
	// (get) Token: 0x06005C8B RID: 23691 RVA: 0x00032D4B File Offset: 0x00030F4B
	// (set) Token: 0x06005C8C RID: 23692 RVA: 0x00032D53 File Offset: 0x00030F53
	public bool ClassLocked { get; private set; }

	// Token: 0x17001EAF RID: 7855
	// (get) Token: 0x06005C8D RID: 23693 RVA: 0x00032D5C File Offset: 0x00030F5C
	// (set) Token: 0x06005C8E RID: 23694 RVA: 0x00032D64 File Offset: 0x00030F64
	public bool SpellLocked { get; private set; }
}
