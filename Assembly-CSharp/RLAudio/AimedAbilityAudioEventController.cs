using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

namespace RLAudio
{
	// Token: 0x02000E3E RID: 3646
	public class AimedAbilityAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x170020F6 RID: 8438
		// (get) Token: 0x060066BD RID: 26301 RVA: 0x000388AE File Offset: 0x00036AAE
		public string Description
		{
			get
			{
				return "Aimed Ability Audio Event Emitter Controller";
			}
		}

		// Token: 0x060066BE RID: 26302 RVA: 0x0017B7CC File Offset: 0x001799CC
		protected virtual void Start()
		{
			this.m_aimedAbility = base.GetComponent<AimedAbility_RL>();
			if (this.m_aimedAbility)
			{
				this.m_aimedAbility.AimSpeedChangeRelay.AddListener(new Action<float>(this.OnAimSpeedChange), false);
				this.m_aimedAbility.BeginCastingRelay.AddListener(new Action(this.OnBeginAbility), false);
				this.m_aimedAbility.FireProjectileRelay.AddListener(new Action<Projectile_RL>(this.OnAbilityFired), false);
				this.m_aimedAbility.SwitchSidesRelay.AddListener(new Action(this.OnSwitchSides), false);
				this.m_aimedAbility.KickbackRelay.AddListener(new Action(this.OnKickback), false);
				this.m_beginAbilityEventInstance = AudioUtility.GetEventInstance(this.m_abilityBeginEventPath, base.transform);
				this.m_aimEventInstance = AudioUtility.GetEventInstance(this.m_aimEventPath, base.transform);
				this.m_switchSideEventInstance = AudioUtility.GetEventInstance(this.m_switchSidesEventPath, base.transform);
				this.m_abilityFiredEventInstance = AudioUtility.GetEventInstance(this.m_abilityFiredEventPath, base.transform);
				this.m_kickbackEventInstance = AudioUtility.GetEventInstance(this.m_kickbackEventPath, base.transform);
				this.m_aimSpeedParameterID = this.GetAimSpeedParameterID();
				return;
			}
			Debug.LogFormat("<color=red>| {0} | Missing <b>AimedAbility_RL</b> component. Please add a bug report to Pivotal and assign it to Paul</color>", new object[]
			{
				this
			});
		}

		// Token: 0x060066BF RID: 26303 RVA: 0x000388B5 File Offset: 0x00036AB5
		protected void OnSwitchSides()
		{
			if (this.m_switchSideEventInstance.isValid())
			{
				AudioManager.PlayAttached(this, this.m_switchSideEventInstance, base.gameObject);
			}
		}

		// Token: 0x060066C0 RID: 26304 RVA: 0x0017B920 File Offset: 0x00179B20
		protected PARAMETER_ID GetAimSpeedParameterID()
		{
			PARAMETER_ID result = default(PARAMETER_ID);
			if (!string.IsNullOrEmpty(this.m_aimEventPath))
			{
				result = AnimBehaviourEventEmitterUtility.GetParameterID(RuntimeManager.GetEventDescription(this.m_aimEventPath), "aim_speed");
			}
			else
			{
				Debug.LogFormat("<color=red>| {0} | No Bow Drawn Event Path has been set. Please add a bug report to Pivotal and assign it to Paul</color>", new object[]
				{
					this
				});
			}
			return result;
		}

