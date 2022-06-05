using System;
using FMODUnity;
using RLAudio;
using UnityEngine;

// Token: 0x020001BB RID: 443
public class FinalBossDamageAudioController : DamageAudioController
{
	// Token: 0x1700098F RID: 2447
	// (get) Token: 0x0600114A RID: 4426 RVA: 0x00032220 File Offset: 0x00030420
	private bool IsInWhiteMode
	{
		get
		{
			if (!this.m_logicScript)
			{
				EnemyController component = base.GetComponent<EnemyController>();
				this.m_logicScript = (component.LogicController.LogicScript as FinalBoss_Basic_AIScript);
			}
			return this.m_logicScript.IsInWhiteMode;
		}
	}

	// Token: 0x0600114B RID: 4427 RVA: 0x00032262 File Offset: 0x00030462
	protected override void OnTakeDamage(GameObject attacker, float damageTaken, bool isCrit)
	{
		base.OnTakeDamage(attacker, damageTaken, isCrit);
		if (this.IsInWhiteMode)
		{
			AudioManager.Play(this, this.m_lightHitEmitter);
			return;
		}
		AudioManager.Play(this, this.m_darkHitEmitter);
	}

	// Token: 0x04001239 RID: 4665
	[SerializeField]
	private StudioEventEmitter m_lightHitEmitter;

	// Token: 0x0400123A RID: 4666
	[SerializeField]
	private StudioEventEmitter m_darkHitEmitter;

	// Token: 0x0400123B RID: 4667
	private FinalBoss_Basic_AIScript m_logicScript;
}
