using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020006DB RID: 1755
public class FadeEffect : BaseEffect
{
	// Token: 0x060035CB RID: 13771 RVA: 0x0001D7FE File Offset: 0x0001B9FE
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x060035CC RID: 13772 RVA: 0x000E27B4 File Offset: 0x000E09B4
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

	// Token: 0x060035CD RID: 13773 RVA: 0x0001D817 File Offset: 0x0001BA17
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

	// Token: 0x060035CE RID: 13774 RVA: 0x0001D834 File Offset: 0x0001BA34
	public override void Stop(EffectStopType stopType)
	{
		if (this.m_fadeTween != null)
		{
			this.m_fadeTween.StopTweenWithConditionChecks(false, this.m_fadeObj, "Fading");
		}
		this.PlayComplete();
	}

	// Token: 0x04002BAE RID: 11182
	[SerializeField]
	private FadeEffect.FadeObjectType m_fadeObjType;

	// Token: 0x04002BAF RID: 11183
	[SerializeField]
	private float m_fadeInDuration;

	// Token: 0x04002BB0 RID: 11184
	[SerializeField]
	private float m_stayDuration;

	// Token: 0x04002BB1 RID: 11185
	[SerializeField]
	private float m_fadeOutDuration;

	// Token: 0x04002BB2 RID: 11186
	[SerializeField]
	private bool m_fadeOutThenFadeIn;

	// Token: 0x04002BB3 RID: 11187
	[SerializeField]
	private bool m_useUnscaledTime;

	// Token: 0x04002BB4 RID: 11188
	private Tween m_fadeTween;

	// Token: 0x04002BB5 RID: 11189
	private object m_fadeObj;

	// Token: 0x04002BB6 RID: 11190
	private WaitRL_Yield m_waitYield;

	// Token: 0x020006DC RID: 1756
	private enum FadeObjectType
	{
		// Token: 0x04002BB8 RID: 11192
		CanvasGroup,
		// Token: 0x04002BB9 RID: 11193
		Image,
		// Token: 0x04002BBA RID: 11194
		SpriteRenderer
	}
}
