using System;

// Token: 0x0200038A RID: 906
public class BossObjectiveCompleteHUDEventArgs : ObjectiveCompleteHUDEventArgs
{
	// Token: 0x060021DB RID: 8667 RVA: 0x0006B8DF File Offset: 0x00069ADF
	public BossObjectiveCompleteHUDEventArgs(EnemyType enemyType, EnemyRank enemyRank, float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null) : base(ObjectiveCompleteHUDType.Boss, displayDuration, null, null, null)
	{
		this.Initialize(enemyType, enemyRank, displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
	}

	// Token: 0x060021DC RID: 8668 RVA: 0x0006B8FB File Offset: 0x00069AFB
	public void Initialize(EnemyType enemyType, EnemyRank enemyRank, float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null)
	{
		base.Initialize(ObjectiveCompleteHUDType.Boss, displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
		this.EnemyType = enemyType;
		this.EnemyRank = enemyRank;
	}

	// Token: 0x17000E2E RID: 3630
	// (get) Token: 0x060021DD RID: 8669 RVA: 0x0006B919 File Offset: 0x00069B19
	// (set) Token: 0x060021DE RID: 8670 RVA: 0x0006B921 File Offset: 0x00069B21
	public EnemyType EnemyType { get; private set; }

	// Token: 0x17000E2F RID: 3631
	// (get) Token: 0x060021DF RID: 8671 RVA: 0x0006B92A File Offset: 0x00069B2A
	// (set) Token: 0x060021E0 RID: 8672 RVA: 0x0006B932 File Offset: 0x00069B32
	public EnemyRank EnemyRank { get; private set; }
}
