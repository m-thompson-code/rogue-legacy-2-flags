using System;
using UnityEngine;

// Token: 0x020001B8 RID: 440
public class SetLegWeightAnimBehaviour : StateMachineBehaviour
{
	// Token: 0x06001141 RID: 4417 RVA: 0x00032050 File Offset: 0x00030250
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (PlayerManager.IsInstantiated)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			if (playerController != null && playerController.IsInitialized)
			{
				this.UpdateLegLayerWeight(playerController, animator);
			}
		}
	}

	// Token: 0x06001142 RID: 4418 RVA: 0x00032084 File Offset: 0x00030284
	private void UpdateLegLayerWeight(PlayerController playerController, Animator animator)
	{
		SetLegWeightAnimBehaviour.m_isGrounded = playerController.IsGrounded;
		SetLegWeightAnimBehaviour.m_isMoving = (playerController.Velocity.x != 0f);
		if (SetLegWeightAnimBehaviour.m_isGrounded)
		{
			if (SetLegWeightAnimBehaviour.m_isMoving)
			{
				animator.SetLayerWeight(2, this.m_movingOnGroundLegWeight);
				return;
			}
			animator.SetLayerWeight(2, this.m_stationaryOnGroundLegWeight);
			return;
		}
		else
		{
			if (SetLegWeightAnimBehaviour.m_isMoving)
			{
				animator.SetLayerWeight(2, this.m_movingInAirLegWeight);
				return;
			}
			animator.SetLayerWeight(2, this.m_stationaryInAirLegWeight);
			return;
		}
	}

	// Token: 0x06001143 RID: 4419 RVA: 0x00032104 File Offset: 0x00030304
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (PlayerManager.IsInstantiated)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			if (playerController != null && playerController.IsInitialized)
			{
				bool isGrounded = playerController.IsGrounded;
				bool flag = playerController.Velocity.x != 0f;
				if (isGrounded != SetLegWeightAnimBehaviour.m_isGrounded || flag != SetLegWeightAnimBehaviour.m_isMoving)
				{
					this.UpdateLegLayerWeight(playerController, animator);
				}
			}
		}
	}

	// Token: 0x0400122F RID: 4655
	[SerializeField]
	private float m_movingOnGroundLegWeight;

	// Token: 0x04001230 RID: 4656
	[SerializeField]
	private float m_movingInAirLegWeight;

	// Token: 0x04001231 RID: 4657
	[SerializeField]
	private float m_stationaryOnGroundLegWeight = 1f;

	// Token: 0x04001232 RID: 4658
	[SerializeField]
	private float m_stationaryInAirLegWeight;

	// Token: 0x04001233 RID: 4659
	private static bool m_isMoving;

	// Token: 0x04001234 RID: 4660
	private static bool m_isGrounded;
}
