using System;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000D97 RID: 3479
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu("")]
	[SaveDuringPlay]
	[ExecuteAlways]
	[DisallowMultipleComponent]
	public class CinemachineConfiner_RL : CinemachineExtension
	{
		// Token: 0x06006299 RID: 25241 RVA: 0x000364F3 File Offset: 0x000346F3
		public bool CameraWasDisplaced(CinemachineVirtualCameraBase vcam)
		{
			return this.GetCameraDisplacementDistance(vcam) > 0f;
		}

		// Token: 0x0600629A RID: 25242 RVA: 0x00036503 File Offset: 0x00034703
		public float GetCameraDisplacementDistance(CinemachineVirtualCameraBase vcam)
		{
			return base.GetExtraState<CinemachineConfiner_RL.VcamExtraState>(vcam).confinerDisplacement;
		}

		// Token: 0x0600629B RID: 25243 RVA: 0x00036511 File Offset: 0x00034711
		private void OnValidate()
		{
			this.m_Damping = Mathf.Max(0f, this.m_Damping);
		}

		// Token: 0x0600629C RID: 25244 RVA: 0x00036529 File Offset: 0x00034729
		protected override void ConnectToVcam(bool connect)
		{
			base.ConnectToVcam(connect);
		}

		// Token: 0x17001FDF RID: 8159
		// (get) Token: 0x0600629D RID: 25245 RVA: 0x001702D0 File Offset: 0x0016E4D0
		public bool IsValid
		{
			get
			{
				return (this.m_ConfineMode == CinemachineConfiner_RL.Mode.Confine3D && this.m_BoundingVolume != null && this.m_BoundingVolume.enabled && this.m_BoundingVolume.gameObject.activeInHierarchy) || (this.m_ConfineMode == CinemachineConfiner_RL.Mode.Confine2D && this.m_BoundingShape2D != null && this.m_BoundingShape2D.enabled && this.m_BoundingShape2D.gameObject.activeInHierarchy);
			}
		}

		// Token: 0x0600629E RID: 25246 RVA: 0x00036532 File Offset: 0x00034732
		public override float GetMaxDampTime()
		{
			return this.m_Damping;
		}

		// Token: 0x0600629F RID: 25247 RVA: 0x0017034C File Offset: 0x0016E54C
		protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
		{
			if (this.IsValid && stage == CinemachineCore.Stage.Body)
			{
				CinemachineConfiner_RL.VcamExtraState extraState = base.GetExtraState<CinemachineConfiner_RL.VcamExtraState>(vcam);
				Vector3 vector;
				if (this.m_ConfineScreenEdges && state.Lens.Orthographic)
				{
					vector = this.ConfineScreenEdges(vcam, ref state);
				}
				else
				{
					vector = this.ConfinePoint(state.CorrectedPosition);
				}
				if (this.m_Damping > 0f && deltaTime >= 0f && base.VirtualCamera.PreviousStateIsValid)
				{
					Vector3 vector2 = vector - extraState.m_previousDisplacement;
					vector2 = Damper.Damp(vector2, this.m_Damping, deltaTime);
					vector = extraState.m_previousDisplacement + vector2;
				}
				extraState.m_previousDisplacement = vector;
				state.PositionCorrection += vector;
				extraState.confinerDisplacement = vector.magnitude;
			}
		}

		// Token: 0x060062A0 RID: 25248 RVA: 0x0003653A File Offset: 0x0003473A
		public void InvalidatePathCache()
		{
			this.m_pathCache = null;
			this.m_BoundingShape2DCache = null;
		}

		// Token: 0x060062A1 RID: 25249 RVA: 0x00170414 File Offset: 0x0016E614
		private bool ValidatePathCache()
		{
			if (this.m_BoundingShape2DCache != this.m_BoundingShape2D)
			{
				this.InvalidatePathCache();
				this.m_BoundingShape2DCache = this.m_BoundingShape2D;
			}
			Type left = (this.m_BoundingShape2D == null) ? null : this.m_BoundingShape2D.GetType();
			if (left == typeof(PolygonCollider2D))
			{
				PolygonCollider2D polygonCollider2D = this.m_BoundingShape2D as PolygonCollider2D;
				if (this.m_pathCache == null || this.m_pathCache.Count != polygonCollider2D.pathCount || this.m_pathTotalPointCount != polygonCollider2D.GetTotalPointCount())
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
					this.m_pathTotalPointCount = polygonCollider2D.GetTotalPointCount();
				}
				return true;
			}
			if (left == typeof(CompositeCollider2D))
			{
				CompositeCollider2D compositeCollider2D = this.m_BoundingShape2D as CompositeCollider2D;
				if (this.m_pathCache == null || this.m_pathCache.Count != compositeCollider2D.pathCount || this.m_pathTotalPointCount != compositeCollider2D.pointCount)
				{
					this.m_pathCache = new List<List<Vector2>>();
					Vector2[] array = new Vector2[compositeCollider2D.pointCount];
					Vector3 lossyScale = this.m_BoundingShape2D.transform.lossyScale;
					Vector2 b = new Vector2(1f / lossyScale.x, 1f / lossyScale.y);
					for (int k = 0; k < compositeCollider2D.pathCount; k++)
					{
						int path2 = compositeCollider2D.GetPath(k, array);
						List<Vector2> list2 = new List<Vector2>();
						for (int l = 0; l < path2; l++)
						{
							list2.Add(array[l] * b);
						}
						this.m_pathCache.Add(list2);
					}
					this.m_pathTotalPointCount = compositeCollider2D.pointCount;
				}
				return true;
			}
			this.InvalidatePathCache();
			return false;
		}

		// Token: 0x060062A2 RID: 25250 RVA: 0x00170628 File Offset: 0x0016E828
		private Vector3 ConfinePoint(Vector3 camPos)
		{
			if (this.m_ConfineMode == CinemachineConfiner_RL.Mode.Confine3D)
			{
				return this.m_BoundingVolume.ClosestPoint(camPos) - camPos;
			}
			Vector2 vector = camPos;
			Vector2 a = vector;
			if (this.m_BoundingShape2D.OverlapPoint(camPos))
			{
				return Vector3.zero;
			}
			if (!this.ValidatePathCache())
			{
				return Vector3.zero;
			}
			float num = float.MaxValue;
			for (int i = 0; i < this.m_pathCache.Count; i++)
			{
				int count = this.m_pathCache[i].Count;
				if (count > 0)
				{
					Vector2 vector2 = this.m_BoundingShape2D.transform.TransformPoint(this.m_pathCache[i][count - 1] + this.m_BoundingShape2D.offset);
					for (int j = 0; j < count; j++)
					{
						Vector2 vector3 = this.m_BoundingShape2D.transform.TransformPoint(this.m_pathCache[i][j] + this.m_BoundingShape2D.offset);
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

		// Token: 0x060062A3 RID: 25251 RVA: 0x0003654A File Offset: 0x0003474A
		private bool AlmostZero(Vector3 v)
		{
			return v.sqrMagnitude < 0.0001f;
		}

		// Token: 0x060062A4 RID: 25252 RVA: 0x00170790 File Offset: 0x0016E990
		private Vector3 ConfineScreenEdges(CinemachineVirtualCameraBase vcam, ref CameraState state)
		{
			Quaternion rotation = Quaternion.Inverse(state.CorrectedOrientation);
			float orthographicSize = state.Lens.OrthographicSize;
			float d = orthographicSize * state.Lens.Aspect;
			Vector3 b = rotation * Vector3.right * d;
			Vector3 b2 = rotation * Vector3.up * orthographicSize;
			Vector3 vector = Vector3.zero;
			Vector3 a = state.CorrectedPosition;
			Vector3 b3 = Vector3.zero;
			for (int i = 0; i < 12; i++)
			{
				Vector3 vector2 = this.ConfinePoint(a - b2 - b);
				if (this.AlmostZero(vector2))
				{
					vector2 = this.ConfinePoint(a + b2 + b);
				}
				if (this.AlmostZero(vector2))
				{
					vector2 = this.ConfinePoint(a - b2 + b);
				}
				if (this.AlmostZero(vector2))
				{
					vector2 = this.ConfinePoint(a + b2 - b);
				}
				if (this.AlmostZero(vector2))
				{
					break;
				}
				if (this.AlmostZero(vector2 + b3))
				{
					vector += vector2 * 0.5f;
					break;
				}
				vector += vector2;
				a += vector2;
				b3 = vector2;
			}
			return vector;
		}

		// Token: 0x04005062 RID: 20578
		[Tooltip("The confiner can operate using a 2D bounding shape or a 3D bounding volume")]
		public CinemachineConfiner_RL.Mode m_ConfineMode;

		// Token: 0x04005063 RID: 20579
		[Tooltip("The volume within which the camera is to be contained")]
		public Collider m_BoundingVolume;

		// Token: 0x04005064 RID: 20580
		[Tooltip("The 2D shape within which the camera is to be contained")]
		public Collider2D m_BoundingShape2D;

		// Token: 0x04005065 RID: 20581
		private Collider2D m_BoundingShape2DCache;

		// Token: 0x04005066 RID: 20582
		[Tooltip("If camera is orthographic, screen edges will be confined to the volume.  If not checked, then only the camera center will be confined")]
		public bool m_ConfineScreenEdges = true;

		// Token: 0x04005067 RID: 20583
		[Tooltip("How gradually to return the camera to the bounding volume if it goes beyond the borders.  Higher numbers are more gradual.")]
		[Range(0f, 10f)]
		public float m_Damping;

		// Token: 0x04005068 RID: 20584
		private List<List<Vector2>> m_pathCache;

		// Token: 0x04005069 RID: 20585
		private int m_pathTotalPointCount;

		// Token: 0x02000D98 RID: 3480
		public enum Mode
		{
			// Token: 0x0400506B RID: 20587
			Confine2D,
			// Token: 0x0400506C RID: 20588
			Confine3D
		}

		// Token: 0x02000D99 RID: 3481
		private class VcamExtraState
		{
			// Token: 0x0400506D RID: 20589
			public Vector3 m_previousDisplacement;

			// Token: 0x0400506E RID: 20590
			public float confinerDisplacement;
		}
	}
}
