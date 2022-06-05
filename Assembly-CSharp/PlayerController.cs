using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using Rewired;
using RLAudio;
using RL_Windows;
using SceneManagement_RL;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000481 RID: 1153
public class PlayerController : BaseCharacterController, IDamageObj, IWeaponOnEnterHitResponse, IHitResponse, IHasProjectileNameArray, IAudioEventEmitter
{
	// Token: 0x17000F5A RID: 3930
	// (get) Token: 0x06002469 RID: 9321 RVA: 0x000142C7 File Offset: 0x000124C7
	public string[] ProjectileNameArray
	{
		get
		{
			return this.m_projectileNameArray;
		}
	}

	// Token: 0x17000F5B RID: 3931
	// (get) Token: 0x0600246A RID: 9322 RVA: 0x0000F49B File Offset: 0x0000D69B
	public string RelicDamageTypeString
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000F5C RID: 3932
	// (get) Token: 0x0600246B RID: 9323 RVA: 0x000142CF File Offset: 0x000124CF
	// (set) Token: 0x0600246C RID: 9324 RVA: 0x000142D7 File Offset: 0x000124D7
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

	// Token: 0x17000F5D RID: 3933
	// (get) Token: 0x0600246D RID: 9325 RVA: 0x000142E0 File Offset: 0x000124E0
	// (set) Token: 0x0600246E RID: 9326 RVA: 0x000142E8 File Offset: 0x000124E8
	public bool IsMushroomBig { get; private set; }

	// Token: 0x17000F5E RID: 3934
	// (get) Token: 0x0600246F RID: 9327 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x17000F5F RID: 3935
	// (get) Token: 0x06002470 RID: 9328 RVA: 0x000142F1 File Offset: 0x000124F1
	// (set) Token: 0x06002471 RID: 9329 RVA: 0x000142F9 File Offset: 0x000124F9
	public float TimeEnteredRoom { get; private set; }

	// Token: 0x17000F60 RID: 3936
	// (get) Token: 0x06002472 RID: 9330 RVA: 0x00014302 File Offset: 0x00012502
	// (set) Token: 0x06002473 RID: 9331 RVA: 0x0001430A File Offset: 0x0001250A
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

	// Token: 0x17000F61 RID: 3937
	// (get) Token: 0x06002474 RID: 9332 RVA: 0x00014313 File Offset: 0x00012513
	// (set) Token: 0x06002475 RID: 9333 RVA: 0x00014320 File Offset: 0x00012520
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

	// Token: 0x17000F62 RID: 3938
	// (get) Token: 0x06002476 RID: 9334 RVA: 0x0001432E File Offset: 0x0001252E
	// (set) Token: 0x06002477 RID: 9335 RVA: 0x00014336 File Offset: 0x00012536
	public bool DisableDoorBlock { get; set; }

	// Token: 0x17000F63 RID: 3939
	// (get) Token: 0x06002478 RID: 9336 RVA: 0x0000F49B File Offset: 0x0000D69B
	public StatusEffectType[] StatusEffectTypes
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000F64 RID: 3940
	// (get) Token: 0x06002479 RID: 9337 RVA: 0x0000F49B File Offset: 0x0000D69B
	public float[] StatusEffectDurations
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000F65 RID: 3941
	// (get) Token: 0x0600247A RID: 9338 RVA: 0x0001433F File Offset: 0x0001253F
	// (set) Token: 0x0600247B RID: 9339 RVA: 0x00014347 File Offset: 0x00012547
	public bool IsSpearSpinning { get; set; }

	// Token: 0x17000F66 RID: 3942
	// (get) Token: 0x0600247C RID: 9340 RVA: 0x00014350 File Offset: 0x00012550
	// (set) Token: 0x0600247D RID: 9341 RVA: 0x00014358 File Offset: 0x00012558
	public bool IsBlocking { get; set; }

	// Token: 0x17000F67 RID: 3943
	// (get) Token: 0x0600247E RID: 9342 RVA: 0x00014361 File Offset: 0x00012561
	public bool IsPerfectBlocking
	{
		get
		{
			return this.IsBlocking && Time.time < this.BlockStartTime + 0.135f;
		}
	}

	// Token: 0x17000F68 RID: 3944
	// (get) Token: 0x0600247F RID: 9343 RVA: 0x00014380 File Offset: 0x00012580
	// (set) Token: 0x06002480 RID: 9344 RVA: 0x00014388 File Offset: 0x00012588
	public bool CloakInterrupted { get; set; }

	// Token: 0x17000F69 RID: 3945
	// (get) Token: 0x06002481 RID: 9345 RVA: 0x00014391 File Offset: 0x00012591
	// (set) Token: 0x06002482 RID: 9346 RVA: 0x00014399 File Offset: 0x00012599
	public float BlockStartTime { get; set; }

	// Token: 0x17000F6A RID: 3946
	// (get) Token: 0x06002483 RID: 9347 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public bool IsDotDamage
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000F6B RID: 3947
	// (get) Token: 0x06002484 RID: 9348 RVA: 0x000143A2 File Offset: 0x000125A2
	public IRelayLink<PlayerDeathEventArgs> OnPlayerDeathRelay
	{
		get
		{
			return this.m_onPlayerDeathRelay.link;
		}
	}

	// Token: 0x17000F6C RID: 3948
	// (get) Token: 0x06002485 RID: 9349 RVA: 0x000143AF File Offset: 0x000125AF
	public IRelayLink<ManaChangeEventArgs> ManaChangeRelay
	{
		get
		{
			return this.m_manaChangeRelay.link;
		}
	}

	// Token: 0x17000F6D RID: 3949
	// (get) Token: 0x06002486 RID: 9350 RVA: 0x00003DE8 File Offset: 0x00001FE8
	public override float BaseScaleToOffsetWith
	{
		get
		{
			return 1.4f;
		}
	}

	// Token: 0x17000F6E RID: 3950
	// (get) Token: 0x06002487 RID: 9351 RVA: 0x000143BC File Offset: 0x000125BC
	// (set) Token: 0x06002488 RID: 9352 RVA: 0x000143C4 File Offset: 0x000125C4
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

	// Token: 0x17000F6F RID: 3951
	// (get) Token: 0x06002489 RID: 9353 RVA: 0x000AF8EC File Offset: 0x000ADAEC
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

	// Token: 0x17000F70 RID: 3952
	// (get) Token: 0x0600248A RID: 9354 RVA: 0x000143CD File Offset: 0x000125CD
	// (set) Token: 0x0600248B RID: 9355 RVA: 0x000143D5 File Offset: 0x000125D5
	public float ResolveAdd { get; private set; }

	// Token: 0x17000F71 RID: 3953
	// (get) Token: 0x0600248C RID: 9356 RVA: 0x000143DE File Offset: 0x000125DE
	// (set) Token: 0x0600248D RID: 9357 RVA: 0x000143E6 File Offset: 0x000125E6
	public float ResolveMod { get; private set; }

