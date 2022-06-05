using System;
using UnityEngine;

// Token: 0x02000425 RID: 1061
public class LookController : MonoBehaviour
{
	// Token: 0x17000ECA RID: 3786
	// (get) Token: 0x060021E7 RID: 8679 RVA: 0x0001219B File Offset: 0x0001039B
	// (set) Token: 0x060021E8 RID: 8680 RVA: 0x000121A3 File Offset: 0x000103A3
	public bool IsInitialized { get; protected set; }

	// Token: 0x17000ECB RID: 3787
	// (get) Token: 0x060021E9 RID: 8681 RVA: 0x000121AC File Offset: 0x000103AC
	public bool IsShopModel
	{
		get
		{
			return this.m_isShopModel;
		}
	}

	// Token: 0x17000ECC RID: 3788
	// (get) Token: 0x060021EA RID: 8682 RVA: 0x000121B4 File Offset: 0x000103B4
	public bool IsPortraitModel
	{
		get
		{
			return this.m_isPortraitModel;
		}
	}

	// Token: 0x17000ECD RID: 3789
	// (get) Token: 0x060021EB RID: 8683 RVA: 0x000121BC File Offset: 0x000103BC
	public SkinnedMeshRenderer LeftEyeGeo
	{
		get
		{
			return this.m_leftEyeGeo;
		}
	}

	// Token: 0x17000ECE RID: 3790
	// (get) Token: 0x060021EC RID: 8684 RVA: 0x000121C4 File Offset: 0x000103C4
	public SkinnedMeshRenderer RightEyeGeo
	{
		get
		{
			return this.m_rightEyeGeo;
		}
	}

	// Token: 0x17000ECF RID: 3791
	// (get) Token: 0x060021ED RID: 8685 RVA: 0x000121CC File Offset: 0x000103CC
	public SkinnedMeshRenderer MouthGeo
	{
		get
		{
			return this.m_mouthGeo;
		}
	}

	// Token: 0x17000ED0 RID: 3792
	// (get) Token: 0x060021EE RID: 8686 RVA: 0x000121D4 File Offset: 0x000103D4
	public SkinnedMeshRenderer HeadGeo
	{
		get
		{
			return this.m_headGeo;
		}
	}

	// Token: 0x17000ED1 RID: 3793
	// (get) Token: 0x060021EF RID: 8687 RVA: 0x000121DC File Offset: 0x000103DC
	public SkinnedMeshRenderer HelmetGeo
	{
		get
		{
			return this.m_helmetGeo;
		}
	}

	// Token: 0x17000ED2 RID: 3794
	// (get) Token: 0x060021F0 RID: 8688 RVA: 0x000121E4 File Offset: 0x000103E4
	public SkinnedMeshRenderer ChestHairGeo
	{
		get
		{
			return this.m_chestHairGeo;
		}
	}

	// Token: 0x17000ED3 RID: 3795
	// (get) Token: 0x060021F1 RID: 8689 RVA: 0x000121EC File Offset: 0x000103EC
	public SkinnedMeshRenderer HelmetHairGeo
	{
		get
		{
			return this.m_helmetHairGeo;
		}
	}

	// Token: 0x17000ED4 RID: 3796
	// (get) Token: 0x060021F2 RID: 8690 RVA: 0x000121F4 File Offset: 0x000103F4
	public SkinnedMeshRenderer LeftShoulderGeo
	{
		get
		{
			return this.m_leftShoulderGeo;
		}
	}

	// Token: 0x17000ED5 RID: 3797
	// (get) Token: 0x060021F3 RID: 8691 RVA: 0x000121FC File Offset: 0x000103FC
	public SkinnedMeshRenderer RightShoulderGeo
	{
		get
		{
			return this.m_rightShoulderGeo;
		}
	}

	// Token: 0x17000ED6 RID: 3798
	// (get) Token: 0x060021F4 RID: 8692 RVA: 0x00012204 File Offset: 0x00010404
	public SkinnedMeshRenderer CapeGeo
	{
		get
		{
			return this.m_capeGeo;
		}
	}

	// Token: 0x17000ED7 RID: 3799
	// (get) Token: 0x060021F5 RID: 8693 RVA: 0x0001220C File Offset: 0x0001040C
	public SkinnedMeshRenderer ScarfGeo
	{
		get
		{
			return this.m_scarfGeo;
		}
	}

