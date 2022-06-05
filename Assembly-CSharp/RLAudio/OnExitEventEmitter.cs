using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E78 RID: 3704
	public class OnExitEventEmitter : AnimBehaviourEventEmitter
	{
		// Token: 0x06006883 RID: 26755 RVA: 0x00039D87 File Offset: 0x00037F87
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.Play(animator);
		}
	}
}
