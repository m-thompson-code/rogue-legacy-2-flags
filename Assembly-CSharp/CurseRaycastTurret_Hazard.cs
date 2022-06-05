using System;
using UnityEngine;

// Token: 0x0200071D RID: 1821
public class CurseRaycastTurret_Hazard : RaycastTurret_Hazard
{
	// Token: 0x170014D6 RID: 5334
	// (get) Token: 0x060037AA RID: 14250 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected override bool AllTurretsFire
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170014D7 RID: 5335
	// (get) Token: 0x060037AB RID: 14251 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float InitializationDelay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170014D8 RID: 5336
	// (get) Token: 0x060037AC RID: 14252 RVA: 0x0000457A File Offset: 0x0000277A
	protected override float FireDelay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x170014D9 RID: 5337
	// (get) Token: 0x060037AD RID: 14253 RVA: 0x000081A4 File Offset: 0x000063A4
	protected override float DelayBetweenShots
	{
		get
		{
			return 8f;
		}
	}

	// Token: 0x170014DA RID: 5338
	// (get) Token: 0x060037AE RID: 14254 RVA: 0x00003DA4 File Offset: 0x00001FA4
	protected override float DetectedRange
	{
		get
		{
			return 15f;
		}
	}

	// Token: 0x170014DB RID: 5339
	// (get) Token: 0x060037AF RID: 14255 RVA: 0x0001E96F File Offset: 0x0001CB6F
	protected override Vector2 FireOffset
	{
		get
		{
			return new Vector2(0.5f, 0f);
		}
	}
}
