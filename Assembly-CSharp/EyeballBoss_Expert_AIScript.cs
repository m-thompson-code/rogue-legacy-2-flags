using System;

// Token: 0x020000BD RID: 189
public class EyeballBoss_Expert_AIScript : EyeballBoss_Basic_AIScript
{
	// Token: 0x17000237 RID: 567
	// (get) Token: 0x060004DA RID: 1242 RVA: 0x00016675 File Offset: 0x00014875
	protected override bool ZoneBlast_Variant
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000238 RID: 568
	// (get) Token: 0x060004DB RID: 1243 RVA: 0x00016678 File Offset: 0x00014878
	protected override bool SpinningFireball_Variant
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000239 RID: 569
	// (get) Token: 0x060004DC RID: 1244 RVA: 0x0001667B File Offset: 0x0001487B
	protected override bool HomingFireball_Variant
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700023A RID: 570
	// (get) Token: 0x060004DD RID: 1245 RVA: 0x0001667E File Offset: 0x0001487E
	protected override bool HomingFireball_Variant_Blue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700023B RID: 571
	// (get) Token: 0x060004DE RID: 1246 RVA: 0x00016681 File Offset: 0x00014881
	protected override float MultiShot_Exit_AttackCD
	{
		get
		{
			return 2.5f;
		}
	}
}
