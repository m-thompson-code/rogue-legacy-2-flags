using System;
using UnityEngine;

// Token: 0x02000639 RID: 1593
public class BossDefeated_SpawnScenario : SpawnScenario
{
	// Token: 0x17001455 RID: 5205
	// (get) Token: 0x0600399B RID: 14747 RVA: 0x000C452C File Offset: 0x000C272C
	// (set) Token: 0x0600399C RID: 14748 RVA: 0x000C4534 File Offset: 0x000C2734
	public bool SpawnIfFalse
	{
		get
		{
			return this.m_spawnIfFalse;
		}
		set
		{
			this.m_spawnIfFalse = value;
		}
	}

	// Token: 0x17001456 RID: 5206
	// (get) Token: 0x0600399D RID: 14749 RVA: 0x000C453D File Offset: 0x000C273D
	// (set) Token: 0x0600399E RID: 14750 RVA: 0x000C4545 File Offset: 0x000C2745
	public bool UseBossBiomeID
	{
		get
		{
			return this.m_useBiomeBossID;
		}
		set
		{
			this.m_useBiomeBossID = value;
		}
	}

	// Token: 0x17001457 RID: 5207
	// (get) Token: 0x0600399F RID: 14751 RVA: 0x000C454E File Offset: 0x000C274E
	// (set) Token: 0x060039A0 RID: 14752 RVA: 0x000C4556 File Offset: 0x000C2756
	public BossID BossID
	{
		get
		{
			return this.m_bossID;
		}
		set
		{
			this.m_bossID = value;
		}
	}

	// Token: 0x17001458 RID: 5208
	// (get) Token: 0x060039A1 RID: 14753 RVA: 0x000C455F File Offset: 0x000C275F
	public override SpawnScenarioType Type
	{
		get
		{
			return SpawnScenarioType.BossDefeated;
		}
	}

	// Token: 0x17001459 RID: 5209
	// (get) Token: 0x060039A2 RID: 14754 RVA: 0x000C4563 File Offset: 0x000C2763
	public override string GizmoDescription
	{
		get
		{
			if (this.UseBossBiomeID)
			{
				return "BOSS DEFEATED - BIOME";
			}
			return "BOSS DEFEATED - CUSTOM";
		}
	}

	// Token: 0x060039A3 RID: 14755 RVA: 0x000C4578 File Offset: 0x000C2778
	public override void RunIsTrueCheck(BaseRoom room)
	{
		this.RunIsTrueCheck(room.AppearanceBiomeType);
	}

	// Token: 0x060039A4 RID: 14756 RVA: 0x000C4586 File Offset: 0x000C2786
	public override void RunIsTrueCheck(GridPointManager gridPointManager)
	{
		this.RunIsTrueCheck(gridPointManager.Biome);
	}

	// Token: 0x060039A5 RID: 14757 RVA: 0x000C4594 File Offset: 0x000C2794
	private void RunIsTrueCheck(BiomeType biome)
	{
		this.IsTrue = false;
		if (this.UseBossBiomeID)
		{
			BossID biomeToBossID = BossID_RL.GetBiomeToBossID(biome);
			this.IsTrue = BossID_RL.IsBossBeaten(biomeToBossID);
		}
		else
		{
			this.IsTrue = BossID_RL.IsBossBeaten(this.BossID);
		}
		if (this.SpawnIfFalse)
		{
			this.IsTrue = !this.IsTrue;
		}
	}

	// Token: 0x04002C5C RID: 11356
	[SerializeField]
	private bool m_spawnIfFalse;

	// Token: 0x04002C5D RID: 11357
	[SerializeField]
	private bool m_useBiomeBossID;

	// Token: 0x04002C5E RID: 11358
	[SerializeField]
	private BossID m_bossID;
}