	// Token: 0x17000ED8 RID: 3800
	// (get) Token: 0x060021F6 RID: 8694 RVA: 0x00012214 File Offset: 0x00010414
	public SkinnedMeshRenderer ChestGeo
	{
		get
		{
			return this.m_chestGeo;
		}
	}

	// Token: 0x17000ED9 RID: 3801
	// (get) Token: 0x060021F7 RID: 8695 RVA: 0x0001221C File Offset: 0x0001041C
	public SkinnedMeshRenderer LeftHandClosedGeo
	{
		get
		{
			return this.m_leftHandClosedGeo;
		}
	}

	// Token: 0x17000EDA RID: 3802
	// (get) Token: 0x060021F8 RID: 8696 RVA: 0x00012224 File Offset: 0x00010424
	public SkinnedMeshRenderer RightHandClosedGeo
	{
		get
		{
			return this.m_rightHandClosedGeo;
		}
	}

	// Token: 0x17000EDB RID: 3803
	// (get) Token: 0x060021F9 RID: 8697 RVA: 0x0001222C File Offset: 0x0001042C
	public SkinnedMeshRenderer LeftHandOpenGeo
	{
		get
		{
			return this.m_leftHandOpenGeo;
		}
	}

	// Token: 0x17000EDC RID: 3804
	// (get) Token: 0x060021FA RID: 8698 RVA: 0x00012234 File Offset: 0x00010434
	public SkinnedMeshRenderer RightHandOpenGeo
	{
		get
		{
			return this.m_rightHandOpenGeo;
		}
	}

	// Token: 0x17000EDD RID: 3805
	// (get) Token: 0x060021FB RID: 8699 RVA: 0x0001223C File Offset: 0x0001043C
	public SkinnedMeshRenderer[] BodyGeoArray
	{
		get
		{
			return this.m_bodyGeoArray;
		}
	}

	// Token: 0x17000EDE RID: 3806
	// (get) Token: 0x060021FC RID: 8700 RVA: 0x00012244 File Offset: 0x00010444
	public GameObject KnightGeo
	{
		get
		{
			return this.m_knightGeo;
		}
	}

	// Token: 0x17000EDF RID: 3807
	// (get) Token: 0x060021FD RID: 8701 RVA: 0x0001224C File Offset: 0x0001044C
	public GameObject BarbarianGeo
	{
		get
		{
			return this.m_barbarianGeo;
		}
	}

	// Token: 0x17000EE0 RID: 3808
	// (get) Token: 0x060021FE RID: 8702 RVA: 0x00012254 File Offset: 0x00010454
	public GameObject MageGeo
	{
		get
		{
			return this.m_mageGeo;
		}
	}

	// Token: 0x17000EE1 RID: 3809
	// (get) Token: 0x060021FF RID: 8703 RVA: 0x0001225C File Offset: 0x0001045C
	public GameObject ArcherGeo
	{
		get
		{
			return this.m_archerGeo;
		}
	}

	// Token: 0x17000EE2 RID: 3810
	// (get) Token: 0x06002200 RID: 8704 RVA: 0x00012264 File Offset: 0x00010464
	public GameObject DuelistGeo
	{
		get
		{
			return this.m_duelistGeo;
		}
	}

	// Token: 0x17000EE3 RID: 3811
	// (get) Token: 0x06002201 RID: 8705 RVA: 0x0001226C File Offset: 0x0001046C
	public GameObject SpearmanGeo
	{
		get
		{
			return this.m_spearmanGeo;
		}
	}

	// Token: 0x17000EE4 RID: 3812
	// (get) Token: 0x06002202 RID: 8706 RVA: 0x00012274 File Offset: 0x00010474
	public GameObject ChefGeo
	{
		get
		{
			return this.m_chefGeo;
		}
	}

	// Token: 0x17000EE5 RID: 3813
	// (get) Token: 0x06002203 RID: 8707 RVA: 0x0001227C File Offset: 0x0001047C
	public GameObject AssassinGeo
	{
		get
		{
			return this.m_assassinGeo;
		}
	}

