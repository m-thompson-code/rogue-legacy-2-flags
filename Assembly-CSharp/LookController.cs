using System;
using UnityEngine;

// Token: 0x02000261 RID: 609
public class LookController : MonoBehaviour
{
	// Token: 0x17000B91 RID: 2961
	// (get) Token: 0x06001810 RID: 6160 RVA: 0x0004AFBD File Offset: 0x000491BD
	// (set) Token: 0x06001811 RID: 6161 RVA: 0x0004AFC5 File Offset: 0x000491C5
	public bool IsInitialized { get; protected set; }

	// Token: 0x17000B92 RID: 2962
	// (get) Token: 0x06001812 RID: 6162 RVA: 0x0004AFCE File Offset: 0x000491CE
	public bool IsShopModel
	{
		get
		{
			return this.m_isShopModel;
		}
	}

	// Token: 0x17000B93 RID: 2963
	// (get) Token: 0x06001813 RID: 6163 RVA: 0x0004AFD6 File Offset: 0x000491D6
	public bool IsPortraitModel
	{
		get
		{
			return this.m_isPortraitModel;
		}
	}

	// Token: 0x17000B94 RID: 2964
	// (get) Token: 0x06001814 RID: 6164 RVA: 0x0004AFDE File Offset: 0x000491DE
	public SkinnedMeshRenderer LeftEyeGeo
	{
		get
		{
			return this.m_leftEyeGeo;
		}
	}

	// Token: 0x17000B95 RID: 2965
	// (get) Token: 0x06001815 RID: 6165 RVA: 0x0004AFE6 File Offset: 0x000491E6
	public SkinnedMeshRenderer RightEyeGeo
	{
		get
		{
			return this.m_rightEyeGeo;
		}
	}

	// Token: 0x17000B96 RID: 2966
	// (get) Token: 0x06001816 RID: 6166 RVA: 0x0004AFEE File Offset: 0x000491EE
	public SkinnedMeshRenderer MouthGeo
	{
		get
		{
			return this.m_mouthGeo;
		}
	}

	// Token: 0x17000B97 RID: 2967
	// (get) Token: 0x06001817 RID: 6167 RVA: 0x0004AFF6 File Offset: 0x000491F6
	public SkinnedMeshRenderer HeadGeo
	{
		get
		{
			return this.m_headGeo;
		}
	}

	// Token: 0x17000B98 RID: 2968
	// (get) Token: 0x06001818 RID: 6168 RVA: 0x0004AFFE File Offset: 0x000491FE
	public SkinnedMeshRenderer HelmetGeo
	{
		get
		{
			return this.m_helmetGeo;
		}
	}

	// Token: 0x17000B99 RID: 2969
	// (get) Token: 0x06001819 RID: 6169 RVA: 0x0004B006 File Offset: 0x00049206
	public SkinnedMeshRenderer ChestHairGeo
	{
		get
		{
			return this.m_chestHairGeo;
		}
	}

	// Token: 0x17000B9A RID: 2970
	// (get) Token: 0x0600181A RID: 6170 RVA: 0x0004B00E File Offset: 0x0004920E
	public SkinnedMeshRenderer HelmetHairGeo
	{
		get
		{
			return this.m_helmetHairGeo;
		}
	}

	// Token: 0x17000B9B RID: 2971
	// (get) Token: 0x0600181B RID: 6171 RVA: 0x0004B016 File Offset: 0x00049216
	public SkinnedMeshRenderer LeftShoulderGeo
	{
		get
		{
			return this.m_leftShoulderGeo;
		}
	}

	// Token: 0x17000B9C RID: 2972
	// (get) Token: 0x0600181C RID: 6172 RVA: 0x0004B01E File Offset: 0x0004921E
	public SkinnedMeshRenderer RightShoulderGeo
	{
		get
		{
			return this.m_rightShoulderGeo;
		}
	}

	// Token: 0x17000B9D RID: 2973
	// (get) Token: 0x0600181D RID: 6173 RVA: 0x0004B026 File Offset: 0x00049226
	public SkinnedMeshRenderer CapeGeo
	{
		get
		{
			return this.m_capeGeo;
		}
	}

	// Token: 0x17000B9E RID: 2974
	// (get) Token: 0x0600181E RID: 6174 RVA: 0x0004B02E File Offset: 0x0004922E
	public SkinnedMeshRenderer ScarfGeo
	{
		get
		{
			return this.m_scarfGeo;
		}
	}

