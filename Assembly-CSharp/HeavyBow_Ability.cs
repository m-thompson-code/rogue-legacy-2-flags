using System;
using UnityEngine;

// Token: 0x0200019A RID: 410
public class HeavyBow_Ability : Bow_Ability
{
	// Token: 0x17000885 RID: 2181
	// (get) Token: 0x06000F7D RID: 3965 RVA: 0x0002DA00 File Offset: 0x0002BC00
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 0.9f;
		}
	}

	// Token: 0x17000886 RID: 2182
	// (get) Token: 0x06000F7E RID: 3966 RVA: 0x0002DA07 File Offset: 0x0002BC07
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000887 RID: 2183
	// (get) Token: 0x06000F7F RID: 3967 RVA: 0x0002DA0E File Offset: 0x0002BC0E
	protected override float TellAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000888 RID: 2184
	// (get) Token: 0x06000F80 RID: 3968 RVA: 0x0002DA15 File Offset: 0x0002BC15
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000889 RID: 2185
	// (get) Token: 0x06000F81 RID: 3969 RVA: 0x0002DA1C File Offset: 0x0002BC1C
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700088A RID: 2186
	// (get) Token: 0x06000F82 RID: 3970 RVA: 0x0002DA23 File Offset: 0x0002BC23
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700088B RID: 2187
	// (get) Token: 0x06000F83 RID: 3971 RVA: 0x0002DA2A File Offset: 0x0002BC2A
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700088C RID: 2188
	// (get) Token: 0x06000F84 RID: 3972 RVA: 0x0002DA31 File Offset: 0x0002BC31
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x1700088D RID: 2189
	// (get) Token: 0x06000F85 RID: 3973 RVA: 0x0002DA38 File Offset: 0x0002BC38
	protected override float ExitAnimSpeed
	{
		get
		{
			return 0.8f;
		}
	}

	// Token: 0x1700088E RID: 2190
	// (get) Token: 0x06000F86 RID: 3974 RVA: 0x0002DA3F File Offset: 0x0002BC3F
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700088F RID: 2191
	// (get) Token: 0x06000F87 RID: 3975 RVA: 0x0002DA46 File Offset: 0x0002BC46
	protected override float GravityReduction
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000890 RID: 2192
	// (get) Token: 0x06000F88 RID: 3976 RVA: 0x0002DA4D File Offset: 0x0002BC4D
	protected override Vector2 BowPushbackAmount
	{
		get
		{
			return new Vector2(17f, 18f);
		}
	}
}
