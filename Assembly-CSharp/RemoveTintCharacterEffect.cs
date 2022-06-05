using System;
using UnityEngine;

// Token: 0x02000425 RID: 1061
public class RemoveTintCharacterEffect : BaseEffect
{
	// Token: 0x0600272A RID: 10026 RVA: 0x00082929 File Offset: 0x00080B29
	protected override void Awake()
	{
		this.m_matPropertyBlock = new MaterialPropertyBlock();
		base.Awake();
	}

	// Token: 0x0600272B RID: 10027 RVA: 0x0008293C File Offset: 0x00080B3C
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

	// Token: 0x0600272C RID: 10028 RVA: 0x0008298E File Offset: 0x00080B8E
	public override void Stop(EffectStopType stopType)
	{
		this.PlayComplete();
	}

	// Token: 0x0600272D RID: 10029 RVA: 0x00082996 File Offset: 0x00080B96
	public override void ResetValues()
	{
		this.m_useUnscaledTime = false;
		base.ResetValues();
	}

	// Token: 0x040020DD RID: 8413
	private MaterialPropertyBlock m_matPropertyBlock;

	// Token: 0x040020DE RID: 8414
	private bool m_useUnscaledTime;
}
