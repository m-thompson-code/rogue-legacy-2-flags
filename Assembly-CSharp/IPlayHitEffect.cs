using System;

// Token: 0x020009CD RID: 2509
public interface IPlayHitEffect
{
	// Token: 0x17001A6E RID: 6766
	// (get) Token: 0x06004C7F RID: 19583
	bool PlayDirectionalHitEffect { get; }

	// Token: 0x17001A6F RID: 6767
	// (get) Token: 0x06004C80 RID: 19584
	bool PlayHitEffect { get; }

	// Token: 0x17001A70 RID: 6768
	// (get) Token: 0x06004C81 RID: 19585
	string EffectNameOverride { get; }
}
