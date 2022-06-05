using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x020005C3 RID: 1475
public class NotMovingSlowGame_Trait : BaseTrait
{
	// Token: 0x17001247 RID: 4679
	// (get) Token: 0x06002DE0 RID: 11744 RVA: 0x000192D8 File Offset: 0x000174D8
	public override TraitType TraitType
	{
		get
		{
			return TraitType.NotMovingSlowGame;
		}
	}

	// Token: 0x06002DE1 RID: 11745 RVA: 0x000192DF File Offset: 0x000174DF
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		this.m_playerController = PlayerManager.GetPlayerController();
		yield break;
	}

	// Token: 0x06002DE2 RID: 11746 RVA: 0x000192EE File Offset: 0x000174EE
	private void OnDisable()
	{
		RLTimeScale.SetTimeScale(TimeScaleType.Game, 1f);
	}

	// Token: 0x06002DE3 RID: 11747 RVA: 0x000C7598 File Offset: 0x000C5798
	private void Update()
	{
		if (this.m_playerController != null && !this.m_playerController.IsDead && !GameManager.IsGamePaused && this.m_playerController.ConditionState != CharacterStates.CharacterConditions.Stunned)
		{
			if (this.m_playerController.Velocity != Vector2.zero || !this.m_playerController.IsGrounded || this.m_playerController.CastAbility.AnyAbilityInProgress || !this.m_playerController.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
			{
				float max = 1f;
				float value = RLTimeScale.TimeScale + this.m_timeScaleUpSpeed * Time.unscaledDeltaTime;
				RLTimeScale.SetTimeScale(TimeScaleType.Game, Mathf.Clamp(value, 0.01f, max));
				return;
			}
			float value2 = RLTimeScale.TimeScale - this.m_timeScaleDownSpeed * Time.unscaledDeltaTime;
			RLTimeScale.SetTimeScale(TimeScaleType.Game, Mathf.Clamp(value2, 0.01f, 1f));
		}
	}

	// Token: 0x040025DD RID: 9693
	[SerializeField]
	private float m_timeScaleUpSpeed = 5f;

	// Token: 0x040025DE RID: 9694
	[SerializeField]
	private float m_timeScaleDownSpeed = 5f;

	// Token: 0x040025DF RID: 9695
	private PlayerController m_playerController;

	// Token: 0x040025E0 RID: 9696
	private bool m_isMoving;

	// Token: 0x040025E1 RID: 9697
	private float m_slowDownDelayCounter;
}