	// Token: 0x17000EE6 RID: 3814
	// (get) Token: 0x06002204 RID: 8708 RVA: 0x00012284 File Offset: 0x00010484
	public GameObject GunslingerGeo
	{
		get
		{
			return this.m_gunslingerGeo;
		}
	}

	// Token: 0x17000EE7 RID: 3815
	// (get) Token: 0x06002205 RID: 8709 RVA: 0x0001228C File Offset: 0x0001048C
	public GameObject BoxerGeo
	{
		get
		{
			return this.m_boxerGeo;
		}
	}

	// Token: 0x17000EE8 RID: 3816
	// (get) Token: 0x06002206 RID: 8710 RVA: 0x00012294 File Offset: 0x00010494
	public GameObject LancerGeo
	{
		get
		{
			return this.m_lancerGeo;
		}
	}

	// Token: 0x17000EE9 RID: 3817
	// (get) Token: 0x06002207 RID: 8711 RVA: 0x0001229C File Offset: 0x0001049C
	public GameObject RoninGeo
	{
		get
		{
			return this.m_roninGeo;
		}
	}

	// Token: 0x17000EEA RID: 3818
	// (get) Token: 0x06002208 RID: 8712 RVA: 0x000122A4 File Offset: 0x000104A4
	public GameObject BardGeo
	{
		get
		{
			return this.m_bardGeo;
		}
	}

	// Token: 0x17000EEB RID: 3819
	// (get) Token: 0x06002209 RID: 8713 RVA: 0x000122AC File Offset: 0x000104AC
	public GameObject PirateGeo
	{
		get
		{
			return this.m_pirateGeo;
		}
	}

	// Token: 0x17000EEC RID: 3820
	// (get) Token: 0x0600220A RID: 8714 RVA: 0x000122B4 File Offset: 0x000104B4
	public GameObject AstroGeo
	{
		get
		{
			return this.m_astroGeo;
		}
	}

	// Token: 0x17000EED RID: 3821
	// (get) Token: 0x0600220B RID: 8715 RVA: 0x000122BC File Offset: 0x000104BC
	public GameObject VisualsGameObject
	{
		get
		{
			return this.m_visualsGO;
		}
	}

	// Token: 0x17000EEE RID: 3822
	// (get) Token: 0x0600220C RID: 8716 RVA: 0x000122C4 File Offset: 0x000104C4
	public SkinnedMeshRenderer[] CurrentOutfitGeoArray
	{
		get
		{
			return this.m_outfitGeoArray;
		}
	}

	// Token: 0x17000EEF RID: 3823
	// (get) Token: 0x0600220D RID: 8717 RVA: 0x000122CC File Offset: 0x000104CC
	public SkinnedMeshRenderer SwordGeo
	{
		get
		{
			return this.m_swordGeo;
		}
	}

	// Token: 0x17000EF0 RID: 3824
	// (get) Token: 0x0600220E RID: 8718 RVA: 0x000122D4 File Offset: 0x000104D4
	public SkinnedMeshRenderer SpearGeo
	{
		get
		{
			return this.m_spearGeo;
		}
	}

	// Token: 0x17000EF1 RID: 3825
	// (get) Token: 0x0600220F RID: 8719 RVA: 0x000122DC File Offset: 0x000104DC
	public SkinnedMeshRenderer LanceGeo
	{
		get
		{
			return this.m_lanceGeo;
		}
	}

	// Token: 0x17000EF2 RID: 3826
	// (get) Token: 0x06002210 RID: 8720 RVA: 0x000122E4 File Offset: 0x000104E4
	public SkinnedMeshRenderer WandGeo
	{
		get
		{
			return this.m_staffGeo;
		}
	}

	// Token: 0x17000EF3 RID: 3827
	// (get) Token: 0x06002211 RID: 8721 RVA: 0x000122EC File Offset: 0x000104EC
	public SkinnedMeshRenderer BowGeo
	{
		get
		{
			return this.m_bowGeo;
		}
	}

	// Token: 0x17000EF4 RID: 3828
	// (get) Token: 0x06002212 RID: 8722 RVA: 0x000122F4 File Offset: 0x000104F4
	public SkinnedMeshRenderer ArrowGeo
	{
		get
		{
			return this.m_arrowGeo;
		}
	}

