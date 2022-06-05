using System;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x020002C7 RID: 711
public class CreatePlatform_Ability : GenericSpell_Ability
{
	// Token: 0x0600150A RID: 5386 RVA: 0x0000A808 File Offset: 0x00008A08
	protected override void Awake()
	{
		base.Awake();
		this.m_resumeCooldownIfPlayerExitsRoom = new Action<MonoBehaviour, EventArgs>(this.ResumeCooldownIfPlayerExitsRoom);
		this.m_resumeCooldown = new Action<Projectile_RL, GameObject>(this.ResumeCooldown);
	}

	// Token: 0x0600150B RID: 5387 RVA: 0x0000A834 File Offset: 0x00008A34
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_resumeCooldownIfPlayerExitsRoom);
	}

	// Token: 0x0600150C RID: 5388 RVA: 0x0000A842 File Offset: 0x00008A42
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_resumeCooldownIfPlayerExitsRoom);
	}

	// Token: 0x0600150D RID: 5389 RVA: 0x00088BAC File Offset: 0x00086DAC
	protected override void FireProjectile()
	{
		base.FireProjectile();
		this.m_abilityController.PlayerController.CharacterJump.ResetBrakeForce();
		this.m_abilityController.PlayerController.SetVelocity(0f, 10f, false);
		this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.DisableHorizontalMovement;
		base.DecreaseCooldownOverTime = false;
		base.DisplayPausedAbilityCooldown = true;
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.OnDeathRelay.AddOnce(this.m_resumeCooldown, false);
		}
	}

	// Token: 0x0600150E RID: 5390 RVA: 0x0000A7CE File Offset: 0x000089CE
	private void ResumeCooldownIfPlayerExitsRoom(object sender, EventArgs args)
	{
		base.DecreaseCooldownOverTime = true;
		base.DisplayPausedAbilityCooldown = false;
	}

	// Token: 0x0600150F RID: 5391 RVA: 0x0000A7CE File Offset: 0x000089CE
	private void ResumeCooldown(Projectile_RL proj, GameObject obj)
	{
		base.DecreaseCooldownOverTime = true;
		base.DisplayPausedAbilityCooldown = false;
	}

	// Token: 0x06001510 RID: 5392 RVA: 0x0000A850 File Offset: 0x00008A50
	public override void StopAbility(bool abilityInterrupted)
	{
		base.StopAbility(abilityInterrupted);
		if (this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.Stunned)
		{
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
	}

	// Token: 0x04001652 RID: 5714
	private Action<MonoBehaviour, EventArgs> m_resumeCooldownIfPlayerExitsRoom;

	// Token: 0x04001653 RID: 5715
	private Action<Projectile_RL, GameObject> m_resumeCooldown;
}
