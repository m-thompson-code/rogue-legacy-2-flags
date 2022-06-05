using System;

// Token: 0x020007BC RID: 1980
public class EnemyActivationStateChangedEventArgs : EventArgs
{
	// Token: 0x06004289 RID: 17033 RVA: 0x000EBDE6 File Offset: 0x000E9FE6
	public EnemyActivationStateChangedEventArgs(EnemyController enemy)
	{
		this.Initialize(enemy);
	}

	// Token: 0x0600428A RID: 17034 RVA: 0x000EBDF5 File Offset: 0x000E9FF5
	public void Initialize(EnemyController enemy)
	{
		this.Enemy = enemy;
	}

	// Token: 0x17001687 RID: 5767
	// (get) Token: 0x0600428B RID: 17035 RVA: 0x000EBDFE File Offset: 0x000E9FFE
	// (set) Token: 0x0600428C RID: 17036 RVA: 0x000EBE06 File Offset: 0x000EA006
	public EnemyController Enemy { get; private set; }
}
