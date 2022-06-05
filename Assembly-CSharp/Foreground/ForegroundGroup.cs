using System;
using UnityEngine;

namespace Foreground
{
	// Token: 0x02000897 RID: 2199
	public class ForegroundGroup : MonoBehaviour
	{
		// Token: 0x17001795 RID: 6037
		// (get) Token: 0x0600480A RID: 18442 RVA: 0x0010375A File Offset: 0x0010195A
		public PropSpawnController[] PropSpawnControllers
		{
			get
			{
				return this.m_propSpawnControllers;
			}
		}

		// Token: 0x17001796 RID: 6038
		// (get) Token: 0x0600480B RID: 18443 RVA: 0x00103762 File Offset: 0x00101962
		// (set) Token: 0x0600480C RID: 18444 RVA: 0x0010376A File Offset: 0x0010196A
		public float ZoomLevel
		{
			get
			{
				return this.m_zoomLevel;
			}
			set
			{
				if (!Application.isPlaying)
				{
					this.m_zoomLevel = value;
				}
			}
		}

		// Token: 0x0600480D RID: 18445 RVA: 0x0010377A File Offset: 0x0010197A
		private void Awake()
		{
			this.m_propSpawnControllers = base.GetComponentsInChildren<PropSpawnController>();
		}

		// Token: 0x04003CE5 RID: 15589
		[SerializeField]
		[Range(1f, 2f)]
		private float m_zoomLevel = 1f;

		// Token: 0x04003CE6 RID: 15590
		private PropSpawnController[] m_propSpawnControllers;
	}
}
