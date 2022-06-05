using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using UnityEngine;

// Token: 0x020007D5 RID: 2005
public class ShakeRoomController : MonoBehaviour
{
	// Token: 0x06003DCB RID: 15819 RVA: 0x000F9D00 File Offset: 0x000F7F00
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

	// Token: 0x06003DCC RID: 15820 RVA: 0x00022347 File Offset: 0x00020547
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

	// Token: 0x06003DCD RID: 15821 RVA: 0x00022356 File Offset: 0x00020556
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

	// Token: 0x06003DCE RID: 15822 RVA: 0x0002238A File Offset: 0x0002058A
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

	// Token: 0x06003DCF RID: 15823 RVA: 0x00022399 File Offset: 0x00020599
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

	// Token: 0x06003DD0 RID: 15824 RVA: 0x000F9D70 File Offset: 0x000F7F70
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

	// Token: 0x0400309D RID: 12445
	[SerializeField]
	private Vector2 m_shakeDuration = new Vector2(0.5f, 1f);

	// Token: 0x0400309E RID: 12446
	[SerializeField]
	private Vector2 m_shakeInterval = new Vector2(1f, 2f);

	// Token: 0x0400309F RID: 12447
	private EventInstance m_debrisSFXEventInstance;

	// Token: 0x040030A0 RID: 12448
	private EventInstance m_rumbleEventInstance;

	// Token: 0x040030A1 RID: 12449
	private BaseEffect m_shakeEffect;
}
