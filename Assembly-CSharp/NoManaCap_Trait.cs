using System;
using Rewired;
using UnityEngine;

// Token: 0x020005C1 RID: 1473
public class NoManaCap_Trait : BaseTrait, IDamageObj
{
	// Token: 0x17001237 RID: 4663
	// (get) Token: 0x06002DC9 RID: 11721 RVA: 0x0001926C File Offset: 0x0001746C
	public override TraitType TraitType
	{
		get
		{
			return TraitType.NoManaCap;
		}
	}

	// Token: 0x17001238 RID: 4664
	// (get) Token: 0x06002DCA RID: 11722 RVA: 0x0000F49B File Offset: 0x0000D69B
	public string RelicDamageTypeString
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17001239 RID: 4665
	// (get) Token: 0x06002DCB RID: 11723 RVA: 0x00019273 File Offset: 0x00017473
	public float BaseDamage
	{
		get
		{
			return (float)this.m_playerController.ActualMaxHealth * 0.02f;
		}
	}

	// Token: 0x1700123A RID: 4666
	// (get) Token: 0x06002DCC RID: 11724 RVA: 0x00019287 File Offset: 0x00017487
	public float ActualDamage
	{
		get
		{
			return this.BaseDamage;
		}
	}

	// Token: 0x1700123B RID: 4667
	// (get) Token: 0x06002DCD RID: 11725 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float ActualCritChance
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700123C RID: 4668
	// (get) Token: 0x06002DCE RID: 11726 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float ActualCritDamage
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700123D RID: 4669
	// (get) Token: 0x06002DCF RID: 11727 RVA: 0x00005FA3 File Offset: 0x000041A3
	public Vector2 ExternalKnockbackMod
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x1700123E RID: 4670
	// (get) Token: 0x06002DD0 RID: 11728 RVA: 0x0001928F File Offset: 0x0001748F
	// (set) Token: 0x06002DD1 RID: 11729 RVA: 0x00019297 File Offset: 0x00017497
	public float BaseKnockbackStrength { get; set; }

	// Token: 0x1700123F RID: 4671
	// (get) Token: 0x06002DD2 RID: 11730 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float ActualKnockbackStrength
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17001240 RID: 4672
	// (get) Token: 0x06002DD3 RID: 11731 RVA: 0x000192A0 File Offset: 0x000174A0
	// (set) Token: 0x06002DD4 RID: 11732 RVA: 0x000192A8 File Offset: 0x000174A8
	public float BaseStunStrength { get; set; }

	// Token: 0x17001241 RID: 4673
	// (get) Token: 0x06002DD5 RID: 11733 RVA: 0x000192B1 File Offset: 0x000174B1
	public float ActualStunStrength
	{
		get
		{
			return this.BaseStunStrength;
		}
	}

	// Token: 0x17001242 RID: 4674
	// (get) Token: 0x06002DD6 RID: 11734 RVA: 0x000046FA File Offset: 0x000028FA
	public StrikeType StrikeType
	{
		get
		{
			return StrikeType.Blunt;
		}
	}

	// Token: 0x17001243 RID: 4675
	// (get) Token: 0x06002DD7 RID: 11735 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public bool IsDotDamage
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17001244 RID: 4676
	// (get) Token: 0x06002DD8 RID: 11736 RVA: 0x0000F49B File Offset: 0x0000D69B
	public StatusEffectType[] StatusEffectTypes
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17001245 RID: 4677
	// (get) Token: 0x06002DD9 RID: 11737 RVA: 0x0000F49B File Offset: 0x0000D69B
	public float[] StatusEffectDurations
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06002DDA RID: 11738 RVA: 0x000192B9 File Offset: 0x000174B9
	private void Start()
	{
		this.m_playerController = PlayerManager.GetPlayerController();
		this.m_damageTickStartTime = Time.time;
	}

	// Token: 0x06002DDB RID: 11739 RVA: 0x000C74E4 File Offset: 0x000C56E4
	private void Update()
	{
		if (this.m_playerController != null && this.m_playerController.CurrentMana > (float)this.m_playerController.ActualMaxMana && ReInput.isReady)
		{
			if (RewiredMapController.CurrentGameInputMode == GameInputMode.Game && RewiredMapController.IsCurrentMapEnabled)
			{
				if (Time.time > this.m_damageTickStartTime + 1f)
				{
					this.m_damageTickStartTime = Time.time;
					this.m_playerController.DisableArmor = true;
					this.m_playerController.CharacterHitResponse.StartHitResponse(this.m_playerController.gameObject, this, -1f, false, true);
					this.m_playerController.DisableArmor = false;
					return;
				}
			}
			else
			{
				this.m_damageTickStartTime = Time.time;
			}
		}
	}

	// Token: 0x06002DDD RID: 11741 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IDamageObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040025D9 RID: 9689
	private global::PlayerController m_playerController;

	// Token: 0x040025DA RID: 9690
	private float m_damageTickStartTime;
}
