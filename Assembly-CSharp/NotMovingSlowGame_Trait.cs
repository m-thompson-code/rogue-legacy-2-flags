using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x02000357 RID: 855
public class NotMovingSlowGame_Trait : BaseTrait
{
	// Token: 0x17000DE8 RID: 3560
	// (get) Token: 0x06002077 RID: 8311 RVA: 0x0006691A File Offset: 0x00064B1A
	public override TraitType TraitType
	{
		get
		{
			return TraitType.NotMovingSlowGame;
		}
	}

	// Token: 0x06002078 RID: 8312 RVA: 0x00066921 File Offset: 0x00064B21
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		this.m_playerController = PlayerManager.GetPlayerController();
		yield break;
	}

	// Token: 0x06002079 RID: 8313 RVA: 0x00066930 File Offset: 0x00064B30
	private void OnDisable()
	{
		RLTimeScale.SetTimeScale(TimeScaleType.Game, 1f);
	}

	// Token: 0x0600207A RID: 8314 RVA: 0x00066940 File Offset: 0x00064B40
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

	// Token: 0x04001C59 RID: 7257
	[SerializeField]
	private float m_timeScaleUpSpeed = 5f;

	// Token: 0x04001C5A RID: 7258
	[SerializeField]
	private float m_timeScaleDownSpeed = 5f;

	// Token: 0x04001C5B RID: 7259
	private PlayerController m_playerController;

	// Token: 0x04001C5C RID: 7260
	private bool m_isMoving;

	// Token: 0x04001C5D RID: 7261
	private float m_slowDownDelayCounter;
}
