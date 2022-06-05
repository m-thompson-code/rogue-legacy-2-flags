using System;

// Token: 0x02000706 RID: 1798
public class NoDash_FairyRule : FairyRule
{
	// Token: 0x17001496 RID: 5270
	// (get) Token: 0x060036E7 RID: 14055 RVA: 0x0001E335 File Offset: 0x0001C535
	public override string Description
	{
		get
		{
			return "LOC_ID_FAIRY_RULE_NO_DASHING_1";
		}
	}

	// Token: 0x17001497 RID: 5271
	// (get) Token: 0x060036E8 RID: 14056 RVA: 0x000065B4 File Offset: 0x000047B4
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.NoDash;
		}
	}

	// Token: 0x17001498 RID: 5272
	// (get) Token: 0x060036E9 RID: 14057 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool LockChestAtStart
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060036EA RID: 14058 RVA: 0x0001E33C File Offset: 0x0001C53C
	private void Awake()
	{
		this.m_onPlayerDash = new Action(this.OnPlayerDash);
	}

	// Token: 0x060036EB RID: 14059 RVA: 0x0001DFA6 File Offset: 0x0001C1A6
	private void OnPlayerDash()
	{
		base.SetIsFailed();
	}

	// Token: 0x060036EC RID: 14060 RVA: 0x0001E350 File Offset: 0x0001C550
	public override void RunRule(FairyRoomController fairyRoomController)
	{
		PlayerManager.GetPlayerController().CharacterDash.DashRelay.AddListener(this.m_onPlayerDash, false);
	}

	// Token: 0x060036ED RID: 14061 RVA: 0x0001E36E File Offset: 0x0001C56E
	public override void StopRule()
	{
		base.StopRule();
		PlayerManager.GetPlayerController().CharacterDash.DashRelay.RemoveListener(this.m_onPlayerDash);
	}

	// Token: 0x04002C6A RID: 11370
	private Action m_onPlayerDash;
}
