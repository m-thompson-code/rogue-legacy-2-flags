using System;

// Token: 0x0200020A RID: 522
public class Starburst_Advanced_AIScript : Starburst_Basic_AIScript
{
	// Token: 0x170006AF RID: 1711
	// (get) Token: 0x06000E5F RID: 3679 RVA: 0x00004792 File Offset: 0x00002992
	protected override int NumberOfShots
	{
		get
		{
			return 6;
		}
	}

	// Token: 0x170006B0 RID: 1712
	// (get) Token: 0x06000E60 RID: 3680 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool AlternateShots
	{
		get
		{
			return true;
		}
	}
}
