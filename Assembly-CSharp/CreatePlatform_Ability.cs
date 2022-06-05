using System;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x02000180 RID: 384
public class CreatePlatform_Ability : GenericSpell_Ability
{
	// Token: 0x06000D95 RID: 3477 RVA: 0x0002989F File Offset: 0x00027A9F
	protected override void Awake()
	{
		base.Awake();
		this.m_resumeCooldownIfPlayerExitsRoom = new Action<MonoBehaviour, EventArgs>(this.ResumeCooldownIfPlayerExitsRoom);
		this.m_resumeCooldown = new Action<Projectile_RL, GameObject>(this.ResumeCooldown);
	}

	// Token: 0x06000D96 RID: 3478 RVA: 0x000298CB File Offset: 0x00027ACB
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_resumeCooldownIfPlayerExitsRoom);
	}

	// Token: 0x06000D97 RID: 3479 RVA: 0x000298D9 File Offset: 0x00027AD9
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_resumeCooldownIfPlayerExitsRoom);
	}

	// Token: 0x06000D98 RID: 3480 RVA: 0x000298E8 File Offset: 0x00027AE8
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

	// Token: 0x06000D99 RID: 3481 RVA: 0x0002996F File Offset: 0x00027B6F
	private void ResumeCooldownIfPlayerExitsRoom(object sender, EventArgs args)
	{
		base.DecreaseCooldownOverTime = true;
		base.DisplayPausedAbilityCooldown = false;
	}

	// Token: 0x06000D9A RID: 3482 RVA: 0x0002997F File Offset: 0x00027B7F
	private void ResumeCooldown(Projectile_RL proj, GameObject obj)
	{
		base.DecreaseCooldownOverTime = true;
		base.DisplayPausedAbilityCooldown = false;
	}

	// Token: 0x06000D9B RID: 3483 RVA: 0x0002998F File Offset: 0x00027B8F
	public override void StopAbility(bool abilityInterrupted)
	{
		base.StopAbility(abilityInterrupted);
		if (this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.Stunned)
		{
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
	}

	// Token: 0x040010E3 RID: 4323
	private Action<MonoBehaviour, EventArgs> m_resumeCooldownIfPlayerExitsRoom;

	// Token: 0x040010E4 RID: 4324
	private Action<Projectile_RL, GameObject> m_resumeCooldown;
}
