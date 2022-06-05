using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008E3 RID: 2275
	public class BuzzSawHazardAudioEventEmitterController : MonoBehaviour
	{
		// Token: 0x06004AC6 RID: 19142 RVA: 0x0010CC1B File Offset: 0x0010AE1B
		private void Start()
		{
			this.m_saw = base.GetComponent<Buzzsaw_Hazard>();
		}

		// Token: 0x06004AC7 RID: 19143 RVA: 0x0010CC29 File Offset: 0x0010AE29
		private void FixedUpdate()
		{
			this.m_sawAudioEmitter.SetParameter("sawPosition", this.m_saw.SawPosition, false);
		}

		// Token: 0x04003EB9 RID: 16057
		[SerializeField]
		private StudioEventEmitter m_sawAudioEmitter;

		// Token: 0x04003EBA RID: 16058
		private Buzzsaw_Hazard m_saw;

		// Token: 0x04003EBB RID: 16059
		private const string PARAMETER_NAME = "sawPosition";
	}
}
