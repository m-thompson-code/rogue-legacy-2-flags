using System;
using UnityEngine;

// Token: 0x020003F6 RID: 1014
public class Breakable_VoidDashOnly : Breakable, IDamageObj
{
	// Token: 0x17000F24 RID: 3876
	// (get) Token: 0x060025A2 RID: 9634 RVA: 0x0007C6CE File Offset: 0x0007A8CE
	public string RelicDamageTypeString
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000F25 RID: 3877
	// (get) Token: 0x060025A3 RID: 9635 RVA: 0x0007C6D1 File Offset: 0x0007A8D1
	public float BaseDamage
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000F26 RID: 3878
	// (get) Token: 0x060025A4 RID: 9636 RVA: 0x0007C6D8 File Offset: 0x0007A8D8
	public float ActualDamage
	{
		get
		{
			return Hazard_EV.GetDamageAmount(PlayerManager.GetCurrentPlayerRoom());
		}
	}

	// Token: 0x17000F27 RID: 3879
	// (get) Token: 0x060025A5 RID: 9637 RVA: 0x0007C6E4 File Offset: 0x0007A8E4
	public float ActualCritChance
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000F28 RID: 3880
	// (get) Token: 0x060025A6 RID: 9638 RVA: 0x0007C6EB File Offset: 0x0007A8EB
	public float ActualCritDamage
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000F29 RID: 3881
	// (get) Token: 0x060025A7 RID: 9639 RVA: 0x0007C6F2 File Offset: 0x0007A8F2
	public Vector2 ExternalKnockbackMod
	{
		get
		{
			return Vector2.one;
		}
	}

	// Token: 0x17000F2A RID: 3882
	// (get) Token: 0x060025A8 RID: 9640 RVA: 0x0007C6F9 File Offset: 0x0007A8F9
	// (set) Token: 0x060025A9 RID: 9641 RVA: 0x0007C701 File Offset: 0x0007A901
	public float BaseKnockbackStrength { get; set; }

	// Token: 0x17000F2B RID: 3883
	// (get) Token: 0x060025AA RID: 9642 RVA: 0x0007C70A File Offset: 0x0007A90A
	public float ActualKnockbackStrength
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000F2C RID: 3884
	// (get) Token: 0x060025AB RID: 9643 RVA: 0x0007C711 File Offset: 0x0007A911
	// (set) Token: 0x060025AC RID: 9644 RVA: 0x0007C719 File Offset: 0x0007A919
	public float BaseStunStrength { get; set; }

	// Token: 0x17000F2D RID: 3885
	// (get) Token: 0x060025AD RID: 9645 RVA: 0x0007C722 File Offset: 0x0007A922
	public float ActualStunStrength
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000F2E RID: 3886
	// (get) Token: 0x060025AE RID: 9646 RVA: 0x0007C729 File Offset: 0x0007A929
	public StrikeType StrikeType
	{
		get
		{
			return StrikeType.Blunt;
		}
	}

	// Token: 0x17000F2F RID: 3887
	// (get) Token: 0x060025AF RID: 9647 RVA: 0x0007C72D File Offset: 0x0007A92D
	public bool IsDotDamage
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000F30 RID: 3888
	// (get) Token: 0x060025B0 RID: 9648 RVA: 0x0007C730 File Offset: 0x0007A930
	public StatusEffectType[] StatusEffectTypes
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000F31 RID: 3889
	// (get) Token: 0x060025B1 RID: 9649 RVA: 0x0007C733 File Offset: 0x0007A933
	public float[] StatusEffectDurations
	{
		get
		{
			return null;
		}
	}

	// Token: 0x060025B2 RID: 9650 RVA: 0x0007C736 File Offset: 0x0007A936
	protected override void Awake()
	{
		base.Awake();
		this.m_hitboxController.RepeatHitDuration = 0f;
		this.m_voidDissolve = base.GetComponent<VoidDissolveComponent>();
	}

	// Token: 0x060025B3 RID: 9651 RVA: 0x0007C75A File Offset: 0x0007A95A
	protected override void TriggerCollision(IDamageObj damageObj)
	{
		if (PlayerManager.GetPlayerController().CharacterDash.IsDashing && SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockVoidDash) > 0)
		{
			base.TriggerCollision(damageObj);
		}
	}

	// Token: 0x060025B4 RID: 9652 RVA: 0x0007C783 File Offset: 0x0007A983
	protected override void Break(IDamageObj damageObj)
	{
		base.Break(damageObj);
		if (this.m_voidDissolve)
		{
			this.m_voidDissolve.StartDissolve(true);
		}
	}

	// Token: 0x060025B5 RID: 9653 RVA: 0x0007C7A5 File Offset: 0x0007A9A5
	public override void ForceBrokenState(bool isBroken)
	{
		if (isBroken)
		{
			this.m_voidDissolve.ForceDissolved();
		}
		base.ForceBrokenState(isBroken);
	}

	// Token: 0x060025B7 RID: 9655 RVA: 0x0007C7C4 File Offset: 0x0007A9C4
	GameObject IDamageObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04001F97 RID: 8087
	private VoidDissolveComponent m_voidDissolve;
}
