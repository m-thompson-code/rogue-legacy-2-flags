using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x020000E6 RID: 230
public class FlyingSkull_Basic_AIScript : BaseAIScript, IAudioEventEmitter
{
	// Token: 0x060006E4 RID: 1764 RVA: 0x00019EFD File Offset: 0x000180FD
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"FlyingSkullBoneProjectile"
		};
	}

	// Token: 0x17000396 RID: 918
	// (get) Token: 0x060006E5 RID: 1765 RVA: 0x00019F13 File Offset: 0x00018113
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x17000397 RID: 919
	// (get) Token: 0x060006E6 RID: 1766 RVA: 0x00019F24 File Offset: 0x00018124
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x17000398 RID: 920
	// (get) Token: 0x060006E7 RID: 1767 RVA: 0x00019F35 File Offset: 0x00018135
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x17000399 RID: 921
	// (get) Token: 0x060006E8 RID: 1768 RVA: 0x00019F46 File Offset: 0x00018146
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(-5f, 5f);
		}
	}

	// Token: 0x1700039A RID: 922
	// (get) Token: 0x060006E9 RID: 1769 RVA: 0x00019F57 File Offset: 0x00018157
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(2f, 5f);
		}
	}

	// Token: 0x1700039B RID: 923
	// (get) Token: 0x060006EA RID: 1770 RVA: 0x00019F68 File Offset: 0x00018168
	protected virtual float m_shoot_TellIntro_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700039C RID: 924
	// (get) Token: 0x060006EB RID: 1771 RVA: 0x00019F6F File Offset: 0x0001816F
	protected virtual float m_shoot_TellHold_AnimationSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700039D RID: 925
	// (get) Token: 0x060006EC RID: 1772 RVA: 0x00019F76 File Offset: 0x00018176
	protected virtual float m_shoot_Tell_Delay
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x1700039E RID: 926
	// (get) Token: 0x060006ED RID: 1773 RVA: 0x00019F7D File Offset: 0x0001817D
	protected virtual float m_shoot_AttackIntro_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700039F RID: 927
	// (get) Token: 0x060006EE RID: 1774 RVA: 0x00019F84 File Offset: 0x00018184
	protected virtual float m_shoot_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003A0 RID: 928
	// (get) Token: 0x060006EF RID: 1775 RVA: 0x00019F8B File Offset: 0x0001818B
	protected virtual float m_shoot_AttackHold_AnimationSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170003A1 RID: 929
	// (get) Token: 0x060006F0 RID: 1776 RVA: 0x00019F92 File Offset: 0x00018192
	protected virtual float m_shoot_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x170003A2 RID: 930
	// (get) Token: 0x060006F1 RID: 1777 RVA: 0x00019F99 File Offset: 0x00018199
	protected virtual float m_shoot_Exit_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170003A3 RID: 931
	// (get) Token: 0x060006F2 RID: 1778 RVA: 0x00019FA0 File Offset: 0x000181A0
	protected virtual float m_shoot_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003A4 RID: 932
	// (get) Token: 0x060006F3 RID: 1779 RVA: 0x00019FA7 File Offset: 0x000181A7
	protected virtual float m_shoot_Exit_ForceIdle
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x170003A5 RID: 933
	// (get) Token: 0x060006F4 RID: 1780 RVA: 0x00019FAE File Offset: 0x000181AE
	protected virtual float m_shoot_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003A6 RID: 934
	// (get) Token: 0x060006F5 RID: 1781 RVA: 0x00019FB5 File Offset: 0x000181B5
	protected virtual float m_shoot_BeforeTellDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003A7 RID: 935
	// (get) Token: 0x060006F6 RID: 1782 RVA: 0x00019FBC File Offset: 0x000181BC
	protected virtual float m_shoot_AfterAttackDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003A8 RID: 936
	// (get) Token: 0x060006F7 RID: 1783 RVA: 0x00019FC3 File Offset: 0x000181C3
	protected virtual int m_shoot_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170003A9 RID: 937
	// (get) Token: 0x060006F8 RID: 1784 RVA: 0x00019FC6 File Offset: 0x000181C6
	protected virtual float m_shoot_MultiShotDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x170003AA RID: 938
	// (get) Token: 0x060006F9 RID: 1785 RVA: 0x00019FCD File Offset: 0x000181CD
	protected virtual bool m_shoot_ShootNear
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170003AB RID: 939
	// (get) Token: 0x060006FA RID: 1786 RVA: 0x00019FD0 File Offset: 0x000181D0
	protected virtual bool m_shoot_ShootFar
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170003AC RID: 940
	// (get) Token: 0x060006FB RID: 1787 RVA: 0x00019FD3 File Offset: 0x000181D3
	protected virtual bool m_shoot_ShootMirror
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x00019FD6 File Offset: 0x000181D6
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ShootFireball()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.AlwaysFacing = false;
		if (this.m_shoot_BeforeTellDelay > 0f)
		{
			yield return base.Wait(this.m_shoot_BeforeTellDelay, false);
		}
		AudioManager.PlayOneShotAttached(this, "event:/SFX/Enemies/sfx_enemy_flyingSkull_shoot_prep", base.gameObject);
		yield return this.Default_TellIntroAndLoop("MultiShot_Tell_Intro", this.m_shoot_TellIntro_AnimationSpeed, "MultiShot_Tell_Hold", this.m_shoot_TellHold_AnimationSpeed, this.m_shoot_Tell_Delay);
		AudioManager.PlayOneShotAttached(this, "event:/SFX/Enemies/sfx_enemy_flyingSkull_shoot", base.gameObject);
		yield return this.Default_Animation("MultiShot_Attack_Intro", this.m_shoot_AttackIntro_AnimationSpeed, this.m_shoot_AttackIntro_Delay, true);
		yield return this.Default_Animation("MultiShot_Attack_Hold", this.m_shoot_AttackHold_AnimationSpeed, 0f, false);
		int num;
		for (int i = 0; i < this.m_shoot_TotalShots; i = num + 1)
		{
			if (this.m_shoot_ShootNear)
			{
				this.FireProjectile("FlyingSkullBoneProjectile", 0, true, (float)(90 - this.m_NearBone_Angle), 1f, true, true, true);
				if (this.m_shoot_ShootMirror)
				{
					this.FireProjectile("FlyingSkullBoneProjectile", 0, true, (float)(90 + this.m_NearBone_Angle), 1f, true, true, true);
				}
			}
			if (this.m_shoot_ShootFar)
			{
				this.FireProjectile("FlyingSkullBoneProjectile", 0, true, (float)(90 - this.m_FarBoneAngle), 1f, true, true, true);
				if (this.m_shoot_ShootMirror)
				{
					this.FireProjectile("FlyingSkullBoneProjectile", 0, true, (float)(90 + this.m_FarBoneAngle), 1f, true, true, true);
				}
			}
			if (this.m_shoot_MultiShotDelay > 0f)
			{
				yield return base.Wait(this.m_shoot_MultiShotDelay, false);
			}
			num = i;
		}
		if (this.m_shoot_AfterAttackDelay > 0f)
		{
			yield return base.Wait(this.m_shoot_AfterAttackDelay, false);
		}
		yield return this.Default_Animation("MultiShot_Exit", this.m_shoot_Exit_AnimationSpeed, this.m_shoot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_shoot_Exit_ForceIdle, this.m_shoot_Exit_AttackCD);
		base.EnemyController.AlwaysFacing = true;
		yield break;
	}

	// Token: 0x170003AD RID: 941
	// (get) Token: 0x060006FD RID: 1789 RVA: 0x00019FE5 File Offset: 0x000181E5
	protected virtual float m_dash_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170003AE RID: 942
	// (get) Token: 0x060006FE RID: 1790 RVA: 0x00019FEC File Offset: 0x000181EC
	protected virtual float m_dash_TellIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003AF RID: 943
	// (get) Token: 0x060006FF RID: 1791 RVA: 0x00019FF3 File Offset: 0x000181F3
	protected virtual float m_dash_TellHold_AnimSpeed
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x170003B0 RID: 944
	// (get) Token: 0x06000700 RID: 1792 RVA: 0x00019FFA File Offset: 0x000181FA
	protected virtual float m_dash_TellHold_Delay
	{
		get
		{
			return 0.65f;
		}
	}

	// Token: 0x170003B1 RID: 945
	// (get) Token: 0x06000701 RID: 1793 RVA: 0x0001A001 File Offset: 0x00018201
	protected virtual float m_dash_AttackIntro_AnimSpeed
	{
		get
		{
			return 2.75f;
		}
	}

	// Token: 0x170003B2 RID: 946
	// (get) Token: 0x06000702 RID: 1794 RVA: 0x0001A008 File Offset: 0x00018208
	protected virtual float m_dash_AttackIntro_Delay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x170003B3 RID: 947
	// (get) Token: 0x06000703 RID: 1795 RVA: 0x0001A00F File Offset: 0x0001820F
	protected virtual float m_dash_AttackHold_AnimSpeed
	{
		get
		{
			return 2.75f;
		}
	}

	// Token: 0x170003B4 RID: 948
	// (get) Token: 0x06000704 RID: 1796 RVA: 0x0001A016 File Offset: 0x00018216
	protected virtual float m_dash_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x06000705 RID: 1797 RVA: 0x0001A01D File Offset: 0x0001821D
	protected virtual float m_dash_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170003B6 RID: 950
	// (get) Token: 0x06000706 RID: 1798 RVA: 0x0001A024 File Offset: 0x00018224
	protected virtual float m_dash_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x06000707 RID: 1799 RVA: 0x0001A02B File Offset: 0x0001822B
	protected virtual float m_dash_Exit_ForceIdle
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170003B8 RID: 952
	// (get) Token: 0x06000708 RID: 1800 RVA: 0x0001A032 File Offset: 0x00018232
	protected virtual float m_dash_Exit_AttackCD
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170003B9 RID: 953
	// (get) Token: 0x06000709 RID: 1801 RVA: 0x0001A039 File Offset: 0x00018239
	protected virtual float m_dash_Attack_Speed
	{
		get
		{
			return 15f;
		}
	}

	// Token: 0x170003BA RID: 954
	// (get) Token: 0x0600070A RID: 1802 RVA: 0x0001A040 File Offset: 0x00018240
	protected virtual float m_dash_Tell_Duration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170003BB RID: 955
	// (get) Token: 0x0600070B RID: 1803 RVA: 0x0001A047 File Offset: 0x00018247
	protected virtual float m_dash_Attack_Duration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170003BC RID: 956
	// (get) Token: 0x0600070C RID: 1804 RVA: 0x0001A04E File Offset: 0x0001824E
	protected virtual bool m_dropsBonesDuringDashAttack
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170003BD RID: 957
	// (get) Token: 0x0600070D RID: 1805 RVA: 0x0001A051 File Offset: 0x00018251
	protected virtual int m_bonesDroppedDuringDashCount
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x0001A054 File Offset: 0x00018254
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator DashAttack()
	{
		this.ToDo("DASH!");
		this.StopAndFaceTarget();
		base.EnemyController.BaseTurnSpeed = 0f;
		base.EnemyController.FollowTarget = false;
		base.EnemyController.ForceFaceTarget();
		base.EnemyController.LockFlip = true;
		AudioManager.PlayOneShotAttached(this, "event:/SFX/Enemies/sfx_enemy_flyingSkull_dash_prep", base.gameObject);
		yield return this.Default_TellIntroAndLoop("SingleShot_Tell_Intro", this.m_dash_TellIntro_AnimSpeed, "SingleShot_Tell_Hold", this.m_dash_TellHold_AnimSpeed, this.m_dash_Tell_Duration);
		this.LeanIntoDash();
		this.SetDashVelocity();
		AudioManager.PlayOneShotAttached(this, "event:/SFX/Enemies/sfx_enemy_flyingSkull_dash", base.gameObject);
		yield return this.Default_Animation(BaseAIScript.NEUTRAL_STATE, 1f, 0f, true);
		if (this.m_dropsBonesDuringDashAttack)
		{
			yield return this.DropBonesDuringDash();
		}
		else if (this.m_dash_Attack_Duration > 0f)
		{
			yield return base.Wait(this.m_dash_Attack_Duration, false);
		}
		base.EnemyController.Pivot.transform.localRotation = Quaternion.identity;
		base.EnemyController.BaseTurnSpeed = 0f;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		if (this.m_dash_AttackHold_Delay > 0f)
		{
			yield return base.Wait(this.m_dash_AttackHold_Delay, false);
		}
		base.EnemyController.BaseSpeed = base.EnemyController.EnemyData.Speed;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		yield return this.Default_Attack_Cooldown(this.m_dash_Exit_ForceIdle, this.m_dash_Exit_AttackCD);
		base.EnemyController.FollowTarget = true;
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetTurnTrigger();
		base.EnemyController.ResetBaseValues();
		yield break;
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x0001A064 File Offset: 0x00018264
	private void SetDashVelocity()
	{
		Vector3 v = Vector2.left;
		if (base.EnemyController.IsFacingRight)
		{
			v = Vector2.right;
		}
		base.EnemyController.Heading = v;
		base.EnemyController.BaseSpeed = this.m_dash_Attack_Speed;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x0001A0C4 File Offset: 0x000182C4
	private void LeanIntoDash()
	{
		float z = 20f;
		if (base.EnemyController.IsFacingRight)
		{
			z = -20f;
		}
		base.EnemyController.Pivot.transform.localRotation = Quaternion.Euler(0f, 0f, z);
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x0001A10F File Offset: 0x0001830F
	private IEnumerator DropBonesDuringDash()
	{
		float timeBetweenDrops = this.m_dash_Attack_Duration / (float)this.m_bonesDroppedDuringDashCount;
		float angle = 80f;
		if (base.EnemyController.IsFacingRight)
		{
			angle = 110f;
		}
		float elapsedDashTime = 0f;
		int num;
		for (int i = 0; i < this.m_bonesDroppedDuringDashCount; i = num + 1)
		{
			this.FireProjectile("FlyingSkullBoneProjectile", 0, false, angle, 1f, true, true, true);
			yield return base.Wait(timeBetweenDrops, false);
			elapsedDashTime += timeBetweenDrops;
			num = i;
		}
		float num2 = this.m_dash_Attack_Duration - elapsedDashTime;
		if (num2 > 0f)
		{
			yield return base.Wait(num2, false);
		}
		yield break;
	}

	// Token: 0x04000ADD RID: 2781
	private const string SHOOT_PREP_AUDIO_EVENT_PATH = "event:/SFX/Enemies/sfx_enemy_flyingSkull_shoot_prep";

	// Token: 0x04000ADE RID: 2782
	private const string SHOOT_AUDIO_EVENT_PATH = "event:/SFX/Enemies/sfx_enemy_flyingSkull_shoot";

	// Token: 0x04000ADF RID: 2783
	private const string DASH_PREP_AUDIO_EVENT_PATH = "event:/SFX/Enemies/sfx_enemy_flyingSkull_dash_prep";

	// Token: 0x04000AE0 RID: 2784
	private const string DASH_AUDIO_EVENT_PATH = "event:/SFX/Enemies/sfx_enemy_flyingSkull_dash";

	// Token: 0x04000AE1 RID: 2785
	protected int m_NearBone_Angle = 7;

	// Token: 0x04000AE2 RID: 2786
	protected int m_FarBoneAngle = 20;

	// Token: 0x04000AE3 RID: 2787
	private const string BONE_PROJECTILE = "FlyingSkullBoneProjectile";

	// Token: 0x04000AE4 RID: 2788
	protected const string SINGLE_TELL_INTRO = "SingleShot_Tell_Intro";

	// Token: 0x04000AE5 RID: 2789
	protected const string SINGLE_TELL_HOLD = "SingleShot_Tell_Hold";

	// Token: 0x04000AE6 RID: 2790
	protected const string SINGLE_ATTACK_INTRO = "SingleShot_Attack_Intro";

	// Token: 0x04000AE7 RID: 2791
	protected const string SINGLE_ATTACK_HOLD = "SingleShot_Attack_Hold";

	// Token: 0x04000AE8 RID: 2792
	protected const string SINGLE_ATTACK_EXIT = "SingleShot_Exit";

	// Token: 0x04000AE9 RID: 2793
	protected const string MULTI_TELL_INTRO = "MultiShot_Tell_Intro";

	// Token: 0x04000AEA RID: 2794
	protected const string MULTI_TELL_HOLD = "MultiShot_Tell_Hold";

	// Token: 0x04000AEB RID: 2795
	protected const string MULTI_ATTACK_INTRO = "MultiShot_Attack_Intro";

	// Token: 0x04000AEC RID: 2796
	protected const string MULTI_ATTACK_HOLD = "MultiShot_Attack_Hold";

	// Token: 0x04000AED RID: 2797
	protected const string MULTI_ATTACK_EXIT = "MultiShot_Exit";
}
