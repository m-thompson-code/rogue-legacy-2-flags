using System;
using UnityEngine;

// Token: 0x0200043E RID: 1086
public class NoDoubleJump_FairyRule : FairyRule
{
	// Token: 0x17000FB6 RID: 4022
	// (get) Token: 0x060027DD RID: 10205 RVA: 0x00084634 File Offset: 0x00082834
	public override string Description
	{
		get
		{
			return "LOC_ID_FAIRY_RULE_NO_DOUBLE_JUMP_1";
		}
	}

	// Token: 0x17000FB7 RID: 4023
	// (get) Token: 0x060027DE RID: 10206 RVA: 0x0008463B File Offset: 0x0008283B
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.NoDoubleJump;
		}
	}

	// Token: 0x17000FB8 RID: 4024
	// (get) Token: 0x060027DF RID: 10207 RVA: 0x0008463F File Offset: 0x0008283F
	public override bool LockChestAtStart
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060027E0 RID: 10208 RVA: 0x00084642 File Offset: 0x00082842
	private void Awake()
	{
		this.m_onPlayerDoubleJump = new Action<MonoBehaviour, EventArgs>(this.OnPlayerDoubleJump);
	}

	// Token: 0x060027E1 RID: 10209 RVA: 0x00084656 File Offset: 0x00082856
	private void OnPlayerDoubleJump(MonoBehaviour sender, EventArgs eventArgs)
	{
		base.SetIsFailed();
	}

	// Token: 0x060027E2 RID: 10210 RVA: 0x0008465E File Offset: 0x0008285E
	public override void RunRule(FairyRoomController fairyRoomController)
	{
		this.m_isListeningForDoubleJumpEvent = true;
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDoubleJump, this.m_onPlayerDoubleJump);
	}

	// Token: 0x060027E3 RID: 10211 RVA: 0x00084674 File Offset: 0x00082874
	public override void StopRule()
	{
		base.StopRule();
		if (this.m_isListeningForDoubleJumpEvent)
		{
			this.m_isListeningForDoubleJumpEvent = false;
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDoubleJump, this.m_onPlayerDoubleJump);
		}
	}

	// Token: 0x0400213A RID: 8506
	private Action<MonoBehaviour, EventArgs> m_onPlayerDoubleJump;

	// Token: 0x0400213B RID: 8507
	private bool m_isListeningForDoubleJumpEvent;
}
