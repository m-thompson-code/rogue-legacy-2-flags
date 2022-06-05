using System;
using Rewired;
using UnityEngine;

// Token: 0x020004A1 RID: 1185
public class RLInputRemapListener : MonoBehaviour
{
	// Token: 0x17000FFA RID: 4090
	// (get) Token: 0x0600262B RID: 9771 RVA: 0x00015329 File Offset: 0x00013529
	public bool UsesAxisContribution
	{
		get
		{
			return this.m_useAxisContribution;
		}
	}

	// Token: 0x17000FFB RID: 4091
	// (get) Token: 0x0600262C RID: 9772 RVA: 0x00015331 File Offset: 0x00013531
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

	// Token: 0x17000FFC RID: 4092
	// (get) Token: 0x0600262D RID: 9773 RVA: 0x00015343 File Offset: 0x00013543
	public bool UseWindowInputActions
	{
		get
		{
			return this.m_useWindowInputActions;
		}
	}

	// Token: 0x17000FFD RID: 4093
	// (get) Token: 0x0600262E RID: 9774 RVA: 0x0001534B File Offset: 0x0001354B
	public Rewired_RL.WindowInputActionType WindowInputActionType
	{
		get
		{
			return this.m_windowInputActionType;
		}
	}

	// Token: 0x17000FFE RID: 4094
	// (get) Token: 0x0600262F RID: 9775 RVA: 0x00015353 File Offset: 0x00013553
	public Rewired_RL.InputActionType InputActionType
	{
		get
		{
			return this.m_inputActionType;
		}
	}

	// Token: 0x17000FFF RID: 4095
	// (get) Token: 0x06002630 RID: 9776 RVA: 0x0001535B File Offset: 0x0001355B
	public bool UsesGamepad
	{
		get
		{
			return this.m_useGamepad;
		}
	}

	// Token: 0x06002631 RID: 9777 RVA: 0x000B5424 File Offset: 0x000B3624
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

	// Token: 0x04002116 RID: 8470
	[SerializeField]
	private bool m_useWindowInputActions;

	// Token: 0x04002117 RID: 8471
	[SerializeField]
	[ConditionalHide("m_useWindowInputActions", Inverse = true, HideInInspector = true)]
	private Rewired_RL.InputActionType m_inputActionType;

	// Token: 0x04002118 RID: 8472
	[SerializeField]
	[ConditionalHide("m_useWindowInputActions", HideInInspector = true)]
	private Rewired_RL.WindowInputActionType m_windowInputActionType;

	// Token: 0x04002119 RID: 8473
	[SerializeField]
	private bool m_useAxisContribution;

	// Token: 0x0400211A RID: 8474
	[SerializeField]
	private bool m_useGamepad;

	// Token: 0x0400211B RID: 8475
	[SerializeField]
	[ConditionalHide("m_useAxisContribution", true)]
	private Pole m_axisContribution;
}
