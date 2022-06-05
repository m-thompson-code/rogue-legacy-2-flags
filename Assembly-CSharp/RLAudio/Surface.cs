using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E97 RID: 3735
	public class Surface : MonoBehaviour
	{
		// Token: 0x17002184 RID: 8580
		// (get) Token: 0x06006952 RID: 26962 RVA: 0x0003A6E9 File Offset: 0x000388E9
		public SurfaceType SurfaceType
		{
			get
			{
				return this.m_surfaceType;
			}
		}

		// Token: 0x040055BF RID: 21951
		[SerializeField]
		private SurfaceType m_surfaceType;
	}
}
