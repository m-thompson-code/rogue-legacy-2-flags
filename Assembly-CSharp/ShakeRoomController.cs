using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using UnityEngine;

// Token: 0x020004BD RID: 1213
public class ShakeRoomController : MonoBehaviour
{
	// Token: 0x06002D1F RID: 11551 RVA: 0x00098F10 File Offset: 0x00097110
	private void OnEnable()
	{
		if (!this.m_debrisSFXEventInstance.isValid())
		{
			this.m_debrisSFXEventInstance = AudioUtility.GetEventInstance("event:/UI/FrontEnd/ui_fe_story_castleShake_debris_loop", base.transform);
		}
		if (!this.m_rumbleEventInstance.isValid())
		{
			this.m_rumbleEventInstance = AudioUtility.GetEventInstance("event:/UI/FrontEnd/ui_fe_story_castleShake_rumble_loop", base.transform);
		}
		base.StartCoroutine(this.ShakeRoomCoroutine());
		base.StartCoroutine(this.StartRumbleSFX());
	}

	// Token: 0x06002D20 RID: 11552 RVA: 0x00098F7D File Offset: 0x0009717D
	private IEnumerator StartRumbleSFX()
	{
		float delay = 0.1f + Time.time;
		while (Time.time < delay)
		{
			yield return null;
		}
		if (this.m_rumbleEventInstance.isValid())
		{
			AudioManager.Play(null, this.m_rumbleEventInstance);
		}
		yield break;
	}

	// Token: 0x06002D21 RID: 11553 RVA: 0x00098F8C File Offset: 0x0009718C
	private void OnDestroy()
	{
		if (this.m_debrisSFXEventInstance.isValid())
		{
			this.m_debrisSFXEventInstance.release();
		}
		if (this.m_rumbleEventInstance.isValid())
		{
			this.m_rumbleEventInstance.release();
		}
	}

	// Token: 0x06002D22 RID: 11554 RVA: 0x00098FC0 File Offset: 0x000971C0
	private IEnumerator ShakeRoomCoroutine()
	{
		float delay = UnityEngine.Random.Range(this.m_shakeInterval.x, this.m_shakeInterval.y);
		delay += Time.time;
		while (Time.time < delay)
		{
			yield return null;
		}
		float num = UnityEngine.Random.Range(this.m_shakeDuration.x, this.m_shakeDuration.y);
		float earthquakeDuration = num + UnityEngine.Random.Range(0f, 1f);
		this.m_shakeEffect = EffectManager.PlayEffect(base.gameObject, null, "CameraShakeVerySmall_Effect", Vector2.zero, num, EffectStopType.Gracefully, EffectTriggerDirection.None);
		base.StartCoroutine(this.PlayDebrisSFX(earthquakeDuration));
		delay = Time.time + UnityEngine.Random.Range(0f, 0.5f);
		while (Time.time < delay)
		{
			yield return null;
		}
		EffectManager.PlayEffect(base.gameObject, null, "Earthquake_Effect", PlayerManager.GetPlayerController().Midpoint, earthquakeDuration, EffectStopType.Gracefully, EffectTriggerDirection.None);
		while (this.m_shakeEffect.IsPlaying)
		{
			yield return null;
		}
		base.StartCoroutine(this.ShakeRoomCoroutine());
		yield break;
	}

	// Token: 0x06002D23 RID: 11555 RVA: 0x00098FCF File Offset: 0x000971CF
	private IEnumerator PlayDebrisSFX(float duration)
	{
		if (this.m_debrisSFXEventInstance.isValid())
		{
			AudioManager.Play(null, this.m_debrisSFXEventInstance);
		}
		duration += 1.5f;
		duration += Time.time;
		while (Time.time < duration)
		{
			yield return null;
		}
		if (this.m_debrisSFXEventInstance.isValid())
		{
			AudioManager.Stop(this.m_debrisSFXEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		AudioManager.PlayOneShot(null, "event:/UI/FrontEnd/ui_fe_story_castleShake_debris_stop", base.transform.position);
		yield break;
	}

	// Token: 0x06002D24 RID: 11556 RVA: 0x00098FE8 File Offset: 0x000971E8
	private void OnDisable()
	{
		if (this.m_shakeEffect && !this.m_shakeEffect.IsFreePoolObj)
		{
			this.m_shakeEffect.Stop(EffectStopType.Immediate);
		}
		if (this.m_debrisSFXEventInstance.isValid())
		{
			AudioManager.Stop(this.m_debrisSFXEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		if (this.m_rumbleEventInstance.isValid())
		{
			AudioManager.Stop(this.m_rumbleEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		this.m_shakeEffect = null;
	}

	// Token: 0x0400243A RID: 9274
	[SerializeField]
	private Vector2 m_shakeDuration = new Vector2(0.5f, 1f);

	// Token: 0x0400243B RID: 9275
	[SerializeField]
	private Vector2 m_shakeInterval = new Vector2(1f, 2f);

	// Token: 0x0400243C RID: 9276
	private EventInstance m_debrisSFXEventInstance;

	// Token: 0x0400243D RID: 9277
	private EventInstance m_rumbleEventInstance;

	// Token: 0x0400243E RID: 9278
	private BaseEffect m_shakeEffect;
}
