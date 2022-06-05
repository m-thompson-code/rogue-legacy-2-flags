using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000199 RID: 409
public class GroundBow_Ability : Bow_Ability
{
	// Token: 0x17000879 RID: 2169
	// (get) Token: 0x06000F6D RID: 3949 RVA: 0x0002D95B File Offset: 0x0002BB5B
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 0.7f;
		}
	}

	// Token: 0x1700087A RID: 2170
	// (get) Token: 0x06000F6E RID: 3950 RVA: 0x0002D962 File Offset: 0x0002BB62
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700087B RID: 2171
	// (get) Token: 0x06000F6F RID: 3951 RVA: 0x0002D969 File Offset: 0x0002BB69
	protected override float TellAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700087C RID: 2172
	// (get) Token: 0x06000F70 RID: 3952 RVA: 0x0002D970 File Offset: 0x0002BB70
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700087D RID: 2173
	// (get) Token: 0x06000F71 RID: 3953 RVA: 0x0002D977 File Offset: 0x0002BB77
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700087E RID: 2174
	// (get) Token: 0x06000F72 RID: 3954 RVA: 0x0002D97E File Offset: 0x0002BB7E
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700087F RID: 2175
	// (get) Token: 0x06000F73 RID: 3955 RVA: 0x0002D985 File Offset: 0x0002BB85
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000880 RID: 2176
	// (get) Token: 0x06000F74 RID: 3956 RVA: 0x0002D98C File Offset: 0x0002BB8C
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x17000881 RID: 2177
	// (get) Token: 0x06000F75 RID: 3957 RVA: 0x0002D993 File Offset: 0x0002BB93
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1.15f;
		}
	}

	// Token: 0x17000882 RID: 2178
	// (get) Token: 0x06000F76 RID: 3958 RVA: 0x0002D99A File Offset: 0x0002BB9A
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000883 RID: 2179
	// (get) Token: 0x06000F77 RID: 3959 RVA: 0x0002D9A1 File Offset: 0x0002BBA1
	protected override float GravityReduction
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000884 RID: 2180
	// (get) Token: 0x06000F78 RID: 3960 RVA: 0x0002D9A8 File Offset: 0x0002BBA8
	protected override Vector2 BowPushbackAmount
	{
		get
		{
			return new Vector2(17f, 18f);
		}
	}

	// Token: 0x06000F79 RID: 3961 RVA: 0x0002D9B9 File Offset: 0x0002BBB9
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

	// Token: 0x06000F7A RID: 3962 RVA: 0x0002D9E1 File Offset: 0x0002BBE1
	public override IEnumerator CastAbility()
	{
		if (this.m_onGround)
		{
			yield return base.CastAbility();
			yield break;
		}
		yield break;
	}

	// Token: 0x04001177 RID: 4471
	private bool m_onGround;
}
