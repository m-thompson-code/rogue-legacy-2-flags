using System;
using UnityEngine;

// Token: 0x0200049C RID: 1180
public class DistanceSkillCritProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06002B44 RID: 11076 RVA: 0x00092B35 File Offset: 0x00090D35
	protected override void Awake()
	{
		base.Awake();
		this.m_onProjectileCollision = new Action<Projectile_RL, GameObject>(this.OnProjectileCollision);
	}

	// Token: 0x06002B45 RID: 11077 RVA: 0x00092B4F File Offset: 0x00090D4F
	private void OnEnable()
	{
		base.SourceProjectile.OnCollisionRelay.AddListener(this.m_onProjectileCollision, false);
		this.m_critWasApplied = false;
	}

	// Token: 0x06002B46 RID: 11078 RVA: 0x00092B70 File Offset: 0x00090D70
	private void OnDisable()
	{
		base.SourceProjectile.OnCollisionRelay.RemoveListener(this.m_onProjectileCollision);
	}

	// Token: 0x06002B47 RID: 11079 RVA: 0x00092B8C File Offset: 0x00090D8C
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

	// Token: 0x04002337 RID: 9015
	[SerializeField]
	private float m_critDistance;

	// Token: 0x04002338 RID: 9016
	[SerializeField]
	private DistanceSkillCritProjectileLogic.DistanceCheckType m_checkType;

	// Token: 0x04002339 RID: 9017
	private bool m_critWasApplied;

	// Token: 0x0400233A RID: 9018
	private Action<Projectile_RL, GameObject> m_onProjectileCollision;

	// Token: 0x02000C7E RID: 3198
	private enum DistanceCheckType
	{
		// Token: 0x040050B0 RID: 20656
		GreaterOrEqual,
		// Token: 0x040050B1 RID: 20657
		LessOrEqual
	}
}
