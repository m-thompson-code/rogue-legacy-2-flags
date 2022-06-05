using System;
using UnityEngine;

// Token: 0x02000685 RID: 1669
public class SpawnConditionOverride
{
	// Token: 0x170014F7 RID: 5367
	// (get) Token: 0x06003C2A RID: 15402 RVA: 0x000D035F File Offset: 0x000CE55F
	// (set) Token: 0x06003C2B RID: 15403 RVA: 0x000D0367 File Offset: 0x000CE567
	public BiomeType Biome { get; private set; }

	// Token: 0x170014F8 RID: 5368
	// (get) Token: 0x06003C2C RID: 15404 RVA: 0x000D0370 File Offset: 0x000CE570
	// (set) Token: 0x06003C2D RID: 15405 RVA: 0x000D0378 File Offset: 0x000CE578
	public bool HasDash { get; private set; }

	// Token: 0x170014F9 RID: 5369
	// (get) Token: 0x06003C2E RID: 15406 RVA: 0x000D0381 File Offset: 0x000CE581
	// (set) Token: 0x06003C2F RID: 15407 RVA: 0x000D0389 File Offset: 0x000CE589
	public bool HasDoubleJump { get; private set; }

	// Token: 0x170014FA RID: 5370
	// (get) Token: 0x06003C30 RID: 15408 RVA: 0x000D0392 File Offset: 0x000CE592
	// (set) Token: 0x06003C31 RID: 15409 RVA: 0x000D039A File Offset: 0x000CE59A
	public bool OverrideHasDash { get; private set; }

	// Token: 0x170014FB RID: 5371
	// (get) Token: 0x06003C32 RID: 15410 RVA: 0x000D03A3 File Offset: 0x000CE5A3
	// (set) Token: 0x06003C33 RID: 15411 RVA: 0x000D03AB File Offset: 0x000CE5AB
	public bool OverrideHasDoubleJump { get; private set; }

	// Token: 0x06003C34 RID: 15412 RVA: 0x000D03B4 File Offset: 0x000CE5B4
	public SpawnConditionOverride(BiomeType biome)
	{
		this.Biome = biome;
	}

	// Token: 0x06003C35 RID: 15413 RVA: 0x000D03C3 File Offset: 0x000CE5C3
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

	// Token: 0x06003C36 RID: 15414 RVA: 0x000D03FC File Offset: 0x000CE5FC
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
