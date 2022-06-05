using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E9C RID: 3740
	public class WalkWithBlendTreeEventEmitter : OnUpdateEventEmitter
	{
		// Token: 0x0600696A RID: 26986 RVA: 0x00182560 File Offset: 0x00180760
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			float @float = animator.GetFloat(this.m_walkParameter);
			if (@float < this.m_idleRange.x || @float > this.m_idleRange.y)
			{
				base.OnStateUpdate(animator, stateInfo, layerIndex);
			}
		}

		// Token: 0x040055CD RID: 21965
		[SerializeField]
		private string m_walkParameter = "FacingSpeed";

		// Token: 0x040055CE RID: 21966
		[SerializeField]
		private Vector2 m_idleRange;
	}
}