	// Token: 0x17000F72 RID: 3954
	// (get) Token: 0x0600248E RID: 9358 RVA: 0x000143EF File Offset: 0x000125EF
	// (set) Token: 0x0600248F RID: 9359 RVA: 0x000143F7 File Offset: 0x000125F7
	public virtual int SpellOrbs { get; set; }

	// Token: 0x17000F73 RID: 3955
	// (get) Token: 0x06002490 RID: 9360 RVA: 0x00014400 File Offset: 0x00012600
	// (set) Token: 0x06002491 RID: 9361 RVA: 0x00014408 File Offset: 0x00012608
	public bool DisableArmor { get; set; }

	// Token: 0x17000F74 RID: 3956
	// (get) Token: 0x06002492 RID: 9362 RVA: 0x00014411 File Offset: 0x00012611
	// (set) Token: 0x06002493 RID: 9363 RVA: 0x00014419 File Offset: 0x00012619
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

	// Token: 0x17000F75 RID: 3957
	// (get) Token: 0x06002494 RID: 9364 RVA: 0x000AF960 File Offset: 0x000ADB60
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

	// Token: 0x17000F76 RID: 3958
	// (get) Token: 0x06002495 RID: 9365 RVA: 0x00014431 File Offset: 0x00012631
	public override float ActualCritChance
	{
		get
		{
			return Mathf.Clamp(this.CritChanceAdd + this.CritChanceTemporaryAdd + 0f, 0f, 100f);
		}
	}

	// Token: 0x17000F77 RID: 3959
	// (get) Token: 0x06002496 RID: 9366 RVA: 0x00014455 File Offset: 0x00012655
	// (set) Token: 0x06002497 RID: 9367 RVA: 0x0001445D File Offset: 0x0001265D
	public float DexterityAdd { get; set; }

	// Token: 0x17000F78 RID: 3960
	// (get) Token: 0x06002498 RID: 9368 RVA: 0x00014466 File Offset: 0x00012666
	// (set) Token: 0x06002499 RID: 9369 RVA: 0x0001446E File Offset: 0x0001266E
	public float DexterityTemporaryAdd { get; set; }

	// Token: 0x17000F79 RID: 3961
	// (get) Token: 0x0600249A RID: 9370 RVA: 0x00014477 File Offset: 0x00012677
	// (set) Token: 0x0600249B RID: 9371 RVA: 0x0001447F File Offset: 0x0001267F
	public float DexterityMod { get; set; }

	// Token: 0x17000F7A RID: 3962
	// (get) Token: 0x0600249C RID: 9372 RVA: 0x00014488 File Offset: 0x00012688
	// (set) Token: 0x0600249D RID: 9373 RVA: 0x00014490 File Offset: 0x00012690
	public float DexterityTemporaryMod { get; set; }

	// Token: 0x17000F7B RID: 3963
	// (get) Token: 0x0600249E RID: 9374 RVA: 0x00014499 File Offset: 0x00012699
	// (set) Token: 0x0600249F RID: 9375 RVA: 0x000144A1 File Offset: 0x000126A1
	public float CritChanceAdd { get; set; }

	// Token: 0x17000F7C RID: 3964
	// (get) Token: 0x060024A0 RID: 9376 RVA: 0x000144AA File Offset: 0x000126AA
	// (set) Token: 0x060024A1 RID: 9377 RVA: 0x000144B2 File Offset: 0x000126B2
	public float CritChanceTemporaryAdd { get; set; }

	// Token: 0x17000F7D RID: 3965
	// (get) Token: 0x060024A2 RID: 9378 RVA: 0x000144BB File Offset: 0x000126BB
	// (set) Token: 0x060024A3 RID: 9379 RVA: 0x000144C3 File Offset: 0x000126C3
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

	// Token: 0x17000F7E RID: 3966
	// (get) Token: 0x060024A4 RID: 9380 RVA: 0x000144DB File Offset: 0x000126DB
	public override float ActualCritDamage
	{
		get
		{
			return Mathf.Clamp(this.BaseCritDamage + this.CritDamageAdd + this.CritDamageTemporaryAdd, 0f, float.MaxValue);
		}
	}

	// Token: 0x17000F7F RID: 3967
	// (get) Token: 0x060024A5 RID: 9381 RVA: 0x00014500 File Offset: 0x00012700
	// (set) Token: 0x060024A6 RID: 9382 RVA: 0x00014508 File Offset: 0x00012708
	public virtual float CritDamageAdd { get; set; }

	// Token: 0x17000F80 RID: 3968
	// (get) Token: 0x060024A7 RID: 9383 RVA: 0x00014511 File Offset: 0x00012711
	// (set) Token: 0x060024A8 RID: 9384 RVA: 0x00014519 File Offset: 0x00012719
	public virtual float CritDamageTemporaryAdd { get; set; }

	// Token: 0x17000F81 RID: 3969
	// (get) Token: 0x060024A9 RID: 9385 RVA: 0x00014522 File Offset: 0x00012722
	// (set) Token: 0x060024AA RID: 9386 RVA: 0x0001452A File Offset: 0x0001272A
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

	// Token: 0x17000F82 RID: 3970
	// (get) Token: 0x060024AB RID: 9387 RVA: 0x000AF9B8 File Offset: 0x000ADBB8
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

	// Token: 0x17000F83 RID: 3971
	// (get) Token: 0x060024AC RID: 9388 RVA: 0x00014542 File Offset: 0x00012742
	public virtual float ActualMagicCritChance
	{
		get
		{
			return Mathf.Clamp(this.MagicCritChanceAdd + this.MagicCritChanceTemporaryAdd + 0f, 0f, 1f);
		}
	}

	// Token: 0x17000F84 RID: 3972
	// (get) Token: 0x060024AD RID: 9389 RVA: 0x00014566 File Offset: 0x00012766
	// (set) Token: 0x060024AE RID: 9390 RVA: 0x0001456E File Offset: 0x0001276E
	public virtual float FocusAdd { get; set; }

	// Token: 0x17000F85 RID: 3973
	// (get) Token: 0x060024AF RID: 9391 RVA: 0x00014577 File Offset: 0x00012777
	// (set) Token: 0x060024B0 RID: 9392 RVA: 0x0001457F File Offset: 0x0001277F
	public virtual float FocusTemporaryAdd { get; set; }

	// Token: 0x17000F86 RID: 3974
	// (get) Token: 0x060024B1 RID: 9393 RVA: 0x00014588 File Offset: 0x00012788
	// (set) Token: 0x060024B2 RID: 9394 RVA: 0x00014590 File Offset: 0x00012790
	public float FocusMod { get; set; }

	// Token: 0x17000F87 RID: 3975
	// (get) Token: 0x060024B3 RID: 9395 RVA: 0x00014599 File Offset: 0x00012799
	// (set) Token: 0x060024B4 RID: 9396 RVA: 0x000145A1 File Offset: 0x000127A1
	public float FocusTemporaryMod { get; set; }

