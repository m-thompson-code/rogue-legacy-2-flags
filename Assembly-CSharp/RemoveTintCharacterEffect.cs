using System;
using UnityEngine;

// Token: 0x020006E7 RID: 1767
public class RemoveTintCharacterEffect : BaseEffect
{
	// Token: 0x06003615 RID: 13845 RVA: 0x0001DA9A File Offset: 0x0001BC9A
	protected override void Awake()
	{
		this.m_matPropertyBlock = new MaterialPropertyBlock();
		base.Awake();
	}

	// Token: 0x06003616 RID: 13846 RVA: 0x000E3540 File Offset: 0x000E1740
	public override void Play(float duration, EffectStopType stopType)
	{
		base.Play(duration, stopType);
		BlinkPulseEffect componentInChildren = base.Source.GetComponentInChildren<BlinkPulseEffect>();
		if (!componentInChildren)
		{
			Debug.Log("Cannot perform TintCharacterEffect.  No BlinkPulseEffect component found on " + base.Source.name);
			return;
		}
		componentInChildren.DisableBlackFill(BlackFillType.TintEffect, duration);
		this.PlayComplete();
	}

	// Token: 0x06003617 RID: 13847 RVA: 0x0001DAAD File Offset: 0x0001BCAD
	public override void Stop(EffectStopType stopType)
	{
		this.PlayComplete();
	}

	// Token: 0x06003618 RID: 13848 RVA: 0x0001DAB5 File Offset: 0x0001BCB5
	public override void ResetValues()
	{
		this.m_useUnscaledTime = false;
		base.ResetValues();
	}

	// Token: 0x04002BEE RID: 11246
	private MaterialPropertyBlock m_matPropertyBlock;

	// Token: 0x04002BEF RID: 11247
	private bool m_useUnscaledTime;
}
