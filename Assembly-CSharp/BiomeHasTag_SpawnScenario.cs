using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000637 RID: 1591
public class BiomeHasTag_SpawnScenario : SpawnScenario
{
	// Token: 0x1700144E RID: 5198
	// (get) Token: 0x06003989 RID: 14729 RVA: 0x000C4324 File Offset: 0x000C2524
	public override string GizmoDescription
	{
		get
		{
			if (this.m_previousTag != this.m_tag)
			{
				this.m_previousTag = this.m_tag;
				this.m_description = string.Format("BiT: {0}", this.m_tag.ToString());
			}
			return this.m_description;
		}
	}

	// Token: 0x1700144F RID: 5199
	// (get) Token: 0x0600398A RID: 14730 RVA: 0x000C4372 File Offset: 0x000C2572
	public override SpawnScenarioType Type
	{
		get
		{
			return SpawnScenarioType.BiomeHasTag;
		}
	}

	// Token: 0x17001450 RID: 5200
	// (get) Token: 0x0600398B RID: 14731 RVA: 0x000C4376 File Offset: 0x000C2576
	// (set) Token: 0x0600398C RID: 14732 RVA: 0x000C437E File Offset: 0x000C257E
	public BiomeTag Tag
	{
		get
		{
			return this.m_tag;
		}
		set
		{
			this.m_tag = value;
		}
	}

	// Token: 0x0600398D RID: 14733 RVA: 0x000C4387 File Offset: 0x000C2587
	public override void RunIsTrueCheck(BaseRoom room)
	{
		this.IsTrue = this.GetHasTag(room.AppearanceBiomeType);
	}

	// Token: 0x0600398E RID: 14734 RVA: 0x000C439B File Offset: 0x000C259B
	public override void RunIsTrueCheck(GridPointManager gridPointManager)
	{
		this.IsTrue = this.GetHasTag(gridPointManager.Biome);
	}

	// Token: 0x0600398F RID: 14735 RVA: 0x000C43B0 File Offset: 0x000C25B0
	private bool GetHasTag(BiomeType biome)
	{
		bool result = false;
		if (BiomeCreation_EV.BIOME_TAGS.ContainsKey(biome))
		{
			List<BiomeTag> list = BiomeCreation_EV.BIOME_TAGS[biome];
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] == this.Tag)
					{
						result = true;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x04002C54 RID: 11348
	[SerializeField]
	private BiomeTag m_tag;

	// Token: 0x04002C55 RID: 11349
	private string m_description = string.Empty;

	// Token: 0x04002C56 RID: 11350
	private BiomeTag m_previousTag;
}
