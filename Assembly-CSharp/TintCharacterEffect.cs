using System;
using UnityEngine;

// Token: 0x02000429 RID: 1065
public class TintCharacterEffect : BaseEffect
{
	// Token: 0x06002740 RID: 10048 RVA: 0x00082C00 File Offset: 0x00080E00
	protected override void Awake()
	{
		this.m_matPropertyBlock = new MaterialPropertyBlock();
		base.Awake();
	}

	// Token: 0x06002741 RID: 10049 RVA: 0x00082C14 File Offset: 0x00080E14
	public override void Play(float duration, EffectStopType stopType)
	{
		if (PlayerManager.IsInstantiated)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			if (Time.time - playerController.TimeEnteredRoom < 0.1f)
			{
				duration = 0f;
			}
		}
		base.Play(duration, stopType);
		BlinkPulseEffect componentInChildren = base.Source.GetComponentInChildren<BlinkPulseEffect>();
		if (!componentInChildren)
		{
			Debug.Log("Cannot perform TintCharacterEffect.  No BlinkPulseEffect component found on " + base.Source.name);
			return;
		}
		componentInChildren.ActivateBlackFill(BlackFillType.TintEffect, duration);
		this.PlayComplete();
	}

	// Token: 0x06002742 RID: 10050 RVA: 0x00082C8D File Offset: 0x00080E8D
	public override void Stop(EffectStopType stopType)
	{
		this.PlayComplete();
	}

	// Token: 0x06002743 RID: 10051 RVA: 0x00082C95 File Offset: 0x00080E95
	public override void ResetValues()
	{
		this.m_useUnscaledTime = false;
		base.ResetValues();
	}

	// Token: 0x040020F0 RID: 8432
	[SerializeField]
	private Color m_tintColor;

	// Token: 0x040020F1 RID: 8433
	private bool m_useUnscaledTime;

	// Token: 0x040020F2 RID: 8434
	private MaterialPropertyBlock m_matPropertyBlock;
}
