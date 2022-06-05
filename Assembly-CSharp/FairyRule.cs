using System;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020006FF RID: 1791
public abstract class FairyRule : MonoBehaviour
{
	// Token: 0x17001486 RID: 5254
	// (get) Token: 0x060036B7 RID: 14007 RVA: 0x0001E19B File Offset: 0x0001C39B
	public IRelayLink<FairyRoomRuleStateChangeEventArgs> StateChangeRelay
	{
		get
		{
			return this.m_stateChangeRelay.link;
		}
	}

	// Token: 0x17001487 RID: 5255
	// (get) Token: 0x060036B8 RID: 14008 RVA: 0x0001E1A8 File Offset: 0x0001C3A8
	// (set) Token: 0x060036B9 RID: 14009 RVA: 0x0001E1B0 File Offset: 0x0001C3B0
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

	// Token: 0x17001488 RID: 5256
	// (get) Token: 0x060036BA RID: 14010
	public abstract string Description { get; }

	// Token: 0x17001489 RID: 5257
	// (get) Token: 0x060036BB RID: 14011
	public abstract bool LockChestAtStart { get; }

	// Token: 0x1700148A RID: 5258
	// (get) Token: 0x060036BC RID: 14012
	public abstract FairyRuleID ID { get; }

	// Token: 0x060036BD RID: 14013
	public abstract void RunRule(FairyRoomController fairyRoomController);

	// Token: 0x060036BE RID: 14014 RVA: 0x0001E1B9 File Offset: 0x0001C3B9
	public virtual void StopRule()
	{
		if (this.State != FairyRoomState.Failed && this.State != FairyRoomState.Passed)
		{
			this.State = FairyRoomState.NotRunning;
		}
	}

	// Token: 0x060036BF RID: 14015 RVA: 0x0001E1D6 File Offset: 0x0001C3D6
	public void SetIsPassed()
	{
		this.State = FairyRoomState.Passed;
		this.FireStateChangeEvent();
	}

	// Token: 0x060036C0 RID: 14016 RVA: 0x0001E1E6 File Offset: 0x0001C3E6
	public void SetIsFailed()
	{
		this.State = FairyRoomState.Failed;
		this.FireStateChangeEvent();
	}

	// Token: 0x060036C1 RID: 14017 RVA: 0x0001E1F6 File Offset: 0x0001C3F6
	public virtual void ResetRule()
	{
		this.State = FairyRoomState.NotRunning;
		this.FireStateChangeEvent();
	}

	// Token: 0x060036C2 RID: 14018 RVA: 0x0001E205 File Offset: 0x0001C405
	protected void FireStateChangeEvent()
	{
		FairyRule.m_fairyRuleChangeEventArgs_STATIC.Initialise(this);
		this.m_stateChangeRelay.Dispatch(FairyRule.m_fairyRuleChangeEventArgs_STATIC);
	}

	// Token: 0x04002C52 RID: 11346
	private FairyRoomState m_state;

	// Token: 0x04002C53 RID: 11347
	private static FairyRoomRuleStateChangeEventArgs m_fairyRuleChangeEventArgs_STATIC = new FairyRoomRuleStateChangeEventArgs(null);

	// Token: 0x04002C54 RID: 11348
	private Relay<FairyRoomRuleStateChangeEventArgs> m_stateChangeRelay = new Relay<FairyRoomRuleStateChangeEventArgs>();
}
