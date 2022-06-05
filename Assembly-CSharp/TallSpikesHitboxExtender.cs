using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200055D RID: 1373
public class TallSpikesHitboxExtender : MonoBehaviour
{
	// Token: 0x06002BED RID: 11245 RVA: 0x000186BF File Offset: 0x000168BF
	private IEnumerator Start()
	{
		IHitboxController hbController = base.GetComponentInChildren<IHitboxController>();
		while (!hbController.IsInitialized)
		{
			yield return null;
		}
		Ferr2DT_PathTerrain component = base.GetComponent<Ferr2DT_PathTerrain>();
		if (component != null)
		{
			if (component.PathData.Count == 2)
			{
				float angle = CDGHelper.WrapAngleDegrees(CDGHelper.AngleBetweenPts(component.PathData.End, component.PathData.Start), true);
				Vector2 vector = component.PathData.End - component.PathData.Start;
				vector = Vector2.Perpendicular(vector).normalized;
				PolygonCollider2D polygonCollider2D = hbController.GetCollider(HitboxType.Platform) as PolygonCollider2D;
				BoxCollider2D boxCollider2D = hbController.GetCollider(HitboxType.Platform) as BoxCollider2D;
				if (polygonCollider2D && polygonCollider2D.GetTotalPointCount() == 4)
				{
					this.ChangePolyColliderSize(polygonCollider2D, this.m_platformDeltaSize, angle, vector * -this.m_platformYOffset);
				}
				else if (boxCollider2D)
				{
					this.ChangeBoxColliderSize(boxCollider2D, this.m_platformDeltaSize, this.m_platformYOffset, angle);
				}
				else
				{
					Debug.Log("<color=yellow>Could not find valid Platform collider for TallSpike: " + base.name + "</color>");
				}
				polygonCollider2D = (hbController.GetCollider(HitboxType.Terrain) as PolygonCollider2D);
				boxCollider2D = (hbController.GetCollider(HitboxType.Terrain) as BoxCollider2D);
				if (polygonCollider2D && polygonCollider2D.GetTotalPointCount() == 4)
				{
					this.ChangePolyColliderSize(polygonCollider2D, this.m_hazardDeltaSize, angle, vector * -this.m_hazardYOffset);
				}
				else if (boxCollider2D)
				{
					this.ChangeBoxColliderSize(boxCollider2D, this.m_hazardDeltaSize, this.m_hazardYOffset, angle);
				}
				else
				{
					Debug.Log("<color=yellow>Could not find valid Terrain collider for TallSpike: " + base.name + "</color>");
				}
			}
			else
			{
				Debug.Log("<color=yellow>TallSpike: " + base.name + " does not contain a Ferr2D component.</color>");
			}
		}
		yield break;
	}

	// Token: 0x06002BEE RID: 11246 RVA: 0x000C4B34 File Offset: 0x000C2D34
	private void ChangePolyColliderSize(PolygonCollider2D polyCollider, Vector2 deltaSize, float angle, Vector2 offsetVector)
	{
		Vector2 theOrigin = polyCollider.bounds.center - polyCollider.transform.position;
		Array.Clear(TallSpikesHitboxExtender.m_pointArrayHelper_STATIC, 0, TallSpikesHitboxExtender.m_pointArrayHelper_STATIC.Length);
		float num = deltaSize.x * 0.5f;
		float num2 = deltaSize.y * 0.5f;
		for (int i = 0; i < polyCollider.GetTotalPointCount(); i++)
		{
			Vector2 vector = CDGHelper.RotatedPoint(polyCollider.points[i], theOrigin, -angle);
			if (i == 0 || i == 3)
			{
				vector.x += num;
			}
			else
			{
				vector.x -= num;
			}
			if (i == 0 || i == 1)
			{
				vector.y += num2;
			}
			else
			{
				vector.y -= num2;
			}
			vector = CDGHelper.RotatedPoint(vector, theOrigin, angle);
			vector += offsetVector;
			TallSpikesHitboxExtender.m_pointArrayHelper_STATIC[i] = vector;
		}
		polyCollider.SetPath(0, TallSpikesHitboxExtender.m_pointArrayHelper_STATIC);
	}

	// Token: 0x06002BEF RID: 11247 RVA: 0x000C4C3C File Offset: 0x000C2E3C
	private void ChangeBoxColliderSize(BoxCollider2D boxCollider, Vector2 deltaSize, float yOffset, float angle)
	{
		if (Mathf.Approximately(angle, 90f) || Mathf.Approximately(angle, -90f))
		{
			Vector2 size = boxCollider.size;
			size.y += deltaSize.x;
			size.x += deltaSize.y;
			boxCollider.size = size;
			if (Mathf.Approximately(angle, -90f))
			{
				Vector2 offset = boxCollider.offset;
				offset.x += yOffset;
				boxCollider.offset = offset;
				return;
			}
			Vector2 offset2 = boxCollider.offset;
			offset2.x -= yOffset;
			boxCollider.offset = offset2;
			return;
		}
		else
		{
			Vector2 size2 = boxCollider.size;
			size2.x += deltaSize.x;
			size2.y += deltaSize.y;
			boxCollider.size = size2;
			if (Mathf.Approximately(angle, 0f))
			{
				Vector2 offset3 = boxCollider.offset;
				offset3.y += yOffset;
				boxCollider.offset = offset3;
				return;
			}
			Vector2 offset4 = boxCollider.offset;
			offset4.y -= yOffset;
			boxCollider.offset = offset4;
			return;
		}
	}

	// Token: 0x04002525 RID: 9509
	private static Vector2[] m_pointArrayHelper_STATIC = new Vector2[4];

	// Token: 0x04002526 RID: 9510
	[SerializeField]
	private Vector2 m_platformDeltaSize;

	// Token: 0x04002527 RID: 9511
	[SerializeField]
	private Vector2 m_hazardDeltaSize;

	// Token: 0x04002528 RID: 9512
	[SerializeField]
	private float m_platformYOffset;

	// Token: 0x04002529 RID: 9513
	[SerializeField]
	private float m_hazardYOffset;
}
