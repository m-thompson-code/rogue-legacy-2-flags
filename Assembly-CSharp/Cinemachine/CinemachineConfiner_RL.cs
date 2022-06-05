using System;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x0200087D RID: 2173
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu("")]
	[SaveDuringPlay]
	[ExecuteAlways]
	[DisallowMultipleComponent]
	public class CinemachineConfiner_RL : CinemachineExtension
	{
		// Token: 0x0600479C RID: 18332 RVA: 0x0010187C File Offset: 0x000FFA7C
		public bool CameraWasDisplaced(CinemachineVirtualCameraBase vcam)
		{
			return this.GetCameraDisplacementDistance(vcam) > 0f;
		}

		// Token: 0x0600479D RID: 18333 RVA: 0x0010188C File Offset: 0x000FFA8C
		public float GetCameraDisplacementDistance(CinemachineVirtualCameraBase vcam)
		{
			return base.GetExtraState<CinemachineConfiner_RL.VcamExtraState>(vcam).confinerDisplacement;
		}

		// Token: 0x0600479E RID: 18334 RVA: 0x0010189A File Offset: 0x000FFA9A
		private void OnValidate()
		{
			this.m_Damping = Mathf.Max(0f, this.m_Damping);
		}

		// Token: 0x0600479F RID: 18335 RVA: 0x001018B2 File Offset: 0x000FFAB2
		protected override void ConnectToVcam(bool connect)
		{
			base.ConnectToVcam(connect);
		}

		// Token: 0x1700177D RID: 6013
		// (get) Token: 0x060047A0 RID: 18336 RVA: 0x001018BC File Offset: 0x000FFABC
		public bool IsValid
		{
			get
			{
				return (this.m_ConfineMode == CinemachineConfiner_RL.Mode.Confine3D && this.m_BoundingVolume != null && this.m_BoundingVolume.enabled && this.m_BoundingVolume.gameObject.activeInHierarchy) || (this.m_ConfineMode == CinemachineConfiner_RL.Mode.Confine2D && this.m_BoundingShape2D != null && this.m_BoundingShape2D.enabled && this.m_BoundingShape2D.gameObject.activeInHierarchy);
			}
		}

		// Token: 0x060047A1 RID: 18337 RVA: 0x00101936 File Offset: 0x000FFB36
		public override float GetMaxDampTime()
		{
			return this.m_Damping;
		}

		// Token: 0x060047A2 RID: 18338 RVA: 0x00101940 File Offset: 0x000FFB40
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

		// Token: 0x060047A3 RID: 18339 RVA: 0x00101A08 File Offset: 0x000FFC08
		public void InvalidatePathCache()
		{
			this.m_pathCache = null;
			this.m_BoundingShape2DCache = null;
		}

		// Token: 0x060047A4 RID: 18340 RVA: 0x00101A18 File Offset: 0x000FFC18
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

		// Token: 0x060047A5 RID: 18341 RVA: 0x00101C2C File Offset: 0x000FFE2C
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

		// Token: 0x060047A6 RID: 18342 RVA: 0x00101D92 File Offset: 0x000FFF92
		private bool AlmostZero(Vector3 v)
		{
			return v.sqrMagnitude < 0.0001f;
		}

		// Token: 0x060047A7 RID: 18343 RVA: 0x00101DA4 File Offset: 0x000FFFA4
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

		// Token: 0x04003C6C RID: 15468
		[Tooltip("The confiner can operate using a 2D bounding shape or a 3D bounding volume")]
		public CinemachineConfiner_RL.Mode m_ConfineMode;

		// Token: 0x04003C6D RID: 15469
		[Tooltip("The volume within which the camera is to be contained")]
		public Collider m_BoundingVolume;

		// Token: 0x04003C6E RID: 15470
		[Tooltip("The 2D shape within which the camera is to be contained")]
		public Collider2D m_BoundingShape2D;

		// Token: 0x04003C6F RID: 15471
		private Collider2D m_BoundingShape2DCache;

		// Token: 0x04003C70 RID: 15472
		[Tooltip("If camera is orthographic, screen edges will be confined to the volume.  If not checked, then only the camera center will be confined")]
		public bool m_ConfineScreenEdges = true;

		// Token: 0x04003C71 RID: 15473
		[Tooltip("How gradually to return the camera to the bounding volume if it goes beyond the borders.  Higher numbers are more gradual.")]
		[Range(0f, 10f)]
		public float m_Damping;

		// Token: 0x04003C72 RID: 15474
		private List<List<Vector2>> m_pathCache;

		// Token: 0x04003C73 RID: 15475
		private int m_pathTotalPointCount;

		// Token: 0x02000E8B RID: 3723
		public enum Mode
		{
			// Token: 0x0400586B RID: 22635
			Confine2D,
			// Token: 0x0400586C RID: 22636
			Confine3D
		}

		// Token: 0x02000E8C RID: 3724
		private class VcamExtraState
		{
			// Token: 0x0400586D RID: 22637
			public Vector3 m_previousDisplacement;

			// Token: 0x0400586E RID: 22638
			public float confinerDisplacement;
		}
	}
}
