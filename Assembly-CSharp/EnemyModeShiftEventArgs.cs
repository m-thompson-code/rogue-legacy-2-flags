using System;

// Token: 0x02000C81 RID: 3201
public class EnemyModeShiftEventArgs : EventArgs
{
	// Token: 0x06005C0E RID: 23566 RVA: 0x000327D3 File Offset: 0x000309D3
	public EnemyModeShiftEventArgs(EnemyController enemy)
	{
		this.Initialize(enemy);
	}

	// Token: 0x06005C0F RID: 23567 RVA: 0x000327E2 File Offset: 0x000309E2
	public void Initialize(EnemyController enemy)
	{
		this.Enemy = enemy;
	}

	// Token: 0x17001E84 RID: 7812
	// (get) Token: 0x06005C10 RID: 23568 RVA: 0x000327EB File Offset: 0x000309EB
	// (set) Token: 0x06005C11 RID: 23569 RVA: 0x000327F3 File Offset: 0x000309F3
	public EnemyController Enemy { get; private set; }
}
