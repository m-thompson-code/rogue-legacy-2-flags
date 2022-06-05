using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000A4 RID: 164
public class Eggplant_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000391 RID: 913 RVA: 0x000150A3 File Offset: 0x000132A3
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"EggplantFireworksProjectile"
		};
	}

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x06000392 RID: 914 RVA: 0x000150B9 File Offset: 0x000132B9
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.55f, 1.75f);
		}
	}

	// Token: 0x17000144 RID: 324
	// (get) Token: 0x06000393 RID: 915 RVA: 0x000150CA File Offset: 0x000132CA
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.55f, 1.25f);
		}
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x06000394 RID: 916 RVA: 0x000150DB File Offset: 0x000132DB
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.55f, 1.25f);
		}
	}

	// Token: 0x06000395 RID: 917 RVA: 0x000150EC File Offset: 0x000132EC
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

	// Token: 0x06000396 RID: 918 RVA: 0x000150FB File Offset: 0x000132FB
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

	// Token: 0x06000397 RID: 919 RVA: 0x0001510A File Offset: 0x0001330A
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

	// Token: 0x040007CD RID: 1997
	private const string HIGH_SHOT_TELL_INTRO = "HighShot_Tell_Intro";

	// Token: 0x040007CE RID: 1998
	private const string HIGH_SHOT_TELL_HOLD = "HighShot_Tell_Hold";

	// Token: 0x040007CF RID: 1999
	private const string HIGH_SHOT_ATTACK_INTRO = "HighShot_Attack_Intro";

	// Token: 0x040007D0 RID: 2000
	private const string HIGH_SHOT_ATTACK_HOLD = "HighShot_Attack_Hold";

	// Token: 0x040007D1 RID: 2001
	private const string HIGH_SHOT_EXIT = "HighShot_Exit";

	// Token: 0x040007D2 RID: 2002
	private const string FIREWORKS_PROJECTILE_NAME = "EggplantFireworksProjectile";

	// Token: 0x040007D3 RID: 2003
	protected float m_highShot_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x040007D4 RID: 2004
	protected float m_highShot_TellHold_AnimationSpeed = 0.85f;

	// Token: 0x040007D5 RID: 2005
	protected const float m_highShot_TellIntroAndHold_Delay = 2.5f;

	// Token: 0x040007D6 RID: 2006
	protected float m_highShot_AttackIntro_AnimationSpeed = 1.1f;

	// Token: 0x040007D7 RID: 2007
	protected float m_highShot_AttackIntro_Delay;

	// Token: 0x040007D8 RID: 2008
	protected float m_highShot_AttackHold_AnimationSpeed = 1.1f;

	// Token: 0x040007D9 RID: 2009
	protected float m_highShot_AttackHold_Delay = 0.65f;

	// Token: 0x040007DA RID: 2010
	protected float m_highShot_Exit_AnimationSpeed = 1f;

	// Token: 0x040007DB RID: 2011
	protected float m_highShot_Exit_Delay;

	// Token: 0x040007DC RID: 2012
	protected float m_highShot_Exit_ForceIdle = 2f;

	// Token: 0x040007DD RID: 2013
	protected float m_highShot_Exit_AttackCD = 10f;

	// Token: 0x040007DE RID: 2014
	private const string LOW_SHOT_TELL_INTRO = "LowShot_Tell_Intro";

	// Token: 0x040007DF RID: 2015
	private const string LOW_SHOT_TELL_HOLD = "LowShot_Tell_Hold";

	// Token: 0x040007E0 RID: 2016
	private const string LOW_SHOT_ATTACK_INTRO = "LowShot_Attack_Intro";

	// Token: 0x040007E1 RID: 2017
	private const string LOW_SHOT_ATTACK_HOLD = "LowShot_Attack_Hold";

	// Token: 0x040007E2 RID: 2018
	private const string LOW_SHOT_EXIT = "LowShot_Exit";

	// Token: 0x040007E3 RID: 2019
	protected float m_lowShot_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x040007E4 RID: 2020
	protected float m_lowShot_TellHold_AnimationSpeed = 0.85f;

	// Token: 0x040007E5 RID: 2021
	protected const float m_lowShot_TellIntroAndHold_Delay = 2.5f;

	// Token: 0x040007E6 RID: 2022
	protected float m_lowShot_AttackIntro_AnimationSpeed = 1.1f;

	// Token: 0x040007E7 RID: 2023
	protected float m_lowShot_AttackIntro_Delay;

	// Token: 0x040007E8 RID: 2024
	protected float m_lowShot_AttackHold_AnimationSpeed = 1.1f;

	// Token: 0x040007E9 RID: 2025
	protected float m_lowShot_AttackHold_Delay = 0.65f;

	// Token: 0x040007EA RID: 2026
	protected float m_lowShot_Exit_AnimationSpeed = 1f;

	// Token: 0x040007EB RID: 2027
	protected float m_lowShot_Exit_Delay;

	// Token: 0x040007EC RID: 2028
	protected float m_lowShot_Exit_ForceIdle = 2f;

	// Token: 0x040007ED RID: 2029
	protected float m_lowShot_Exit_AttackCD = 10f;

	// Token: 0x040007EE RID: 2030
	private const string DUAL_SHOT_TELL_INTRO = "MegaShot_Tell_Intro";

	// Token: 0x040007EF RID: 2031
	private const string DUAL_SHOT_TELL_HOLD = "MegaShot_Tell_Hold";

	// Token: 0x040007F0 RID: 2032
	private const string DUAL_SHOT_ATTACK_INTRO = "MegaShot_Attack_Intro";

	// Token: 0x040007F1 RID: 2033
	private const string DUAL_SHOT_ATTACK_HOLD = "MegaShot_Attack_Hold";

	// Token: 0x040007F2 RID: 2034
	private const string DUAL_SHOT_EXIT = "MegaShot_Exit";

	// Token: 0x040007F3 RID: 2035
	protected float m_dualShot_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x040007F4 RID: 2036
	protected float m_dualShot_TellHold_AnimationSpeed = 0.85f;

	// Token: 0x040007F5 RID: 2037
	protected const float m_dualShot_TellIntroAndHold_Delay = 2.5f;

	// Token: 0x040007F6 RID: 2038
	protected float m_dualShot_AttackIntro_AnimationSpeed = 1.1f;

	// Token: 0x040007F7 RID: 2039
	protected float m_dualShot_AttackIntro_Delay;

	// Token: 0x040007F8 RID: 2040
	protected float m_dualShot_AttackHold_AnimationSpeed = 1.1f;

	// Token: 0x040007F9 RID: 2041
	protected float m_dualShot_AttackHold_Delay = 0.65f;

	// Token: 0x040007FA RID: 2042
	protected float m_dualShot_Exit_AnimationSpeed = 1f;

	// Token: 0x040007FB RID: 2043
	protected float m_dualShot_Exit_Delay;

	// Token: 0x040007FC RID: 2044
	protected float m_dualShot_Exit_ForceIdle = 2f;

	// Token: 0x040007FD RID: 2045
	protected float m_dualShot_Exit_AttackCD = 10f;
}
