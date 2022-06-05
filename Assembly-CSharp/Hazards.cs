using System;
using System.Collections.Generic;

// Token: 0x02000233 RID: 563
[Serializable]
public class Hazards
{
	// Token: 0x17000B32 RID: 2866
	// (get) Token: 0x060016BC RID: 5820 RVA: 0x00046EBA File Offset: 0x000450BA
	public Dictionary<BiomeType, HazardType[]> HazardsInBiomeTable
	{
		get
		{
			if (this.m_hazardsInBiomeTable == null)
			{
				this.InitialiseHazardTable();
			}
			return this.m_hazardsInBiomeTable;
		}
	}

	// Token: 0x060016BD RID: 5821 RVA: 0x00046ED0 File Offset: 0x000450D0
	private void InitialiseHazardTable()
	{
		this.m_hazardsInBiomeTable = new Dictionary<BiomeType, HazardType[]>();
		foreach (HazardEntry hazardEntry in this.HazardsInBiomes)
		{
			this.m_hazardsInBiomeTable.Add(hazardEntry.Biome, hazardEntry.Hazards);
		}
	}

	// Token: 0x04001630 RID: 5680
	public HazardEntry[] HazardsInBiomes;

	// Token: 0x04001631 RID: 5681
	public HazardType[] DefaultHazardsInBiomes;

	// Token: 0x04001632 RID: 5682
	private Dictionary<BiomeType, HazardType[]> m_hazardsInBiomeTable;
}
