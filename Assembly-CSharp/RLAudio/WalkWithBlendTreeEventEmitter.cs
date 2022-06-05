using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x0200091E RID: 2334
	public class WalkWithBlendTreeEventEmitter : OnUpdateEventEmitter
	{
		// Token: 0x06004C69 RID: 19561 RVA: 0x00112884 File Offset: 0x00110A84
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			float @float = animator.GetFloat(this.m_walkParameter);
			if (@float < this.m_idleRange.x || @float > this.m_idleRange.y)
			{
				base.OnStateUpdate(animator, stateInfo, layerIndex);
			}
		}

		// Token: 0x04004065 RID: 16485
		[SerializeField]
		private string m_walkParameter = "FacingSpeed";

		// Token: 0x04004066 RID: 16486
		[SerializeField]
		private Vector2 m_idleRange;
	}
}
