using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

namespace RLAudio
{
	// Token: 0x020008D1 RID: 2257
	public class AimedAbilityAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700181A RID: 6170
		// (get) Token: 0x06004A16 RID: 18966 RVA: 0x0010AC3C File Offset: 0x00108E3C
		public string Description
		{
			get
			{
				return "Aimed Ability Audio Event Emitter Controller";
			}
		}

		// Token: 0x06004A17 RID: 18967 RVA: 0x0010AC44 File Offset: 0x00108E44
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

		// Token: 0x06004A18 RID: 18968 RVA: 0x0010AD98 File Offset: 0x00108F98
		protected void OnSwitchSides()
		{
			if (this.m_switchSideEventInstance.isValid())
			{
				AudioManager.PlayAttached(this, this.m_switchSideEventInstance, base.gameObject);
			}
		}

		// Token: 0x06004A19 RID: 18969 RVA: 0x0010ADBC File Offset: 0x00108FBC
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

		// Token: 0x06004A1A RID: 18970 RVA: 0x0010AE0B File Offset: 0x0010900B
		protected virtual void OnAbilityFired(Projectile_RL projectile)
		{
			this.m_isAbilityStarted = false;
			if (this.m_abilityFiredEventInstance.isValid())
			{
				AudioManager.PlayAttached(this, this.m_abilityFiredEventInstance, base.gameObject);
			}
			AudioManager.Stop(this.m_aimEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}

		// Token: 0x06004A1B RID: 18971 RVA: 0x0010AE40 File Offset: 0x00109040
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

		// Token: 0x06004A1C RID: 18972 RVA: 0x0010AEAC File Offset: 0x001090AC
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

		// Token: 0x06004A1D RID: 18973 RVA: 0x0010AF26 File Offset: 0x00109126
		protected void OnKickback()
		{
			if (this.m_kickbackEventInstance.isValid())
			{
				AudioManager.PlayAttached(this, this.m_kickbackEventInstance, base.gameObject);
			}
		}

		// Token: 0x06004A1E RID: 18974 RVA: 0x0010AF48 File Offset: 0x00109148
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

		// Token: 0x04003E44 RID: 15940
		[SerializeField]
		[EventRef]
		[FormerlySerializedAs("m_bowDrawEventPath")]
		protected string m_abilityBeginEventPath;

		// Token: 0x04003E45 RID: 15941
		[SerializeField]
		[EventRef]
		[FormerlySerializedAs("m_bowAimEventPath")]
		protected string m_aimEventPath;

		// Token: 0x04003E46 RID: 15942
		[SerializeField]
		[EventRef]
		protected string m_switchSidesEventPath;

		// Token: 0x04003E47 RID: 15943
		[SerializeField]
		[EventRef]
		[FormerlySerializedAs("m_bowFiredEventPath")]
		protected string m_abilityFiredEventPath;

		// Token: 0x04003E48 RID: 15944
		[SerializeField]
		[EventRef]
		protected string m_kickbackEventPath;

		// Token: 0x04003E49 RID: 15945
		protected const string AIM_SPEED_PARAMETER_NAME = "aim_speed";

		// Token: 0x04003E4A RID: 15946
		protected const float MIN_AIM_SPEED_DELTA = 0.1f;

		// Token: 0x04003E4B RID: 15947
		protected AimedAbility_RL m_aimedAbility;

		// Token: 0x04003E4C RID: 15948
		protected bool m_isAbilityStarted;

		// Token: 0x04003E4D RID: 15949
		protected PARAMETER_ID m_aimSpeedParameterID;

		// Token: 0x04003E4E RID: 15950
		protected float m_prevAimSpeedChange;

		// Token: 0x04003E4F RID: 15951
		protected EventInstance m_aimEventInstance;

		// Token: 0x04003E50 RID: 15952
		protected EventInstance m_beginAbilityEventInstance;

		// Token: 0x04003E51 RID: 15953
		protected EventInstance m_switchSideEventInstance;

		// Token: 0x04003E52 RID: 15954
		protected EventInstance m_abilityFiredEventInstance;

		// Token: 0x04003E53 RID: 15955
		protected EventInstance m_kickbackEventInstance;
	}
}
