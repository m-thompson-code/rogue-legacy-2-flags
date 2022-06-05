using System;
using UnityEngine;

// Token: 0x0200069F RID: 1695
public class Breakable_VoidDashOnly : Breakable, IDamageObj
{
	// Token: 0x170013D3 RID: 5075
	// (get) Token: 0x06003410 RID: 13328 RVA: 0x0000F49B File Offset: 0x0000D69B
	public string RelicDamageTypeString
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170013D4 RID: 5076
	// (get) Token: 0x06003411 RID: 13329 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float BaseDamage
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170013D5 RID: 5077
	// (get) Token: 0x06003412 RID: 13330 RVA: 0x0001C925 File Offset: 0x0001AB25
	public float ActualDamage
	{
		get
		{
			return Hazard_EV.GetDamageAmount(PlayerManager.GetCurrentPlayerRoom());
		}
	}

	// Token: 0x170013D6 RID: 5078
	// (get) Token: 0x06003413 RID: 13331 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float ActualCritChance
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170013D7 RID: 5079
	// (get) Token: 0x06003414 RID: 13332 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float ActualCritDamage
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170013D8 RID: 5080
	// (get) Token: 0x06003415 RID: 13333 RVA: 0x0001484F File Offset: 0x00012A4F
	public Vector2 ExternalKnockbackMod
	{
		get
		{
			return Vector2.one;
		}
	}

	// Token: 0x170013D9 RID: 5081
	// (get) Token: 0x06003416 RID: 13334 RVA: 0x0001C931 File Offset: 0x0001AB31
	// (set) Token: 0x06003417 RID: 13335 RVA: 0x0001C939 File Offset: 0x0001AB39
	public float BaseKnockbackStrength { get; set; }

	// Token: 0x170013DA RID: 5082
	// (get) Token: 0x06003418 RID: 13336 RVA: 0x00003C70 File Offset: 0x00001E70
	public float ActualKnockbackStrength
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170013DB RID: 5083
	// (get) Token: 0x06003419 RID: 13337 RVA: 0x0001C942 File Offset: 0x0001AB42
	// (set) Token: 0x0600341A RID: 13338 RVA: 0x0001C94A File Offset: 0x0001AB4A
	public float BaseStunStrength { get; set; }

	// Token: 0x170013DC RID: 5084
	// (get) Token: 0x0600341B RID: 13339 RVA: 0x00003C70 File Offset: 0x00001E70
	public float ActualStunStrength
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170013DD RID: 5085
	// (get) Token: 0x0600341C RID: 13340 RVA: 0x000046FA File Offset: 0x000028FA
	public StrikeType StrikeType
	{
		get
		{
			return StrikeType.Blunt;
		}
	}

	// Token: 0x170013DE RID: 5086
	// (get) Token: 0x0600341D RID: 13341 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public bool IsDotDamage
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170013DF RID: 5087
	// (get) Token: 0x0600341E RID: 13342 RVA: 0x0000F49B File Offset: 0x0000D69B
	public StatusEffectType[] StatusEffectTypes
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170013E0 RID: 5088
	// (get) Token: 0x0600341F RID: 13343 RVA: 0x0000F49B File Offset: 0x0000D69B
	public float[] StatusEffectDurations
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06003420 RID: 13344 RVA: 0x0001C953 File Offset: 0x0001AB53
	protected override void Awake()
	{
		base.Awake();
		this.m_hitboxController.RepeatHitDuration = 0f;
		this.m_voidDissolve = base.GetComponent<VoidDissolveComponent>();
	}

	// Token: 0x06003421 RID: 13345 RVA: 0x0001C977 File Offset: 0x0001AB77
	protected override void TriggerCollision(IDamageObj damageObj)
	{
		if (PlayerManager.GetPlayerController().CharacterDash.IsDashing && SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockVoidDash) > 0)
		{
			base.TriggerCollision(damageObj);
		}
	}

	// Token: 0x06003422 RID: 13346 RVA: 0x0001C9A0 File Offset: 0x0001ABA0
	protected override void Break(IDamageObj damageObj)
	{
		base.Break(damageObj);
		if (this.m_voidDissolve)
		{
			this.m_voidDissolve.StartDissolve(true);
		}
	}

	// Token: 0x06003423 RID: 13347 RVA: 0x0001C9C2 File Offset: 0x0001ABC2
	public override void ForceBrokenState(bool isBroken)
	{
		if (isBroken)
		{
			this.m_voidDissolve.ForceDissolved();
		}
		base.ForceBrokenState(isBroken);
	}

	// Token: 0x06003425 RID: 13349 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IDamageObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002A38 RID: 10808
	private VoidDissolveComponent m_voidDissolve;
}
