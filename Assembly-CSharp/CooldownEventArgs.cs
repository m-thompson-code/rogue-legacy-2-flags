using System;

// Token: 0x020007BE RID: 1982
public class CooldownEventArgs : EventArgs
{
	// Token: 0x06004293 RID: 17043 RVA: 0x000EBE51 File Offset: 0x000EA051
	public CooldownEventArgs(ICooldown cooldown)
	{
		this.Initialize(cooldown);
	}

	// Token: 0x06004294 RID: 17044 RVA: 0x000EBE60 File Offset: 0x000EA060
	public void Initialize(ICooldown cooldown)
	{
		this.Cooldown = cooldown;
	}

	// Token: 0x1700168A RID: 5770
	// (get) Token: 0x06004295 RID: 17045 RVA: 0x000EBE69 File Offset: 0x000EA069
	// (set) Token: 0x06004296 RID: 17046 RVA: 0x000EBE71 File Offset: 0x000EA071
	public ICooldown Cooldown { get; private set; }
}
