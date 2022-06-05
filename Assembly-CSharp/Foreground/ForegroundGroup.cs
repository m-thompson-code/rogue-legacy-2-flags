using System;
using UnityEngine;

namespace Foreground
{
	// Token: 0x02000DC4 RID: 3524
	public class ForegroundGroup : MonoBehaviour
	{
		// Token: 0x17002007 RID: 8199
		// (get) Token: 0x06006345 RID: 25413 RVA: 0x00036AE4 File Offset: 0x00034CE4
		public PropSpawnController[] PropSpawnControllers
		{
			get
			{
				return this.m_propSpawnControllers;
			}
		}

		// Token: 0x17002008 RID: 8200
		// (get) Token: 0x06006346 RID: 25414 RVA: 0x00036AEC File Offset: 0x00034CEC
		// (set) Token: 0x06006347 RID: 25415 RVA: 0x00036AF4 File Offset: 0x00034CF4
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

		// Token: 0x06006348 RID: 25416 RVA: 0x00036B04 File Offset: 0x00034D04
		private void Awake()
		{
			this.m_propSpawnControllers = base.GetComponentsInChildren<PropSpawnController>();
		}

		// Token: 0x04005113 RID: 20755
		[SerializeField]
		[Range(1f, 2f)]
		private float m_zoomLevel = 1f;

		// Token: 0x04005114 RID: 20756
		private PropSpawnController[] m_propSpawnControllers;
	}
}