	// Token: 0x17000EF5 RID: 3829
	// (get) Token: 0x06002213 RID: 8723 RVA: 0x000122FC File Offset: 0x000104FC
	public SkinnedMeshRenderer SaberGeo
	{
		get
		{
			return this.m_saberGeo;
		}
	}

	// Token: 0x17000EF6 RID: 3830
	// (get) Token: 0x06002214 RID: 8724 RVA: 0x00012304 File Offset: 0x00010504
	public SkinnedMeshRenderer LadleGeo
	{
		get
		{
			return this.m_ladleGeo;
		}
	}

	// Token: 0x17000EF7 RID: 3831
	// (get) Token: 0x06002215 RID: 8725 RVA: 0x0001230C File Offset: 0x0001050C
	public SkinnedMeshRenderer AxeGeo
	{
		get
		{
			return this.m_axeGeo;
		}
	}

	// Token: 0x17000EF8 RID: 3832
	// (get) Token: 0x06002216 RID: 8726 RVA: 0x00012314 File Offset: 0x00010514
	public SkinnedMeshRenderer TonfaLeftGeo
	{
		get
		{
			return this.m_tonfaGeoL;
		}
	}

	// Token: 0x17000EF9 RID: 3833
	// (get) Token: 0x06002217 RID: 8727 RVA: 0x0001231C File Offset: 0x0001051C
	public SkinnedMeshRenderer TonfaRightGeo
	{
		get
		{
			return this.m_tonfaGeoR;
		}
	}

	// Token: 0x17000EFA RID: 3834
	// (get) Token: 0x06002218 RID: 8728 RVA: 0x00012324 File Offset: 0x00010524
	public SkinnedMeshRenderer ChakramGeo
	{
		get
		{
			return this.m_chakramGeo;
		}
	}

	// Token: 0x17000EFB RID: 3835
	// (get) Token: 0x06002219 RID: 8729 RVA: 0x0001232C File Offset: 0x0001052C
	public SkinnedMeshRenderer KunaiGeo
	{
		get
		{
			return this.m_kunaiGeo;
		}
	}

	// Token: 0x17000EFC RID: 3836
	// (get) Token: 0x0600221A RID: 8730 RVA: 0x00012334 File Offset: 0x00010534
	public SkinnedMeshRenderer DualBladeLeftGeo
	{
		get
		{
			return this.m_dualBladeL;
		}
	}

	// Token: 0x17000EFD RID: 3837
	// (get) Token: 0x0600221B RID: 8731 RVA: 0x0001233C File Offset: 0x0001053C
	public SkinnedMeshRenderer DualBladeRightGeo
	{
		get
		{
			return this.m_dualBladeR;
		}
	}

	// Token: 0x17000EFE RID: 3838
	// (get) Token: 0x0600221C RID: 8732 RVA: 0x00012344 File Offset: 0x00010544
	public SkinnedMeshRenderer FryingPanGeo
	{
		get
		{
			return this.m_fryingPanGeo;
		}
	}

	// Token: 0x17000EFF RID: 3839
	// (get) Token: 0x0600221D RID: 8733 RVA: 0x0001234C File Offset: 0x0001054C
	public SkinnedMeshRenderer PistolGeo
	{
		get
		{
			return this.m_pistolGeo;
		}
	}

	// Token: 0x17000F00 RID: 3840
	// (get) Token: 0x0600221E RID: 8734 RVA: 0x00012354 File Offset: 0x00010554
	public SkinnedMeshRenderer BoxingGloveGeo
	{
		get
		{
			return this.m_boxingGlovesGeo;
		}
	}

	// Token: 0x17000F01 RID: 3841
	// (get) Token: 0x0600221F RID: 8735 RVA: 0x0001235C File Offset: 0x0001055C
	public SkinnedMeshRenderer KatanaGeo
	{
		get
		{
			return this.m_katanaGeo;
		}
	}

	// Token: 0x17000F02 RID: 3842
	// (get) Token: 0x06002220 RID: 8736 RVA: 0x00012364 File Offset: 0x00010564
	public SkinnedMeshRenderer CannonGeo
	{
		get
		{
			return this.m_cannonGeo;
		}
	}

