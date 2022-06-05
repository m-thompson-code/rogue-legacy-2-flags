using System;
using UnityEngine;

// Token: 0x02000707 RID: 1799
public class NoDoubleJump_FairyRule : FairyRule
{
	// Token: 0x17001499 RID: 5273
	// (get) Token: 0x060036EF RID: 14063 RVA: 0x0001E391 File Offset: 0x0001C591
	public override string Description
	{
		get
		{
			return "LOC_ID_FAIRY_RULE_NO_DOUBLE_JUMP_1";
		}
	}

	// Token: 0x1700149A RID: 5274
	// (get) Token: 0x060036F0 RID: 14064 RVA: 0x000081DE File Offset: 0x000063DE
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.NoDoubleJump;
		}
	}

	// Token: 0x1700149B RID: 5275
	// (get) Token: 0x060036F1 RID: 14065 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool LockChestAtStart
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060036F2 RID: 14066 RVA: 0x0001E398 File Offset: 0x0001C598
	private void Awake()
	{
		this.m_onPlayerDoubleJump = new Action<MonoBehaviour, EventArgs>(this.OnPlayerDoubleJump);
	}

	// Token: 0x060036F3 RID: 14067 RVA: 0x0001DFA6 File Offset: 0x0001C1A6
	private void OnPlayerDoubleJump(MonoBehaviour sender, EventArgs eventArgs)
	{
		base.SetIsFailed();
	}

	// Token: 0x060036F4 RID: 14068 RVA: 0x0001E3AC File Offset: 0x0001C5AC
	public override void RunRule(FairyRoomController fairyRoomController)
	{
		this.m_isListeningForDoubleJumpEvent = true;
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDoubleJump, this.m_onPlayerDoubleJump);
	}

	// Token: 0x060036F5 RID: 14069 RVA: 0x0001E3C2 File Offset: 0x0001C5C2
	public override void StopRule()
	{
		base.StopRule();
		if (this.m_isListeningForDoubleJumpEvent)
		{
			this.m_isListeningForDoubleJumpEvent = false;
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDoubleJump, this.m_onPlayerDoubleJump);
		}
	}

	// Token: 0x04002C6B RID: 11371
	private Action<MonoBehaviour, EventArgs> m_onPlayerDoubleJump;

	// Token: 0x04002C6C RID: 11372
	private bool m_isListeningForDoubleJumpEvent;
}
