using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008FE RID: 2302
	public class OnExitEventEmitter : AnimBehaviourEventEmitter
	{
		// Token: 0x06004B9A RID: 19354 RVA: 0x0010FC1E File Offset: 0x0010DE1E
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.Play(animator);
		}
	}
}
