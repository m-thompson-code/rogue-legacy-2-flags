using System;

// Token: 0x02000C7C RID: 3196
public class PlayerBlockedEventArgs : EventArgs
{
	// Token: 0x06005BF0 RID: 23536 RVA: 0x00032689 File Offset: 0x00030889
	public PlayerBlockedEventArgs(IDamageObj damageObj)
	{
		this.Initialize(damageObj);
	}

	// Token: 0x06005BF1 RID: 23537 RVA: 0x00032698 File Offset: 0x00030898
	public void Initialize(IDamageObj damageObj)
	{
		this.DamageObj = damageObj;
	}

	// Token: 0x17001E7A RID: 7802
	// (get) Token: 0x06005BF2 RID: 23538 RVA: 0x000326A1 File Offset: 0x000308A1
	// (set) Token: 0x06005BF3 RID: 23539 RVA: 0x000326A9 File Offset: 0x000308A9
	public IDamageObj DamageObj { get; private set; }
}
