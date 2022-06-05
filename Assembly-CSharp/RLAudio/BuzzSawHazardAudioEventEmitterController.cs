using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E53 RID: 3667
	public class BuzzSawHazardAudioEventEmitterController : MonoBehaviour
	{
		// Token: 0x0600677F RID: 26495 RVA: 0x000390FC File Offset: 0x000372FC
		private void Start()
		{
			this.m_saw = base.GetComponent<Buzzsaw_Hazard>();
		}

		// Token: 0x06006780 RID: 26496 RVA: 0x0003910A File Offset: 0x0003730A
		private void FixedUpdate()
		{
			this.m_sawAudioEmitter.SetParameter("sawPosition", this.m_saw.SawPosition, false);
		}

		// Token: 0x040053D7 RID: 21463
		[SerializeField]
		private StudioEventEmitter m_sawAudioEmitter;

		// Token: 0x040053D8 RID: 21464
		private Buzzsaw_Hazard m_saw;

		// Token: 0x040053D9 RID: 21465
		private const string PARAMETER_NAME = "sawPosition";
	}
}
