using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000470 RID: 1136
public class ExtendRoots_ParadeAction : MonoBehaviour, IParadeAction
{
	// Token: 0x06002418 RID: 9240 RVA: 0x0001406D File Offset: 0x0001226D
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

	// Token: 0x04001FDD RID: 8157
	[SerializeField]
	private ExtendRoots_ParadeAction.ExtendRootEntry[] m_rootsToExtend;

	// Token: 0x02000471 RID: 1137
	[Serializable]
	private class ExtendRootEntry
	{
		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x0600241A RID: 9242 RVA: 0x00014083 File Offset: 0x00012283
		public float Delay
		{
			get
			{
				return this.m_delay;
			}
		}

		// Token: 0x17000F45 RID: 3909
		// (get) Token: 0x0600241B RID: 9243 RVA: 0x0001408B File Offset: 0x0001228B
		public Animator Animator
		{
			get
			{
				return this.m_animator;
			}
		}

		// Token: 0x04001FDE RID: 8158
		[SerializeField]
		private Animator m_animator;

		// Token: 0x04001FDF RID: 8159
		[SerializeField]
		private float m_delay;
	}
}
