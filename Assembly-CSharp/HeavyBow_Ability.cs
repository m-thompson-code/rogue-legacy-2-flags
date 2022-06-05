using System;
using UnityEngine;

// Token: 0x020002F3 RID: 755
public class HeavyBow_Ability : Bow_Ability
{
	// Token: 0x17000B23 RID: 2851
	// (get) Token: 0x0600175E RID: 5982 RVA: 0x000052A9 File Offset: 0x000034A9
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 0.9f;
		}
	}

	// Token: 0x17000B24 RID: 2852
	// (get) Token: 0x0600175F RID: 5983 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B25 RID: 2853
	// (get) Token: 0x06001760 RID: 5984 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float TellAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000B26 RID: 2854
	// (get) Token: 0x06001761 RID: 5985 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B27 RID: 2855
	// (get) Token: 0x06001762 RID: 5986 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000B28 RID: 2856
	// (get) Token: 0x06001763 RID: 5987 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B29 RID: 2857
	// (get) Token: 0x06001764 RID: 5988 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000B2A RID: 2858
	// (get) Token: 0x06001765 RID: 5989 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000B2B RID: 2859
	// (get) Token: 0x06001766 RID: 5990 RVA: 0x0000566E File Offset: 0x0000386E
	protected override float ExitAnimSpeed
	{
		get
		{
			return 0.8f;
		}
	}

	// Token: 0x17000B2C RID: 2860
	// (get) Token: 0x06001767 RID: 5991 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B2D RID: 2861
	// (get) Token: 0x06001768 RID: 5992 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float GravityReduction
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B2E RID: 2862
	// (get) Token: 0x06001769 RID: 5993 RVA: 0x0000BCEF File Offset: 0x00009EEF
	protected override Vector2 BowPushbackAmount
	{
		get
		{
			return new Vector2(17f, 18f);
		}
	}
}
