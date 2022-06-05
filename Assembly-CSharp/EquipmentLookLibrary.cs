using System;
using System.Linq;
using UnityEngine;

// Token: 0x020003E3 RID: 995
[CreateAssetMenu(menuName = "Custom/Libraries/Equipment Look Library")]
public class EquipmentLookLibrary : ScriptableObject
{
	// Token: 0x17000E57 RID: 3671
	// (get) Token: 0x06002047 RID: 8263 RVA: 0x0001121A File Offset: 0x0000F41A
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

	// Token: 0x06002048 RID: 8264 RVA: 0x000A4A84 File Offset: 0x000A2C84
	public static Material GetFabledMaterial(AbilityType abilityType)
	{
		Material result;
		if (EquipmentLookLibrary.Instance.m_fabledWeaponDict.TryGetValue(abilityType, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x17000E58 RID: 3672
	// (get) Token: 0x06002049 RID: 8265 RVA: 0x00011243 File Offset: 0x0000F443
	public static Material CantAttackMaterial
	{
		get
		{
			return EquipmentLookLibrary.Instance.m_cantAttackMaterial;
		}
	}

	// Token: 0x0600204A RID: 8266 RVA: 0x0001124F File Offset: 0x0000F44F
	public static MaterialGeoObject[] GetHelmetEquipmentLookData()
	{
		return EquipmentLookLibrary.Instance.m_helmetEquipmentLookLibrary.ObjectList;
	}

	// Token: 0x0600204B RID: 8267 RVA: 0x000A4AA8 File Offset: 0x000A2CA8
	public static MaterialGeoObject GetHelmetEquipmentLookData(EquipmentType equipType)
	{
		return (from obj in EquipmentLookLibrary.Instance.m_helmetEquipmentLookLibrary.ObjectList
		where obj.EquipmentType == equipType
		select obj).SingleOrDefault<MaterialGeoObject>();
	}

	// Token: 0x0600204C RID: 8268 RVA: 0x00011260 File Offset: 0x0000F460
	public static MaterialGeoObject[] GetArmorEquipmentLookData()
	{
		return EquipmentLookLibrary.Instance.m_armorEquipmentLookLibrary.ObjectList;
	}

	// Token: 0x0600204D RID: 8269 RVA: 0x000A4AE8 File Offset: 0x000A2CE8
	public static MaterialGeoObject GetArmorEquipmentLookData(EquipmentType equipType)
	{
		return (from obj in EquipmentLookLibrary.Instance.m_armorEquipmentLookLibrary.ObjectList
		where obj.EquipmentType == equipType
		select obj).SingleOrDefault<MaterialGeoObject>();
	}

	// Token: 0x0600204E RID: 8270 RVA: 0x00011271 File Offset: 0x0000F471
	public static MaterialBlendWeightObject[] GetCapeEquipmentLookData()
	{
		return EquipmentLookLibrary.Instance.m_capeEquipmentLookLibrary.ObjectList;
	}

	// Token: 0x0600204F RID: 8271 RVA: 0x000A4B28 File Offset: 0x000A2D28
	public static MaterialBlendWeightObject GetCapeEquipmentLookData(EquipmentType equipType)
	{
		return (from obj in EquipmentLookLibrary.Instance.m_capeEquipmentLookLibrary.ObjectList
		where obj.EquipmentType == equipType
		select obj).SingleOrDefault<MaterialBlendWeightObject>();
	}

	// Token: 0x06002050 RID: 8272 RVA: 0x000A4B68 File Offset: 0x000A2D68
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

	// Token: 0x06002051 RID: 8273 RVA: 0x000A4D24 File Offset: 0x000A2F24
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

	// Token: 0x04001CE9 RID: 7401
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/EquipmentLookLibrary";

	// Token: 0x04001CEA RID: 7402
	[Space(10f)]
	[Header("Body Parts")]
	[Space(10f)]
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_helmetEquipmentLookLibrary;

	// Token: 0x04001CEB RID: 7403
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_armorEquipmentLookLibrary;

	// Token: 0x04001CEC RID: 7404
	[SerializeField]
	private MaterialBlendWeight_EquipmentLookData m_capeEquipmentLookLibrary;

	// Token: 0x04001CED RID: 7405
	[Space(10f)]
	[Header("Weapons")]
	[Space(10f)]
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_swordEquipmentLookLibrary;

	// Token: 0x04001CEE RID: 7406
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_spearEquipmentLookLibrary;

	// Token: 0x04001CEF RID: 7407
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_staffEquipmentLookLibrary;

	// Token: 0x04001CF0 RID: 7408
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_lanceEquipmentLookLibrary;

	// Token: 0x04001CF1 RID: 7409
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_bowEquipmentLookLibrary;

	// Token: 0x04001CF2 RID: 7410
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_saberEquipmentLookLibrary;

	// Token: 0x04001CF3 RID: 7411
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_ladleEquipmentLookLibrary;

	// Token: 0x04001CF4 RID: 7412
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_axeEquipmentLookLibrary;

	// Token: 0x04001CF5 RID: 7413
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_kunaiEquipmentLookLibrary;

	// Token: 0x04001CF6 RID: 7414
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_chakramEquipmentLookLibrary;

	// Token: 0x04001CF7 RID: 7415
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_tonfaEquipmentLookLibrary;

	// Token: 0x04001CF8 RID: 7416
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_dualBladesEquipmentLookLibrary;

	// Token: 0x04001CF9 RID: 7417
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_fryingPanEquipmentLookLibrary;

	// Token: 0x04001CFA RID: 7418
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_pistolEquipmentLookLibrary;

	// Token: 0x04001CFB RID: 7419
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_boxingGloveEquipmentLookLibrary;

	// Token: 0x04001CFC RID: 7420
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_katanaEquipmentLookLibrary;

	// Token: 0x04001CFD RID: 7421
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_cannonEquipmentLookLibrary;

	// Token: 0x04001CFE RID: 7422
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_luteEquipmentLookLibrary;

	// Token: 0x04001CFF RID: 7423
	[SerializeField]
	private MaterialGeo_EquipmentLookData m_astroWandEquipmentLookLibrary;

	// Token: 0x04001D00 RID: 7424
	[Space(10f)]
	[SerializeField]
	private Material m_cantAttackMaterial;

	// Token: 0x04001D01 RID: 7425
	[Space(10f)]
	[Header("Fabled Weapon Materials")]
	[Space(10f)]
	[SerializeField]
	private AbilityTypeMaterialDictionary m_fabledWeaponDict;

	// Token: 0x04001D02 RID: 7426
	private static EquipmentLookLibrary m_instance;
}
