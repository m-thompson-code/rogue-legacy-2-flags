using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using Rewired;
using RLAudio;
using RL_Windows;
using SceneManagement_RL;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020002AA RID: 682
public class PlayerController : BaseCharacterController, IDamageObj, IWeaponOnEnterHitResponse, IHitResponse, IHasProjectileNameArray, IAudioEventEmitter
{
	// Token: 0x17000C07 RID: 3079
	// (get) Token: 0x06001A45 RID: 6725 RVA: 0x00052FFC File Offset: 0x000511FC
	public string[] ProjectileNameArray
	{
		get
		{
			return this.m_projectileNameArray;
		}
	}

	// Token: 0x17000C08 RID: 3080
	// (get) Token: 0x06001A46 RID: 6726 RVA: 0x00053004 File Offset: 0x00051204
	public string RelicDamageTypeString
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000C09 RID: 3081
	// (get) Token: 0x06001A47 RID: 6727 RVA: 0x00053007 File Offset: 0x00051207
	// (set) Token: 0x06001A48 RID: 6728 RVA: 0x0005300F File Offset: 0x0005120F
	public GameObject RangeBonusDamageCurseIndicatorGO
	{
		get
		{
			return this.m_rangeBonusRelicIndicatorGO;
		}
		set
		{
			this.m_rangeBonusRelicIndicatorGO = value;
		}
	}

	// Token: 0x17000C0A RID: 3082
	// (get) Token: 0x06001A49 RID: 6729 RVA: 0x00053018 File Offset: 0x00051218
	// (set) Token: 0x06001A4A RID: 6730 RVA: 0x00053020 File Offset: 0x00051220
	public bool IsMushroomBig { get; private set; }

	// Token: 0x17000C0B RID: 3083
	// (get) Token: 0x06001A4B RID: 6731 RVA: 0x00053029 File Offset: 0x00051229
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x17000C0C RID: 3084
	// (get) Token: 0x06001A4C RID: 6732 RVA: 0x00053031 File Offset: 0x00051231
	// (set) Token: 0x06001A4D RID: 6733 RVA: 0x00053039 File Offset: 0x00051239
	public float TimeEnteredRoom { get; private set; }

	// Token: 0x17000C0D RID: 3085
	// (get) Token: 0x06001A4E RID: 6734 RVA: 0x00053042 File Offset: 0x00051242
	// (set) Token: 0x06001A4F RID: 6735 RVA: 0x0005304A File Offset: 0x0005124A
	public GameObject FollowTargetGO
	{
		get
		{
			return this.m_followTargetGO;
		}
		set
		{
			this.m_followTargetGO = value;
		}
	}

	// Token: 0x17000C0E RID: 3086
	// (get) Token: 0x06001A50 RID: 6736 RVA: 0x00053053 File Offset: 0x00051253
	// (set) Token: 0x06001A51 RID: 6737 RVA: 0x00053060 File Offset: 0x00051260
	public bool IsInteractIconVisible
	{
		get
		{
			return this.m_interactIconController.IsIconVisible;
		}
		set
		{
			this.m_interactIconController.SetIconVisible(value);
		}
	}

	// Token: 0x17000C0F RID: 3087
	// (get) Token: 0x06001A52 RID: 6738 RVA: 0x0005306E File Offset: 0x0005126E
	// (set) Token: 0x06001A53 RID: 6739 RVA: 0x00053076 File Offset: 0x00051276
	public bool DisableDoorBlock { get; set; }

	// Token: 0x17000C10 RID: 3088
	// (get) Token: 0x06001A54 RID: 6740 RVA: 0x0005307F File Offset: 0x0005127F
	public StatusEffectType[] StatusEffectTypes
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000C11 RID: 3089
	// (get) Token: 0x06001A55 RID: 6741 RVA: 0x00053082 File Offset: 0x00051282
	public float[] StatusEffectDurations
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000C12 RID: 3090
	// (get) Token: 0x06001A56 RID: 6742 RVA: 0x00053085 File Offset: 0x00051285
	// (set) Token: 0x06001A57 RID: 6743 RVA: 0x0005308D File Offset: 0x0005128D
	public bool IsSpearSpinning { get; set; }

	// Token: 0x17000C13 RID: 3091
	// (get) Token: 0x06001A58 RID: 6744 RVA: 0x00053096 File Offset: 0x00051296
	// (set) Token: 0x06001A59 RID: 6745 RVA: 0x0005309E File Offset: 0x0005129E
	public bool IsBlocking { get; set; }

	// Token: 0x17000C14 RID: 3092
	// (get) Token: 0x06001A5A RID: 6746 RVA: 0x000530A7 File Offset: 0x000512A7
	public bool IsPerfectBlocking
	{
		get
		{
			return this.IsBlocking && Time.time < this.BlockStartTime + 0.135f;
		}
	}

	// Token: 0x17000C15 RID: 3093
	// (get) Token: 0x06001A5B RID: 6747 RVA: 0x000530C6 File Offset: 0x000512C6
	// (set) Token: 0x06001A5C RID: 6748 RVA: 0x000530CE File Offset: 0x000512CE
	public bool CloakInterrupted { get; set; }

	// Token: 0x17000C16 RID: 3094
	// (get) Token: 0x06001A5D RID: 6749 RVA: 0x000530D7 File Offset: 0x000512D7
	// (set) Token: 0x06001A5E RID: 6750 RVA: 0x000530DF File Offset: 0x000512DF
	public float BlockStartTime { get; set; }

	// Token: 0x17000C17 RID: 3095
	// (get) Token: 0x06001A5F RID: 6751 RVA: 0x000530E8 File Offset: 0x000512E8
	public bool IsDotDamage
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000C18 RID: 3096
	// (get) Token: 0x06001A60 RID: 6752 RVA: 0x000530EB File Offset: 0x000512EB
	public IRelayLink<PlayerDeathEventArgs> OnPlayerDeathRelay
	{
		get
		{
			return this.m_onPlayerDeathRelay.link;
		}
	}

	// Token: 0x17000C19 RID: 3097
	// (get) Token: 0x06001A61 RID: 6753 RVA: 0x000530F8 File Offset: 0x000512F8
	public IRelayLink<ManaChangeEventArgs> ManaChangeRelay
	{
		get
		{
			return this.m_manaChangeRelay.link;
		}
	}

	// Token: 0x17000C1A RID: 3098
	// (get) Token: 0x06001A62 RID: 6754 RVA: 0x00053105 File Offset: 0x00051305
	public override float BaseScaleToOffsetWith
	{
		get
		{
			return 1.4f;
		}
	}

	// Token: 0x17000C1B RID: 3099
	// (get) Token: 0x06001A63 RID: 6755 RVA: 0x0005310C File Offset: 0x0005130C
	// (set) Token: 0x06001A64 RID: 6756 RVA: 0x00053114 File Offset: 0x00051314
	public float BaseResolve
	{
		get
		{
			return this.m_baseResolve;
		}
		private set
		{
			this.m_baseResolve = value;
		}
	}

