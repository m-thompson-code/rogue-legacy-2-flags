using System;
using UnityEngine;

// Token: 0x020001B4 RID: 436
[SharedBetweenAnimators]
public class DisableGOAnimBehaviour : StateMachineBehaviour
{
	// Token: 0x0600112D RID: 4397 RVA: 0x00031ACA File Offset: 0x0002FCCA
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.gameObject.SetActive(false);
	}
}
