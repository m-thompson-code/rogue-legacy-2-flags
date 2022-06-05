using System;
using UnityEngine;

// Token: 0x02000A2A RID: 2602
public class Ferr2DHazardSpawnController : HazardSpawnControllerBase
{
	// Token: 0x17001B3B RID: 6971
	// (get) Token: 0x06004ECF RID: 20175 RVA: 0x00017640 File Offset: 0x00015840
	public override HazardCategory Category
	{
		get
		{
			return HazardCategory.Ferr2D;
		}
	}

	// Token: 0x06004ED0 RID: 20176 RVA: 0x0002AFB1 File Offset: 0x000291B1
	protected override void Awake()
	{
		base.Awake();
		this.m_ferr2D = base.gameObject.GetComponent<Ferr2DT_PathTerrain>();
		base.gameObject.GetComponent<MeshRenderer>().enabled = false;
	}

	// Token: 0x06004ED1 RID: 20177 RVA: 0x0002AFDB File Offset: 0x000291DB
	protected override IHazard GetHazard(HazardType hazardType)
	{
		return base.GetHazard(hazardType);
	}

	// Token: 0x06004ED2 RID: 20178 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void Reset()
	{
	}

	// Token: 0x06004ED3 RID: 20179 RVA: 0x0012E40C File Offset: 0x0012C60C
	protected override void Spawn()
	{
		if (base.Type == HazardType.None)
		{
			return;
		}
		if (!base.Hazard.IsNativeNull())
		{
			if (base.Hazard is IFerr2DHazard && !base.Hazard.gameObject.activeSelf)
			{
				base.Hazard.gameObject.SetActive(true);
			}
			return;
		}
		if (!this.m_ferr2D)
		{
			this.m_ferr2D = base.gameObject.GetComponent<Ferr2DT_PathTerrain>();
		}
		base.CreateHazard();
		if (base.Hazard != null && base.Hazard is IFerr2DHazard)
		{
			IFerr2DHazard ferr2DHazard = base.Hazard as IFerr2DHazard;
			ferr2DHazard.gameObject.transform.SetParent(base.transform.parent);
			Ferr2DHazardArgs hazardArgs = new Ferr2DHazardArgs(StateID.One, base.transform.localPosition, this.m_ferr2D.PathData.GetPoints(0));
			ferr2DHazard.Initialize(hazardArgs);
		}
	}

	// Token: 0x04003B4E RID: 15182
	private Ferr2DT_PathTerrain m_ferr2D;
}
