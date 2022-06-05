using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x020004B1 RID: 1201
public class RicochetCollision : MonoBehaviour, IWeaponOnEnterHitResponse, IHitResponse, ITerrainOnEnterHitResponse
{
	// Token: 0x060026BE RID: 9918 RVA: 0x00015AF9 File Offset: 0x00013CF9
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(this.m_ricochetControlsLockoutDuration, false);
		this.m_projectile = base.GetComponent<Projectile_RL>();
		if (this.m_projectile == null)
		{
			this.m_charController = this.GetRoot(false).GetComponent<BaseCharacterController>();
		}
	}

	// Token: 0x060026BF RID: 9919 RVA: 0x00015B39 File Offset: 0x00013D39
	public void WeaponOnEnterHitResponse(IHitboxController otherHBController)
	{
		this.Ricochet(otherHBController.gameObject);
	}

	// Token: 0x060026C0 RID: 9920 RVA: 0x00015B39 File Offset: 0x00013D39
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		this.Ricochet(otherHBController.gameObject);
	}

	// Token: 0x060026C1 RID: 9921 RVA: 0x000B6F38 File Offset: 0x000B5138
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

	// Token: 0x060026C2 RID: 9922 RVA: 0x00015B47 File Offset: 0x00013D47
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

	// Token: 0x04002172 RID: 8562
	[SerializeField]
	private RicochetCollision.KnockbackType m_knockbackType;

	// Token: 0x04002173 RID: 8563
	[SerializeField]
	private bool m_ricochetMatchFacing;

	// Token: 0x04002174 RID: 8564
	[SerializeField]
	private float m_ricochetControlsLockoutDuration;

	// Token: 0x04002175 RID: 8565
	[SerializeField]
	private bool m_resetsPlayerJump;

	// Token: 0x04002176 RID: 8566
	private Projectile_RL m_projectile;

	// Token: 0x04002177 RID: 8567
	private BaseCharacterController m_charController;

	// Token: 0x04002178 RID: 8568
	private Coroutine m_controlsLockoutCoroutine;

	// Token: 0x04002179 RID: 8569
	private WaitRL_Yield m_waitYield;

	// Token: 0x020004B2 RID: 1202
	private enum KnockbackType
	{
		// Token: 0x0400217B RID: 8571
		Any,
		// Token: 0x0400217C RID: 8572
		VerticalOnly,
		// Token: 0x0400217D RID: 8573
		HorizontalOnly
	}
}
