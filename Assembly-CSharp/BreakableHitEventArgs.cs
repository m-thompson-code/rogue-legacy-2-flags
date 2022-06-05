using System;

// Token: 0x020007E3 RID: 2019
public class BreakableHitEventArgs : EventArgs
{
	// Token: 0x170016CF RID: 5839
	// (get) Token: 0x06004360 RID: 17248 RVA: 0x000EC72E File Offset: 0x000EA92E
	// (set) Token: 0x06004361 RID: 17249 RVA: 0x000EC736 File Offset: 0x000EA936
	public Breakable Breakable { get; private set; }

	// Token: 0x170016D0 RID: 5840
	// (get) Token: 0x06004362 RID: 17250 RVA: 0x000EC73F File Offset: 0x000EA93F
	// (set) Token: 0x06004363 RID: 17251 RVA: 0x000EC747 File Offset: 0x000EA947
	public IDamageObj DamageObject { get; private set; }

	// Token: 0x06004364 RID: 17252 RVA: 0x000EC750 File Offset: 0x000EA950
	public BreakableHitEventArgs(Breakable breakable, IDamageObj damageObject)
	{
		this.Initialize(breakable, damageObject);
	}

	// Token: 0x06004365 RID: 17253 RVA: 0x000EC760 File Offset: 0x000EA960
	public void Initialize(Breakable breakable, IDamageObj damageObject)
	{
		this.Breakable = breakable;
		this.DamageObject = damageObject;
	}
}
