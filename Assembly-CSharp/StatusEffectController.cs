using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000554 RID: 1364
public class StatusEffectController : MonoBehaviour
{
	// Token: 0x1700119E RID: 4510
	// (get) Token: 0x06002BB1 RID: 11185 RVA: 0x0001851F File Offset: 0x0001671F
	public bool HasInvulnStack
	{
		get
		{
			return this.IsInitialized && this.m_invulnStatusEffectStacks.Count > 0;
		}
	}

	// Token: 0x1700119F RID: 4511
	// (get) Token: 0x06002BB2 RID: 11186 RVA: 0x00018539 File Offset: 0x00016739
	// (set) Token: 0x06002BB3 RID: 11187 RVA: 0x00018541 File Offset: 0x00016741
	public bool ImmuneToAllStatusEffects { get; set; }

	// Token: 0x170011A0 RID: 4512
	// (get) Token: 0x06002BB4 RID: 11188 RVA: 0x0001854A File Offset: 0x0001674A
	// (set) Token: 0x06002BB5 RID: 11189 RVA: 0x00018552 File Offset: 0x00016752
	public bool IsInitialized { get; private set; }

	// Token: 0x06002BB6 RID: 11190 RVA: 0x000C44F8 File Offset: 0x000C26F8
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

	// Token: 0x06002BB7 RID: 11191 RVA: 0x000C453C File Offset: 0x000C273C
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

	// Token: 0x06002BB8 RID: 11192 RVA: 0x000C4584 File Offset: 0x000C2784
	public bool HasStatusEffect(StatusEffectType statusEffectType)
	{
		BaseStatusEffect baseStatusEffect;
		return this.m_statusEffectTable.TryGetValue(statusEffectType, out baseStatusEffect) && baseStatusEffect.IsPlaying;
	}

	// Token: 0x06002BB9 RID: 11193 RVA: 0x000C45AC File Offset: 0x000C27AC
	public BaseStatusEffect GetStatusEffect(StatusEffectType statusEffectType)
	{
		BaseStatusEffect result;
		if (this.m_statusEffectTable.TryGetValue(statusEffectType, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06002BBA RID: 11194 RVA: 0x0001855B File Offset: 0x0001675B
	public void Initialize(BaseCharacterController charController)
	{
		this.m_charController = charController;
		this.InitializeStatusEffects();
	}

	// Token: 0x06002BBB RID: 11195 RVA: 0x000C45CC File Offset: 0x000C27CC
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

	// Token: 0x06002BBC RID: 11196 RVA: 0x0001856A File Offset: 0x0001676A
	private void OnDisable()
	{
		if (!GameManager.IsApplicationClosing)
		{
			this.StopAllStatusEffects(true);
		}
	}

	// Token: 0x06002BBD RID: 11197 RVA: 0x000C4640 File Offset: 0x000C2840
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

	// Token: 0x06002BBE RID: 11198 RVA: 0x000C472C File Offset: 0x000C292C
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

	// Token: 0x06002BBF RID: 11199 RVA: 0x000C4774 File Offset: 0x000C2974
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

	// Token: 0x06002BC0 RID: 11200 RVA: 0x0001857A File Offset: 0x0001677A
	public void AddStatusEffectImmunity(StatusEffectType immunityType)
	{
		if (!this.m_immunityList.Contains(immunityType))
		{
			this.m_immunityList.Add(immunityType);
		}
	}

	// Token: 0x06002BC1 RID: 11201 RVA: 0x00018596 File Offset: 0x00016796
	public void RemoveStatusEffectImmunity(StatusEffectType immunityType)
	{
		this.m_immunityList.Remove(immunityType);
	}

	// Token: 0x06002BC2 RID: 11202 RVA: 0x000C47C4 File Offset: 0x000C29C4
	public void SetStatusEffectHidden(StatusEffectType statusEffectType, bool hide)
	{
		BaseStatusEffect statusEffect = this.GetStatusEffect(statusEffectType);
		if (statusEffect)
		{
			statusEffect.SetIsHidden(hide);
		}
	}

	// Token: 0x06002BC3 RID: 11203 RVA: 0x000C47E8 File Offset: 0x000C29E8
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

	// Token: 0x06002BC4 RID: 11204 RVA: 0x000185A5 File Offset: 0x000167A5
	public void AddStatusEffectInvulnStack(StatusEffectType statusEffectType)
	{
		this.m_invulnStatusEffectStacks.Add(statusEffectType);
	}

	// Token: 0x06002BC5 RID: 11205 RVA: 0x000185B4 File Offset: 0x000167B4
	public void RemoveStatusEffectInvulnStack(StatusEffectType statusEffectType)
	{
		if (this.m_invulnStatusEffectStacks.Contains(statusEffectType))
		{
			this.m_invulnStatusEffectStacks.Remove(statusEffectType);
		}
	}

	// Token: 0x0400250D RID: 9485
	public static bool DISABLE_ADDING_STATUS_EFFECTS;

	// Token: 0x0400250E RID: 9486
	private BaseCharacterController m_charController;

	// Token: 0x0400250F RID: 9487
	private List<StatusEffectType> m_immunityList;

	// Token: 0x04002510 RID: 9488
	private HashSet<StatusEffectType> m_invulnStatusEffectStacks;

	// Token: 0x04002511 RID: 9489
	private Dictionary<StatusEffectType, BaseStatusEffect> m_statusEffectTable;
}
