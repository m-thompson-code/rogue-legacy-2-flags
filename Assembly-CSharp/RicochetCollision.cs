using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x020002C0 RID: 704
public class RicochetCollision : MonoBehaviour, IWeaponOnEnterHitResponse, IHitResponse, ITerrainOnEnterHitResponse
{
	// Token: 0x06001C02 RID: 7170 RVA: 0x0005A514 File Offset: 0x00058714
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(this.m_ricochetControlsLockoutDuration, false);
		this.m_projectile = base.GetComponent<Projectile_RL>();
		if (this.m_projectile == null)
		{
			this.m_charController = this.GetRoot(false).GetComponent<BaseCharacterController>();
		}
	}

	// Token: 0x06001C03 RID: 7171 RVA: 0x0005A554 File Offset: 0x00058754
	public void WeaponOnEnterHitResponse(IHitboxController otherHBController)
	{
		this.Ricochet(otherHBController.gameObject);
	}

	// Token: 0x06001C04 RID: 7172 RVA: 0x0005A562 File Offset: 0x00058762
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		this.Ricochet(otherHBController.gameObject);
	}

	// Token: 0x06001C05 RID: 7173 RVA: 0x0005A570 File Offset: 0x00058770
	private void Ricochet(GameObject otherObj)
	{
		if (this.m_controlsLockoutCoroutine != null)
		{
			base.StopCoroutine(this.m_controlsLockoutCoroutine);
		}
		BaseCharacterController baseCharacterController = this.m_charController;
		if (this.m_projectile != null)
		{
			baseCharacterController = this.m_projectile.OwnerController;
		}
		PlayerController playerController = baseCharacterController as PlayerController;
		if (playerController != null)
		{
			playerController.StopActiveAbilities(false);
			playerController.CharacterJump.ResetBrakeForce();
			if (this.m_resetsPlayerJump)
			{
				playerController.CharacterJump.SetNumberOfJumpsLeft(playerController.CharacterJump.NumberOfJumps - 1);
			}
		}
		GameObject root = otherObj.GetRoot(false);
		Vector2 base_RICOCHET_DISTANCE = Player_EV.BASE_RICOCHET_DISTANCE;
		if (this.m_knockbackType == RicochetCollision.KnockbackType.HorizontalOnly)
		{
			base_RICOCHET_DISTANCE.y = baseCharacterController.Velocity.y;
		}
		else if (this.m_knockbackType == RicochetCollision.KnockbackType.VerticalOnly)
		{
			base_RICOCHET_DISTANCE.x = baseCharacterController.Velocity.x;
		}
		if (base_RICOCHET_DISTANCE.y > 0f)
		{
			baseCharacterController.CharacterCorgi.MovementState.ChangeState(CharacterStates.MovementStates.Jumping);
		}
		baseCharacterController.Animator.SetTrigger("AirJump");
		if (this.m_ricochetControlsLockoutDuration > 0f)
		{
			baseCharacterController.ConditionState = CharacterStates.CharacterConditions.DisableHorizontalMovement;
			this.m_controlsLockoutCoroutine = base.StartCoroutine(this.LockoutControls(this.m_ricochetControlsLockoutDuration, baseCharacterController));
		}
		if (!this.m_ricochetMatchFacing)
		{
			baseCharacterController.SetVelocity(base_RICOCHET_DISTANCE.x, base_RICOCHET_DISTANCE.y, false);
			return;
		}
		if (root.transform.position.x < baseCharacterController.transform.position.x)
		{
			baseCharacterController.SetVelocity(base_RICOCHET_DISTANCE.x, base_RICOCHET_DISTANCE.y, false);
			return;
		}
		baseCharacterController.SetVelocity(-base_RICOCHET_DISTANCE.x, base_RICOCHET_DISTANCE.y, false);
	}

	// Token: 0x06001C06 RID: 7174 RVA: 0x0005A6FB File Offset: 0x000588FB
	private IEnumerator LockoutControls(float duration, BaseCharacterController objToRicochet)
	{
		this.m_waitYield.CreateNew(duration, false);
		yield return this.m_waitYield;
		if (objToRicochet.ConditionState != CharacterStates.CharacterConditions.Stunned)
		{
			objToRicochet.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
		yield break;
	}

	// Token: 0x04001980 RID: 6528
	[SerializeField]
	private RicochetCollision.KnockbackType m_knockbackType;

	// Token: 0x04001981 RID: 6529
	[SerializeField]
	private bool m_ricochetMatchFacing;

	// Token: 0x04001982 RID: 6530
	[SerializeField]
	private float m_ricochetControlsLockoutDuration;

	// Token: 0x04001983 RID: 6531
	[SerializeField]
	private bool m_resetsPlayerJump;

	// Token: 0x04001984 RID: 6532
	private Projectile_RL m_projectile;

	// Token: 0x04001985 RID: 6533
	private BaseCharacterController m_charController;

	// Token: 0x04001986 RID: 6534
	private Coroutine m_controlsLockoutCoroutine;

	// Token: 0x04001987 RID: 6535
	private WaitRL_Yield m_waitYield;

	// Token: 0x02000B6A RID: 2922
	private enum KnockbackType
	{
		// Token: 0x04004C88 RID: 19592
		Any,
		// Token: 0x04004C89 RID: 19593
		VerticalOnly,
		// Token: 0x04004C8A RID: 19594
		HorizontalOnly
	}
}
