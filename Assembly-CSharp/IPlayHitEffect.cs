using System;

// Token: 0x020005C5 RID: 1477
public interface IPlayHitEffect
{
	// Token: 0x17001341 RID: 4929
	// (get) Token: 0x0600366D RID: 13933
	bool PlayDirectionalHitEffect { get; }

	// Token: 0x17001342 RID: 4930
	// (get) Token: 0x0600366E RID: 13934
	bool PlayHitEffect { get; }

	// Token: 0x17001343 RID: 4931
	// (get) Token: 0x0600366F RID: 13935
	string EffectNameOverride { get; }
}
