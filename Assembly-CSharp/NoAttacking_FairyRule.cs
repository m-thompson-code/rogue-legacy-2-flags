using System;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x0200043C RID: 1084
public class NoAttacking_FairyRule : FairyRule
{
	// Token: 0x17000FB0 RID: 4016
	// (get) Token: 0x060027CD RID: 10189 RVA: 0x000844A3 File Offset: 0x000826A3
	public override string Description
	{
		get
		{
			return "LOC_ID_FAIRY_RULE_NO_ATTACKING_1";
		}
	}

	// Token: 0x17000FB1 RID: 4017
	// (get) Token: 0x060027CE RID: 10190 RVA: 0x000844AA File Offset: 0x000826AA
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.NoAttacking;
		}
	}

	// Token: 0x17000FB2 RID: 4018
	// (get) Token: 0x060027CF RID: 10191 RVA: 0x000844AE File Offset: 0x000826AE
	public override bool LockChestAtStart
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060027D0 RID: 10192 RVA: 0x000844B1 File Offset: 0x000826B1
	private void Awake()
	{
		this.m_onAbilityCast = new Action<MonoBehaviour, EventArgs>(this.OnAbilityCast);
	}

	// Token: 0x060027D1 RID: 10193 RVA: 0x000844C8 File Offset: 0x000826C8
	public override void RunRule(FairyRoomController fairyRoomController)
	{
		CharacterDownStrike_RL characterDownStrike = PlayerManager.GetPlayerController().CharacterDownStrike;
		characterDownStrike.OnDownStrikeEvent = (OnDownStrikeDelegate)Delegate.Combine(characterDownStrike.OnDownStrikeEvent, new OnDownStrikeDelegate(base.SetIsFailed));
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerSpellAbilityCast, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerTalentAbilityCast, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerWeaponAbilityCast, this.m_onAbilityCast);
	}

	// Token: 0x060027D2 RID: 10194 RVA: 0x00084528 File Offset: 0x00082728
	private void OnAbilityCast(object sender, EventArgs args)
	{
		AbilityUsedEventArgs abilityUsedEventArgs = args as AbilityUsedEventArgs;
		if (abilityUsedEventArgs != null && abilityUsedEventArgs.Ability.DealsNoDamage)
		{
			return;
		}
		base.SetIsFailed();
	}

	// Token: 0x060027D3 RID: 10195 RVA: 0x00084554 File Offset: 0x00082754
	public override void StopRule()
	{
		base.StopRule();
		CharacterDownStrike_RL characterDownStrike = PlayerManager.GetPlayerController().CharacterDownStrike;
		characterDownStrike.OnDownStrikeEvent = (OnDownStrikeDelegate)Delegate.Remove(characterDownStrike.OnDownStrikeEvent, new OnDownStrikeDelegate(base.SetIsFailed));
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerSpellAbilityCast, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerTalentAbilityCast, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerWeaponAbilityCast, this.m_onAbilityCast);
	}

	// Token: 0x04002138 RID: 8504
	private Action<MonoBehaviour, EventArgs> m_onAbilityCast;
}
