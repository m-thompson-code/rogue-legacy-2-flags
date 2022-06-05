using System;
using UnityEngine;

// Token: 0x02000708 RID: 1800
public class NoJumping_FairyRule : FairyRule
{
	// Token: 0x1700149C RID: 5276
	// (get) Token: 0x060036F7 RID: 14071 RVA: 0x0001E3E6 File Offset: 0x0001C5E6
	public override string Description
	{
		get
		{
			return "LOC_ID_FAIRY_RULE_NO_JUMPING_1";
		}
	}

	// Token: 0x1700149D RID: 5277
	// (get) Token: 0x060036F8 RID: 14072 RVA: 0x000054AD File Offset: 0x000036AD
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.NoJumping;
		}
	}

	// Token: 0x1700149E RID: 5278
	// (get) Token: 0x060036F9 RID: 14073 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool LockChestAtStart
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060036FA RID: 14074 RVA: 0x0001E3ED File Offset: 0x0001C5ED
	private void Awake()
	{
		this.m_onPlayerJump = new Action<MonoBehaviour, EventArgs>(this.OnPlayerJump);
	}

	// Token: 0x060036FB RID: 14075 RVA: 0x0001E401 File Offset: 0x0001C601
	public override void RunRule(FairyRoomController fairyRoomController)
	{
		if (base.State != FairyRoomState.Failed)
		{
			base.State = FairyRoomState.Running;
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerJump, this.m_onPlayerJump);
		}
	}

	// Token: 0x060036FC RID: 14076 RVA: 0x0001E422 File Offset: 0x0001C622
	public override void StopRule()
	{
		base.StopRule();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerJump, this.m_onPlayerJump);
	}

	// Token: 0x060036FD RID: 14077 RVA: 0x0001DFA6 File Offset: 0x0001C1A6
	private void OnPlayerJump(object sender, EventArgs eventArgs)
	{
		base.SetIsFailed();
	}

	// Token: 0x04002C6D RID: 11373
	private Action<MonoBehaviour, EventArgs> m_onPlayerJump;
}
