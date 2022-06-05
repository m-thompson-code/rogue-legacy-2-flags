using System;

// Token: 0x020007B1 RID: 1969
public class ForceManaRegenEventArgs : EventArgs
{
	// Token: 0x06004245 RID: 16965 RVA: 0x000EBAF7 File Offset: 0x000E9CF7
	public ForceManaRegenEventArgs(float regenAmount, bool usePlayerRegenMods)
	{
		this.Initialise(regenAmount, usePlayerRegenMods);
	}

	// Token: 0x06004246 RID: 16966 RVA: 0x000EBB07 File Offset: 0x000E9D07
	public void Initialise(float regenAmount, bool usePlayerRegenMods)
	{
		this.RegenAmount = regenAmount;
		this.UsePlayerRegenMods = usePlayerRegenMods;
	}

	// Token: 0x17001670 RID: 5744
	// (get) Token: 0x06004247 RID: 16967 RVA: 0x000EBB17 File Offset: 0x000E9D17
	// (set) Token: 0x06004248 RID: 16968 RVA: 0x000EBB1F File Offset: 0x000E9D1F
	public float RegenAmount { get; private set; }

	// Token: 0x17001671 RID: 5745
	// (get) Token: 0x06004249 RID: 16969 RVA: 0x000EBB28 File Offset: 0x000E9D28
	// (set) Token: 0x0600424A RID: 16970 RVA: 0x000EBB30 File Offset: 0x000E9D30
	public bool UsePlayerRegenMods { get; private set; }
}
