using System;
using RLAudio;

// Token: 0x0200022B RID: 555
public class SwordKnight_Miniboss_AIScript : SwordKnight_Basic_AIScript
{
	// Token: 0x17000728 RID: 1832
	// (get) Token: 0x06000F6A RID: 3946 RVA: 0x00004565 File Offset: 0x00002765
	protected override float m_slash_TellIntroAndHold_Delay
	{
		get
		{
			return 1.75f;
		}
	}

	// Token: 0x17000729 RID: 1833
	// (get) Token: 0x06000F6B RID: 3947 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float m_slash_Attack_Speed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700072A RID: 1834
	// (get) Token: 0x06000F6C RID: 3948 RVA: 0x00004A00 File Offset: 0x00002C00
	protected override float m_slash_Attack_Duration
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x1700072B RID: 1835
	// (get) Token: 0x06000F6D RID: 3949 RVA: 0x00006780 File Offset: 0x00004980
	protected override float m_cricket_Exit_ForceIdle
	{
		get
		{
			return 0.65f;
		}
	}

	// Token: 0x1700072C RID: 1836
	// (get) Token: 0x06000F6E RID: 3950 RVA: 0x000086B1 File Offset: 0x000068B1
	protected override float m_cricket_AttackCD
	{
		get
		{
			return 8.5f;
		}
	}

	// Token: 0x1700072D RID: 1837
	// (get) Token: 0x06000F6F RID: 3951 RVA: 0x000086B8 File Offset: 0x000068B8
	protected override int m_cricket_Attack_ProjectileAmount
	{
		get
		{
			return 250;
		}
	}

	// Token: 0x1700072E RID: 1838
	// (get) Token: 0x06000F70 RID: 3952 RVA: 0x000086BF File Offset: 0x000068BF
	protected override float m_cricket_Attack_ProjectileDelay
	{
		get
		{
			return 2.275f;
		}
	}

	// Token: 0x06000F71 RID: 3953 RVA: 0x000086C6 File Offset: 0x000068C6
	protected override void PlayDeathAnimAudio()
	{
		AudioManager.PlayOneShot(this, "event:/SFX/Enemies/sfx_statueBoss_death_flash", base.gameObject.transform.position);
	}

	// Token: 0x06000F72 RID: 3954 RVA: 0x000086E3 File Offset: 0x000068E3
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.EnemyController.StatusEffectController.AddStatusEffectImmunity(StatusEffectType.Enemy_Freeze);
	}

	// Token: 0x0400129D RID: 4765
	private const string MURMUR_DEATH_SCREEN_FLASH_AUDIO_PATH = "event:/SFX/Enemies/sfx_statueBoss_death_flash";
}
