using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000427 RID: 1063
public class SelfAnimateEffect : BaseEffect
{
	// Token: 0x06002734 RID: 10036 RVA: 0x00082A01 File Offset: 0x00080C01
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, true);
	}

	// Token: 0x06002735 RID: 10037 RVA: 0x00082A1A File Offset: 0x00080C1A
	public override void Play(float duration = 0f, EffectStopType stopType = EffectStopType.Gracefully)
	{
		base.Play(duration, stopType);
		if (base.Source)
		{
			if (base.SourceAnimator)
			{
				base.StartCoroutine(this.PlayTimedAnimator(duration, stopType));
				return;
			}
			Debug.Log("<color=yellow>Cannot Trigger SelfAnimate Effect.  Source does not have animator.</color>");
		}
	}

	// Token: 0x06002736 RID: 10038 RVA: 0x00082A58 File Offset: 0x00080C58
	private IEnumerator PlayTimedAnimator(float duration, EffectStopType stopType)
	{
		string text = "";
		string endParamName = "";
		if ((base.EffectDirection & EffectTriggerDirection.MovingDown) != EffectTriggerDirection.None)
		{
			text = this.m_movingDownParamStart;
			endParamName = this.m_movingDownParamEnd;
		}
		else if ((base.EffectDirection & EffectTriggerDirection.MovingUp) != EffectTriggerDirection.None)
		{
			text = this.m_movingUpParamStart;
			endParamName = this.m_movingUpParamEnd;
		}
		if ((base.EffectDirection & EffectTriggerDirection.MovingLeft) != EffectTriggerDirection.None)
		{
			text = this.m_movingLeftParamStart;
			endParamName = this.m_movingLeftParamEnd;
		}
		else if ((base.EffectDirection & EffectTriggerDirection.MovingRight) != EffectTriggerDirection.None)
		{
			text = this.m_movingRightParamStart;
			endParamName = this.m_movingRightParamEnd;
		}
		if (string.IsNullOrEmpty(text))
		{
			text = this.m_movingLeftParamStart;
		}
		if (string.IsNullOrEmpty(endParamName))
		{
			endParamName = this.m_movingLeftParamEnd;
		}
		if (!string.IsNullOrEmpty(text))
		{
			base.SourceAnimator.SetTrigger(text);
		}
		if (stopType == EffectStopType.Gracefully)
		{
			yield return null;
			AnimatorStateInfo currentAnimatorStateInfo = base.SourceAnimator.GetCurrentAnimatorStateInfo(base.AnimatorLayer);
			float num = currentAnimatorStateInfo.length * (1f - currentAnimatorStateInfo.normalizedTime);
			if (duration > num)
			{
				num = duration;
			}
			this.m_waitYield.CreateNew(num, false);
		}
		else
		{
			this.m_waitYield.CreateNew(duration, false);
		}
		yield return this.m_waitYield;
		if (!string.IsNullOrEmpty(endParamName))
		{
			base.SourceAnimator.SetTrigger(endParamName);
		}
		this.PlayComplete();
		yield break;
	}

	// Token: 0x06002737 RID: 10039 RVA: 0x00082A78 File Offset: 0x00080C78
	public override void Stop(EffectStopType stopType)
	{
		string text = "";
		EffectTriggerDirection effectDirection = base.EffectDirection;
		if (effectDirection <= EffectTriggerDirection.MovingRight)
		{
			if (effectDirection != EffectTriggerDirection.Anywhere && effectDirection != EffectTriggerDirection.MovingLeft)
			{
				if (effectDirection != EffectTriggerDirection.MovingRight)
				{
					goto IL_6B;
				}
				string movingRightParamStart = this.m_movingRightParamStart;
				text = this.m_movingRightParamEnd;
				goto IL_6B;
			}
		}
		else
		{
			if (effectDirection == EffectTriggerDirection.MovingDown)
			{
				string movingDownParamStart = this.m_movingDownParamStart;
				text = this.m_movingDownParamEnd;
				goto IL_6B;
			}
			if (effectDirection == EffectTriggerDirection.MovingUp)
			{
				string movingUpParamStart = this.m_movingUpParamStart;
				text = this.m_movingUpParamEnd;
				goto IL_6B;
			}
			if (effectDirection != EffectTriggerDirection.StandingOn)
			{
				goto IL_6B;
			}
		}
		string movingLeftParamStart = this.m_movingLeftParamStart;
		text = this.m_movingLeftParamEnd;
		IL_6B:
		if (base.SourceAnimator && !string.IsNullOrEmpty(text))
		{
			base.SourceAnimator.SetTrigger(text);
		}
		this.PlayComplete();
	}

	// Token: 0x040020E0 RID: 8416
	[SerializeField]
	private string m_movingLeftParamStart;

	// Token: 0x040020E1 RID: 8417
	[SerializeField]
	private string m_movingLeftParamEnd;

	// Token: 0x040020E2 RID: 8418
	[SerializeField]
	private string m_movingRightParamStart;

	// Token: 0x040020E3 RID: 8419
	[SerializeField]
	private string m_movingRightParamEnd;

	// Token: 0x040020E4 RID: 8420
	[SerializeField]
	private string m_movingUpParamStart;

	// Token: 0x040020E5 RID: 8421
	[SerializeField]
	private string m_movingUpParamEnd;

	// Token: 0x040020E6 RID: 8422
	[SerializeField]
	private string m_movingDownParamStart;

	// Token: 0x040020E7 RID: 8423
	[SerializeField]
	private string m_movingDownParamEnd;

	// Token: 0x040020E8 RID: 8424
	[Space(10f)]
	[SerializeField]
	private bool m_runOnlyOnce;

	// Token: 0x040020E9 RID: 8425
	private WaitRL_Yield m_waitYield;
}
