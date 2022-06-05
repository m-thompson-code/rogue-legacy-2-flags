using System;
using UnityEngine;

// Token: 0x020006EE RID: 1774
public class TintCharacterEffect : BaseEffect
{
	// Token: 0x0600363D RID: 13885 RVA: 0x0001DCAA File Offset: 0x0001BEAA
	protected override void Awake()
	{
		this.m_matPropertyBlock = new MaterialPropertyBlock();
		base.Awake();
	}

	// Token: 0x0600363E RID: 13886 RVA: 0x000E3928 File Offset: 0x000E1B28
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

	// Token: 0x0600363F RID: 13887 RVA: 0x0001DAAD File Offset: 0x0001BCAD
	public override void Stop(EffectStopType stopType)
	{
		this.PlayComplete();
	}

	// Token: 0x06003640 RID: 13888 RVA: 0x0001DCBD File Offset: 0x0001BEBD
	public override void ResetValues()
	{
		this.m_useUnscaledTime = false;
		base.ResetValues();
	}

	// Token: 0x04002C0E RID: 11278
	[SerializeField]
	private Color m_tintColor;

	// Token: 0x04002C0F RID: 11279
	private bool m_useUnscaledTime;

	// Token: 0x04002C10 RID: 11280
	private MaterialPropertyBlock m_matPropertyBlock;
}
