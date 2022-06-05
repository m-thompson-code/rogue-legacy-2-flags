using System;
using System.Collections.Generic;
using Ferr;
using UnityEngine;

// Token: 0x020003CA RID: 970
[RequireComponent(typeof(Ferr2DT_PathTerrain))]
public class LeylineController : MonoBehaviour
{
	// Token: 0x17000E3E RID: 3646
	// (get) Token: 0x06001FE5 RID: 8165 RVA: 0x00010DF4 File Offset: 0x0000EFF4
	public bool UseCircleCorners
	{
		get
		{
			return this.m_useCircleCorners;
		}
	}

	// Token: 0x06001FE6 RID: 8166 RVA: 0x000A3FC0 File Offset: 0x000A21C0
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

	// Token: 0x04001C8B RID: 7307
	[SerializeField]
	private bool m_useCircleCorners;
}
