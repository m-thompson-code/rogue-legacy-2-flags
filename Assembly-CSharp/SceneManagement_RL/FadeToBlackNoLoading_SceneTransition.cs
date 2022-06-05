using System;
using System.Collections;
using UnityEngine;

namespace SceneManagement_RL
{
	// Token: 0x020008C3 RID: 2243
	public class FadeToBlackNoLoading_SceneTransition : Transition_V2, ISceneLoadingTransition, ITransition
	{
		// Token: 0x170017FA RID: 6138
		// (get) Token: 0x0600499F RID: 18847 RVA: 0x00109BC8 File Offset: 0x00107DC8
		public override TransitionID ID
		{
			get
			{
				return TransitionID.FadeToBlackNoLoading;
			}
		}

		// Token: 0x060049A0 RID: 18848 RVA: 0x00109BCB File Offset: 0x00107DCB
		protected override void Awake()
		{
			base.Awake();
			this.m_canvasGroup.alpha = 0f;
			base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("UI"), false);
		}

		// Token: 0x060049A1 RID: 18849 RVA: 0x00109BFE File Offset: 0x00107DFE
		public IEnumerator TransitionIn()
		{
			this.m_canvasGroup.alpha = 0f;
			yield return TweenManager.TweenTo_UnscaledTime(this.m_canvasGroup, this.m_timeToFade, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				1
			}).TweenCoroutine;
			yield break;
		}

		// Token: 0x060049A2 RID: 18850 RVA: 0x00109C0D File Offset: 0x00107E0D
		public IEnumerator TransitionOut()
		{
			this.m_canvasGroup.alpha = 1f;
			yield return TweenManager.TweenTo_UnscaledTime(this.m_canvasGroup, this.m_timeToFade, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				0
			}).TweenCoroutine;
			yield break;
		}

		// Token: 0x060049A3 RID: 18851 RVA: 0x00109C1C File Offset: 0x00107E1C
		public override IEnumerator Run()
		{
			yield break;
		}

		// Token: 0x04003DFA RID: 15866
		[SerializeField]
		private float m_timeToFade = 1f;

		// Token: 0x04003DFB RID: 15867
		[SerializeField]
		private CanvasGroup m_canvasGroup;
	}
}
