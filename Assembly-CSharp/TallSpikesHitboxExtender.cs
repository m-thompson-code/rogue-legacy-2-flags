using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200031A RID: 794
public class TallSpikesHitboxExtender : MonoBehaviour
{
	// Token: 0x06001F5E RID: 8030 RVA: 0x000648F3 File Offset: 0x00062AF3
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

	// Token: 0x06001F5F RID: 8031 RVA: 0x00064904 File Offset: 0x00062B04
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

	// Token: 0x06001F60 RID: 8032 RVA: 0x00064A0C File Offset: 0x00062C0C
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

	// Token: 0x04001C0F RID: 7183
	private static Vector2[] m_pointArrayHelper_STATIC = new Vector2[4];

	// Token: 0x04001C10 RID: 7184
	[SerializeField]
	private Vector2 m_platformDeltaSize;

	// Token: 0x04001C11 RID: 7185
	[SerializeField]
	private Vector2 m_hazardDeltaSize;

	// Token: 0x04001C12 RID: 7186
	[SerializeField]
	private float m_platformYOffset;

	// Token: 0x04001C13 RID: 7187
	[SerializeField]
	private float m_hazardYOffset;
}
