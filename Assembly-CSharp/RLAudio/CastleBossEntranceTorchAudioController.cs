using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E55 RID: 3669
	public class CastleBossEntranceTorchAudioController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700212D RID: 8493
		// (get) Token: 0x06006785 RID: 26501 RVA: 0x0003915B File Offset: 0x0003735B
		public string Description
		{
			get
			{
				if (this.m_description == string.Empty)
				{
					this.m_description = this.ToString();
				}
				return this.m_description;
			}
		}

		// Token: 0x06006786 RID: 26502 RVA: 0x00039181 File Offset: 0x00037381
		private void Awake()
		{
			base.GetComponent<TorchesRoomPropController>().FlameStateChangeRelay.AddListener(new Action<bool>(this.OnStateChange), false);
		}

		// Token: 0x06006787 RID: 26503 RVA: 0x000391A1 File Offset: 0x000373A1
		private void OnDisable()
		{
			this.m_flameOnAudioEventEmitter.Stop();
		}

		// Token: 0x06006788 RID: 26504 RVA: 0x000391AE File Offset: 0x000373AE
		private void OnStateChange(bool isFlameOn)
		{
			if (isFlameOn)
			{
				this.m_flameOnAudioEventEmitter.Play();
				return;
			}
			this.m_flameOnAudioEventEmitter.Stop();
			this.m_flameOffAudioEventEmitter.Play();
		}

		// Token: 0x040053DB RID: 21467
		[SerializeField]
		private StudioEventEmitter m_flameOnAudioEventEmitter;

		// Token: 0x040053DC RID: 21468
		[SerializeField]
		private StudioEventEmitter m_flameOffAudioEventEmitter;

		// Token: 0x040053DD RID: 21469
		private string m_description = string.Empty;

		// Token: 0x040053DE RID: 21470
		private EventInstance m_torchAudioEventInstance;
	}
}
