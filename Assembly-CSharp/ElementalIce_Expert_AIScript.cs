using System;

// Token: 0x0200010B RID: 267
public class ElementalIce_Expert_AIScript : ElementalIce_Basic_AIScript
{
	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x06000661 RID: 1633 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_shoot_CentreShot
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x06000662 RID: 1634 RVA: 0x00004A8D File Offset: 0x00002C8D
	protected override int m_shoot_TotalSideShots
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x170002AA RID: 682
	// (get) Token: 0x06000663 RID: 1635 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected override float m_spinAttack_TimesShotDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x170002AB RID: 683
	// (get) Token: 0x06000664 RID: 1636 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override int m_spinAttack_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170002AC RID: 684
	// (get) Token: 0x06000665 RID: 1637 RVA: 0x00003E42 File Offset: 0x00002042
	protected override int m_spinAttack_ShotPatternLoops
	{
		get
		{
			return 7;
		}
	}

	// Token: 0x170002AD RID: 685
	// (get) Token: 0x06000666 RID: 1638 RVA: 0x000053A7 File Offset: 0x000035A7
	protected override float m_spinAttack_projectile_RandomSpread
	{
		get
		{
			return 180f;
		}
	}
}
