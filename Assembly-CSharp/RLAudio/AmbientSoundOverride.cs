using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008D8 RID: 2264
	public class AmbientSoundOverride : MonoBehaviour
	{
		// Token: 0x17001830 RID: 6192
		// (get) Token: 0x06004A5D RID: 19037 RVA: 0x0010BC20 File Offset: 0x00109E20
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

		// Token: 0x17001831 RID: 6193
		// (get) Token: 0x06004A5E RID: 19038 RVA: 0x0010BCC0 File Offset: 0x00109EC0
		public bool HasAudioOverride
		{
			get
			{
				return this.m_audioPaths.Length != 0;
			}
		}

		// Token: 0x17001832 RID: 6194
		// (get) Token: 0x06004A5F RID: 19039 RVA: 0x0010BCCC File Offset: 0x00109ECC
		public bool HasSnapshotOverride
		{
			get
			{
				return !string.IsNullOrEmpty(this.m_snapshotPath);
			}
		}

		// Token: 0x17001833 RID: 6195
		// (get) Token: 0x06004A60 RID: 19040 RVA: 0x0010BCDC File Offset: 0x00109EDC
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

		// Token: 0x04003E83 RID: 16003
		[SerializeField]
		[EventRef]
		private string[] m_audioPaths;

		// Token: 0x04003E84 RID: 16004
		[SerializeField]
		[EventRef]
		private string m_snapshotPath;

		// Token: 0x04003E85 RID: 16005
		private EventInstance[] m_audioInstances;

		// Token: 0x04003E86 RID: 16006
		private EventInstance m_snapshotInstance;
	}
}
