using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

namespace RLAudio
{
	// Token: 0x020008E9 RID: 2281
	public class DamageAudioController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700184E RID: 6222
		// (get) Token: 0x06004AE2 RID: 19170 RVA: 0x0010D08D File Offset: 0x0010B28D
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

		// Token: 0x06004AE3 RID: 19171 RVA: 0x0010D0B4 File Offset: 0x0010B2B4
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

		// Token: 0x06004AE4 RID: 19172 RVA: 0x0010D10C File Offset: 0x0010B30C
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

		// Token: 0x06004AE5 RID: 19173 RVA: 0x0010D160 File Offset: 0x0010B360
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

		// Token: 0x06004AE6 RID: 19174 RVA: 0x0010D2BC File Offset: 0x0010B4BC
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

		// Token: 0x06004AE7 RID: 19175 RVA: 0x0010D2FE File Offset: 0x0010B4FE
		public void PlayImmuneHit()
		{
			if (this.m_immuneAudioEvent)
			{
				AudioManager.Play(this, this.m_immuneAudioEvent);
			}
		}

		// Token: 0x04003ECE RID: 16078
		[SerializeField]
		private bool m_isCharacter;

		// Token: 0x04003ECF RID: 16079
		[SerializeField]
		private StudioEventEmitter m_immuneAudioEvent;

		// Token: 0x04003ED0 RID: 16080
		[SerializeField]
		private StudioEventEmitter m_critAudioEvent;

		// Token: 0x04003ED1 RID: 16081
		[SerializeField]
		[FormerlySerializedAs("m_additionalAudioEvents")]
		private StudioEventEmitter[] m_additionalHitAudioEvents;

		// Token: 0x04003ED2 RID: 16082
		[SerializeField]
		private StudioEventEmitter[] m_deathAudioEvents;

		// Token: 0x04003ED3 RID: 16083
		private string m_description = string.Empty;
	}
}
