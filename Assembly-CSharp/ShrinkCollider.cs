using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004F4 RID: 1268
public class ShrinkCollider : MonoBehaviour
{
	// Token: 0x060028CD RID: 10445 RVA: 0x00016EB5 File Offset: 0x000150B5
	private void Start()
	{
		this.Shrink();
	}

	// Token: 0x060028CE RID: 10446 RVA: 0x000BEEB4 File Offset: 0x000BD0B4
	public void Shrink()
	{
		bool flag = false;
		Ferr2DT_PathTerrain component = base.GetComponent<Ferr2DT_PathTerrain>();
		if (component)
		{
			component.RecreateCollider();
		}
		PolygonCollider2D component2 = base.GetComponent<PolygonCollider2D>();
		if (component2)
		{
			if (component2.pathCount > 1)
			{
				Debug.Log("ShrinkPolygonCollider - Could not shrink polygon collider.  Collider had more than one path. Please convert to simple polygon first.");
				return;
			}
			Vector2 offset = component2.offset;
			Vector2[] points = component2.points;
			ShrinkCollider.m_colliderPointsHelper_STATIC.Clear();
			ShrinkCollider.m_colliderPointsHelper_STATIC.AddRange(points);
			for (int i = 0; i < points.Length; i++)
			{
				int index = (i <= 0) ? (points.Length - 1) : (i - 1);
				Vector2 inverseNormalVector = this.GetInverseNormalVector(points, component2, index);
				Vector2 inverseNormalVector2 = this.GetInverseNormalVector(points, component2, i);
				Vector2 vector = points[i];
				Vector2 normalized = (inverseNormalVector2 + inverseNormalVector).normalized;
				vector -= offset;
				float num = Math.Abs(vector.x);
				float num2 = Math.Abs(vector.y);
				float num3 = (!this.m_shrinkByPercent) ? this.m_shrinkAmount : (num * this.m_shrinkAmount);
				num3 = Mathf.Clamp(num3, 0f, num);
				num3 *= normalized.x;
				float num4 = (!this.m_shrinkByPercent) ? this.m_shrinkAmount : (num2 * this.m_shrinkAmount);
				num4 = Mathf.Clamp(num4, 0f, num2);
				num4 *= normalized.y;
				vector += offset;
				vector.x += num3;
				vector.y += num4;
				ShrinkCollider.m_colliderPointsHelper_STATIC[i] = vector;
			}
			component2.SetPath(0, ShrinkCollider.m_colliderPointsHelper_STATIC);
			flag = true;
		}
		else
		{
			BoxCollider2D component3 = base.GetComponent<BoxCollider2D>();
			if (component3 != null)
			{
				Vector2 size = component3.size;
				float num5 = (!this.m_shrinkByPercent) ? this.m_shrinkAmount : (size.x * this.m_shrinkAmount);
				float num6 = (!this.m_shrinkByPercent) ? this.m_shrinkAmount : (size.y * this.m_shrinkAmount);
				size.x -= num5;
				size.y -= num6;
				component3.size = size;
				flag = true;
			}
		}
		if (!flag)
		{
			Debug.Log("<color=red>TEDDY: NO COLLIDER FOUND ON HIDDEN WALL. THIS MEANS ALL EDGES ARE SET TO 1 ON THE FERR2D. FIX THIS!!! DO NOT IGNORE THIS MESSAGE!!!</color>");
		}
	}

	// Token: 0x060028CF RID: 10447 RVA: 0x000BF0EC File Offset: 0x000BD2EC
	private Vector2 GetInverseNormalVector(Vector2[] vertices, PolygonCollider2D collider, int index)
	{
		Vector2 b = vertices[index];
		index++;
		if (index >= vertices.Length)
		{
			index = 0;
		}
		Vector2 a = vertices[index];
		Vector2 b2 = (a + b) / 2f;
		Vector2 normalized = Vector2.Perpendicular(a - b).normalized;
		if (collider.OverlapPoint(collider.transform.position + b2 + normalized))
		{
			return normalized;
		}
		return -normalized;
	}

	// Token: 0x040023C0 RID: 9152
	private static List<Vector2> m_colliderPointsHelper_STATIC = new List<Vector2>();

	// Token: 0x040023C1 RID: 9153
	[SerializeField]
	private float m_shrinkAmount;

	// Token: 0x040023C2 RID: 9154
	[SerializeField]
	private bool m_shrinkByPercent;
}
