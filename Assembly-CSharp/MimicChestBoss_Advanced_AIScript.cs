using System;
using UnityEngine;

// Token: 0x020000F5 RID: 245
public class MimicChestBoss_Advanced_AIScript : MimicChestBoss_Basic_AIScript
{
	// Token: 0x170003FF RID: 1023
	// (get) Token: 0x06000777 RID: 1911 RVA: 0x0001A85B File Offset: 0x00018A5B
	protected override Vector2 JumpHeight
	{
		get
		{
			return new Vector2(28f, 35f);
		}
	}

	// Token: 0x17000400 RID: 1024
	// (get) Token: 0x06000778 RID: 1912 RVA: 0x0001A86C File Offset: 0x00018A6C
	protected override int NumCoinsFiredOnLanding
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000401 RID: 1025
	// (get) Token: 0x06000779 RID: 1913 RVA: 0x0001A86F File Offset: 0x00018A6F
	protected override bool m_advancedBoss
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000402 RID: 1026
	// (get) Token: 0x0600077A RID: 1914 RVA: 0x0001A872 File Offset: 0x00018A72
	protected override int m_verticalShot_TotalShotSpread
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x17000403 RID: 1027
	// (get) Token: 0x0600077B RID: 1915 RVA: 0x0001A875 File Offset: 0x00018A75
	protected override int m_verticalShot_TotalLoops
	{
		get
		{
			return 8;
		}
	}

	// Token: 0x17000404 RID: 1028
	// (get) Token: 0x0600077C RID: 1916 RVA: 0x0001A878 File Offset: 0x00018A78
	protected override int m_verticalShot_InitialAngle
	{
		get
		{
			return 90;
		}
	}

	// Token: 0x17000405 RID: 1029
	// (get) Token: 0x0600077D RID: 1917 RVA: 0x0001A87C File Offset: 0x00018A7C
	protected override Vector2 m_verticalShot_RandomAngleAngleOffset
	{
		get
		{
			return new Vector2(-15f, 6f);
		}
	}

	// Token: 0x17000406 RID: 1030
	// (get) Token: 0x0600077E RID: 1918 RVA: 0x0001A88D File Offset: 0x00018A8D
	protected override float m_verticalShot_LoopDelay
	{
		get
		{
			return 0.7f;
		}
	}

	// Token: 0x17000407 RID: 1031
	// (get) Token: 0x0600077F RID: 1919 RVA: 0x0001A894 File Offset: 0x00018A94
	protected override float m_verticalShot_SpeedMod
	{
		get
		{
			return 1.15f;
		}
	}
}
