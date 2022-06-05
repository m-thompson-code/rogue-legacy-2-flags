using System;
using UnityEngine;

// Token: 0x02000323 RID: 803
[SharedBetweenAnimators]
public class DisableGOAnimBehaviour : StateMachineBehaviour
{
	// Token: 0x06001976 RID: 6518 RVA: 0x0000CD64 File Offset: 0x0000AF64
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.gameObject.SetActive(false);
	}
}
