using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000459 RID: 1113
public class ShrinkOnDash_Hazard : Hazard
{
	// Token: 0x0600291A RID: 10522 RVA: 0x00087F59 File Offset: 0x00086159
	protected override void Awake()
	{
		base.Awake();
		this.m_onPlayerDash = new Action<MonoBehaviour, EventArgs>(this.OnPlayerDash);
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x0600291B RID: 10523 RVA: 0x00087F84 File Offset: 0x00086184
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDash, this.m_onPlayerDash);
	}

	// Token: 0x0600291C RID: 10524 RVA: 0x00087F93 File Offset: 0x00086193
	protected override void OnDisable()
	{
		base.OnDisable();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDash, this.m_onPlayerDash);
	}

	// Token: 0x0600291D RID: 10525 RVA: 0x00087FA8 File Offset: 0x000861A8
	public override void Initialize(HazardArgs hazardArgs)
	{
		base.InitialState = hazardArgs.InitialState;
		StateID initialState = base.InitialState;
		if (initialState != StateID.One)
		{
			if (initialState != StateID.Two)
			{
				if (initialState == StateID.Random)
				{
					if (CDGHelper.RandomPlusMinus() > 0)
					{
						this.m_initialScale = 6f;
					}
					else
					{
						this.m_initialScale = 6f;
					}
				}
			}
			else
			{
				this.m_initialScale = 6f;
			}
		}
		else
		{
			this.m_initialScale = 6f;
		}
		base.transform.localScale = new Vector3(this.m_initialScale, this.m_initialScale, base.transform.localScale.z);
	}

	// Token: 0x0600291E RID: 10526 RVA: 0x00088040 File Offset: 0x00086240
	private void OnPlayerDash(MonoBehaviour sender, EventArgs args)
	{
		this.StopShrinkCoroutine();
		base.StartCoroutine(this.ShrinkCoroutine());
	}

	// Token: 0x0600291F RID: 10527 RVA: 0x00088058 File Offset: 0x00086258
	private void StopShrinkCoroutine()
	{
		base.StopAllCoroutines();
		if (this.m_shrinkTween != null)
		{
			this.m_shrinkTween.StopTweenWithConditionChecks(false, base.transform, "ShrinkOnDash_Shrink");
		}
		if (this.m_growTween != null)
		{
			this.m_growTween.StopTweenWithConditionChecks(false, base.transform, "ShrinkOnDash_Grow");
		}
	}

	// Token: 0x06002920 RID: 10528 RVA: 0x000880B5 File Offset: 0x000862B5
	private IEnumerator ShrinkCoroutine()
	{
		float num = 0.25f;
		this.m_shrinkTween = TweenManager.TweenTo(base.transform, 0.25f, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
		{
			"localScale.x",
			num,
			"localScale.y",
			num
		});
		this.m_shrinkTween.ID = "ShrinkOnDash_Shrink";
		yield return this.m_shrinkTween.TweenCoroutine;
		this.m_waitYield.CreateNew(2f, false);
		yield return this.m_waitYield;
		float duration = this.m_initialScale / 1.5f;
		this.m_growTween = TweenManager.TweenTo(base.transform, duration, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
		{
			"localScale.x",
			this.m_initialScale,
			"localScale.y",
			this.m_initialScale
		});
		this.m_growTween.ID = "ShrinkOnDash_Grow";
		yield return this.m_growTween.TweenCoroutine;
		yield break;
	}

	// Token: 0x06002921 RID: 10529 RVA: 0x000880C4 File Offset: 0x000862C4
	public override void ResetHazard()
	{
		this.StopShrinkCoroutine();
		base.transform.localScale = new Vector3(this.m_initialScale, this.m_initialScale, base.transform.localScale.z);
	}

	// Token: 0x040021E5 RID: 8677
	private float m_initialScale;

	// Token: 0x040021E6 RID: 8678
	private WaitRL_Yield m_waitYield;

	// Token: 0x040021E7 RID: 8679
	private Tween m_shrinkTween;

	// Token: 0x040021E8 RID: 8680
	private Tween m_growTween;

	// Token: 0x040021E9 RID: 8681
	private Action<MonoBehaviour, EventArgs> m_onPlayerDash;
}
