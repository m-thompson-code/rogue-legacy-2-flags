using System;
using UnityEngine;

// Token: 0x020007A1 RID: 1953
public class DistanceSkillCritProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06003B99 RID: 15257 RVA: 0x00020C0C File Offset: 0x0001EE0C
	protected override void Awake()
	{
		base.Awake();
		this.m_onProjectileCollision = new Action<Projectile_RL, GameObject>(this.OnProjectileCollision);
	}

	// Token: 0x06003B9A RID: 15258 RVA: 0x00020C26 File Offset: 0x0001EE26
	private void OnEnable()
	{
		base.SourceProjectile.OnCollisionRelay.AddListener(this.m_onProjectileCollision, false);
		this.m_critWasApplied = false;
	}

	// Token: 0x06003B9B RID: 15259 RVA: 0x00020C47 File Offset: 0x0001EE47
	private void OnDisable()
	{
		base.SourceProjectile.OnCollisionRelay.RemoveListener(this.m_onProjectileCollision);
	}

	// Token: 0x06003B9C RID: 15260 RVA: 0x000F451C File Offset: 0x000F271C
	private void OnProjectileCollision(Projectile_RL projectile, GameObject obj)
	{
		float num = PlayerManager.GetPlayerController().transform.localScale.x;
		num /= 1.4f;
		Vector3 v = base.SourceProjectile.transform.position;
		Collider2D lastCollidedWith = base.SourceProjectile.HitboxController.LastCollidedWith;
		if (lastCollidedWith)
		{
			v = lastCollidedWith.ClosestPoint(v);
		}
		float num2 = CDGHelper.DistanceBetweenPts(base.SourceProjectile.transform.position, v);
		bool flag;
		if (this.m_checkType == DistanceSkillCritProjectileLogic.DistanceCheckType.GreaterOrEqual)
		{
			flag = (num2 >= this.m_critDistance * num);
		}
		else
		{
			flag = (num2 <= this.m_critDistance * num);
		}
		if (flag)
		{
			if (base.SourceProjectile.ActualCritChance < 100f)
			{
				base.SourceProjectile.ActualCritChance += 100f;
				this.m_critWasApplied = true;
				return;
			}
		}
		else if (this.m_critWasApplied && base.SourceProjectile.ActualCritChance >= 100f)
		{
			base.SourceProjectile.ActualCritChance -= 100f;
			this.m_critWasApplied = false;
		}
	}

	// Token: 0x04002F52 RID: 12114
	[SerializeField]
	private float m_critDistance;

	// Token: 0x04002F53 RID: 12115
	[SerializeField]
	private DistanceSkillCritProjectileLogic.DistanceCheckType m_checkType;

	// Token: 0x04002F54 RID: 12116
	private bool m_critWasApplied;

	// Token: 0x04002F55 RID: 12117
	private Action<Projectile_RL, GameObject> m_onProjectileCollision;

	// Token: 0x020007A2 RID: 1954
	private enum DistanceCheckType
	{
		// Token: 0x04002F57 RID: 12119
		GreaterOrEqual,
		// Token: 0x04002F58 RID: 12120
		LessOrEqual
	}
}