		// Token: 0x060066C1 RID: 26305 RVA: 0x000388D6 File Offset: 0x00036AD6
		protected virtual void OnAbilityFired(Projectile_RL projectile)
		{
			this.m_isAbilityStarted = false;
			if (this.m_abilityFiredEventInstance.isValid())
			{
				AudioManager.PlayAttached(this, this.m_abilityFiredEventInstance, base.gameObject);
			}
			AudioManager.Stop(this.m_aimEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}

		// Token: 0x060066C2 RID: 26306 RVA: 0x0017B970 File Offset: 0x00179B70
		private void OnBeginAbility()
		{
			this.m_isAbilityStarted = true;
			if (this.m_beginAbilityEventInstance.isValid())
			{
				AudioManager.PlayAttached(this, this.m_beginAbilityEventInstance, base.gameObject);
			}
			if (this.m_aimEventInstance.isValid())
			{
				this.m_aimEventInstance.setParameterByID(this.m_aimSpeedParameterID, 0f, false);
				AudioManager.PlayAttached(this, this.m_aimEventInstance, base.gameObject);
			}
		}

		// Token: 0x060066C3 RID: 26307 RVA: 0x0017B9DC File Offset: 0x00179BDC
		protected void OnAimSpeedChange(float changeAsPercentOfMax)
		{
			PLAYBACK_STATE playback_STATE;
			this.m_aimEventInstance.getPlaybackState(out playback_STATE);
			if (this.m_isAbilityStarted && this.m_aimEventPath != null && !this.m_aimSpeedParameterID.Equals(default(PARAMETER_ID)))
			{
				if (playback_STATE == PLAYBACK_STATE.STOPPED || playback_STATE == PLAYBACK_STATE.STOPPING)
				{
					AudioManager.PlayAttached(this, this.m_aimEventInstance, base.gameObject);
				}
				this.m_aimEventInstance.setParameterByID(this.m_aimSpeedParameterID, changeAsPercentOfMax, false);
			}
		}

		// Token: 0x060066C4 RID: 26308 RVA: 0x0003890A File Offset: 0x00036B0A
		protected void OnKickback()
		{
			if (this.m_kickbackEventInstance.isValid())
			{
				AudioManager.PlayAttached(this, this.m_kickbackEventInstance, base.gameObject);
			}
		}

		// Token: 0x060066C5 RID: 26309 RVA: 0x0017BA58 File Offset: 0x00179C58
		private void OnDestroy()
		{
			if (this.m_beginAbilityEventInstance.isValid())
			{
				this.m_beginAbilityEventInstance.release();
			}
			if (this.m_aimEventInstance.isValid())
			{
				this.m_aimEventInstance.release();
			}
			if (this.m_switchSideEventInstance.isValid())
			{
				this.m_switchSideEventInstance.release();
			}
			if (this.m_abilityFiredEventInstance.isValid())
			{
				this.m_abilityFiredEventInstance.release();
			}
			if (this.m_kickbackEventInstance.isValid())
			{
				this.m_kickbackEventInstance.release();
			}
		}

		// Token: 0x04005356 RID: 21334
		[SerializeField]
		[EventRef]
		[FormerlySerializedAs("m_bowDrawEventPath")]
		protected string m_abilityBeginEventPath;

		// Token: 0x04005357 RID: 21335
		[SerializeField]
		[EventRef]
		[FormerlySerializedAs("m_bowAimEventPath")]
		protected string m_aimEventPath;

		// Token: 0x04005358 RID: 21336
		[SerializeField]
		[EventRef]
		protected string m_switchSidesEventPath;

		// Token: 0x04005359 RID: 21337
		[SerializeField]
		[EventRef]
		[FormerlySerializedAs("m_bowFiredEventPath")]
		protected string m_abilityFiredEventPath;

		// Token: 0x0400535A RID: 21338
		[SerializeField]
		[EventRef]
		protected string m_kickbackEventPath;

		// Token: 0x0400535B RID: 21339
		protected const string AIM_SPEED_PARAMETER_NAME = "aim_speed";

		// Token: 0x0400535C RID: 21340
		protected const float MIN_AIM_SPEED_DELTA = 0.1f;

		// Token: 0x0400535D RID: 21341
		protected AimedAbility_RL m_aimedAbility;

		// Token: 0x0400535E RID: 21342
		protected bool m_isAbilityStarted;

		// Token: 0x0400535F RID: 21343
		protected PARAMETER_ID m_aimSpeedParameterID;

		// Token: 0x04005360 RID: 21344
		protected float m_prevAimSpeedChange;

		// Token: 0x04005361 RID: 21345
		protected EventInstance m_aimEventInstance;

		// Token: 0x04005362 RID: 21346
		protected EventInstance m_beginAbilityEventInstance;

		// Token: 0x04005363 RID: 21347
		protected EventInstance m_switchSideEventInstance;

		// Token: 0x04005364 RID: 21348
		protected EventInstance m_abilityFiredEventInstance;

		// Token: 0x04005365 RID: 21349
		protected EventInstance m_kickbackEventInstance;
	}
}
