using System;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.Serialization;

namespace RLAudio
{
	// Token: 0x02000E8A RID: 3722
	public class RLFMODEventEmitter : BaseFMODEventEmitter, IAudioEventEmitter
	{
		// Token: 0x17002176 RID: 8566
		// (get) Token: 0x06006904 RID: 26884 RVA: 0x0003A2D0 File Offset: 0x000384D0
		public bool AttachToGameObject
		{
			get
			{
				return base.Is3D && this.m_attachToGameObject;
			}
		}

		// Token: 0x17002177 RID: 8567
		// (get) Token: 0x06006905 RID: 26885 RVA: 0x0003A2E2 File Offset: 0x000384E2
		public string EventPath
		{
			get
			{
				return this.m_eventPath;
			}
		}

		// Token: 0x17002178 RID: 8568
		// (get) Token: 0x06006906 RID: 26886 RVA: 0x0003A2EA File Offset: 0x000384EA
		public bool IsOneShot
		{
			get
			{
				return this.m_isOneShot;
			}
		}

		// Token: 0x06006907 RID: 26887 RVA: 0x0018134C File Offset: 0x0017F54C
		protected override void Initialize()
		{
			base.Initialize();
			base.IsInitialized = false;
			if (!this.IsOneShot && !string.IsNullOrEmpty(this.EventPath))
			{
				try
				{
					this.m_eventInstance = AudioUtility.GetEventInstance(this.EventPath, base.transform);
				}
				catch (Exception)
				{
					this.m_eventInstance = default(EventInstance);
					Debug.LogFormat("<color=red>| {0} | The given path (<b>{1}</b>) is not a valid FMOD Event Path</color>", new object[]
					{
						this,
						this.EventPath
					});
				}
			}
			base.IsInitialized = true;
		}

		// Token: 0x06006908 RID: 26888 RVA: 0x0003A2F2 File Offset: 0x000384F2
		public override void Play()
		{
			if (base.Is3D)
			{
				this.Play3DAudio();
				return;
			}
			this.Play2DAudio();
		}

		// Token: 0x06006909 RID: 26889 RVA: 0x001813D8 File Offset: 0x0017F5D8
		private void Play3DAudio()
		{
			if (base.Is3D)
			{
				if (this.IsOneShot)
				{
					if (this.AttachToGameObject)
					{
						AudioManager.PlayOneShotAttached(this, this.EventPath, base.gameObject);
						return;
					}
					AudioManager.PlayOneShot(this, this.EventPath, base.transform.position);
					return;
				}
				else if (this.m_eventInstance.isValid())
				{
					if (this.AttachToGameObject)
					{
						AudioManager.PlayAttached(this, this.m_eventInstance, base.gameObject);
						return;
					}
					AudioManager.Play(this, this.m_eventInstance, base.transform.position);
					return;
				}
			}
			else
			{
				Debug.LogFormat("<color=red>| {0} | The FMOD Event is <b>NOT</b> 3D, but you're calling a method that requires that it is. If you see this message, please add a bug report to Pivotal.</color>", new object[]
				{
					this
				});
			}
		}

		// Token: 0x0600690A RID: 26890 RVA: 0x0018147C File Offset: 0x0017F67C
		private void Play2DAudio()
		{
			if (!base.Is3D)
			{
				if (this.IsOneShot)
				{
					AudioManager.PlayOneShot(this, this.EventPath, default(Vector3));
					return;
				}
				if (this.m_eventInstance.isValid())
				{
					AudioManager.Play(this, this.m_eventInstance);
					return;
				}
			}
			else
			{
				Debug.LogFormat("<color=red>| { 0} | The FMOD Event is <b>NOT</b> 2D, but you're calling a method that requires that it is. If you see this message, please add a bug report to Pivotal.</color>", new object[]
				{
					this
				});
			}
		}

		// Token: 0x0600690B RID: 26891 RVA: 0x001814E0 File Offset: 0x0017F6E0
		public void Play(Vector3 worldPosition)
		{
			if (base.Is3D)
			{
				if (this.IsOneShot)
				{
					AudioManager.PlayOneShot(this, this.EventPath, worldPosition);
					return;
				}
				if (this.m_eventInstance.isValid())
				{
					AudioManager.Play(this, this.m_eventInstance, worldPosition);
					return;
				}
			}
			else
			{
				Debug.LogFormat("<color=red>| {0} | The FMOD Event is not 3D, but you're calling a method that requires that it is. If you see this message, please add a bug report to Pivotal.</color>", new object[]
				{
					this
				});
			}
		}

		// Token: 0x0600690C RID: 26892 RVA: 0x0003A309 File Offset: 0x00038509
		public void SetParameter(string parameterName, float value)
		{
			if (!this.IsOneShot)
			{
				if (this.m_eventInstance.isValid())
				{
					this.m_eventInstance.setParameterByName(parameterName, value, false);
					return;
				}
			}
			else
			{
				Debug.LogFormat("<color=red>| {0} | You called SetParameter on an Emitter marked as 'One Shot'. This is not allowed. If you see this message, please add a bug report to Pivotal.</color>", new object[]
				{
					this
				});
			}
		}

		// Token: 0x0400556A RID: 21866
		[SerializeField]
		[FormerlySerializedAs("m_stopImmediatelyOnGameObjectDestroyed")]
		private bool m_isOneShot = true;

		// Token: 0x0400556B RID: 21867
		[SerializeField]
		private bool m_attachToGameObject = true;
	}
}
