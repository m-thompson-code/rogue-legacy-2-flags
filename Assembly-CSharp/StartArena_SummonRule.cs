using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200052C RID: 1324
[Serializable]
public class StartArena_SummonRule : BaseSummonRule
{
	// Token: 0x17001205 RID: 4613
	// (get) Token: 0x060030C1 RID: 12481 RVA: 0x000A5FFA File Offset: 0x000A41FA
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.StartArena;
		}
	}

	// Token: 0x17001206 RID: 4614
	// (get) Token: 0x060030C2 RID: 12482 RVA: 0x000A6001 File Offset: 0x000A4201
	public override string RuleLabel
	{
		get
		{
			return "Start Arena";
		}
	}

	// Token: 0x060030C3 RID: 12483 RVA: 0x000A6008 File Offset: 0x000A4208
	public override void Initialize(SummonRuleController summonController)
	{
		base.Initialize(summonController);
		this.m_waitUntilPlayerEnterRoomYield = new WaitUntil(() => PlayerManager.IsInstantiated && PlayerManager.GetCurrentPlayerRoom() != null);
	}

	// Token: 0x060030C4 RID: 12484 RVA: 0x000A603B File Offset: 0x000A423B
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

	// Token: 0x040026A3 RID: 9891
	[SerializeField]
	private bool m_triggerWithInteractAction;

	// Token: 0x040026A4 RID: 9892
	private WaitUntil m_waitUntilPlayerEnterRoomYield;
}
