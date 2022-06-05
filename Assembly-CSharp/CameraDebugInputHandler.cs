using System;
using System.Collections;
using Rewired;
using RL_Windows;
using UnityEngine;

// Token: 0x020001E1 RID: 481
public class CameraDebugInputHandler : MonoBehaviour
{
	// Token: 0x060013FD RID: 5117 RVA: 0x0003CAA3 File Offset: 0x0003ACA3
	private IEnumerator Start()
	{
		while (Rewired_RL.Player == null)
		{
			yield return null;
		}
		Rewired_RL.Player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnJumpPressed), UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Jump");
		Rewired_RL.Player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnJumpPressed), UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Jump");
		Rewired_RL.Player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnSpellPressed), UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Spell");
		Rewired_RL.Player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnSpellPressed), UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Spell");
		yield break;
	}

	// Token: 0x17000A67 RID: 2663
	// (get) Token: 0x060013FE RID: 5118 RVA: 0x0003CAB2 File Offset: 0x0003ACB2
	// (set) Token: 0x060013FF RID: 5119 RVA: 0x0003CABA File Offset: 0x0003ACBA
	public bool JumpPressed
	{
		get
		{
			return this.m_jumpPressed;
		}
		private set
		{
			this.m_jumpPressed = value;
		}
	}

	// Token: 0x17000A68 RID: 2664
	// (get) Token: 0x06001400 RID: 5120 RVA: 0x0003CAC3 File Offset: 0x0003ACC3
	// (set) Token: 0x06001401 RID: 5121 RVA: 0x0003CACB File Offset: 0x0003ACCB
	public bool SpellPressed
	{
		get
		{
			return this.m_spellPressed;
		}
		private set
		{
			this.m_spellPressed = value;
		}
	}

	// Token: 0x06001402 RID: 5122 RVA: 0x0003CAD4 File Offset: 0x0003ACD4
	private void OnJumpPressed(InputActionEventData obj)
	{
		if (WindowManager.GetIsWindowOpen(WindowID.CameraDebug))
		{
			return;
		}
		if (obj.eventType == InputActionEventType.ButtonJustPressed)
		{
			this.JumpPressed = true;
		}
		else
		{
			this.JumpPressed = false;
		}
		if (this.JumpPressed && this.m_spellPressed)
		{
			WindowManager.SetWindowIsOpen(WindowID.CameraDebug, true);
			this.JumpPressed = false;
			this.SpellPressed = false;
			return;
		}
		WindowManager.SetWindowIsOpen(WindowID.CameraDebug, false);
	}

	// Token: 0x06001403 RID: 5123 RVA: 0x0003CB34 File Offset: 0x0003AD34
	private void OnSpellPressed(InputActionEventData obj)
	{
		if (WindowManager.GetIsWindowOpen(WindowID.CameraDebug))
		{
			return;
		}
		if (obj.eventType == InputActionEventType.ButtonJustPressed)
		{
			this.SpellPressed = true;
		}
		else
		{
			this.SpellPressed = false;
		}
		if (this.JumpPressed && this.SpellPressed)
		{
			WindowManager.SetWindowIsOpen(WindowID.CameraDebug, true);
			this.JumpPressed = false;
			this.SpellPressed = false;
			return;
		}
		WindowManager.SetWindowIsOpen(WindowID.CameraDebug, false);
	}

	// Token: 0x040013C9 RID: 5065
	private bool m_jumpPressed;

	// Token: 0x040013CA RID: 5066
	private bool m_spellPressed;
}
