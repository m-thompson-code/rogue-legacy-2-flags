using System;
using UnityEngine;

// Token: 0x0200069D RID: 1693
[RequireComponent(typeof(Breakable))]
public class BreakableSpawnProjectile : MonoBehaviour
{
	// Token: 0x170013D2 RID: 5074
	// (get) Token: 0x06003408 RID: 13320 RVA: 0x00003C70 File Offset: 0x00001E70
	public float BaseDamage
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x06003409 RID: 13321 RVA: 0x0001C8B3 File Offset: 0x0001AAB3
	private void Awake()
	{
		this.m_breakable = base.GetComponent<Breakable>();
		this.m_breakable.OnDeathEffectTriggerRelay.AddListener(new Action<GameObject>(this.SpawnProjectile), false);
	}

	// Token: 0x0600340A RID: 13322 RVA: 0x0001C8DF File Offset: 0x0001AADF
	private void OnEnable()
	{
		ProjectileManager.Instance.AddProjectileToPool(this.m_projectileName);
	}

	// Token: 0x0600340B RID: 13323 RVA: 0x0001C8F1 File Offset: 0x0001AAF1
	private void OnDestroy()
	{
		if (this.m_breakable)
		{
			this.m_breakable.OnDeathEffectTriggerRelay.RemoveListener(new Action<GameObject>(this.SpawnProjectile));
		}
	}

	// Token: 0x0600340C RID: 13324 RVA: 0x000DC294 File Offset: 0x000DA494
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

	// Token: 0x04002A34 RID: 10804
	[SerializeField]
	private string m_projectileName;

	// Token: 0x04002A35 RID: 10805
	[SerializeField]
	private GameObject m_projectileSpawnPos;

	// Token: 0x04002A36 RID: 10806
	[SerializeField]
	private bool m_flipBasedOnAttackerPos;

	// Token: 0x04002A37 RID: 10807
	private Breakable m_breakable;
}
