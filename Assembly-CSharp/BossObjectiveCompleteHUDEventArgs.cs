using System;

// Token: 0x02000617 RID: 1559
public class BossObjectiveCompleteHUDEventArgs : ObjectiveCompleteHUDEventArgs
{
	// Token: 0x06002FE7 RID: 12263 RVA: 0x0001A3B2 File Offset: 0x000185B2
	public BossObjectiveCompleteHUDEventArgs(EnemyType enemyType, EnemyRank enemyRank, float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null) : base(ObjectiveCompleteHUDType.Boss, displayDuration, null, null, null)
	{
		this.Initialize(enemyType, enemyRank, displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
	}

	// Token: 0x06002FE8 RID: 12264 RVA: 0x0001A3CE File Offset: 0x000185CE
	public void Initialize(EnemyType enemyType, EnemyRank enemyRank, float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null)
	{
		base.Initialize(ObjectiveCompleteHUDType.Boss, displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
		this.EnemyType = enemyType;
		this.EnemyRank = enemyRank;
	}

	// Token: 0x170012BD RID: 4797
	// (get) Token: 0x06002FE9 RID: 12265 RVA: 0x0001A3EC File Offset: 0x000185EC
	// (set) Token: 0x06002FEA RID: 12266 RVA: 0x0001A3F4 File Offset: 0x000185F4
	public EnemyType EnemyType { get; private set; }

	// Token: 0x170012BE RID: 4798
	// (get) Token: 0x06002FEB RID: 12267 RVA: 0x0001A3FD File Offset: 0x000185FD
	// (set) Token: 0x06002FEC RID: 12268 RVA: 0x0001A405 File Offset: 0x00018605
	public EnemyRank EnemyRank { get; private set; }
}
