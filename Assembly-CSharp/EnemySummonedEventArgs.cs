using System;

// Token: 0x02000C7F RID: 3199
public class EnemySummonedEventArgs : EventArgs
{
	// Token: 0x06005C02 RID: 23554 RVA: 0x0003274F File Offset: 0x0003094F
	public EnemySummonedEventArgs(EnemyController summonedEnemy, ISummoner summoner)
	{
		this.Initialize(summonedEnemy, summoner);
	}

	// Token: 0x06005C03 RID: 23555 RVA: 0x0003275F File Offset: 0x0003095F
	public void Initialize(EnemyController summonedEnemy, ISummoner summoner)
	{
		this.SummonedEnemy = summonedEnemy;
		this.Summoner = summoner;
	}

	// Token: 0x17001E80 RID: 7808
	// (get) Token: 0x06005C04 RID: 23556 RVA: 0x0003276F File Offset: 0x0003096F
	// (set) Token: 0x06005C05 RID: 23557 RVA: 0x00032777 File Offset: 0x00030977
	public ISummoner Summoner { get; private set; }

	// Token: 0x17001E81 RID: 7809
	// (get) Token: 0x06005C06 RID: 23558 RVA: 0x00032780 File Offset: 0x00030980
	// (set) Token: 0x06005C07 RID: 23559 RVA: 0x00032788 File Offset: 0x00030988
	public EnemyController SummonedEnemy { get; private set; }
}
