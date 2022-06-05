using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008B1 RID: 2225
[Serializable]
public class StartArena_SummonRule : BaseSummonRule
{
	// Token: 0x1700183C RID: 6204
	// (get) Token: 0x060043E3 RID: 17379 RVA: 0x000256FC File Offset: 0x000238FC
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.StartArena;
		}
	}

	// Token: 0x1700183D RID: 6205
	// (get) Token: 0x060043E4 RID: 17380 RVA: 0x00025703 File Offset: 0x00023903
	public override string RuleLabel
	{
		get
		{
			return "Start Arena";
		}
	}

	// Token: 0x060043E5 RID: 17381 RVA: 0x0002570A File Offset: 0x0002390A
	public override void Initialize(SummonRuleController summonController)
	{
		base.Initialize(summonController);
		this.m_waitUntilPlayerEnterRoomYield = new WaitUntil(() => PlayerManager.IsInstantiated && PlayerManager.GetCurrentPlayerRoom() != null);
	}

	// Token: 0x060043E6 RID: 17382 RVA: 0x0002573D File Offset: 0x0002393D
	public override IEnumerator RunSummonRule()
	{
		BoxCollider2D collider = (base.SerializedObject != null) ? (base.SerializedObject as BoxCollider2D) : null;
		if (collider)
		{
			yield return this.m_waitUntilPlayerEnterRoomYield;
			PlayerController playerController = PlayerManager.GetPlayerController();
			bool flag = collider.bounds.Contains(playerController.Midpoint);
			while (!flag || (!Rewired_RL.Player.GetButtonDown("Interact") && this.m_triggerWithInteractAction))
			{
				if (this.m_triggerWithInteractAction)
				{
					if (flag)
					{
						if (!playerController.IsInteractIconVisible)
						{
							playerController.IsInteractIconVisible = true;
						}
					}
					else if (playerController.IsInteractIconVisible)
					{
						playerController.IsInteractIconVisible = false;
					}
				}
				yield return null;
				flag = collider.bounds.Contains(playerController.Midpoint);
			}
			playerController.IsInteractIconVisible = false;
			playerController = null;
		}
		base.SummonController.StartArena();
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x040034C3 RID: 13507
	[SerializeField]
	private bool m_triggerWithInteractAction;

	// Token: 0x040034C4 RID: 13508
	private WaitUntil m_waitUntilPlayerEnterRoomYield;
}
