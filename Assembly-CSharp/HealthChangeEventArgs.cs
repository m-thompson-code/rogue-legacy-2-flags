using System;

// Token: 0x02000C79 RID: 3193
public class HealthChangeEventArgs : EventArgs
{
	// Token: 0x06005BDA RID: 23514 RVA: 0x00032591 File Offset: 0x00030791
	public HealthChangeEventArgs(IHealth healthObj, float newHealth, float prevHealth)
	{
		this.Initialise(healthObj, newHealth, prevHealth);
	}

	// Token: 0x06005BDB RID: 23515 RVA: 0x000325A2 File Offset: 0x000307A2
	public void Initialise(IHealth healthObj, float newHealth, float prevHealth)
	{
		this.HealthObj = healthObj;
		this.NewHealthValue = newHealth;
		this.PrevHealthValue = prevHealth;
	}

	// Token: 0x17001E72 RID: 7794
	// (get) Token: 0x06005BDC RID: 23516 RVA: 0x000325B9 File Offset: 0x000307B9
	// (set) Token: 0x06005BDD RID: 23517 RVA: 0x000325C1 File Offset: 0x000307C1
	public IHealth HealthObj { get; private set; }

	// Token: 0x17001E73 RID: 7795
	// (get) Token: 0x06005BDE RID: 23518 RVA: 0x000325CA File Offset: 0x000307CA
	// (set) Token: 0x06005BDF RID: 23519 RVA: 0x000325D2 File Offset: 0x000307D2
	public float NewHealthValue { get; private set; }

	// Token: 0x17001E74 RID: 7796
	// (get) Token: 0x06005BE0 RID: 23520 RVA: 0x000325DB File Offset: 0x000307DB
	// (set) Token: 0x06005BE1 RID: 23521 RVA: 0x000325E3 File Offset: 0x000307E3
	public float PrevHealthValue { get; private set; }
}
