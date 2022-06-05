using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000E9 RID: 233
public class Eggplant_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000510 RID: 1296 RVA: 0x00004EB5 File Offset: 0x000030B5
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"EggplantFireworksProjectile"
		};
	}

	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x06000511 RID: 1297 RVA: 0x00004ECB File Offset: 0x000030CB
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.55f, 1.75f);
		}
	}

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x06000512 RID: 1298 RVA: 0x00004EDC File Offset: 0x000030DC
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.55f, 1.25f);
		}
	}

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x06000513 RID: 1299 RVA: 0x00004EDC File Offset: 0x000030DC
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.55f, 1.25f);
		}
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x00004EED File Offset: 0x000030ED
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator High_Shot_Attack()
	{
		base.SetVelocityX(0f, false);
		yield return this.Default_TellIntroAndLoop("HighShot_Tell_Intro", this.m_highShot_TellIntro_AnimationSpeed, "HighShot_Tell_Hold", this.m_highShot_TellHold_AnimationSpeed, 2.5f);
		yield return this.Default_Animation("HighShot_Attack_Intro", this.m_highShot_AttackIntro_AnimationSpeed, this.m_highShot_AttackIntro_Delay, true);
		yield return this.Default_Animation("HighShot_Attack_Hold", this.m_highShot_AttackHold_AnimationSpeed, 0f, false);
		this.FireProjectile("EggplantFireworksProjectile", 1, true, 0f, 1f, true, true, true);
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return base.Wait(this.m_highShot_AttackHold_Delay, false);
		yield return this.Default_Animation("HighShot_Exit", this.m_highShot_Exit_AnimationSpeed, this.m_highShot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_highShot_Exit_ForceIdle, this.m_highShot_Exit_AttackCD);
		yield break;
	}

	// Token: 0x06000515 RID: 1301 RVA: 0x00004EFC File Offset: 0x000030FC
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Low_Shot_Attack()
	{
		base.SetVelocityX(0f, false);
		yield return this.Default_TellIntroAndLoop("LowShot_Tell_Intro", this.m_lowShot_TellIntro_AnimationSpeed, "LowShot_Tell_Hold", this.m_lowShot_TellHold_AnimationSpeed, 2.5f);
		yield return this.Default_Animation("LowShot_Attack_Intro", this.m_lowShot_AttackIntro_AnimationSpeed, this.m_lowShot_AttackIntro_Delay, true);
		yield return this.Default_Animation("LowShot_Attack_Hold", this.m_lowShot_AttackHold_AnimationSpeed, 0f, false);
		this.FireProjectile("EggplantFireworksProjectile", 2, true, 0f, 1f, true, true, true);
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return base.Wait(this.m_lowShot_AttackHold_Delay, false);
		yield return this.Default_Animation("LowShot_Exit", this.m_lowShot_Exit_AnimationSpeed, this.m_lowShot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_lowShot_Exit_ForceIdle, this.m_lowShot_Exit_AttackCD);
		yield break;
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x00004F0B File Offset: 0x0000310B
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Dual_Shot_Attack()
	{
		base.SetVelocityX(0f, false);
		yield return this.Default_TellIntroAndLoop("MegaShot_Tell_Intro", this.m_dualShot_TellIntro_AnimationSpeed, "MegaShot_Tell_Hold", this.m_dualShot_TellHold_AnimationSpeed, 2.5f);
		yield return this.Default_Animation("MegaShot_Attack_Intro", this.m_dualShot_AttackIntro_AnimationSpeed, this.m_dualShot_AttackIntro_Delay, true);
		yield return this.Default_Animation("MegaShot_Attack_Hold", this.m_dualShot_AttackHold_AnimationSpeed, 0f, false);
		this.FireProjectile("EggplantFireworksProjectile", 1, true, 0f, 1f, true, true, true);
		this.FireProjectile("EggplantFireworksProjectile", 2, true, 0f, 1f, true, true, true);
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return base.Wait(this.m_dualShot_AttackHold_Delay, false);
		yield return this.Default_Animation("MegaShot_Exit", this.m_dualShot_Exit_AnimationSpeed, this.m_dualShot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_dualShot_Exit_ForceIdle, this.m_dualShot_Exit_AttackCD);
		yield break;
	}

	// Token: 0x040008F9 RID: 2297
	private const string HIGH_SHOT_TELL_INTRO = "HighShot_Tell_Intro";

	// Token: 0x040008FA RID: 2298
	private const string HIGH_SHOT_TELL_HOLD = "HighShot_Tell_Hold";

	// Token: 0x040008FB RID: 2299
	private const string HIGH_SHOT_ATTACK_INTRO = "HighShot_Attack_Intro";

	// Token: 0x040008FC RID: 2300
	private const string HIGH_SHOT_ATTACK_HOLD = "HighShot_Attack_Hold";

	// Token: 0x040008FD RID: 2301
	private const string HIGH_SHOT_EXIT = "HighShot_Exit";

	// Token: 0x040008FE RID: 2302
	private const string FIREWORKS_PROJECTILE_NAME = "EggplantFireworksProjectile";

	// Token: 0x040008FF RID: 2303
	protected float m_highShot_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x04000900 RID: 2304
	protected float m_highShot_TellHold_AnimationSpeed = 0.85f;

	// Token: 0x04000901 RID: 2305
	protected const float m_highShot_TellIntroAndHold_Delay = 2.5f;

	// Token: 0x04000902 RID: 2306
	protected float m_highShot_AttackIntro_AnimationSpeed = 1.1f;

	// Token: 0x04000903 RID: 2307
	protected float m_highShot_AttackIntro_Delay;

	// Token: 0x04000904 RID: 2308
	protected float m_highShot_AttackHold_AnimationSpeed = 1.1f;

	// Token: 0x04000905 RID: 2309
	protected float m_highShot_AttackHold_Delay = 0.65f;

	// Token: 0x04000906 RID: 2310
	protected float m_highShot_Exit_AnimationSpeed = 1f;

	// Token: 0x04000907 RID: 2311
	protected float m_highShot_Exit_Delay;

	// Token: 0x04000908 RID: 2312
	protected float m_highShot_Exit_ForceIdle = 2f;

	// Token: 0x04000909 RID: 2313
	protected float m_highShot_Exit_AttackCD = 10f;

	// Token: 0x0400090A RID: 2314
	private const string LOW_SHOT_TELL_INTRO = "LowShot_Tell_Intro";

	// Token: 0x0400090B RID: 2315
	private const string LOW_SHOT_TELL_HOLD = "LowShot_Tell_Hold";

	// Token: 0x0400090C RID: 2316
	private const string LOW_SHOT_ATTACK_INTRO = "LowShot_Attack_Intro";

	// Token: 0x0400090D RID: 2317
	private const string LOW_SHOT_ATTACK_HOLD = "LowShot_Attack_Hold";

	// Token: 0x0400090E RID: 2318
	private const string LOW_SHOT_EXIT = "LowShot_Exit";

	// Token: 0x0400090F RID: 2319
	protected float m_lowShot_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x04000910 RID: 2320
	protected float m_lowShot_TellHold_AnimationSpeed = 0.85f;

	// Token: 0x04000911 RID: 2321
	protected const float m_lowShot_TellIntroAndHold_Delay = 2.5f;

	// Token: 0x04000912 RID: 2322
	protected float m_lowShot_AttackIntro_AnimationSpeed = 1.1f;

	// Token: 0x04000913 RID: 2323
	protected float m_lowShot_AttackIntro_Delay;

	// Token: 0x04000914 RID: 2324
	protected float m_lowShot_AttackHold_AnimationSpeed = 1.1f;

	// Token: 0x04000915 RID: 2325
	protected float m_lowShot_AttackHold_Delay = 0.65f;

	// Token: 0x04000916 RID: 2326
	protected float m_lowShot_Exit_AnimationSpeed = 1f;

	// Token: 0x04000917 RID: 2327
	protected float m_lowShot_Exit_Delay;

	// Token: 0x04000918 RID: 2328
	protected float m_lowShot_Exit_ForceIdle = 2f;

	// Token: 0x04000919 RID: 2329
	protected float m_lowShot_Exit_AttackCD = 10f;

	// Token: 0x0400091A RID: 2330
	private const string DUAL_SHOT_TELL_INTRO = "MegaShot_Tell_Intro";

	// Token: 0x0400091B RID: 2331
	private const string DUAL_SHOT_TELL_HOLD = "MegaShot_Tell_Hold";

	// Token: 0x0400091C RID: 2332
	private const string DUAL_SHOT_ATTACK_INTRO = "MegaShot_Attack_Intro";

	// Token: 0x0400091D RID: 2333
	private const string DUAL_SHOT_ATTACK_HOLD = "MegaShot_Attack_Hold";

	// Token: 0x0400091E RID: 2334
	private const string DUAL_SHOT_EXIT = "MegaShot_Exit";

	// Token: 0x0400091F RID: 2335
	protected float m_dualShot_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x04000920 RID: 2336
	protected float m_dualShot_TellHold_AnimationSpeed = 0.85f;

	// Token: 0x04000921 RID: 2337
	protected const float m_dualShot_TellIntroAndHold_Delay = 2.5f;

	// Token: 0x04000922 RID: 2338
	protected float m_dualShot_AttackIntro_AnimationSpeed = 1.1f;

	// Token: 0x04000923 RID: 2339
	protected float m_dualShot_AttackIntro_Delay;

	// Token: 0x04000924 RID: 2340
	protected float m_dualShot_AttackHold_AnimationSpeed = 1.1f;

	// Token: 0x04000925 RID: 2341
	protected float m_dualShot_AttackHold_Delay = 0.65f;

	// Token: 0x04000926 RID: 2342
	protected float m_dualShot_Exit_AnimationSpeed = 1f;

	// Token: 0x04000927 RID: 2343
	protected float m_dualShot_Exit_Delay;

	// Token: 0x04000928 RID: 2344
	protected float m_dualShot_Exit_ForceIdle = 2f;

	// Token: 0x04000929 RID: 2345
	protected float m_dualShot_Exit_AttackCD = 10f;
}