	// Token: 0x17000F88 RID: 3976
	// (get) Token: 0x060024B5 RID: 9397 RVA: 0x000145AA File Offset: 0x000127AA
	// (set) Token: 0x060024B6 RID: 9398 RVA: 0x000145B2 File Offset: 0x000127B2
	public float MagicCritChanceAdd { get; set; }

	// Token: 0x17000F89 RID: 3977
	// (get) Token: 0x060024B7 RID: 9399 RVA: 0x000145BB File Offset: 0x000127BB
	// (set) Token: 0x060024B8 RID: 9400 RVA: 0x000145C3 File Offset: 0x000127C3
	public float MagicCritChanceTemporaryAdd { get; set; }

	// Token: 0x17000F8A RID: 3978
	// (get) Token: 0x060024B9 RID: 9401 RVA: 0x000145CC File Offset: 0x000127CC
	// (set) Token: 0x060024BA RID: 9402 RVA: 0x000145D4 File Offset: 0x000127D4
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

	// Token: 0x17000F8B RID: 3979
	// (get) Token: 0x060024BB RID: 9403 RVA: 0x000145EC File Offset: 0x000127EC
	public float ActualMagicCritDamage
	{
		get
		{
			return Mathf.Clamp(this.BaseMagicCritDamage + this.MagicCritDamageAdd + this.MagicCritDamageTemporaryAdd, 0f, float.MaxValue);
		}
	}

	// Token: 0x17000F8C RID: 3980
	// (get) Token: 0x060024BC RID: 9404 RVA: 0x00014611 File Offset: 0x00012811
	// (set) Token: 0x060024BD RID: 9405 RVA: 0x00014619 File Offset: 0x00012819
	public float MagicCritDamageAdd { get; set; }

	// Token: 0x17000F8D RID: 3981
	// (get) Token: 0x060024BE RID: 9406 RVA: 0x00014622 File Offset: 0x00012822
	// (set) Token: 0x060024BF RID: 9407 RVA: 0x0001462A File Offset: 0x0001282A
	public float MagicCritDamageTemporaryAdd { get; set; }

	// Token: 0x17000F8E RID: 3982
	// (get) Token: 0x060024C0 RID: 9408 RVA: 0x00014633 File Offset: 0x00012833
	// (set) Token: 0x060024C1 RID: 9409 RVA: 0x0001463B File Offset: 0x0001283B
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

	// Token: 0x17000F8F RID: 3983
	// (get) Token: 0x060024C2 RID: 9410 RVA: 0x0001464F File Offset: 0x0001284F
	// (set) Token: 0x060024C3 RID: 9411 RVA: 0x00014657 File Offset: 0x00012857
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

	// Token: 0x17000F90 RID: 3984
	// (get) Token: 0x060024C4 RID: 9412 RVA: 0x0001466C File Offset: 0x0001286C
	public int ActualArmor
	{
		get
		{
			return Mathf.Clamp(this.BaseArmor + this.ArmorAdds, 0, int.MaxValue);
		}
	}

	// Token: 0x17000F91 RID: 3985
	// (get) Token: 0x060024C5 RID: 9413 RVA: 0x00014686 File Offset: 0x00012886
	// (set) Token: 0x060024C6 RID: 9414 RVA: 0x0001468E File Offset: 0x0001288E
	public int ArmorAdds { get; set; }

	// Token: 0x17000F92 RID: 3986
	// (get) Token: 0x060024C7 RID: 9415 RVA: 0x00014697 File Offset: 0x00012897
	// (set) Token: 0x060024C8 RID: 9416 RVA: 0x0001469F File Offset: 0x0001289F
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

	// Token: 0x060024C9 RID: 9417 RVA: 0x000AFA10 File Offset: 0x000ADC10
	public void SetMasteryXP(int value, bool additive)
	{
		int runAccumulatedXP = SaveManager.PlayerSaveData.RunAccumulatedXP;
		int runAccumulatedXP2 = additive ? (runAccumulatedXP + value) : value;
		SaveManager.PlayerSaveData.RunAccumulatedXP = runAccumulatedXP2;
	}

	// Token: 0x17000F93 RID: 3987
	// (get) Token: 0x060024CA RID: 9418 RVA: 0x000AFA40 File Offset: 0x000ADC40
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

	// Token: 0x17000F94 RID: 3988
	// (get) Token: 0x060024CB RID: 9419 RVA: 0x000AFA74 File Offset: 0x000ADC74
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

	// Token: 0x17000F95 RID: 3989
	// (get) Token: 0x060024CC RID: 9420 RVA: 0x000146B0 File Offset: 0x000128B0
	// (set) Token: 0x060024CD RID: 9421 RVA: 0x000146B8 File Offset: 0x000128B8
	public float CachedHealthOverride { get; set; }

	// Token: 0x17000F96 RID: 3990
	// (get) Token: 0x060024CE RID: 9422 RVA: 0x000146C1 File Offset: 0x000128C1
	// (set) Token: 0x060024CF RID: 9423 RVA: 0x000146C9 File Offset: 0x000128C9
	public float CachedManaOverride { get; set; }

	// Token: 0x17000F97 RID: 3991
	// (get) Token: 0x060024D0 RID: 9424 RVA: 0x000146D2 File Offset: 0x000128D2
	// (set) Token: 0x060024D1 RID: 9425 RVA: 0x000146DA File Offset: 0x000128DA
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

	// Token: 0x17000F98 RID: 3992
	// (get) Token: 0x060024D2 RID: 9426 RVA: 0x000146EE File Offset: 0x000128EE
	public int ClassModdedMaxHealth
	{
		get
		{
			return Mathf.CeilToInt((float)this.VitalityModdedMaxHealth * this.CharacterClass.ClassData.PassiveData.MaxHPMod);
		}
	}

	// Token: 0x17000F99 RID: 3993
	// (get) Token: 0x060024D3 RID: 9427 RVA: 0x000AFAA8 File Offset: 0x000ADCA8
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

	// Token: 0x17000F9A RID: 3994
	// (get) Token: 0x060024D4 RID: 9428 RVA: 0x00014712 File Offset: 0x00012912
	// (set) Token: 0x060024D5 RID: 9429 RVA: 0x0001471A File Offset: 0x0001291A
	public int VitalityAdd { get; set; }

	// Token: 0x17000F9B RID: 3995
	// (get) Token: 0x060024D6 RID: 9430 RVA: 0x00014723 File Offset: 0x00012923
	// (set) Token: 0x060024D7 RID: 9431 RVA: 0x0001472B File Offset: 0x0001292B
	public float VitalityMod { get; set; }

	// Token: 0x17000F9C RID: 3996
	// (get) Token: 0x060024D8 RID: 9432 RVA: 0x00014734 File Offset: 0x00012934
	public override int BaseMaxHealth
	{
		get
		{
			return Mathf.CeilToInt((float)this.BaseVitality * 10f);
		}
	}

	// Token: 0x17000F9D RID: 3997
	// (get) Token: 0x060024D9 RID: 9433 RVA: 0x00014748 File Offset: 0x00012948
	public int VitalityModdedMaxHealth
	{
		get
		{
			return Mathf.CeilToInt((float)this.ActualVitality * 10f);
		}
	}

