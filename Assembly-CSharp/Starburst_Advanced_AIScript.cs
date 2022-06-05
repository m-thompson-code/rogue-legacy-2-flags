using System;

// Token: 0x0200012D RID: 301
public class Starburst_Advanced_AIScript : Starburst_Basic_AIScript
{
	// Token: 0x17000505 RID: 1285
	// (get) Token: 0x0600095C RID: 2396 RVA: 0x0001EB21 File Offset: 0x0001CD21
	protected override int NumberOfShots
	{
		get
		{
			return 6;
		}
	}

	// Token: 0x17000506 RID: 1286
	// (get) Token: 0x0600095D RID: 2397 RVA: 0x0001EB24 File Offset: 0x0001CD24
	protected override bool AlternateShots
	{
		get
		{
			return true;
		}
	}
}