	// Token: 0x17000F03 RID: 3843
	// (get) Token: 0x06002221 RID: 8737 RVA: 0x0001236C File Offset: 0x0001056C
	public SkinnedMeshRenderer LuteGeo
	{
		get
		{
			return this.m_luteGeo;
		}
	}

	// Token: 0x17000F04 RID: 3844
	// (get) Token: 0x06002222 RID: 8738 RVA: 0x00012374 File Offset: 0x00010574
	public SkinnedMeshRenderer AstroWandGeo
	{
		get
		{
			return this.m_astroWandGeo;
		}
	}

	// Token: 0x17000F05 RID: 3845
	// (get) Token: 0x06002223 RID: 8739 RVA: 0x0001237C File Offset: 0x0001057C
	// (set) Token: 0x06002224 RID: 8740 RVA: 0x00012384 File Offset: 0x00010584
	public SkinnedMeshRenderer CurrentWeaponGeo { get; set; }

	// Token: 0x17000F06 RID: 3846
	// (get) Token: 0x06002225 RID: 8741 RVA: 0x0001238D File Offset: 0x0001058D
	// (set) Token: 0x06002226 RID: 8742 RVA: 0x00012395 File Offset: 0x00010595
	public SkinnedMeshRenderer SecondaryWeaponGeo { get; set; }

	// Token: 0x17000F07 RID: 3847
	// (get) Token: 0x06002227 RID: 8743 RVA: 0x0001239E File Offset: 0x0001059E
	// (set) Token: 0x06002228 RID: 8744 RVA: 0x000123A6 File Offset: 0x000105A6
	public GameObject CustomArmorMesh
	{
		get
		{
			return this.m_customArmorMesh;
		}
		set
		{
			if (value != this.m_customArmorMesh)
			{
				this.m_customGeoMeshIsDirty = true;
			}
			this.m_customArmorMesh = value;
		}
	}

	// Token: 0x17000F08 RID: 3848
	// (get) Token: 0x06002229 RID: 8745 RVA: 0x000123C4 File Offset: 0x000105C4
	// (set) Token: 0x0600222A RID: 8746 RVA: 0x000123CC File Offset: 0x000105CC
	public GameObject CustomHelmetMesh
	{
		get
		{
			return this.m_customHelmetMesh;
		}
		set
		{
			if (value != this.m_customHelmetMesh)
			{
				this.m_customGeoMeshIsDirty = true;
			}
			this.m_customHelmetMesh = value;
		}
	}

	// Token: 0x17000F09 RID: 3849
	// (get) Token: 0x0600222B RID: 8747 RVA: 0x000123EA File Offset: 0x000105EA
	public SkinnedMeshRenderer[] CustomGeoArray
	{
		get
		{
			return this.m_customGeoArray;
		}
	}

	// Token: 0x17000F0A RID: 3850
	// (get) Token: 0x0600222C RID: 8748 RVA: 0x000123F2 File Offset: 0x000105F2
	// (set) Token: 0x0600222D RID: 8749 RVA: 0x000A9074 File Offset: 0x000A7274
	public GameObject CurrentClassOutfit
	{
		get
		{
			return this.m_currentOutfit;
		}
		set
		{
			this.m_currentOutfit = value;
			if (this.m_currentOutfit != null)
			{
				if (this.m_outfitGeoArray != null)
				{
					Array.Clear(this.m_outfitGeoArray, 0, this.m_outfitGeoArray.Length);
					this.m_outfitGeoArray = null;
				}
				this.m_outfitGeoArray = this.m_currentOutfit.GetComponentsInChildren<SkinnedMeshRenderer>();
			}
		}
	}

	// Token: 0x17000F0B RID: 3851
	// (get) Token: 0x0600222E RID: 8750 RVA: 0x000123FA File Offset: 0x000105FA
	public Animator Animator
	{
		get
		{
			return this.m_animator;
		}
	}

	// Token: 0x17000F0C RID: 3852
	// (get) Token: 0x0600222F RID: 8751 RVA: 0x00012402 File Offset: 0x00010602
	public MaterialPropertyBlock PropertyBlock
	{
		get
		{
			return this.m_matPropBlock;
		}
	}

