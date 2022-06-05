using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using SceneManagement_RL;
using UnityEngine;

namespace RL_Windows
{
	// Token: 0x020008B7 RID: 2231
	public class DisclaimerWindowController : WindowController
	{
		// Token: 0x170017D3 RID: 6099
		// (get) Token: 0x060048B4 RID: 18612 RVA: 0x00104EC9 File Offset: 0x001030C9
		public override WindowID ID
		{
			get
			{
				return WindowID.Disclaimer;
			}
		}

		// Token: 0x060048B5 RID: 18613 RVA: 0x00104ECD File Offset: 0x001030CD
		protected override void OnOpen()
		{
			this.m_windowCanvas.gameObject.SetActive(true);
			this.m_entryTime = Time.unscaledTime;
			this.m_isLoading = false;
			base.StartCoroutine(this.AnimateControllerCoroutine());
		}

		// Token: 0x060048B6 RID: 18614 RVA: 0x00104EFF File Offset: 0x001030FF
		private IEnumerator AnimateControllerCoroutine()
		{
			float delay = Time.time + 1f;
			while (Time.time < delay)
			{
				yield return null;
			}
			float tweenAmount = 10f;
			for (;;)
			{
				yield return TweenManager.TweenBy(this.m_controllerRectTransform, 0.5f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
				{
					"anchoredPosition.y",
					tweenAmount
				}).TweenCoroutine;
				yield return TweenManager.TweenBy(this.m_controllerRectTransform, 0.5f, new EaseDelegate(Ease.Bounce.EaseOut), new object[]
				{
					"anchoredPosition.y",
					-tweenAmount
				}).TweenCoroutine;
				delay = Time.time + 2f;
				while (Time.time < delay)
				{
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x060048B7 RID: 18615 RVA: 0x00104F0E File Offset: 0x0010310E
		protected override void OnClose()
		{
		}

		// Token: 0x060048B8 RID: 18616 RVA: 0x00104F10 File Offset: 0x00103110
		protected override void OnFocus()
		{
		}

		// Token: 0x060048B9 RID: 18617 RVA: 0x00104F12 File Offset: 0x00103112
		protected override void OnLostFocus()
		{
		}

		// Token: 0x060048BA RID: 18618 RVA: 0x00104F14 File Offset: 0x00103114
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (this.m_isLoading)
			{
				return;
			}
			if (!RewiredMapController.IsCurrentMapEnabled)
			{
				return;
			}
			if (Time.unscaledTime < this.m_entryTime + 1f)
			{
				return;
			}
			IList<Joystick> joysticks = ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				Joystick joystick = joysticks[i];
				if (Rewired_RL.IsStandardJoystick(joystick) && joystick.GetAnyButtonDown())
				{
					this.LoadMainMenu();
					return;
				}
			}
			if (ReInput.controllers.Keyboard.GetAnyButton())
			{
				this.LoadMainMenu();
				return;
			}
			if (ReInput.controllers.Mouse.GetAnyButton())
			{
				this.LoadMainMenu();
				return;
			}
			if (Time.unscaledTime > this.m_entryTime + 10f)
			{
				this.LoadMainMenu();
				return;
			}
		}

		// Token: 0x060048BB RID: 18619 RVA: 0x00104FD3 File Offset: 0x001031D3
		private void LoadMainMenu()
		{
			this.m_isLoading = true;
			SceneLoader_RL.LoadScene(SceneID.MainMenu, TransitionID.FadeToBlackNoLoading);
		}

		// Token: 0x04003D5C RID: 15708
		private const float AUTO_LOAD_DELAY = 10f;

		// Token: 0x04003D5D RID: 15709
		private const float INPUT_LOCKOUT_DURATION = 1f;

		// Token: 0x04003D5E RID: 15710
		[SerializeField]
		private RectTransform m_controllerRectTransform;

		// Token: 0x04003D5F RID: 15711
		private bool m_isLoading;

		// Token: 0x04003D60 RID: 15712
		private float m_entryTime;
	}
}
