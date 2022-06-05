using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008E0 RID: 2272
	public abstract class BaseFMODEventEmitter : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001845 RID: 6213
		// (get) Token: 0x06004AAF RID: 19119 RVA: 0x0010C941 File Offset: 0x0010AB41
		public string Description
		{
			get
			{
				return this.ToString();
			}
		}

		// Token: 0x17001846 RID: 6214
		// (get) Token: 0x06004AB0 RID: 19120 RVA: 0x0010C949 File Offset: 0x0010AB49
		// (set) Token: 0x06004AB1 RID: 19121 RVA: 0x0010C951 File Offset: 0x0010AB51
		public bool Is3D { get; private set; }

		// Token: 0x17001847 RID: 6215
		// (get) Token: 0x06004AB2 RID: 19122 RVA: 0x0010C95A File Offset: 0x0010AB5A
		// (set) Token: 0x06004AB3 RID: 19123 RVA: 0x0010C962 File Offset: 0x0010AB62
		public bool IsInitialized { get; protected set; }

		// Token: 0x06004AB4 RID: 19124 RVA: 0x0010C96B File Offset: 0x0010AB6B
		private void Awake()
		{
			if (!this.IsInitialized)
			{
				this.Initialize();
			}
		}

		// Token: 0x06004AB5 RID: 19125 RVA: 0x0010C97B File Offset: 0x0010AB7B
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

		// Token: 0x06004AB6 RID: 19126 RVA: 0x0010C999 File Offset: 0x0010AB99
		private void OnDisable()
		{
			if (this.m_stopOnDisable)
			{
				this.Stop();
			}
		}

		// Token: 0x06004AB7 RID: 19127 RVA: 0x0010C9A9 File Offset: 0x0010ABA9
		private void OnDestroy()
		{
			if (this.m_eventInstance.isValid())
			{
				this.m_eventInstance.release();
			}
		}

		// Token: 0x06004AB8 RID: 19128 RVA: 0x0010C9C4 File Offset: 0x0010ABC4
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

		// Token: 0x06004AB9 RID: 19129
		public abstract void Play();

		// Token: 0x06004ABA RID: 19130 RVA: 0x0010CA34 File Offset: 0x0010AC34
		public void Stop()
		{
			if (this.m_eventInstance.isValid())
			{
				AudioManager.Stop(this.m_eventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
		}

		// Token: 0x04003EAD RID: 16045
		[SerializeField]
		[EventRef]
		protected string m_eventPath;

		// Token: 0x04003EAE RID: 16046
		[SerializeField]
		protected bool m_playOnEnable = true;

		// Token: 0x04003EAF RID: 16047
		[SerializeField]
		protected bool m_stopOnDisable = true;

		// Token: 0x04003EB0 RID: 16048
		protected EventInstance m_eventInstance;
	}
}