	// Token: 0x17000F0D RID: 3853
	// (get) Token: 0x06002230 RID: 8752 RVA: 0x0001240A File Offset: 0x0001060A
	// (set) Token: 0x06002231 RID: 8753 RVA: 0x00012424 File Offset: 0x00010624
	public bool GenerateRandomLook
	{
		get
		{
			return !SaveManager.IsRunning && !LineageWindowController.CharacterLoadedFromLineage && this.m_generateRandomLook;
		}
		set
		{
			this.m_generateRandomLook = value;
		}
	}

	// Token: 0x06002232 RID: 8754 RVA: 0x0001242D File Offset: 0x0001062D
	protected virtual void Awake()
	{
		if (!this.IsInitialized)
		{
			this.Initialize();
		}
	}

	// Token: 0x06002233 RID: 8755 RVA: 0x0001243D File Offset: 0x0001063D
	public virtual void Initialize()
	{
		this.m_animator = base.GetComponent<Animator>();
		this.m_matPropBlock = new MaterialPropertyBlock();
		this.m_bodyGeoArray = this.LeftEyeGeo.transform.parent.GetComponentsInChildren<SkinnedMeshRenderer>(true);
		this.IsInitialized = true;
	}

	// Token: 0x06002234 RID: 8756 RVA: 0x00012479 File Offset: 0x00010679
	public virtual void InitializeLook(CharacterData charData)
	{
		if (!this.IsInitialized)
		{
			this.Initialize();
		}
		LookCreator.InitializeClassLook(charData.ClassType, this);
		LookCreator.InitializeCharacterLook(charData, this, this.GenerateRandomLook);
		LookCreator.DisableAllWeaponGeo(this);
	}

