using System;
using UnityEngine;

// Token: 0x02000431 RID: 1073
public class DontGetHit_FairyRule : FairyRule
{
	// Token: 0x17000F96 RID: 3990
	// (get) Token: 0x0600276B RID: 10091 RVA: 0x00083227 File Offset: 0x00081427
	public override string Description
	{
		get
		{
			return "LOC_ID_FAIRY_RULE_TAKE_NO_DAMAGE_1";
		}
	}

	// Token: 0x17000F97 RID: 3991
	// (get) Token: 0x0600276C RID: 10092 RVA: 0x0008322E File Offset: 0x0008142E
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.DontGetHit;
		}
	}

	// Token: 0x17000F98 RID: 3992
	// (get) Token: 0x0600276D RID: 10093 RVA: 0x00083232 File Offset: 0x00081432
	public override bool LockChestAtStart
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600276E RID: 10094 RVA: 0x00083235 File Offset: 0x00081435
	private void Awake()
	{
		this.m_onPlayerTakeDamage = new Action<MonoBehaviour, EventArgs>(this.OnPlayerTakeDamage);
	}

	// Token: 0x0600276F RID: 10095 RVA: 0x0008324A File Offset: 0x0008144A
	public override void RunRule(FairyRoomController fairyRoomController)
	{
		if (base.State != FairyRoomState.Failed)
		{
			base.State = FairyRoomState.Running;
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerHit, this.m_onPlayerTakeDamage);
		}
	}

	// Token: 0x06002770 RID: 10096 RVA: 0x0008326A File Offset: 0x0008146A
	public override void StopRule()
	{
		base.StopRule();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHit, this.m_onPlayerTakeDamage);
	}

	// Token: 0x06002771 RID: 10097 RVA: 0x0008327E File Offset: 0x0008147E
	protected virtual void OnPlayerTakeDamage(object sender, EventArgs eventArgs)
	{
		base.SetIsFailed();
	}

	// Token: 0x06002772 RID: 10098 RVA: 0x00083286 File Offset: 0x00081486
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHit, this.m_onPlayerTakeDamage);
	}

	// Token: 0x04002106 RID: 8454
	private Action<MonoBehaviour, EventArgs> m_onPlayerTakeDamage;
}