	// Token: 0x17000B9F RID: 2975
	// (get) Token: 0x0600181F RID: 6175 RVA: 0x0004B036 File Offset: 0x00049236
	public SkinnedMeshRenderer ChestGeo
	{
		get
		{
			return this.m_chestGeo;
		}
	}

	// Token: 0x17000BA0 RID: 2976
	// (get) Token: 0x06001820 RID: 6176 RVA: 0x0004B03E File Offset: 0x0004923E
	public SkinnedMeshRenderer LeftHandClosedGeo
	{
		get
		{
			return this.m_leftHandClosedGeo;
		}
	}

	// Token: 0x17000BA1 RID: 2977
	// (get) Token: 0x06001821 RID: 6177 RVA: 0x0004B046 File Offset: 0x00049246
	public SkinnedMeshRenderer RightHandClosedGeo
	{
		get
		{
			return this.m_rightHandClosedGeo;
		}
	}

	// Token: 0x17000BA2 RID: 2978
	// (get) Token: 0x06001822 RID: 6178 RVA: 0x0004B04E File Offset: 0x0004924E
	public SkinnedMeshRenderer LeftHandOpenGeo
	{
		get
		{
			return this.m_leftHandOpenGeo;
		}
	}

	// Token: 0x17000BA3 RID: 2979
	// (get) Token: 0x06001823 RID: 6179 RVA: 0x0004B056 File Offset: 0x00049256
	public SkinnedMeshRenderer RightHandOpenGeo
	{
		get
		{
			return this.m_rightHandOpenGeo;
		}
	}

	// Token: 0x17000BA4 RID: 2980
	// (get) Token: 0x06001824 RID: 6180 RVA: 0x0004B05E File Offset: 0x0004925E
	public SkinnedMeshRenderer[] BodyGeoArray
	{
		get
		{
			return this.m_bodyGeoArray;
		}
	}

	// Token: 0x17000BA5 RID: 2981
	// (get) Token: 0x06001825 RID: 6181 RVA: 0x0004B066 File Offset: 0x00049266
	public GameObject KnightGeo
	{
		get
		{
			return this.m_knightGeo;
		}
	}

	// Token: 0x17000BA6 RID: 2982
	// (get) Token: 0x06001826 RID: 6182 RVA: 0x0004B06E File Offset: 0x0004926E
	public GameObject BarbarianGeo
	{
		get
		{
			return this.m_barbarianGeo;
		}
	}

	// Token: 0x17000BA7 RID: 2983
	// (get) Token: 0x06001827 RID: 6183 RVA: 0x0004B076 File Offset: 0x00049276
	public GameObject MageGeo
	{
		get
		{
			return this.m_mageGeo;
		}
	}

	// Token: 0x17000BA8 RID: 2984
	// (get) Token: 0x06001828 RID: 6184 RVA: 0x0004B07E File Offset: 0x0004927E
	public GameObject ArcherGeo
	{
		get
		{
			return this.m_archerGeo;
		}
	}

	// Token: 0x17000BA9 RID: 2985
	// (get) Token: 0x06001829 RID: 6185 RVA: 0x0004B086 File Offset: 0x00049286
	public GameObject DuelistGeo
	{
		get
		{
			return this.m_duelistGeo;
		}
	}

	// Token: 0x17000BAA RID: 2986
	// (get) Token: 0x0600182A RID: 6186 RVA: 0x0004B08E File Offset: 0x0004928E
	public GameObject SpearmanGeo
	{
		get
		{
			return this.m_spearmanGeo;
		}
	}

	// Token: 0x17000BAB RID: 2987
	// (get) Token: 0x0600182B RID: 6187 RVA: 0x0004B096 File Offset: 0x00049296
	public GameObject ChefGeo
	{
		get
		{
			return this.m_chefGeo;
		}
	}

	// Token: 0x17000BAC RID: 2988
	// (get) Token: 0x0600182C RID: 6188 RVA: 0x0004B09E File Offset: 0x0004929E
	public GameObject AssassinGeo
	{
		get
		{
			return this.m_assassinGeo;
		}
	}

