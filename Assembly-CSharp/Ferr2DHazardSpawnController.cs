using System;
using UnityEngine;

// Token: 0x02000609 RID: 1545
public class Ferr2DHazardSpawnController : HazardSpawnControllerBase
{
	// Token: 0x170013E4 RID: 5092
	// (get) Token: 0x06003832 RID: 14386 RVA: 0x000BFF37 File Offset: 0x000BE137
	public override HazardCategory Category
	{
		get
		{
			return HazardCategory.Ferr2D;
		}
	}

	// Token: 0x06003833 RID: 14387 RVA: 0x000BFF3B File Offset: 0x000BE13B
	protected override void Awake()
	{
		base.Awake();
		this.m_ferr2D = base.gameObject.GetComponent<Ferr2DT_PathTerrain>();
		base.gameObject.GetComponent<MeshRenderer>().enabled = false;
	}

	// Token: 0x06003834 RID: 14388 RVA: 0x000BFF65 File Offset: 0x000BE165
	protected override IHazard GetHazard(HazardType hazardType)
	{
		return base.GetHazard(hazardType);
	}

	// Token: 0x06003835 RID: 14389 RVA: 0x000BFF6E File Offset: 0x000BE16E
	protected override void Reset()
	{
	}

	// Token: 0x06003836 RID: 14390 RVA: 0x000BFF70 File Offset: 0x000BE170
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

	// Token: 0x04002AEB RID: 10987
	private Ferr2DT_PathTerrain m_ferr2D;
}
