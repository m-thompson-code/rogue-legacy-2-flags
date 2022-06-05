using System;

// Token: 0x020007B9 RID: 1977
public class EnemySummonedEventArgs : EventArgs
{
	// Token: 0x06004279 RID: 17017 RVA: 0x000EBD39 File Offset: 0x000E9F39
	public EnemySummonedEventArgs(EnemyController summonedEnemy, ISummoner summoner)
	{
		this.Initialize(summonedEnemy, summoner);
	}

	// Token: 0x0600427A RID: 17018 RVA: 0x000EBD49 File Offset: 0x000E9F49
	public void Initialize(EnemyController summonedEnemy, ISummoner summoner)
	{
		this.SummonedEnemy = summonedEnemy;
		this.Summoner = summoner;
	}

	// Token: 0x17001682 RID: 5762
	// (get) Token: 0x0600427B RID: 17019 RVA: 0x000EBD59 File Offset: 0x000E9F59
	// (set) Token: 0x0600427C RID: 17020 RVA: 0x000EBD61 File Offset: 0x000E9F61
	public ISummoner Summoner { get; private set; }

	// Token: 0x17001683 RID: 5763
	// (get) Token: 0x0600427D RID: 17021 RVA: 0x000EBD6A File Offset: 0x000E9F6A
	// (set) Token: 0x0600427E RID: 17022 RVA: 0x000EBD72 File Offset: 0x000E9F72
	public EnemyController SummonedEnemy { get; private set; }
}
