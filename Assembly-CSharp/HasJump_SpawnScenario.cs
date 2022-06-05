using System;
using UnityEngine;

// Token: 0x02000A6B RID: 2667
public class HasJump_SpawnScenario : SpawnScenario
{
	// Token: 0x17001BCB RID: 7115
	// (get) Token: 0x060050A1 RID: 20641 RVA: 0x0002C029 File Offset: 0x0002A229
	// (set) Token: 0x060050A2 RID: 20642 RVA: 0x0002C031 File Offset: 0x0002A231
	public bool HasDoubleJump
	{
		get
		{
			return this.m_hasDoubleJump;
		}
		set
		{
			this.m_hasDoubleJump = value;
		}
	}

	// Token: 0x17001BCC RID: 7116
	// (get) Token: 0x060050A3 RID: 20643 RVA: 0x00017640 File Offset: 0x00015840
	public override SpawnScenarioType Type
	{
		get
		{
			return SpawnScenarioType.HasJump;
		}
	}

	// Token: 0x17001BCD RID: 7117
	// (get) Token: 0x060050A4 RID: 20644 RVA: 0x0002C03A File Offset: 0x0002A23A
	public override string GizmoDescription
	{
		get
		{
			if (this.m_hasDoubleJump)
			{
				return "JUMP";
			}
			return "NO JUMP";
		}
	}

	// Token: 0x060050A5 RID: 20645 RVA: 0x0002C04F File Offset: 0x0002A24F
	public override void RunIsTrueCheck(BaseRoom room)
	{
		this.RunIsTrueCheck(room.AppearanceBiomeType);
	}

	// Token: 0x060050A6 RID: 20646 RVA: 0x0002C05D File Offset: 0x0002A25D
	public override void RunIsTrueCheck(GridPointManager gridPointManager)
	{
		this.RunIsTrueCheck(gridPointManager.Biome);
	}

	// Token: 0x060050A7 RID: 20647 RVA: 0x00132FCC File Offset: 0x001311CC
	private void RunIsTrueCheck(BiomeType biome)
	{
		SpawnConditionOverride spawnConditionOverride = BiomeRuleManager.GetSpawnConditionOverride(biome);
		if (spawnConditionOverride.OverrideHasDoubleJump)
		{
			this.IsTrue = (this.HasDoubleJump == spawnConditionOverride.HasDoubleJump);
			return;
		}
		if (PlayerManager.IsInstantiated)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			if (this.HasDoubleJump)
			{
				this.IsTrue = (playerController.CharacterJump.NumberOfJumps > 1);
				return;
			}
			this.IsTrue = (playerController.CharacterJump.NumberOfJumps <= 1);
		}
	}

	// Token: 0x04003D02 RID: 15618
	[SerializeField]
	private bool m_hasDoubleJump;
}
