using System;
using System.Collections.Generic;

// Token: 0x020003F0 RID: 1008
[Serializable]
public class Hazards
{
	// Token: 0x17000E5F RID: 3679
	// (get) Token: 0x0600206F RID: 8303 RVA: 0x0001131F File Offset: 0x0000F51F
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

	// Token: 0x06002070 RID: 8304 RVA: 0x000A4FA8 File Offset: 0x000A31A8
	private void InitialiseHazardTable()
	{
		this.m_hazardsInBiomeTable = new Dictionary<BiomeType, HazardType[]>();
		foreach (HazardEntry hazardEntry in this.HazardsInBiomes)
		{
			this.m_hazardsInBiomeTable.Add(hazardEntry.Biome, hazardEntry.Hazards);
		}
	}

	// Token: 0x04001D48 RID: 7496
	public HazardEntry[] HazardsInBiomes;

	// Token: 0x04001D49 RID: 7497
	public HazardType[] DefaultHazardsInBiomes;

	// Token: 0x04001D4A RID: 7498
	private Dictionary<BiomeType, HazardType[]> m_hazardsInBiomeTable;
}
