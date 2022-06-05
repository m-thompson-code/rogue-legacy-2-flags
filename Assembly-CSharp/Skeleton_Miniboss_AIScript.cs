using System;
using UnityEngine;

// Token: 0x020001E6 RID: 486
public class Skeleton_Miniboss_AIScript : Skeleton_Basic_AIScript
{
	// Token: 0x17000626 RID: 1574
	// (get) Token: 0x06000D49 RID: 3401 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_jump_tweak_X
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000D4A RID: 3402 RVA: 0x00070328 File Offset: 0x0006E528
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		this.m_jump_Power = new Vector2(-8.5f, 40.5f);
		this.m_jump_Tell_Delay = 0.55f;
		this.m_jump_Exit_AttackCD = 0f;
		this.m_throwBone_AttackCD = 0f;
		this.m_throwRib_Exit_AttackCD = 4.5f;
		base.LogicController.DisableLogicActivationByDistance = true;
	}
}
