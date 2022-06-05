using System;
using System.Collections;
using UnityEngine;

namespace SceneManagement_RL
{
	// Token: 0x02000E30 RID: 3632
	public class QuickSwipe_WindowTransition : Transition_V2, ISceneLoadingTransition, ITransition
	{
		// Token: 0x170020D6 RID: 8406
		// (get) Token: 0x0600665C RID: 26204 RVA: 0x000047A4 File Offset: 0x000029A4
		public override TransitionID ID
		{
			get
			{
				return TransitionID.QuickSwipe;
			}
		}

		// Token: 0x0600665D RID: 26205 RVA: 0x000385C1 File Offset: 0x000367C1
		protected override void Awake()
		{
			base.Awake();
			this.m_animator = base.GetComponent<Animator>();
			this.m_waitRL = new WaitRL_Yield(0f, true);
		}

		// Token: 0x0600665E RID: 26206 RVA: 0x000385E6 File Offset: 0x000367E6
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

		// Token: 0x0600665F RID: 26207 RVA: 0x000385F5 File Offset: 0x000367F5
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

		// Token: 0x06006660 RID: 26208 RVA: 0x00038604 File Offset: 0x00036804
		public override IEnumerator Run()
		{
			yield break;
		}

		// Token: 0x04005330 RID: 21296
		private Animator m_animator;

		// Token: 0x04005331 RID: 21297
		private WaitRL_Yield m_waitRL;
	}
}
