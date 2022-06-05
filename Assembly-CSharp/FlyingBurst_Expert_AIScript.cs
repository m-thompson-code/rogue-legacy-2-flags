using System;

// Token: 0x02000158 RID: 344
public class FlyingBurst_Expert_AIScript : FlyingBurst_Basic_AIScript
{
	// Token: 0x1700040F RID: 1039
	// (get) Token: 0x06000917 RID: 2327 RVA: 0x000053A7 File Offset: 0x000035A7
	protected override float MeleeFireSpread
	{
		get
		{
			return 180f;
		}
	}

	// Token: 0x17000410 RID: 1040
	// (get) Token: 0x06000918 RID: 2328 RVA: 0x00004792 File Offset: 0x00002992
	protected override int NumFireballsMelee
	{
		get
		{
			return 6;
		}
	}

	// Token: 0x17000411 RID: 1041
	// (get) Token: 0x06000919 RID: 2329 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected override int NumFireballsBasic
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x17000412 RID: 1042
	// (get) Token: 0x0600091A RID: 2330 RVA: 0x00004A8D File Offset: 0x00002C8D
	protected override int m_bigShake_NumFireballsBasic
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x17000413 RID: 1043
	// (get) Token: 0x0600091B RID: 2331 RVA: 0x000063AC File Offset: 0x000045AC
	protected override float m_bigShake_BasicFireSpread
	{
		get
		{
			return 50f;
		}
	}
}
