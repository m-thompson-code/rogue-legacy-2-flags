using System;
using UnityEngine;

// Token: 0x02000A65 RID: 2661
public class BiomeType_SpawnScenario : SpawnScenario
{
	// Token: 0x17001BB8 RID: 7096
	// (get) Token: 0x06005070 RID: 20592 RVA: 0x001328CC File Offset: 0x00130ACC
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

	// Token: 0x17001BB9 RID: 7097
	// (get) Token: 0x06005071 RID: 20593 RVA: 0x00004527 File Offset: 0x00002727
	public override SpawnScenarioType Type
	{
		get
		{
			return SpawnScenarioType.BiomeType;
		}
	}

	// Token: 0x17001BBA RID: 7098
	// (get) Token: 0x06005072 RID: 20594 RVA: 0x0002BE9C File Offset: 0x0002A09C
	// (set) Token: 0x06005073 RID: 20595 RVA: 0x0002BEA4 File Offset: 0x0002A0A4
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

	// Token: 0x17001BBB RID: 7099
	// (get) Token: 0x06005074 RID: 20596 RVA: 0x0002BEAD File Offset: 0x0002A0AD
	// (set) Token: 0x06005075 RID: 20597 RVA: 0x0002BEB5 File Offset: 0x0002A0B5
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

	// Token: 0x06005076 RID: 20598 RVA: 0x0002BEBE File Offset: 0x0002A0BE
	public override void RunIsTrueCheck(BaseRoom room)
	{
		this.IsTrue = this.DoBiomeTypesMatch(room.AppearanceBiomeType);
	}

	// Token: 0x06005077 RID: 20599 RVA: 0x0002BED2 File Offset: 0x0002A0D2
	public override void RunIsTrueCheck(GridPointManager gridPointManager)
	{
		if (gridPointManager.AppearanceOverride == BiomeType.None)
		{
			this.IsTrue = this.DoBiomeTypesMatch(gridPointManager.Biome);
			return;
		}
		this.IsTrue = this.DoBiomeTypesMatch(gridPointManager.AppearanceOverride);
	}

	// Token: 0x06005078 RID: 20600 RVA: 0x0002BF01 File Offset: 0x0002A101
	private bool DoBiomeTypesMatch(BiomeType currentBiome)
	{
		if (!this.m_isNot)
		{
			return currentBiome == this.m_type;
		}
		return currentBiome != this.m_type;
	}

	// Token: 0x04003CE9 RID: 15593
	[SerializeField]
	private BiomeType m_type;

	// Token: 0x04003CEA RID: 15594
	[SerializeField]
	private bool m_isNot;

	// Token: 0x04003CEB RID: 15595
	private string m_description = string.Empty;

	// Token: 0x04003CEC RID: 15596
	private BiomeType m_previousType;

	// Token: 0x04003CED RID: 15597
	private bool m_previousIsNot;
}
