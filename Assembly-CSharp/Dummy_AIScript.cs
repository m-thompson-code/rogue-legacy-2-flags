using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000A2 RID: 162
public class Dummy_AIScript : BaseAIScript
{
	// Token: 0x06000385 RID: 901 RVA: 0x0001500B File Offset: 0x0001320B
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"SkeletonArcherBoltProjectile"
		};
	}

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x06000386 RID: 902 RVA: 0x00015021 File Offset: 0x00013221
	protected virtual float ArrowAttack_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x06000387 RID: 903 RVA: 0x00015028 File Offset: 0x00013228
	protected virtual float ArrowAttack_Duration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x06000388 RID: 904 RVA: 0x0001502F File Offset: 0x0001322F
	protected virtual float ArrowAttack_Exit_ForceIdle
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x06000389 RID: 905 RVA: 0x00015036 File Offset: 0x00013236
	protected virtual float ArrowAttack_Exit_AttackCD
	{
		get
		{
			return 2.75f;
		}
	}

	// Token: 0x17000142 RID: 322
	// (get) Token: 0x0600038A RID: 906 RVA: 0x0001503D File Offset: 0x0001323D
	protected virtual int ArrowAttack_NumProjectiles
	{
		get
		{
			return 10;
		}
	}

	// Token: 0x0600038B RID: 907 RVA: 0x00015041 File Offset: 0x00013241
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[MinibossEnemy]
	public IEnumerator Arrow_Attack()
	{
		base.EnemyController.AlwaysFacing = false;
		Color red = Color.red;
		red.a = 0.5f;
		this.m_blinkCoroutine = this.RunPersistentCoroutine(this.BlinkCoroutine(red));
		yield return this.Default_Animation("Stunned", this.ArrowAttack_AnimSpeed, this.ArrowAttack_Duration, true);
		yield return base.Wait(0.5f, false);
		float num = (float)(180 / this.ArrowAttack_NumProjectiles);
		for (int i = 0; i < this.ArrowAttack_NumProjectiles; i++)
		{
			this.FireProjectile("SkeletonArcherBoltProjectile", 0, false, num * (float)i, 1f, true, true, true);
		}
		this.StopPersistentCoroutine(this.m_blinkCoroutine);
		base.SetAttackingWithContactDamage(false, 0.1f);
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.ArrowAttack_Exit_ForceIdle, this.ArrowAttack_Exit_AttackCD);
		yield break;
	}

	// Token: 0x0600038C RID: 908 RVA: 0x00015050 File Offset: 0x00013250
	private IEnumerator BlinkCoroutine(Color color)
	{
		for (;;)
		{
			float blinkDuration = Time.time + base.EnemyController.BlinkPulseEffect.SingleBlinkDuration * 2f;
			base.EnemyController.BlinkPulseEffect.StartSingleBlinkEffect(color);
			while (Time.time < blinkDuration)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x0600038D RID: 909 RVA: 0x00015066 File Offset: 0x00013266
	public override void OnLBCompleteOrCancelled()
	{
		this.StopPersistentCoroutine(this.m_blinkCoroutine);
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x0600038E RID: 910 RVA: 0x0001507A File Offset: 0x0001327A
	public override void ResetScript()
	{
		base.EnemyController.ControllerCorgi.GravityActive(false);
		base.ResetScript();
	}

	// Token: 0x040007CA RID: 1994
	protected const string ARROW_ATTACK_ATTACK = "Stunned";

	// Token: 0x040007CB RID: 1995
	protected const string ARROW_PROJECTILE = "SkeletonArcherBoltProjectile";

	// Token: 0x040007CC RID: 1996
	private Coroutine m_blinkCoroutine;
}
