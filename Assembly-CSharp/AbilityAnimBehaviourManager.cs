using System;
using Sigtrap.Relays;

// Token: 0x0200031E RID: 798
public class AbilityAnimBehaviourManager
{
	// Token: 0x17000C47 RID: 3143
	// (get) Token: 0x06001965 RID: 6501 RVA: 0x0000CC96 File Offset: 0x0000AE96
	private static AbilityAnimBehaviourManager Instance
	{
		get
		{
			if (AbilityAnimBehaviourManager.m_instance == null)
			{
				AbilityAnimBehaviourManager.m_instance = new AbilityAnimBehaviourManager();
				AbilityAnimBehaviourManager.m_instance.Initialize();
			}
			return AbilityAnimBehaviourManager.m_instance;
		}
	}

	// Token: 0x17000C48 RID: 3144
	// (get) Token: 0x06001966 RID: 6502 RVA: 0x0000CCB8 File Offset: 0x0000AEB8
	public static IRelayLink<AbilityAnimState> OnAnimStateEnterRelay
	{
		get
		{
			return AbilityAnimBehaviourManager.Instance.m_onAnimStateEnterRelay.link;
		}
	}

	// Token: 0x17000C49 RID: 3145
	// (get) Token: 0x06001967 RID: 6503 RVA: 0x0000CCC9 File Offset: 0x0000AEC9
	public static IRelayLink<AbilityAnimState> OnAnimStateExitRelay
	{
		get
		{
			return AbilityAnimBehaviourManager.Instance.m_onAnimStateExitRelay.link;
		}
	}

	// Token: 0x17000C4A RID: 3146
	// (get) Token: 0x06001968 RID: 6504 RVA: 0x0000CCDA File Offset: 0x0000AEDA
	// (set) Token: 0x06001969 RID: 6505 RVA: 0x0000CCE6 File Offset: 0x0000AEE6
	public static AbilityAnimBehaviour CurrentAnimatorState
	{
		get
		{
			return AbilityAnimBehaviourManager.Instance.m_currentAnimatorState;
		}
		set
		{
			AbilityAnimBehaviourManager.Instance.m_currentAnimatorState = value;
		}
	}

	// Token: 0x17000C4B RID: 3147
	// (get) Token: 0x0600196A RID: 6506 RVA: 0x0000CCF3 File Offset: 0x0000AEF3
	// (set) Token: 0x0600196B RID: 6507 RVA: 0x0000CCFF File Offset: 0x0000AEFF
	public static bool AnimationComplete
	{
		get
		{
			return AbilityAnimBehaviourManager.Instance.m_animationComplete;
		}
		set
		{
			AbilityAnimBehaviourManager.Instance.m_animationComplete = value;
		}
	}

	// Token: 0x0600196C RID: 6508 RVA: 0x0000CD0C File Offset: 0x0000AF0C
	public static void InvokeAnimOnEnter(AbilityAnimState animState)
	{
		AbilityAnimBehaviourManager.Instance.m_onAnimStateEnterRelay.Dispatch(animState);
	}

	// Token: 0x0600196D RID: 6509 RVA: 0x0000CD1E File Offset: 0x0000AF1E
	public static void InvokeAnimOnExit(AbilityAnimState animState)
	{
		AbilityAnimBehaviourManager.Instance.m_onAnimStateExitRelay.Dispatch(animState);
	}

	// Token: 0x0600196E RID: 6510 RVA: 0x00002FCA File Offset: 0x000011CA
	private void Initialize()
	{
	}

	// Token: 0x0600196F RID: 6511 RVA: 0x0000CD30 File Offset: 0x0000AF30
	public static void ResetManager()
	{
		AbilityAnimBehaviourManager.CurrentAnimatorState = null;
		AbilityAnimBehaviourManager.AnimationComplete = false;
	}

	// Token: 0x0400180C RID: 6156
	private AbilityAnimBehaviour m_currentAnimatorState;

	// Token: 0x0400180D RID: 6157
	private bool m_animationComplete;

	// Token: 0x0400180E RID: 6158
	private Relay<AbilityAnimState> m_onAnimStateEnterRelay = new Relay<AbilityAnimState>();

	// Token: 0x0400180F RID: 6159
	private Relay<AbilityAnimState> m_onAnimStateExitRelay = new Relay<AbilityAnimState>();

	// Token: 0x04001810 RID: 6160
	private static AbilityAnimBehaviourManager m_instance;
}
