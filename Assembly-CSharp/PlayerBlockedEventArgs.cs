using System;

// Token: 0x020007B6 RID: 1974
public class PlayerBlockedEventArgs : EventArgs
{
	// Token: 0x06004267 RID: 16999 RVA: 0x000EBC73 File Offset: 0x000E9E73
	public PlayerBlockedEventArgs(IDamageObj damageObj)
	{
		this.Initialize(damageObj);
	}

	// Token: 0x06004268 RID: 17000 RVA: 0x000EBC82 File Offset: 0x000E9E82
	public void Initialize(IDamageObj damageObj)
	{
		this.DamageObj = damageObj;
	}

	// Token: 0x1700167C RID: 5756
	// (get) Token: 0x06004269 RID: 17001 RVA: 0x000EBC8B File Offset: 0x000E9E8B
	// (set) Token: 0x0600426A RID: 17002 RVA: 0x000EBC93 File Offset: 0x000E9E93
	public IDamageObj DamageObj { get; private set; }
}
