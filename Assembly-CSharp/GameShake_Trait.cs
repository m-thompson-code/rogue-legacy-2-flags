using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005A2 RID: 1442
public class GameShake_Trait : BaseTrait
{
	// Token: 0x17001211 RID: 4625
	// (get) Token: 0x06002D50 RID: 11600 RVA: 0x00019023 File Offset: 0x00017223
	public override TraitType TraitType
	{
		get
		{
			return TraitType.GameShake;
		}
	}

	// Token: 0x06002D51 RID: 11601 RVA: 0x0001902A File Offset: 0x0001722A
	private void Start()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
		if (!this.m_isShaking && !base.IsPaused)
		{
			base.StartCoroutine(this.RandomShakeCoroutine());
		}
	}

	// Token: 0x06002D52 RID: 11602 RVA: 0x0001905A File Offset: 0x0001725A
	private IEnumerator RandomShakeCoroutine()
	{
		this.m_isShaking = true;
		for (;;)
		{
			float waitTime = UnityEngine.Random.Range(Trait_EV.GAME_SHAKE_TIME_RANDOMIZER.x, Trait_EV.GAME_SHAKE_TIME_RANDOMIZER.y);
			this.m_waitYield.CreateNew(waitTime, false);
			yield return this.m_waitYield;
			float num = UnityEngine.Random.Range(Trait_EV.GAME_SHAKE_INTENSITY.x, Trait_EV.GAME_SHAKE_INTENSITY.y);
			EffectManager.SetEffectParams("CameraShake_Effect", new object[]
			{
				"m_shakeSpeed",
				num,
				"m_shakeAmplitude",
				num
			});
			float num2 = UnityEngine.Random.Range(Trait_EV.GAME_SHAKE_DURATION.x, Trait_EV.GAME_SHAKE_DURATION.y);
			this.m_shakeEffect = EffectManager.PlayEffect(PlayerManager.GetCurrentPlayerRoom().gameObject, null, "CameraShake_Effect", Vector3.zero, num2, EffectStopType.Gracefully, EffectTriggerDirection.None);
			this.m_waitYield.CreateNew(num2, false);
			yield return this.m_waitYield;
			this.m_shakeEffect = null;
		}
		yield break;
	}

	// Token: 0x06002D53 RID: 11603 RVA: 0x000C6EA0 File Offset: 0x000C50A0
	public override void SetPaused(bool paused)
	{
		base.SetPaused(paused);
		if (paused)
		{
			base.StopAllCoroutines();
			if (this.m_shakeEffect != null)
			{
				EffectManager.StopEffect(this.m_shakeEffect, EffectStopType.Immediate);
			}
			this.m_isShaking = false;
			return;
		}
		if (!this.m_isShaking)
		{
			base.StartCoroutine(this.RandomShakeCoroutine());
		}
	}

	// Token: 0x040025B2 RID: 9650
	private WaitRL_Yield m_waitYield;

	// Token: 0x040025B3 RID: 9651
	private bool m_isShaking;

	// Token: 0x040025B4 RID: 9652
	private BaseEffect m_shakeEffect;
}
