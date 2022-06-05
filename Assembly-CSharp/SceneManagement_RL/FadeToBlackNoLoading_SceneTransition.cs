using System;
using System.Collections;
using UnityEngine;

namespace SceneManagement_RL
{
	// Token: 0x02000E14 RID: 3604
	public class FadeToBlackNoLoading_SceneTransition : Transition_V2, ISceneLoadingTransition, ITransition
	{
		// Token: 0x170020A2 RID: 8354
		// (get) Token: 0x060065A4 RID: 26020 RVA: 0x000047A7 File Offset: 0x000029A7
		public override TransitionID ID
		{
			get
			{
				return TransitionID.FadeToBlackNoLoading;
			}
		}

		// Token: 0x060065A5 RID: 26021 RVA: 0x000380A8 File Offset: 0x000362A8
		protected override void Awake()
		{
			base.Awake();
			this.m_canvasGroup.alpha = 0f;
			base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("UI"), false);
		}

		// Token: 0x060065A6 RID: 26022 RVA: 0x000380DB File Offset: 0x000362DB
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

		// Token: 0x060065A7 RID: 26023 RVA: 0x000380EA File Offset: 0x000362EA
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

		// Token: 0x060065A8 RID: 26024 RVA: 0x000380F9 File Offset: 0x000362F9
		public override IEnumerator Run()
		{
			yield break;
		}

		// Token: 0x040052A9 RID: 21161
		[SerializeField]
		private float m_timeToFade = 1f;

		// Token: 0x040052AA RID: 21162
		[SerializeField]
		private CanvasGroup m_canvasGroup;
	}
}
