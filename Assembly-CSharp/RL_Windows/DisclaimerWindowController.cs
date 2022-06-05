using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using SceneManagement_RL;
using UnityEngine;

namespace RL_Windows
{
	// Token: 0x02000DEE RID: 3566
	public class DisclaimerWindowController : WindowController
	{
		// Token: 0x17002055 RID: 8277
		// (get) Token: 0x06006434 RID: 25652 RVA: 0x000054B5 File Offset: 0x000036B5
		public override WindowID ID
		{
			get
			{
				return WindowID.Disclaimer;
			}
		}

		// Token: 0x06006435 RID: 25653 RVA: 0x000375FF File Offset: 0x000357FF
		protected override void OnOpen()
		{
			this.m_windowCanvas.gameObject.SetActive(true);
			this.m_entryTime = Time.unscaledTime;
			this.m_isLoading = false;
			base.StartCoroutine(this.AnimateControllerCoroutine());
		}

		// Token: 0x06006436 RID: 25654 RVA: 0x00037631 File Offset: 0x00035831
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

		// Token: 0x06006437 RID: 25655 RVA: 0x00002FCA File Offset: 0x000011CA
		protected override void OnClose()
		{
		}

		// Token: 0x06006438 RID: 25656 RVA: 0x00002FCA File Offset: 0x000011CA
		protected override void OnFocus()
		{
		}

		// Token: 0x06006439 RID: 25657 RVA: 0x00002FCA File Offset: 0x000011CA
		protected override void OnLostFocus()
		{
		}

		// Token: 0x0600643A RID: 25658 RVA: 0x00173CAC File Offset: 0x00171EAC
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

		// Token: 0x0600643B RID: 25659 RVA: 0x00037640 File Offset: 0x00035840
		private void LoadMainMenu()
		{
			this.m_isLoading = true;
			SceneLoader_RL.LoadScene(SceneID.MainMenu, TransitionID.FadeToBlackNoLoading);
		}

		// Token: 0x040051B6 RID: 20918
		private const float AUTO_LOAD_DELAY = 10f;

		// Token: 0x040051B7 RID: 20919
		private const float INPUT_LOCKOUT_DURATION = 1f;

		// Token: 0x040051B8 RID: 20920
		[SerializeField]
		private RectTransform m_controllerRectTransform;

		// Token: 0x040051B9 RID: 20921
		private bool m_isLoading;

		// Token: 0x040051BA RID: 20922
		private float m_entryTime;
	}
}
