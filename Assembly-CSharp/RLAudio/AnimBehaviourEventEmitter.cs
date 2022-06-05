using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E47 RID: 3655
	public abstract class AnimBehaviourEventEmitter : StateMachineBehaviour, IAudioEventEmitter
	{
		// Token: 0x17002113 RID: 8467
		// (get) Token: 0x06006712 RID: 26386 RVA: 0x00038BFA File Offset: 0x00036DFA
		// (set) Token: 0x06006713 RID: 26387 RVA: 0x00038C02 File Offset: 0x00036E02
		public string Description { get; protected set; }

		// Token: 0x06006714 RID: 26388 RVA: 0x0017C614 File Offset: 0x0017A814
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

		// Token: 0x06006715 RID: 26389 RVA: 0x0017C6CC File Offset: 0x0017A8CC
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

		// Token: 0x06006716 RID: 26390 RVA: 0x00038C0B File Offset: 0x00036E0B
		private void OnDisable()
		{
			this.Stop();
		}

		// Token: 0x06006717 RID: 26391 RVA: 0x00038C13 File Offset: 0x00036E13
		private void OnDestroy()
		{
			if (this.m_eventInstance.isValid())
			{
				this.m_eventInstance.release();
			}
		}

		// Token: 0x06006718 RID: 26392 RVA: 0x0017C7B0 File Offset: 0x0017A9B0
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

		// Token: 0x06006719 RID: 26393 RVA: 0x0017C7F4 File Offset: 0x0017A9F4
		private void OnStandingOnParameterChange(object sender, float newValue)
		{
			if (this.m_eventInstance.isValid())
			{
				this.m_eventInstance.setParameterByID(this.m_surfaceParamaterID, newValue, false);
				float num;
				this.m_eventInstance.getParameterByID(this.m_surfaceParamaterID, out num);
			}
		}

		// Token: 0x0600671A RID: 26394 RVA: 0x00038C2E File Offset: 0x00036E2E
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

		// Token: 0x0600671B RID: 26395 RVA: 0x00038C60 File Offset: 0x00036E60
		protected void Stop()
		{
			if (this.m_eventInstance.isValid())
			{
				AudioManager.Stop(this.m_eventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
		}

		// Token: 0x0400539C RID: 21404
		public static bool DisableEmitter_STATIC;

		// Token: 0x0400539D RID: 21405
		[EventRef]
		public string Event;

		// Token: 0x0400539E RID: 21406
		public ParamRef[] Params = new ParamRef[0];

		// Token: 0x0400539F RID: 21407
		[SerializeField]
		protected bool m_useSurfaceParameter;

		// Token: 0x040053A0 RID: 21408
		[SerializeField]
		protected AnimBehaviourCondition[] m_conditions;

		// Token: 0x040053A1 RID: 21409
		private const string SURFACE_PARAMETER_NAME = "Surface";

		// Token: 0x040053A2 RID: 21410
		protected PARAMETER_ID m_surfaceParamaterID;

		// Token: 0x040053A3 RID: 21411
		protected EventInstance m_eventInstance;

		// Token: 0x040053A4 RID: 21412
		protected EventDescription m_eventDescription;

		// Token: 0x040053A5 RID: 21413
		private bool m_hasSearchedForSurfaceController;

		// Token: 0x040053A6 RID: 21414
		private SurfaceAudioController m_surfaceController;

		// Token: 0x040053A7 RID: 21415
		private bool m_isInitialized;
	}
}