	// Token: 0x17000F9E RID: 3998
	// (get) Token: 0x060024DA RID: 9434 RVA: 0x000AFB00 File Offset: 0x000ADD00
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

	// Token: 0x17000F9F RID: 3999
	// (get) Token: 0x060024DB RID: 9435 RVA: 0x0001475C File Offset: 0x0001295C
	// (set) Token: 0x060024DC RID: 9436 RVA: 0x00014764 File Offset: 0x00012964
	public float TraitMaxHealthMod { get; set; }

	// Token: 0x17000FA0 RID: 4000
	// (get) Token: 0x060024DD RID: 9437 RVA: 0x0001476D File Offset: 0x0001296D
	// (set) Token: 0x060024DE RID: 9438 RVA: 0x00014775 File Offset: 0x00012975
	public float RelicMaxHealthMod { get; set; }

	// Token: 0x17000FA1 RID: 4001
	// (get) Token: 0x060024DF RID: 9439 RVA: 0x0001477E File Offset: 0x0001297E
	// (set) Token: 0x060024E0 RID: 9440 RVA: 0x00014786 File Offset: 0x00012986
	public int BaseMaxMana { get; set; }

	// Token: 0x17000FA2 RID: 4002
	// (get) Token: 0x060024E1 RID: 9441 RVA: 0x0001478F File Offset: 0x0001298F
	public int ClassModdedMaxMana
	{
		get
		{
			return Mathf.CeilToInt((float)this.BaseMaxMana * this.CharacterClass.ClassData.PassiveData.MaxManaMod * (1f + this.EquipmentMaxManaMod));
		}
	}

	// Token: 0x17000FA3 RID: 4003
	// (get) Token: 0x060024E2 RID: 9442 RVA: 0x000147C0 File Offset: 0x000129C0
	public int ActualMaxMana
	{
		get
		{
			return Mathf.Clamp(Mathf.CeilToInt((float)this.ClassModdedMaxMana * (1f + this.TraitMaxManaMod)) + this.PostModMaxManaAdd, 1, int.MaxValue);
		}
	}

	// Token: 0x17000FA4 RID: 4004
	// (get) Token: 0x060024E3 RID: 9443 RVA: 0x000147ED File Offset: 0x000129ED
	// (set) Token: 0x060024E4 RID: 9444 RVA: 0x000147F5 File Offset: 0x000129F5
	public int PostModMaxManaAdd { get; set; }

	// Token: 0x17000FA5 RID: 4005
	// (get) Token: 0x060024E5 RID: 9445 RVA: 0x000147FE File Offset: 0x000129FE
	// (set) Token: 0x060024E6 RID: 9446 RVA: 0x00014806 File Offset: 0x00012A06
	public float TraitMaxManaMod { get; set; }

	// Token: 0x17000FA6 RID: 4006
	// (get) Token: 0x060024E7 RID: 9447 RVA: 0x0001480F File Offset: 0x00012A0F
	// (set) Token: 0x060024E8 RID: 9448 RVA: 0x00014817 File Offset: 0x00012A17
	public float EquipmentMaxManaMod { get; set; }

	// Token: 0x17000FA7 RID: 4007
	// (get) Token: 0x060024E9 RID: 9449 RVA: 0x00014820 File Offset: 0x00012A20
	// (set) Token: 0x060024EA RID: 9450 RVA: 0x00014828 File Offset: 0x00012A28
	public float ManaRegenMod { get; set; }

	// Token: 0x17000FA8 RID: 4008
	// (get) Token: 0x060024EB RID: 9451 RVA: 0x00014831 File Offset: 0x00012A31
	// (set) Token: 0x060024EC RID: 9452 RVA: 0x00014839 File Offset: 0x00012A39
	public float CurrentMana { get; private set; }

	// Token: 0x060024ED RID: 9453 RVA: 0x000AFB90 File Offset: 0x000ADD90
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

	// Token: 0x060024EE RID: 9454 RVA: 0x000AFC48 File Offset: 0x000ADE48
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

	// Token: 0x17000FA9 RID: 4009
	// (get) Token: 0x060024EF RID: 9455 RVA: 0x00014842 File Offset: 0x00012A42
	public int CurrentManaAsInt
	{
		get
		{
			return Mathf.CeilToInt(this.CurrentMana);
		}
	}

	// Token: 0x17000FAA RID: 4010
	// (get) Token: 0x060024F0 RID: 9456 RVA: 0x0001484F File Offset: 0x00012A4F
	// (set) Token: 0x060024F1 RID: 9457 RVA: 0x00002FCA File Offset: 0x000011CA
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

	// Token: 0x17000FAB RID: 4011
	// (get) Token: 0x060024F2 RID: 9458 RVA: 0x00014856 File Offset: 0x00012A56
	// (set) Token: 0x060024F3 RID: 9459 RVA: 0x0001485E File Offset: 0x00012A5E
	public int BaseRuneWeight { get; set; }

	// Token: 0x17000FAC RID: 4012
	// (get) Token: 0x060024F4 RID: 9460 RVA: 0x00014867 File Offset: 0x00012A67
	public int ActualRuneWeight
	{
		get
		{
			return this.BaseRuneWeight + this.RuneWeightAdds;
		}
	}

	// Token: 0x17000FAD RID: 4013
	// (get) Token: 0x060024F5 RID: 9461 RVA: 0x00014876 File Offset: 0x00012A76
	// (set) Token: 0x060024F6 RID: 9462 RVA: 0x0001487E File Offset: 0x00012A7E
	public int RuneWeightAdds { get; set; }

	// Token: 0x17000FAE RID: 4014
	// (get) Token: 0x060024F7 RID: 9463 RVA: 0x00014887 File Offset: 0x00012A87
	// (set) Token: 0x060024F8 RID: 9464 RVA: 0x0001488F File Offset: 0x00012A8F
	public int BaseAllowedEquipmentWeight { get; set; }

	// Token: 0x17000FAF RID: 4015
	// (get) Token: 0x060024F9 RID: 9465 RVA: 0x00014898 File Offset: 0x00012A98
	public int ActualAllowedEquipmentWeight
	{
		get
		{
			return this.BaseAllowedEquipmentWeight + this.AllowedEquipmentWeightAdds;
		}
	}

	// Token: 0x17000FB0 RID: 4016
	// (get) Token: 0x060024FA RID: 9466 RVA: 0x000148A7 File Offset: 0x00012AA7
	// (set) Token: 0x060024FB RID: 9467 RVA: 0x000148AF File Offset: 0x00012AAF
	public int AllowedEquipmentWeightAdds { get; set; }

	// Token: 0x17000FB1 RID: 4017
	// (get) Token: 0x060024FC RID: 9468 RVA: 0x000148B8 File Offset: 0x00012AB8
	// (set) Token: 0x060024FD RID: 9469 RVA: 0x000148C0 File Offset: 0x00012AC0
	public float AbilityCoolDownMod { get; set; }

