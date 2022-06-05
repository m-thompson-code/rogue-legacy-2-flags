using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008E5 RID: 2277
	public class CastleBossEntranceTorchAudioController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700184A RID: 6218
		// (get) Token: 0x06004ACC RID: 19148 RVA: 0x0010CCA6 File Offset: 0x0010AEA6
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

		// Token: 0x06004ACD RID: 19149 RVA: 0x0010CCCC File Offset: 0x0010AECC
		private void Awake()
		{
			base.GetComponent<TorchesRoomPropController>().FlameStateChangeRelay.AddListener(new Action<bool>(this.OnStateChange), false);
		}

		// Token: 0x06004ACE RID: 19150 RVA: 0x0010CCEC File Offset: 0x0010AEEC
		private void OnDisable()
		{
			this.m_flameOnAudioEventEmitter.Stop();
		}

		// Token: 0x06004ACF RID: 19151 RVA: 0x0010CCF9 File Offset: 0x0010AEF9
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

		// Token: 0x04003EBD RID: 16061
		[SerializeField]
		private StudioEventEmitter m_flameOnAudioEventEmitter;

		// Token: 0x04003EBE RID: 16062
		[SerializeField]
		private StudioEventEmitter m_flameOffAudioEventEmitter;

		// Token: 0x04003EBF RID: 16063
		private string m_description = string.Empty;

		// Token: 0x04003EC0 RID: 16064
		private EventInstance m_torchAudioEventInstance;
	}
}
