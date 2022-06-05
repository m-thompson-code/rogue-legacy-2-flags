using System;
using System.Collections.Generic;
using Cinemachine;
using Cinemachine.Utility;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class ZoomTest : CinemachineExtension
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public bool CameraWasDisplaced(CinemachineVirtualCameraBase vcam)
	{
		return base.GetExtraState<ZoomTest.VcamExtraState>(vcam).confinerDisplacement > 0f;
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002065 File Offset: 0x00000265
	private void OnValidate()
	{
		this.m_Damping = Mathf.Max(0f, this.m_Damping);
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00002080 File Offset: 0x00000280
	private bool ValidatePathCache()
	{
		this.m_pathCache = null;
		Type left = (this.m_BoundingShape2D == null) ? null : this.m_BoundingShape2D.GetType();
		if (left == typeof(PolygonCollider2D))
		{
			PolygonCollider2D polygonCollider2D = this.m_BoundingShape2D as PolygonCollider2D;
			if (this.m_pathCache == null || this.m_pathCache.Count != polygonCollider2D.pathCount)
			{
				this.m_pathCache = new List<List<Vector2>>();
				for (int i = 0; i < polygonCollider2D.pathCount; i++)
				{
					Vector2[] path = polygonCollider2D.GetPath(i);
					List<Vector2> list = new List<Vector2>();
					for (int j = 0; j < path.Length; j++)
					{
						list.Add(path[j]);
					}
					this.m_pathCache.Add(list);
				}
			}
			return true;
		}
		if (left == typeof(CompositeCollider2D))
		{
			CompositeCollider2D compositeCollider2D = this.m_BoundingShape2D as CompositeCollider2D;
			if (this.m_pathCache == null || this.m_pathCache.Count != compositeCollider2D.pathCount)
			{
				this.m_pathCache = new List<List<Vector2>>();
				Vector2[] array = new Vector2[compositeCollider2D.pointCount];
				for (int k = 0; k < compositeCollider2D.pathCount; k++)
				{
					int path2 = compositeCollider2D.GetPath(k, array);
					List<Vector2> list2 = new List<Vector2>();
					for (int l = 0; l < path2; l++)
					{
						list2.Add(array[l]);
					}
					this.m_pathCache.Add(list2);
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x06000004 RID: 4 RVA: 0x000021FC File Offset: 0x000003FC
	private Vector3 ConfinePointMod(Vector3 camPos)
	{
		if (this.m_BoundingShape2D.OverlapPoint(camPos))
		{
			return Vector3.zero;
		}
		if (!this.ValidatePathCache())
		{
			return camPos;
		}
		Vector2 vector = camPos;
		Vector2 vector2 = vector;
		Vector2 vector3 = vector;
		float num = float.MaxValue;
		float num2 = float.MaxValue;
		for (int i = 0; i < this.m_pathCache.Count; i++)
		{
			int count = this.m_pathCache[i].Count;
			if (count > 0)
			{
				Vector2 vector4 = this.m_BoundingShape2D.transform.TransformPoint(this.m_pathCache[i][count - 1]);
				for (int j = 0; j < count; j++)
				{
					Vector2 vector5 = this.m_BoundingShape2D.transform.TransformPoint(this.m_pathCache[i][j]);
					float num3 = vector.ClosestPointOnSegment(vector4, vector5);
					if ((num3 == 0f || num3 == 1f) && vector.x != vector4.x && vector.x != vector5.x && vector.y != vector4.y && vector.y != vector5.y)
					{
						vector4 = vector5;
					}
					else
					{
						Vector2 vector6 = Vector2.Lerp(vector4, vector5, num3);
						float num4 = Vector2.SqrMagnitude(vector - vector6);
						if (num4 < num)
						{
							num2 = num;
							vector3 = vector2;
							num = num4;
							vector2 = vector6;
						}
						else if (num4 < num2)
						{
							num2 = num4;
							vector3 = vector6;
						}
						vector4 = vector5;
					}
				}
			}
		}
		vector2 -= vector;
		vector3 -= vector;
		if (vector3.x != 0f && (Mathf.Abs(vector3.x) < Mathf.Abs(vector2.x) || vector2.x == 0f))
		{
			vector2.x = vector3.x;
		}
		if (vector3.y != 0f && (Mathf.Abs(vector3.y) < Mathf.Abs(vector2.y) || vector2.y == 0f))
		{
			vector2.y = vector3.y;
		}
		return vector2;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x0000242C File Offset: 0x0000062C
	private Vector3 ConfinePoint(Vector3 camPos)
	{
		if (this.m_BoundingShape2D.OverlapPoint(camPos))
		{
			return Vector3.zero;
		}
		if (!this.ValidatePathCache())
		{
			return camPos;
		}
		Vector2 vector = camPos;
		Vector2 a = vector;
		float num = float.MaxValue;
		for (int i = 0; i < this.m_pathCache.Count; i++)
		{
			int count = this.m_pathCache[i].Count;
			if (count > 0)
			{
				Vector2 vector2 = this.m_BoundingShape2D.transform.TransformPoint(this.m_pathCache[i][count - 1]);
				for (int j = 0; j < count; j++)
				{
					Vector2 vector3 = this.m_BoundingShape2D.transform.TransformPoint(this.m_pathCache[i][j]);
					Vector2 vector4 = Vector2.Lerp(vector2, vector3, vector.ClosestPointOnSegment(vector2, vector3));
					float num2 = Vector2.SqrMagnitude(vector - vector4);
					if (num2 < num)
					{
						num = num2;
						a = vector4;
					}
					vector2 = vector3;
				}
			}
		}
		return a - vector;
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002554 File Offset: 0x00000754
	protected virtual void PostPipelineStageCallback3(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
	{
		if (this.m_BoundingShape2D == null)
		{
			return;
		}
		Vector3 correctedPosition = state.CorrectedPosition;
		float maxOrthoSize = this.m_maxOrthoSize;
		float d = maxOrthoSize * state.Lens.Aspect;
		Vector3 b = Vector3.right * d;
		Vector3 b2 = Vector3.up * maxOrthoSize;
		Vector3 vector = correctedPosition - b + b2;
		Vector3 vector2 = correctedPosition - b - b2;
		Vector3 vector3 = correctedPosition + b + b2;
		Vector3 vector4 = correctedPosition + b - b2;
		Vector3 b3 = this.ConfinePointMod(vector);
		vector += b3;
		Vector3 b4 = this.ConfinePointMod(vector2);
		vector2 += b4;
		Vector3 b5 = this.ConfinePointMod(vector3);
		vector3 += b5;
		Vector3 b6 = this.ConfinePointMod(vector4);
		vector4 += b6;
		Vector2 vector5 = default(Vector2);
		vector5.x = ((vector.x > vector2.x) ? vector.x : vector2.x);
		vector5.y = ((vector.y < vector3.y) ? vector.y : vector3.y);
		Vector2 vector6 = default(Vector2);
		vector6.x = vector5.x;
		vector6.y = ((vector2.y > vector4.y) ? vector2.y : vector4.y);
		Vector2 vector7 = default(Vector2);
		vector7.x = ((vector3.x < vector4.x) ? vector3.x : vector4.x);
		vector7.y = vector5.y;
		Vector2 vector8 = default(Vector2);
		vector8.x = vector7.x;
		vector8.y = vector6.y;
	}

	// Token: 0x06000007 RID: 7 RVA: 0x0000272C File Offset: 0x0000092C
	protected virtual void PostPipelineStageCallback5(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
	{
		if (this.m_BoundingShape2D == null)
		{
			return;
		}
		if (stage == CinemachineCore.Stage.Body)
		{
			Vector3 correctedPosition = state.CorrectedPosition;
			float maxOrthoSize = this.m_maxOrthoSize;
			float num = maxOrthoSize * state.Lens.Aspect;
			float orthographicSize = state.Lens.OrthographicSize;
			float num2 = orthographicSize * state.Lens.Aspect;
			Vector3 b = Vector3.right * num;
			Vector3 b2 = Vector3.up * maxOrthoSize;
			Vector3 vector = correctedPosition - b + b2;
			Vector3 vector2 = correctedPosition - b - b2;
			Vector3 vector3 = correctedPosition + b + b2;
			Vector3 vector4 = correctedPosition + b - b2;
			Vector3 vector5 = this.ConfinePoint(vector);
			Vector3 vector6 = this.ConfinePoint(vector2);
			Vector3 vector7 = this.ConfinePoint(vector3);
			Vector3 vector8 = this.ConfinePoint(vector4);
			Vector3 zero = Vector3.zero;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;
			if (!this.AlmostZero(vector5.x) && this.AlmostZero(vector7.x))
			{
				Vector2 v = vector3 + Vector3.right * vector5.x;
				vector7 = this.ConfinePoint(v);
				zero.x = vector5.x;
				flag = true;
			}
			else if (this.AlmostZero(vector5.x) && !this.AlmostZero(vector7.x))
			{
				Vector2 v2 = vector + Vector3.right * vector7.x;
				vector5 = this.ConfinePoint(v2);
				zero.x = vector7.x;
				flag2 = true;
			}
			if (!this.AlmostZero(vector6.x) && this.AlmostZero(vector8.x))
			{
				Vector2 v3 = vector4 + Vector3.right * vector6.x;
				vector8 = this.ConfinePoint(v3);
				if (Mathf.Abs(vector6.x) > Mathf.Abs(zero.x))
				{
					zero.x = vector6.x;
				}
				flag3 = true;
			}
			else if (this.AlmostZero(vector6.x) && !this.AlmostZero(vector8.x))
			{
				Vector2 v4 = vector2 + Vector3.right * vector8.x;
				vector6 = this.ConfinePoint(v4);
				if (Mathf.Abs(vector8.x) > Mathf.Abs(zero.x))
				{
					zero.x = vector8.x;
				}
				flag4 = true;
			}
			if (!this.AlmostZero(vector5.y) && this.AlmostZero(vector6.y))
			{
				Vector2 v5 = vector2 + Vector3.up * vector5.y;
				vector6 = this.ConfinePoint(v5);
				zero.y = vector5.y;
				flag5 = true;
			}
			else if (this.AlmostZero(vector5.y) && !this.AlmostZero(vector6.y))
			{
				Vector2 v6 = vector + Vector3.up * vector6.y;
				vector5 = this.ConfinePoint(v6);
				zero.y = vector6.y;
				flag7 = true;
			}
			if (!this.AlmostZero(vector7.y) && this.AlmostZero(vector8.y))
			{
				Vector2 v7 = vector4 + Vector3.up * vector7.y;
				vector8 = this.ConfinePoint(v7);
				if (Mathf.Abs(vector7.y) > Mathf.Abs(zero.y))
				{
					zero.y = vector7.y;
				}
				flag6 = true;
			}
			else if (this.AlmostZero(vector7.y) && !this.AlmostZero(vector8.y))
			{
				Vector2 v8 = vector3 + Vector3.up * vector8.y;
				vector7 = this.ConfinePoint(v8);
				if (Mathf.Abs(vector8.y) > Mathf.Abs(zero.y))
				{
					zero.y = vector8.y;
				}
				flag8 = true;
			}
			if (flag)
			{
				vector5.x = 0f;
			}
			if (flag5)
			{
				vector5.y = 0f;
			}
			if (flag2)
			{
				vector7.x = 0f;
			}
			if (flag6)
			{
				vector7.y = 0f;
			}
			if (flag3)
			{
				vector6.x = 0f;
			}
			if (flag7)
			{
				vector6.y = 0f;
			}
			if (flag4)
			{
				vector8.x = 0f;
			}
			if (flag8)
			{
				vector8.y = 0f;
			}
			bool flag9 = (vector6.x != 0f && vector8.x != 0f) || (vector5.x != 0f && vector7.x != 0f);
			if (flag9)
			{
				zero.x = 0f;
			}
			bool flag10 = (vector6.y != 0f && vector5.y != 0f) || (vector8.y != 0f && vector7.y != 0f);
			if (flag10)
			{
				zero.y = 0f;
			}
			float num3 = vector3.x + vector7.x - (vector.x + vector5.x);
			float num4 = vector4.x + vector8.x - (vector2.x + vector6.x);
			float num5 = (num3 < num4) ? num3 : num4;
			float num6 = vector.y + vector5.y - (vector2.y + vector6.y);
			float num7 = vector3.y + vector7.y - (vector4.y + vector8.y);
			float num8 = (num6 < num7) ? num6 : num7;
			float num9 = maxOrthoSize / orthographicSize;
			float num10 = num8 / (orthographicSize * 2f);
			float num11 = num5 / (num2 * 2f);
			if (num10 < num9)
			{
				num9 = num10;
			}
			if (num11 < num9)
			{
				num9 = num11;
			}
			LensSettings lens = state.Lens;
			lens.OrthographicSize *= num9;
			Debug.Log("percent is: " + num9.ToString() + " new orthosize: " + lens.OrthographicSize.ToString());
			state.Lens = lens;
			float num12 = maxOrthoSize - lens.OrthographicSize;
			float num13 = num - lens.OrthographicSize * state.Lens.Aspect;
			if (flag10)
			{
				float num14 = (vector5.y < vector7.y) ? vector5.y : vector7.y;
				float num15 = (vector6.y > vector8.y) ? vector6.y : vector8.y;
				float y = (Mathf.Abs(num14) > Mathf.Abs(num15)) ? num14 : num15;
				zero.y = y;
			}
			if (Mathf.Abs(zero.y) > num12)
			{
				if (zero.y < 0f)
				{
					zero.y += num12;
				}
				else if (zero.y > 0f)
				{
					zero.y -= num12;
				}
			}
			else
			{
				zero.y = 0f;
			}
			if (flag9)
			{
				float num16 = (vector5.x > vector6.x) ? vector5.x : vector6.x;
				float num17 = (vector7.x < vector8.x) ? vector7.x : vector8.x;
				float num18 = (Mathf.Abs(num16) > Mathf.Abs(num17)) ? num16 : num17;
				zero.x += num18;
			}
			if (Mathf.Abs(zero.x) > num13)
			{
				if (zero.x < 0f)
				{
					zero.x += num13;
				}
				else if (zero.x > 0f)
				{
					zero.x -= num13;
				}
			}
			else
			{
				zero.x = 0f;
			}
			state.PositionCorrection += zero;
		}
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00002F6C File Offset: 0x0000116C
	protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
	{
		if (this.m_BoundingShape2D == null)
		{
			return;
		}
		if (stage == CinemachineCore.Stage.Body)
		{
			Vector3 correctedPosition = state.CorrectedPosition;
			float maxOrthoSize = this.m_maxOrthoSize;
			float d = maxOrthoSize * state.Lens.Aspect;
			float orthographicSize = state.Lens.OrthographicSize;
			float num = orthographicSize * state.Lens.Aspect;
			Vector3 b = Vector3.right * d;
			Vector3 b2 = Vector3.up * maxOrthoSize;
			Vector3 vector = correctedPosition - b + b2;
			Vector3 vector2 = correctedPosition - b - b2;
			Vector3 vector3 = correctedPosition + b + b2;
			Vector3 vector4 = correctedPosition + b - b2;
			Vector3 vector5 = this.ConfinePoint(vector);
			Vector3 vector6 = this.ConfinePoint(vector2);
			Vector3 vector7 = this.ConfinePoint(vector3);
			Vector3 vector8 = this.ConfinePoint(vector4);
			if (!this.AlmostZero(vector5.x) && this.AlmostZero(vector7.x))
			{
				vector3 += Vector3.right * vector5.x;
				vector += Vector3.right * vector5.x;
				vector7 = this.ConfinePoint(vector3);
				vector5 = this.ConfinePoint(vector);
			}
			else if (this.AlmostZero(vector5.x) && !this.AlmostZero(vector7.x))
			{
				vector3 += Vector3.right * vector7.x;
				vector += Vector3.right * vector7.x;
				vector7 = this.ConfinePoint(vector3);
				vector5 = this.ConfinePoint(vector);
			}
			if (!this.AlmostZero(vector6.x) && this.AlmostZero(vector8.x))
			{
				vector4 += Vector3.right * vector6.x;
				vector2 += Vector3.right * vector6.x;
				vector8 = this.ConfinePoint(vector4);
				vector6 = this.ConfinePoint(vector2);
			}
			else if (this.AlmostZero(vector6.x) && !this.AlmostZero(vector8.x))
			{
				vector4 += Vector3.right * vector8.x;
				vector2 += Vector3.right * vector8.x;
				vector8 = this.ConfinePoint(vector4);
				vector6 = this.ConfinePoint(vector2);
			}
			if (!this.AlmostZero(vector5.y) && this.AlmostZero(vector6.y))
			{
				vector2 += Vector3.up * vector5.y;
				vector += Vector3.up * vector5.y;
				vector6 = this.ConfinePoint(vector2);
				vector5 = this.ConfinePoint(vector);
			}
			else if (this.AlmostZero(vector5.y) && !this.AlmostZero(vector6.y))
			{
				vector2 += Vector3.up * vector6.y;
				vector += Vector3.up * vector6.y;
				vector6 = this.ConfinePoint(vector2);
				vector5 = this.ConfinePoint(vector);
			}
			if (!this.AlmostZero(vector7.y) && this.AlmostZero(vector8.y))
			{
				vector4 += Vector3.up * vector7.y;
				vector3 += Vector3.up * vector7.y;
				vector8 = this.ConfinePoint(vector4);
				vector7 = this.ConfinePoint(vector3);
			}
			else if (this.AlmostZero(vector7.y) && !this.AlmostZero(vector8.y))
			{
				vector4 += Vector3.up * vector8.y;
				vector3 += Vector3.up * vector8.y;
				vector8 = this.ConfinePoint(vector4);
				vector7 = this.ConfinePoint(vector3);
			}
			float num2 = vector3.x + vector7.x - (vector.x + vector5.x);
			float num3 = vector4.x + vector8.x - (vector2.x + vector6.x);
			float num4 = (num2 < num3) ? num2 : num3;
			float num5 = vector.y + vector5.y - (vector2.y + vector6.y);
			float num6 = vector3.y + vector7.y - (vector4.y + vector8.y);
			float num7 = (num5 < num6) ? num5 : num6;
			float num8 = maxOrthoSize / orthographicSize;
			float num9 = num7 / (orthographicSize * 2f);
			float num10 = num4 / (num * 2f);
			if (num9 < num8)
			{
				num8 = num9;
			}
			if (num10 < num8)
			{
				num8 = num10;
			}
			LensSettings lens = state.Lens;
			lens.OrthographicSize *= num8;
			state.Lens = lens;
			orthographicSize = lens.OrthographicSize;
			num = orthographicSize * state.Lens.Aspect;
			b = Vector3.right * num;
			b2 = Vector3.up * orthographicSize;
			vector = correctedPosition - b + b2;
			vector2 = correctedPosition - b - b2;
			vector3 = correctedPosition + b + b2;
			vector4 = correctedPosition + b - b2;
			vector5 = this.ConfinePoint(vector);
			vector6 = this.ConfinePoint(vector2);
			vector7 = this.ConfinePoint(vector3);
			vector8 = this.ConfinePoint(vector4);
			float num11 = (vector5.x > vector6.x) ? vector5.x : vector6.x;
			float num12 = (vector7.x < vector8.x) ? vector7.x : vector8.x;
			float x = (Mathf.Abs(num11) > Mathf.Abs(num12)) ? num11 : num12;
			float num13 = (vector5.y < vector7.y) ? vector5.y : vector7.y;
			float num14 = (vector6.y > vector8.y) ? vector6.y : vector8.y;
			float y = (Mathf.Abs(num13) > Mathf.Abs(num14)) ? num13 : num14;
			state.PositionCorrection = new Vector3(x, y);
		}
	}

	// Token: 0x06000009 RID: 9 RVA: 0x000035D5 File Offset: 0x000017D5
	private bool AlmostZero(float value)
	{
		return value < 0.01f && value > -0.01f;
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000035EC File Offset: 0x000017EC
	protected void PostPipelineStageCallbackOld(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
	{
		if (this.m_BoundingShape2D == null)
		{
			return;
		}
		if (stage == CinemachineCore.Stage.Body)
		{
			Vector3 vector;
			if (this.m_ConfineScreenEdges && state.Lens.Orthographic)
			{
				vector = this.ConfineScreenEdges(vcam, ref state);
			}
			else
			{
				vector = this.ConfinePoint(state.CorrectedPosition);
			}
			ZoomTest.VcamExtraState extraState = base.GetExtraState<ZoomTest.VcamExtraState>(vcam);
			if (this.m_Damping > 0f && deltaTime >= 0f)
			{
				Vector3 vector2 = vector - extraState.m_previousDisplacement;
				vector2 = Damper.Damp(vector2, this.m_Damping, deltaTime);
				vector = extraState.m_previousDisplacement + vector2;
			}
			extraState.m_previousDisplacement = vector;
			state.PositionCorrection += vector;
			extraState.confinerDisplacement = vector.magnitude;
			Vector3 correctedPosition = state.CorrectedPosition;
			float orthographicSize = state.Lens.OrthographicSize;
			float num = orthographicSize * state.Lens.Aspect;
			float maxOrthoSize = this.m_maxOrthoSize;
			float minOrthoSize = this.m_minOrthoSize;
			float num2 = maxOrthoSize * state.Lens.Aspect;
			num2 = (float)Mathf.RoundToInt(num2);
			Vector3 b = Vector3.right * num2;
			Vector3 b2 = Vector3.up * maxOrthoSize;
			Vector3 vector3 = correctedPosition - b - b2;
			Vector3 vector4 = correctedPosition - b + b2;
			Vector3 vector5 = correctedPosition + b - b2;
			Vector3 vector6 = correctedPosition + b + b2;
			Vector3 vector7 = Vector3.zero;
			Vector3 vector8 = Vector3.zero;
			Vector3 vector9 = Vector3.zero;
			Vector3 vector10 = Vector3.zero;
			vector7 = this.ConfinePoint(vector3);
			vector8 = this.ConfinePoint(vector5);
			vector9 = this.ConfinePoint(vector4);
			vector10 = this.ConfinePoint(vector6);
			bool flag = (vector7.x != 0f && vector8.x != 0f) || (vector9.x != 0f && vector10.x != 0f);
			bool flag2 = (vector7.y != 0f && vector9.y != 0f) || (vector8.y != 0f && vector10.y != 0f);
			vector3 += vector7;
			vector5 += vector8;
			vector4 += vector9;
			vector6 += vector10;
			float num3 = vector5.x - vector3.x;
			float num4 = vector6.x - vector4.x;
			float num5 = ((num3 < num4) ? num3 : num4) / (num * 2f);
			float num6 = vector4.y - vector3.y;
			float num7 = vector6.y - vector5.y;
			float num8 = ((num6 < num7) ? num6 : num7) / (orthographicSize * 2f);
			float num9 = maxOrthoSize / orthographicSize;
			if (flag2 && num8 < num9)
			{
				num9 = num8;
			}
			if (flag && num5 < num9)
			{
				num9 = num5;
			}
			float num10 = minOrthoSize / orthographicSize;
			if (num9 < num10)
			{
				num9 = num10;
			}
			LensSettings lens = state.Lens;
			lens.OrthographicSize *= num9;
			Debug.Log("percent is: " + num9.ToString() + " new orthosize: " + lens.OrthographicSize.ToString());
			state.Lens = lens;
		}
	}

	// Token: 0x0600000B RID: 11 RVA: 0x00003938 File Offset: 0x00001B38
	private Vector3 ConfineScreenEdges(CinemachineVirtualCameraBase vcam, ref CameraState state)
	{
		Quaternion rotation = Quaternion.Inverse(state.CorrectedOrientation);
		float orthographicSize = state.Lens.OrthographicSize;
		float num = orthographicSize * state.Lens.Aspect;
		num = (float)Mathf.RoundToInt(num);
		Vector3 b = rotation * Vector3.right * num;
		Vector3 b2 = rotation * Vector3.up * orthographicSize;
		Vector3 vector = Vector3.zero;
		Vector3 a = state.CorrectedPosition;
		for (int i = 0; i < 12; i++)
		{
			Vector3 vector2 = this.ConfinePoint(a - b2 - b);
			if (vector2.AlmostZero())
			{
				vector2 = this.ConfinePoint(a - b2 + b);
			}
			if (vector2.AlmostZero())
			{
				vector2 = this.ConfinePoint(a + b2 - b);
			}
			if (vector2.AlmostZero())
			{
				vector2 = this.ConfinePoint(a + b2 + b);
			}
			if (vector2.AlmostZero())
			{
				break;
			}
			vector += vector2;
			a += vector2;
		}
		return vector;
	}

	// Token: 0x04000001 RID: 1
	[SerializeField]
	private float m_maxOrthoSize = 12f;

	// Token: 0x04000002 RID: 2
	[SerializeField]
	private float m_minOrthoSize = 7f;

	// Token: 0x04000003 RID: 3
	[SerializeField]
	private bool ConstrainToBounds;

	// Token: 0x04000004 RID: 4
	[SerializeField]
	public Collider2D m_BoundingShape2D;

	// Token: 0x04000005 RID: 5
	private List<List<Vector2>> m_pathCache;

	// Token: 0x04000006 RID: 6
	public bool m_ConfineScreenEdges = true;

	// Token: 0x04000007 RID: 7
	[Range(0f, 10f)]
	public float m_Damping;

	// Token: 0x02000979 RID: 2425
	private class VcamExtraState
	{
		// Token: 0x04004495 RID: 17557
		public Vector3 m_previousDisplacement;

		// Token: 0x04004496 RID: 17558
		public float confinerDisplacement;
	}
}
