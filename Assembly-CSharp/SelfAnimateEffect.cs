using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006EA RID: 1770
public class SelfAnimateEffect : BaseEffect
{
	// Token: 0x06003625 RID: 13861 RVA: 0x0001DB27 File Offset: 0x0001BD27
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, true);
	}

	// Token: 0x06003626 RID: 13862 RVA: 0x0001DB40 File Offset: 0x0001BD40
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

	// Token: 0x06003627 RID: 13863 RVA: 0x0001DB7E File Offset: 0x0001BD7E
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

	// Token: 0x06003628 RID: 13864 RVA: 0x000E3614 File Offset: 0x000E1814
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

	// Token: 0x04002BF5 RID: 11253
	[SerializeField]
	private string m_movingLeftParamStart;

	// Token: 0x04002BF6 RID: 11254
	[SerializeField]
	private string m_movingLeftParamEnd;

	// Token: 0x04002BF7 RID: 11255
	[SerializeField]
	private string m_movingRightParamStart;

	// Token: 0x04002BF8 RID: 11256
	[SerializeField]
	private string m_movingRightParamEnd;

	// Token: 0x04002BF9 RID: 11257
	[SerializeField]
	private string m_movingUpParamStart;

	// Token: 0x04002BFA RID: 11258
	[SerializeField]
	private string m_movingUpParamEnd;

	// Token: 0x04002BFB RID: 11259
	[SerializeField]
	private string m_movingDownParamStart;

	// Token: 0x04002BFC RID: 11260
	[SerializeField]
	private string m_movingDownParamEnd;

	// Token: 0x04002BFD RID: 11261
	[Space(10f)]
	[SerializeField]
	private bool m_runOnlyOnce;

	// Token: 0x04002BFE RID: 11262
	private WaitRL_Yield m_waitYield;
}
