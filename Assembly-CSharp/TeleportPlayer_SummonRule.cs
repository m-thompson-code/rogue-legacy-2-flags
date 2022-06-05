using System;
using System.Collections;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x02000530 RID: 1328
[Serializable]
public class TeleportPlayer_SummonRule : BaseSummonRule
{
	// Token: 0x1700120F RID: 4623
	// (get) Token: 0x060030E7 RID: 12519 RVA: 0x000A65CB File Offset: 0x000A47CB
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.TeleportPlayer;
		}
	}

	// Token: 0x17001210 RID: 4624
	// (get) Token: 0x060030E8 RID: 12520 RVA: 0x000A65D2 File Offset: 0x000A47D2
	public override Color BoxColor
	{
		get
		{
			return Color.red;
		}
	}

	// Token: 0x17001211 RID: 4625
	// (get) Token: 0x060030E9 RID: 12521 RVA: 0x000A65D9 File Offset: 0x000A47D9
	public override string RuleLabel
	{
		get
		{
			return "Teleport Player";
		}
	}

	// Token: 0x060030EA RID: 12522 RVA: 0x000A65E0 File Offset: 0x000A47E0
	public override void Initialize(SummonRuleController summonController)
	{
		base.Initialize(summonController);
		this.m_waitUntilPlayerEnterRoomYield = new WaitUntil(() => PlayerManager.IsInstantiated && PlayerManager.GetCurrentPlayerRoom());
	}

	// Token: 0x060030EB RID: 12523 RVA: 0x000A6613 File Offset: 0x000A4813
	public override IEnumerator RunSummonRule()
	{
		yield return this.m_waitUntilPlayerEnterRoomYield;
		Tunnel tunnelToUse = null;
		TunnelSpawnController tunnelSpawnController = (base.SerializedObject != null) ? (base.SerializedObject as TunnelSpawnController) : null;
		if (tunnelSpawnController)
		{
			tunnelToUse = tunnelSpawnController.Tunnel;
		}
		if (tunnelToUse)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			if (!playerController.IsDead)
			{
				playerController.CastAbility.StopPersistentAbility(CastAbilityType.Weapon);
				playerController.CastAbility.StopPersistentAbility(CastAbilityType.Spell);
				playerController.CastAbility.StopPersistentAbility(CastAbilityType.Talent);
				base.SummonController.StopArena(true);
				base.IsRuleComplete = true;
				if (this.m_transitionID != TransitionID.None)
				{
					SceneLoader_RL.RunTransitionWithLogic(delegate()
					{
						this.TeleportPlayer(tunnelToUse);
					}, this.m_transitionID, false);
				}
				else
				{
					tunnelToUse.ForceEnterTunnel(false, null);
				}
			}
			else
			{
				Debug.Log("<color=yellow>Cannot execute TeleportPlayer Summon Rule. Player is dead.</color>");
			}
		}
		else
		{
			Debug.Log("<color=yellow>Cannot execute TeleportPlayer Summon Rule. No tunnels were assigned. Skipping rule...</color>");
		}
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x060030EC RID: 12524 RVA: 0x000A6622 File Offset: 0x000A4822
	private void TeleportPlayer(Tunnel tunnelToUse)
	{
		PlayerManager.GetPlayerController().CharacterHitResponse.StopInvincibleTime();
		tunnelToUse.ForceEnterTunnel(false, null);
	}

	// Token: 0x040026BE RID: 9918
	[SerializeField]
	private TransitionID m_transitionID = TransitionID.QuickSwipe;

	// Token: 0x040026BF RID: 9919
	private WaitUntil m_waitUntilPlayerEnterRoomYield;
}
