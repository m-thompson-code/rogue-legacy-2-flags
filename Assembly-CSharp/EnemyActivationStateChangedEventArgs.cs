using System;

// Token: 0x02000C82 RID: 3202
public class EnemyActivationStateChangedEventArgs : EventArgs
{
	// Token: 0x06005C12 RID: 23570 RVA: 0x000327FC File Offset: 0x000309FC
	public EnemyActivationStateChangedEventArgs(EnemyController enemy)
	{
		this.Initialize(enemy);
	}

	// Token: 0x06005C13 RID: 23571 RVA: 0x0003280B File Offset: 0x00030A0B
	public void Initialize(EnemyController enemy)
	{
		this.Enemy = enemy;
	}

	// Token: 0x17001E85 RID: 7813
	// (get) Token: 0x06005C14 RID: 23572 RVA: 0x00032814 File Offset: 0x00030A14
	// (set) Token: 0x06005C15 RID: 23573 RVA: 0x0003281C File Offset: 0x00030A1C
	public EnemyController Enemy { get; private set; }
}
