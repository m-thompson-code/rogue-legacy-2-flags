using System;
using Sigtrap.Relays;

// Token: 0x020001AF RID: 431
public class AbilityAnimBehaviourManager
{
	// Token: 0x17000987 RID: 2439
	// (get) Token: 0x0600111C RID: 4380 RVA: 0x00031655 File Offset: 0x0002F855
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

	// Token: 0x17000988 RID: 2440
	// (get) Token: 0x0600111D RID: 4381 RVA: 0x00031677 File Offset: 0x0002F877
	public static IRelayLink<AbilityAnimState> OnAnimStateEnterRelay
	{
		get
		{
			return AbilityAnimBehaviourManager.Instance.m_onAnimStateEnterRelay.link;
		}
	}

	// Token: 0x17000989 RID: 2441
	// (get) Token: 0x0600111E RID: 4382 RVA: 0x00031688 File Offset: 0x0002F888
	public static IRelayLink<AbilityAnimState> OnAnimStateExitRelay
	{
		get
		{
			return AbilityAnimBehaviourManager.Instance.m_onAnimStateExitRelay.link;
		}
	}

	// Token: 0x1700098A RID: 2442
	// (get) Token: 0x0600111F RID: 4383 RVA: 0x00031699 File Offset: 0x0002F899
	// (set) Token: 0x06001120 RID: 4384 RVA: 0x000316A5 File Offset: 0x0002F8A5
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

	// Token: 0x1700098B RID: 2443
	// (get) Token: 0x06001121 RID: 4385 RVA: 0x000316B2 File Offset: 0x0002F8B2
	// (set) Token: 0x06001122 RID: 4386 RVA: 0x000316BE File Offset: 0x0002F8BE
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

	// Token: 0x06001123 RID: 4387 RVA: 0x000316CB File Offset: 0x0002F8CB
	public static void InvokeAnimOnEnter(AbilityAnimState animState)
	{
		AbilityAnimBehaviourManager.Instance.m_onAnimStateEnterRelay.Dispatch(animState);
	}

	// Token: 0x06001124 RID: 4388 RVA: 0x000316DD File Offset: 0x0002F8DD
	public static void InvokeAnimOnExit(AbilityAnimState animState)
	{
		AbilityAnimBehaviourManager.Instance.m_onAnimStateExitRelay.Dispatch(animState);
	}

	// Token: 0x06001125 RID: 4389 RVA: 0x000316EF File Offset: 0x0002F8EF
	private void Initialize()
	{
	}

	// Token: 0x06001126 RID: 4390 RVA: 0x000316F1 File Offset: 0x0002F8F1
	public static void ResetManager()
	{
		AbilityAnimBehaviourManager.CurrentAnimatorState = null;
		AbilityAnimBehaviourManager.AnimationComplete = false;
	}

	// Token: 0x04001203 RID: 4611
	private AbilityAnimBehaviour m_currentAnimatorState;

	// Token: 0x04001204 RID: 4612
	private bool m_animationComplete;

	// Token: 0x04001205 RID: 4613
	private Relay<AbilityAnimState> m_onAnimStateEnterRelay = new Relay<AbilityAnimState>();

	// Token: 0x04001206 RID: 4614
	private Relay<AbilityAnimState> m_onAnimStateExitRelay = new Relay<AbilityAnimState>();

	// Token: 0x04001207 RID: 4615
	private static AbilityAnimBehaviourManager m_instance;
}
