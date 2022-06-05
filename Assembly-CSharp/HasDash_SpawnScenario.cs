using System;
using UnityEngine;

// Token: 0x02000A6A RID: 2666
public class HasDash_SpawnScenario : SpawnScenario
{
	// Token: 0x17001BC8 RID: 7112
	// (get) Token: 0x06005099 RID: 20633 RVA: 0x0002BFE7 File Offset: 0x0002A1E7
	// (set) Token: 0x0600509A RID: 20634 RVA: 0x0002BFEF File Offset: 0x0002A1EF
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

	// Token: 0x17001BC9 RID: 7113
	// (get) Token: 0x0600509B RID: 20635 RVA: 0x00006732 File Offset: 0x00004932
	public override SpawnScenarioType Type
	{
		get
		{
			return SpawnScenarioType.HasDash;
		}
	}

	// Token: 0x17001BCA RID: 7114
	// (get) Token: 0x0600509C RID: 20636 RVA: 0x0002BFF8 File Offset: 0x0002A1F8
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

	// Token: 0x0600509D RID: 20637 RVA: 0x0002C00D File Offset: 0x0002A20D
	public override void RunIsTrueCheck(BaseRoom room)
	{
		this.RunIsTrueCheck(room.AppearanceBiomeType);
	}

	// Token: 0x0600509E RID: 20638 RVA: 0x0002C01B File Offset: 0x0002A21B
	public override void RunIsTrueCheck(GridPointManager gridPointManager)
	{
		this.RunIsTrueCheck(gridPointManager.Biome);
	}

	// Token: 0x0600509F RID: 20639 RVA: 0x00132F58 File Offset: 0x00131158
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

	// Token: 0x04003D01 RID: 15617
	[SerializeField]
	private bool m_hasDash;
}
