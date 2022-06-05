using System;
using FMODUnity;
using RLAudio;
using UnityEngine;

// Token: 0x02000326 RID: 806
public class SetCollisionWithPlayerAudio_StateMachineBehaviour : StateMachineBehaviour
{
	// Token: 0x06001987 RID: 6535 RVA: 0x00090304 File Offset: 0x0008E504
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex);
		if (this.m_collisionWithPlayerAudioController == null)
		{
			this.m_collisionWithPlayerAudioController = animator.gameObject.GetComponent<CollisionWithPlayerAudioController>();
			if (this.m_collisionWithPlayerAudioController == null)
			{
				throw new MissingComponentException(string.Format("| {0} | {1} is missing a ICharacterCollisionAudioOverrideController Component.", this, animator.gameObject.name));
			}
		}
		this.m_collisionWithPlayerAudioController.SetCollisionWithPlayerAudioOverride(this.m_audioPath);
	}

	// Token: 0x06001988 RID: 6536 RVA: 0x0000CE3A File Offset: 0x0000B03A
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateExit(animator, stateInfo, layerIndex);
		if (this.m_clearOnExit)
		{
			this.m_collisionWithPlayerAudioController.SetCollisionWithPlayerAudioOverride(string.Empty);
		}
	}

	// Token: 0x04001835 RID: 6197
	[SerializeField]
	[EventRef]
	private string m_audioPath = string.Empty;

	// Token: 0x04001836 RID: 6198
	[SerializeField]
	private bool m_clearOnExit = true;

	// Token: 0x04001837 RID: 6199
	private CollisionWithPlayerAudioController m_collisionWithPlayerAudioController;
}
