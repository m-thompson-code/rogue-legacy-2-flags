using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000317 RID: 791
public class SuaveStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D88 RID: 3464
	// (get) Token: 0x06001F4F RID: 8015 RVA: 0x00064807 File Offset: 0x00062A07
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Player_Suave;
		}
	}

	// Token: 0x17000D89 RID: 3465
	// (get) Token: 0x06001F50 RID: 8016 RVA: 0x0006480E File Offset: 0x00062A0E
	public override float StartingDurationOverride
	{
		get
		{
			return 9999999f;
		}
	}

	// Token: 0x06001F51 RID: 8017 RVA: 0x00064815 File Offset: 0x00062A15
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

	// Token: 0x06001F52 RID: 8018 RVA: 0x00064824 File Offset: 0x00062A24
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		if (PlayerManager.IsInstantiated && !GameManager.IsApplicationClosing)
		{
			(this.m_charController as PlayerController).InitializeStrengthMods();
		}
	}
}