	// Token: 0x17000FB2 RID: 4018
	// (get) Token: 0x060024FE RID: 9470 RVA: 0x000148C9 File Offset: 0x00012AC9
	// (set) Token: 0x060024FF RID: 9471 RVA: 0x000148D6 File Offset: 0x00012AD6
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

	// Token: 0x17000FB3 RID: 4019
	// (get) Token: 0x06002500 RID: 9472 RVA: 0x000148C9 File Offset: 0x00012AC9
	public float ActualMovementSpeed
	{
		get
		{
			return this.m_characterMove.MovementSpeed;
		}
	}

	// Token: 0x17000FB4 RID: 4020
	// (get) Token: 0x06002501 RID: 9473 RVA: 0x000148E4 File Offset: 0x00012AE4
	// (set) Token: 0x06002502 RID: 9474 RVA: 0x000148F1 File Offset: 0x00012AF1
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

	// Token: 0x17000FB5 RID: 4021
	// (get) Token: 0x06002503 RID: 9475 RVA: 0x00014909 File Offset: 0x00012B09
	public CharacterDash_RL CharacterDash
	{
		get
		{
			return this.m_characterDash;
		}
	}

	// Token: 0x17000FB6 RID: 4022
	// (get) Token: 0x06002504 RID: 9476 RVA: 0x00014911 File Offset: 0x00012B11
	public CharacterDownStrike_RL CharacterDownStrike
	{
		get
		{
			return this.m_characterDownStrike;
		}
	}

	// Token: 0x17000FB7 RID: 4023
	// (get) Token: 0x06002505 RID: 9477 RVA: 0x00014919 File Offset: 0x00012B19
	public CharacterClass CharacterClass
	{
		get
		{
			return this.m_characterClass;
		}
	}

	// Token: 0x17000FB8 RID: 4024
	// (get) Token: 0x06002506 RID: 9478 RVA: 0x00014921 File Offset: 0x00012B21
	public CharacterHorizontalMovement_RL CharacterMove
	{
		get
		{
			return this.m_characterMove;
		}
	}

	// Token: 0x17000FB9 RID: 4025
	// (get) Token: 0x06002507 RID: 9479 RVA: 0x00014929 File Offset: 0x00012B29
	public CharacterFlight_RL CharacterFlight
	{
		get
		{
			return this.m_characterFlight;
		}
	}

	// Token: 0x17000FBA RID: 4026
	// (get) Token: 0x06002508 RID: 9480 RVA: 0x00014931 File Offset: 0x00012B31
	public CharacterJump_RL CharacterJump
	{
		get
		{
			return this.m_characterJump;
		}
	}

	// Token: 0x17000FBB RID: 4027
	// (get) Token: 0x06002509 RID: 9481 RVA: 0x00014939 File Offset: 0x00012B39
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

	// Token: 0x17000FBC RID: 4028
	// (get) Token: 0x0600250A RID: 9482 RVA: 0x0001495A File Offset: 0x00012B5A
	public PlayerLookController LookController
	{
		get
		{
			return this.m_lookController;
		}
	}

	// Token: 0x17000FBD RID: 4029
	// (get) Token: 0x0600250B RID: 9483 RVA: 0x00014962 File Offset: 0x00012B62
	// (set) Token: 0x0600250C RID: 9484 RVA: 0x0001496A File Offset: 0x00012B6A
	public BaseRoom PreviouslyInRoom { get; private set; }

	// Token: 0x17000FBE RID: 4030
	// (get) Token: 0x0600250D RID: 9485 RVA: 0x00014973 File Offset: 0x00012B73
	// (set) Token: 0x0600250E RID: 9486 RVA: 0x0001497B File Offset: 0x00012B7B
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

	// Token: 0x17000FBF RID: 4031
	// (get) Token: 0x0600250F RID: 9487 RVA: 0x00014990 File Offset: 0x00012B90
	// (set) Token: 0x06002510 RID: 9488 RVA: 0x00014998 File Offset: 0x00012B98
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

	// Token: 0x17000FC0 RID: 4032
	// (get) Token: 0x06002511 RID: 9489 RVA: 0x00003CCB File Offset: 0x00001ECB
	// (set) Token: 0x06002512 RID: 9490 RVA: 0x00002FCA File Offset: 0x000011CA
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

	// Token: 0x17000FC1 RID: 4033
	// (get) Token: 0x06002513 RID: 9491 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float ActualKnockbackStrength
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000FC2 RID: 4034
	// (get) Token: 0x06002514 RID: 9492 RVA: 0x00003CCB File Offset: 0x00001ECB
	// (set) Token: 0x06002515 RID: 9493 RVA: 0x00002FCA File Offset: 0x000011CA
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

	// Token: 0x17000FC3 RID: 4035
	// (get) Token: 0x06002516 RID: 9494 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float ActualStunStrength
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000FC4 RID: 4036
	// (get) Token: 0x06002517 RID: 9495 RVA: 0x000046FA File Offset: 0x000028FA
	public StrikeType StrikeType
	{
		get
		{
			return StrikeType.Blunt;
		}
	}

	// Token: 0x17000FC5 RID: 4037
	// (get) Token: 0x06002518 RID: 9496 RVA: 0x000149D4 File Offset: 0x00012BD4
	// (set) Token: 0x06002519 RID: 9497 RVA: 0x000149DC File Offset: 0x00012BDC
	public Projectile_RL DamageAuraProjectile { get; set; }

