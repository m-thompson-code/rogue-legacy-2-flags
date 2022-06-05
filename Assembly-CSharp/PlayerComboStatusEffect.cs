using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200030E RID: 782
public class PlayerComboStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D65 RID: 3429
	// (get) Token: 0x06001EEF RID: 7919 RVA: 0x00063CB0 File Offset: 0x00061EB0
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Player_Combo;
		}
	}

	// Token: 0x17000D66 RID: 3430
	// (get) Token: 0x06001EF0 RID: 7920 RVA: 0x00063CB7 File Offset: 0x00061EB7
	public override float StartingDurationOverride
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000D67 RID: 3431
	// (get) Token: 0x06001EF1 RID: 7921 RVA: 0x00063CC0 File Offset: 0x00061EC0
	public override int StacksPerHit
	{
		get
		{
			int num = base.StacksPerHit;
			int level = SaveManager.PlayerSaveData.GetRelic(RelicType.WeaponsComboAdd).Level;
			if (level > 0)
			{
				if (this.m_addRelicStacks)
				{
					num += level;
				}
				else
				{
					num = level;
				}
			}
			return num;
		}
	}

	// Token: 0x06001EF2 RID: 7922 RVA: 0x00063D00 File Offset: 0x00061F00
	public override void StartEffect(float duration, IDamageObj caster)
	{
		float duration2 = duration;
		this.m_addRelicStacks = false;
		if (duration == 3.4028235E+38f)
		{
			this.m_addRelicStacks = true;
			duration2 = this.StartingDurationOverride;
		}
		base.StartEffect(duration2, caster);
	}

	// Token: 0x06001EF3 RID: 7923 RVA: 0x00063D34 File Offset: 0x00061F34
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		base.TimesStacked = Mathf.Clamp(base.TimesStacked, 0, 30);
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Combo, base.Duration, 30, base.TimesStacked);
		if (base.TimesStacked >= 15)
		{
			PlayerManager.GetPlayerController().LookController.SetCritBlinkEffectEnabled(true, CritBlinkEffectTriggerType.PlayerComboStatusEffect);
		}
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06001EF4 RID: 7924 RVA: 0x00063D43 File Offset: 0x00061F43
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		if (PlayerManager.IsInstantiated)
		{
			PlayerManager.GetPlayerController().LookController.SetCritBlinkEffectEnabled(false, CritBlinkEffectTriggerType.PlayerComboStatusEffect);
		}
	}

	// Token: 0x04001BE8 RID: 7144
	public const float ADD_RELIC_STACKS_HACK = 3.4028235E+38f;

	// Token: 0x04001BE9 RID: 7145
	private bool m_addRelicStacks;
}
