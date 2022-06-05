using System;

// Token: 0x020005B6 RID: 1462
public class TurretHazardArgs : HazardArgs
{
	// Token: 0x06003648 RID: 13896 RVA: 0x000BC28C File Offset: 0x000BA48C
	public TurretHazardArgs(StateID initialState, TurretLogicType logicType, bool useHalfLoopDelay, float initialFireDelay, float loopFireDelay, float projectileSpeedMod) : base(initialState)
	{
		this.UseHalfLoopDelay = useHalfLoopDelay;
		this.LogicType = logicType;
		this.InitialFireDelay = initialFireDelay;
		this.LoopFireDelay = loopFireDelay;
		this.ProjectileSpeedMod = projectileSpeedMod;
	}

	// Token: 0x1700132B RID: 4907
	// (get) Token: 0x06003649 RID: 13897 RVA: 0x000BC2BB File Offset: 0x000BA4BB
	// (set) Token: 0x0600364A RID: 13898 RVA: 0x000BC2C3 File Offset: 0x000BA4C3
	public TurretLogicType LogicType { get; private set; }

	// Token: 0x1700132C RID: 4908
	// (get) Token: 0x0600364B RID: 13899 RVA: 0x000BC2CC File Offset: 0x000BA4CC
	// (set) Token: 0x0600364C RID: 13900 RVA: 0x000BC2D4 File Offset: 0x000BA4D4
	public float InitialFireDelay { get; private set; }

	// Token: 0x1700132D RID: 4909
	// (get) Token: 0x0600364D RID: 13901 RVA: 0x000BC2DD File Offset: 0x000BA4DD
	// (set) Token: 0x0600364E RID: 13902 RVA: 0x000BC2E5 File Offset: 0x000BA4E5
	public float LoopFireDelay { get; private set; }

	// Token: 0x1700132E RID: 4910
	// (get) Token: 0x0600364F RID: 13903 RVA: 0x000BC2EE File Offset: 0x000BA4EE
	// (set) Token: 0x06003650 RID: 13904 RVA: 0x000BC2F6 File Offset: 0x000BA4F6
	public float ProjectileSpeedMod { get; private set; }

	// Token: 0x1700132F RID: 4911
	// (get) Token: 0x06003651 RID: 13905 RVA: 0x000BC2FF File Offset: 0x000BA4FF
	// (set) Token: 0x06003652 RID: 13906 RVA: 0x000BC307 File Offset: 0x000BA507
	public bool UseHalfLoopDelay { get; private set; }
}