	// Token: 0x17000BAD RID: 2989
	// (get) Token: 0x0600182D RID: 6189 RVA: 0x0004B0A6 File Offset: 0x000492A6
	public GameObject GunslingerGeo
	{
		get
		{
			return this.m_gunslingerGeo;
		}
	}

	// Token: 0x17000BAE RID: 2990
	// (get) Token: 0x0600182E RID: 6190 RVA: 0x0004B0AE File Offset: 0x000492AE
	public GameObject BoxerGeo
	{
		get
		{
			return this.m_boxerGeo;
		}
	}

	// Token: 0x17000BAF RID: 2991
	// (get) Token: 0x0600182F RID: 6191 RVA: 0x0004B0B6 File Offset: 0x000492B6
	public GameObject LancerGeo
	{
		get
		{
			return this.m_lancerGeo;
		}
	}

	// Token: 0x17000BB0 RID: 2992
	// (get) Token: 0x06001830 RID: 6192 RVA: 0x0004B0BE File Offset: 0x000492BE
	public GameObject RoninGeo
	{
		get
		{
			return this.m_roninGeo;
		}
	}

	// Token: 0x17000BB1 RID: 2993
	// (get) Token: 0x06001831 RID: 6193 RVA: 0x0004B0C6 File Offset: 0x000492C6
	public GameObject BardGeo
	{
		get
		{
			return this.m_bardGeo;
		}
	}

	// Token: 0x17000BB2 RID: 2994
	// (get) Token: 0x06001832 RID: 6194 RVA: 0x0004B0CE File Offset: 0x000492CE
	public GameObject PirateGeo
	{
		get
		{
			return this.m_pirateGeo;
		}
	}

	// Token: 0x17000BB3 RID: 2995
	// (get) Token: 0x06001833 RID: 6195 RVA: 0x0004B0D6 File Offset: 0x000492D6
	public GameObject AstroGeo
	{
		get
		{
			return this.m_astroGeo;
		}
	}

	// Token: 0x17000BB4 RID: 2996
	// (get) Token: 0x06001834 RID: 6196 RVA: 0x0004B0DE File Offset: 0x000492DE
	public GameObject VisualsGameObject
	{
		get
		{
			return this.m_visualsGO;
		}
	}

	// Token: 0x17000BB5 RID: 2997
	// (get) Token: 0x06001835 RID: 6197 RVA: 0x0004B0E6 File Offset: 0x000492E6
	public SkinnedMeshRenderer[] CurrentOutfitGeoArray
	{
		get
		{
			return this.m_outfitGeoArray;
		}
	}

	// Token: 0x17000BB6 RID: 2998
	// (get) Token: 0x06001836 RID: 6198 RVA: 0x0004B0EE File Offset: 0x000492EE
	public SkinnedMeshRenderer SwordGeo
	{
		get
		{
			return this.m_swordGeo;
		}
	}

	// Token: 0x17000BB7 RID: 2999
	// (get) Token: 0x06001837 RID: 6199 RVA: 0x0004B0F6 File Offset: 0x000492F6
	public SkinnedMeshRenderer SpearGeo
	{
		get
		{
			return this.m_spearGeo;
		}
	}

	// Token: 0x17000BB8 RID: 3000
	// (get) Token: 0x06001838 RID: 6200 RVA: 0x0004B0FE File Offset: 0x000492FE
	public SkinnedMeshRenderer LanceGeo
	{
		get
		{
			return this.m_lanceGeo;
		}
	}

	// Token: 0x17000BB9 RID: 3001
	// (get) Token: 0x06001839 RID: 6201 RVA: 0x0004B106 File Offset: 0x00049306
	public SkinnedMeshRenderer WandGeo
	{
		get
		{
			return this.m_staffGeo;
		}
	}

	// Token: 0x17000BBA RID: 3002
	// (get) Token: 0x0600183A RID: 6202 RVA: 0x0004B10E File Offset: 0x0004930E
	public SkinnedMeshRenderer BowGeo
	{
		get
		{
			return this.m_bowGeo;
		}
	}

	// Token: 0x17000BBB RID: 3003
	// (get) Token: 0x0600183B RID: 6203 RVA: 0x0004B116 File Offset: 0x00049316
	public SkinnedMeshRenderer ArrowGeo
	{
		get
		{
			return this.m_arrowGeo;
		}
	}

