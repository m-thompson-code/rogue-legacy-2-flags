using System;

// Token: 0x020000D6 RID: 214
public class FlyingBurst_Expert_AIScript : FlyingBurst_Basic_AIScript
{
	// Token: 0x1700031B RID: 795
	// (get) Token: 0x06000636 RID: 1590 RVA: 0x0001902B File Offset: 0x0001722B
	protected override float MeleeFireSpread
	{
		get
		{
			return 180f;
		}
	}

	// Token: 0x1700031C RID: 796
	// (get) Token: 0x06000637 RID: 1591 RVA: 0x00019032 File Offset: 0x00017232
	protected override int NumFireballsMelee
	{
		get
		{
			return 6;
		}
	}

	// Token: 0x1700031D RID: 797
	// (get) Token: 0x06000638 RID: 1592 RVA: 0x00019035 File Offset: 0x00017235
	protected override int NumFireballsBasic
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x1700031E RID: 798
	// (get) Token: 0x06000639 RID: 1593 RVA: 0x00019038 File Offset: 0x00017238
	protected override int m_bigShake_NumFireballsBasic
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x1700031F RID: 799
	// (get) Token: 0x0600063A RID: 1594 RVA: 0x0001903B File Offset: 0x0001723B
	protected override float m_bigShake_BasicFireSpread
	{
		get
		{
			return 50f;
		}
	}
}
