using System;
using UnityEngine;

// Token: 0x02000884 RID: 2180
public class TowerBossEntranceRoomController : BossEntranceRoomController
{
	// Token: 0x060042DB RID: 17115 RVA: 0x0010BE68 File Offset: 0x0010A068
	public override void SetBossTunnelState(BossTunnelState state, bool skipToIdleState)
	{
		Animator animator = base.BossTunnelSpawner.Tunnel.Animator;
		Animator animator2 = this.m_sunPropSpawner.PropInstance.Animators[0];
		(base.BossTunnelSpawner.Tunnel as BossTunnel).TunnelState = state;
		string text = null;
		string text2 = null;
		switch (state)
		{
		case BossTunnelState.Closed:
			if (!skipToIdleState)
			{
				text = "Disable";
				text2 = "TurnYellow";
			}
			else
			{
				text = "IdleDisabled";
				text2 = "IdleYellow";
			}
			base.BossTunnelSpawner.Tunnel.SetIsLocked(true);
			base.BossTunnelSpawner.Tunnel.Interactable.SetIsInteractableActive(true);
			break;
		case BossTunnelState.PartlyOpen:
			this.m_doorPartlyOpenedRelay.Dispatch(skipToIdleState);
			text = "IdleActive";
			text2 = "IdleRed";
			base.BossTunnelSpawner.Tunnel.SetIsLocked(false);
			break;
		case BossTunnelState.Open:
			this.m_doorOpenedRelay.Dispatch();
			text = "IdleActive";
			text2 = "TurnRedInstant";
			base.BossTunnelSpawner.Tunnel.SetIsLocked(false);
			break;
		case BossTunnelState.Destroyed:
			this.m_doorDestroyedRelay.Dispatch(skipToIdleState);
			if (!skipToIdleState)
			{
				text = "Disable";
				text2 = "TurnYellow";
			}
			else
			{
				text = "IdleDisabled";
				text2 = "IdleYellow";
			}
			base.BossTunnelSpawner.Tunnel.SetIsLocked(true);
			break;
		}
		if (!string.IsNullOrEmpty(text))
		{
			animator.Play(text);
		}
		if (!string.IsNullOrEmpty(text2))
		{
			animator2.Play(text2);
		}
	}

	// Token: 0x04003430 RID: 13360
	[SerializeField]
	private PropSpawnController m_sunPropSpawner;
}
