using System;

// Token: 0x020000D4 RID: 212
public class FlyingBurst_Advanced_AIScript : FlyingBurst_Basic_AIScript
{
	// Token: 0x1700030A RID: 778
	// (get) Token: 0x06000620 RID: 1568 RVA: 0x00018E71 File Offset: 0x00017071
	protected override float MeleeFireSpread
	{
		get
		{
			return 180f;
		}
	}

	// Token: 0x1700030B RID: 779
	// (get) Token: 0x06000621 RID: 1569 RVA: 0x00018E78 File Offset: 0x00017078
	protected override int NumFireballsMelee
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x1700030C RID: 780
	// (get) Token: 0x06000622 RID: 1570 RVA: 0x00018E7B File Offset: 0x0001707B
	protected override int NumFireballsBasic
	{
		get
		{
			return 0;
		}
	}
}
