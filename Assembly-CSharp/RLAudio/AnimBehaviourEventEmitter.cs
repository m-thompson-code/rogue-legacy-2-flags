using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008D9 RID: 2265
	public abstract class AnimBehaviourEventEmitter : StateMachineBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001834 RID: 6196
		// (get) Token: 0x06004A62 RID: 19042 RVA: 0x0010BD18 File Offset: 0x00109F18
		// (set) Token: 0x06004A63 RID: 19043 RVA: 0x0010BD20 File Offset: 0x00109F20
		public string Description { get; protected set; }

		// Token: 0x06004A64 RID: 19044 RVA: 0x0010BD2C File Offset: 0x00109F2C
		protected virtual void InitializeEventInstance(Animator animator)
		{
			try
			{
				this.m_eventInstance = RuntimeManager.CreateInstance(this.Event);
				this.m_eventInstance.getDescription(out this.m_eventDescription);
				if (this.m_useSurfaceParameter)
				{
					this.m_surfaceParamaterID = AnimBehaviourEventEmitterUtility.GetParameterID(this.m_eventDescription, "Surface");
				}
			}
			catch (EventNotFoundException)
			{
				if (this.Event == string.Empty)
				{
					Debug.LogFormat("<color=red>| {0} | No FMOD Event path has been specified on a State in Animator ({1}). If you see this message, please add a bug report to Pivotal.</color>", new object[]
					{
						this,
						animator.name
					});
				}
				else
				{
					Debug.LogFormat("<color=red>| {0} | No FMOD Event named ({1}). If you If you see this message, please add a bug report to Pivotal.</color>", new object[]
					{
						this,
						this.Event
					});
				}
			}
			this.m_isInitialized = true;
		}

		// Token: 0x06004A65 RID: 19045 RVA: 0x0010BDE4 File Offset: 0x00109FE4
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (AnimBehaviourEventEmitter.DisableEmitter_STATIC)
			{
				return;
			}
			if (!this.GetShouldPlay(animator))
			{
				return;
			}
			if (!this.m_isInitialized)
			{
				this.InitializeEventInstance(animator);
			}
			if (this.m_useSurfaceParameter && !this.m_hasSearchedForSurfaceController && !this.m_surfaceController)
			{
				this.m_hasSearchedForSurfaceController = true;
				this.m_surfaceController = animator.GetComponent<SurfaceAudioController>();
				if (!this.m_surfaceController)
				{
					Debug.LogFormat("<color=red>| {0} | No SurfaceController found on ({1}). If you see this message, please add a bug report to Pivotal.</color>", new object[]
					{
						this,
						animator.name
					});
				}
				else
				{
					SurfaceAudioController surfaceController = this.m_surfaceController;
					surfaceController.StandingOnParameterChangeEvent = (EventHandler<float>)Delegate.Combine(surfaceController.StandingOnParameterChangeEvent, new EventHandler<float>(this.OnStandingOnParameterChange));
				}
			}
			if (string.IsNullOrEmpty(this.Description))
			{
				this.Description = string.Format("({0} : Animation State ({1}) : {2})", animator.name, stateInfo.fullPathHash, this);
			}
		}

		// Token: 0x06004A66 RID: 19046 RVA: 0x0010BEC5 File Offset: 0x0010A0C5
		private void OnDisable()
		{
			this.Stop();
		}

		// Token: 0x06004A67 RID: 19047 RVA: 0x0010BECD File Offset: 0x0010A0CD
		private void OnDestroy()
		{
			if (this.m_eventInstance.isValid())
			{
				this.m_eventInstance.release();
			}
		}

		// Token: 0x06004A68 RID: 19048 RVA: 0x0010BEE8 File Offset: 0x0010A0E8
		protected bool GetShouldPlay(Animator animator)
		{
			bool result = true;
			if (this.m_conditions.Length != 0)
			{
				for (int i = 0; i < this.m_conditions.Length; i++)
				{
					if (!this.m_conditions[i].IsTrue(animator))
					{
						result = false;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06004A69 RID: 19049 RVA: 0x0010BF2C File Offset: 0x0010A12C
		private void OnStandingOnParameterChange(object sender, float newValue)
		{
			if (this.m_eventInstance.isValid())
			{
				this.m_eventInstance.setParameterByID(this.m_surfaceParamaterID, newValue, false);
				float num;
				this.m_eventInstance.getParameterByID(this.m_surfaceParamaterID, out num);
			}
		}

		// Token: 0x06004A6A RID: 19050 RVA: 0x0010BF6E File Offset: 0x0010A16E
		protected void Play(Animator animator)
		{
			if (EffectTriggerAnimBehaviour.DISABLE_GLOBALLY)
			{
				return;
			}
			if (EffectManager.AnimatorEffectsDisabled(animator))
			{
				return;
			}
			if (this.m_eventInstance.isValid())
			{
				AudioManager.PlayAttached(this, this.m_eventInstance, animator.gameObject);
			}
		}

		// Token: 0x06004A6B RID: 19051 RVA: 0x0010BFA0 File Offset: 0x0010A1A0
		protected void Stop()
		{
			if (this.m_eventInstance.isValid())
			{
				AudioManager.Stop(this.m_eventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
		}

		// Token: 0x04003E87 RID: 16007
		public static bool DisableEmitter_STATIC;

		// Token: 0x04003E88 RID: 16008
		[EventRef]
		public string Event;

		// Token: 0x04003E89 RID: 16009
		public ParamRef[] Params = new ParamRef[0];

		// Token: 0x04003E8A RID: 16010
		[SerializeField]
		protected bool m_useSurfaceParameter;

		// Token: 0x04003E8B RID: 16011
		[SerializeField]
		protected AnimBehaviourCondition[] m_conditions;

		// Token: 0x04003E8C RID: 16012
		private const string SURFACE_PARAMETER_NAME = "Surface";

		// Token: 0x04003E8D RID: 16013
		protected PARAMETER_ID m_surfaceParamaterID;

		// Token: 0x04003E8E RID: 16014
		protected EventInstance m_eventInstance;

		// Token: 0x04003E8F RID: 16015
		protected EventDescription m_eventDescription;

		// Token: 0x04003E90 RID: 16016
		private bool m_hasSearchedForSurfaceController;

		// Token: 0x04003E91 RID: 16017
		private SurfaceAudioController m_surfaceController;

		// Token: 0x04003E92 RID: 16018
		private bool m_isInitialized;
	}
}