	// Token: 0x17000BBC RID: 3004
	// (get) Token: 0x0600183C RID: 6204 RVA: 0x0004B11E File Offset: 0x0004931E
	public SkinnedMeshRenderer SaberGeo
	{
		get
		{
			return this.m_saberGeo;
		}
	}

	// Token: 0x17000BBD RID: 3005
	// (get) Token: 0x0600183D RID: 6205 RVA: 0x0004B126 File Offset: 0x00049326
	public SkinnedMeshRenderer LadleGeo
	{
		get
		{
			return this.m_ladleGeo;
		}
	}

	// Token: 0x17000BBE RID: 3006
	// (get) Token: 0x0600183E RID: 6206 RVA: 0x0004B12E File Offset: 0x0004932E
	public SkinnedMeshRenderer AxeGeo
	{
		get
		{
			return this.m_axeGeo;
		}
	}

	// Token: 0x17000BBF RID: 3007
	// (get) Token: 0x0600183F RID: 6207 RVA: 0x0004B136 File Offset: 0x00049336
	public SkinnedMeshRenderer TonfaLeftGeo
	{
		get
		{
			return this.m_tonfaGeoL;
		}
	}

	// Token: 0x17000BC0 RID: 3008
	// (get) Token: 0x06001840 RID: 6208 RVA: 0x0004B13E File Offset: 0x0004933E
	public SkinnedMeshRenderer TonfaRightGeo
	{
		get
		{
			return this.m_tonfaGeoR;
		}
	}

	// Token: 0x17000BC1 RID: 3009
	// (get) Token: 0x06001841 RID: 6209 RVA: 0x0004B146 File Offset: 0x00049346
	public SkinnedMeshRenderer ChakramGeo
	{
		get
		{
			return this.m_chakramGeo;
		}
	}

	// Token: 0x17000BC2 RID: 3010
	// (get) Token: 0x06001842 RID: 6210 RVA: 0x0004B14E File Offset: 0x0004934E
	public SkinnedMeshRenderer KunaiGeo
	{
		get
		{
			return this.m_kunaiGeo;
		}
	}

	// Token: 0x17000BC3 RID: 3011
	// (get) Token: 0x06001843 RID: 6211 RVA: 0x0004B156 File Offset: 0x00049356
	public SkinnedMeshRenderer DualBladeLeftGeo
	{
		get
		{
			return this.m_dualBladeL;
		}
	}

	// Token: 0x17000BC4 RID: 3012
	// (get) Token: 0x06001844 RID: 6212 RVA: 0x0004B15E File Offset: 0x0004935E
	public SkinnedMeshRenderer DualBladeRightGeo
	{
		get
		{
			return this.m_dualBladeR;
		}
	}

	// Token: 0x17000BC5 RID: 3013
	// (get) Token: 0x06001845 RID: 6213 RVA: 0x0004B166 File Offset: 0x00049366
	public SkinnedMeshRenderer FryingPanGeo
	{
		get
		{
			return this.m_fryingPanGeo;
		}
	}

	// Token: 0x17000BC6 RID: 3014
	// (get) Token: 0x06001846 RID: 6214 RVA: 0x0004B16E File Offset: 0x0004936E
	public SkinnedMeshRenderer PistolGeo
	{
		get
		{
			return this.m_pistolGeo;
		}
	}

	// Token: 0x17000BC7 RID: 3015
	// (get) Token: 0x06001847 RID: 6215 RVA: 0x0004B176 File Offset: 0x00049376
	public SkinnedMeshRenderer BoxingGloveGeo
	{
		get
		{
			return this.m_boxingGlovesGeo;
		}
	}

	// Token: 0x17000BC8 RID: 3016
	// (get) Token: 0x06001848 RID: 6216 RVA: 0x0004B17E File Offset: 0x0004937E
	public SkinnedMeshRenderer KatanaGeo
	{
		get
		{
			return this.m_katanaGeo;
		}
	}

	// Token: 0x17000BC9 RID: 3017
	// (get) Token: 0x06001849 RID: 6217 RVA: 0x0004B186 File Offset: 0x00049386
	public SkinnedMeshRenderer CannonGeo
	{
		get
		{
			return this.m_cannonGeo;
		}
	}