	// Token: 0x0600251A RID: 9498 RVA: 0x000AFCD4 File Offset: 0x000ADED4
	protected void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.EquippedChanged, this.m_onEquippedOrLevelChanged);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.EquipmentPurchasedLevelChanged, this.m_onEquippedOrLevelChanged);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.RuneEquippedLevelChanged, this.m_onEquippedOrLevelChanged);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.RunePurchaseLevelChanged, this.m_onEquippedOrLevelChanged);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.RelicLevelChanged, this.m_onRelicChanged);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.UpdatePools, this.m_applyPermanentStatusEffects);
	}

	// Token: 0x0600251B RID: 9499 RVA: 0x000AFD30 File Offset: 0x000ADF30
	protected void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.EquippedChanged, this.m_onEquippedOrLevelChanged);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.EquipmentPurchasedLevelChanged, this.m_onEquippedOrLevelChanged);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.RuneEquippedLevelChanged, this.m_onEquippedOrLevelChanged);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.RunePurchaseLevelChanged, this.m_onEquippedOrLevelChanged);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.RelicLevelChanged, this.m_onRelicChanged);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.UpdatePools, this.m_applyPermanentStatusEffects);
	}

	// Token: 0x0600251C RID: 9500 RVA: 0x000149E5 File Offset: 0x00012BE5
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

	// Token: 0x0600251D RID: 9501 RVA: 0x000149FB File Offset: 0x00012BFB
	private void OnEquippedOrLevelChanged(MonoBehaviour sender, EventArgs args)
	{
		this.InitializeAllMods(true, true);
	}

	// Token: 0x0600251E RID: 9502 RVA: 0x000AFD8C File Offset: 0x000ADF8C
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

	// Token: 0x0600251F RID: 9503 RVA: 0x00014A05 File Offset: 0x00012C05
	public void UpdateFrameAccumulatedXP(float amount)
	{
		this.m_updateAccumulatedXP += amount;
	}

	// Token: 0x06002520 RID: 9504 RVA: 0x00014A15 File Offset: 0x00012C15
	public void UpdateFrameAccumulatedLifeSteal(float amount)
	{
		this.m_updateAccumulatedLifeSteal += amount;
	}

	// Token: 0x06002521 RID: 9505 RVA: 0x000AFDE4 File Offset: 0x000ADFE4
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

	// Token: 0x06002522 RID: 9506 RVA: 0x000AFEBC File Offset: 0x000AE0BC
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

	// Token: 0x06002523 RID: 9507 RVA: 0x00014A25 File Offset: 0x00012C25
	private void OnRelicChanged(object sender, EventArgs args)
	{
		this.InitializeAbilities();
		this.InitializeAllMods(false, false);
	}

	// Token: 0x06002524 RID: 9508 RVA: 0x000AFF60 File Offset: 0x000AE160
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

	// Token: 0x06002525 RID: 9509 RVA: 0x000AFFFC File Offset: 0x000AE1FC
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

	// Token: 0x06002526 RID: 9510 RVA: 0x000B0148 File Offset: 0x000AE348
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

	// Token: 0x06002527 RID: 9511 RVA: 0x000B0218 File Offset: 0x000AE418
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

	// Token: 0x06002528 RID: 9512 RVA: 0x000B04C4 File Offset: 0x000AE6C4
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

	// Token: 0x06002529 RID: 9513 RVA: 0x000B0588 File Offset: 0x000AE788
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

	// Token: 0x0600252A RID: 9514 RVA: 0x000B05E4 File Offset: 0x000AE7E4
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

	// Token: 0x0600252B RID: 9515 RVA: 0x000B0664 File Offset: 0x000AE864
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

	// Token: 0x0600252C RID: 9516 RVA: 0x000B0784 File Offset: 0x000AE984
	private void InitializeRelicMaxHealthMods()
	{
		float num = 0f;
		num += SaveManager.PlayerSaveData.TemporaryMaxHealthMods;
		num += (float)SaveManager.PlayerSaveData.GetRelic(RelicType.ReplacementRelic).Level * 0.1f;
		num += (float)SaveManager.PlayerSaveData.GetRelic(RelicType.FoodChallengeUsed).Level * 0.3f;
		this.RelicMaxHealthMod = num;
	}

	// Token: 0x0600252D RID: 9517 RVA: 0x000B07E8 File Offset: 0x000AE9E8
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

	// Token: 0x0600252E RID: 9518 RVA: 0x000B08CC File Offset: 0x000AEACC
	private void InitializeMaxHealthMods()
	{
		float maxHealthMod = 0f;
		base.MaxHealthMod = maxHealthMod;
		if (this.CurrentHealth > (float)this.ActualMaxHealth)
		{
			this.SetHealth((float)this.ActualMaxHealth, false, false);
		}
	}

	// Token: 0x0600252F RID: 9519 RVA: 0x000B0904 File Offset: 0x000AEB04
	public void InitializeManaMods()
	{
		this.InitializeMaxManaAdds();
		this.InitializeEquipmentMaxManaMods();
		this.InitializeTraitMaxManaMods();
		this.InitializeManaRegenMods();
		this.m_manaChangeEventArgs.Initialise(this, (float)this.ActualMaxMana, (float)this.ActualMaxMana);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerManaChange, this, this.m_manaChangeEventArgs);
	}

	// Token: 0x06002530 RID: 9520 RVA: 0x000B0954 File Offset: 0x000AEB54
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

	// Token: 0x06002531 RID: 9521 RVA: 0x000B09D4 File Offset: 0x000AEBD4
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

	// Token: 0x06002532 RID: 9522 RVA: 0x000B0A2C File Offset: 0x000AEC2C
	private void InitializeEquipmentMaxManaMods()
	{
		float equipmentMaxManaMod = 0f;
		this.EquipmentMaxManaMod = equipmentMaxManaMod;
	}

	// Token: 0x06002533 RID: 9523 RVA: 0x000B0A48 File Offset: 0x000AEC48
	private void InitializeManaRegenMods()
	{
		float num = 0f;
		num += RuneLogicHelper.GetManaRegenPercent();
		num += EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.ManaRegenMod);
		int weightLevel = EquipmentManager.GetWeightLevel();
		num += Mathf.Clamp((float)weightLevel * --0f, -1f, 0f);
		this.ManaRegenMod = num;
	}

	// Token: 0x06002534 RID: 9524 RVA: 0x000B0A98 File Offset: 0x000AEC98
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

	// Token: 0x06002535 RID: 9525 RVA: 0x000B0B20 File Offset: 0x000AED20
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

	// Token: 0x06002536 RID: 9526 RVA: 0x000B0BA8 File Offset: 0x000AEDA8
	public void InitializeAbilityMods()
	{
		float num = 0f;
		num += SkillTreeLogicHelper.GetAbilityCooldownMods();
		this.AbilityCoolDownMod = num;
	}

	// Token: 0x06002537 RID: 9527 RVA: 0x000B0BCC File Offset: 0x000AEDCC
	public void InitializeInvincibilityMods()
	{
		float num = 0f;
		num += SkillTreeLogicHelper.GetInvulnTimeExtension();
		num += (float)SaveManager.PlayerSaveData.GetRelic(RelicType.ExtendInvuln).Level * 1.25f;
		base.InvincibilityDurationAdd = num;
	}

	// Token: 0x06002538 RID: 9528 RVA: 0x000B0C0C File Offset: 0x000AEE0C
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

	// Token: 0x06002539 RID: 9529 RVA: 0x000B0C5C File Offset: 0x000AEE5C
	public void InitializeExhaustMods()
	{
		int num = 0;
		num += SaveManager.PlayerSaveData.GetRelic(RelicType.AttackExhaust).Level * 25;
		this.CurrentExhaust = num;
	}

	// Token: 0x0600253A RID: 9530 RVA: 0x000B0C8C File Offset: 0x000AEE8C
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

	// Token: 0x0600253B RID: 9531 RVA: 0x000B0D10 File Offset: 0x000AEF10
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

	// Token: 0x0600253C RID: 9532 RVA: 0x000B10B0 File Offset: 0x000AF2B0
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

	// Token: 0x0600253D RID: 9533 RVA: 0x000B11D4 File Offset: 0x000AF3D4
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

	// Token: 0x0600253E RID: 9534 RVA: 0x000B1264 File Offset: 0x000AF464
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

	// Token: 0x0600253F RID: 9535 RVA: 0x00014A35 File Offset: 0x00012C35
	public void ApplyPermanentStatusEffects(object sender, EventArgs args)
	{
		if (base.isActiveAndEnabled)
		{
			base.StartCoroutine(this.ApplyPermanentStatusEffectsCoroutine());
		}
	}

	// Token: 0x06002540 RID: 9536 RVA: 0x00014A4C File Offset: 0x00012C4C
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

	// Token: 0x06002541 RID: 9537 RVA: 0x00014A5B File Offset: 0x00012C5B
	public override void ResetHealth()
	{
		this.CurrentArmor = this.ActualArmor;
		this.CachedHealthOverride = 0f;
		base.ResetHealth();
	}

	// Token: 0x06002542 RID: 9538 RVA: 0x00014A7A File Offset: 0x00012C7A
	public void ResetMana()
	{
		this.SpellOrbs = 0;
		this.CachedManaOverride = 0f;
		this.SetMana((float)this.ActualMaxMana, false, true, false);
	}

	// Token: 0x06002543 RID: 9539 RVA: 0x000B161C File Offset: 0x000AF81C
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

	// Token: 0x06002544 RID: 9540 RVA: 0x00014A9E File Offset: 0x00012C9E
	public void ResetAbilityCooldowns()
	{
		this.m_characterAbilities.ResetAbilityCooldowns(CastAbilityType.Weapon, false);
		this.m_characterAbilities.ResetAbilityCooldowns(CastAbilityType.Talent, false);
		this.m_characterAbilities.ResetAbilityCooldowns(CastAbilityType.Spell, false);
	}

	// Token: 0x06002545 RID: 9541 RVA: 0x00014AC7 File Offset: 0x00012CC7
	public void ResetAllAbilityAmmo()
	{
		this.m_characterAbilities.ResetAbilityAmmo(CastAbilityType.Weapon, false);
		this.m_characterAbilities.ResetAbilityAmmo(CastAbilityType.Talent, false);
		this.m_characterAbilities.ResetAbilityAmmo(CastAbilityType.Spell, false);
	}

	// Token: 0x06002546 RID: 9542 RVA: 0x000B166C File Offset: 0x000AF86C
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

	// Token: 0x06002547 RID: 9543 RVA: 0x00014AF0 File Offset: 0x00012CF0
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

	// Token: 0x06002548 RID: 9544 RVA: 0x00014B29 File Offset: 0x00012D29
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

	// Token: 0x06002549 RID: 9545 RVA: 0x00014B3F File Offset: 0x00012D3F
	public void SetAllAbilitiesPermitted(bool permitted)
	{
		this.m_characterDash.AbilityPermitted = permitted;
		this.m_characterAbilities.AbilityPermitted = permitted;
		this.m_characterDownStrike.AbilityPermitted = permitted;
		this.m_characterMove.AbilityPermitted = permitted;
		this.m_characterJump.AbilityPermitted = permitted;
	}

	// Token: 0x0600254A RID: 9546 RVA: 0x000B1714 File Offset: 0x000AF914
	private float ApplyAssistDamageMods(IDamageObj damageObj, float damageTaken)
	{
		float num = damageTaken;
		if (SaveManager.PlayerSaveData.EnableHouseRules)
		{
			num *= SaveManager.PlayerSaveData.Assist_EnemyDamageMod;
		}
		return num;
	}

	// Token: 0x0600254B RID: 9547 RVA: 0x000B1740 File Offset: 0x000AF940
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

	// Token: 0x0600254C RID: 9548 RVA: 0x000B1CC4 File Offset: 0x000AFEC4
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

	// Token: 0x0600254D RID: 9549 RVA: 0x000B1E24 File Offset: 0x000B0024
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

	// Token: 0x0600254E RID: 9550 RVA: 0x000B1EE0 File Offset: 0x000B00E0
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

	// Token: 0x0600254F RID: 9551 RVA: 0x000B1F24 File Offset: 0x000B0124
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

	// Token: 0x06002550 RID: 9552 RVA: 0x00014B7D File Offset: 0x00012D7D
	public void DisableEffectsOnEnterTunnel()
	{
		base.StartCoroutine(this.DisableEffectsOnEnterTunnelCoroutine(0.1f));
	}

	// Token: 0x06002551 RID: 9553 RVA: 0x00014B91 File Offset: 0x00012D91
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

	// Token: 0x06002552 RID: 9554 RVA: 0x00014BA7 File Offset: 0x00012DA7
	private IEnumerator ResetCorgiControllerRayParameters()
	{
		while (!base.ControllerCorgi.IsInitialized)
		{
			yield return null;
		}
		base.ControllerCorgi.SetRaysParameters();
		yield break;
	}

	// Token: 0x06002553 RID: 9555 RVA: 0x000B2060 File Offset: 0x000B0260
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

	// Token: 0x06002554 RID: 9556 RVA: 0x00014BB6 File Offset: 0x00012DB6
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

	// Token: 0x06002555 RID: 9557 RVA: 0x00014BED File Offset: 0x00012DED
	public void ResumeGravity()
	{
		base.ControllerCorgi.GravityActive(true);
		if (base.ConditionState != CharacterStates.CharacterConditions.Stunned)
		{
			base.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
	}

	// Token: 0x06002556 RID: 9558 RVA: 0x000B211C File Offset: 0x000B031C
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

	// Token: 0x06002557 RID: 9559 RVA: 0x00014C0B File Offset: 0x00012E0B
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

	// Token: 0x06002558 RID: 9560 RVA: 0x00014C21 File Offset: 0x00012E21
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

	// Token: 0x06002559 RID: 9561 RVA: 0x00014C3E File Offset: 0x00012E3E
	public void StartOnHitAreaDamageTimer()
	{
		if (this.m_onHitAreaDamageCoroutine != null)
		{
			base.StopCoroutine(this.m_onHitAreaDamageCoroutine);
			this.m_onHitAreaDamageCoroutine = null;
		}
		this.m_onHitAreaDamageCoroutine = base.StartCoroutine(this.OnHitAreaDamageCoroutine());
	}

	// Token: 0x0600255A RID: 9562 RVA: 0x00014C6D File Offset: 0x00012E6D
	public void StopOnHitAreaDamageTimer()
	{
		SaveManager.PlayerSaveData.GetRelic(RelicType.OnHitAreaDamage).SetIntValue(0, false, true);
		if (this.m_onHitAreaDamageCoroutine != null)
		{
			base.StopCoroutine(this.m_onHitAreaDamageCoroutine);
			this.m_onHitAreaDamageCoroutine = null;
		}
	}

	// Token: 0x0600255B RID: 9563 RVA: 0x00014CA1 File Offset: 0x00012EA1
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

	// Token: 0x0600255C RID: 9564 RVA: 0x00014CB0 File Offset: 0x00012EB0
	public void StartNoAttackDamageBonusTimer()
	{
		if (this.m_noAttackDamageBonusCoroutine != null)
		{
			base.StopCoroutine(this.m_noAttackDamageBonusCoroutine);
			this.m_noAttackDamageBonusCoroutine = null;
		}
		this.m_noAttackDamageBonusCoroutine = base.StartCoroutine(this.NoAttackDamageBonusCoroutine());
	}

	// Token: 0x0600255D RID: 9565 RVA: 0x00014CDF File Offset: 0x00012EDF
	public void StopNoAttackDamageBonusTimer()
	{
		SaveManager.PlayerSaveData.GetRelic(RelicType.NoAttackDamageBonus).SetIntValue(0, false, true);
		if (this.m_noAttackDamageBonusCoroutine != null)
		{
			base.StopCoroutine(this.m_noAttackDamageBonusCoroutine);
			this.m_noAttackDamageBonusCoroutine = null;
		}
	}

	// Token: 0x0600255E RID: 9566 RVA: 0x00014D13 File Offset: 0x00012F13
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

	// Token: 0x0600255F RID: 9567 RVA: 0x00014D22 File Offset: 0x00012F22
	public void StartSpinKicksDropCaltropsTimer()
	{
		if (this.m_spinKicksDropCaltropsCoroutine != null)
		{
			base.StopCoroutine(this.m_spinKicksDropCaltropsCoroutine);
			this.m_spinKicksDropCaltropsCoroutine = null;
		}
		this.m_spinKicksDropCaltropsCoroutine = base.StartCoroutine(this.SpinKicksDropCaltropsCoroutine());
	}

	// Token: 0x06002560 RID: 9568 RVA: 0x00014D51 File Offset: 0x00012F51
	public void StopSpinKicksDropCaltropsTimer()
	{
		SaveManager.PlayerSaveData.GetRelic(RelicType.SpinKickLeavesCaltrops).SetIntValue(0, false, true);
		if (this.m_spinKicksDropCaltropsCoroutine != null)
		{
			base.StopCoroutine(this.m_spinKicksDropCaltropsCoroutine);
			this.m_spinKicksDropCaltropsCoroutine = null;
		}
	}

	// Token: 0x06002561 RID: 9569 RVA: 0x00014D85 File Offset: 0x00012F85
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

	// Token: 0x06002563 RID: 9571 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IDamageObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002024 RID: 8228
	private string[] m_projectileNameArray = new string[]
	{
		"CreatePlatformTalentProjectile",
		"SuperFartProjectile",
		"SporeBurstProjectile",
		"RelicOnHitAreaDamageProjectile",
		"RelicLandShockwaveProjectile",
		"RelicDamageAuraOnHitProjectile"
	};

	// Token: 0x04002025 RID: 8229
	[SerializeField]
	private GameObject m_followTargetGO;

	// Token: 0x04002026 RID: 8230
	[SerializeField]
	private GameObject m_rangeBonusRelicIndicatorGO;

	// Token: 0x04002027 RID: 8231
	private int m_baseArmor;

	// Token: 0x04002028 RID: 8232
	private float m_baseCritDamage;

	// Token: 0x04002029 RID: 8233
	private float m_baseDexterity;

	// Token: 0x0400202A RID: 8234
	private float m_baseMagicCritDamage;

	// Token: 0x0400202B RID: 8235
	private float m_baseMagicDexterity;

	// Token: 0x0400202C RID: 8236
	private float m_baseResolve;

	// Token: 0x0400202D RID: 8237
	private int m_baseVitality;

	// Token: 0x0400202E RID: 8238
	private CharacterDash_RL m_characterDash;

	// Token: 0x0400202F RID: 8239
	private CharacterDownStrike_RL m_characterDownStrike;

	// Token: 0x04002030 RID: 8240
	private CastAbility_RL m_characterAbilities;

	// Token: 0x04002031 RID: 8241
	private CharacterHorizontalMovement_RL m_characterMove;

	// Token: 0x04002032 RID: 8242
	private CharacterJump_RL m_characterJump;

	// Token: 0x04002033 RID: 8243
	private CharacterClass m_characterClass;

	// Token: 0x04002034 RID: 8244
	private CharacterFlight_RL m_characterFlight;

	// Token: 0x04002035 RID: 8245
	private PlayerLookController m_lookController;

	// Token: 0x04002036 RID: 8246
	private InteractIconController m_interactIconController;

	// Token: 0x04002037 RID: 8247
	private BaseRoom m_currentlyInRoom;

	// Token: 0x04002038 RID: 8248
	private Player m_rewiredPlayer;

	// Token: 0x04002039 RID: 8249
	private BiomeEventArgs m_biomeEventArgs;

	// Token: 0x0400203A RID: 8250
	private PlayerDeathEventArgs m_playerDeathEventArgs;

	// Token: 0x0400203B RID: 8251
	private bool m_justRolled;

	// Token: 0x0400203C RID: 8252
	private Coroutine m_rollCoroutine;

	// Token: 0x0400203D RID: 8253
	protected ManaChangeEventArgs m_manaChangeEventArgs;

	// Token: 0x0400203E RID: 8254
	private TraitChangedEventArgs m_traitChangeEventArgs;

	// Token: 0x0400203F RID: 8255
	private Coroutine m_onHitAreaDamageCoroutine;

	// Token: 0x04002040 RID: 8256
	private Coroutine m_noAttackDamageBonusCoroutine;

	// Token: 0x04002041 RID: 8257
	private Coroutine m_spinKicksDropCaltropsCoroutine;

	// Token: 0x04002042 RID: 8258
	private WaitRL_Yield m_onHitAreaDamage_waitYield;

	// Token: 0x04002043 RID: 8259
	private WaitRL_Yield m_noAttackDamageBonus_waitYield;

	// Token: 0x04002044 RID: 8260
	private Action<MonoBehaviour, EventArgs> m_onEquippedOrLevelChanged;

	// Token: 0x04002045 RID: 8261
	private Action<MonoBehaviour, EventArgs> m_onRelicChanged;

	// Token: 0x04002046 RID: 8262
	private Action<MonoBehaviour, EventArgs> m_applyPermanentStatusEffects;

	// Token: 0x0400204E RID: 8270
	protected Relay<PlayerDeathEventArgs> m_onPlayerDeathRelay = new Relay<PlayerDeathEventArgs>();

	// Token: 0x0400204F RID: 8271
	private Relay<ManaChangeEventArgs> m_manaChangeRelay = new Relay<ManaChangeEventArgs>();

	// Token: 0x04002064 RID: 8292
	private int m_currentArmor;

	// Token: 0x04002066 RID: 8294
	private int m_currentExhaust;

	// Token: 0x0400207A RID: 8314
	private float m_updateAccumulatedXP;

	// Token: 0x0400207B RID: 8315
	private float m_updateAccumulatedLifeSteal;

	// Token: 0x0400207C RID: 8316
	private TextPopupObj m_xpPopup;

	// Token: 0x0400207D RID: 8317
	private RelicChangedEventArgs m_extraLifeEventArgs = new RelicChangedEventArgs(RelicType.ExtraLife_Unity);

	// Token: 0x0400207E RID: 8318
	private Coroutine m_disableAbilitiesCoroutine;
}
