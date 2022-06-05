using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000341 RID: 833
public class GameShake_Trait : BaseTrait
{
	// Token: 0x17000DC4 RID: 3524
	// (get) Token: 0x06002023 RID: 8227 RVA: 0x000663B0 File Offset: 0x000645B0
	public override TraitType TraitType
	{
		get
		{
			return TraitType.GameShake;
		}
	}

	// Token: 0x06002024 RID: 8228 RVA: 0x000663B7 File Offset: 0x000645B7
	private void Start()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
		if (!this.m_isShaking && !base.IsPaused)
		{
			base.StartCoroutine(this.RandomShakeCoroutine());
		}
	}

	// Token: 0x06002025 RID: 8229 RVA: 0x000663E7 File Offset: 0x000645E7
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

	// Token: 0x06002026 RID: 8230 RVA: 0x000663F8 File Offset: 0x000645F8
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

	// Token: 0x04001C4A RID: 7242
	private WaitRL_Yield m_waitYield;

	// Token: 0x04001C4B RID: 7243
	private bool m_isShaking;

	// Token: 0x04001C4C RID: 7244
	private BaseEffect m_shakeEffect;
}
