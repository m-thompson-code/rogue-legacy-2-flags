using System;
using UnityEngine;

// Token: 0x020007AD RID: 1965
public class ManaBombRegenProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06003BD2 RID: 15314 RVA: 0x00020EF2 File Offset: 0x0001F0F2
	protected override void Awake()
	{
		base.Awake();
		this.m_onCollision = new Action<Projectile_RL, GameObject>(this.OnCollision);
	}

	// Token: 0x06003BD3 RID: 15315 RVA: 0x00020F0C File Offset: 0x0001F10C
	private void OnEnable()
	{
		if (base.SourceProjectile)
		{
			base.SourceProjectile.OnCollisionRelay.AddListener(this.m_onCollision, false);
		}
	}

	// Token: 0x06003BD4 RID: 15316 RVA: 0x00020F33 File Offset: 0x0001F133
	private void OnDisable()
	{
		if (base.SourceProjectile)
		{
			base.SourceProjectile.OnCollisionRelay.RemoveListener(this.m_onCollision);
		}
	}

	// Token: 0x06003BD5 RID: 15317 RVA: 0x00020F59 File Offset: 0x0001F159
	protected void OnCollision(Projectile_RL projectile, GameObject colliderObj)
	{
		colliderObj.CompareTag("EnemyProjectile");
	}

	// Token: 0x06003BD6 RID: 15318 RVA: 0x00020F67 File Offset: 0x0001F167
	private void RegenMana(GameObject colliderObj)
	{
		this.m_regenEventArgs.Initialise(0f, false);
		EffectManager.PlayEffect(colliderObj, null, "ManaRegenBurst_Effect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerForceManaRegen, this, this.m_regenEventArgs);
	}

	// Token: 0x04002F81 RID: 12161
	private Action<Projectile_RL, GameObject> m_onCollision;

	// Token: 0x04002F82 RID: 12162
	private ForceManaRegenEventArgs m_regenEventArgs = new ForceManaRegenEventArgs(0f, false);
}
