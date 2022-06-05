using System;
using UnityEngine;

// Token: 0x0200063E RID: 1598
public class HasJump_SpawnScenario : SpawnScenario
{
	// Token: 0x17001464 RID: 5220
	// (get) Token: 0x060039C2 RID: 14786 RVA: 0x000C4CCA File Offset: 0x000C2ECA
	// (set) Token: 0x060039C3 RID: 14787 RVA: 0x000C4CD2 File Offset: 0x000C2ED2
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

	// Token: 0x17001465 RID: 5221
	// (get) Token: 0x060039C4 RID: 14788 RVA: 0x000C4CDB File Offset: 0x000C2EDB
	public override SpawnScenarioType Type
	{
		get
		{
			return SpawnScenarioType.HasJump;
		}
	}

	// Token: 0x17001466 RID: 5222
	// (get) Token: 0x060039C5 RID: 14789 RVA: 0x000C4CDF File Offset: 0x000C2EDF
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

	// Token: 0x060039C6 RID: 14790 RVA: 0x000C4CF4 File Offset: 0x000C2EF4
	public override void RunIsTrueCheck(BaseRoom room)
	{
		this.RunIsTrueCheck(room.AppearanceBiomeType);
	}

	// Token: 0x060039C7 RID: 14791 RVA: 0x000C4D02 File Offset: 0x000C2F02
	public override void RunIsTrueCheck(GridPointManager gridPointManager)
	{
		this.RunIsTrueCheck(gridPointManager.Biome);
	}

	// Token: 0x060039C8 RID: 14792 RVA: 0x000C4D10 File Offset: 0x000C2F10
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

	// Token: 0x04002C70 RID: 11376
	[SerializeField]
	private bool m_hasDoubleJump;
}
