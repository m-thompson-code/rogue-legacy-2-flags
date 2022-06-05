using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000315 RID: 789
public class StatusEffectController : MonoBehaviour
{
	// Token: 0x17000D85 RID: 3461
	// (get) Token: 0x06001F34 RID: 7988 RVA: 0x0006425C File Offset: 0x0006245C
	public bool HasInvulnStack
	{
		get
		{
			return this.IsInitialized && this.m_invulnStatusEffectStacks.Count > 0;
		}
	}

	// Token: 0x17000D86 RID: 3462
	// (get) Token: 0x06001F35 RID: 7989 RVA: 0x00064276 File Offset: 0x00062476
	// (set) Token: 0x06001F36 RID: 7990 RVA: 0x0006427E File Offset: 0x0006247E
	public bool ImmuneToAllStatusEffects { get; set; }

	// Token: 0x17000D87 RID: 3463
	// (get) Token: 0x06001F37 RID: 7991 RVA: 0x00064287 File Offset: 0x00062487
	// (set) Token: 0x06001F38 RID: 7992 RVA: 0x0006428F File Offset: 0x0006248F
	public bool IsInitialized { get; private set; }

	// Token: 0x06001F39 RID: 7993 RVA: 0x00064298 File Offset: 0x00062498
	public bool HasAnyActiveStatusEffect(bool includeCommanderBuffs)
	{
		foreach (StatusEffectType statusEffectType in StatusEffectType_RL.TypeArray)
		{
			if ((includeCommanderBuffs || StatusEffect_EV.COMMANDER_STATUS_EFFECT_ARRAY.IndexOf(statusEffectType) == -1) && this.HasStatusEffect(statusEffectType))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001F3A RID: 7994 RVA: 0x000642DC File Offset: 0x000624DC
	public bool HasAnyActiveTintedStatusEffect()
	{
		foreach (StatusEffectType key in StatusEffectType_RL.TypeArray)
		{
			BaseStatusEffect baseStatusEffect;
			if (this.m_statusEffectTable.TryGetValue(key, out baseStatusEffect) && baseStatusEffect.IsPlaying && baseStatusEffect.AppliesTint)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001F3B RID: 7995 RVA: 0x00064324 File Offset: 0x00062524
	public bool HasStatusEffect(StatusEffectType statusEffectType)
	{
		BaseStatusEffect baseStatusEffect;
		return this.m_statusEffectTable.TryGetValue(statusEffectType, out baseStatusEffect) && baseStatusEffect.IsPlaying;
	}

	// Token: 0x06001F3C RID: 7996 RVA: 0x0006434C File Offset: 0x0006254C
	public BaseStatusEffect GetStatusEffect(StatusEffectType statusEffectType)
	{
		BaseStatusEffect result;
		if (this.m_statusEffectTable.TryGetValue(statusEffectType, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06001F3D RID: 7997 RVA: 0x0006436C File Offset: 0x0006256C
	public void Initialize(BaseCharacterController charController)
	{
		this.m_charController = charController;
		this.InitializeStatusEffects();
	}

	// Token: 0x06001F3E RID: 7998 RVA: 0x0006437C File Offset: 0x0006257C
	private void InitializeStatusEffects()
	{
		this.m_immunityList = new List<StatusEffectType>();
		BaseStatusEffect[] components = base.GetComponents<BaseStatusEffect>();
		this.m_statusEffectTable = new Dictionary<StatusEffectType, BaseStatusEffect>(components.Length);
		foreach (BaseStatusEffect baseStatusEffect in components)
		{
			this.m_statusEffectTable.Add(baseStatusEffect.StatusEffectType, baseStatusEffect);
			baseStatusEffect.Initialize(this, this.m_charController);
		}
		this.m_invulnStatusEffectStacks = new HashSet<StatusEffectType>();
		this.IsInitialized = true;
	}

	// Token: 0x06001F3F RID: 7999 RVA: 0x000643EE File Offset: 0x000625EE
	private void OnDisable()
	{
		if (!GameManager.IsApplicationClosing)
		{
			this.StopAllStatusEffects(true);
		}
	}

	// Token: 0x06001F40 RID: 8000 RVA: 0x00064400 File Offset: 0x00062600
	public void StartStatusEffect(StatusEffectType statusEffectType, float duration, IDamageObj caster)
	{
		if (StatusEffectController.DISABLE_ADDING_STATUS_EFFECTS)
		{
			return;
		}
		if (statusEffectType == StatusEffectType.None)
		{
			return;
		}
		if (this.ImmuneToAllStatusEffects)
		{
			return;
		}
		if (this.m_immunityList.Contains(statusEffectType))
		{
			return;
		}
		BaseStatusEffect baseStatusEffect;
		if (!this.m_statusEffectTable.TryGetValue(statusEffectType, out baseStatusEffect))
		{
			Debug.Log(string.Concat(new string[]
			{
				"Could not execute StatusEffect: ",
				statusEffectType.ToString(),
				" on ",
				base.name,
				". Status Effect not found.  Make sure m_statusEffectTable has the effect added"
			}));
			return;
		}
		if (duration == 0f)
		{
			duration = baseStatusEffect.StartingDurationOverride;
		}
		if (duration == 0f)
		{
			return;
		}
		if (caster != null && !string.IsNullOrEmpty(caster.RelicDamageTypeString) && (string.IsNullOrEmpty(baseStatusEffect.RelicDamageTypeString) || !baseStatusEffect.RelicDamageTypeString.Contains(caster.RelicDamageTypeString)))
		{
			BaseStatusEffect baseStatusEffect2 = baseStatusEffect;
			baseStatusEffect2.RelicDamageTypeString += caster.RelicDamageTypeString;
		}
		baseStatusEffect.StartEffect(duration, caster);
	}

	// Token: 0x06001F41 RID: 8001 RVA: 0x000644EC File Offset: 0x000626EC
	public void StopStatusEffect(StatusEffectType statusEffectType, bool interrupted)
	{
		BaseStatusEffect baseStatusEffect;
		if (this.m_statusEffectTable.TryGetValue(statusEffectType, out baseStatusEffect) && baseStatusEffect.IsPlaying)
		{
			baseStatusEffect.StopEffect(interrupted);
		}
		if (this.m_invulnStatusEffectStacks.Contains(statusEffectType))
		{
			this.m_invulnStatusEffectStacks.Remove(statusEffectType);
		}
	}

	// Token: 0x06001F42 RID: 8002 RVA: 0x00064534 File Offset: 0x00062734
	public void StopAllStatusEffects(bool interrupted)
	{
		foreach (StatusEffectType key in StatusEffectType_RL.TypeArray)
		{
			BaseStatusEffect baseStatusEffect;
			if (this.m_statusEffectTable.TryGetValue(key, out baseStatusEffect) && baseStatusEffect.IsPlaying)
			{
				baseStatusEffect.StopEffect(interrupted);
			}
		}
		this.m_invulnStatusEffectStacks.Clear();
	}

	// Token: 0x06001F43 RID: 8003 RVA: 0x00064583 File Offset: 0x00062783
	public void AddStatusEffectImmunity(StatusEffectType immunityType)
	{
		if (!this.m_immunityList.Contains(immunityType))
		{
			this.m_immunityList.Add(immunityType);
		}
	}

	// Token: 0x06001F44 RID: 8004 RVA: 0x0006459F File Offset: 0x0006279F
	public void RemoveStatusEffectImmunity(StatusEffectType immunityType)
	{
		this.m_immunityList.Remove(immunityType);
	}

	// Token: 0x06001F45 RID: 8005 RVA: 0x000645B0 File Offset: 0x000627B0
	public void SetStatusEffectHidden(StatusEffectType statusEffectType, bool hide)
	{
		BaseStatusEffect statusEffect = this.GetStatusEffect(statusEffectType);
		if (statusEffect)
		{
			statusEffect.SetIsHidden(hide);
		}
	}

	// Token: 0x06001F46 RID: 8006 RVA: 0x000645D4 File Offset: 0x000627D4
	public void SetAllStatusEffectsHidden(bool hide)
	{
		foreach (KeyValuePair<StatusEffectType, BaseStatusEffect> keyValuePair in this.m_statusEffectTable)
		{
			this.SetStatusEffectHidden(keyValuePair.Key, hide);
		}
		if (hide)
		{
			this.m_charController.StatusBarController.SetCanvasVisible(false);
			return;
		}
		this.m_charController.StatusBarController.SetCanvasVisible(true);
	}

	// Token: 0x06001F47 RID: 8007 RVA: 0x00064654 File Offset: 0x00062854
	public void AddStatusEffectInvulnStack(StatusEffectType statusEffectType)
	{
		this.m_invulnStatusEffectStacks.Add(statusEffectType);
	}

	// Token: 0x06001F48 RID: 8008 RVA: 0x00064663 File Offset: 0x00062863
	public void RemoveStatusEffectInvulnStack(StatusEffectType statusEffectType)
	{
		if (this.m_invulnStatusEffectStacks.Contains(statusEffectType))
		{
			this.m_invulnStatusEffectStacks.Remove(statusEffectType);
		}
	}

	// Token: 0x04001C03 RID: 7171
	public static bool DISABLE_ADDING_STATUS_EFFECTS;

	// Token: 0x04001C04 RID: 7172
	private BaseCharacterController m_charController;

	// Token: 0x04001C05 RID: 7173
	private List<StatusEffectType> m_immunityList;

	// Token: 0x04001C06 RID: 7174
	private HashSet<StatusEffectType> m_invulnStatusEffectStacks;

	// Token: 0x04001C07 RID: 7175
	private Dictionary<StatusEffectType, BaseStatusEffect> m_statusEffectTable;
}
