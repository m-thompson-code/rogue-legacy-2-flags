using System;
using FMODUnity;
using RLAudio;
using UnityEngine;

// Token: 0x020001B7 RID: 439
public class SetCollisionWithPlayerAudio_StateMachineBehaviour : StateMachineBehaviour
{
	// Token: 0x0600113E RID: 4414 RVA: 0x00031FA0 File Offset: 0x000301A0
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

	// Token: 0x0600113F RID: 4415 RVA: 0x00032010 File Offset: 0x00030210
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateExit(animator, stateInfo, layerIndex);
		if (this.m_clearOnExit)
		{
			this.m_collisionWithPlayerAudioController.SetCollisionWithPlayerAudioOverride(string.Empty);
		}
	}

	// Token: 0x0400122C RID: 4652
	[SerializeField]
	[EventRef]
	private string m_audioPath = string.Empty;

	// Token: 0x0400122D RID: 4653
	[SerializeField]
	private bool m_clearOnExit = true;

	// Token: 0x0400122E RID: 4654
	private CollisionWithPlayerAudioController m_collisionWithPlayerAudioController;
}
