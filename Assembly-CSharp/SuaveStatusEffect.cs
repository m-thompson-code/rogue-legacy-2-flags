using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000557 RID: 1367
public class SuaveStatusEffect : BaseStatusEffect
{
	// Token: 0x170011A3 RID: 4515
	// (get) Token: 0x06002BD2 RID: 11218 RVA: 0x0001860A File Offset: 0x0001680A
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Player_Suave;
		}
	}

	// Token: 0x170011A4 RID: 4516
	// (get) Token: 0x06002BD3 RID: 11219 RVA: 0x0001832B File Offset: 0x0001652B
	public override float StartingDurationOverride
	{
		get
		{
			return 9999999f;
		}
	}

	// Token: 0x06002BD4 RID: 11220 RVA: 0x00018611 File Offset: 0x00016811
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		(this.m_charController as PlayerController).InitializeStrengthMods();
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06002BD5 RID: 11221 RVA: 0x00018620 File Offset: 0x00016820
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		if (PlayerManager.IsInstantiated && !GameManager.IsApplicationClosing)
		{
			(this.m_charController as PlayerController).InitializeStrengthMods();
		}
	}
}
