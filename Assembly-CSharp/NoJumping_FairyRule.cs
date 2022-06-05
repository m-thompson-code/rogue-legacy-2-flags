using System;
using UnityEngine;

// Token: 0x0200043F RID: 1087
public class NoJumping_FairyRule : FairyRule
{
	// Token: 0x17000FB9 RID: 4025
	// (get) Token: 0x060027E5 RID: 10213 RVA: 0x000846A0 File Offset: 0x000828A0
	public override string Description
	{
		get
		{
			return "LOC_ID_FAIRY_RULE_NO_JUMPING_1";
		}
	}

	// Token: 0x17000FBA RID: 4026
	// (get) Token: 0x060027E6 RID: 10214 RVA: 0x000846A7 File Offset: 0x000828A7
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.NoJumping;
		}
	}

	// Token: 0x17000FBB RID: 4027
	// (get) Token: 0x060027E7 RID: 10215 RVA: 0x000846AB File Offset: 0x000828AB
	public override bool LockChestAtStart
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060027E8 RID: 10216 RVA: 0x000846AE File Offset: 0x000828AE
	private void Awake()
	{
		this.m_onPlayerJump = new Action<MonoBehaviour, EventArgs>(this.OnPlayerJump);
	}

	// Token: 0x060027E9 RID: 10217 RVA: 0x000846C2 File Offset: 0x000828C2
	public override void RunRule(FairyRoomController fairyRoomController)
	{
		if (base.State != FairyRoomState.Failed)
		{
			base.State = FairyRoomState.Running;
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerJump, this.m_onPlayerJump);
		}
	}

	// Token: 0x060027EA RID: 10218 RVA: 0x000846E3 File Offset: 0x000828E3
	public override void StopRule()
	{
		base.StopRule();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerJump, this.m_onPlayerJump);
	}

	// Token: 0x060027EB RID: 10219 RVA: 0x000846F8 File Offset: 0x000828F8
	private void OnPlayerJump(object sender, EventArgs eventArgs)
	{
		base.SetIsFailed();
	}

	// Token: 0x0400213C RID: 8508
	private Action<MonoBehaviour, EventArgs> m_onPlayerJump;
}
