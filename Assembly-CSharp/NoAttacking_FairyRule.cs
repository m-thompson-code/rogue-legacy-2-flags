using System;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x02000705 RID: 1797
public class NoAttacking_FairyRule : FairyRule
{
	// Token: 0x17001493 RID: 5267
	// (get) Token: 0x060036DF RID: 14047 RVA: 0x0001E31A File Offset: 0x0001C51A
	public override string Description
	{
		get
		{
			return "LOC_ID_FAIRY_RULE_NO_ATTACKING_1";
		}
	}

	// Token: 0x17001494 RID: 5268
	// (get) Token: 0x060036E0 RID: 14048 RVA: 0x0000452B File Offset: 0x0000272B
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.NoAttacking;
		}
	}

	// Token: 0x17001495 RID: 5269
	// (get) Token: 0x060036E1 RID: 14049 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool LockChestAtStart
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060036E2 RID: 14050 RVA: 0x0001E321 File Offset: 0x0001C521
	private void Awake()
	{
		this.m_onAbilityCast = new Action<MonoBehaviour, EventArgs>(this.OnAbilityCast);
	}

	// Token: 0x060036E3 RID: 14051 RVA: 0x000E4EEC File Offset: 0x000E30EC
	public override void RunRule(FairyRoomController fairyRoomController)
	{
		CharacterDownStrike_RL characterDownStrike = PlayerManager.GetPlayerController().CharacterDownStrike;
		characterDownStrike.OnDownStrikeEvent = (OnDownStrikeDelegate)Delegate.Combine(characterDownStrike.OnDownStrikeEvent, new OnDownStrikeDelegate(base.SetIsFailed));
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerSpellAbilityCast, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerTalentAbilityCast, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerWeaponAbilityCast, this.m_onAbilityCast);
	}

	// Token: 0x060036E4 RID: 14052 RVA: 0x000E4F4C File Offset: 0x000E314C
	private void OnAbilityCast(object sender, EventArgs args)
	{
		AbilityUsedEventArgs abilityUsedEventArgs = args as AbilityUsedEventArgs;
		if (abilityUsedEventArgs != null && abilityUsedEventArgs.Ability.DealsNoDamage)
		{
			return;
		}
		base.SetIsFailed();
	}

	// Token: 0x060036E5 RID: 14053 RVA: 0x000E4F78 File Offset: 0x000E3178
	public override void StopRule()
	{
		base.StopRule();
		CharacterDownStrike_RL characterDownStrike = PlayerManager.GetPlayerController().CharacterDownStrike;
		characterDownStrike.OnDownStrikeEvent = (OnDownStrikeDelegate)Delegate.Remove(characterDownStrike.OnDownStrikeEvent, new OnDownStrikeDelegate(base.SetIsFailed));
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerSpellAbilityCast, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerTalentAbilityCast, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerWeaponAbilityCast, this.m_onAbilityCast);
	}

	// Token: 0x04002C69 RID: 11369
	private Action<MonoBehaviour, EventArgs> m_onAbilityCast;
}
