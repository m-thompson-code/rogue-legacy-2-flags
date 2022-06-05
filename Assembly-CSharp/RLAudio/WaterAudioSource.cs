using System;
using System.Collections;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E9D RID: 3741
	public class WaterAudioSource : MonoBehaviour
	{
		// Token: 0x1700218A RID: 8586
		// (get) Token: 0x0600696C RID: 26988 RVA: 0x0003A7DB File Offset: 0x000389DB
		public bool IsChoppy
		{
			get
			{
				return this.m_isChoppy;
			}
		}

		// Token: 0x1700218B RID: 8587
		// (get) Token: 0x0600696D RID: 26989 RVA: 0x0003A7E3 File Offset: 0x000389E3
		// (set) Token: 0x0600696E RID: 26990 RVA: 0x0003A7EB File Offset: 0x000389EB
		public float WaterLevel { get; private set; }

		// Token: 0x0600696F RID: 26991 RVA: 0x0003A7F4 File Offset: 0x000389F4
		private void OnEnable()
		{
			base.StartCoroutine(this.AddWaterSourceCoroutine(base.GetComponent<Prop>()));
		}

		// Token: 0x06006970 RID: 26992 RVA: 0x0003A809 File Offset: 0x00038A09
		private IEnumerator AddWaterSourceCoroutine(Prop prop)
		{
			if (!prop)
			{
				yield break;
			}
			while (!prop.IsInitialized)
			{
				yield return null;
			}
			this.WaterLevel = base.transform.position.y;
			WaterAudioSourceManager.AddWaterSource(this);
			yield break;
		}

		// Token: 0x06006971 RID: 26993 RVA: 0x0003A81F File Offset: 0x00038A1F
		private void OnDisable()
		{
			WaterAudioSourceManager.RemoveWaterSource(this);
		}

		// Token: 0x040055CF RID: 21967
		[SerializeField]
		private bool m_isChoppy;
	}
}
