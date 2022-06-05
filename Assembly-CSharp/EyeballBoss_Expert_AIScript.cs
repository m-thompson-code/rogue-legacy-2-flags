using System;

// Token: 0x02000117 RID: 279
public class EyeballBoss_Expert_AIScript : EyeballBoss_Basic_AIScript
{
	// Token: 0x170002DD RID: 733
	// (get) Token: 0x060006D1 RID: 1745 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool ZoneBlast_Variant
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002DE RID: 734
	// (get) Token: 0x060006D2 RID: 1746 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool SpinningFireball_Variant
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002DF RID: 735
	// (get) Token: 0x060006D3 RID: 1747 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool HomingFireball_Variant
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002E0 RID: 736
	// (get) Token: 0x060006D4 RID: 1748 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool HomingFireball_Variant_Blue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002E1 RID: 737
	// (get) Token: 0x060006D5 RID: 1749 RVA: 0x00005319 File Offset: 0x00003519
	protected override float MultiShot_Exit_AttackCD
	{
		get
		{
			return 2.5f;
		}
	}
}
