using System;

// Token: 0x02000154 RID: 340
public class FlyingBurst_Advanced_AIScript : FlyingBurst_Basic_AIScript
{
	// Token: 0x170003FA RID: 1018
	// (get) Token: 0x060008F5 RID: 2293 RVA: 0x000053A7 File Offset: 0x000035A7
	protected override float MeleeFireSpread
	{
		get
		{
			return 180f;
		}
	}

	// Token: 0x170003FB RID: 1019
	// (get) Token: 0x060008F6 RID: 2294 RVA: 0x00004762 File Offset: 0x00002962
	protected override int NumFireballsMelee
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x170003FC RID: 1020
	// (get) Token: 0x060008F7 RID: 2295 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected override int NumFireballsBasic
	{
		get
		{
			return 0;
		}
	}
}
