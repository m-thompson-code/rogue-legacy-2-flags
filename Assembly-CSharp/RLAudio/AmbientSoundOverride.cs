using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E46 RID: 3654
	public class AmbientSoundOverride : MonoBehaviour
	{
		// Token: 0x1700210F RID: 8463
		// (get) Token: 0x0600670D RID: 26381 RVA: 0x0017C574 File Offset: 0x0017A774
		public EventInstance[] Audio
		{
			get
			{
				if (this.m_audioInstances == null)
				{
					this.m_audioInstances = new EventInstance[this.m_audioPaths.Length];
					for (int i = 0; i < this.m_audioInstances.Length; i++)
					{
						try
						{
							this.m_audioInstances[i] = RuntimeManager.CreateInstance(this.m_audioPaths[i]);
						}
						catch (EventNotFoundException)
						{
							Debug.LogFormat("<color=red>| {0} | The path specified at index <b>{1}</b> in the Audio Paths collection is invalid</color>", new object[]
							{
								this,
								i
							});
							this.m_audioInstances[i] = default(EventInstance);
						}
					}
				}
				return this.m_audioInstances;
			}
		}

		// Token: 0x17002110 RID: 8464
		// (get) Token: 0x0600670E RID: 26382 RVA: 0x00038BAA File Offset: 0x00036DAA
		public bool HasAudioOverride
		{
			get
			{
				return this.m_audioPaths.Length != 0;
			}
		}

		// Token: 0x17002111 RID: 8465
		// (get) Token: 0x0600670F RID: 26383 RVA: 0x00038BB6 File Offset: 0x00036DB6
		public bool HasSnapshotOverride
		{
			get
			{
				return !string.IsNullOrEmpty(this.m_snapshotPath);
			}
		}

		// Token: 0x17002112 RID: 8466
		// (get) Token: 0x06006710 RID: 26384 RVA: 0x00038BC6 File Offset: 0x00036DC6
		public EventInstance Snapshot
		{
			get
			{
				if (!string.IsNullOrEmpty(this.m_snapshotPath) && this.m_snapshotPath.Equals(null))
				{
					this.m_snapshotInstance = RuntimeManager.CreateInstance(this.m_snapshotPath);
				}
				return this.m_snapshotInstance;
			}
		}

		// Token: 0x04005398 RID: 21400
		[SerializeField]
		[EventRef]
		private string[] m_audioPaths;

		// Token: 0x04005399 RID: 21401
		[SerializeField]
		[EventRef]
		private string m_snapshotPath;

		// Token: 0x0400539A RID: 21402
		private EventInstance[] m_audioInstances;

		// Token: 0x0400539B RID: 21403
		private EventInstance m_snapshotInstance;
	}
}
