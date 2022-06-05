using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x0200085B RID: 2139
	public class ObjectSpin : MonoBehaviour
	{
		// Token: 0x060046F1 RID: 18161 RVA: 0x000FE0CC File Offset: 0x000FC2CC
		private void Awake()
		{
			this.m_transform = base.transform;
			this.m_initial_Rotation = this.m_transform.rotation.eulerAngles;
			this.m_initial_Position = this.m_transform.position;
			Light component = base.GetComponent<Light>();
			this.m_lightColor = ((component != null) ? component.color : Color.black);
		}

		// Token: 0x060046F2 RID: 18162 RVA: 0x000FE138 File Offset: 0x000FC338
		private void Update()
		{
			if (this.Motion == ObjectSpin.MotionType.Rotation)
			{
				this.m_transform.Rotate(0f, this.SpinSpeed * Time.deltaTime, 0f);
				return;
			}
			if (this.Motion == ObjectSpin.MotionType.BackAndForth)
			{
				this.m_time += this.SpinSpeed * Time.deltaTime;
				this.m_transform.rotation = Quaternion.Euler(this.m_initial_Rotation.x, Mathf.Sin(this.m_time) * (float)this.RotationRange + this.m_initial_Rotation.y, this.m_initial_Rotation.z);
				return;
			}
			this.m_time += this.SpinSpeed * Time.deltaTime;
			float x = 15f * Mathf.Cos(this.m_time * 0.95f);
			float z = 10f;
			float y = 0f;
			this.m_transform.position = this.m_initial_Position + new Vector3(x, y, z);
			this.m_prevPOS = this.m_transform.position;
			this.frames++;
		}

		// Token: 0x04003BF5 RID: 15349
		public float SpinSpeed = 5f;

		// Token: 0x04003BF6 RID: 15350
		public int RotationRange = 15;

		// Token: 0x04003BF7 RID: 15351
		private Transform m_transform;

		// Token: 0x04003BF8 RID: 15352
		private float m_time;

		// Token: 0x04003BF9 RID: 15353
		private Vector3 m_prevPOS;

		// Token: 0x04003BFA RID: 15354
		private Vector3 m_initial_Rotation;

		// Token: 0x04003BFB RID: 15355
		private Vector3 m_initial_Position;

		// Token: 0x04003BFC RID: 15356
		private Color32 m_lightColor;

		// Token: 0x04003BFD RID: 15357
		private int frames;

		// Token: 0x04003BFE RID: 15358
		public ObjectSpin.MotionType Motion;

		// Token: 0x02000E77 RID: 3703
		public enum MotionType
		{
			// Token: 0x040057FE RID: 22526
			Rotation,
			// Token: 0x040057FF RID: 22527
			BackAndForth,
			// Token: 0x04005800 RID: 22528
			Translation
		}
	}
}
