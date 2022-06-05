using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E50 RID: 3664
	public abstract class BaseFMODEventEmitter : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17002128 RID: 8488
		// (get) Token: 0x06006768 RID: 26472 RVA: 0x00009A7B File Offset: 0x00007C7B
		public string Description
		{
			get
			{
				return this.ToString();
			}
		}

		// Token: 0x17002129 RID: 8489
		// (get) Token: 0x06006769 RID: 26473 RVA: 0x00038FCE File Offset: 0x000371CE
		// (set) Token: 0x0600676A RID: 26474 RVA: 0x00038FD6 File Offset: 0x000371D6
		public bool Is3D { get; private set; }

		// Token: 0x1700212A RID: 8490
		// (get) Token: 0x0600676B RID: 26475 RVA: 0x00038FDF File Offset: 0x000371DF
		// (set) Token: 0x0600676C RID: 26476 RVA: 0x00038FE7 File Offset: 0x000371E7
		public bool IsInitialized { get; protected set; }

		// Token: 0x0600676D RID: 26477 RVA: 0x00038FF0 File Offset: 0x000371F0
		private void Awake()
		{
			if (!this.IsInitialized)
			{
				this.Initialize();
			}
		}

		// Token: 0x0600676E RID: 26478 RVA: 0x00039000 File Offset: 0x00037200
		private void OnEnable()
		{
			if (!this.IsInitialized)
			{
				this.Initialize();
			}
			if (this.m_playOnEnable)
			{
				this.Play();
			}
		}

		// Token: 0x0600676F RID: 26479 RVA: 0x0003901E File Offset: 0x0003721E
		private void OnDisable()
		{
			if (this.m_stopOnDisable)
			{
				this.Stop();
			}
		}

		// Token: 0x06006770 RID: 26480 RVA: 0x0003902E File Offset: 0x0003722E
		private void OnDestroy()
		{
			if (this.m_eventInstance.isValid())
			{
				this.m_eventInstance.release();
			}
		}

		// Token: 0x06006771 RID: 26481 RVA: 0x0017CF3C File Offset: 0x0017B13C
		protected virtual void Initialize()
		{
			if (!string.IsNullOrEmpty(this.m_eventPath))
			{
				try
				{
					bool is3D;
					RuntimeManager.GetEventDescription(this.m_eventPath).is3D(out is3D);
					this.Is3D = is3D;
				}
				catch (Exception)
				{
					Debug.LogFormat("<color=red>| {0} | The given FMOD Event path ({1}) is invalid.</color>", new object[]
					{
						this,
						this.m_eventPath
					});
				}
			}
			this.IsInitialized = true;
		}

		// Token: 0x06006772 RID: 26482
		public abstract void Play();

		// Token: 0x06006773 RID: 26483 RVA: 0x00039049 File Offset: 0x00037249
		public void Stop()
		{
			if (this.m_eventInstance.isValid())
			{
				AudioManager.Stop(this.m_eventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
		}

		// Token: 0x040053CB RID: 21451
		[SerializeField]
		[EventRef]
		protected string m_eventPath;

		// Token: 0x040053CC RID: 21452
		[SerializeField]
		protected bool m_playOnEnable = true;

		// Token: 0x040053CD RID: 21453
		[SerializeField]
		protected bool m_stopOnDisable = true;

		// Token: 0x040053CE RID: 21454
		protected EventInstance m_eventInstance;
	}
}