	// Token: 0x17000C1C RID: 3100
	// (get) Token: 0x06001A65 RID: 6757 RVA: 0x00053120 File Offset: 0x00051320
	public float ActualResolve
	{
		get
		{
			float num = 0f;
			SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(SoulShopType.MinimumResolveBlock);
			if (!soulShopObj.IsNativeNull())
			{
				num += soulShopObj.CurrentStatGain;
			}
			num += EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.MinimumResolve);
			float num2 = this.BaseResolve;
			if (ChallengeManager.IsInChallenge)
			{
				num2 = 2.5f;
			}
			return Mathf.Clamp((num2 + this.ResolveAdd) * (1f + this.ResolveMod), num, float.MaxValue);
		}
	}

	// Token: 0x17000C1D RID: 3101
	// (get) Token: 0x06001A66 RID: 6758 RVA: 0x00053192 File Offset: 0x00051392
	// (set) Token: 0x06001A67 RID: 6759 RVA: 0x0005319A File Offset: 0x0005139A
	public float ResolveAdd { get; private set; }

	// Token: 0x17000C1E RID: 3102
	// (get) Token: 0x06001A68 RID: 6760 RVA: 0x000531A3 File Offset: 0x000513A3
	// (set) Token: 0x06001A69 RID: 6761 RVA: 0x000531AB File Offset: 0x000513AB
	public float ResolveMod { get; private set; }

	// Token: 0x17000C1F RID: 3103
	// (get) Token: 0x06001A6A RID: 6762 RVA: 0x000531B4 File Offset: 0x000513B4
	// (set) Token: 0x06001A6B RID: 6763 RVA: 0x000531BC File Offset: 0x000513BC
	public virtual int SpellOrbs { get; set; }

	// Token: 0x17000C20 RID: 3104
	// (get) Token: 0x06001A6C RID: 6764 RVA: 0x000531C5 File Offset: 0x000513C5
	// (set) Token: 0x06001A6D RID: 6765 RVA: 0x000531CD File Offset: 0x000513CD
	public bool DisableArmor { get; set; }

	// Token: 0x17000C21 RID: 3105
	// (get) Token: 0x06001A6E RID: 6766 RVA: 0x000531D6 File Offset: 0x000513D6
	// (set) Token: 0x06001A6F RID: 6767 RVA: 0x000531DE File Offset: 0x000513DE
	public virtual float BaseDexterity
	{
		get
		{
			return this.m_baseDexterity;
		}
		set
		{
			this.m_baseDexterity = Mathf.Clamp(value, 0f, float.MaxValue);
		}
	}

	// Token: 0x17000C22 RID: 3106
	// (get) Token: 0x06001A70 RID: 6768 RVA: 0x000531F8 File Offset: 0x000513F8
	public virtual float ActualDexterity
	{
		get
		{
			float num = Mathf.Clamp((this.BaseDexterity + this.DexterityAdd + this.DexterityTemporaryAdd) * (1f + this.DexterityMod + this.DexterityTemporaryMod), 0f, float.MaxValue);
			if (ChallengeManager.IsInChallenge)
			{
				num = ChallengeManager.ApplyStatCap(num, true);
			}
			return num;
		}
	}

	// Token: 0x17000C23 RID: 3107
	// (get) Token: 0x06001A71 RID: 6769 RVA: 0x0005324D File Offset: 0x0005144D
	public override float ActualCritChance
	{
		get
		{
			return Mathf.Clamp(this.CritChanceAdd + this.CritChanceTemporaryAdd + 0f, 0f, 100f);
		}
	}

	// Token: 0x17000C24 RID: 3108
	// (get) Token: 0x06001A72 RID: 6770 RVA: 0x00053271 File Offset: 0x00051471
	// (set) Token: 0x06001A73 RID: 6771 RVA: 0x00053279 File Offset: 0x00051479
	public float DexterityAdd { get; set; }

	// Token: 0x17000C25 RID: 3109
	// (get) Token: 0x06001A74 RID: 6772 RVA: 0x00053282 File Offset: 0x00051482
	// (set) Token: 0x06001A75 RID: 6773 RVA: 0x0005328A File Offset: 0x0005148A
	public float DexterityTemporaryAdd { get; set; }

	// Token: 0x17000C26 RID: 3110
	// (get) Token: 0x06001A76 RID: 6774 RVA: 0x00053293 File Offset: 0x00051493
	// (set) Token: 0x06001A77 RID: 6775 RVA: 0x0005329B File Offset: 0x0005149B
	public float DexterityMod { get; set; }

	// Token: 0x17000C27 RID: 3111
	// (get) Token: 0x06001A78 RID: 6776 RVA: 0x000532A4 File Offset: 0x000514A4
	// (set) Token: 0x06001A79 RID: 6777 RVA: 0x000532AC File Offset: 0x000514AC
	public float DexterityTemporaryMod { get; set; }

	// Token: 0x17000C28 RID: 3112
	// (get) Token: 0x06001A7A RID: 6778 RVA: 0x000532B5 File Offset: 0x000514B5
	// (set) Token: 0x06001A7B RID: 6779 RVA: 0x000532BD File Offset: 0x000514BD
	public float CritChanceAdd { get; set; }

	// Token: 0x17000C29 RID: 3113
	// (get) Token: 0x06001A7C RID: 6780 RVA: 0x000532C6 File Offset: 0x000514C6
	// (set) Token: 0x06001A7D RID: 6781 RVA: 0x000532CE File Offset: 0x000514CE
	public float CritChanceTemporaryAdd { get; set; }

	// Token: 0x17000C2A RID: 3114
	// (get) Token: 0x06001A7E RID: 6782 RVA: 0x000532D7 File Offset: 0x000514D7
	// (set) Token: 0x06001A7F RID: 6783 RVA: 0x000532DF File Offset: 0x000514DF
	public virtual float BaseCritDamage
	{
		get
		{
			return this.m_baseCritDamage;
		}
		set
		{
			this.m_baseCritDamage = Mathf.Clamp(value, 0f, float.MaxValue);
		}
	}

	// Token: 0x17000C2B RID: 3115
	// (get) Token: 0x06001A80 RID: 6784 RVA: 0x000532F7 File Offset: 0x000514F7
	public override float ActualCritDamage
	{
		get
		{
			return Mathf.Clamp(this.BaseCritDamage + this.CritDamageAdd + this.CritDamageTemporaryAdd, 0f, float.MaxValue);
		}
	}

	// Token: 0x17000C2C RID: 3116
	// (get) Token: 0x06001A81 RID: 6785 RVA: 0x0005331C File Offset: 0x0005151C
	// (set) Token: 0x06001A82 RID: 6786 RVA: 0x00053324 File Offset: 0x00051524
	public virtual float CritDamageAdd { get; set; }

	// Token: 0x17000C2D RID: 3117
	// (get) Token: 0x06001A83 RID: 6787 RVA: 0x0005332D File Offset: 0x0005152D
	// (set) Token: 0x06001A84 RID: 6788 RVA: 0x00053335 File Offset: 0x00051535
	public virtual float CritDamageTemporaryAdd { get; set; }

	// Token: 0x17000C2E RID: 3118
	// (get) Token: 0x06001A85 RID: 6789 RVA: 0x0005333E File Offset: 0x0005153E
	// (set) Token: 0x06001A86 RID: 6790 RVA: 0x00053346 File Offset: 0x00051546
	public virtual float BaseFocus
	{
		get
		{
			return this.m_baseMagicDexterity;
		}
		set
		{
			this.m_baseMagicDexterity = Mathf.Clamp(value, 0f, float.MaxValue);
		}
	}

	// Token: 0x17000C2F RID: 3119
	// (get) Token: 0x06001A87 RID: 6791 RVA: 0x00053360 File Offset: 0x00051560
	public virtual float ActualFocus
	{
		get
		{
			float num = Mathf.Clamp((this.BaseFocus + this.FocusAdd + this.FocusTemporaryAdd) * (1f + this.FocusMod + this.FocusTemporaryMod), 0f, float.MaxValue);
			if (ChallengeManager.IsInChallenge)
			{
				num = ChallengeManager.ApplyStatCap(num, true);
			}
			return num;
		}
	}

	// Token: 0x17000C30 RID: 3120
	// (get) Token: 0x06001A88 RID: 6792 RVA: 0x000533B5 File Offset: 0x000515B5
	public virtual float ActualMagicCritChance
	{
		get
		{
			return Mathf.Clamp(this.MagicCritChanceAdd + this.MagicCritChanceTemporaryAdd + 0f, 0f, 1f);
		}
	}

	// Token: 0x17000C31 RID: 3121
	// (get) Token: 0x06001A89 RID: 6793 RVA: 0x000533D9 File Offset: 0x000515D9
	// (set) Token: 0x06001A8A RID: 6794 RVA: 0x000533E1 File Offset: 0x000515E1
	public virtual float FocusAdd { get; set; }

	// Token: 0x17000C32 RID: 3122
	// (get) Token: 0x06001A8B RID: 6795 RVA: 0x000533EA File Offset: 0x000515EA
	// (set) Token: 0x06001A8C RID: 6796 RVA: 0x000533F2 File Offset: 0x000515F2
	public virtual float FocusTemporaryAdd { get; set; }

	// Token: 0x17000C33 RID: 3123
	// (get) Token: 0x06001A8D RID: 6797 RVA: 0x000533FB File Offset: 0x000515FB
	// (set) Token: 0x06001A8E RID: 6798 RVA: 0x00053403 File Offset: 0x00051603
	public float FocusMod { get; set; }

	// Token: 0x17000C34 RID: 3124
	// (get) Token: 0x06001A8F RID: 6799 RVA: 0x0005340C File Offset: 0x0005160C
	// (set) Token: 0x06001A90 RID: 6800 RVA: 0x00053414 File Offset: 0x00051614
	public float FocusTemporaryMod { get; set; }

	// Token: 0x17000C35 RID: 3125
	// (get) Token: 0x06001A91 RID: 6801 RVA: 0x0005341D File Offset: 0x0005161D
	// (set) Token: 0x06001A92 RID: 6802 RVA: 0x00053425 File Offset: 0x00051625
	public float MagicCritChanceAdd { get; set; }

	// Token: 0x17000C36 RID: 3126
	// (get) Token: 0x06001A93 RID: 6803 RVA: 0x0005342E File Offset: 0x0005162E
	// (set) Token: 0x06001A94 RID: 6804 RVA: 0x00053436 File Offset: 0x00051636
	public float MagicCritChanceTemporaryAdd { get; set; }

	// Token: 0x17000C37 RID: 3127
	// (get) Token: 0x06001A95 RID: 6805 RVA: 0x0005343F File Offset: 0x0005163F
	// (set) Token: 0x06001A96 RID: 6806 RVA: 0x00053447 File Offset: 0x00051647
	public float BaseMagicCritDamage
	{
		get
		{
			return this.m_baseMagicCritDamage;
		}
		set
		{
			this.m_baseMagicCritDamage = Mathf.Clamp(value, 0f, float.MaxValue);
		}
	}

	// Token: 0x17000C38 RID: 3128
	// (get) Token: 0x06001A97 RID: 6807 RVA: 0x0005345F File Offset: 0x0005165F
	public float ActualMagicCritDamage
	{
		get
		{
			return Mathf.Clamp(this.BaseMagicCritDamage + this.MagicCritDamageAdd + this.MagicCritDamageTemporaryAdd, 0f, float.MaxValue);
		}
	}

	// Token: 0x17000C39 RID: 3129
	// (get) Token: 0x06001A98 RID: 6808 RVA: 0x00053484 File Offset: 0x00051684
	// (set) Token: 0x06001A99 RID: 6809 RVA: 0x0005348C File Offset: 0x0005168C
	public float MagicCritDamageAdd { get; set; }

	// Token: 0x17000C3A RID: 3130
	// (get) Token: 0x06001A9A RID: 6810 RVA: 0x00053495 File Offset: 0x00051695
	// (set) Token: 0x06001A9B RID: 6811 RVA: 0x0005349D File Offset: 0x0005169D
	public float MagicCritDamageTemporaryAdd { get; set; }

	// Token: 0x17000C3B RID: 3131
	// (get) Token: 0x06001A9C RID: 6812 RVA: 0x000534A6 File Offset: 0x000516A6
	// (set) Token: 0x06001A9D RID: 6813 RVA: 0x000534AE File Offset: 0x000516AE
	public int BaseArmor
	{
		get
		{
			return this.m_baseArmor;
		}
		private set
		{
			this.m_baseArmor = Mathf.Clamp(value, 0, int.MaxValue);
		}
	}

	// Token: 0x17000C3C RID: 3132
	// (get) Token: 0x06001A9E RID: 6814 RVA: 0x000534C2 File Offset: 0x000516C2
	// (set) Token: 0x06001A9F RID: 6815 RVA: 0x000534CA File Offset: 0x000516CA
	public int CurrentArmor
	{
		get
		{
			return this.m_currentArmor;
		}
		set
		{
			this.m_currentArmor = Mathf.Clamp(value, 0, this.ActualArmor);
		}
	}

	// Token: 0x17000C3D RID: 3133
	// (get) Token: 0x06001AA0 RID: 6816 RVA: 0x000534DF File Offset: 0x000516DF
	public int ActualArmor
	{
		get
		{
			return Mathf.Clamp(this.BaseArmor + this.ArmorAdds, 0, int.MaxValue);
		}
	}

	// Token: 0x17000C3E RID: 3134
	// (get) Token: 0x06001AA1 RID: 6817 RVA: 0x000534F9 File Offset: 0x000516F9
	// (set) Token: 0x06001AA2 RID: 6818 RVA: 0x00053501 File Offset: 0x00051701
	public int ArmorAdds { get; set; }

	// Token: 0x17000C3F RID: 3135
	// (get) Token: 0x06001AA3 RID: 6819 RVA: 0x0005350A File Offset: 0x0005170A
	// (set) Token: 0x06001AA4 RID: 6820 RVA: 0x00053512 File Offset: 0x00051712
	public int CurrentExhaust
	{
		get
		{
			return this.m_currentExhaust;
		}
		set
		{
			this.m_currentExhaust = Mathf.Clamp(value, 0, 99);
		}
	}

	// Token: 0x06001AA5 RID: 6821 RVA: 0x00053524 File Offset: 0x00051724
	public void SetMasteryXP(int value, bool additive)
	{
		int runAccumulatedXP = SaveManager.PlayerSaveData.RunAccumulatedXP;
		int runAccumulatedXP2 = additive ? (runAccumulatedXP + value) : value;
		SaveManager.PlayerSaveData.RunAccumulatedXP = runAccumulatedXP2;
	}

	// Token: 0x17000C40 RID: 3136
	// (get) Token: 0x06001AA6 RID: 6822 RVA: 0x00053554 File Offset: 0x00051754
	public override float ActualStrength
	{
		get
		{
			float num = base.ActualStrength;
			if (ChallengeManager.IsInChallenge)
			{
				num = ChallengeManager.ApplyStatCap(num, false);
				num *= 1f + ChallengeManager.GetActiveHandicapMod();
			}
			return num;
		}
	}

	// Token: 0x17000C41 RID: 3137
	// (get) Token: 0x06001AA7 RID: 6823 RVA: 0x00053588 File Offset: 0x00051788
	public override float ActualMagic
	{
		get
		{
			float num = base.ActualMagic;
			if (ChallengeManager.IsInChallenge)
			{
				num = ChallengeManager.ApplyStatCap(num, false);
				num *= 1f + ChallengeManager.GetActiveHandicapMod();
			}
			return num;
		}
	}

	// Token: 0x17000C42 RID: 3138
	// (get) Token: 0x06001AA8 RID: 6824 RVA: 0x000535BA File Offset: 0x000517BA
	// (set) Token: 0x06001AA9 RID: 6825 RVA: 0x000535C2 File Offset: 0x000517C2
	public float CachedHealthOverride { get; set; }

	// Token: 0x17000C43 RID: 3139
	// (get) Token: 0x06001AAA RID: 6826 RVA: 0x000535CB File Offset: 0x000517CB
	// (set) Token: 0x06001AAB RID: 6827 RVA: 0x000535D3 File Offset: 0x000517D3
	public float CachedManaOverride { get; set; }

	// Token: 0x17000C44 RID: 3140
	// (get) Token: 0x06001AAC RID: 6828 RVA: 0x000535DC File Offset: 0x000517DC
	// (set) Token: 0x06001AAD RID: 6829 RVA: 0x000535E4 File Offset: 0x000517E4
	public int BaseVitality
	{
		get
		{
			return this.m_baseVitality;
		}
		set
		{
			this.m_baseVitality = Mathf.Clamp(value, 0, int.MaxValue);
		}
	}

	// Token: 0x17000C45 RID: 3141
	// (get) Token: 0x06001AAE RID: 6830 RVA: 0x000535F8 File Offset: 0x000517F8
	public int ClassModdedMaxHealth
	{
		get
		{
			return Mathf.CeilToInt((float)this.VitalityModdedMaxHealth * this.CharacterClass.ClassData.PassiveData.MaxHPMod);
		}
	}

	// Token: 0x17000C46 RID: 3142
	// (get) Token: 0x06001AAF RID: 6831 RVA: 0x0005361C File Offset: 0x0005181C
	public int ActualVitality
	{
		get
		{
			int num = this.BaseVitality + this.VitalityAdd;
			num = Mathf.CeilToInt((float)num * (1f + this.VitalityMod));
			if (ChallengeManager.IsInChallenge)
			{
				num = (int)ChallengeManager.ApplyStatCap((float)num, false);
				num = Mathf.CeilToInt((float)num * (1f + ChallengeManager.GetActiveHandicapMod()));
			}
			return num;
		}
	}

	// Token: 0x17000C47 RID: 3143
	// (get) Token: 0x06001AB0 RID: 6832 RVA: 0x00053672 File Offset: 0x00051872
	// (set) Token: 0x06001AB1 RID: 6833 RVA: 0x0005367A File Offset: 0x0005187A
	public int VitalityAdd { get; set; }

	// Token: 0x17000C48 RID: 3144
	// (get) Token: 0x06001AB2 RID: 6834 RVA: 0x00053683 File Offset: 0x00051883
	// (set) Token: 0x06001AB3 RID: 6835 RVA: 0x0005368B File Offset: 0x0005188B
	public float VitalityMod { get; set; }

	// Token: 0x17000C49 RID: 3145
	// (get) Token: 0x06001AB4 RID: 6836 RVA: 0x00053694 File Offset: 0x00051894
	public override int BaseMaxHealth
	{
		get
		{
			return Mathf.CeilToInt((float)this.BaseVitality * 10f);
		}
	}

	// Token: 0x17000C4A RID: 3146
	// (get) Token: 0x06001AB5 RID: 6837 RVA: 0x000536A8 File Offset: 0x000518A8
	public int VitalityModdedMaxHealth
	{
		get
		{
			return Mathf.CeilToInt((float)this.ActualVitality * 10f);
		}
	}

	// Token: 0x17000C4B RID: 3147
	// (get) Token: 0x06001AB6 RID: 6838 RVA: 0x000536BC File Offset: 0x000518BC
	public override int ActualMaxHealth
	{
		get
		{
			if (TraitManager.IsTraitActive(TraitType.OneHitDeath))
			{
				return 1;
			}
			float classModdedMaxHealth = (float)this.ClassModdedMaxHealth;
			float traitMaxHealthMod = this.TraitMaxHealthMod;
			float relicMaxHealthMod = this.RelicMaxHealthMod;
			float maxHealthMod = base.MaxHealthMod;
			float num = 1f;
			num += traitMaxHealthMod + relicMaxHealthMod + maxHealthMod;
			float num2 = Mathf.Clamp(1f - this.ActualResolve, 0f, 1f);
			num2 *= 1f;
			return Mathf.Clamp(Mathf.CeilToInt((classModdedMaxHealth * num + 100f * num) * (1f - num2)), 1, int.MaxValue);
		}
	}

	// Token: 0x17000C4C RID: 3148
	// (get) Token: 0x06001AB7 RID: 6839 RVA: 0x0005374B File Offset: 0x0005194B
	// (set) Token: 0x06001AB8 RID: 6840 RVA: 0x00053753 File Offset: 0x00051953
	public float TraitMaxHealthMod { get; set; }

	// Token: 0x17000C4D RID: 3149
	// (get) Token: 0x06001AB9 RID: 6841 RVA: 0x0005375C File Offset: 0x0005195C
	// (set) Token: 0x06001ABA RID: 6842 RVA: 0x00053764 File Offset: 0x00051964
	public float RelicMaxHealthMod { get; set; }

	// Token: 0x17000C4E RID: 3150
	// (get) Token: 0x06001ABB RID: 6843 RVA: 0x0005376D File Offset: 0x0005196D
	// (set) Token: 0x06001ABC RID: 6844 RVA: 0x00053775 File Offset: 0x00051975
	public int BaseMaxMana { get; set; }

	// Token: 0x17000C4F RID: 3151
	// (get) Token: 0x06001ABD RID: 6845 RVA: 0x0005377E File Offset: 0x0005197E
	public int ClassModdedMaxMana
	{
		get
		{
			return Mathf.CeilToInt((float)this.BaseMaxMana * this.CharacterClass.ClassData.PassiveData.MaxManaMod * (1f + this.EquipmentMaxManaMod));
		}
	}

	// Token: 0x17000C50 RID: 3152
	// (get) Token: 0x06001ABE RID: 6846 RVA: 0x000537AF File Offset: 0x000519AF
	public int ActualMaxMana
	{
		get
		{
			return Mathf.Clamp(Mathf.CeilToInt((float)this.ClassModdedMaxMana * (1f + this.TraitMaxManaMod)) + this.PostModMaxManaAdd, 1, int.MaxValue);
		}
	}

	// Token: 0x17000C51 RID: 3153
	// (get) Token: 0x06001ABF RID: 6847 RVA: 0x000537DC File Offset: 0x000519DC
	// (set) Token: 0x06001AC0 RID: 6848 RVA: 0x000537E4 File Offset: 0x000519E4
	public int PostModMaxManaAdd { get; set; }

	// Token: 0x17000C52 RID: 3154
	// (get) Token: 0x06001AC1 RID: 6849 RVA: 0x000537ED File Offset: 0x000519ED
	// (set) Token: 0x06001AC2 RID: 6850 RVA: 0x000537F5 File Offset: 0x000519F5
	public float TraitMaxManaMod { get; set; }

	// Token: 0x17000C53 RID: 3155
	// (get) Token: 0x06001AC3 RID: 6851 RVA: 0x000537FE File Offset: 0x000519FE
	// (set) Token: 0x06001AC4 RID: 6852 RVA: 0x00053806 File Offset: 0x00051A06
	public float EquipmentMaxManaMod { get; set; }

	// Token: 0x17000C54 RID: 3156
	// (get) Token: 0x06001AC5 RID: 6853 RVA: 0x0005380F File Offset: 0x00051A0F
	// (set) Token: 0x06001AC6 RID: 6854 RVA: 0x00053817 File Offset: 0x00051A17
	public float ManaRegenMod { get; set; }

	// Token: 0x17000C55 RID: 3157
	// (get) Token: 0x06001AC7 RID: 6855 RVA: 0x00053820 File Offset: 0x00051A20
	// (set) Token: 0x06001AC8 RID: 6856 RVA: 0x00053828 File Offset: 0x00051A28
	public float CurrentMana { get; private set; }

	// Token: 0x06001AC9 RID: 6857 RVA: 0x00053834 File Offset: 0x00051A34
	public void SetMana(float value, bool additive, bool runEvents, bool canExceedMax = false)
	{
		float currentMana = this.CurrentMana;
		float num = value;
		if (additive)
		{
			num = this.CurrentMana + value;
		}
		if (TraitManager.IsTraitActive(TraitType.NoManaCap))
		{
			canExceedMax = true;
		}
		if (!canExceedMax && num > currentMana && num > (float)this.ActualMaxMana)
		{
			if (currentMana > (float)this.ActualMaxMana)
			{
				num = currentMana;
			}
			else
			{
				num = Mathf.Min(num, (float)this.ActualMaxMana);
			}
		}
		this.CurrentMana = Mathf.Max(num, 0f);
		if (runEvents)
		{
			if (this.m_manaChangeEventArgs != null)
			{
				this.m_manaChangeEventArgs.Initialise(this, this.CurrentMana, currentMana);
			}
			if (this.m_manaChangeRelay != null)
			{
				this.m_manaChangeRelay.Dispatch(this.m_manaChangeEventArgs);
			}
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerManaChange, this, this.m_manaChangeEventArgs);
		}
	}

	// Token: 0x06001ACA RID: 6858 RVA: 0x000538EC File Offset: 0x00051AEC
	public override void SetHealth(float value, bool additive, bool runEvents)
	{
		if ((SaveManager.PlayerSaveData.InHubTown || SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround) && !ChallengeManager.IsInChallenge && ((this.CurrentHealth + value <= 0f && additive) || (value <= 0f && !additive)))
		{
			value = 1f;
			additive = false;
		}
		if (additive && value > 0f && TraitManager.IsTraitActive(TraitType.MegaHealth))
		{
			value = 0f;
		}
		base.SetHealth(value, additive, runEvents);
		if (runEvents)
		{
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerHealthChange, this, this.m_healthChangeEventArgs);
		}
	}

	// Token: 0x17000C56 RID: 3158
	// (get) Token: 0x06001ACB RID: 6859 RVA: 0x00053978 File Offset: 0x00051B78
	public int CurrentManaAsInt
	{
		get
		{
			return Mathf.CeilToInt(this.CurrentMana);
		}
	}

	// Token: 0x17000C57 RID: 3159
	// (get) Token: 0x06001ACC RID: 6860 RVA: 0x00053985 File Offset: 0x00051B85
	// (set) Token: 0x06001ACD RID: 6861 RVA: 0x0005398C File Offset: 0x00051B8C
	public override Vector2 ExternalKnockbackMod
	{
		get
		{
			return Vector2.one;
		}
		set
		{
		}
	}

	// Token: 0x17000C58 RID: 3160
	// (get) Token: 0x06001ACE RID: 6862 RVA: 0x0005398E File Offset: 0x00051B8E
	// (set) Token: 0x06001ACF RID: 6863 RVA: 0x00053996 File Offset: 0x00051B96
	public int BaseRuneWeight { get; set; }

	// Token: 0x17000C59 RID: 3161
	// (get) Token: 0x06001AD0 RID: 6864 RVA: 0x0005399F File Offset: 0x00051B9F
	public int ActualRuneWeight
	{
		get
		{
			return this.BaseRuneWeight + this.RuneWeightAdds;
		}
	}

	// Token: 0x17000C5A RID: 3162
	// (get) Token: 0x06001AD1 RID: 6865 RVA: 0x000539AE File Offset: 0x00051BAE
	// (set) Token: 0x06001AD2 RID: 6866 RVA: 0x000539B6 File Offset: 0x00051BB6
	public int RuneWeightAdds { get; set; }

	// Token: 0x17000C5B RID: 3163
	// (get) Token: 0x06001AD3 RID: 6867 RVA: 0x000539BF File Offset: 0x00051BBF
	// (set) Token: 0x06001AD4 RID: 6868 RVA: 0x000539C7 File Offset: 0x00051BC7
	public int BaseAllowedEquipmentWeight { get; set; }

	// Token: 0x17000C5C RID: 3164
	// (get) Token: 0x06001AD5 RID: 6869 RVA: 0x000539D0 File Offset: 0x00051BD0
	public int ActualAllowedEquipmentWeight
	{
		get
		{
			return this.BaseAllowedEquipmentWeight + this.AllowedEquipmentWeightAdds;
		}
	}

	// Token: 0x17000C5D RID: 3165
	// (get) Token: 0x06001AD6 RID: 6870 RVA: 0x000539DF File Offset: 0x00051BDF
	// (set) Token: 0x06001AD7 RID: 6871 RVA: 0x000539E7 File Offset: 0x00051BE7
	public int AllowedEquipmentWeightAdds { get; set; }

	// Token: 0x17000C5E RID: 3166
	// (get) Token: 0x06001AD8 RID: 6872 RVA: 0x000539F0 File Offset: 0x00051BF0
	// (set) Token: 0x06001AD9 RID: 6873 RVA: 0x000539F8 File Offset: 0x00051BF8
	public float AbilityCoolDownMod { get; set; }

	// Token: 0x17000C5F RID: 3167
	// (get) Token: 0x06001ADA RID: 6874 RVA: 0x00053A01 File Offset: 0x00051C01
	// (set) Token: 0x06001ADB RID: 6875 RVA: 0x00053A0E File Offset: 0x00051C0E
	public float BaseMovementSpeed
	{
		get
		{
			return this.m_characterMove.MovementSpeed;
		}
		set
		{
			this.m_characterMove.MovementSpeed = value;
		}
	}

	// Token: 0x17000C60 RID: 3168
	// (get) Token: 0x06001ADC RID: 6876 RVA: 0x00053A1C File Offset: 0x00051C1C
	public float ActualMovementSpeed
	{
		get
		{
			return this.m_characterMove.MovementSpeed;
		}
	}

	// Token: 0x17000C61 RID: 3169
	// (get) Token: 0x06001ADD RID: 6877 RVA: 0x00053A29 File Offset: 0x00051C29
	// (set) Token: 0x06001ADE RID: 6878 RVA: 0x00053A36 File Offset: 0x00051C36
	public float MovementSpeedMod
	{
		get
		{
			return this.m_characterMove.MovementSpeedMultiplier;
		}
		set
		{
			this.m_characterMove.MovementSpeedMultiplier = Mathf.Max(0f, value);
		}
	}

	// Token: 0x17000C62 RID: 3170
	// (get) Token: 0x06001ADF RID: 6879 RVA: 0x00053A4E File Offset: 0x00051C4E
	public CharacterDash_RL CharacterDash
	{
		get
		{
			return this.m_characterDash;
		}
	}

	// Token: 0x17000C63 RID: 3171
	// (get) Token: 0x06001AE0 RID: 6880 RVA: 0x00053A56 File Offset: 0x00051C56
	public CharacterDownStrike_RL CharacterDownStrike
	{
		get
		{
			return this.m_characterDownStrike;
		}
	}

	// Token: 0x17000C64 RID: 3172
	// (get) Token: 0x06001AE1 RID: 6881 RVA: 0x00053A5E File Offset: 0x00051C5E
	public CharacterClass CharacterClass
	{
		get
		{
			return this.m_characterClass;
		}
	}

	// Token: 0x17000C65 RID: 3173
	// (get) Token: 0x06001AE2 RID: 6882 RVA: 0x00053A66 File Offset: 0x00051C66
	public CharacterHorizontalMovement_RL CharacterMove
	{
		get
		{
			return this.m_characterMove;
		}
	}

	// Token: 0x17000C66 RID: 3174
	// (get) Token: 0x06001AE3 RID: 6883 RVA: 0x00053A6E File Offset: 0x00051C6E
	public CharacterFlight_RL CharacterFlight
	{
		get
		{
			return this.m_characterFlight;
		}
	}

	// Token: 0x17000C67 RID: 3175
	// (get) Token: 0x06001AE4 RID: 6884 RVA: 0x00053A76 File Offset: 0x00051C76
	public CharacterJump_RL CharacterJump
	{
		get
		{
			return this.m_characterJump;
		}
	}

	// Token: 0x17000C68 RID: 3176
	// (get) Token: 0x06001AE5 RID: 6885 RVA: 0x00053A7E File Offset: 0x00051C7E
	public CastAbility_RL CastAbility
	{
		get
		{
			if (!this.m_characterAbilities)
			{
				this.m_characterAbilities = base.GetComponentInChildren<CastAbility_RL>();
			}
			return this.m_characterAbilities;
		}
	}

	// Token: 0x17000C69 RID: 3177
	// (get) Token: 0x06001AE6 RID: 6886 RVA: 0x00053A9F File Offset: 0x00051C9F
	public PlayerLookController LookController
	{
		get
		{
			return this.m_lookController;
		}
	}

	// Token: 0x17000C6A RID: 3178
	// (get) Token: 0x06001AE7 RID: 6887 RVA: 0x00053AA7 File Offset: 0x00051CA7
	// (set) Token: 0x06001AE8 RID: 6888 RVA: 0x00053AAF File Offset: 0x00051CAF
	public BaseRoom PreviouslyInRoom { get; private set; }

	// Token: 0x17000C6B RID: 3179
	// (get) Token: 0x06001AE9 RID: 6889 RVA: 0x00053AB8 File Offset: 0x00051CB8
	// (set) Token: 0x06001AEA RID: 6890 RVA: 0x00053AC0 File Offset: 0x00051CC0
	public BaseRoom CurrentlyInRoom
	{
		get
		{
			return this.m_currentlyInRoom;
		}
		set
		{
			this.PreviouslyInRoom = this.m_currentlyInRoom;
			this.m_currentlyInRoom = value;
		}
	}

	// Token: 0x17000C6C RID: 3180
	// (get) Token: 0x06001AEB RID: 6891 RVA: 0x00053AD5 File Offset: 0x00051CD5
	// (set) Token: 0x06001AEC RID: 6892 RVA: 0x00053ADD File Offset: 0x00051CDD
	public bool JustRolled
	{
		get
		{
			return this.m_justRolled;
		}
		set
		{
			if (this.m_rollCoroutine != null)
			{
				base.StopCoroutine(this.m_rollCoroutine);
			}
			this.m_justRolled = value;
			if (this.m_justRolled)
			{
				this.m_rollCoroutine = base.StartCoroutine(this.RollCoroutine(0f));
			}
		}
	}

	// Token: 0x17000C6D RID: 3181
	// (get) Token: 0x06001AED RID: 6893 RVA: 0x00053B19 File Offset: 0x00051D19
	// (set) Token: 0x06001AEE RID: 6894 RVA: 0x00053B20 File Offset: 0x00051D20
	public float BaseKnockbackStrength
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	// Token: 0x17000C6E RID: 3182
	// (get) Token: 0x06001AEF RID: 6895 RVA: 0x00053B22 File Offset: 0x00051D22
	public float ActualKnockbackStrength
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000C6F RID: 3183
	// (get) Token: 0x06001AF0 RID: 6896 RVA: 0x00053B29 File Offset: 0x00051D29
	// (set) Token: 0x06001AF1 RID: 6897 RVA: 0x00053B30 File Offset: 0x00051D30
	public float BaseStunStrength
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	// Token: 0x17000C70 RID: 3184
	// (get) Token: 0x06001AF2 RID: 6898 RVA: 0x00053B32 File Offset: 0x00051D32
	public float ActualStunStrength
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000C71 RID: 3185
	// (get) Token: 0x06001AF3 RID: 6899 RVA: 0x00053B39 File Offset: 0x00051D39
	public StrikeType StrikeType
	{
		get
		{
			return StrikeType.Blunt;
		}
	}

	// Token: 0x17000C72 RID: 3186
	// (get) Token: 0x06001AF4 RID: 6900 RVA: 0x00053B3D File Offset: 0x00051D3D
	// (set) Token: 0x06001AF5 RID: 6901 RVA: 0x00053B45 File Offset: 0x00051D45
	public Projectile_RL DamageAuraProjectile { get; set; }

	// Token: 0x06001AF6 RID: 6902 RVA: 0x00053B50 File Offset: 0x00051D50
	protected void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.EquippedChanged, this.m_onEquippedOrLevelChanged);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.EquipmentPurchasedLevelChanged, this.m_onEquippedOrLevelChanged);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.RuneEquippedLevelChanged, this.m_onEquippedOrLevelChanged);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.RunePurchaseLevelChanged, this.m_onEquippedOrLevelChanged);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.RelicLevelChanged, this.m_onRelicChanged);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.UpdatePools, this.m_applyPermanentStatusEffects);
	}

	// Token: 0x06001AF7 RID: 6903 RVA: 0x00053BAC File Offset: 0x00051DAC
	protected void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.EquippedChanged, this.m_onEquippedOrLevelChanged);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.EquipmentPurchasedLevelChanged, this.m_onEquippedOrLevelChanged);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.RuneEquippedLevelChanged, this.m_onEquippedOrLevelChanged);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.RunePurchaseLevelChanged, this.m_onEquippedOrLevelChanged);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.RelicLevelChanged, this.m_onRelicChanged);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.UpdatePools, this.m_applyPermanentStatusEffects);
	}

	// Token: 0x06001AF8 RID: 6904 RVA: 0x00053C05 File Offset: 0x00051E05
	private IEnumerator RollCoroutine(float duration)
	{
		float startTime = Time.time;
		while (Time.time < startTime + duration)
		{
			yield return null;
		}
		this.JustRolled = false;
		yield break;
	}

	// Token: 0x06001AF9 RID: 6905 RVA: 0x00053C1B File Offset: 0x00051E1B
	private void OnEquippedOrLevelChanged(MonoBehaviour sender, EventArgs args)
	{
		this.InitializeAllMods(true, true);
	}

	// Token: 0x06001AFA RID: 6906 RVA: 0x00053C28 File Offset: 0x00051E28
	public float GetActualStatValue(PlayerStat stat)
	{
		switch (stat)
		{
		case PlayerStat.Vitality:
			return (float)this.ActualVitality;
		case PlayerStat.Strength:
			return this.ActualStrength;
		case PlayerStat.Magic:
			return this.ActualMagic;
		case PlayerStat.Dexterity:
			return this.ActualDexterity;
		case PlayerStat.Focus:
			return this.ActualFocus;
		default:
			throw new Exception("Attempting to scale off of an unknown PlayerStat");
		}
	}

	// Token: 0x06001AFB RID: 6907 RVA: 0x00053C7F File Offset: 0x00051E7F
	public void UpdateFrameAccumulatedXP(float amount)
	{
		this.m_updateAccumulatedXP += amount;
	}

	// Token: 0x06001AFC RID: 6908 RVA: 0x00053C8F File Offset: 0x00051E8F
	public void UpdateFrameAccumulatedLifeSteal(float amount)
	{
		this.m_updateAccumulatedLifeSteal += amount;
	}

	// Token: 0x06001AFD RID: 6909 RVA: 0x00053CA0 File Offset: 0x00051EA0
	private void LateUpdate()
	{
		if (this.m_updateAccumulatedXP > 0f)
		{
			if (this.m_xpPopup && this.m_xpPopup.gameObject.activeSelf)
			{
				this.m_xpPopup.gameObject.SetActive(false);
			}
			string text = string.Format(LocalizationManager.GetString("LOC_ID_XP_UI_XP_POPUP_1", false, false), this.m_updateAccumulatedXP);
			this.m_xpPopup = TextPopupManager.DisplayTextDefaultPos(TextPopupType.XPGain, text, this, true, false);
			this.m_updateAccumulatedXP = 0f;
		}
		if (this.m_updateAccumulatedLifeSteal > 0f)
		{
			string text2 = string.Format(LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_HEALTH_RESTORE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), (int)this.m_updateAccumulatedLifeSteal);
			TextPopupManager.DisplayTextDefaultPos(TextPopupType.HPGained, text2, this, true, true);
			this.m_updateAccumulatedLifeSteal = 0f;
		}
	}

	// Token: 0x06001AFE RID: 6910 RVA: 0x00053D78 File Offset: 0x00051F78
	protected override void OnJustGrounded()
	{
		if (EffectManager.AnimatorEffectsDisabled(this.m_animator))
		{
			return;
		}
		if (SceneLoader_RL.IsRunningTransitionWithLogic)
		{
			return;
		}
		if (RewiredMapController.IsInCutscene)
		{
			return;
		}
		RelicObj relic = SaveManager.PlayerSaveData.GetRelic(RelicType.LandShockwave);
		if (relic.Level > 0)
		{
			Projectile_RL projectile_RL = ProjectileManager.FireProjectile(base.gameObject, "RelicLandShockwaveProjectile", new Vector2(0f, 0f), true, 0f, 1f, false, true, true, true);
			projectile_RL.CastAbilityType = CastAbilityType.Talent;
			projectile_RL.MagicScale += 0.75f * (float)(relic.Level - 1);
			projectile_RL.ActualCritDamage = ProjectileManager.CalculateProjectileCritDamage(projectile_RL, true);
		}
	}

	// Token: 0x06001AFF RID: 6911 RVA: 0x00053E19 File Offset: 0x00052019
	private void OnRelicChanged(object sender, EventArgs args)
	{
		this.InitializeAbilities();
		this.InitializeAllMods(false, false);
	}

	// Token: 0x06001B00 RID: 6912 RVA: 0x00053E2C File Offset: 0x0005202C
	private void CheckForBiomeChange(BiomeType roomBiome)
	{
		if (this.CurrentlyInRoom == null || (this.CurrentlyInRoom != null && this.CurrentlyInRoom.BiomeType != roomBiome))
		{
			if (this.m_biomeEventArgs == null)
			{
				this.m_biomeEventArgs = new BiomeEventArgs();
			}
			BiomeType biome = BiomeType.None;
			if (this.CurrentlyInRoom != null)
			{
				biome = this.CurrentlyInRoom.BiomeType;
			}
			this.m_biomeEventArgs.SetBiome(biome);
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.BiomeExit, this, this.m_biomeEventArgs);
			this.m_biomeEventArgs.SetBiome(roomBiome);
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.BiomeEnter, this, this.m_biomeEventArgs);
		}
	}

	// Token: 0x06001B01 RID: 6913 RVA: 0x00053EC8 File Offset: 0x000520C8
	protected override void Awake()
	{
		base.Awake();
		this.m_characterDash = base.gameObject.GetComponent<CharacterDash_RL>();
		this.m_characterDownStrike = base.gameObject.GetComponent<CharacterDownStrike_RL>();
		this.m_characterAbilities = base.gameObject.GetComponent<CastAbility_RL>();
		this.m_characterMove = base.gameObject.GetComponent<CharacterHorizontalMovement_RL>();
		this.m_characterJump = base.gameObject.GetComponent<CharacterJump_RL>();
		this.m_characterFlight = base.gameObject.GetComponent<CharacterFlight_RL>();
		this.m_lookController = base.gameObject.GetComponentInChildren<PlayerLookController>();
		this.m_characterClass = base.gameObject.GetComponent<CharacterClass>();
		this.m_interactIconController = base.gameObject.GetComponentInChildren<InteractIconController>();
		this.m_playerDeathEventArgs = new PlayerDeathEventArgs(this, null);
		this.m_manaChangeEventArgs = new ManaChangeEventArgs(this, 0f, 0f);
		this.m_traitChangeEventArgs = new TraitChangedEventArgs(TraitType.None, TraitType.None);
		this.m_onHitAreaDamage_waitYield = new WaitRL_Yield(1f, false);
		this.m_noAttackDamageBonus_waitYield = new WaitRL_Yield(1f, false);
		CameraLayerController component = base.GetComponent<CameraLayerController>();
		component.SetCameraLayer(CameraLayer.Game);
		component.SetSubLayer(CameraLayerUtility.DefaultPlayerSubLayer, false);
		this.m_onEquippedOrLevelChanged = new Action<MonoBehaviour, EventArgs>(this.OnEquippedOrLevelChanged);
		this.m_onRelicChanged = new Action<MonoBehaviour, EventArgs>(this.OnRelicChanged);
		this.m_applyPermanentStatusEffects = new Action<MonoBehaviour, EventArgs>(this.ApplyPermanentStatusEffects);
	}

	// Token: 0x06001B02 RID: 6914 RVA: 0x00054014 File Offset: 0x00052214
	public override void ResetBaseValues()
	{
		base.BaseScale = 1.4f;
		this.BaseMaxMana = 100;
		this.BaseAllowedEquipmentWeight = 50;
		this.BaseRuneWeight = 50;
		this.BaseArmor = 0;
		this.BaseCritDamage = 1.1f;
		this.BaseMagicCritDamage = 1.1f;
		this.BaseResolve = 1f;
		base.BaseStrength = 15f;
		base.BaseMagic = 15f;
		this.BaseDexterity = 5f;
		this.BaseFocus = 5f;
		this.BaseVitality = 15;
		this.BaseStunDefense = -1f;
		this.BaseKnockbackDefense = -1f;
		this.MovementSpeedMod = 1f;
		base.StunDuration = 99f;
		base.BaseInvincibilityDuration = 1f;
		this.InitializeAbilities();
		base.ResetBaseValues();
	}

	// Token: 0x06001B03 RID: 6915 RVA: 0x000540E4 File Offset: 0x000522E4
	public void InitializeAbilities()
	{
		this.CharacterMove.WalkSpeed = 12f;
		float num = 1f + RuneLogicHelper.GetHastePercent();
		this.CharacterMove.WalkSpeed *= num;
		this.CharacterMove.MovementSpeed = this.CharacterMove.WalkSpeed;
		base.ControllerCorgi.DefaultParameters.SpeedAccelerationOnGround = 120f;
		base.ControllerCorgi.DefaultParameters.SpeedAccelerationInAir = 120f;
		this.CharacterJump.JumpHeight = 8.75f;
		this.CharacterJump.DoubleJumpHeight = 4.75f;
		this.CharacterJump.JumpReleaseForce = 4.5f;
		this.CharacterJump.JumpTimeWindow = 0.1375f;
		this.CharacterJump.CanJumpWhileDashing = true;
		int num2 = RuneLogicHelper.GetExtraJumps();
		num2 += (int)EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.ExtraJump);
		num2 += (int)EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.ExtraDashJump);
		int level = SaveManager.PlayerSaveData.GetRelic(RelicType.LowMultiJump).Level;
		if (level > 0)
		{
			num2 += level * 3;
			this.CharacterJump.JumpHeight /= 2f;
		}
		this.CharacterJump.NumberOfJumps = 1;
		if (this.CharacterJump.NumberOfJumps <= 1 && SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockDoubleJump) > 0)
		{
			this.CharacterJump.NumberOfJumps = 2;
		}
		this.CharacterJump.NumberOfJumps += num2;
		this.CharacterJump.ResetNumberOfJumps();
		this.CharacterDash.EnableOmnidash = false;
		if (TraitManager.IsTraitActive(TraitType.OmniDash))
		{
			this.CharacterDash.EnableOmnidash = true;
		}
		this.CharacterDash.DashDistance = 8f;
		this.CharacterDash.DashForce = 26f;
		this.CharacterDash.DashCooldown = 0f;
		int num3 = RuneLogicHelper.GetExtraDashes();
		num3 += (int)EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.ExtraDash);
		num3 += (int)EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.ExtraDashJump);
		this.CharacterDash.TotalDashesAllowed = 0;
		if (this.CharacterDash.TotalDashesAllowed <= 0 && SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockAirDash) > 0)
		{
			this.CharacterDash.TotalDashesAllowed = 1;
		}
		this.CharacterDash.TotalDashesAllowed += num3;
		this.CharacterDash.ResetNumberOfDashes();
		this.CharacterDownStrike.AttackSpeed = 32f;
		this.CharacterDownStrike.ForwardKickAngle = -55f;
		this.CharacterDownStrike.ForwardKickMinMaxAngle = Player_EV.DOWN_STRIKE_FORWARDKICK_MINMAX_ANGLE;
		this.CharacterDownStrike.DownKickMinMaxAngle = Player_EV.DOWN_STRIKE_DOWNKICK_MINMAX_ANGLE;
		this.CharacterDownStrike.AttackBounceHeight = 22f;
		this.CharacterDownStrike.BounceInputLockDuration = 0.275f;
		this.CharacterDownStrike.ResetsDoubleJump = false;
		this.CharacterDownStrike.ResetsDash = false;
	}

	// Token: 0x06001B04 RID: 6916 RVA: 0x00054390 File Offset: 0x00052590
	public void InitializeAllMods(bool resetHP, bool resetMP)
	{
		this.ResetMods();
		this.InitializeWeightMods();
		this.InitializeHealthMods();
		this.InitializeManaMods();
		this.InitializeStrengthMods();
		this.InitializeMagicMods();
		this.InitializeAbilityMods();
		this.InitializeInvincibilityMods();
		this.InitializeArmorMods();
		this.InitializeCritMods();
		this.InitializeExhaustMods();
		int value = (int)EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.Revives);
		if (SaveManager.PlayerSaveData.GetRelic(RelicType.ExtraLife_UnityUsed).Level <= 0 && !base.IsDead)
		{
			SaveManager.PlayerSaveData.GetRelic(RelicType.ExtraLife_Unity).SetLevel(value, false, false);
		}
		else
		{
			SaveManager.PlayerSaveData.GetRelic(RelicType.ExtraLife_Unity).SetLevel(0, false, false);
		}
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.RelicStatsChanged, this, this.m_extraLifeEventArgs);
		if (resetHP)
		{
			this.ResetHealth();
		}
		if (resetMP)
		{
			this.ResetMana();
		}
	}

	// Token: 0x06001B05 RID: 6917 RVA: 0x00054454 File Offset: 0x00052654
	public void InitializeHealthMods()
	{
		this.InitializeResolveHealthMods();
		this.InitializeVitalityMods();
		this.InitializeTraitHealthMods();
		this.InitializeRelicMaxHealthMods();
		this.InitializeMaxHealthMods();
		this.m_healthChangeEventArgs.Initialise(this, (float)this.ActualMaxHealth, (float)this.ActualMaxHealth);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerHealthChange, this, this.m_healthChangeEventArgs);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerResolveChanged, this, null);
	}

	// Token: 0x06001B06 RID: 6918 RVA: 0x000544B0 File Offset: 0x000526B0
	private void InitializeVitalityMods()
	{
		int num = 0;
		num += (int)EquipmentManager.GetTotalEquippedStatValue(EquipmentStatType.Vitality);
		num += SkillTreeLogicHelper.GetVitalityAdds();
		num += (int)EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.VitalityAdd);
		num += (int)Mastery_EV.GetTotalMasteryBonus(MasteryBonusType.Health_Up);
		this.VitalityAdd = num;
		float num2 = 0f;
		num2 += this.CharacterClass.ClassData.PassiveData.VitalityMod - 1f;
		num2 += Mastery_EV.GetTotalMasteryBonus(MasteryBonusType.Health_Up_Mod);
		num2 += EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.VitalityMod);
		this.VitalityMod = num2;
	}

	// Token: 0x06001B07 RID: 6919 RVA: 0x00054530 File Offset: 0x00052730
	private void InitializeTraitHealthMods()
	{
		float num = 0f;
		if (TraitManager.IsTraitActive(TraitType.BonusHealth))
		{
			num += 0.1f;
		}
		if (TraitManager.IsTraitActive(TraitType.InvulnDash))
		{
			num -= 0.5f;
		}
		if (TraitManager.IsTraitActive(TraitType.MagicBoost))
		{
			num -= 0.25f;
		}
		if (TraitManager.IsTraitActive(TraitType.DamageBoost))
		{
			num -= 0.25f;
		}
		if (TraitManager.IsTraitActive(TraitType.CantAttack))
		{
			num -= 0.6f;
		}
		if (TraitManager.IsTraitActive(TraitType.CanNowAttack))
		{
			num -= 0.6f;
		}
		if (TraitManager.IsTraitActive(TraitType.SmallHitbox))
		{
			num -= 0.25f;
		}
		if (TraitManager.IsTraitActive(TraitType.BonusMagicStrength))
		{
			num -= 0.5f;
		}
		if (TraitManager.IsTraitActive(TraitType.RevealAllChests))
		{
			num -= 0.100000024f;
		}
		if (TraitManager.IsTraitActive(TraitType.SuperHealer))
		{
			num -= 0f;
		}
		if (TraitManager.IsTraitActive(TraitType.OmniDash))
		{
			num -= 0.19999999f;
		}
		if (TraitManager.IsTraitActive(TraitType.BounceTerrain))
		{
			num -= 0.3f;
		}
		if (TraitManager.IsTraitActive(TraitType.MegaHealth))
		{
			num += 1f;
		}
		this.TraitMaxHealthMod = num;
	}

	// Token: 0x06001B08 RID: 6920 RVA: 0x00054650 File Offset: 0x00052850
	private void InitializeRelicMaxHealthMods()
	{
		float num = 0f;
		num += SaveManager.PlayerSaveData.TemporaryMaxHealthMods;
		num += (float)SaveManager.PlayerSaveData.GetRelic(RelicType.ReplacementRelic).Level * 0.1f;
		num += (float)SaveManager.PlayerSaveData.GetRelic(RelicType.FoodChallengeUsed).Level * 0.3f;
		this.RelicMaxHealthMod = num;
	}

	// Token: 0x06001B09 RID: 6921 RVA: 0x000546B4 File Offset: 0x000528B4
	private void InitializeResolveHealthMods()
	{
		float num = 0f;
		num -= SaveManager.PlayerSaveData.GetTotalRelicResolveCost();
		if (!ChallengeManager.IsInChallenge)
		{
			int num2 = EquipmentManager.GetWeightLevel();
			num2 = Mathf.Clamp(num2, 0, Equipment_EV.CRIT_DAMAGE_BONUS_PER_WEIGHT_LEVEL.Length - 1);
			float num3 = Equipment_EV.RESOLVE_BONUS_PER_WEIGHT_LEVEL[num2];
			num += num3;
		}
		num += RuneLogicHelper.GetResolveAdd();
		num += SkillTreeLogicHelper.GetResolveAdds();
		num += EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.Resolve);
		int num4 = BurdenManager.BurdenRequiredForNG(SaveManager.PlayerSaveData.NewGamePlusLevel);
		if (num4 < 0)
		{
			SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(SoulShopType.BurdenOverload);
			if (!soulShopObj.IsNativeNull())
			{
				num += soulShopObj.CurrentStatGain * (float)Mathf.Abs(num4);
			}
		}
		this.ResolveAdd = num;
		float num5 = 0f;
		if (SaveManager.PlayerSaveData.GetRelic(RelicType.GoldIntoResolve).Level > 0)
		{
			num5 += Economy_EV.GetGoldGainMod();
			num5 *= NPC_EV.GetArchitectGoldMod(-1);
		}
		this.ResolveMod = num5;
	}

	// Token: 0x06001B0A RID: 6922 RVA: 0x00054798 File Offset: 0x00052998
	private void InitializeMaxHealthMods()
	{
		float maxHealthMod = 0f;
		base.MaxHealthMod = maxHealthMod;
		if (this.CurrentHealth > (float)this.ActualMaxHealth)
		{
			this.SetHealth((float)this.ActualMaxHealth, false, false);
		}
	}

	// Token: 0x06001B0B RID: 6923 RVA: 0x000547D0 File Offset: 0x000529D0
	public void InitializeManaMods()
	{
		this.InitializeMaxManaAdds();
		this.InitializeEquipmentMaxManaMods();
		this.InitializeTraitMaxManaMods();
		this.InitializeManaRegenMods();
		this.m_manaChangeEventArgs.Initialise(this, (float)this.ActualMaxMana, (float)this.ActualMaxMana);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerManaChange, this, this.m_manaChangeEventArgs);
	}

	// Token: 0x06001B0C RID: 6924 RVA: 0x00054820 File Offset: 0x00052A20
	private void InitializeMaxManaAdds()
	{
		int num = 0;
		num += RuneLogicHelper.GetMaxManaFlat();
		int level = SaveManager.PlayerSaveData.GetRelic(RelicType.BonusMana).Level;
		num += 50 * level;
		RelicObj relic = SaveManager.PlayerSaveData.GetRelic(RelicType.SpellKillMaxMana);
		num += relic.IntValue;
		int level2 = SaveManager.PlayerSaveData.GetRelic(RelicType.FoodChallengeUsed).Level;
		num += 50 * level2;
		num += (int)EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.MaxMana);
		this.PostModMaxManaAdd = num;
	}

	// Token: 0x06001B0D RID: 6925 RVA: 0x000548A0 File Offset: 0x00052AA0
	private void InitializeTraitMaxManaMods()
	{
		float num = 0f;
		if (TraitManager.IsTraitActive(TraitType.DamageBoost))
		{
			num += 0f;
		}
		if (TraitManager.IsTraitActive(TraitType.MagicBoost))
		{
			num += 0.5f;
		}
		if (TraitManager.IsTraitActive(TraitType.BonusMagicStrength))
		{
			num += 0f;
		}
		this.TraitMaxManaMod = num;
	}

	// Token: 0x06001B0E RID: 6926 RVA: 0x000548F8 File Offset: 0x00052AF8
	private void InitializeEquipmentMaxManaMods()
	{
		float equipmentMaxManaMod = 0f;
		this.EquipmentMaxManaMod = equipmentMaxManaMod;
	}

	// Token: 0x06001B0F RID: 6927 RVA: 0x00054914 File Offset: 0x00052B14
	private void InitializeManaRegenMods()
	{
		float num = 0f;
		num += RuneLogicHelper.GetManaRegenPercent();
		num += EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.ManaRegenMod);
		int weightLevel = EquipmentManager.GetWeightLevel();
		num += Mathf.Clamp((float)weightLevel * --0f, -1f, 0f);
		this.ManaRegenMod = num;
	}

	// Token: 0x06001B10 RID: 6928 RVA: 0x00054964 File Offset: 0x00052B64
	public void InitializeStrengthMods()
	{
		int num = 0;
		num += (int)EquipmentManager.GetTotalEquippedStatValue(EquipmentStatType.Strength);
		num += SkillTreeLogicHelper.GetStrengthAdds();
		num += (int)EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.StrengthAdd);
		num += (int)Mastery_EV.GetTotalMasteryBonus(MasteryBonusType.Strength_Up);
		base.StrengthAdd = num;
		int strengthTemporaryAdd = 0;
		base.StrengthTemporaryAdd = strengthTemporaryAdd;
		float num2 = 0f;
		num2 += this.CharacterClass.ClassData.PassiveData.StrengthMod - 1f;
		num2 += Mastery_EV.GetTotalMasteryBonus(MasteryBonusType.Strength_Up_Mod);
		num2 += EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.StrengthMod);
		base.StrengthMod = num2;
	}

	// Token: 0x06001B11 RID: 6929 RVA: 0x000549EC File Offset: 0x00052BEC
	public void InitializeMagicMods()
	{
		int num = 0;
		num += (int)EquipmentManager.GetTotalEquippedStatValue(EquipmentStatType.Magic);
		num += SkillTreeLogicHelper.GetMagicAdds();
		num += (int)EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.MagicAdd);
		num += (int)Mastery_EV.GetTotalMasteryBonus(MasteryBonusType.Magic_Up);
		base.MagicAdd = num;
		int magicTemporaryAdd = 0;
		base.MagicTemporaryAdd = magicTemporaryAdd;
		float num2 = 0f;
		num2 += this.CharacterClass.ClassData.PassiveData.IntelligenceMod - 1f;
		num2 += Mastery_EV.GetTotalMasteryBonus(MasteryBonusType.Magic_Up_Mod);
		num2 += EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.MagicMod);
		base.MagicMod = num2;
	}

	// Token: 0x06001B12 RID: 6930 RVA: 0x00054A74 File Offset: 0x00052C74
	public void InitializeAbilityMods()
	{
		float num = 0f;
		num += SkillTreeLogicHelper.GetAbilityCooldownMods();
		this.AbilityCoolDownMod = num;
	}

	// Token: 0x06001B13 RID: 6931 RVA: 0x00054A98 File Offset: 0x00052C98
	public void InitializeInvincibilityMods()
	{
		float num = 0f;
		num += SkillTreeLogicHelper.GetInvulnTimeExtension();
		num += (float)SaveManager.PlayerSaveData.GetRelic(RelicType.ExtendInvuln).Level * 1.25f;
		base.InvincibilityDurationAdd = num;
	}

	// Token: 0x06001B14 RID: 6932 RVA: 0x00054AD8 File Offset: 0x00052CD8
	public void InitializeWeightMods()
	{
		int num = 0;
		num += SkillTreeLogicHelper.GetEquipWeightAdds();
		num += (int)EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.WeightReduction);
		num += (int)Mastery_EV.GetTotalMasteryBonus(MasteryBonusType.EquipmentWeight_Up);
		this.AllowedEquipmentWeightAdds = num;
		int num2 = 0;
		num2 += SkillTreeLogicHelper.GetRuneWeightAdds();
		num2 += (int)Mastery_EV.GetTotalMasteryBonus(MasteryBonusType.RuneWeight_Up);
		this.RuneWeightAdds = num2;
	}

	// Token: 0x06001B15 RID: 6933 RVA: 0x00054B28 File Offset: 0x00052D28
	public void InitializeExhaustMods()
	{
		int num = 0;
		num += SaveManager.PlayerSaveData.GetRelic(RelicType.AttackExhaust).Level * 25;
		this.CurrentExhaust = num;
	}

	// Token: 0x06001B16 RID: 6934 RVA: 0x00054B58 File Offset: 0x00052D58
	public void InitializeArmorMods()
	{
		int num = 0;
		num += (int)EquipmentManager.GetTotalEquippedStatValue(EquipmentStatType.Armor);
		num += SkillTreeLogicHelper.GetArmorAdds();
		num += (int)EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.Armor);
		num += (int)Mastery_EV.GetTotalMasteryBonus(MasteryBonusType.Armor_Add_Up);
		float num2 = 1f;
		num2 += this.CharacterClass.ClassData.PassiveData.ArmorMod - 1f;
		num2 += EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.ArmorMod);
		num2 += Mastery_EV.GetTotalMasteryBonus(MasteryBonusType.Armor_Add_Up_Mod);
		num2 += RuneLogicHelper.GetArmorRegenMod();
		this.ArmorAdds = (int)((float)num * num2);
	}

	// Token: 0x06001B17 RID: 6935 RVA: 0x00054BDC File Offset: 0x00052DDC
	public void InitializeCritMods()
	{
		float num = 0f;
		num += (float)((int)EquipmentManager.GetTotalEquippedStatValue(EquipmentStatType.Dexterity_Add));
		num += SkillTreeLogicHelper.GetDexterityAdds();
		num += (float)((int)EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.Dexterity_Add));
		num += (float)((int)Mastery_EV.GetTotalMasteryBonus(MasteryBonusType.Dexterity_Add));
		this.DexterityAdd = num;
		float dexterityTemporaryAdd = 0f;
		this.DexterityTemporaryAdd = dexterityTemporaryAdd;
		float num2 = 0f;
		num2 += this.CharacterClass.ClassData.PassiveData.DexterityMod - 1f;
		num2 += EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.DexterityMod);
		num2 += Mastery_EV.GetTotalMasteryBonus(MasteryBonusType.Dexterity_Mod);
		this.DexterityMod = num2;
		float dexterityTemporaryMod = 0f;
		this.DexterityTemporaryMod = dexterityTemporaryMod;
		float num3 = 0f;
		num3 += this.CharacterClass.ClassData.PassiveData.WeaponCritChanceAdd;
		num3 += SkillTreeLogicHelper.GetCritChanceAdds();
		num3 += RuneLogicHelper.GetWeaponCritChanceAdd();
		num3 += EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.FlatWeaponCritChance);
		this.CritChanceAdd = num3;
		float num4 = 0f;
		num4 += (float)SaveManager.PlayerSaveData.GetRelic(RelicType.AllCritChanceUp).Level * 0.1f;
		this.CritChanceTemporaryAdd = num4;
		float num5 = 0f;
		num5 += this.CharacterClass.ClassData.PassiveData.WeaponCritDamageAdd;
		num5 += EquipmentManager.GetTotalEquippedStatValue(EquipmentStatType.CritDamage);
		num5 += SkillTreeLogicHelper.GetCritDamageAdds();
		num5 += EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.CritDamage);
		num5 += (float)((int)Mastery_EV.GetTotalMasteryBonus(MasteryBonusType.WeaponCritDamage_Up));
		num5 += RuneLogicHelper.GetWeaponCritDamageAdd();
		int num6 = EquipmentManager.GetWeightLevel();
		num6 = Mathf.Clamp(num6, 0, Equipment_EV.CRIT_DAMAGE_BONUS_PER_WEIGHT_LEVEL.Length - 1);
		float num7 = Equipment_EV.CRIT_DAMAGE_BONUS_PER_WEIGHT_LEVEL[num6];
		num5 += num7;
		this.CritDamageAdd = num5;
		float num8 = 0f;
		num8 += (float)SaveManager.PlayerSaveData.GetRelic(RelicType.AllCritDamageUp).Level * 0.2f;
		this.CritDamageTemporaryAdd = num8;
		float num9 = 0f;
		num9 += (float)((int)EquipmentManager.GetTotalEquippedStatValue(EquipmentStatType.Focus_Add));
		num9 += SkillTreeLogicHelper.GetFocusAdds();
		num9 += (float)((int)EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.Focus_Add));
		num9 += (float)((int)Mastery_EV.GetTotalMasteryBonus(MasteryBonusType.Focus_Add));
		this.FocusAdd = num9;
		float focusTemporaryAdd = 0f;
		this.FocusTemporaryAdd = focusTemporaryAdd;
		float num10 = 0f;
		num10 += this.CharacterClass.ClassData.PassiveData.FocusMod - 1f;
		num10 += Mastery_EV.GetTotalMasteryBonus(MasteryBonusType.Focus_Mod);
		num10 += EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.FocusMod);
		this.FocusMod = num10;
		float focusTemporaryMod = 0f;
		this.FocusTemporaryMod = focusTemporaryMod;
		float num11 = 0f;
		num11 += this.CharacterClass.ClassData.PassiveData.MagicCritChanceAdd;
		num11 += SkillTreeLogicHelper.GetMagicCritChanceAdds();
		num11 += RuneLogicHelper.GetMagicCritChanceAdd();
		num11 += EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.FlatMagicCritChance);
		this.MagicCritChanceAdd = num11;
		float num12 = 0f;
		num12 += (float)SaveManager.PlayerSaveData.GetRelic(RelicType.AllCritChanceUp).Level * 0.1f;
		this.MagicCritChanceTemporaryAdd = num12;
		float num13 = 0f;
		num13 += this.CharacterClass.ClassData.PassiveData.MagicCritDamageAdd;
		num13 += EquipmentManager.GetTotalEquippedStatValue(EquipmentStatType.MagicCritDamage);
		num13 += SkillTreeLogicHelper.GetMagicCritDamageAdds();
		num13 += EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.MagicCritDamage);
		num13 += (float)((int)Mastery_EV.GetTotalMasteryBonus(MasteryBonusType.MagicCritDamage_Up));
		num13 += RuneLogicHelper.GetMagicCritDamageAdd();
		float num14 = Equipment_EV.MAGIC_CRIT_DAMAGE_BONUS_PER_WEIGHT_LEVEL[num6];
		num13 += num14;
		this.MagicCritDamageAdd = num13;
		float num15 = 0f;
		num15 += (float)SaveManager.PlayerSaveData.GetRelic(RelicType.AllCritDamageUp).Level * 0.2f;
		this.MagicCritDamageTemporaryAdd = num15;
	}

	// Token: 0x06001B18 RID: 6936 RVA: 0x00054F7C File Offset: 0x0005317C
	public override void ResetMods()
	{
		this.JustRolled = false;
		base.ResetMods();
		this.AbilityCoolDownMod = 0f;
		this.TraitMaxManaMod = 0f;
		this.PostModMaxManaAdd = 0;
		this.VitalityAdd = 0;
		this.TraitMaxHealthMod = 0f;
		this.RelicMaxHealthMod = 0f;
		base.InvincibilityDurationAdd = 0f;
		this.RuneWeightAdds = 0;
		this.AllowedEquipmentWeightAdds = 0;
		this.ArmorAdds = 0;
		this.DexterityAdd = 0f;
		this.DexterityMod = 0f;
		this.DexterityTemporaryAdd = 0f;
		this.DexterityTemporaryMod = 0f;
		this.CritDamageAdd = 0f;
		this.CritDamageTemporaryAdd = 0f;
		this.CritChanceAdd = 0f;
		this.CritChanceTemporaryAdd = 0f;
		this.FocusAdd = 0f;
		this.FocusMod = 0f;
		this.FocusTemporaryAdd = 0f;
		this.FocusTemporaryMod = 0f;
		this.MagicCritDamageAdd = 0f;
		this.MagicCritDamageTemporaryAdd = 0f;
		this.MagicCritChanceAdd = 0f;
		this.MagicCritChanceTemporaryAdd = 0f;
	}

	// Token: 0x06001B19 RID: 6937 RVA: 0x000550A0 File Offset: 0x000532A0
	public void ResetGravityAndMovement()
	{
		if (base.ControllerCorgi)
		{
			base.FallMultiplierOverride = 1f;
			base.AscentMultiplierOverride = 1f;
			this.CharacterMove.MovementSpeedMultiplier = 1f;
			if (TraitManager.IsTraitActive(TraitType.LowerGravity))
			{
				LowerGravity_Trait lowerGravity_Trait = TraitManager.GetActiveTrait(TraitType.LowerGravity) as LowerGravity_Trait;
				if (lowerGravity_Trait)
				{
					base.AscentMultiplierOverride *= 1f;
					base.FallMultiplierOverride *= 0.35f;
					lowerGravity_Trait.LowerGravityApplied = true;
				}
			}
		}
	}

	// Token: 0x06001B1A RID: 6938 RVA: 0x00055130 File Offset: 0x00053330
	public override void ResetCharacter()
	{
		base.ResetCharacter();
		base.Pivot.SetActive(true);
		PlayerManager.GetPlayerController().ControllerCorgi.enabled = true;
		base.TakesNoDamage = false;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.GoldSavedChanged, this, null);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.GoldChanged, this, null);
		SkillTreeManager.ResetCachedTotalSkills();
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.SkillLevelChanged, this, null);
		SaveManager.PlayerSaveData.TemporaryHeirloomList.Clear();
		if (base.ControllerCorgi)
		{
			base.ControllerCorgi.GravityActive(true);
		}
		this.m_animator.updateMode = AnimatorUpdateMode.Normal;
		this.m_animator.SetBool("Victory", false);
		for (int i = 0; i < 12; i++)
		{
			this.m_animator.ResetTrigger("Death" + i.ToString());
		}
		this.m_animator.Play("Idle", 0);
		this.m_animator.Update(1f);
		this.InitializeAllMods(true, true);
		if (!this.CharacterClass.OverrideSaveFileValues)
		{
			this.LookController.InitializeLook(SaveManager.PlayerSaveData.CurrentCharacter);
			base.ControllerCorgi.SetRaysParameters();
			if (SaveManager.PlayerSaveData.CurrentCharacter.ClassType != this.CharacterClass.ClassType)
			{
				this.CharacterClass.ClassType = SaveManager.PlayerSaveData.CurrentCharacter.ClassType;
			}
			if (SaveManager.PlayerSaveData.CurrentCharacter.Weapon != this.CharacterClass.WeaponAbilityType)
			{
				this.CharacterClass.SetAbility(CastAbilityType.Weapon, SaveManager.PlayerSaveData.CurrentCharacter.Weapon, true);
			}
			else
			{
				this.CastAbility.ReinitializeAbility(CastAbilityType.Weapon);
			}
			if (SaveManager.PlayerSaveData.CurrentCharacter.Spell != this.CharacterClass.SpellAbilityType)
			{
				this.CharacterClass.SetAbility(CastAbilityType.Spell, SaveManager.PlayerSaveData.CurrentCharacter.Spell, true);
			}
			else
			{
				this.CastAbility.ReinitializeAbility(CastAbilityType.Spell);
			}
			if (SaveManager.PlayerSaveData.CurrentCharacter.Talent != this.CharacterClass.TalentAbilityType)
			{
				this.CharacterClass.SetAbility(CastAbilityType.Talent, SaveManager.PlayerSaveData.CurrentCharacter.Talent, true);
			}
			else
			{
				this.CastAbility.ReinitializeAbility(CastAbilityType.Talent);
			}
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.UpdateAbilityHUD, this, null);
		}
		else
		{
			SaveManager.PlayerSaveData.CurrentCharacter.ClassType = this.CharacterClass.ClassType;
			SaveManager.PlayerSaveData.CurrentCharacter.Weapon = this.CharacterClass.WeaponAbilityType;
			SaveManager.PlayerSaveData.CurrentCharacter.Talent = this.CharacterClass.TalentAbilityType;
			SaveManager.PlayerSaveData.CurrentCharacter.Spell = this.CharacterClass.SpellAbilityType;
		}
		if (TraitManager.IsInitialized)
		{
			TraitType traitOne = SaveManager.PlayerSaveData.CurrentCharacter.TraitOne;
			TraitType traitTwo = SaveManager.PlayerSaveData.CurrentCharacter.TraitTwo;
			if (!TraitManager.IsTraitActive(traitOne) || !TraitManager.IsTraitActive(traitTwo))
			{
				this.m_traitChangeEventArgs.Initialize(traitOne, traitTwo);
				Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.TraitsChanged, this, this.m_traitChangeEventArgs);
			}
		}
		base.RecreateRendererArray();
		base.ResetRendererArrayColor();
		base.BlinkPulseEffect.ResetAllBlackFills();
		Vector3 localPosition = base.Visuals.transform.localPosition;
		localPosition.x = 0f;
		base.Visuals.transform.localPosition = localPosition;
		base.Visuals.gameObject.SetLayerRecursively(0, true);
		base.GetComponentInChildren<CharacterSortController>().ResetCharacterLayers();
		if (this.m_disableAbilitiesCoroutine != null)
		{
			base.StopCoroutine(this.m_disableAbilitiesCoroutine);
		}
		this.SetAllAbilitiesPermitted(true);
		this.ResetAbilityCooldowns();
		this.ResetAllAbilityAmmo();
		this.SetMushroomBig(false, false);
		if (this.CurrentExhaust > 0 && !base.StatusBarController.HasActiveStatusBarEntry(StatusBarEntryType.Exhaust))
		{
			base.StatusBarController.ApplyUIEffect(StatusBarEntryType.Exhaust, 99, this.CurrentExhaust);
		}
		this.ApplyPermanentStatusEffects(null, null);
	}

	// Token: 0x06001B1B RID: 6939 RVA: 0x000554E5 File Offset: 0x000536E5
	public void ApplyPermanentStatusEffects(object sender, EventArgs args)
	{
		if (base.isActiveAndEnabled)
		{
			base.StartCoroutine(this.ApplyPermanentStatusEffectsCoroutine());
		}
	}

	// Token: 0x06001B1C RID: 6940 RVA: 0x000554FC File Offset: 0x000536FC
	private IEnumerator ApplyPermanentStatusEffectsCoroutine()
	{
		yield return null;
		if (this.CharacterClass.ClassType == ClassType.BoxingGloveClass)
		{
			base.StatusEffectController.StartStatusEffect(StatusEffectType.Player_NoContactDamage, 0f, this);
		}
		if (this.CharacterClass.ClassType == ClassType.GunClass)
		{
			base.StatusEffectController.StartStatusEffect(StatusEffectType.Player_Suave, 0f, this);
		}
		yield break;
	}

	// Token: 0x06001B1D RID: 6941 RVA: 0x0005550B File Offset: 0x0005370B
	public override void ResetHealth()
	{
		this.CurrentArmor = this.ActualArmor;
		this.CachedHealthOverride = 0f;
		base.ResetHealth();
	}

	// Token: 0x06001B1E RID: 6942 RVA: 0x0005552A File Offset: 0x0005372A
	public void ResetMana()
	{
		this.SpellOrbs = 0;
		this.CachedManaOverride = 0f;
		this.SetMana((float)this.ActualMaxMana, false, true, false);
	}

	// Token: 0x06001B1F RID: 6943 RVA: 0x00055550 File Offset: 0x00053750
	public override void ResetStates()
	{
		this.StopActiveAbilities(true);
		this.DisableDoorBlock = false;
		this.DisableArmor = false;
		this.ResetGravityAndMovement();
		this.IsBlocking = false;
		this.IsSpearSpinning = false;
		this.CloakInterrupted = false;
		this.BlockStartTime = 0f;
		base.ResetStates();
	}

	// Token: 0x06001B20 RID: 6944 RVA: 0x0005559E File Offset: 0x0005379E
	public void ResetAbilityCooldowns()
	{
		this.m_characterAbilities.ResetAbilityCooldowns(CastAbilityType.Weapon, false);
		this.m_characterAbilities.ResetAbilityCooldowns(CastAbilityType.Talent, false);
		this.m_characterAbilities.ResetAbilityCooldowns(CastAbilityType.Spell, false);
	}

	// Token: 0x06001B21 RID: 6945 RVA: 0x000555C7 File Offset: 0x000537C7
	public void ResetAllAbilityAmmo()
	{
		this.m_characterAbilities.ResetAbilityAmmo(CastAbilityType.Weapon, false);
		this.m_characterAbilities.ResetAbilityAmmo(CastAbilityType.Talent, false);
		this.m_characterAbilities.ResetAbilityAmmo(CastAbilityType.Spell, false);
	}

	// Token: 0x06001B22 RID: 6946 RVA: 0x000555F0 File Offset: 0x000537F0
	public void StopActiveAbilities(bool stopPersistentAbilities)
	{
		this.m_characterAbilities.StopAllAbilities(stopPersistentAbilities);
		base.Animator.SetBool("Bounce", false);
		base.Animator.SetBool("DanceBounce", false);
		switch (base.MovementState)
		{
		case CharacterStates.MovementStates.Dashing:
			this.m_characterDash.StopDash();
			break;
		case CharacterStates.MovementStates.DownStriking:
			this.m_characterDownStrike.StopDownStrike();
			break;
		case CharacterStates.MovementStates.Jumping:
		case CharacterStates.MovementStates.DoubleJumping:
			this.m_characterJump.JumpStop();
			this.m_characterJump.ResetBrakeForce();
			break;
		}
		this.m_characterFlight.StopFlight();
		this.DisableAbilitiesForXSeconds(0.1f);
	}

	// Token: 0x06001B23 RID: 6947 RVA: 0x00055696 File Offset: 0x00053896
	public void DisableAbilitiesForXSeconds(float duration)
	{
		if (this.m_disableAbilitiesCoroutine != null)
		{
			base.StopCoroutine(this.m_disableAbilitiesCoroutine);
		}
		if (duration > 0f)
		{
			this.m_disableAbilitiesCoroutine = base.StartCoroutine(this.DisableAbilitiesCoroutine(duration));
			return;
		}
		this.SetAllAbilitiesPermitted(true);
	}

	// Token: 0x06001B24 RID: 6948 RVA: 0x000556CF File Offset: 0x000538CF
	private IEnumerator DisableAbilitiesCoroutine(float duration)
	{
		this.SetAllAbilitiesPermitted(false);
		float startTime = Time.time;
		while (Time.time < startTime + duration)
		{
			yield return null;
		}
		this.SetAllAbilitiesPermitted(true);
		yield break;
	}

	// Token: 0x06001B25 RID: 6949 RVA: 0x000556E5 File Offset: 0x000538E5
	public void SetAllAbilitiesPermitted(bool permitted)
	{
		this.m_characterDash.AbilityPermitted = permitted;
		this.m_characterAbilities.AbilityPermitted = permitted;
		this.m_characterDownStrike.AbilityPermitted = permitted;
		this.m_characterMove.AbilityPermitted = permitted;
		this.m_characterJump.AbilityPermitted = permitted;
	}

	// Token: 0x06001B26 RID: 6950 RVA: 0x00055724 File Offset: 0x00053924
	private float ApplyAssistDamageMods(IDamageObj damageObj, float damageTaken)
	{
		float num = damageTaken;
		if (SaveManager.PlayerSaveData.EnableHouseRules)
		{
			num *= SaveManager.PlayerSaveData.Assist_EnemyDamageMod;
		}
		return num;
	}

	// Token: 0x06001B27 RID: 6951 RVA: 0x00055750 File Offset: 0x00053950
	public override float CalculateDamageTaken(IDamageObj damageObj, out CriticalStrikeType critType, out float damageBlocked, float damageOverride = -1f, bool trueDamage = false, bool pureCalculation = true)
	{
		critType = CriticalStrikeType.None;
		damageBlocked = 0f;
		float num = 1f;
		if (base.TakesNoDamage)
		{
			return 0f;
		}
		if (base.StatusEffectController.HasInvulnStack)
		{
			return 0f;
		}
		Projectile_RL projectile_RL = damageObj as Projectile_RL;
		if (!this.DisableArmor)
		{
			RelicObj relic = SaveManager.PlayerSaveData.GetRelic(RelicType.TakeNoDamage);
			if (relic.Level > 0)
			{
				if (!pureCalculation)
				{
					relic.SetLevel(-1, true, true);
				}
				return 0f;
			}
		}
		float num2 = damageObj.ActualDamage;
		if (damageOverride != -1f)
		{
			if (trueDamage)
			{
				return damageOverride;
			}
			num2 = this.ApplyAssistDamageMods(damageObj, damageOverride);
		}
		else
		{
			num2 = this.ApplyAssistDamageMods(damageObj, num2);
			float actualCritChance = damageObj.ActualCritChance;
			float num3 = UnityEngine.Random.Range(0f, 1f);
			if (actualCritChance > 0f && actualCritChance >= num3)
			{
				num2 += damageObj.ActualCritDamage;
				if (actualCritChance >= 100f)
				{
					critType = CriticalStrikeType.Guaranteed;
				}
				else
				{
					critType = CriticalStrikeType.Regular;
				}
			}
			else if ((damageObj.gameObject.CompareTag("Enemy") || damageObj.gameObject.CompareTag("EnemyProjectile")) && BurdenManager.GetBurdenLevel(BurdenType.EnemyArmorShred) > 0 && this.CurrentArmor <= 0)
			{
				num += 1.15f;
				critType = CriticalStrikeType.Guaranteed;
			}
			if (projectile_RL)
			{
				num += projectile_RL.DamageMod;
			}
		}
		if (TraitManager.IsTraitActive(TraitType.Vampire))
		{
			num += 1.25f;
		}
		num += 1f * (float)SaveManager.PlayerSaveData.GetRelic(RelicType.BonusDamageCurse).Level;
		num += 0.75f * (float)SaveManager.PlayerSaveData.GetRelic(RelicType.FlightBonusCurse).Level;
		if (base.StatusEffectController.HasStatusEffect(StatusEffectType.Player_GodMode))
		{
			num += 0f;
		}
		if (base.MovementState == CharacterStates.MovementStates.Dashing)
		{
			num -= SkillTreeLogicHelper.GetDashDamageMod();
		}
		num = Mathf.Clamp(num, 0f, 999999f);
		num2 *= num;
		int currentArmor = this.CurrentArmor;
		if (!this.DisableArmor)
		{
			if (this.IsBlocking)
			{
				float num4;
				if (Time.time < this.BlockStartTime + 0.135f)
				{
					num4 = 0f;
				}
				else
				{
					num4 = 0.5f;
				}
				num2 *= num4;
			}
			float num5 = Mathf.Min((float)this.CurrentArmor, (0.35f + RuneLogicHelper.GetArmorMinBlockAdd()) * num2);
			float num6 = num2 - num5;
			int num7 = (int)(num2 - num6);
			if (num7 > 0 && (!this.IsBlocking || (this.IsBlocking && num6 > 0f)))
			{
				int num8 = 0;
				float num9 = 0f;
				if (damageObj.gameObject.CompareTag("Enemy") || damageObj.gameObject.CompareTag("EnemyProjectile"))
				{
					float burdenStatGain = BurdenManager.GetBurdenStatGain(BurdenType.EnemyArmorShred);
					if (burdenStatGain > 0f)
					{
						num8 += Mathf.Clamp(Mathf.RoundToInt((float)this.ActualArmor * burdenStatGain), 1, int.MaxValue);
					}
					EnemyController enemyController = damageObj as EnemyController;
					if (enemyController || projectile_RL)
					{
						if (!enemyController)
						{
							enemyController = (projectile_RL.OwnerController as EnemyController);
						}
						if (enemyController && enemyController.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_ArmorShred))
						{
							num9 += 0.1f;
						}
					}
				}
				num8 += Mathf.RoundToInt(num9 * (float)this.ActualArmor);
				if (!pureCalculation)
				{
					this.CurrentArmor -= num8;
				}
			}
			num2 -= (float)num7;
			damageBlocked += (float)num7;
			damageBlocked = (float)Mathf.RoundToInt(damageBlocked);
		}
		num2 = Mathf.Max(0f, Mathf.Floor(num2));
		if (num2 > 0f)
		{
			if (SaveManager.PlayerSaveData.GetRelic(RelicType.ResolveCombatChallenge).Level > 0)
			{
				num2 = 99999f;
			}
			if (SaveManager.PlayerSaveData.GetRelic(RelicType.GoldCombatChallenge).Level > 0)
			{
				num2 = 99999f;
			}
			if (SaveManager.PlayerSaveData.GetRelic(RelicType.FoodChallenge).Level > 0)
			{
				num2 = 99999f;
			}
			RelicObj relic2 = SaveManager.PlayerSaveData.GetRelic(RelicType.FreeHitRegenerate);
			if (relic2.Level > 0)
			{
				int num10 = 6;
				num10 -= relic2.Level - 1;
				if (relic2.IntValue >= num10)
				{
					num2 = 0f;
					damageBlocked = 0f;
					if (!pureCalculation)
					{
						this.CurrentArmor = currentArmor;
					}
					if (!pureCalculation)
					{
						relic2.SetIntValue(0, false, true);
						BaseCharacterHitResponse.GLYPHS_TO_ADD_TO_PLAYER_DAMAGE_HELPER.Add(RelicType.FreeHitRegenerate.ToString());
					}
				}
			}
			RelicObj relic3 = SaveManager.PlayerSaveData.GetRelic(RelicType.ManaDamageReduction);
			if (relic3.Level > 0 && num2 > 0f && (float)this.CurrentManaAsInt >= 150f && relic3.IntValue < 2 * relic3.Level)
			{
				num2 = Mathf.Floor(num2 * 0f);
				if (!pureCalculation)
				{
					this.SetMana(-150f, true, true, false);
					relic3.SetIntValue(1, true, true);
					BaseCharacterHitResponse.GLYPHS_TO_ADD_TO_PLAYER_DAMAGE_HELPER.Add(RelicType.ManaDamageReduction.ToString());
				}
			}
			if (this.CurrentExhaust > 0)
			{
				float num11 = (float)this.CurrentExhaust * 0.01f;
				float num12 = (float)this.ActualMaxHealth * num11;
				num2 += num12;
			}
			if (!pureCalculation && this.CurrentHealth - num2 <= 0f)
			{
				int level = SaveManager.PlayerSaveData.GetRelic(RelicType.FatalBlowDodge).Level;
				if (level > 0 && UnityEngine.Random.Range(0f, 1f) <= 0.25f * (float)level)
				{
					num2 = 0f;
					BaseCharacterHitResponse.GLYPHS_TO_ADD_TO_PLAYER_DAMAGE_HELPER.Add(RelicType.FatalBlowDodge.ToString());
					AudioManager.PlayOneShot(this, "event:/UI/InGame/ui_ig_gravebells_lose", default(Vector3));
				}
			}
		}
		return num2;
	}

	// Token: 0x06001B28 RID: 6952 RVA: 0x00055CD4 File Offset: 0x00053ED4
	public override void KillCharacter(GameObject killer, bool broadcastEvent)
	{
		if (base.IsDead)
		{
			return;
		}
		Debug.LogFormat("Player died in {0} of {1} | {2} | {3}", new object[]
		{
			PlayerManager.GetCurrentPlayerRoom(),
			PlayerManager.GetCurrentPlayerRoom().BiomeType,
			SaveManager.PlayerSaveData.InHubTown,
			ChallengeManager.IsInChallenge
		});
		base.KillCharacter(killer, broadcastEvent);
		if (broadcastEvent)
		{
			this.m_playerDeathEventArgs.Initialize(this, killer);
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerDeath, this, this.m_playerDeathEventArgs);
			this.m_onPlayerDeathRelay.Dispatch(this.m_playerDeathEventArgs);
			this.m_onPreDisableRelay.Dispatch(this);
			this.m_onDeathEffectTriggerRelay.Dispatch((killer != null) ? killer : null);
			if (killer)
			{
				EnemyController component = killer.GetComponent<EnemyController>();
				if (component)
				{
					SaveManager.ModeSaveData.SetTimesDefeatedByEnemy(SaveManager.PlayerSaveData.GameModeType, component.EnemyType, component.EnemyRank, 1, true);
				}
			}
		}
		foreach (AnimatorControllerParameter animatorControllerParameter in base.Animator.parameters)
		{
			AnimatorControllerParameterType type = animatorControllerParameter.type;
			if (type != AnimatorControllerParameterType.Bool)
			{
				if (type == AnimatorControllerParameterType.Trigger)
				{
					base.Animator.ResetTrigger(animatorControllerParameter.name);
				}
			}
			else
			{
				base.Animator.SetBool(animatorControllerParameter.name, false);
			}
		}
		this.StopActiveAbilities(true);
		WindowManager.SetWindowIsOpen(WindowID.PlayerDeath, true);
	}

	// Token: 0x06001B29 RID: 6953 RVA: 0x00055E34 File Offset: 0x00054034
	public void EnterRoom(BaseRoom room, Door door, Vector3 localPosition)
	{
		this.CheckForBiomeChange(room.BiomeType);
		this.CurrentlyInRoom = room;
		base.transform.position = room.gameObject.transform.TransformPoint(localPosition);
		if (!door || room.BiomeType == BiomeType.Stone)
		{
			base.ControllerCorgi.SetLastStandingPosition(base.transform.position);
		}
		else
		{
			base.ControllerCorgi.SetLastStandingPosition(door.transform.position);
		}
		if (!room.gameObject.activeInHierarchy)
		{
			room.gameObject.SetActive(true);
		}
		this.TimeEnteredRoom = Time.time;
		if (base.ControllerCorgi)
		{
			base.StartCoroutine(this.ResetCorgiControllerRayParameters());
		}
	}

	// Token: 0x06001B2A RID: 6954 RVA: 0x00055EF0 File Offset: 0x000540F0
	public void EnterRoom(BaseRoom room)
	{
		if (room is Room)
		{
			Room room2 = room as Room;
			this.EnterRoom(room2, null, room2.PlayerSpawn.transform.localPosition);
			return;
		}
		this.EnterRoom(room, null, Vector3.zero);
	}

	// Token: 0x06001B2B RID: 6955 RVA: 0x00055F34 File Offset: 0x00054134
	public void EnterRoom(Door door)
	{
		Vector2 b = new Vector2(0f, 0f);
		if (door.Side == RoomSide.Left || door.Side == RoomSide.Right)
		{
			int num = 1;
			if (door.Side == RoomSide.Right)
			{
				num = -1;
			}
			float y = base.transform.position.y - door.CenterPoint.y;
			b = new Vector2((float)num * Room_EV.TRANSITION_PLAYER_POSITION_LEFT_RIGHT_X_OFFSET, y);
		}
		else if (door.Side == RoomSide.Bottom)
		{
			float x = base.transform.position.x - door.CenterPoint.x;
			b = new Vector2(x, Room_EV.TRANSITION_PLAYER_POSITION_BOTTOM_Y_OFFSET);
			if (base.Velocity.y < Room_EV.TRANSITION_MINIMUM_SPEED_ON_ENTER_BOTTOM)
			{
				this.SetVelocity(base.Velocity.x, Room_EV.TRANSITION_MINIMUM_SPEED_ON_ENTER_BOTTOM, false);
			}
		}
		else if (door.Side == RoomSide.Top)
		{
			float x2 = base.transform.position.x - door.CenterPoint.x;
			b = new Vector2(x2, Room_EV.TRANSITION_PLAYER_POSITION_TOP_Y_OFFSET);
		}
		Vector2 v = door.CenterPoint + b;
		Vector3 localPosition = door.Room.gameObject.transform.InverseTransformPoint(v);
		this.EnterRoom(door.Room, door, localPosition);
	}

	// Token: 0x06001B2C RID: 6956 RVA: 0x00056070 File Offset: 0x00054270
	public void DisableEffectsOnEnterTunnel()
	{
		base.StartCoroutine(this.DisableEffectsOnEnterTunnelCoroutine(0.1f));
	}

	// Token: 0x06001B2D RID: 6957 RVA: 0x00056084 File Offset: 0x00054284
	private IEnumerator DisableEffectsOnEnterTunnelCoroutine(float duration)
	{
		if (this.m_animator)
		{
			EffectManager.AddAnimatorToDisableList(this.m_animator);
			this.m_animator.Play("LandIdle", 0, 1f);
			float delay = Time.time + duration;
			while (Time.time < delay)
			{
				yield return null;
			}
			EffectManager.RemoveAnimatorFromDisableList(this.m_animator);
		}
		yield break;
	}

	// Token: 0x06001B2E RID: 6958 RVA: 0x0005609A File Offset: 0x0005429A
	private IEnumerator ResetCorgiControllerRayParameters()
	{
		while (!base.ControllerCorgi.IsInitialized)
		{
			yield return null;
		}
		base.ControllerCorgi.SetRaysParameters();
		yield break;
	}

	// Token: 0x06001B2F RID: 6959 RVA: 0x000560AC File Offset: 0x000542AC
	public void WeaponOnEnterHitResponse(IHitboxController otherHBController)
	{
		Vector3 collisionPoint = base.Midpoint;
		if (base.HitboxController.LastCollidedWith != null)
		{
			collisionPoint = base.HitboxController.LastCollidedWith.ClosestPoint(base.Midpoint);
		}
		IPlayHitEffect playHitEffect = (otherHBController != null) ? otherHBController.RootGameObject.GetComponent<IPlayHitEffect>() : null;
		bool flag = playHitEffect.IsNativeNull();
		if (otherHBController.IsNativeNull() || (!flag && playHitEffect.PlayHitEffect))
		{
			if (!flag)
			{
				EffectManager.PlayHitEffect(this, collisionPoint, playHitEffect.EffectNameOverride, this.StrikeType, false);
			}
			else
			{
				EffectManager.PlayHitEffect(this, collisionPoint, null, this.StrikeType, false);
			}
		}
		if (!flag && playHitEffect.PlayDirectionalHitEffect)
		{
			EffectManager.PlayDirectionalHitEffect(this, base.HitboxController.RootGameObject, collisionPoint);
		}
	}

	// Token: 0x06001B30 RID: 6960 RVA: 0x00056168 File Offset: 0x00054368
	public void PauseGravity(bool stopVelocity, bool lockPlayerControls)
	{
		base.ControllerCorgi.GravityActive(false);
		if (stopVelocity)
		{
			this.SetVelocity(0f, 0f, false);
			this.CharacterJump.ResetBrakeForce();
		}
		if (lockPlayerControls)
		{
			base.ConditionState = CharacterStates.CharacterConditions.DisableHorizontalMovement;
		}
	}

	// Token: 0x06001B31 RID: 6961 RVA: 0x0005619F File Offset: 0x0005439F
	public void ResumeGravity()
	{
		base.ControllerCorgi.GravityActive(true);
		if (base.ConditionState != CharacterStates.CharacterConditions.Stunned)
		{
			base.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
	}

	// Token: 0x06001B32 RID: 6962 RVA: 0x000561C0 File Offset: 0x000543C0
	public void SetMushroomBig(bool setMushroomBig, bool animate)
	{
		if (setMushroomBig != this.IsMushroomBig)
		{
			this.IsMushroomBig = setMushroomBig;
			if (animate)
			{
				base.StartCoroutine(this.MushroomAnimCoroutine(setMushroomBig));
				return;
			}
			Vector3 localScale = new Vector3(1.4f, 1.4f, 1f);
			if (TraitManager.IsTraitActive(TraitType.YouAreSmall))
			{
				localScale.x = 0.77f;
				localScale.y = 0.77f;
			}
			base.transform.localScale = localScale;
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerScaleChanged, null, null);
		}
	}

	// Token: 0x06001B33 RID: 6963 RVA: 0x0005623B File Offset: 0x0005443B
	private IEnumerator MushroomAnimCoroutine(bool growBig)
	{
		Vector3 smallScale = new Vector3(1.4f, 1.4f, 1f);
		Vector3 largeScale = smallScale;
		if (!TraitManager.IsTraitActive(TraitType.YouAreSmall))
		{
			largeScale.x = 2.1f;
			largeScale.y = 2.1f;
		}
		else
		{
			smallScale.x = 0.77f;
			smallScale.y = 0.77f;
		}
		float num = smallScale.x + (largeScale.x - smallScale.x) / 2f;
		Vector3 mediumScale = new Vector3(num, num, 1f);
		float interval = 0.1f;
		RewiredMapController.SetCurrentMapEnabled(false);
		RLTimeScale.SetTimeScale(TimeScaleType.Game, 0f);
		if (growBig)
		{
			yield return this.MushroomIntervalCoroutine(interval, mediumScale);
			yield return this.MushroomIntervalCoroutine(interval, smallScale);
			yield return this.MushroomIntervalCoroutine(interval, mediumScale);
			yield return this.MushroomIntervalCoroutine(interval, smallScale);
			yield return this.MushroomIntervalCoroutine(interval, mediumScale);
			yield return this.MushroomIntervalCoroutine(interval, largeScale);
		}
		else
		{
			yield return this.MushroomIntervalCoroutine(interval, mediumScale);
			yield return this.MushroomIntervalCoroutine(interval, largeScale);
			yield return this.MushroomIntervalCoroutine(interval, mediumScale);
			yield return this.MushroomIntervalCoroutine(interval, largeScale);
			yield return this.MushroomIntervalCoroutine(interval, mediumScale);
			yield return this.MushroomIntervalCoroutine(interval, smallScale);
		}
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerScaleChanged, null, null);
		RewiredMapController.SetCurrentMapEnabled(true);
		RLTimeScale.SetTimeScale(TimeScaleType.Game, 1f);
		yield break;
	}

	// Token: 0x06001B34 RID: 6964 RVA: 0x00056251 File Offset: 0x00054451
	private IEnumerator MushroomIntervalCoroutine(float interval, Vector3 scale)
	{
		base.transform.localScale = scale;
		float delay = Time.unscaledTime + interval;
		while (Time.unscaledTime < delay)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001B35 RID: 6965 RVA: 0x0005626E File Offset: 0x0005446E
	public void StartOnHitAreaDamageTimer()
	{
		if (this.m_onHitAreaDamageCoroutine != null)
		{
			base.StopCoroutine(this.m_onHitAreaDamageCoroutine);
			this.m_onHitAreaDamageCoroutine = null;
		}
		this.m_onHitAreaDamageCoroutine = base.StartCoroutine(this.OnHitAreaDamageCoroutine());
	}

	// Token: 0x06001B36 RID: 6966 RVA: 0x0005629D File Offset: 0x0005449D
	public void StopOnHitAreaDamageTimer()
	{
		SaveManager.PlayerSaveData.GetRelic(RelicType.OnHitAreaDamage).SetIntValue(0, false, true);
		if (this.m_onHitAreaDamageCoroutine != null)
		{
			base.StopCoroutine(this.m_onHitAreaDamageCoroutine);
			this.m_onHitAreaDamageCoroutine = null;
		}
	}

	// Token: 0x06001B37 RID: 6967 RVA: 0x000562D1 File Offset: 0x000544D1
	private IEnumerator OnHitAreaDamageCoroutine()
	{
		this.m_onHitAreaDamage_waitYield.CreateNew(1f, false);
		RelicObj relicObj = SaveManager.PlayerSaveData.GetRelic(RelicType.OnHitAreaDamage);
		int timeRequirement = Relic_EV.GetRelicMaxStack(relicObj.RelicType, relicObj.Level);
		while (relicObj.IntValue < timeRequirement)
		{
			yield return this.m_onHitAreaDamage_waitYield;
			relicObj.SetIntValue(1, true, true);
		}
		yield break;
	}

	// Token: 0x06001B38 RID: 6968 RVA: 0x000562E0 File Offset: 0x000544E0
	public void StartNoAttackDamageBonusTimer()
	{
		if (this.m_noAttackDamageBonusCoroutine != null)
		{
			base.StopCoroutine(this.m_noAttackDamageBonusCoroutine);
			this.m_noAttackDamageBonusCoroutine = null;
		}
		this.m_noAttackDamageBonusCoroutine = base.StartCoroutine(this.NoAttackDamageBonusCoroutine());
	}

	// Token: 0x06001B39 RID: 6969 RVA: 0x0005630F File Offset: 0x0005450F
	public void StopNoAttackDamageBonusTimer()
	{
		SaveManager.PlayerSaveData.GetRelic(RelicType.NoAttackDamageBonus).SetIntValue(0, false, true);
		if (this.m_noAttackDamageBonusCoroutine != null)
		{
			base.StopCoroutine(this.m_noAttackDamageBonusCoroutine);
			this.m_noAttackDamageBonusCoroutine = null;
		}
	}

	// Token: 0x06001B3A RID: 6970 RVA: 0x00056343 File Offset: 0x00054543
	private IEnumerator NoAttackDamageBonusCoroutine()
	{
		this.m_noAttackDamageBonus_waitYield.CreateNew(1f, false);
		RelicObj relicObj = SaveManager.PlayerSaveData.GetRelic(RelicType.NoAttackDamageBonus);
		int timeRequirement = Relic_EV.GetRelicMaxStack(relicObj.RelicType, relicObj.Level);
		while (relicObj.IntValue < timeRequirement)
		{
			yield return this.m_noAttackDamageBonus_waitYield;
			relicObj.SetIntValue(1, true, true);
		}
		yield break;
	}

	// Token: 0x06001B3B RID: 6971 RVA: 0x00056352 File Offset: 0x00054552
	public void StartSpinKicksDropCaltropsTimer()
	{
		if (this.m_spinKicksDropCaltropsCoroutine != null)
		{
			base.StopCoroutine(this.m_spinKicksDropCaltropsCoroutine);
			this.m_spinKicksDropCaltropsCoroutine = null;
		}
		this.m_spinKicksDropCaltropsCoroutine = base.StartCoroutine(this.SpinKicksDropCaltropsCoroutine());
	}

	// Token: 0x06001B3C RID: 6972 RVA: 0x00056381 File Offset: 0x00054581
	public void StopSpinKicksDropCaltropsTimer()
	{
		SaveManager.PlayerSaveData.GetRelic(RelicType.SpinKickLeavesCaltrops).SetIntValue(0, false, true);
		if (this.m_spinKicksDropCaltropsCoroutine != null)
		{
			base.StopCoroutine(this.m_spinKicksDropCaltropsCoroutine);
			this.m_spinKicksDropCaltropsCoroutine = null;
		}
	}

	// Token: 0x06001B3D RID: 6973 RVA: 0x000563B5 File Offset: 0x000545B5
	private IEnumerator SpinKicksDropCaltropsCoroutine()
	{
		RelicObj relicObj = SaveManager.PlayerSaveData.GetRelic(RelicType.SpinKickLeavesCaltrops);
		int timeRequirement = Relic_EV.GetRelicMaxStack(relicObj.RelicType, relicObj.Level);
		float delay = 0f;
		while (relicObj.IntValue < timeRequirement)
		{
			delay = Time.time + 1f;
			while (Time.time < delay)
			{
				yield return null;
			}
			relicObj.SetIntValue(1, true, true);
		}
		yield break;
	}

	// Token: 0x06001B3F RID: 6975 RVA: 0x00056432 File Offset: 0x00054632
	GameObject IDamageObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040018A7 RID: 6311
	private string[] m_projectileNameArray = new string[]
	{
		"CreatePlatformTalentProjectile",
		"SuperFartProjectile",
		"SporeBurstProjectile",
		"RelicOnHitAreaDamageProjectile",
		"RelicLandShockwaveProjectile",
		"RelicDamageAuraOnHitProjectile"
	};

	// Token: 0x040018A8 RID: 6312
	[SerializeField]
	private GameObject m_followTargetGO;

	// Token: 0x040018A9 RID: 6313
	[SerializeField]
	private GameObject m_rangeBonusRelicIndicatorGO;

	// Token: 0x040018AA RID: 6314
	private int m_baseArmor;

	// Token: 0x040018AB RID: 6315
	private float m_baseCritDamage;

	// Token: 0x040018AC RID: 6316
	private float m_baseDexterity;

	// Token: 0x040018AD RID: 6317
	private float m_baseMagicCritDamage;

	// Token: 0x040018AE RID: 6318
	private float m_baseMagicDexterity;

	// Token: 0x040018AF RID: 6319
	private float m_baseResolve;

	// Token: 0x040018B0 RID: 6320
	private int m_baseVitality;

	// Token: 0x040018B1 RID: 6321
	private CharacterDash_RL m_characterDash;

	// Token: 0x040018B2 RID: 6322
	private CharacterDownStrike_RL m_characterDownStrike;

	// Token: 0x040018B3 RID: 6323
	private CastAbility_RL m_characterAbilities;

	// Token: 0x040018B4 RID: 6324
	private CharacterHorizontalMovement_RL m_characterMove;

	// Token: 0x040018B5 RID: 6325
	private CharacterJump_RL m_characterJump;

	// Token: 0x040018B6 RID: 6326
	private CharacterClass m_characterClass;

	// Token: 0x040018B7 RID: 6327
	private CharacterFlight_RL m_characterFlight;

	// Token: 0x040018B8 RID: 6328
	private PlayerLookController m_lookController;

	// Token: 0x040018B9 RID: 6329
	private InteractIconController m_interactIconController;

	// Token: 0x040018BA RID: 6330
	private BaseRoom m_currentlyInRoom;

	// Token: 0x040018BB RID: 6331
	private Player m_rewiredPlayer;

	// Token: 0x040018BC RID: 6332
	private BiomeEventArgs m_biomeEventArgs;

	// Token: 0x040018BD RID: 6333
	private PlayerDeathEventArgs m_playerDeathEventArgs;

	// Token: 0x040018BE RID: 6334
	private bool m_justRolled;

	// Token: 0x040018BF RID: 6335
	private Coroutine m_rollCoroutine;

	// Token: 0x040018C0 RID: 6336
	protected ManaChangeEventArgs m_manaChangeEventArgs;

	// Token: 0x040018C1 RID: 6337
	private TraitChangedEventArgs m_traitChangeEventArgs;

	// Token: 0x040018C2 RID: 6338
	private Coroutine m_onHitAreaDamageCoroutine;

	// Token: 0x040018C3 RID: 6339
	private Coroutine m_noAttackDamageBonusCoroutine;

	// Token: 0x040018C4 RID: 6340
	private Coroutine m_spinKicksDropCaltropsCoroutine;

	// Token: 0x040018C5 RID: 6341
	private WaitRL_Yield m_onHitAreaDamage_waitYield;

	// Token: 0x040018C6 RID: 6342
	private WaitRL_Yield m_noAttackDamageBonus_waitYield;

	// Token: 0x040018C7 RID: 6343
	private Action<MonoBehaviour, EventArgs> m_onEquippedOrLevelChanged;

	// Token: 0x040018C8 RID: 6344
	private Action<MonoBehaviour, EventArgs> m_onRelicChanged;

	// Token: 0x040018C9 RID: 6345
	private Action<MonoBehaviour, EventArgs> m_applyPermanentStatusEffects;

	// Token: 0x040018D1 RID: 6353
	protected Relay<PlayerDeathEventArgs> m_onPlayerDeathRelay = new Relay<PlayerDeathEventArgs>();

	// Token: 0x040018D2 RID: 6354
	private Relay<ManaChangeEventArgs> m_manaChangeRelay = new Relay<ManaChangeEventArgs>();

	// Token: 0x040018E7 RID: 6375
	private int m_currentArmor;

	// Token: 0x040018E9 RID: 6377
	private int m_currentExhaust;

	// Token: 0x040018FD RID: 6397
	private float m_updateAccumulatedXP;

	// Token: 0x040018FE RID: 6398
	private float m_updateAccumulatedLifeSteal;

	// Token: 0x040018FF RID: 6399
	private TextPopupObj m_xpPopup;

	// Token: 0x04001900 RID: 6400
	private RelicChangedEventArgs m_extraLifeEventArgs = new RelicChangedEventArgs(RelicType.ExtraLife_Unity);

	// Token: 0x04001901 RID: 6401
	private Coroutine m_disableAbilitiesCoroutine;
}
