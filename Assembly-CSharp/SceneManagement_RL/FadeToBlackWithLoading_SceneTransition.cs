using System;
using System.Collections;
using UnityEngine;

namespace SceneManagement_RL
{
	// Token: 0x02000E18 RID: 3608
	public class FadeToBlackWithLoading_SceneTransition : Transition_V2, ISceneLoadingTransition, ITransition
	{
		// Token: 0x170020A9 RID: 8361
		// (get) Token: 0x060065BC RID: 26044 RVA: 0x00004A8D File Offset: 0x00002C8D
		public override TransitionID ID
		{
			get
			{
				return TransitionID.FadeToBlackWithLoading;
			}
		}

		// Token: 0x060065BD RID: 26045 RVA: 0x00038159 File Offset: 0x00036359
		protected override void Awake()
		{
			base.Awake();
			this.m_canvasGroup.alpha = 0f;
			base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("UI"), false);
		}

		// Token: 0x060065BE RID: 26046 RVA: 0x0003818C File Offset: 0x0003638C
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

		// Token: 0x060065BF RID: 26047 RVA: 0x0003819B File Offset: 0x0003639B
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

		// Token: 0x060065C0 RID: 26048 RVA: 0x000381AA File Offset: 0x000363AA
		public override IEnumerator Run()
		{
			yield break;
		}

		// Token: 0x060065C1 RID: 26049 RVA: 0x001795A8 File Offset: 0x001777A8
		private void Update()
		{
			Vector3 localEulerAngles = this.m_rotatingGear.localEulerAngles;
			localEulerAngles.z -= 30f * Time.unscaledDeltaTime;
			this.m_rotatingGear.localEulerAngles = localEulerAngles;
		}

		// Token: 0x040052B3 RID: 21171
		[SerializeField]
		private float m_timeToFade = 1f;

		// Token: 0x040052B4 RID: 21172
		[SerializeField]
		private CanvasGroup m_canvasGroup;

		// Token: 0x040052B5 RID: 21173
		[SerializeField]
		private CanvasGroup m_loadingIndicatorCanvasGroup;

		// Token: 0x040052B6 RID: 21174
		[SerializeField]
		private RectTransform m_rotatingGear;
	}
}
