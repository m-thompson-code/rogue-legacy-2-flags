using System;

// Token: 0x0200020D RID: 525
public class Starburst_Expert_AIScript : Starburst_Basic_AIScript
{
	// Token: 0x170006BA RID: 1722
	// (get) Token: 0x06000E74 RID: 3700 RVA: 0x00004792 File Offset: 0x00002992
	protected override int NumberOfShots
	{
		get
		{
			return 6;
		}
	}

	// Token: 0x170006BB RID: 1723
	// (get) Token: 0x06000E75 RID: 3701 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool AlternateShots
	{
		get
		{
			return true;
		}
	}
}
