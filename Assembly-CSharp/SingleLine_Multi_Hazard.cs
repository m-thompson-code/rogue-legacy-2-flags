using System;
using UnityEngine;

// Token: 0x02000744 RID: 1860
public class SingleLine_Multi_Hazard : Multi_Hazard
{
	// Token: 0x060038EB RID: 14571 RVA: 0x000E9D18 File Offset: 0x000E7F18
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

	// Token: 0x04002D9B RID: 11675
	protected Vector2 m_startingLocalPos;

	// Token: 0x04002D9C RID: 11676
	protected PivotPoint m_pivotPoint;

	// Token: 0x04002D9D RID: 11677
	protected float m_halfWidth = 10f;
}
