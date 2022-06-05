using System;
using UnityEngine;

// Token: 0x02000449 RID: 1097
public class CurseRaycastTurret_Hazard : RaycastTurret_Hazard
{
	// Token: 0x17000FDD RID: 4061
	// (get) Token: 0x06002856 RID: 10326 RVA: 0x00085ABF File Offset: 0x00083CBF
	protected override bool AllTurretsFire
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000FDE RID: 4062
	// (get) Token: 0x06002857 RID: 10327 RVA: 0x00085AC2 File Offset: 0x00083CC2
	protected override float InitializationDelay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000FDF RID: 4063
	// (get) Token: 0x06002858 RID: 10328 RVA: 0x00085AC9 File Offset: 0x00083CC9
	protected override float FireDelay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x17000FE0 RID: 4064
	// (get) Token: 0x06002859 RID: 10329 RVA: 0x00085AD0 File Offset: 0x00083CD0
	protected override float DelayBetweenShots
	{
		get
		{
			return 8f;
		}
	}

	// Token: 0x17000FE1 RID: 4065
	// (get) Token: 0x0600285A RID: 10330 RVA: 0x00085AD7 File Offset: 0x00083CD7
	protected override float DetectedRange
	{
		get
		{
			return 15f;
		}
	}

	// Token: 0x17000FE2 RID: 4066
	// (get) Token: 0x0600285B RID: 10331 RVA: 0x00085ADE File Offset: 0x00083CDE
	protected override Vector2 FireOffset
	{
		get
		{
			return new Vector2(0.5f, 0f);
		}
	}
}
