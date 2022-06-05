using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

namespace RLAudio
{
	// Token: 0x02000E59 RID: 3673
	public class DamageAudioController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17002131 RID: 8497
		// (get) Token: 0x0600679B RID: 26523 RVA: 0x0003930E File Offset: 0x0003750E
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

		// Token: 0x0600679C RID: 26524 RVA: 0x0017D334 File Offset: 0x0017B534
		protected virtual void Awake()
		{
			IEffectTriggerEvent_OnDamage component = base.GetComponent<IEffectTriggerEvent_OnDamage>();
			if (component != null)
			{
				component.OnDamageEffectTriggerRelay.AddListener(new Action<GameObject, float, bool>(this.OnTakeDamage), false);
			}
			IEffectTriggerEvent_OnDeath component2 = base.GetComponent<IEffectTriggerEvent_OnDeath>();
			if (component2 != null)
			{
				component2.OnDeathEffectTriggerRelay.AddListener(new Action<GameObject>(this.OnDeath), false);
			}
		}

		// Token: 0x0600679D RID: 26525 RVA: 0x0017D38C File Offset: 0x0017B58C
		protected virtual void OnDestroy()
		{
			IEffectTriggerEvent_OnDamage component = base.GetComponent<IEffectTriggerEvent_OnDamage>();
			if (component != null)
			{
				component.OnDamageEffectTriggerRelay.RemoveListener(new Action<GameObject, float, bool>(this.OnTakeDamage));
			}
			IEffectTriggerEvent_OnDeath component2 = base.GetComponent<IEffectTriggerEvent_OnDeath>();
			if (component2 != null)
			{
				component2.OnDeathEffectTriggerRelay.RemoveListener(new Action<GameObject>(this.OnDeath));
			}
		}

		// Token: 0x0600679E RID: 26526 RVA: 0x0017D3E0 File Offset: 0x0017B5E0
		protected virtual void OnTakeDamage(GameObject attacker, float damageTaken, bool isCrit)
		{
			BaseDamageAudioData component = attacker.GetComponent<BaseDamageAudioData>();
			if (!component.IsNativeNull())
			{
				string text = component.BreakableDamageAudioPath;
				if (this.m_isCharacter)
				{
					text = component.CharacterDamageAudioPath;
				}
				if (!string.IsNullOrEmpty(text) && component.HitAudioParameters != null)
				{
					EventInstance eventInstance = AudioUtility.GetEventInstance(text, attacker.transform);
					foreach (KeyValuePair<string, float> keyValuePair in component.HitAudioParameters)
					{
						eventInstance.setParameterByName(keyValuePair.Key, keyValuePair.Value, false);
					}
					AudioManager.PlayAttached(this, eventInstance, attacker);
					eventInstance.release();
				}
				else if (!string.IsNullOrEmpty(text))
				{
					AudioManager.PlayOneShotAttached(this, text, base.gameObject);
				}
			}
			for (int i = 0; i < this.m_additionalHitAudioEvents.Length; i++)
			{
				if (!string.IsNullOrEmpty(this.m_additionalHitAudioEvents[i].Event))
				{
					AudioManager.Play(this, this.m_additionalHitAudioEvents[i]);
				}
			}
			if (isCrit && this.m_critAudioEvent)
			{
				AudioManager.Play(this, this.m_critAudioEvent);
			}
			if (damageTaken <= 0f && this.m_immuneAudioEvent && attacker.GetComponent<IDamageObj>().ActualDamage > 0f)
			{
				AudioManager.Play(this, this.m_immuneAudioEvent);
			}
		}

		// Token: 0x0600679F RID: 26527 RVA: 0x0017D53C File Offset: 0x0017B73C
		protected virtual void OnDeath(GameObject otherObj)
		{
			for (int i = 0; i < this.m_deathAudioEvents.Length; i++)
			{
				if (!string.IsNullOrEmpty(this.m_deathAudioEvents[i].Event))
				{
					AudioManager.Play(this, this.m_deathAudioEvents[i]);
				}
			}
		}

		// Token: 0x060067A0 RID: 26528 RVA: 0x00039334 File Offset: 0x00037534
		public void PlayImmuneHit()
		{
			if (this.m_immuneAudioEvent)
			{
				AudioManager.Play(this, this.m_immuneAudioEvent);
			}
		}

		// Token: 0x040053EC RID: 21484
		[SerializeField]
		private bool m_isCharacter;

		// Token: 0x040053ED RID: 21485
		[SerializeField]
		private StudioEventEmitter m_immuneAudioEvent;

		// Token: 0x040053EE RID: 21486
		[SerializeField]
		private StudioEventEmitter m_critAudioEvent;

		// Token: 0x040053EF RID: 21487
		[SerializeField]
		[FormerlySerializedAs("m_additionalAudioEvents")]
		private StudioEventEmitter[] m_additionalHitAudioEvents;

		// Token: 0x040053F0 RID: 21488
		[SerializeField]
		private StudioEventEmitter[] m_deathAudioEvents;

		// Token: 0x040053F1 RID: 21489
		private string m_description = string.Empty;
	}
}
