using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SceneManagement_RL
{
	// Token: 0x020008C7 RID: 2247
	public class LoadingGate_SceneTransition : Transition_V2, ISceneLoadingTransition, ITransition
	{
		// Token: 0x170017FF RID: 6143
		// (get) Token: 0x060049C3 RID: 18883 RVA: 0x0010A051 File Offset: 0x00108251
		public override TransitionID ID
		{
			get
			{
				return TransitionID.CastleGate;
			}
		}

		// Token: 0x17001800 RID: 6144
		// (get) Token: 0x060049C4 RID: 18884 RVA: 0x0010A054 File Offset: 0x00108254
		// (set) Token: 0x060049C5 RID: 18885 RVA: 0x0010A05C File Offset: 0x0010825C
		public bool DropPlayer { get; private set; }

		// Token: 0x060049C6 RID: 18886 RVA: 0x0010A065 File Offset: 0x00108265
		protected override void Awake()
		{
			base.Awake();
			this.m_gateStartingY = this.m_gate.anchoredPosition.y;
			this.m_archwayStartingScale = this.m_archway.localScale.x;
			this.m_animator = base.GetComponent<Animator>();
		}

		// Token: 0x060049C7 RID: 18887 RVA: 0x0010A0A5 File Offset: 0x001082A5
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

		// Token: 0x060049C8 RID: 18888 RVA: 0x0010A0B4 File Offset: 0x001082B4
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

		// Token: 0x060049C9 RID: 18889 RVA: 0x0010A0C3 File Offset: 0x001082C3
		public override IEnumerator Run()
		{
			yield break;
		}

		// Token: 0x060049CA RID: 18890 RVA: 0x0010A0CC File Offset: 0x001082CC
		private void Update()
		{
			Vector3 localEulerAngles = this.m_loadingGear.localEulerAngles;
			localEulerAngles.z -= 30f * Time.unscaledDeltaTime;
			this.m_loadingGear.localEulerAngles = localEulerAngles;
		}

		// Token: 0x04003E11 RID: 15889
		[SerializeField]
		private RectTransform m_gate;

		// Token: 0x04003E12 RID: 15890
		[SerializeField]
		private RectTransform m_archway;

		// Token: 0x04003E13 RID: 15891
		[SerializeField]
		private CanvasGroup m_blackness;

		// Token: 0x04003E14 RID: 15892
		[SerializeField]
		private RectTransform m_loadingGear;

		// Token: 0x04003E15 RID: 15893
		[SerializeField]
		private GameObject m_loadingGatePanel;

		// Token: 0x04003E16 RID: 15894
		[SerializeField]
		private UnityEvent m_closeStartEvent;

		// Token: 0x04003E17 RID: 15895
		[SerializeField]
		private UnityEvent m_closeEndEvent;

		// Token: 0x04003E18 RID: 15896
		[SerializeField]
		private UnityEvent m_openStartEvent;

		// Token: 0x04003E19 RID: 15897
		[SerializeField]
		private UnityEvent m_openEndEvent;

		// Token: 0x04003E1A RID: 15898
		private float m_gateStartingY;

		// Token: 0x04003E1B RID: 15899
		private float m_archwayStartingScale;

		// Token: 0x04003E1C RID: 15900
		private Animator m_animator;
	}
}
