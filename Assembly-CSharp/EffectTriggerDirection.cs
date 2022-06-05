using System;

// Token: 0x020006CC RID: 1740
[Flags]
public enum EffectTriggerDirection
{
	// Token: 0x04002B80 RID: 11136
	None = 0,
	// Token: 0x04002B81 RID: 11137
	MovingLeft = 2,
	// Token: 0x04002B82 RID: 11138
	MovingRight = 4,
	// Token: 0x04002B83 RID: 11139
	MovingDown = 8,
	// Token: 0x04002B84 RID: 11140
	MovingUp = 16,
	// Token: 0x04002B85 RID: 11141
	StandingOn = 32,
	// Token: 0x04002B86 RID: 11142
	Anywhere = -1
}
