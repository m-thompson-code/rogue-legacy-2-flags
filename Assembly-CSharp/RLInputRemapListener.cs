using System;
using Rewired;
using UnityEngine;

// Token: 0x020002B4 RID: 692
public class RLInputRemapListener : MonoBehaviour
{
	// Token: 0x17000C7B RID: 3195
	// (get) Token: 0x06001B83 RID: 7043 RVA: 0x000584FB File Offset: 0x000566FB
	public bool UsesAxisContribution
	{
		get
		{
			return this.m_useAxisContribution;
		}
	}

	// Token: 0x17000C7C RID: 3196
	// (get) Token: 0x06001B84 RID: 7044 RVA: 0x00058503 File Offset: 0x00056703
	public Pole Axis
	{
		get
		{
			if (this.UsesAxisContribution)
			{
				return this.m_axisContribution;
			}
			return Pole.Positive;
		}
	}

	// Token: 0x17000C7D RID: 3197
	// (get) Token: 0x06001B85 RID: 7045 RVA: 0x00058515 File Offset: 0x00056715
	public bool UseWindowInputActions
	{
		get
		{
			return this.m_useWindowInputActions;
		}
	}

	// Token: 0x17000C7E RID: 3198
	// (get) Token: 0x06001B86 RID: 7046 RVA: 0x0005851D File Offset: 0x0005671D
	public Rewired_RL.WindowInputActionType WindowInputActionType
	{
		get
		{
			return this.m_windowInputActionType;
		}
	}

	// Token: 0x17000C7F RID: 3199
	// (get) Token: 0x06001B87 RID: 7047 RVA: 0x00058525 File Offset: 0x00056725
	public Rewired_RL.InputActionType InputActionType
	{
		get
		{
			return this.m_inputActionType;
		}
	}

	// Token: 0x17000C80 RID: 3200
	// (get) Token: 0x06001B88 RID: 7048 RVA: 0x0005852D File Offset: 0x0005672D
	public bool UsesGamepad
	{
		get
		{
			return this.m_useGamepad;
		}
	}

	// Token: 0x06001B89 RID: 7049 RVA: 0x00058538 File Offset: 0x00056738
	public void ListenForRemapInput()
	{
		if (!this.UseWindowInputActions)
		{
			if (this.m_useAxisContribution)
			{
				RLInputRemapper.ChangeInputRequested(this.InputActionType, this.UsesGamepad, this.m_axisContribution);
				return;
			}
			RLInputRemapper.ChangeInputRequested(this.InputActionType, this.UsesGamepad, Pole.Positive);
			return;
		}
		else
		{
			if (this.m_useAxisContribution)
			{
				RLInputRemapper.ChangeInputRequested(this.WindowInputActionType, this.UsesGamepad, this.m_axisContribution);
				return;
			}
			RLInputRemapper.ChangeInputRequested(this.WindowInputActionType, this.UsesGamepad, Pole.Positive);
			return;
		}
	}

	// Token: 0x04001930 RID: 6448
	[SerializeField]
	private bool m_useWindowInputActions;

	// Token: 0x04001931 RID: 6449
	[SerializeField]
	[ConditionalHide("m_useWindowInputActions", Inverse = true, HideInInspector = true)]
	private Rewired_RL.InputActionType m_inputActionType;

	// Token: 0x04001932 RID: 6450
	[SerializeField]
	[ConditionalHide("m_useWindowInputActions", HideInInspector = true)]
	private Rewired_RL.WindowInputActionType m_windowInputActionType;

	// Token: 0x04001933 RID: 6451
	[SerializeField]
	private bool m_useAxisContribution;

	// Token: 0x04001934 RID: 6452
	[SerializeField]
	private bool m_useGamepad;

	// Token: 0x04001935 RID: 6453
	[SerializeField]
	[ConditionalHide("m_useAxisContribution", true)]
	private Pole m_axisContribution;
}
