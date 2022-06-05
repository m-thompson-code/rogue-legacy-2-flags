using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000742 RID: 1858
public class ShrinkOnDash_Hazard : Hazard
{
	// Token: 0x060038DC RID: 14556 RVA: 0x0001F3B0 File Offset: 0x0001D5B0
	protected override void Awake()
	{
		base.Awake();
		this.m_onPlayerDash = new Action<MonoBehaviour, EventArgs>(this.OnPlayerDash);
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x060038DD RID: 14557 RVA: 0x0001F3DB File Offset: 0x0001D5DB
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDash, this.m_onPlayerDash);
	}

	// Token: 0x060038DE RID: 14558 RVA: 0x0001F3EA File Offset: 0x0001D5EA
	protected override void OnDisable()
	{
		base.OnDisable();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDash, this.m_onPlayerDash);
	}

	// Token: 0x060038DF RID: 14559 RVA: 0x000E9AA8 File Offset: 0x000E7CA8
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

	// Token: 0x060038E0 RID: 14560 RVA: 0x0001F3FF File Offset: 0x0001D5FF
	private void OnPlayerDash(MonoBehaviour sender, EventArgs args)
	{
		this.StopShrinkCoroutine();
		base.StartCoroutine(this.ShrinkCoroutine());
	}

	// Token: 0x060038E1 RID: 14561 RVA: 0x000E9B40 File Offset: 0x000E7D40
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

	// Token: 0x060038E2 RID: 14562 RVA: 0x0001F414 File Offset: 0x0001D614
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

	// Token: 0x060038E3 RID: 14563 RVA: 0x0001F423 File Offset: 0x0001D623
	public override void ResetHazard()
	{
		this.StopShrinkCoroutine();
		base.transform.localScale = new Vector3(this.m_initialScale, this.m_initialScale, base.transform.localScale.z);
	}

	// Token: 0x04002D93 RID: 11667
	private float m_initialScale;

	// Token: 0x04002D94 RID: 11668
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002D95 RID: 11669
	private Tween m_shrinkTween;

	// Token: 0x04002D96 RID: 11670
	private Tween m_growTween;

	// Token: 0x04002D97 RID: 11671
	private Action<MonoBehaviour, EventArgs> m_onPlayerDash;
}
