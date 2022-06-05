using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002A2 RID: 674
public class ExtendRoots_ParadeAction : MonoBehaviour, IParadeAction
{
	// Token: 0x06001A1D RID: 6685 RVA: 0x0005231A File Offset: 0x0005051A
	public IEnumerator TriggerAction(ParadeController controller)
	{
		controller.SetRootMoveAudioParam(true);
		foreach (ExtendRoots_ParadeAction.ExtendRootEntry entry in this.m_rootsToExtend)
		{
			entry.Animator.speed = 1.15f;
			if (entry.Delay > 0f)
			{
				float delay = entry.Delay * 0.85f;
				delay += Time.time;
				while (Time.time < delay)
				{
					yield return null;
				}
			}
			entry.Animator.SetBool("Extend", true);
			entry = null;
		}
		ExtendRoots_ParadeAction.ExtendRootEntry[] array = null;
		controller.SetRootMoveAudioParam(false);
		yield break;
	}

	// Token: 0x04001886 RID: 6278
	[SerializeField]
	private ExtendRoots_ParadeAction.ExtendRootEntry[] m_rootsToExtend;

	// Token: 0x02000B47 RID: 2887
	[Serializable]
	private class ExtendRootEntry
	{
		// Token: 0x17001E7C RID: 7804
		// (get) Token: 0x06005C68 RID: 23656 RVA: 0x0015D4B7 File Offset: 0x0015B6B7
		public float Delay
		{
			get
			{
				return this.m_delay;
			}
		}

		// Token: 0x17001E7D RID: 7805
		// (get) Token: 0x06005C69 RID: 23657 RVA: 0x0015D4BF File Offset: 0x0015B6BF
		public Animator Animator
		{
			get
			{
				return this.m_animator;
			}
		}

		// Token: 0x04004BEC RID: 19436
		[SerializeField]
		private Animator m_animator;

		// Token: 0x04004BED RID: 19437
		[SerializeField]
		private float m_delay;
	}
}
