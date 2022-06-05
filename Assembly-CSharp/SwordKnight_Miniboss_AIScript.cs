using System;
using RLAudio;

// Token: 0x0200013C RID: 316
public class SwordKnight_Miniboss_AIScript : SwordKnight_Basic_AIScript
{
	// Token: 0x1700055A RID: 1370
	// (get) Token: 0x060009FB RID: 2555 RVA: 0x0001FEDB File Offset: 0x0001E0DB
	protected override float m_slash_TellIntroAndHold_Delay
	{
		get
		{
			return 1.75f;
		}
	}

	// Token: 0x1700055B RID: 1371
	// (get) Token: 0x060009FC RID: 2556 RVA: 0x0001FEE2 File Offset: 0x0001E0E2
	protected override float m_slash_Attack_Speed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700055C RID: 1372
	// (get) Token: 0x060009FD RID: 2557 RVA: 0x0001FEE9 File Offset: 0x0001E0E9
	protected override float m_slash_Attack_Duration
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x1700055D RID: 1373
	// (get) Token: 0x060009FE RID: 2558 RVA: 0x0001FEF0 File Offset: 0x0001E0F0
	protected override float m_cricket_Exit_ForceIdle
	{
		get
		{
			return 0.65f;
		}
	}

	// Token: 0x1700055E RID: 1374
	// (get) Token: 0x060009FF RID: 2559 RVA: 0x0001FEF7 File Offset: 0x0001E0F7
	protected override float m_cricket_AttackCD
	{
		get
		{
			return 8.5f;
		}
	}

	// Token: 0x1700055F RID: 1375
	// (get) Token: 0x06000A00 RID: 2560 RVA: 0x0001FEFE File Offset: 0x0001E0FE
	protected override int m_cricket_Attack_ProjectileAmount
	{
		get
		{
			return 250;
		}
	}

	// Token: 0x17000560 RID: 1376
	// (get) Token: 0x06000A01 RID: 2561 RVA: 0x0001FF05 File Offset: 0x0001E105
	protected override float m_cricket_Attack_ProjectileDelay
	{
		get
		{
			return 2.275f;
		}
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x0001FF0C File Offset: 0x0001E10C
	protected override void PlayDeathAnimAudio()
	{
		AudioManager.PlayOneShot(this, "event:/SFX/Enemies/sfx_statueBoss_death_flash", base.gameObject.transform.position);
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x0001FF29 File Offset: 0x0001E129
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.EnemyController.StatusEffectController.AddStatusEffectImmunity(StatusEffectType.Enemy_Freeze);
	}

	// Token: 0x04000E9F RID: 3743
	private const string MURMUR_DEATH_SCREEN_FLASH_AUDIO_PATH = "event:/SFX/Enemies/sfx_statueBoss_death_flash";
}
