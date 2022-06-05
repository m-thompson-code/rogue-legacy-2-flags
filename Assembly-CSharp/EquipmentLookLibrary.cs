using System;
using System.Linq;
using UnityEngine;

// Token: 0x0200022C RID: 556
[CreateAssetMenu(menuName = "Custom/Libraries/Equipment Look Library")]
public class EquipmentLookLibrary : ScriptableObject
{
	// Token: 0x17000B2E RID: 2862
	// (get) Token: 0x060016A2 RID: 5794 RVA: 0x000468CB File Offset: 0x00044ACB
	private static EquipmentLookLibrary Instance
	{
		get
		{
			if (EquipmentLookLibrary.m_instance == null)
			{
				EquipmentLookLibrary.m_instance = CDGResources.Load<EquipmentLookLibrary>("Scriptable Objects/Libraries/EquipmentLookLibrary", "", true);
			}
			return EquipmentLookLibrary.m_instance;
		}
	}

	// Token: 0x060016A3 RID: 5795 RVA: 0x000468F4 File Offset: 0x00044AF4
	public static Material GetFabledMaterial(AbilityType abilityType)
	{
		Material result;
		if (EquipmentLookLibrary.Instance.m_fabledWeaponDict.TryGetValue(abilityType, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x17000B2F RID: 2863
	// (get) Token: 0x060016A4 RID: 5796 RVA: 0x00046918 File Offset: 0x00044B18
	public static Material CantAttackMaterial
	{
		get
		{
			return EquipmentLookLibrary.Instance.m_cantAttackMaterial;
		}
	}

	// Token: 0x060016A5 RID: 5797 RVA: 0x00046924 File Offset: 0x00044B24
	public static MaterialGeoObject[] GetHelmetEquipmentLookData()
	{
		return EquipmentLookLibrary.Instance.m_helmetEquipmentLookLibrary.ObjectList;
	}

	// Token: 0x060016A6 RID: 5798 RVA: 0x00046938 File Offset: 0x00044B38
	public static MaterialGeoObject GetHelmetEquipmentLookData(EquipmentType equipType)
	{
		return (from obj in EquipmentLookLibrary.Instance.m_helmetEquipmentLookLibrary.ObjectList
		where obj.EquipmentType == equipType
		select obj).SingleOrDefault<MaterialGeoObject>();
	}

	// Token: 0x060016A7 RID: 5799 RVA: 0x00046977 File Offset: 0x00044B77
	public static MaterialGeoObject[] GetArmorEquipmentLookData()
	{
		return EquipmentLookLibrary.Instance.m_armorEquipmentLookLibrary.ObjectList;
	}

	// Token: 0x060016A8 RID: 5800 RVA: 0x00046988 File Offset: 0x00044B88
	public static MaterialGeoObject GetArmorEquipmentLookData(EquipmentType equipType)
	{
		return (from obj in EquipmentLookLibrary.Instance.m_armorEquipmentLookLibrary.ObjectList
		where obj.EquipmentType == equipType
		select obj).SingleOrDefault<MaterialGeoObject>();
	}

	// Token: 0x060016A9 RID: 5801 RVA: 0x000469C7 File Offset: 0x00044BC7
	public static MaterialBlendWeightObject[] GetCapeEquipmentLookData()
	{
		return EquipmentLookLibrary.Instance.m_capeEquipmentLookLibrary.ObjectList;
	}

	// Token: 0x060016AA RID: 5802 RVA: 0x000469D8 File Offset: 0x00044BD8
	public static MaterialBlendWeightObject GetCapeEquipmentLookData(EquipmentType equipType)
	{
		return (from obj in EquipmentLookLibrary.Instance.m_capeEquipmentLookLibrary.ObjectList
		where obj.EquipmentType == equipType
		select obj).SingleOrDefault<MaterialBlendWeightObject>();
	}

	// Token: 0x060016AB RID: 5803 RVA: 0x00046A18 File Offset: 0x00044C18
	public static MaterialGeo_EquipmentLookData GetWeaponEquipmentLookData(AbilityType weaponAbilityType)
	{
		if (weaponAbilityType > AbilityType.AxeSpinnerWeapon)
		{
			if (weaponAbilityType <= AbilityType.CannonWeapon)
			{
				switch (weaponAbilityType)
				{
				case AbilityType.BowWeapon:
				case AbilityType.GroundBowWeapon:
					return EquipmentLookLibrary.Instance.m_bowEquipmentLookLibrary;
				case AbilityType.KunaiWeapon:
					return EquipmentLookLibrary.Instance.m_kunaiEquipmentLookLibrary;
				case AbilityType.BolaWeapon:
				case (AbilityType)57:
				case (AbilityType)58:
				case (AbilityType)59:
				case (AbilityType)63:
				case (AbilityType)65:
				case (AbilityType)67:
				case (AbilityType)69:
					goto IL_1AD;
				case AbilityType.KineticBowWeapon:
					goto IL_197;
				case AbilityType.PistolWeapon:
				case AbilityType.DragonPistolWeapon:
					return EquipmentLookLibrary.Instance.m_pistolEquipmentLookLibrary;
				case AbilityType.FryingPanWeapon:
					return EquipmentLookLibrary.Instance.m_fryingPanEquipmentLookLibrary;
				case AbilityType.SpoonsWeapon:
					return EquipmentLookLibrary.Instance.m_ladleEquipmentLookLibrary;
				case AbilityType.ChakramWeapon:
					return EquipmentLookLibrary.Instance.m_chakramEquipmentLookLibrary;
				case AbilityType.TonfaWeapon:
					return EquipmentLookLibrary.Instance.m_tonfaEquipmentLookLibrary;
				case AbilityType.DualBladesWeapon:
					return EquipmentLookLibrary.Instance.m_dualBladesEquipmentLookLibrary;
				case AbilityType.BoxingGloveWeapon:
					break;
				case AbilityType.MagicWandWeapon:
					return EquipmentLookLibrary.Instance.m_staffEquipmentLookLibrary;
				default:
					if (weaponAbilityType != AbilityType.CannonWeapon)
					{
						goto IL_1AD;
					}
					return EquipmentLookLibrary.Instance.m_cannonEquipmentLookLibrary;
				}
			}
			else
			{
				if (weaponAbilityType == AbilityType.SaberWeapon)
				{
					return EquipmentLookLibrary.Instance.m_saberEquipmentLookLibrary;
				}
				switch (weaponAbilityType)
				{
				case AbilityType.LanceWeapon:
				case AbilityType.ScytheWeapon:
					return EquipmentLookLibrary.Instance.m_lanceEquipmentLookLibrary;
				case (AbilityType)121:
				case (AbilityType)123:
				case (AbilityType)125:
					goto IL_1AD;
				case AbilityType.KatanaWeapon:
					return EquipmentLookLibrary.Instance.m_katanaEquipmentLookLibrary;
				case AbilityType.ExplosiveHandsWeapon:
					break;
				case AbilityType.LuteWeapon:
					goto IL_197;
				case AbilityType.AstroWandWeapon:
					return EquipmentLookLibrary.Instance.m_astroWandEquipmentLookLibrary;
				default:
					goto IL_1AD;
				}
			}
			return EquipmentLookLibrary.Instance.m_boxingGloveEquipmentLookLibrary;
			IL_197:
			return EquipmentLookLibrary.Instance.m_luteEquipmentLookLibrary;
		}
		if (weaponAbilityType <= AbilityType.SwordBeamWeapon)
		{
			if (weaponAbilityType == AbilityType.SwordWeapon || weaponAbilityType == AbilityType.SwordBeamWeapon)
			{
				return EquipmentLookLibrary.Instance.m_swordEquipmentLookLibrary;
			}
		}
		else
		{
			if (weaponAbilityType == AbilityType.SpearWeapon)
			{
				return EquipmentLookLibrary.Instance.m_spearEquipmentLookLibrary;
			}
			if (weaponAbilityType - AbilityType.AxeWeapon <= 1)
			{
				return EquipmentLookLibrary.Instance.m_axeEquipmentLookLibrary;
			}
		}
		IL_1AD:
		return null;
	}

	// Token: 0x060016AC RID: 5804 RVA: 0x00046BD4 File Offset: 0x00044DD4
	public static MaterialGeoObject GetWeaponEquipmentLookData(AbilityType weaponAbilityType, EquipmentType equipType)
	{
		MaterialGeo_EquipmentLookData weaponEquipmentLookData = EquipmentLookLibrary.GetWeaponEquipmentLookData(weaponAbilityType);
		if (weaponEquipmentLookData)
		{
			return (from obj in weaponEquipmentLookData.ObjectList
			where obj.EquipmentType == equipType
			select obj).SingleOrDefault<MaterialGeoObject>();
		}
		return null;
	}

	// Token: 0x040015D9 RID: 5593
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/EquipmentLookLibrary";

	// Token: 0x040015DA RID: 5594
	[Space(10f)]
	[Header("Body Parts")]
	[Space(10f)]
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_helmetEquipmentLookLibrary;

	// Token: 0x040015DB RID: 5595
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_armorEquipmentLookLibrary;

	// Token: 0x040015DC RID: 5596
	[SerializeField]
	private MaterialBlendWeight_EquipmentLookData m_capeEquipmentLookLibrary;

	// Token: 0x040015DD RID: 5597
	[Space(10f)]
	[Header("Weapons")]
	[Space(10f)]
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_swordEquipmentLookLibrary;

	// Token: 0x040015DE RID: 5598
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_spearEquipmentLookLibrary;

	// Token: 0x040015DF RID: 5599
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_staffEquipmentLookLibrary;

	// Token: 0x040015E0 RID: 5600
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_lanceEquipmentLookLibrary;

	// Token: 0x040015E1 RID: 5601
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_bowEquipmentLookLibrary;

	// Token: 0x040015E2 RID: 5602
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_saberEquipmentLookLibrary;

	// Token: 0x040015E3 RID: 5603
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_ladleEquipmentLookLibrary;

	// Token: 0x040015E4 RID: 5604
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_axeEquipmentLookLibrary;

	// Token: 0x040015E5 RID: 5605
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_kunaiEquipmentLookLibrary;

	// Token: 0x040015E6 RID: 5606
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_chakramEquipmentLookLibrary;

	// Token: 0x040015E7 RID: 5607
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_tonfaEquipmentLookLibrary;

	// Token: 0x040015E8 RID: 5608
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_dualBladesEquipmentLookLibrary;

	// Token: 0x040015E9 RID: 5609
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_fryingPanEquipmentLookLibrary;

	// Token: 0x040015EA RID: 5610
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_pistolEquipmentLookLibrary;

	// Token: 0x040015EB RID: 5611
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_boxingGloveEquipmentLookLibrary;

	// Token: 0x040015EC RID: 5612
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_katanaEquipmentLookLibrary;

	// Token: 0x040015ED RID: 5613
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_cannonEquipmentLookLibrary;

	// Token: 0x040015EE RID: 5614
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_luteEquipmentLookLibrary;

	// Token: 0x040015EF RID: 5615
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_astroWandEquipmentLookLibrary;

	// Token: 0x040015F0 RID: 5616
	[Space(10f)]
	[SerializeField]
	private Material m_cantAttackMaterial;

	// Token: 0x040015F1 RID: 5617
	[Space(10f)]
	[Header("Fabled Weapon Materials")]
	[Space(10f)]
	[SerializeField]
	private AbilityTypeMaterialDictionary m_fabledWeaponDict;

	// Token: 0x040015F2 RID: 5618
	private static EquipmentLookLibrary m_instance;
}
