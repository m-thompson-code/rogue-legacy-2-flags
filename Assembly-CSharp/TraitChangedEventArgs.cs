using System;

// Token: 0x020007D3 RID: 2003
public class TraitChangedEventArgs : EventArgs
{
	// Token: 0x06004306 RID: 17158 RVA: 0x000EC357 File Offset: 0x000EA557
	public TraitChangedEventArgs(TraitType traitOne, TraitType traitTwo)
	{
		this.Initialize(traitOne, traitTwo);
	}

	// Token: 0x06004307 RID: 17159 RVA: 0x000EC367 File Offset: 0x000EA567
	public void Initialize(TraitType traitOne, TraitType traitTwo)
	{
		this.TraitOne = traitOne;
		this.TraitTwo = traitTwo;
	}

	// Token: 0x170016B2 RID: 5810
	// (get) Token: 0x06004308 RID: 17160 RVA: 0x000EC377 File Offset: 0x000EA577
	// (set) Token: 0x06004309 RID: 17161 RVA: 0x000EC37F File Offset: 0x000EA57F
	public TraitType TraitOne { get; private set; }

	// Token: 0x170016B3 RID: 5811
	// (get) Token: 0x0600430A RID: 17162 RVA: 0x000EC388 File Offset: 0x000EA588
	// (set) Token: 0x0600430B RID: 17163 RVA: 0x000EC390 File Offset: 0x000EA590
	public TraitType TraitTwo { get; private set; }
}