	// Token: 0x17000BCA RID: 3018
	// (get) Token: 0x0600184A RID: 6218 RVA: 0x0004B18E File Offset: 0x0004938E
	public SkinnedMeshRenderer LuteGeo
	{
		get
		{
			return this.m_luteGeo;
		}
	}

	// Token: 0x17000BCB RID: 3019
	// (get) Token: 0x0600184B RID: 6219 RVA: 0x0004B196 File Offset: 0x00049396
	public SkinnedMeshRenderer AstroWandGeo
	{
		get
		{
			return this.m_astroWandGeo;
		}
	}

	// Token: 0x17000BCC RID: 3020
	// (get) Token: 0x0600184C RID: 6220 RVA: 0x0004B19E File Offset: 0x0004939E
	// (set) Token: 0x0600184D RID: 6221 RVA: 0x0004B1A6 File Offset: 0x000493A6
	public SkinnedMeshRenderer CurrentWeaponGeo { get; set; }

	// Token: 0x17000BCD RID: 3021
	// (get) Token: 0x0600184E RID: 6222 RVA: 0x0004B1AF File Offset: 0x000493AF
	// (set) Token: 0x0600184F RID: 6223 RVA: 0x0004B1B7 File Offset: 0x000493B7
	public SkinnedMeshRenderer SecondaryWeaponGeo { get; set; }

	// Token: 0x17000BCE RID: 3022
	// (get) Token: 0x06001850 RID: 6224 RVA: 0x0004B1C0 File Offset: 0x000493C0
	// (set) Token: 0x06001851 RID: 6225 RVA: 0x0004B1C8 File Offset: 0x000493C8
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

	// Token: 0x17000BCF RID: 3023
	// (get) Token: 0x06001852 RID: 6226 RVA: 0x0004B1E6 File Offset: 0x000493E6
	// (set) Token: 0x06001853 RID: 6227 RVA: 0x0004B1EE File Offset: 0x000493EE
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

	// Token: 0x17000BD0 RID: 3024
	// (get) Token: 0x06001854 RID: 6228 RVA: 0x0004B20C File Offset: 0x0004940C
	public SkinnedMeshRenderer[] CustomGeoArray
	{
		get
		{
			return this.m_customGeoArray;
		}
	}

	// Token: 0x17000BD1 RID: 3025
	// (get) Token: 0x06001855 RID: 6229 RVA: 0x0004B214 File Offset: 0x00049414
	// (set) Token: 0x06001856 RID: 6230 RVA: 0x0004B21C File Offset: 0x0004941C
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

	// Token: 0x17000BD2 RID: 3026
	// (get) Token: 0x06001857 RID: 6231 RVA: 0x0004B272 File Offset: 0x00049472
	public Animator Animator
	{
		get
		{
			return this.m_animator;
		}
	}

	// Token: 0x17000BD3 RID: 3027
	// (get) Token: 0x06001858 RID: 6232 RVA: 0x0004B27A File Offset: 0x0004947A
	public MaterialPropertyBlock PropertyBlock
	{
		get
		{
			return this.m_matPropBlock;
		}
	}

	// Token: 0x17000BD4 RID: 3028
	// (get) Token: 0x06001859 RID: 6233 RVA: 0x0004B282 File Offset: 0x00049482
	// (set) Token: 0x0600185A RID: 6234 RVA: 0x0004B29C File Offset: 0x0004949C
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

	// Token: 0x0600185B RID: 6235 RVA: 0x0004B2A5 File Offset: 0x000494A5
	protected virtual void Awake()
	{
		if (!this.IsInitialized)
		{
			this.Initialize();
		}
	}

	// Token: 0x0600185C RID: 6236 RVA: 0x0004B2B5 File Offset: 0x000494B5
	public virtual void Initialize()
	{
		this.m_animator = base.GetComponent<Animator>();
		this.m_matPropBlock = new MaterialPropertyBlock();
		this.m_bodyGeoArray = this.LeftEyeGeo.transform.parent.GetComponentsInChildren<SkinnedMeshRenderer>(true);
		this.IsInitialized = true;
	}

	// Token: 0x0600185D RID: 6237 RVA: 0x0004B2F1 File Offset: 0x000494F1
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

	// Token: 0x0600185E RID: 6238 RVA: 0x0004B320 File Offset: 0x00049520
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

	// Token: 0x0600185F RID: 6239 RVA: 0x0004B404 File Offset: 0x00049604
	public void SetCustomMeshDirty()
	{
		this.m_customGeoMeshIsDirty = true;
	}

