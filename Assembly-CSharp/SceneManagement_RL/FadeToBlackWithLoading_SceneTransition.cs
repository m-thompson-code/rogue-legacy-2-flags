using System;
using System.Collections;
using UnityEngine;

namespace SceneManagement_RL
{
	// Token: 0x020008C4 RID: 2244
	public class FadeToBlackWithLoading_SceneTransition : Transition_V2, ISceneLoadingTransition, ITransition
	{
		// Token: 0x170017FB RID: 6139
		// (get) Token: 0x060049A5 RID: 18853 RVA: 0x00109C37 File Offset: 0x00107E37
		public override TransitionID ID
		{
			get
			{
				return TransitionID.FadeToBlackWithLoading;
			}
		}

		// Token: 0x060049A6 RID: 18854 RVA: 0x00109C3A File Offset: 0x00107E3A
		protected override void Awake()
		{
			base.Awake();
			this.m_canvasGroup.alpha = 0f;
			base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("UI"), false);
		}

		// Token: 0x060049A7 RID: 18855 RVA: 0x00109C6D File Offset: 0x00107E6D
		public IEnumerator TransitionIn()
		{
			this.m_loadingIndicatorCanvasGroup.alpha = 0f;
			this.m_canvasGroup.alpha = 0f;
			yield return TweenManager.TweenTo_UnscaledTime(this.m_canvasGroup, this.m_timeToFade, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				1
			}).TweenCoroutine;
			yield return TweenManager.TweenTo_UnscaledTime(this.m_loadingIndicatorCanvasGroup, 0.5f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				1
			}).TweenCoroutine;
			yield break;
		}

		// Token: 0x060049A8 RID: 18856 RVA: 0x00109C7C File Offset: 0x00107E7C
		public IEnumerator TransitionOut()
		{
			this.m_canvasGroup.alpha = 1f;
			yield return TweenManager.TweenTo_UnscaledTime(this.m_loadingIndicatorCanvasGroup, 0.5f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				0
			}).TweenCoroutine;
			yield return TweenManager.TweenTo_UnscaledTime(this.m_canvasGroup, this.m_timeToFade, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				0
			}).TweenCoroutine;
			yield break;
		}

		// Token: 0x060049A9 RID: 18857 RVA: 0x00109C8B File Offset: 0x00107E8B
		public override IEnumerator Run()
		{
			yield break;
		}

		// Token: 0x060049AA RID: 18858 RVA: 0x00109C94 File Offset: 0x00107E94
		private void Update()
		{
			Vector3 localEulerAngles = this.m_rotatingGear.localEulerAngles;
			localEulerAngles.z -= 30f * Time.unscaledDeltaTime;
			this.m_rotatingGear.localEulerAngles = localEulerAngles;
		}

		// Token: 0x04003DFC RID: 15868
		[SerializeField]
		private float m_timeToFade = 1f;

		// Token: 0x04003DFD RID: 15869
		[SerializeField]
		private CanvasGroup m_canvasGroup;

		// Token: 0x04003DFE RID: 15870
		[SerializeField]
		private CanvasGroup m_loadingIndicatorCanvasGroup;

		// Token: 0x04003DFF RID: 15871
		[SerializeField]
		private RectTransform m_rotatingGear;
	}
}
