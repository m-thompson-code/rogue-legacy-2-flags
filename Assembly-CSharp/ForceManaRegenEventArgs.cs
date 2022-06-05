using System;

// Token: 0x02000C77 RID: 3191
public class ForceManaRegenEventArgs : EventArgs
{
	// Token: 0x06005BCE RID: 23502 RVA: 0x0003250D File Offset: 0x0003070D
	public ForceManaRegenEventArgs(float regenAmount, bool usePlayerRegenMods)
	{
		this.Initialise(regenAmount, usePlayerRegenMods);
	}

	// Token: 0x06005BCF RID: 23503 RVA: 0x0003251D File Offset: 0x0003071D
	public void Initialise(float regenAmount, bool usePlayerRegenMods)
	{
		this.RegenAmount = regenAmount;
		this.UsePlayerRegenMods = usePlayerRegenMods;
	}

	// Token: 0x17001E6E RID: 7790
	// (get) Token: 0x06005BD0 RID: 23504 RVA: 0x0003252D File Offset: 0x0003072D
	// (set) Token: 0x06005BD1 RID: 23505 RVA: 0x00032535 File Offset: 0x00030735
	public float RegenAmount { get; private set; }

	// Token: 0x17001E6F RID: 7791
	// (get) Token: 0x06005BD2 RID: 23506 RVA: 0x0003253E File Offset: 0x0003073E
	// (set) Token: 0x06005BD3 RID: 23507 RVA: 0x00032546 File Offset: 0x00030746
	public bool UsePlayerRegenMods { get; private set; }
}
