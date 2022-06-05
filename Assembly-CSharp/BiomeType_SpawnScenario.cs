using System;
using UnityEngine;

// Token: 0x02000638 RID: 1592
public class BiomeType_SpawnScenario : SpawnScenario
{
	// Token: 0x17001451 RID: 5201
	// (get) Token: 0x06003991 RID: 14737 RVA: 0x000C4414 File Offset: 0x000C2614
	public override string GizmoDescription
	{
		get
		{
			if (this.m_previousType != this.m_type || this.m_previousIsNot != this.m_isNot)
			{
				this.m_previousType = this.m_type;
				this.m_previousIsNot = this.m_isNot;
				this.m_description = string.Format("BiTy: {0}{1}", this.m_isNot ? "!" : "", this.m_type.ToString());
			}
			return this.m_description;
		}
	}

	// Token: 0x17001452 RID: 5202
	// (get) Token: 0x06003992 RID: 14738 RVA: 0x000C4490 File Offset: 0x000C2690
	public override SpawnScenarioType Type
	{
		get
		{
			return SpawnScenarioType.BiomeType;
		}
	}

	// Token: 0x17001453 RID: 5203
	// (get) Token: 0x06003993 RID: 14739 RVA: 0x000C4494 File Offset: 0x000C2694
	// (set) Token: 0x06003994 RID: 14740 RVA: 0x000C449C File Offset: 0x000C269C
	public BiomeType BiomeType
	{
		get
		{
			return this.m_type;
		}
		set
		{
			this.m_type = value;
		}
	}

	// Token: 0x17001454 RID: 5204
	// (get) Token: 0x06003995 RID: 14741 RVA: 0x000C44A5 File Offset: 0x000C26A5
	// (set) Token: 0x06003996 RID: 14742 RVA: 0x000C44AD File Offset: 0x000C26AD
	public bool IsNot
	{
		get
		{
			return this.m_isNot;
		}
		set
		{
			this.m_isNot = value;
		}
	}

	// Token: 0x06003997 RID: 14743 RVA: 0x000C44B6 File Offset: 0x000C26B6
	public override void RunIsTrueCheck(BaseRoom room)
	{
		this.IsTrue = this.DoBiomeTypesMatch(room.AppearanceBiomeType);
	}

	// Token: 0x06003998 RID: 14744 RVA: 0x000C44CA File Offset: 0x000C26CA
	public override void RunIsTrueCheck(GridPointManager gridPointManager)
	{
		if (gridPointManager.AppearanceOverride == BiomeType.None)
		{
			this.IsTrue = this.DoBiomeTypesMatch(gridPointManager.Biome);
			return;
		}
		this.IsTrue = this.DoBiomeTypesMatch(gridPointManager.AppearanceOverride);
	}

	// Token: 0x06003999 RID: 14745 RVA: 0x000C44F9 File Offset: 0x000C26F9
	private bool DoBiomeTypesMatch(BiomeType currentBiome)
	{
		if (!this.m_isNot)
		{
			return currentBiome == this.m_type;
		}
		return currentBiome != this.m_type;
	}

	// Token: 0x04002C57 RID: 11351
	[SerializeField]
	private BiomeType m_type;

	// Token: 0x04002C58 RID: 11352
	[SerializeField]
	private bool m_isNot;

	// Token: 0x04002C59 RID: 11353
	private string m_description = string.Empty;

	// Token: 0x04002C5A RID: 11354
	private BiomeType m_previousType;

	// Token: 0x04002C5B RID: 11355
	private bool m_previousIsNot;
}
