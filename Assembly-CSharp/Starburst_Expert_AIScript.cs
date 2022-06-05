using System;

// Token: 0x0200012F RID: 303
public class Starburst_Expert_AIScript : Starburst_Basic_AIScript
{
	// Token: 0x1700050E RID: 1294
	// (get) Token: 0x0600096B RID: 2411 RVA: 0x0001ECAC File Offset: 0x0001CEAC
	protected override int NumberOfShots
	{
		get
		{
			return 6;
		}
	}

	// Token: 0x1700050F RID: 1295
	// (get) Token: 0x0600096C RID: 2412 RVA: 0x0001ECAF File Offset: 0x0001CEAF
	protected override bool AlternateShots
	{
		get
		{
			return true;
		}
	}
}
