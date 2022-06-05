using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000E5 RID: 229
public class Dummy_AIScript : BaseAIScript
{
	// Token: 0x060004F8 RID: 1272 RVA: 0x00004E17 File Offset: 0x00003017
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"SkeletonArcherBoltProjectile"
		};
	}

	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x060004F9 RID: 1273 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float ArrowAttack_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x060004FA RID: 1274 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float ArrowAttack_Duration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001BA RID: 442
	// (get) Token: 0x060004FB RID: 1275 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float ArrowAttack_Exit_ForceIdle
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x170001BB RID: 443
	// (get) Token: 0x060004FC RID: 1276 RVA: 0x00004573 File Offset: 0x00002773
	protected virtual float ArrowAttack_Exit_AttackCD
	{
		get
		{
			return 2.75f;
		}
	}

	// Token: 0x170001BC RID: 444
	// (get) Token: 0x060004FD RID: 1277 RVA: 0x000046FA File Offset: 0x000028FA
	protected virtual int ArrowAttack_NumProjectiles
	{
		get
		{
			return 10;
		}
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x00004E2D File Offset: 0x0000302D
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

	// Token: 0x060004FF RID: 1279 RVA: 0x00004E3C File Offset: 0x0000303C
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

	// Token: 0x06000500 RID: 1280 RVA: 0x00004E52 File Offset: 0x00003052
	public override void OnLBCompleteOrCancelled()
	{
		this.StopPersistentCoroutine(this.m_blinkCoroutine);
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x00004E66 File Offset: 0x00003066
	public override void ResetScript()
	{
		base.EnemyController.ControllerCorgi.GravityActive(false);
		base.ResetScript();
	}

	// Token: 0x040008EE RID: 2286
	protected const string ARROW_ATTACK_ATTACK = "Stunned";

	// Token: 0x040008EF RID: 2287
	protected const string ARROW_PROJECTILE = "SkeletonArcherBoltProjectile";

	// Token: 0x040008F0 RID: 2288
	private Coroutine m_blinkCoroutine;
}
