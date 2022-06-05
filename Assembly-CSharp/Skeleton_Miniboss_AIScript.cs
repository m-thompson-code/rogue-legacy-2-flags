using System;
using UnityEngine;

// Token: 0x0200011C RID: 284
public class Skeleton_Miniboss_AIScript : Skeleton_Basic_AIScript
{
	// Token: 0x170004A2 RID: 1186
	// (get) Token: 0x060008B8 RID: 2232 RVA: 0x0001D168 File Offset: 0x0001B368
	protected override bool m_jump_tweak_X
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x0001D16C File Offset: 0x0001B36C
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
