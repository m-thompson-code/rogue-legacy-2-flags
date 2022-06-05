using System;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.Serialization;

namespace RLAudio
{
	// Token: 0x0200090D RID: 2317
	public class RLFMODEventEmitter : BaseFMODEventEmitter, IAudioEventEmitter
	{
		// Token: 0x1700187D RID: 6269
		// (get) Token: 0x06004C09 RID: 19465 RVA: 0x001112B3 File Offset: 0x0010F4B3
		public bool AttachToGameObject
		{
			get
			{
				return base.Is3D && this.m_attachToGameObject;
			}
		}

		// Token: 0x1700187E RID: 6270
		// (get) Token: 0x06004C0A RID: 19466 RVA: 0x001112C5 File Offset: 0x0010F4C5
		public string EventPath
		{
			get
			{
				return this.m_eventPath;
			}
		}

		// Token: 0x1700187F RID: 6271
		// (get) Token: 0x06004C0B RID: 19467 RVA: 0x001112CD File Offset: 0x0010F4CD
		public bool IsOneShot
		{
			get
			{
				return this.m_isOneShot;
			}
		}

		// Token: 0x06004C0C RID: 19468 RVA: 0x001112D8 File Offset: 0x0010F4D8
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

		// Token: 0x06004C0D RID: 19469 RVA: 0x00111364 File Offset: 0x0010F564
		public override void Play()
		{
			if (base.Is3D)
			{
				this.Play3DAudio();
				return;
			}
			this.Play2DAudio();
		}

		// Token: 0x06004C0E RID: 19470 RVA: 0x0011137C File Offset: 0x0010F57C
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

		// Token: 0x06004C0F RID: 19471 RVA: 0x00111420 File Offset: 0x0010F620
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

		// Token: 0x06004C10 RID: 19472 RVA: 0x00111484 File Offset: 0x0010F684
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

		// Token: 0x06004C11 RID: 19473 RVA: 0x001114DE File Offset: 0x0010F6DE
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

		// Token: 0x0400400A RID: 16394
		[SerializeField]
		[FormerlySerializedAs("m_stopImmediatelyOnGameObjectDestroyed")]
		private bool m_isOneShot = true;

		// Token: 0x0400400B RID: 16395
		[SerializeField]
		private bool m_attachToGameObject = true;
	}
}
