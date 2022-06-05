using System;

// Token: 0x020007BB RID: 1979
public class EnemyModeShiftEventArgs : EventArgs
{
	// Token: 0x06004285 RID: 17029 RVA: 0x000EBDBD File Offset: 0x000E9FBD
	public EnemyModeShiftEventArgs(EnemyController enemy)
	{
		this.Initialize(enemy);
	}

	// Token: 0x06004286 RID: 17030 RVA: 0x000EBDCC File Offset: 0x000E9FCC
	public void Initialize(EnemyController enemy)
	{
		this.Enemy = enemy;
	}

	// Token: 0x17001686 RID: 5766
	// (get) Token: 0x06004287 RID: 17031 RVA: 0x000EBDD5 File Offset: 0x000E9FD5
	// (set) Token: 0x06004288 RID: 17032 RVA: 0x000EBDDD File Offset: 0x000E9FDD
	public EnemyController Enemy { get; private set; }
}
