using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000D61 RID: 3425
	public class ObjectSpin : MonoBehaviour
	{
		// Token: 0x0600619E RID: 24990 RVA: 0x0016B188 File Offset: 0x00169388
		private void Awake()
		{
			this.m_transform = base.transform;
			this.m_initial_Rotation = this.m_transform.rotation.eulerAngles;
			this.m_initial_Position = this.m_transform.position;
			Light component = base.GetComponent<Light>();
			this.m_lightColor = ((component != null) ? component.color : Color.black);
		}

		// Token: 0x0600619F RID: 24991 RVA: 0x0016B1F4 File Offset: 0x001693F4
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

		// Token: 0x04004F7E RID: 20350
		public float SpinSpeed = 5f;

		// Token: 0x04004F7F RID: 20351
		public int RotationRange = 15;

		// Token: 0x04004F80 RID: 20352
		private Transform m_transform;

		// Token: 0x04004F81 RID: 20353
		private float m_time;

		// Token: 0x04004F82 RID: 20354
		private Vector3 m_prevPOS;

		// Token: 0x04004F83 RID: 20355
		private Vector3 m_initial_Rotation;

		// Token: 0x04004F84 RID: 20356
		private Vector3 m_initial_Position;

		// Token: 0x04004F85 RID: 20357
		private Color32 m_lightColor;

		// Token: 0x04004F86 RID: 20358
		private int frames;

		// Token: 0x04004F87 RID: 20359
		public ObjectSpin.MotionType Motion;

		// Token: 0x02000D62 RID: 3426
		public enum MotionType
		{
			// Token: 0x04004F89 RID: 20361
			Rotation,
			// Token: 0x04004F8A RID: 20362
			BackAndForth,
			// Token: 0x04004F8B RID: 20363
			Translation
		}
	}
}
