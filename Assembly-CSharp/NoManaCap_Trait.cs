using System;
using Rewired;
using UnityEngine;

// Token: 0x02000355 RID: 853
public class NoManaCap_Trait : BaseTrait, IDamageObj
{
	// Token: 0x17000DD8 RID: 3544
	// (get) Token: 0x06002060 RID: 8288 RVA: 0x000667B7 File Offset: 0x000649B7
	public override TraitType TraitType
	{
		get
		{
			return TraitType.NoManaCap;
		}
	}

	// Token: 0x17000DD9 RID: 3545
	// (get) Token: 0x06002061 RID: 8289 RVA: 0x000667BE File Offset: 0x000649BE
	public string RelicDamageTypeString
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000DDA RID: 3546
	// (get) Token: 0x06002062 RID: 8290 RVA: 0x000667C1 File Offset: 0x000649C1
	public float BaseDamage
	{
		get
		{
			return (float)this.m_playerController.ActualMaxHealth * 0.02f;
		}
	}

	// Token: 0x17000DDB RID: 3547
	// (get) Token: 0x06002063 RID: 8291 RVA: 0x000667D5 File Offset: 0x000649D5
	public float ActualDamage
	{
		get
		{
			return this.BaseDamage;
		}
	}

	// Token: 0x17000DDC RID: 3548
	// (get) Token: 0x06002064 RID: 8292 RVA: 0x000667DD File Offset: 0x000649DD
	public float ActualCritChance
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000DDD RID: 3549
	// (get) Token: 0x06002065 RID: 8293 RVA: 0x000667E4 File Offset: 0x000649E4
	public float ActualCritDamage
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000DDE RID: 3550
	// (get) Token: 0x06002066 RID: 8294 RVA: 0x000667EB File Offset: 0x000649EB
	public Vector2 ExternalKnockbackMod
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x17000DDF RID: 3551
	// (get) Token: 0x06002067 RID: 8295 RVA: 0x000667F2 File Offset: 0x000649F2
	// (set) Token: 0x06002068 RID: 8296 RVA: 0x000667FA File Offset: 0x000649FA
	public float BaseKnockbackStrength { get; set; }

	// Token: 0x17000DE0 RID: 3552
	// (get) Token: 0x06002069 RID: 8297 RVA: 0x00066803 File Offset: 0x00064A03
	public float ActualKnockbackStrength
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000DE1 RID: 3553
	// (get) Token: 0x0600206A RID: 8298 RVA: 0x0006680A File Offset: 0x00064A0A
	// (set) Token: 0x0600206B RID: 8299 RVA: 0x00066812 File Offset: 0x00064A12
	public float BaseStunStrength { get; set; }

	// Token: 0x17000DE2 RID: 3554
	// (get) Token: 0x0600206C RID: 8300 RVA: 0x0006681B File Offset: 0x00064A1B
	public float ActualStunStrength
	{
		get
		{
			return this.BaseStunStrength;
		}
	}

	// Token: 0x17000DE3 RID: 3555
	// (get) Token: 0x0600206D RID: 8301 RVA: 0x00066823 File Offset: 0x00064A23
	public StrikeType StrikeType
	{
		get
		{
			return StrikeType.Blunt;
		}
	}

	// Token: 0x17000DE4 RID: 3556
	// (get) Token: 0x0600206E RID: 8302 RVA: 0x00066827 File Offset: 0x00064A27
	public bool IsDotDamage
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000DE5 RID: 3557
	// (get) Token: 0x0600206F RID: 8303 RVA: 0x0006682A File Offset: 0x00064A2A
	public StatusEffectType[] StatusEffectTypes
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000DE6 RID: 3558
	// (get) Token: 0x06002070 RID: 8304 RVA: 0x0006682D File Offset: 0x00064A2D
	public float[] StatusEffectDurations
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06002071 RID: 8305 RVA: 0x00066830 File Offset: 0x00064A30
	private void Start()
	{
		this.m_playerController = PlayerManager.GetPlayerController();
		this.m_damageTickStartTime = Time.time;
	}

	// Token: 0x06002072 RID: 8306 RVA: 0x00066848 File Offset: 0x00064A48
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

	// Token: 0x06002074 RID: 8308 RVA: 0x00066903 File Offset: 0x00064B03
	GameObject IDamageObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04001C55 RID: 7253
	private global::PlayerController m_playerController;

	// Token: 0x04001C56 RID: 7254
	private float m_damageTickStartTime;
}
