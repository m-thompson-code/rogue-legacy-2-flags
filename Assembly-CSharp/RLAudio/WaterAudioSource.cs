using System;
using System.Collections;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x0200091F RID: 2335
	public class WaterAudioSource : MonoBehaviour
	{
		// Token: 0x1700188F RID: 6287
		// (get) Token: 0x06004C6B RID: 19563 RVA: 0x001128D6 File Offset: 0x00110AD6
		public bool IsChoppy
		{
			get
			{
				return this.m_isChoppy;
			}
		}

		// Token: 0x17001890 RID: 6288
		// (get) Token: 0x06004C6C RID: 19564 RVA: 0x001128DE File Offset: 0x00110ADE
		// (set) Token: 0x06004C6D RID: 19565 RVA: 0x001128E6 File Offset: 0x00110AE6
		public float WaterLevel { get; private set; }

		// Token: 0x06004C6E RID: 19566 RVA: 0x001128EF File Offset: 0x00110AEF
		private void OnEnable()
		{
			base.StartCoroutine(this.AddWaterSourceCoroutine(base.GetComponent<Prop>()));
		}

		// Token: 0x06004C6F RID: 19567 RVA: 0x00112904 File Offset: 0x00110B04
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

		// Token: 0x06004C70 RID: 19568 RVA: 0x0011291A File Offset: 0x00110B1A
		private void OnDisable()
		{
			WaterAudioSourceManager.RemoveWaterSource(this);
		}

		// Token: 0x04004067 RID: 16487
		[SerializeField]
		private bool m_isChoppy;
	}
}
