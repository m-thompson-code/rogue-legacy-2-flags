using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002F1 RID: 753
public class GroundBow_Ability : Bow_Ability
{
	// Token: 0x17000B15 RID: 2837
	// (get) Token: 0x06001748 RID: 5960 RVA: 0x00006CC8 File Offset: 0x00004EC8
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 0.7f;
		}
	}

	// Token: 0x17000B16 RID: 2838
	// (get) Token: 0x06001749 RID: 5961 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B17 RID: 2839
	// (get) Token: 0x0600174A RID: 5962 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float TellAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000B18 RID: 2840
	// (get) Token: 0x0600174B RID: 5963 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B19 RID: 2841
	// (get) Token: 0x0600174C RID: 5964 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000B1A RID: 2842
	// (get) Token: 0x0600174D RID: 5965 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B1B RID: 2843
	// (get) Token: 0x0600174E RID: 5966 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000B1C RID: 2844
	// (get) Token: 0x0600174F RID: 5967 RVA: 0x00004A00 File Offset: 0x00002C00
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x17000B1D RID: 2845
	// (get) Token: 0x06001750 RID: 5968 RVA: 0x00004FDE File Offset: 0x000031DE
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1.15f;
		}
	}

	// Token: 0x17000B1E RID: 2846
	// (get) Token: 0x06001751 RID: 5969 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B1F RID: 2847
	// (get) Token: 0x06001752 RID: 5970 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float GravityReduction
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000B20 RID: 2848
	// (get) Token: 0x06001753 RID: 5971 RVA: 0x0000BCEF File Offset: 0x00009EEF
	protected override Vector2 BowPushbackAmount
	{
		get
		{
			return new Vector2(17f, 18f);
		}
	}

	// Token: 0x06001754 RID: 5972 RVA: 0x0000BD00 File Offset: 0x00009F00
	public override void PreCastAbility()
	{
		this.m_onGround = PlayerManager.GetPlayerController().IsGrounded;
		if (!this.m_onGround)
		{
			this.StopAbility(true);
			return;
		}
		base.PreCastAbility();
	}

	// Token: 0x06001755 RID: 5973 RVA: 0x0000BD28 File Offset: 0x00009F28
	public override IEnumerator CastAbility()
	{
		if (this.m_onGround)
		{
			yield return base.CastAbility();
			yield break;
		}
		yield break;
	}

	// Token: 0x04001724 RID: 5924
	private bool m_onGround;
}
