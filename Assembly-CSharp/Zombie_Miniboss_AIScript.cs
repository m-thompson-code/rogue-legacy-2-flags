using System;
using UnityEngine;

// Token: 0x0200027E RID: 638
public class Zombie_Miniboss_AIScript : Zombie_Basic_AIScript
{
	// Token: 0x170008B2 RID: 2226
	// (get) Token: 0x06001275 RID: 4725 RVA: 0x00006F6D File Offset: 0x0000516D
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.75f, 1f);
		}
	}

	// Token: 0x170008B3 RID: 2227
	// (get) Token: 0x06001276 RID: 4726 RVA: 0x00006F6D File Offset: 0x0000516D
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.75f, 1f);
		}
	}

	// Token: 0x170008B4 RID: 2228
	// (get) Token: 0x06001277 RID: 4727 RVA: 0x000052A9 File Offset: 0x000034A9
	protected override float DigDownAnimSpeed
	{
		get
		{
			return 0.9f;
		}
	}

	// Token: 0x170008B5 RID: 2229
	// (get) Token: 0x06001278 RID: 4728 RVA: 0x000052A9 File Offset: 0x000034A9
	protected override float DigUpAnimSpeed
	{
		get
		{
			return 0.9f;
		}
	}

	// Token: 0x170008B6 RID: 2230
	// (get) Token: 0x06001279 RID: 4729 RVA: 0x000096A5 File Offset: 0x000078A5
	protected override Vector2 swing_Dash_AttackSpeed
	{
		get
		{
			return new Vector2(17f, 0f);
		}
	}

	// Token: 0x170008B7 RID: 2231
	// (get) Token: 0x0600127A RID: 4730 RVA: 0x00004A00 File Offset: 0x00002C00
	protected override float m_swing_Dash_AttackTime
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170008B8 RID: 2232
	// (get) Token: 0x0600127B RID: 4731 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float m_swing_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008B9 RID: 2233
	// (get) Token: 0x0600127C RID: 4732 RVA: 0x000096B6 File Offset: 0x000078B6
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(-15f, 15f);
		}
	}

	// Token: 0x170008BA RID: 2234
	// (get) Token: 0x0600127D RID: 4733 RVA: 0x000096C7 File Offset: 0x000078C7
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(-9f, 9f);
		}
	}
}
