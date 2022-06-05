using System;
using UnityEngine;

// Token: 0x0200045A RID: 1114
public class SingleLine_Multi_Hazard : Multi_Hazard
{
	// Token: 0x06002923 RID: 10531 RVA: 0x00088100 File Offset: 0x00086300
	public override void Initialize(PivotPoint pivot, int width, HazardArgs hazardArgs)
	{
		this.m_halfWidth = (float)width / 2f;
		Vector3 zero = Vector3.zero;
		if (pivot != PivotPoint.Left)
		{
			if (pivot == PivotPoint.Right)
			{
				zero.x -= this.m_halfWidth;
			}
		}
		else
		{
			zero.x += this.m_halfWidth;
		}
		this.m_startingLocalPos = zero;
		if (this.m_pivot != null)
		{
			this.m_pivot.transform.localPosition = zero;
			this.m_pivotPoint = pivot;
		}
		base.InitialState = hazardArgs.InitialState;
		this.ResetHazard();
	}

	// Token: 0x040021EA RID: 8682
	protected Vector2 m_startingLocalPos;

	// Token: 0x040021EB RID: 8683
	protected PivotPoint m_pivotPoint;

	// Token: 0x040021EC RID: 8684
	protected float m_halfWidth = 10f;
}
