using System;

// Token: 0x02000C84 RID: 3204
public class CooldownEventArgs : EventArgs
{
	// Token: 0x06005C1C RID: 23580 RVA: 0x00032867 File Offset: 0x00030A67
	public CooldownEventArgs(ICooldown cooldown)
	{
		this.Initialize(cooldown);
	}

	// Token: 0x06005C1D RID: 23581 RVA: 0x00032876 File Offset: 0x00030A76
	public void Initialize(ICooldown cooldown)
	{
		this.Cooldown = cooldown;
	}

	// Token: 0x17001E88 RID: 7816
	// (get) Token: 0x06005C1E RID: 23582 RVA: 0x0003287F File Offset: 0x00030A7F
	// (set) Token: 0x06005C1F RID: 23583 RVA: 0x00032887 File Offset: 0x00030A87
	public ICooldown Cooldown { get; private set; }
}
