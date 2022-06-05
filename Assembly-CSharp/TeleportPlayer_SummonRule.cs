using System;
using System.Collections;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x020008B9 RID: 2233
[Serializable]
public class TeleportPlayer_SummonRule : BaseSummonRule
{
	// Token: 0x1700184C RID: 6220
	// (get) Token: 0x0600441E RID: 17438 RVA: 0x00019212 File Offset: 0x00017412
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.TeleportPlayer;
		}
	}

	// Token: 0x1700184D RID: 6221
	// (get) Token: 0x0600441F RID: 17439 RVA: 0x00025383 File Offset: 0x00023583
	public override Color BoxColor
	{
		get
		{
			return Color.red;
		}
	}

	// Token: 0x1700184E RID: 6222
	// (get) Token: 0x06004420 RID: 17440 RVA: 0x0002590B File Offset: 0x00023B0B
	public override string RuleLabel
	{
		get
		{
			return "Teleport Player";
		}
	}

	// Token: 0x06004421 RID: 17441 RVA: 0x00025912 File Offset: 0x00023B12
	public override void Initialize(SummonRuleController summonController)
	{
		base.Initialize(summonController);
		this.m_waitUntilPlayerEnterRoomYield = new WaitUntil(() => PlayerManager.IsInstantiated && PlayerManager.GetCurrentPlayerRoom());
	}

	// Token: 0x06004422 RID: 17442 RVA: 0x00025945 File Offset: 0x00023B45
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

	// Token: 0x06004423 RID: 17443 RVA: 0x00025954 File Offset: 0x00023B54
	private void TeleportPlayer(Tunnel tunnelToUse)
	{
		PlayerManager.GetPlayerController().CharacterHitResponse.StopInvincibleTime();
		tunnelToUse.ForceEnterTunnel(false, null);
	}

	// Token: 0x040034F2 RID: 13554
	[SerializeField]
	private TransitionID m_transitionID = TransitionID.QuickSwipe;

	// Token: 0x040034F3 RID: 13555
	private WaitUntil m_waitUntilPlayerEnterRoomYield;
}
