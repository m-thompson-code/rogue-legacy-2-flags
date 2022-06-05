using System;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000437 RID: 1079
public abstract class FairyRule : MonoBehaviour
{
	// Token: 0x17000FA5 RID: 4005
	// (get) Token: 0x060027AB RID: 10155 RVA: 0x000840A7 File Offset: 0x000822A7
	public IRelayLink<FairyRoomRuleStateChangeEventArgs> StateChangeRelay
	{
		get
		{
			return this.m_stateChangeRelay.link;
		}
	}

	// Token: 0x17000FA6 RID: 4006
	// (get) Token: 0x060027AC RID: 10156 RVA: 0x000840B4 File Offset: 0x000822B4
	// (set) Token: 0x060027AD RID: 10157 RVA: 0x000840BC File Offset: 0x000822BC
	public FairyRoomState State
	{
		get
		{
			return this.m_state;
		}
		protected set
		{
			this.m_state = value;
		}
	}

	// Token: 0x17000FA7 RID: 4007
	// (get) Token: 0x060027AE RID: 10158
	public abstract string Description { get; }

	// Token: 0x17000FA8 RID: 4008
	// (get) Token: 0x060027AF RID: 10159
	public abstract bool LockChestAtStart { get; }

	// Token: 0x17000FA9 RID: 4009
	// (get) Token: 0x060027B0 RID: 10160
	public abstract FairyRuleID ID { get; }

	// Token: 0x060027B1 RID: 10161
	public abstract void RunRule(FairyRoomController fairyRoomController);

	// Token: 0x060027B2 RID: 10162 RVA: 0x000840C5 File Offset: 0x000822C5
	public virtual void StopRule()
	{
		if (this.State != FairyRoomState.Failed && this.State != FairyRoomState.Passed)
		{
			this.State = FairyRoomState.NotRunning;
		}
	}

	// Token: 0x060027B3 RID: 10163 RVA: 0x000840E2 File Offset: 0x000822E2
	public void SetIsPassed()
	{
		this.State = FairyRoomState.Passed;
		this.FireStateChangeEvent();
	}

	// Token: 0x060027B4 RID: 10164 RVA: 0x000840F2 File Offset: 0x000822F2
	public void SetIsFailed()
	{
		this.State = FairyRoomState.Failed;
		this.FireStateChangeEvent();
	}

	// Token: 0x060027B5 RID: 10165 RVA: 0x00084102 File Offset: 0x00082302
	public virtual void ResetRule()
	{
		this.State = FairyRoomState.NotRunning;
		this.FireStateChangeEvent();
	}

	// Token: 0x060027B6 RID: 10166 RVA: 0x00084111 File Offset: 0x00082311
	protected void FireStateChangeEvent()
	{
		FairyRule.m_fairyRuleChangeEventArgs_STATIC.Initialise(this);
		this.m_stateChangeRelay.Dispatch(FairyRule.m_fairyRuleChangeEventArgs_STATIC);
	}

	// Token: 0x04002125 RID: 8485
	private FairyRoomState m_state;

	// Token: 0x04002126 RID: 8486
	private static FairyRoomRuleStateChangeEventArgs m_fairyRuleChangeEventArgs_STATIC = new FairyRoomRuleStateChangeEventArgs(null);

	// Token: 0x04002127 RID: 8487
	private Relay<FairyRoomRuleStateChangeEventArgs> m_stateChangeRelay = new Relay<FairyRoomRuleStateChangeEventArgs>();
}
