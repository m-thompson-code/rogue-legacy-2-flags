using System;

// Token: 0x02000CA9 RID: 3241
public class BreakableHitEventArgs : EventArgs
{
	// Token: 0x17001ECD RID: 7885
	// (get) Token: 0x06005CE9 RID: 23785 RVA: 0x00033144 File Offset: 0x00031344
	// (set) Token: 0x06005CEA RID: 23786 RVA: 0x0003314C File Offset: 0x0003134C
	public Breakable Breakable { get; private set; }

	// Token: 0x17001ECE RID: 7886
	// (get) Token: 0x06005CEB RID: 23787 RVA: 0x00033155 File Offset: 0x00031355
	// (set) Token: 0x06005CEC RID: 23788 RVA: 0x0003315D File Offset: 0x0003135D
	public IDamageObj DamageObject { get; private set; }

	// Token: 0x06005CED RID: 23789 RVA: 0x00033166 File Offset: 0x00031366
	public BreakableHitEventArgs(Breakable breakable, IDamageObj damageObject)
	{
		this.Initialize(breakable, damageObject);
	}

	// Token: 0x06005CEE RID: 23790 RVA: 0x00033176 File Offset: 0x00031376
	public void Initialize(Breakable breakable, IDamageObj damageObject)
	{
		this.Breakable = breakable;
		this.DamageObject = damageObject;
	}
}
