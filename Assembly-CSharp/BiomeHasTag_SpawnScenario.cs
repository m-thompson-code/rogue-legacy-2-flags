using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A64 RID: 2660
public class BiomeHasTag_SpawnScenario : SpawnScenario
{
	// Token: 0x17001BB5 RID: 7093
	// (get) Token: 0x06005068 RID: 20584 RVA: 0x0013282C File Offset: 0x00130A2C
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

	// Token: 0x17001BB6 RID: 7094
	// (get) Token: 0x06005069 RID: 20585 RVA: 0x000065B4 File Offset: 0x000047B4
	public override SpawnScenarioType Type
	{
		get
		{
			return SpawnScenarioType.BiomeHasTag;
		}
	}

	// Token: 0x17001BB7 RID: 7095
	// (get) Token: 0x0600506A RID: 20586 RVA: 0x0002BE50 File Offset: 0x0002A050
	// (set) Token: 0x0600506B RID: 20587 RVA: 0x0002BE58 File Offset: 0x0002A058
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

	// Token: 0x0600506C RID: 20588 RVA: 0x0002BE61 File Offset: 0x0002A061
	public override void RunIsTrueCheck(BaseRoom room)
	{
		this.IsTrue = this.GetHasTag(room.AppearanceBiomeType);
	}

	// Token: 0x0600506D RID: 20589 RVA: 0x0002BE75 File Offset: 0x0002A075
	public override void RunIsTrueCheck(GridPointManager gridPointManager)
	{
		this.IsTrue = this.GetHasTag(gridPointManager.Biome);
	}

	// Token: 0x0600506E RID: 20590 RVA: 0x0013287C File Offset: 0x00130A7C
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

	// Token: 0x04003CE6 RID: 15590
	[SerializeField]
	private BiomeTag m_tag;

	// Token: 0x04003CE7 RID: 15591
	private string m_description = string.Empty;

	// Token: 0x04003CE8 RID: 15592
	private BiomeTag m_previousTag;
}
