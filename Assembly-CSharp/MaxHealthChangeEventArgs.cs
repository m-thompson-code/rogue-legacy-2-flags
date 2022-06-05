using System;

// Token: 0x02000C7A RID: 3194
public class MaxHealthChangeEventArgs : EventArgs
{
	// Token: 0x06005BE2 RID: 23522 RVA: 0x000325EC File Offset: 0x000307EC
	public MaxHealthChangeEventArgs(float newMaxHealth, float prevMaxHealth)
	{
		this.Initialise(newMaxHealth, prevMaxHealth);
	}

	// Token: 0x06005BE3 RID: 23523 RVA: 0x000325FC File Offset: 0x000307FC
	public void Initialise(float newMaxHealth, float prevMaxHealth)
	{
		this.NewMaxHealthValue = newMaxHealth;
		this.PrevMaxHealthValue = prevMaxHealth;
	}

	// Token: 0x17001E75 RID: 7797
	// (get) Token: 0x06005BE4 RID: 23524 RVA: 0x0003260C File Offset: 0x0003080C
	// (set) Token: 0x06005BE5 RID: 23525 RVA: 0x00032614 File Offset: 0x00030814
	public float NewMaxHealthValue { get; private set; }

	// Token: 0x17001E76 RID: 7798
	// (get) Token: 0x06005BE6 RID: 23526 RVA: 0x0003261D File Offset: 0x0003081D
	// (set) Token: 0x06005BE7 RID: 23527 RVA: 0x00032625 File Offset: 0x00030825
	public float PrevMaxHealthValue { get; private set; }
}
