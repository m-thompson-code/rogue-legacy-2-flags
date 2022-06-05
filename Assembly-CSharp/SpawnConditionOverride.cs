using System;
using UnityEngine;

// Token: 0x02000B05 RID: 2821
public class SpawnConditionOverride
{
	// Token: 0x17001CB5 RID: 7349
	// (get) Token: 0x06005489 RID: 21641 RVA: 0x0002DCF0 File Offset: 0x0002BEF0
	// (set) Token: 0x0600548A RID: 21642 RVA: 0x0002DCF8 File Offset: 0x0002BEF8
	public BiomeType Biome { get; private set; }

	// Token: 0x17001CB6 RID: 7350
	// (get) Token: 0x0600548B RID: 21643 RVA: 0x0002DD01 File Offset: 0x0002BF01
	// (set) Token: 0x0600548C RID: 21644 RVA: 0x0002DD09 File Offset: 0x0002BF09
	public bool HasDash { get; private set; }

	// Token: 0x17001CB7 RID: 7351
	// (get) Token: 0x0600548D RID: 21645 RVA: 0x0002DD12 File Offset: 0x0002BF12
	// (set) Token: 0x0600548E RID: 21646 RVA: 0x0002DD1A File Offset: 0x0002BF1A
	public bool HasDoubleJump { get; private set; }

	// Token: 0x17001CB8 RID: 7352
	// (get) Token: 0x0600548F RID: 21647 RVA: 0x0002DD23 File Offset: 0x0002BF23
	// (set) Token: 0x06005490 RID: 21648 RVA: 0x0002DD2B File Offset: 0x0002BF2B
	public bool OverrideHasDash { get; private set; }

	// Token: 0x17001CB9 RID: 7353
	// (get) Token: 0x06005491 RID: 21649 RVA: 0x0002DD34 File Offset: 0x0002BF34
	// (set) Token: 0x06005492 RID: 21650 RVA: 0x0002DD3C File Offset: 0x0002BF3C
	public bool OverrideHasDoubleJump { get; private set; }

	// Token: 0x06005493 RID: 21651 RVA: 0x0002DD45 File Offset: 0x0002BF45
	public SpawnConditionOverride(BiomeType biome)
	{
		this.Biome = biome;
	}

	// Token: 0x06005494 RID: 21652 RVA: 0x0002DD54 File Offset: 0x0002BF54
	public void SetIsOverrideOn(SpawnConditionOverrideID condition, bool isOn)
	{
		if (condition == SpawnConditionOverrideID.Dash)
		{
			this.OverrideHasDash = isOn;
			return;
		}
		if (condition == SpawnConditionOverrideID.DoubleJump)
		{
			this.OverrideHasDoubleJump = isOn;
			return;
		}
		Debug.LogFormat("<color=red>| {0} | Unhandled condition ({1}). If you see this message, please add a bug to pivotal along with the Stack Trace</color>", new object[]
		{
			this,
			condition
		});
	}

	// Token: 0x06005495 RID: 21653 RVA: 0x0002DD8D File Offset: 0x0002BF8D
	public void SetOverrideValue(SpawnConditionOverrideID condition, bool value)
	{
		if (condition == SpawnConditionOverrideID.Dash)
		{
			this.HasDash = value;
			return;
		}
		if (condition == SpawnConditionOverrideID.DoubleJump)
		{
			this.HasDoubleJump = value;
			return;
		}
		Debug.LogFormat("<color=red>| {0} | Unhandled condition ({1}). If you see this message, please add a bug to pivotal along with the Stack Trace</color>", new object[]
		{
			this,
			condition
		});
	}
}
