using System;
using UnityEngine;

// Token: 0x02000A66 RID: 2662
public class BossDefeated_SpawnScenario : SpawnScenario
{
	// Token: 0x17001BBC RID: 7100
	// (get) Token: 0x0600507A RID: 20602 RVA: 0x0002BF34 File Offset: 0x0002A134
	// (set) Token: 0x0600507B RID: 20603 RVA: 0x0002BF3C File Offset: 0x0002A13C
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

	// Token: 0x17001BBD RID: 7101
	// (get) Token: 0x0600507C RID: 20604 RVA: 0x0002BF45 File Offset: 0x0002A145
	// (set) Token: 0x0600507D RID: 20605 RVA: 0x0002BF4D File Offset: 0x0002A14D
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

	// Token: 0x17001BBE RID: 7102
	// (get) Token: 0x0600507E RID: 20606 RVA: 0x0002BF56 File Offset: 0x0002A156
	// (set) Token: 0x0600507F RID: 20607 RVA: 0x0002BF5E File Offset: 0x0002A15E
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

	// Token: 0x17001BBF RID: 7103
	// (get) Token: 0x06005080 RID: 20608 RVA: 0x0000452B File Offset: 0x0000272B
	public override SpawnScenarioType Type
	{
		get
		{
			return SpawnScenarioType.BossDefeated;
		}
	}

	// Token: 0x17001BC0 RID: 7104
	// (get) Token: 0x06005081 RID: 20609 RVA: 0x0002BF67 File Offset: 0x0002A167
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

	// Token: 0x06005082 RID: 20610 RVA: 0x0002BF7C File Offset: 0x0002A17C
	public override void RunIsTrueCheck(BaseRoom room)
	{
		this.RunIsTrueCheck(room.AppearanceBiomeType);
	}

	// Token: 0x06005083 RID: 20611 RVA: 0x0002BF8A File Offset: 0x0002A18A
	public override void RunIsTrueCheck(GridPointManager gridPointManager)
	{
		this.RunIsTrueCheck(gridPointManager.Biome);
	}

	// Token: 0x06005084 RID: 20612 RVA: 0x00132948 File Offset: 0x00130B48
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

	// Token: 0x04003CEE RID: 15598
	[SerializeField]
	private bool m_spawnIfFalse;

	// Token: 0x04003CEF RID: 15599
	[SerializeField]
	private bool m_useBiomeBossID;

	// Token: 0x04003CF0 RID: 15600
	[SerializeField]
	private BossID m_bossID;
}
