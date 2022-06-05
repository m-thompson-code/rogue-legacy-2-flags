using System;

// Token: 0x020007B4 RID: 1972
public class MaxHealthChangeEventArgs : EventArgs
{
	// Token: 0x06004259 RID: 16985 RVA: 0x000EBBD6 File Offset: 0x000E9DD6
	public MaxHealthChangeEventArgs(float newMaxHealth, float prevMaxHealth)
	{
		this.Initialise(newMaxHealth, prevMaxHealth);
	}

	// Token: 0x0600425A RID: 16986 RVA: 0x000EBBE6 File Offset: 0x000E9DE6
	public void Initialise(float newMaxHealth, float prevMaxHealth)
	{
		this.NewMaxHealthValue = newMaxHealth;
		this.PrevMaxHealthValue = prevMaxHealth;
	}

	// Token: 0x17001677 RID: 5751
	// (get) Token: 0x0600425B RID: 16987 RVA: 0x000EBBF6 File Offset: 0x000E9DF6
	// (set) Token: 0x0600425C RID: 16988 RVA: 0x000EBBFE File Offset: 0x000E9DFE
	public float NewMaxHealthValue { get; private set; }

	// Token: 0x17001678 RID: 5752
	// (get) Token: 0x0600425D RID: 16989 RVA: 0x000EBC07 File Offset: 0x000E9E07
	// (set) Token: 0x0600425E RID: 16990 RVA: 0x000EBC0F File Offset: 0x000E9E0F
	public float PrevMaxHealthValue { get; private set; }
}