	// Token: 0x06001860 RID: 6240 RVA: 0x0004B410 File Offset: 0x00049610
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

	// Token: 0x04001773 RID: 6003
	public const float SHOP_MODEL_OUTLINE_SCALE = 1.7f;

	// Token: 0x04001774 RID: 6004
	[SerializeField]
	private bool m_isShopModel;

	// Token: 0x04001775 RID: 6005
	[SerializeField]
	private bool m_isPortraitModel;

	// Token: 0x04001776 RID: 6006
	[Header("Debug")]
	[SerializeField]
	private bool m_generateRandomLook;

	// Token: 0x04001777 RID: 6007
	[Header("Player Geo")]
	[SerializeField]
	private SkinnedMeshRenderer m_leftEyeGeo;

	// Token: 0x04001778 RID: 6008
	[SerializeField]
	private SkinnedMeshRenderer m_rightEyeGeo;

	// Token: 0x04001779 RID: 6009
	[SerializeField]
	private SkinnedMeshRenderer m_mouthGeo;

	// Token: 0x0400177A RID: 6010
	[SerializeField]
	private SkinnedMeshRenderer m_headGeo;

	// Token: 0x0400177B RID: 6011
	[SerializeField]
	private SkinnedMeshRenderer m_helmetGeo;

	// Token: 0x0400177C RID: 6012
	[SerializeField]
	private SkinnedMeshRenderer m_chestHairGeo;

	// Token: 0x0400177D RID: 6013
	[SerializeField]
	private SkinnedMeshRenderer m_helmetHairGeo;

	// Token: 0x0400177E RID: 6014
	[SerializeField]
	private SkinnedMeshRenderer m_leftShoulderGeo;

	// Token: 0x0400177F RID: 6015
	[SerializeField]
	private SkinnedMeshRenderer m_rightShoulderGeo;

	// Token: 0x04001780 RID: 6016
	[SerializeField]
	private SkinnedMeshRenderer m_capeGeo;

	// Token: 0x04001781 RID: 6017
	[SerializeField]
	private SkinnedMeshRenderer m_scarfGeo;

	// Token: 0x04001782 RID: 6018
	[SerializeField]
	private SkinnedMeshRenderer m_chestGeo;

	// Token: 0x04001783 RID: 6019
	[SerializeField]
	private SkinnedMeshRenderer m_leftHandClosedGeo;

	// Token: 0x04001784 RID: 6020
	[SerializeField]
	private SkinnedMeshRenderer m_rightHandClosedGeo;

	// Token: 0x04001785 RID: 6021
	[SerializeField]
	private SkinnedMeshRenderer m_leftHandOpenGeo;

	// Token: 0x04001786 RID: 6022
	[SerializeField]
	private SkinnedMeshRenderer m_rightHandOpenGeo;

	// Token: 0x04001787 RID: 6023
	[Header("Player Class Geo", order = 10)]
	[SerializeField]
	private GameObject m_knightGeo;

	// Token: 0x04001788 RID: 6024
	[SerializeField]
	private GameObject m_barbarianGeo;

	// Token: 0x04001789 RID: 6025
	[SerializeField]
	private GameObject m_mageGeo;

	// Token: 0x0400178A RID: 6026
	[SerializeField]
	private GameObject m_archerGeo;

	// Token: 0x0400178B RID: 6027
	[SerializeField]
	private GameObject m_duelistGeo;

	// Token: 0x0400178C RID: 6028
	[SerializeField]
	private GameObject m_spearmanGeo;

	// Token: 0x0400178D RID: 6029
	[SerializeField]
	private GameObject m_chefGeo;

	// Token: 0x0400178E RID: 6030
	[SerializeField]
	private GameObject m_assassinGeo;

	// Token: 0x0400178F RID: 6031
	[SerializeField]
	private GameObject m_gunslingerGeo;

	// Token: 0x04001790 RID: 6032
	[SerializeField]
	private GameObject m_boxerGeo;

	// Token: 0x04001791 RID: 6033
	[SerializeField]
	private GameObject m_lancerGeo;

	// Token: 0x04001792 RID: 6034
	[SerializeField]
	private GameObject m_roninGeo;

