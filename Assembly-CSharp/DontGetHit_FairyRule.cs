using System;
using UnityEngine;

// Token: 0x020006F8 RID: 1784
public class DontGetHit_FairyRule : FairyRule
{
	// Token: 0x17001477 RID: 5239
	// (get) Token: 0x06003674 RID: 13940 RVA: 0x0001DF56 File Offset: 0x0001C156
	public override string Description
	{
		get
		{
			return "LOC_ID_FAIRY_RULE_TAKE_NO_DAMAGE_1";
		}
	}

	// Token: 0x17001478 RID: 5240
	// (get) Token: 0x06003675 RID: 13941 RVA: 0x00005315 File Offset: 0x00003515
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.DontGetHit;
		}
	}

	// Token: 0x17001479 RID: 5241
	// (get) Token: 0x06003676 RID: 13942 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool LockChestAtStart
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003677 RID: 13943 RVA: 0x0001DF5D File Offset: 0x0001C15D
	private void Awake()
	{
		this.m_onPlayerTakeDamage = new Action<MonoBehaviour, EventArgs>(this.OnPlayerTakeDamage);
	}

	// Token: 0x06003678 RID: 13944 RVA: 0x0001DF72 File Offset: 0x0001C172
	public override void RunRule(FairyRoomController fairyRoomController)
	{
		if (base.State != FairyRoomState.Failed)
		{
			base.State = FairyRoomState.Running;
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerHit, this.m_onPlayerTakeDamage);
		}
	}

	// Token: 0x06003679 RID: 13945 RVA: 0x0001DF92 File Offset: 0x0001C192
	public override void StopRule()
	{
		base.StopRule();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHit, this.m_onPlayerTakeDamage);
	}

	// Token: 0x0600367A RID: 13946 RVA: 0x0001DFA6 File Offset: 0x0001C1A6
	protected virtual void OnPlayerTakeDamage(object sender, EventArgs eventArgs)
	{
		base.SetIsFailed();
	}

	// Token: 0x0600367B RID: 13947 RVA: 0x0001DFAE File Offset: 0x0001C1AE
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHit, this.m_onPlayerTakeDamage);
	}

	// Token: 0x04002C31 RID: 11313
	private Action<MonoBehaviour, EventArgs> m_onPlayerTakeDamage;
}
