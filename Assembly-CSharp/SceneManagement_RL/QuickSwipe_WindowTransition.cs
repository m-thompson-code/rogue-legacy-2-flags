using System;
using System.Collections;
using UnityEngine;

namespace SceneManagement_RL
{
	// Token: 0x020008C9 RID: 2249
	public class QuickSwipe_WindowTransition : Transition_V2, ISceneLoadingTransition, ITransition
	{
		// Token: 0x17001802 RID: 6146
		// (get) Token: 0x060049D3 RID: 18899 RVA: 0x0010A441 File Offset: 0x00108641
		public override TransitionID ID
		{
			get
			{
				return TransitionID.QuickSwipe;
			}
		}

		// Token: 0x060049D4 RID: 18900 RVA: 0x0010A444 File Offset: 0x00108644
		protected override void Awake()
		{
			base.Awake();
			this.m_animator = base.GetComponent<Animator>();
			this.m_waitRL = new WaitRL_Yield(0f, true);
		}

		// Token: 0x060049D5 RID: 18901 RVA: 0x0010A469 File Offset: 0x00108669
		public IEnumerator TransitionIn()
		{
			this.m_animator.SetBool("Covered", true);
			yield return null;
			AnimatorStateInfo currentAnimatorStateInfo = this.m_animator.GetCurrentAnimatorStateInfo(0);
			float waitTime = currentAnimatorStateInfo.length * Mathf.Abs(currentAnimatorStateInfo.speed) * currentAnimatorStateInfo.speedMultiplier;
			this.m_waitRL.CreateNew(waitTime, true);
			yield return this.m_waitRL;
			yield break;
		}

		// Token: 0x060049D6 RID: 18902 RVA: 0x0010A478 File Offset: 0x00108678
		public IEnumerator TransitionOut()
		{
			yield return null;
			this.m_animator.SetBool("Covered", false);
			yield return null;
			AnimatorStateInfo currentAnimatorStateInfo = this.m_animator.GetCurrentAnimatorStateInfo(0);
			float waitTime = currentAnimatorStateInfo.length * Mathf.Abs(currentAnimatorStateInfo.speed) * currentAnimatorStateInfo.speedMultiplier;
			this.m_waitRL.CreateNew(waitTime, true);
			yield return this.m_waitRL;
			yield break;
		}

		// Token: 0x060049D7 RID: 18903 RVA: 0x0010A487 File Offset: 0x00108687
		public override IEnumerator Run()
		{
			yield break;
		}

		// Token: 0x04003E2E RID: 15918
		private Animator m_animator;

		// Token: 0x04003E2F RID: 15919
		private WaitRL_Yield m_waitRL;
	}
}
