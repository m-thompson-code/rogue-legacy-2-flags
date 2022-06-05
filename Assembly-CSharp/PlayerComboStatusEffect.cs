using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000542 RID: 1346
public class PlayerComboStatusEffect : BaseStatusEffect
{
	// Token: 0x17001168 RID: 4456
	// (get) Token: 0x06002B2A RID: 11050 RVA: 0x0001814C File Offset: 0x0001634C
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Player_Combo;
		}
	}

	// Token: 0x17001169 RID: 4457
	// (get) Token: 0x06002B2B RID: 11051 RVA: 0x00004536 File Offset: 0x00002736
	public override float StartingDurationOverride
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700116A RID: 4458
	// (get) Token: 0x06002B2C RID: 11052 RVA: 0x000C3754 File Offset: 0x000C1954
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

	// Token: 0x06002B2D RID: 11053 RVA: 0x000C3794 File Offset: 0x000C1994
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

	// Token: 0x06002B2E RID: 11054 RVA: 0x00018153 File Offset: 0x00016353
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

	// Token: 0x06002B2F RID: 11055 RVA: 0x00018162 File Offset: 0x00016362
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		if (PlayerManager.IsInstantiated)
		{
			PlayerManager.GetPlayerController().LookController.SetCritBlinkEffectEnabled(false, CritBlinkEffectTriggerType.PlayerComboStatusEffect);
		}
	}

	// Token: 0x040024CA RID: 9418
	public const float ADD_RELIC_STACKS_HACK = 3.4028235E+38f;

	// Token: 0x040024CB RID: 9419
	private bool m_addRelicStacks;
}
