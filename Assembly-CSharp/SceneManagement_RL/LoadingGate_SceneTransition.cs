using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SceneManagement_RL
{
	// Token: 0x02000E28 RID: 3624
	public class LoadingGate_SceneTransition : Transition_V2, ISceneLoadingTransition, ITransition
	{
		// Token: 0x170020C7 RID: 8391
		// (get) Token: 0x06006628 RID: 26152 RVA: 0x00003DA1 File Offset: 0x00001FA1
		public override TransitionID ID
		{
			get
			{
				return TransitionID.CastleGate;
			}
		}

		// Token: 0x170020C8 RID: 8392
		// (get) Token: 0x06006629 RID: 26153 RVA: 0x0003847B File Offset: 0x0003667B
		// (set) Token: 0x0600662A RID: 26154 RVA: 0x00038483 File Offset: 0x00036683
		public bool DropPlayer { get; private set; }

		// Token: 0x0600662B RID: 26155 RVA: 0x0003848C File Offset: 0x0003668C
		protected override void Awake()
		{
			base.Awake();
			this.m_gateStartingY = this.m_gate.anchoredPosition.y;
			this.m_archwayStartingScale = this.m_archway.localScale.x;
			this.m_animator = base.GetComponent<Animator>();
		}

		// Token: 0x0600662C RID: 26156 RVA: 0x000384CC File Offset: 0x000366CC
		public IEnumerator TransitionIn()
		{
			this.DropPlayer = false;
			if (this.m_closeStartEvent != null)
			{
				this.m_closeStartEvent.Invoke();
			}
			this.m_animator.SetBool("Closed", true);
			float delayTime = Time.unscaledTime + 0.5f;
			while (Time.unscaledTime < delayTime)
			{
				yield return null;
			}
			delayTime = Time.unscaledTime + 0.325f;
			while (Time.unscaledTime < delayTime)
			{
				yield return null;
			}
			RumbleManager.StartRumble(true, true, 0.8f, 0.2f, true);
			while (this.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f)
			{
				yield return null;
			}
			if (this.m_closeEndEvent != null)
			{
				this.m_closeEndEvent.Invoke();
			}
			yield break;
		}

		// Token: 0x0600662D RID: 26157 RVA: 0x000384DB File Offset: 0x000366DB
		public IEnumerator TransitionOut()
		{
			if (this.m_openStartEvent != null)
			{
				this.m_openStartEvent.Invoke();
			}
			float delayTime = Time.unscaledTime + 0.5f;
			while (Time.unscaledTime < delayTime)
			{
				yield return null;
			}
			RumbleManager.StartRumble(true, true, 0.8f, 0.2f, true);
			this.m_animator.SetBool("Closed", false);
			delayTime = Time.unscaledTime + 0.25f;
			while (Time.unscaledTime < delayTime)
			{
				yield return null;
			}
			this.DropPlayer = true;
			delayTime = Time.unscaledTime + 0.25f;
			while (Time.unscaledTime < delayTime)
			{
				yield return null;
			}
			while (this.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
			{
				yield return null;
			}
			if (this.m_openEndEvent != null)
			{
				this.m_openEndEvent.Invoke();
			}
			yield break;
		}

		// Token: 0x0600662E RID: 26158 RVA: 0x000384EA File Offset: 0x000366EA
		public override IEnumerator Run()
		{
			yield break;
		}

		// Token: 0x0600662F RID: 26159 RVA: 0x0017A6E4 File Offset: 0x001788E4
		private void Update()
		{
			Vector3 localEulerAngles = this.m_loadingGear.localEulerAngles;
			localEulerAngles.z -= 30f * Time.unscaledDeltaTime;
			this.m_loadingGear.localEulerAngles = localEulerAngles;
		}

		// Token: 0x04005300 RID: 21248
		[SerializeField]
		private RectTransform m_gate;

		// Token: 0x04005301 RID: 21249
		[SerializeField]
		private RectTransform m_archway;

		// Token: 0x04005302 RID: 21250
		[SerializeField]
		private CanvasGroup m_blackness;

		// Token: 0x04005303 RID: 21251
		[SerializeField]
		private RectTransform m_loadingGear;

		// Token: 0x04005304 RID: 21252
		[SerializeField]
		private GameObject m_loadingGatePanel;

		// Token: 0x04005305 RID: 21253
		[SerializeField]
		private UnityEvent m_closeStartEvent;

		// Token: 0x04005306 RID: 21254
		[SerializeField]
		private UnityEvent m_closeEndEvent;

		// Token: 0x04005307 RID: 21255
		[SerializeField]
		private UnityEvent m_openStartEvent;

		// Token: 0x04005308 RID: 21256
		[SerializeField]
		private UnityEvent m_openEndEvent;

		// Token: 0x04005309 RID: 21257
		private float m_gateStartingY;

		// Token: 0x0400530A RID: 21258
		private float m_archwayStartingScale;

		// Token: 0x0400530B RID: 21259
		private Animator m_animator;
	}
}
