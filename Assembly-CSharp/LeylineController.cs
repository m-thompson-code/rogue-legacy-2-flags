using System;
using System.Collections.Generic;
using Ferr;
using UnityEngine;

// Token: 0x02000217 RID: 535
[RequireComponent(typeof(Ferr2DT_PathTerrain))]
public class LeylineController : MonoBehaviour
{
	// Token: 0x17000B17 RID: 2839
	// (get) Token: 0x06001649 RID: 5705 RVA: 0x000459DC File Offset: 0x00043BDC
	public bool UseCircleCorners
	{
		get
		{
			return this.m_useCircleCorners;
		}
	}

	// Token: 0x0600164A RID: 5706 RVA: 0x000459E4 File Offset: 0x00043BE4
	private void Start()
	{
		Ferr2DT_PathTerrain component = base.GetComponent<Ferr2DT_PathTerrain>();
		if (component != null)
		{
			using (List<PointControl>.Enumerator enumerator = component.PathData.GetControls().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PointControl pointControl = enumerator.Current;
					if (this.m_useCircleCorners)
					{
						pointControl.type = PointType.CircleCorner;
					}
					else
					{
						pointControl.type = PointType.Sharp;
					}
				}
				return;
			}
		}
		Debug.LogFormat("<color=red>| {0} | No Ferr2D Component found.</color>", new object[]
		{
			this
		});
	}

	// Token: 0x04001588 RID: 5512
	[SerializeField]
	private bool m_useCircleCorners;
}
