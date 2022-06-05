using System;

// Token: 0x020007B3 RID: 1971
public class HealthChangeEventArgs : EventArgs
{
	// Token: 0x06004251 RID: 16977 RVA: 0x000EBB7B File Offset: 0x000E9D7B
	public HealthChangeEventArgs(IHealth healthObj, float newHealth, float prevHealth)
	{
		this.Initialise(healthObj, newHealth, prevHealth);
	}

	// Token: 0x06004252 RID: 16978 RVA: 0x000EBB8C File Offset: 0x000E9D8C
	public void Initialise(IHealth healthObj, float newHealth, float prevHealth)
	{
		this.HealthObj = healthObj;
		this.NewHealthValue = newHealth;
		this.PrevHealthValue = prevHealth;
	}

	// Token: 0x17001674 RID: 5748
	// (get) Token: 0x06004253 RID: 16979 RVA: 0x000EBBA3 File Offset: 0x000E9DA3
	// (set) Token: 0x06004254 RID: 16980 RVA: 0x000EBBAB File Offset: 0x000E9DAB
	public IHealth HealthObj { get; private set; }

	// Token: 0x17001675 RID: 5749
	// (get) Token: 0x06004255 RID: 16981 RVA: 0x000EBBB4 File Offset: 0x000E9DB4
	// (set) Token: 0x06004256 RID: 16982 RVA: 0x000EBBBC File Offset: 0x000E9DBC
	public float NewHealthValue { get; private set; }

	// Token: 0x17001676 RID: 5750
	// (get) Token: 0x06004257 RID: 16983 RVA: 0x000EBBC5 File Offset: 0x000E9DC5
	// (set) Token: 0x06004258 RID: 16984 RVA: 0x000EBBCD File Offset: 0x000E9DCD
	public float PrevHealthValue { get; private set; }
}
