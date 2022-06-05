using System;
using UnityEngine;

// Token: 0x0200067A RID: 1658
public struct Line_RL
{
	// Token: 0x06003BE8 RID: 15336 RVA: 0x000CE276 File Offset: 0x000CC476
	public Line_RL(Vector2 pointA, Vector2 pointB)
	{
		this.PointA = pointA;
		this.PointB = pointB;
	}

	// Token: 0x170014EA RID: 5354
	// (get) Token: 0x06003BE9 RID: 15337 RVA: 0x000CE286 File Offset: 0x000CC486
	public readonly Vector2 PointA { get; }

	// Token: 0x170014EB RID: 5355
	// (get) Token: 0x06003BEA RID: 15338 RVA: 0x000CE28E File Offset: 0x000CC48E
	public readonly Vector2 PointB { get; }

	// Token: 0x06003BEB RID: 15339 RVA: 0x000CE298 File Offset: 0x000CC498
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

	// Token: 0x06003BEC RID: 15340 RVA: 0x000CE305 File Offset: 0x000CC505
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
