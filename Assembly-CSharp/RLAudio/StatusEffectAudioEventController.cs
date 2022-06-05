using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000917 RID: 2327
	public class StatusEffectAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001888 RID: 6280
		// (get) Token: 0x06004C49 RID: 19529 RVA: 0x00112043 File Offset: 0x00110243
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

		// Token: 0x06004C4A RID: 19530 RVA: 0x0011206C File Offset: 0x0011026C
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

		// Token: 0x06004C4B RID: 19531 RVA: 0x00112124 File Offset: 0x00110324
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

		// Token: 0x06004C4C RID: 19532 RVA: 0x0011218C File Offset: 0x0011038C
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

		// Token: 0x06004C4D RID: 19533 RVA: 0x00112219 File Offset: 0x00110419
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

		// Token: 0x06004C4E RID: 19534 RVA: 0x00112230 File Offset: 0x00110430
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

		// Token: 0x0400403C RID: 16444
		private string m_description = string.Empty;

		// Token: 0x0400403D RID: 16445
		private Dictionary<StatusEffectType, EventInstance> m_lifetimeAudioEventTable = new Dictionary<StatusEffectType, EventInstance>();

		// Token: 0x0400403E RID: 16446
		private static Dictionary<StatusEffectType, string> START_AUDIO_TABLE = new Dictionary<StatusEffectType, string>
		{
			{
				StatusEffectType.Enemy_SporeBurst,
				"event:/SFX/Spells/sfx_spell_sporeShot_hit"
			}
		};

		// Token: 0x0400403F RID: 16447
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

		// Token: 0x04004040 RID: 16448
		private static Dictionary<StatusEffectType, string> LIFETIME_PARAMETER_NAME_TABLE = new Dictionary<StatusEffectType, string>
		{
			{
				StatusEffectType.Enemy_SporeBurst,
				"spore_buildUp"
			}
		};

		// Token: 0x04004041 RID: 16449
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
