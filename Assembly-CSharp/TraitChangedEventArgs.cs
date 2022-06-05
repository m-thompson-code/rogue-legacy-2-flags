using System;

// Token: 0x02000C99 RID: 3225
public class TraitChangedEventArgs : EventArgs
{
	// Token: 0x06005C8F RID: 23695 RVA: 0x00032D6D File Offset: 0x00030F6D
	public TraitChangedEventArgs(TraitType traitOne, TraitType traitTwo)
	{
		this.Initialize(traitOne, traitTwo);
	}

	// Token: 0x06005C90 RID: 23696 RVA: 0x00032D7D File Offset: 0x00030F7D
	public void Initialize(TraitType traitOne, TraitType traitTwo)
	{
		this.TraitOne = traitOne;
		this.TraitTwo = traitTwo;
	}

	// Token: 0x17001EB0 RID: 7856
	// (get) Token: 0x06005C91 RID: 23697 RVA: 0x00032D8D File Offset: 0x00030F8D
	// (set) Token: 0x06005C92 RID: 23698 RVA: 0x00032D95 File Offset: 0x00030F95
	public TraitType TraitOne { get; private set; }

	// Token: 0x17001EB1 RID: 7857
	// (get) Token: 0x06005C93 RID: 23699 RVA: 0x00032D9E File Offset: 0x00030F9E
	// (set) Token: 0x06005C94 RID: 23700 RVA: 0x00032DA6 File Offset: 0x00030FA6
	public TraitType TraitTwo { get; private set; }
}
