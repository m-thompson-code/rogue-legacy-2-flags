using System;

// Token: 0x020009BE RID: 2494
public class TurretHazardArgs : HazardArgs
{
	// Token: 0x06004C5A RID: 19546 RVA: 0x00029B69 File Offset: 0x00027D69
	public TurretHazardArgs(StateID initialState, TurretLogicType logicType, bool useHalfLoopDelay, float initialFireDelay, float loopFireDelay, float projectileSpeedMod) : base(initialState)
	{
		this.UseHalfLoopDelay = useHalfLoopDelay;
		this.LogicType = logicType;
		this.InitialFireDelay = initialFireDelay;
		this.LoopFireDelay = loopFireDelay;
		this.ProjectileSpeedMod = projectileSpeedMod;
	}

	// Token: 0x17001A58 RID: 6744
	// (get) Token: 0x06004C5B RID: 19547 RVA: 0x00029B98 File Offset: 0x00027D98
	// (set) Token: 0x06004C5C RID: 19548 RVA: 0x00029BA0 File Offset: 0x00027DA0
	public TurretLogicType LogicType { get; private set; }

	// Token: 0x17001A59 RID: 6745
	// (get) Token: 0x06004C5D RID: 19549 RVA: 0x00029BA9 File Offset: 0x00027DA9
	// (set) Token: 0x06004C5E RID: 19550 RVA: 0x00029BB1 File Offset: 0x00027DB1
	public float InitialFireDelay { get; private set; }

	// Token: 0x17001A5A RID: 6746
	// (get) Token: 0x06004C5F RID: 19551 RVA: 0x00029BBA File Offset: 0x00027DBA
	// (set) Token: 0x06004C60 RID: 19552 RVA: 0x00029BC2 File Offset: 0x00027DC2
	public float LoopFireDelay { get; private set; }

	// Token: 0x17001A5B RID: 6747
	// (get) Token: 0x06004C61 RID: 19553 RVA: 0x00029BCB File Offset: 0x00027DCB
	// (set) Token: 0x06004C62 RID: 19554 RVA: 0x00029BD3 File Offset: 0x00027DD3
	public float ProjectileSpeedMod { get; private set; }

	// Token: 0x17001A5C RID: 6748
	// (get) Token: 0x06004C63 RID: 19555 RVA: 0x00029BDC File Offset: 0x00027DDC
	// (set) Token: 0x06004C64 RID: 19556 RVA: 0x00029BE4 File Offset: 0x00027DE4
	public bool UseHalfLoopDelay { get; private set; }
}