	// Token: 0x06002235 RID: 8757 RVA: 0x000A90CC File Offset: 0x000A72CC
	protected void ApplyOutlineScale()
	{
		if (this.IsShopModel)
		{
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.BodyGeoArray)
			{
				skinnedMeshRenderer.GetPropertyBlock(this.m_matPropBlock);
				this.m_matPropBlock.SetFloat(ShaderID_RL._OutlineScale, 1.7f);
				skinnedMeshRenderer.SetPropertyBlock(this.m_matPropBlock);
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in this.CurrentOutfitGeoArray)
			{
				skinnedMeshRenderer2.GetPropertyBlock(this.m_matPropBlock);
				this.m_matPropBlock.SetFloat(ShaderID_RL._OutlineScale, 1.7f);
				skinnedMeshRenderer2.SetPropertyBlock(this.m_matPropBlock);
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer3 in this.CustomGeoArray)
			{
				skinnedMeshRenderer3.GetPropertyBlock(this.m_matPropBlock);
				this.m_matPropBlock.SetFloat(ShaderID_RL._OutlineScale, 1.7f);
				skinnedMeshRenderer3.SetPropertyBlock(this.m_matPropBlock);
			}
		}
	}

	// Token: 0x06002236 RID: 8758 RVA: 0x000124A8 File Offset: 0x000106A8
	public void SetCustomMeshDirty()
	{
		this.m_customGeoMeshIsDirty = true;
	}

	// Token: 0x06002237 RID: 8759 RVA: 0x000A91B0 File Offset: 0x000A73B0
	protected void UpdateCustomMeshArray()
	{
		if (this.m_customGeoMeshIsDirty || this.m_customGeoArray == null)
		{
			SkinnedMeshRenderer[] array = (this.m_customArmorMesh != null) ? this.m_customArmorMesh.GetComponentsInChildren<SkinnedMeshRenderer>() : null;
			SkinnedMeshRenderer[] array2 = (this.m_customHelmetMesh != null) ? this.m_customHelmetMesh.GetComponentsInChildren<SkinnedMeshRenderer>() : null;
			int num = (array != null) ? array.Length : 0;
			int num2 = (array2 != null) ? array2.Length : 0;
			this.m_customGeoArray = new SkinnedMeshRenderer[num + num2];
			if (array != null)
			{
				Array.Copy(array, this.m_customGeoArray, num);
			}
			if (array2 != null)
			{
				Array.Copy(array2, 0, this.m_customGeoArray, num, num2);
			}
			this.m_customGeoMeshIsDirty = false;
		}
	}

	// Token: 0x04001EA5 RID: 7845
	public const float SHOP_MODEL_OUTLINE_SCALE = 1.7f;

	// Token: 0x04001EA6 RID: 7846
	[SerializeField]
	private bool m_isShopModel;

	// Token: 0x04001EA7 RID: 7847
	[SerializeField]
	private bool m_isPortraitModel;

	// Token: 0x04001EA8 RID: 7848
	[Header("Debug")]
	[SerializeField]
	private bool m_generateRandomLook;

	// Token: 0x04001EA9 RID: 7849
	[Header("Player Geo")]
	[SerializeField]
	private SkinnedMeshRenderer m_leftEyeGeo;

	// Token: 0x04001EAA RID: 7850
	[SerializeField]
	private SkinnedMeshRenderer m_rightEyeGeo;

	// Token: 0x04001EAB RID: 7851
	[SerializeField]
	private SkinnedMeshRenderer m_mouthGeo;

	// Token: 0x04001EAC RID: 7852
	[SerializeField]
	private SkinnedMeshRenderer m_headGeo;

	// Token: 0x04001EAD RID: 7853
	[SerializeField]
	private SkinnedMeshRenderer m_helmetGeo;

	// Token: 0x04001EAE RID: 7854
	[SerializeField]
	private SkinnedMeshRenderer m_chestHairGeo;

	// Token: 0x04001EAF RID: 7855
	[SerializeField]
	private SkinnedMeshRenderer m_helmetHairGeo;

	// Token: 0x04001EB0 RID: 7856
	[SerializeField]
	private SkinnedMeshRenderer m_leftShoulderGeo;

	// Token: 0x04001EB1 RID: 7857
	[SerializeField]
	private SkinnedMeshRenderer m_rightShoulderGeo;

	// Token: 0x04001EB2 RID: 7858
	[SerializeField]
	private SkinnedMeshRenderer m_capeGeo;

	// Token: 0x04001EB3 RID: 7859
	[SerializeField]
	private SkinnedMeshRenderer m_scarfGeo;

	// Token: 0x04001EB4 RID: 7860
	[SerializeField]
	private SkinnedMeshRenderer m_chestGeo;

	// Token: 0x04001EB5 RID: 7861
	[SerializeField]
	private SkinnedMeshRenderer m_leftHandClosedGeo;

	// Token: 0x04001EB6 RID: 7862
	[SerializeField]
	private SkinnedMeshRenderer m_rightHandClosedGeo;

	// Token: 0x04001EB7 RID: 7863
	[SerializeField]
	private SkinnedMeshRenderer m_leftHandOpenGeo;

	// Token: 0x04001EB8 RID: 7864
	[SerializeField]
	private SkinnedMeshRenderer m_rightHandOpenGeo;

	// Token: 0x04001EB9 RID: 7865
	[Header("Player Class Geo", order = 10)]
	[SerializeField]
	private GameObject m_knightGeo;

	// Token: 0x04001EBA RID: 7866
	[SerializeField]
	private GameObject m_barbarianGeo;

	// Token: 0x04001EBB RID: 7867
	[SerializeField]
	private GameObject m_mageGeo;

	// Token: 0x04001EBC RID: 7868
	[SerializeField]
	private GameObject m_archerGeo;

	// Token: 0x04001EBD RID: 7869
	[SerializeField]
	private GameObject m_duelistGeo;

	// Token: 0x04001EBE RID: 7870
	[SerializeField]
	private GameObject m_spearmanGeo;

	// Token: 0x04001EBF RID: 7871
	[SerializeField]
	private GameObject m_chefGeo;

	// Token: 0x04001EC0 RID: 7872
	[SerializeField]
	private GameObject m_assassinGeo;

	// Token: 0x04001EC1 RID: 7873
	[SerializeField]
	private GameObject m_gunslingerGeo;

	// Token: 0x04001EC2 RID: 7874
	[SerializeField]
	private GameObject m_boxerGeo;

	// Token: 0x04001EC3 RID: 7875
	[SerializeField]
	private GameObject m_lancerGeo;

	// Token: 0x04001EC4 RID: 7876
	[SerializeField]
	private GameObject m_roninGeo;

	// Token: 0x04001EC5 RID: 7877
	[SerializeField]
	private GameObject m_pirateGeo;

	// Token: 0x04001EC6 RID: 7878
	[SerializeField]
	private GameObject m_bardGeo;

	// Token: 0x04001EC7 RID: 7879
	[SerializeField]
	private GameObject m_astroGeo;

	// Token: 0x04001EC8 RID: 7880
	[Header("Player Weapon Geo")]
	[SerializeField]
	private SkinnedMeshRenderer m_swordGeo;

	// Token: 0x04001EC9 RID: 7881
	[SerializeField]
	private SkinnedMeshRenderer m_spearGeo;

	// Token: 0x04001ECA RID: 7882
	[SerializeField]
	private SkinnedMeshRenderer m_lanceGeo;

	// Token: 0x04001ECB RID: 7883
	[SerializeField]
	private SkinnedMeshRenderer m_staffGeo;

	// Token: 0x04001ECC RID: 7884
	[SerializeField]
	private SkinnedMeshRenderer m_bowGeo;

	// Token: 0x04001ECD RID: 7885
	[SerializeField]
	private SkinnedMeshRenderer m_arrowGeo;

	// Token: 0x04001ECE RID: 7886
	[SerializeField]
	private SkinnedMeshRenderer m_saberGeo;

	// Token: 0x04001ECF RID: 7887
	[SerializeField]
	private SkinnedMeshRenderer m_ladleGeo;

	// Token: 0x04001ED0 RID: 7888
	[SerializeField]
	private SkinnedMeshRenderer m_axeGeo;

	// Token: 0x04001ED1 RID: 7889
	[SerializeField]
	private SkinnedMeshRenderer m_kunaiGeo;

	// Token: 0x04001ED2 RID: 7890
	[SerializeField]
	private SkinnedMeshRenderer m_tonfaGeoL;

	// Token: 0x04001ED3 RID: 7891
	[SerializeField]
	private SkinnedMeshRenderer m_tonfaGeoR;

	// Token: 0x04001ED4 RID: 7892
	[SerializeField]
	private SkinnedMeshRenderer m_chakramGeo;

	// Token: 0x04001ED5 RID: 7893
	[SerializeField]
	private SkinnedMeshRenderer m_dualBladeL;

	// Token: 0x04001ED6 RID: 7894
	[SerializeField]
	private SkinnedMeshRenderer m_dualBladeR;

	// Token: 0x04001ED7 RID: 7895
	[SerializeField]
	private SkinnedMeshRenderer m_fryingPanGeo;

	// Token: 0x04001ED8 RID: 7896
	[SerializeField]
	private SkinnedMeshRenderer m_pistolGeo;

	// Token: 0x04001ED9 RID: 7897
	[SerializeField]
	private SkinnedMeshRenderer m_boxingGlovesGeo;

	// Token: 0x04001EDA RID: 7898
	[SerializeField]
	private SkinnedMeshRenderer m_katanaGeo;

	// Token: 0x04001EDB RID: 7899
	[SerializeField]
	private SkinnedMeshRenderer m_cannonGeo;

	// Token: 0x04001EDC RID: 7900
	[SerializeField]
	private SkinnedMeshRenderer m_luteGeo;

	// Token: 0x04001EDD RID: 7901
	[SerializeField]
	private SkinnedMeshRenderer m_astroWandGeo;

	// Token: 0x04001EDE RID: 7902
	[Header("Misc Geo")]
	[SerializeField]
	private GameObject m_visualsGO;

	// Token: 0x04001EDF RID: 7903
	private Animator m_animator;

	// Token: 0x04001EE0 RID: 7904
	private MaterialPropertyBlock m_matPropBlock;

	// Token: 0x04001EE1 RID: 7905
	private SkinnedMeshRenderer[] m_bodyGeoArray;

	// Token: 0x04001EE2 RID: 7906
	private SkinnedMeshRenderer[] m_outfitGeoArray;

	// Token: 0x04001EE3 RID: 7907
	private SkinnedMeshRenderer[] m_customGeoArray;

	// Token: 0x04001EE4 RID: 7908
	private GameObject m_currentOutfit;

	// Token: 0x04001EE5 RID: 7909
	private GameObject m_customArmorMesh;

	// Token: 0x04001EE6 RID: 7910
	private GameObject m_customHelmetMesh;

	// Token: 0x04001EE7 RID: 7911
	private bool m_customGeoMeshIsDirty;
}
