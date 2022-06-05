using System;
using UnityEngine;

// Token: 0x020003F4 RID: 1012
[RequireComponent(typeof(Breakable))]
public class BreakableSpawnProjectile : MonoBehaviour
{
	// Token: 0x17000F23 RID: 3875
	// (get) Token: 0x0600259A RID: 9626 RVA: 0x0007C533 File Offset: 0x0007A733
	public float BaseDamage
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x0600259B RID: 9627 RVA: 0x0007C53A File Offset: 0x0007A73A
	private void Awake()
	{
		this.m_breakable = base.GetComponent<Breakable>();
		this.m_breakable.OnDeathEffectTriggerRelay.AddListener(new Action<GameObject>(this.SpawnProjectile), false);
	}

	// Token: 0x0600259C RID: 9628 RVA: 0x0007C566 File Offset: 0x0007A766
	private void OnEnable()
	{
		ProjectileManager.Instance.AddProjectileToPool(this.m_projectileName);
	}

	// Token: 0x0600259D RID: 9629 RVA: 0x0007C578 File Offset: 0x0007A778
	private void OnDestroy()
	{
		if (this.m_breakable)
		{
			this.m_breakable.OnDeathEffectTriggerRelay.RemoveListener(new Action<GameObject>(this.SpawnProjectile));
		}
	}

	// Token: 0x0600259E RID: 9630 RVA: 0x0007C5A4 File Offset: 0x0007A7A4
	private void SpawnProjectile(GameObject obj)
	{
		if (!string.IsNullOrEmpty(this.m_projectileName))
		{
			Vector3 localPosition = this.m_projectileSpawnPos.transform.localPosition;
			if (this.m_flipBasedOnAttackerPos && this.m_breakable.AttackerIsOnRight)
			{
				localPosition.x = -localPosition.x;
			}
			Projectile_RL projectile_RL = ProjectileManager.FireProjectile(PlayerManager.GetPlayerController().gameObject, this.m_projectileName, localPosition + base.transform.position, false, 0f, 1f, true, true, true, true);
			projectile_RL.SnapToOwner = false;
			if (this.m_flipBasedOnAttackerPos && this.m_breakable.AttackerIsOnRight)
			{
				projectile_RL.Flip();
			}
			projectile_RL.RemoveAllStatusEffects();
		}
	}

	// Token: 0x04001F93 RID: 8083
	[SerializeField]
	private string m_projectileName;

	// Token: 0x04001F94 RID: 8084
	[SerializeField]
	private GameObject m_projectileSpawnPos;

	// Token: 0x04001F95 RID: 8085
	[SerializeField]
	private bool m_flipBasedOnAttackerPos;

	// Token: 0x04001F96 RID: 8086
	private Breakable m_breakable;
}
