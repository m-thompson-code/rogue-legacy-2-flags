using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E94 RID: 3732
	public class StatusEffectAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17002181 RID: 8577
		// (get) Token: 0x06006944 RID: 26948 RVA: 0x0003A678 File Offset: 0x00038878
		public string Description
		{
			get
			{
				if (this.m_description == string.Empty)
				{
					this.m_description = this.ToString();
				}
				return this.m_description;
			}
		}

		// Token: 0x06006945 RID: 26949 RVA: 0x00181D18 File Offset: 0x0017FF18
		protected virtual void Awake()
		{
			foreach (BaseStatusEffect baseStatusEffect in base.gameObject.GetComponents<BaseStatusEffect>())
			{
				if (StatusEffectAudioEventController.LIFETIME_AUDIO_TABLE.ContainsKey(baseStatusEffect.StatusEffectType))
				{
					baseStatusEffect.StartEffectRelay.AddListener(new Action<BaseStatusEffect>(this.OnStatusEffectStart), false);
					EventInstance eventInstance = AudioUtility.GetEventInstance(StatusEffectAudioEventController.LIFETIME_AUDIO_TABLE[baseStatusEffect.StatusEffectType], base.transform);
					this.m_lifetimeAudioEventTable.Add(baseStatusEffect.StatusEffectType, eventInstance);
				}
				if (StatusEffectAudioEventController.STOP_AUDIO_TABLE.ContainsKey(baseStatusEffect.StatusEffectType))
				{
					baseStatusEffect.StopEffectRelay.AddListener(new Action<BaseStatusEffect>(this.OnStatusEffectStop), false);
				}
			}
		}

		// Token: 0x06006946 RID: 26950 RVA: 0x00181DD0 File Offset: 0x0017FFD0
		private void OnDestroy()
		{
			foreach (KeyValuePair<StatusEffectType, EventInstance> keyValuePair in this.m_lifetimeAudioEventTable)
			{
				EventInstance value = keyValuePair.Value;
				if (value.isValid())
				{
					value.release();
				}
			}
		}

		// Token: 0x06006947 RID: 26951 RVA: 0x00181E38 File Offset: 0x00180038
		protected virtual void OnStatusEffectStart(BaseStatusEffect statusEffect)
		{
			if (StatusEffectAudioEventController.START_AUDIO_TABLE.ContainsKey(statusEffect.StatusEffectType))
			{
				string path = StatusEffectAudioEventController.START_AUDIO_TABLE[statusEffect.StatusEffectType];
				AudioManager.PlayOneShotAttached(this, path, base.gameObject);
			}
			if (this.m_lifetimeAudioEventTable.ContainsKey(statusEffect.StatusEffectType))
			{
				AudioManager.PlayAttached(this, this.m_lifetimeAudioEventTable[statusEffect.StatusEffectType], base.gameObject);
				if (StatusEffectAudioEventController.LIFETIME_PARAMETER_NAME_TABLE.ContainsKey(statusEffect.StatusEffectType))
				{
					base.StartCoroutine(this.UpdateStatusEffectParameter(statusEffect));
				}
			}
		}

		// Token: 0x06006948 RID: 26952 RVA: 0x0003A69E File Offset: 0x0003889E
		protected virtual IEnumerator UpdateStatusEffectParameter(BaseStatusEffect statusEffect)
		{
			EventInstance eventInstance = this.m_lifetimeAudioEventTable[statusEffect.StatusEffectType];
			string parameterName = StatusEffectAudioEventController.LIFETIME_PARAMETER_NAME_TABLE[statusEffect.StatusEffectType];
			if (AudioUtility.GetHasParameter(eventInstance, parameterName))
			{
				float duration = statusEffect.Duration;
				if (statusEffect.StartingDurationOverride > 0f)
				{
					duration = statusEffect.StartingDurationOverride;
				}
				if (duration > 0f)
				{
					float timeStart = Time.time;
					while (Time.time - timeStart < duration)
					{
						float value = (Time.time - timeStart) / duration;
						eventInstance.setParameterByName(parameterName, value, false);
						yield return null;
					}
				}
			}
			else
			{
				Debug.LogFormat("<color=red>| {0} | The <b>{1}</b> Status Effect's audio event instance does not have a parameter named <b>{2}</b></color>", new object[]
				{
					this,
					statusEffect.StatusEffectType,
					parameterName
				});
			}
			yield break;
		}

		// Token: 0x06006949 RID: 26953 RVA: 0x00181EC8 File Offset: 0x001800C8
		protected virtual void OnStatusEffectStop(BaseStatusEffect statusEffect)
		{
			if (this.m_lifetimeAudioEventTable.ContainsKey(statusEffect.StatusEffectType))
			{
				AudioManager.Stop(this.m_lifetimeAudioEventTable[statusEffect.StatusEffectType], FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			if (StatusEffectAudioEventController.STOP_AUDIO_TABLE.ContainsKey(statusEffect.StatusEffectType))
			{
				string path = StatusEffectAudioEventController.STOP_AUDIO_TABLE[statusEffect.StatusEffectType];
				AudioManager.PlayOneShotAttached(this, path, base.gameObject);
			}
		}

		// Token: 0x0400559C RID: 21916
		private string m_description = string.Empty;

		// Token: 0x0400559D RID: 21917
		private Dictionary<StatusEffectType, EventInstance> m_lifetimeAudioEventTable = new Dictionary<StatusEffectType, EventInstance>();

		// Token: 0x0400559E RID: 21918
		private static Dictionary<StatusEffectType, string> START_AUDIO_TABLE = new Dictionary<StatusEffectType, string>
		{
			{
				StatusEffectType.Enemy_SporeBurst,
				"event:/SFX/Spells/sfx_spell_sporeShot_hit"
			}
		};

		// Token: 0x0400559F RID: 21919
		private static Dictionary<StatusEffectType, string> LIFETIME_AUDIO_TABLE = new Dictionary<StatusEffectType, string>
		{
			{
				StatusEffectType.Enemy_ManaBurn,
				"event:/SFX/Spells/sfx_spell_manaBurn_loop"
			},
			{
				StatusEffectType.Enemy_SporeBurst,
				"event:/SFX/Spells/sfx_spell_sporeShot_buildup"
			},
			{
				StatusEffectType.Enemy_Freeze,
				"event:/SFX/Spells/sfx_spell_iceBlast_freezeLoop"
			}
		};

		// Token: 0x040055A0 RID: 21920
		private static Dictionary<StatusEffectType, string> LIFETIME_PARAMETER_NAME_TABLE = new Dictionary<StatusEffectType, string>
		{
			{
				StatusEffectType.Enemy_SporeBurst,
				"spore_buildUp"
			}
		};

		// Token: 0x040055A1 RID: 21921
		private static Dictionary<StatusEffectType, string> STOP_AUDIO_TABLE = new Dictionary<StatusEffectType, string>
		{
			{
				StatusEffectType.Enemy_ManaBurn,
				"event:/SFX/Spells/sfx_spell_manaBurn_end"
			},
			{
				StatusEffectType.Enemy_SporeBurst,
				"event:/SFX/Spells/sfx_spell_sporeShot_explode"
			},
			{
				StatusEffectType.Enemy_Freeze,
				"event:/SFX/Enemies/sfx_enemy_elemental_iceShard_crystalize"
			}
		};
	}
}
