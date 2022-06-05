using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000919 RID: 2329
	public class Surface : MonoBehaviour
	{
		// Token: 0x17001889 RID: 6281
		// (get) Token: 0x06004C51 RID: 19537 RVA: 0x00112355 File Offset: 0x00110555
		public SurfaceType SurfaceType
		{
			get
			{
				return this.m_surfaceType;
			}
		}

		// Token: 0x04004057 RID: 16471
		[SerializeField]
		private SurfaceType m_surfaceType;
	}
}
