using System;

// Token: 0x02000414 RID: 1044
[Flags]
public enum EffectTriggerDirection
{
	// Token: 0x0400209D RID: 8349
	None = 0,
	// Token: 0x0400209E RID: 8350
	MovingLeft = 2,
	// Token: 0x0400209F RID: 8351
	MovingRight = 4,
	// Token: 0x040020A0 RID: 8352
	MovingDown = 8,
	// Token: 0x040020A1 RID: 8353
	MovingUp = 16,
	// Token: 0x040020A2 RID: 8354
	StandingOn = 32,
	// Token: 0x040020A3 RID: 8355
	Anywhere = -1
}
