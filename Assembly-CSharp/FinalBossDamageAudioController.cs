using System;
using FMODUnity;
using RLAudio;
using UnityEngine;

// Token: 0x0200032A RID: 810
public class FinalBossDamageAudioController : DamageAudioController
{
	// Token: 0x17000C4F RID: 3151
	// (get) Token: 0x06001993 RID: 6547 RVA: 0x00090510 File Offset: 0x0008E710
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

	// Token: 0x06001994 RID: 6548 RVA: 0x0000CEA0 File Offset: 0x0000B0A0
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

	// Token: 0x04001842 RID: 6210
	[SerializeField]
	private StudioEventEmitter m_lightHitEmitter;

	// Token: 0x04001843 RID: 6211
	[SerializeField]
	private StudioEventEmitter m_darkHitEmitter;

	// Token: 0x04001844 RID: 6212
	private FinalBoss_Basic_AIScript m_logicScript;
}
