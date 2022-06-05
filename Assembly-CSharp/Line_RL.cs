using System;
using UnityEngine;

// Token: 0x02000AF5 RID: 2805
public struct Line_RL
{
	// Token: 0x06005438 RID: 21560 RVA: 0x0002DB03 File Offset: 0x0002BD03
	public Line_RL(Vector2 pointA, Vector2 pointB)
	{
		this.PointA = pointA;
		this.PointB = pointB;
	}

	// Token: 0x17001CA8 RID: 7336
	// (get) Token: 0x06005439 RID: 21561 RVA: 0x0002DB13 File Offset: 0x0002BD13
	public readonly Vector2 PointA { get; }

	// Token: 0x17001CA9 RID: 7337
	// (get) Token: 0x0600543A RID: 21562 RVA: 0x0002DB1B File Offset: 0x0002BD1B
	public readonly Vector2 PointB { get; }

	// Token: 0x0600543B RID: 21563 RVA: 0x0013E1A8 File Offset: 0x0013C3A8
	public override bool Equals(object obj)
	{
		Line_RL line_RL = (Line_RL)obj;
		bool result = false;
		if (this.PointA == line_RL.PointA && this.PointB == line_RL.PointB)
		{
			result = true;
		}
		else if (this.PointA == line_RL.PointB && this.PointB == line_RL.PointA)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x0600543C RID: 21564 RVA: 0x0002DB23 File Offset: 0x0002BD23
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