	// Token: 0x04001793 RID: 6035
	[SerializeField]
	private GameObject m_pirateGeo;

	// Token: 0x04001794 RID: 6036
	[SerializeField]
	private GameObject m_bardGeo;

	// Token: 0x04001795 RID: 6037
	[SerializeField]
	private GameObject m_astroGeo;

	// Token: 0x04001796 RID: 6038
	[Header("Player Weapon Geo")]
	[SerializeField]
	private SkinnedMeshRenderer m_swordGeo;

	// Token: 0x04001797 RID: 6039
	[SerializeField]
	private SkinnedMeshRenderer m_spearGeo;

	// Token: 0x04001798 RID: 6040
	[SerializeField]
	private SkinnedMeshRenderer m_lanceGeo;

	// Token: 0x04001799 RID: 6041
	[SerializeField]
	private SkinnedMeshRenderer m_staffGeo;

	// Token: 0x0400179A RID: 6042
	[SerializeField]
	private SkinnedMeshRenderer m_bowGeo;

	// Token: 0x0400179B RID: 6043
	[SerializeField]
	private SkinnedMeshRenderer m_arrowGeo;

	// Token: 0x0400179C RID: 6044
	[SerializeField]
	private SkinnedMeshRenderer m_saberGeo;

	// Token: 0x0400179D RID: 6045
	[SerializeField]
	private SkinnedMeshRenderer m_ladleGeo;

	// Token: 0x0400179E RID: 6046
	[SerializeField]
	private SkinnedMeshRenderer m_axeGeo;

	// Token: 0x0400179F RID: 6047
	[SerializeField]
	private SkinnedMeshRenderer m_kunaiGeo;

	// Token: 0x040017A0 RID: 6048
	[SerializeField]
	private SkinnedMeshRenderer m_tonfaGeoL;

	// Token: 0x040017A1 RID: 6049
	[SerializeField]
	private SkinnedMeshRenderer m_tonfaGeoR;

	// Token: 0x040017A2 RID: 6050
	[SerializeField]
	private SkinnedMeshRenderer m_chakramGeo;

	// Token: 0x040017A3 RID: 6051
	[SerializeField]
	private SkinnedMeshRenderer m_dualBladeL;

	// Token: 0x040017A4 RID: 6052
	[SerializeField]
	private SkinnedMeshRenderer m_dualBladeR;

	// Token: 0x040017A5 RID: 6053
	[SerializeField]
	private SkinnedMeshRenderer m_fryingPanGeo;

	// Token: 0x040017A6 RID: 6054
	[SerializeField]
	private SkinnedMeshRenderer m_pistolGeo;

	// Token: 0x040017A7 RID: 6055
	[SerializeField]
	private SkinnedMeshRenderer m_boxingGlovesGeo;

	// Token: 0x040017A8 RID: 6056
	[SerializeField]
	private SkinnedMeshRenderer m_katanaGeo;

	// Token: 0x040017A9 RID: 6057
	[SerializeField]
	private SkinnedMeshRenderer m_cannonGeo;

	// Token: 0x040017AA RID: 6058
	[SerializeField]
	private SkinnedMeshRenderer m_luteGeo;

	// Token: 0x040017AB RID: 6059
	[SerializeField]
	private SkinnedMeshRenderer m_astroWandGeo;

	// Token: 0x040017AC RID: 6060
	[Header("Misc Geo")]
	[SerializeField]
	private GameObject m_visualsGO;

	// Token: 0x040017AD RID: 6061
	private Animator m_animator;

	// Token: 0x040017AE RID: 6062
	private MaterialPropertyBlock m_matPropBlock;

	// Token: 0x040017AF RID: 6063
	private SkinnedMeshRenderer[] m_bodyGeoArray;

	// Token: 0x040017B0 RID: 6064
	private SkinnedMeshRenderer[] m_outfitGeoArray;

	// Token: 0x040017B1 RID: 6065
	private SkinnedMeshRenderer[] m_customGeoArray;

	// Token: 0x040017B2 RID: 6066
	private GameObject m_currentOutfit;

	// Token: 0x040017B3 RID: 6067
	private GameObject m_customArmorMesh;

	// Token: 0x040017B4 RID: 6068
	private GameObject m_customHelmetMesh;

	// Token: 0x040017B5 RID: 6069
	private bool m_customGeoMeshIsDirty;
}
