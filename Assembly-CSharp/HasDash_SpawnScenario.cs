using System;
using UnityEngine;

// Token: 0x0200063D RID: 1597
public class HasDash_SpawnScenario : SpawnScenario
{
	// Token: 0x17001461 RID: 5217
	// (get) Token: 0x060039BA RID: 14778 RVA: 0x000C4C08 File Offset: 0x000C2E08
	// (set) Token: 0x060039BB RID: 14779 RVA: 0x000C4C10 File Offset: 0x000C2E10
	public bool HasDash
	{
		get
		{
			return this.m_hasDash;
		}
		set
		{
			this.m_hasDash = value;
		}
	}

	// Token: 0x17001462 RID: 5218
	// (get) Token: 0x060039BC RID: 14780 RVA: 0x000C4C19 File Offset: 0x000C2E19
	public override SpawnScenarioType Type
	{
		get
		{
			return SpawnScenarioType.HasDash;
		}
	}

	// Token: 0x17001463 RID: 5219
	// (get) Token: 0x060039BD RID: 14781 RVA: 0x000C4C1D File Offset: 0x000C2E1D
	public override string GizmoDescription
	{
		get
		{
			if (this.m_hasDash)
			{
				return "DASH";
			}
			return "NO DASH";
		}
	}

	// Token: 0x060039BE RID: 14782 RVA: 0x000C4C32 File Offset: 0x000C2E32
	public override void RunIsTrueCheck(BaseRoom room)
	{
		this.RunIsTrueCheck(room.AppearanceBiomeType);
	}

	// Token: 0x060039BF RID: 14783 RVA: 0x000C4C40 File Offset: 0x000C2E40
	public override void RunIsTrueCheck(GridPointManager gridPointManager)
	{
		this.RunIsTrueCheck(gridPointManager.Biome);
	}

	// Token: 0x060039C0 RID: 14784 RVA: 0x000C4C50 File Offset: 0x000C2E50
	private void RunIsTrueCheck(BiomeType biome)
	{
		SpawnConditionOverride spawnConditionOverride = BiomeRuleManager.GetSpawnConditionOverride(biome);
		if (spawnConditionOverride.OverrideHasDash)
		{
			this.IsTrue = (this.HasDash == spawnConditionOverride.HasDash);
			return;
		}
		if (PlayerManager.IsInstantiated)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			if (this.HasDash)
			{
				this.IsTrue = (playerController.CharacterDash.TotalDashesAllowed > 0);
				return;
			}
			this.IsTrue = (playerController.CharacterDash.TotalDashesAllowed <= 0);
		}
	}

	// Token: 0x04002C6F RID: 11375
	[SerializeField]
	private bool m_hasDash;
}
