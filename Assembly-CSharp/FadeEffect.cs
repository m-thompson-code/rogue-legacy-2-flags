using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000422 RID: 1058
public class FadeEffect : BaseEffect
{
	// Token: 0x06002710 RID: 10000 RVA: 0x00082550 File Offset: 0x00080750
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06002711 RID: 10001 RVA: 0x0008256C File Offset: 0x0008076C
	public override void Play(float duration = 0f, EffectStopType stopType = EffectStopType.Gracefully)
	{
		base.Play(duration, stopType);
		base.StopAllCoroutines();
		switch (this.m_fadeObjType)
		{
		case FadeEffect.FadeObjectType.CanvasGroup:
		{
			CanvasGroup componentInChildren = base.GetComponentInChildren<CanvasGroup>();
			if (componentInChildren != null)
			{
				base.StartCoroutine(this.FadeTween(componentInChildren, "alpha"));
				return;
			}
			break;
		}
		case FadeEffect.FadeObjectType.Image:
		{
			Image componentInChildren2 = base.GetComponentInChildren<Image>();
			if (componentInChildren2 != null)
			{
				base.StartCoroutine(this.FadeTween(componentInChildren2, "color.a"));
				return;
			}
			break;
		}
		case FadeEffect.FadeObjectType.SpriteRenderer:
		{
			SpriteRenderer componentInChildren3 = base.GetComponentInChildren<SpriteRenderer>();
			if (componentInChildren3 != null)
			{
				base.StartCoroutine(this.FadeTween(componentInChildren3, "color.a"));
			}
			break;
		}
		default:
			return;
		}
	}

	// Token: 0x06002712 RID: 10002 RVA: 0x0008260C File Offset: 0x0008080C
	private IEnumerator FadeTween(object fadeObj, string property)
	{
		this.m_fadeObj = fadeObj;
		float num = 1f;
		float fadeOutEndAlpha = 0f;
		float duration = this.m_fadeInDuration;
		float fadeOutDuration = this.m_fadeOutDuration;
		if (this.m_fadeOutThenFadeIn)
		{
			num = 0f;
			fadeOutEndAlpha = 1f;
			duration = this.m_fadeOutDuration;
			fadeOutDuration = this.m_fadeInDuration;
		}
		if (!this.m_useUnscaledTime)
		{
			this.m_fadeTween = TweenManager.TweenTo(fadeObj, duration, new EaseDelegate(Ease.None), new object[]
			{
				property,
				num
			});
			this.m_fadeTween.ID = "Fading";
			yield return this.m_fadeTween.TweenCoroutine;
			if (this.m_stayDuration > 0f)
			{
				this.m_waitYield.CreateNew(this.m_stayDuration, this.m_useUnscaledTime);
				yield return this.m_waitYield;
			}
			this.m_fadeTween = TweenManager.TweenTo(fadeObj, fadeOutDuration, new EaseDelegate(Ease.None), new object[]
			{
				property,
				fadeOutEndAlpha
			});
			this.m_fadeTween.ID = "Fading";
			yield return this.m_fadeTween.TweenCoroutine;
		}
		else
		{
			this.m_fadeTween = TweenManager.TweenTo_UnscaledTime(fadeObj, duration, new EaseDelegate(Ease.None), new object[]
			{
				property,
				num
			});
			this.m_fadeTween.ID = "Fading";
			yield return this.m_fadeTween.TweenCoroutine;
			if (this.m_stayDuration > 0f)
			{
				this.m_waitYield.CreateNew(this.m_stayDuration, this.m_useUnscaledTime);
				yield return this.m_waitYield;
			}
			this.m_fadeTween = TweenManager.TweenTo_UnscaledTime(fadeObj, fadeOutDuration, new EaseDelegate(Ease.None), new object[]
			{
				property,
				fadeOutEndAlpha
			});
			this.m_fadeTween.ID = "Fading";
			yield return this.m_fadeTween.TweenCoroutine;
		}
		this.Stop(EffectStopType.Gracefully);
		yield break;
	}

	// Token: 0x06002713 RID: 10003 RVA: 0x00082629 File Offset: 0x00080829
	public override void Stop(EffectStopType stopType)
	{
		if (this.m_fadeTween != null)
		{
			this.m_fadeTween.StopTweenWithConditionChecks(false, this.m_fadeObj, "Fading");
		}
		this.PlayComplete();
	}

	// Token: 0x040020C8 RID: 8392
	[SerializeField]
	private FadeEffect.FadeObjectType m_fadeObjType;

	// Token: 0x040020C9 RID: 8393
	[SerializeField]
	private float m_fadeInDuration;

	// Token: 0x040020CA RID: 8394
	[SerializeField]
	private float m_stayDuration;

	// Token: 0x040020CB RID: 8395
	[SerializeField]
	private float m_fadeOutDuration;

	// Token: 0x040020CC RID: 8396
	[SerializeField]
	private bool m_fadeOutThenFadeIn;

	// Token: 0x040020CD RID: 8397
	[SerializeField]
	private bool m_useUnscaledTime;

	// Token: 0x040020CE RID: 8398
	private Tween m_fadeTween;

	// Token: 0x040020CF RID: 8399
	private object m_fadeObj;

	// Token: 0x040020D0 RID: 8400
	private WaitRL_Yield m_waitYield;

	// Token: 0x02000C32 RID: 3122
	private enum FadeObjectType
	{
		// Token: 0x04004F7B RID: 20347
		CanvasGroup,
		// Token: 0x04004F7C RID: 20348
		Image,
		// Token: 0x04004F7D RID: 20349
		SpriteRenderer
	}
}
