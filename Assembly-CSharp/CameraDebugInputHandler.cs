using System;
using System.Collections;
using Rewired;
using RL_Windows;
using UnityEngine;

// Token: 0x02000377 RID: 887
public class CameraDebugInputHandler : MonoBehaviour
{
	// Token: 0x06001D08 RID: 7432 RVA: 0x0000EFA5 File Offset: 0x0000D1A5
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

	// Token: 0x17000D63 RID: 3427
	// (get) Token: 0x06001D09 RID: 7433 RVA: 0x0000EFB4 File Offset: 0x0000D1B4
	// (set) Token: 0x06001D0A RID: 7434 RVA: 0x0000EFBC File Offset: 0x0000D1BC
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

	// Token: 0x17000D64 RID: 3428
	// (get) Token: 0x06001D0B RID: 7435 RVA: 0x0000EFC5 File Offset: 0x0000D1C5
	// (set) Token: 0x06001D0C RID: 7436 RVA: 0x0000EFCD File Offset: 0x0000D1CD
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

	// Token: 0x06001D0D RID: 7437 RVA: 0x0009B8D8 File Offset: 0x00099AD8
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

	// Token: 0x06001D0E RID: 7438 RVA: 0x0009B938 File Offset: 0x00099B38
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

	// Token: 0x04001A63 RID: 6755
	private bool m_jumpPressed;

	// Token: 0x04001A64 RID: 6756
	private bool m_spellPressed;
}
