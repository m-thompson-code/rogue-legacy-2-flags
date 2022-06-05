using System;
using UnityEngine;

// Token: 0x020004A2 RID: 1186
public class ManaBombRegenProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06002B65 RID: 11109 RVA: 0x000933D5 File Offset: 0x000915D5
	protected override void Awake()
	{
		base.Awake();
		this.m_onCollision = new Action<Projectile_RL, GameObject>(this.OnCollision);
	}

	// Token: 0x06002B66 RID: 11110 RVA: 0x000933EF File Offset: 0x000915EF
	private void OnEnable()
	{
		if (base.SourceProjectile)
		{
			base.SourceProjectile.OnCollisionRelay.AddListener(this.m_onCollision, false);
		}
	}

	// Token: 0x06002B67 RID: 11111 RVA: 0x00093416 File Offset: 0x00091616
	private void OnDisable()
	{
		if (base.SourceProjectile)
		{
			base.SourceProjectile.OnCollisionRelay.RemoveListener(this.m_onCollision);
		}
	}

	// Token: 0x06002B68 RID: 11112 RVA: 0x0009343C File Offset: 0x0009163C
	protected void OnCollision(Projectile_RL projectile, GameObject colliderObj)
	{
		colliderObj.CompareTag("EnemyProjectile");
	}

	// Token: 0x06002B69 RID: 11113 RVA: 0x0009344A File Offset: 0x0009164A
	private void RegenMana(GameObject colliderObj)
	{
		this.m_regenEventArgs.Initialise(0f, false);
		EffectManager.PlayEffect(colliderObj, null, "ManaRegenBurst_Effect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerForceManaRegen, this, this.m_regenEventArgs);
	}

	// Token: 0x0400234F RID: 9039
	private Action<Projectile_RL, GameObject> m_onCollision;

	// Token: 0x04002350 RID: 9040
	private ForceManaRegenEventArgs m_regenEventArgs = new ForceManaRegenEventArgs(0f, false);
}
