using System;
using UnityEngine;

// Token: 0x02000327 RID: 807
public class SetLegWeightAnimBehaviour : StateMachineBehaviour
{
	// Token: 0x0600198A RID: 6538 RVA: 0x00090374 File Offset: 0x0008E574
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

	// Token: 0x0600198B RID: 6539 RVA: 0x000903A8 File Offset: 0x0008E5A8
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

	// Token: 0x0600198C RID: 6540 RVA: 0x00090428 File Offset: 0x0008E628
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

	// Token: 0x04001838 RID: 6200
	[SerializeField]
	private float m_movingOnGroundLegWeight;

	// Token: 0x04001839 RID: 6201
	[SerializeField]
	private float m_movingInAirLegWeight;

	// Token: 0x0400183A RID: 6202
	[SerializeField]
	private float m_stationaryOnGroundLegWeight = 1f;

	// Token: 0x0400183B RID: 6203
	[SerializeField]
	private float m_stationaryInAirLegWeight;

	// Token: 0x0400183C RID: 6204
	private static bool m_isMoving;

	// Token: 0x0400183D RID: 6205
	private static bool m_isGrounded;
}
