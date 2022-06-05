using System;

// Token: 0x0200043D RID: 1085
public class NoDash_FairyRule : FairyRule
{
	// Token: 0x17000FB3 RID: 4019
	// (get) Token: 0x060027D5 RID: 10197 RVA: 0x000845C1 File Offset: 0x000827C1
	public override string Description
	{
		get
		{
			return "LOC_ID_FAIRY_RULE_NO_DASHING_1";
		}
	}

	// Token: 0x17000FB4 RID: 4020
	// (get) Token: 0x060027D6 RID: 10198 RVA: 0x000845C8 File Offset: 0x000827C8
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.NoDash;
		}
	}

	// Token: 0x17000FB5 RID: 4021
	// (get) Token: 0x060027D7 RID: 10199 RVA: 0x000845CC File Offset: 0x000827CC
	public override bool LockChestAtStart
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060027D8 RID: 10200 RVA: 0x000845CF File Offset: 0x000827CF
	private void Awake()
	{
		this.m_onPlayerDash = new Action(this.OnPlayerDash);
	}

	// Token: 0x060027D9 RID: 10201 RVA: 0x000845E3 File Offset: 0x000827E3
	private void OnPlayerDash()
	{
		base.SetIsFailed();
	}

	// Token: 0x060027DA RID: 10202 RVA: 0x000845EB File Offset: 0x000827EB
	public override void RunRule(FairyRoomController fairyRoomController)
	{
		PlayerManager.GetPlayerController().CharacterDash.DashRelay.AddListener(this.m_onPlayerDash, false);
	}

	// Token: 0x060027DB RID: 10203 RVA: 0x00084609 File Offset: 0x00082809
	public override void StopRule()
	{
		base.StopRule();
		PlayerManager.GetPlayerController().CharacterDash.DashRelay.RemoveListener(this.m_onPlayerDash);
	}

	// Token: 0x04002139 RID: 8505
	private Action m_onPlayerDash;
}
