using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000426 RID: 1062
public static class LookCreator
{
	// Token: 0x06002239 RID: 8761 RVA: 0x000A9258 File Offset: 0x000A7458
	public static void InitializeClassLook(ClassType classType, LookController lookObj)
	{
		LookCreator.DisableAllClassOutfitGeo(lookObj);
		lookObj.Animator.SetBool("HideForeheadHair", false);
		GameObject gameObject;
		if (classType <= ClassType.SaberClass)
		{
			if (classType <= ClassType.MagicWandClass)
			{
				if (classType != ClassType.SwordClass)
				{
					if (classType == ClassType.AxeClass)
					{
						gameObject = lookObj.BarbarianGeo;
						goto IL_184;
					}
					if (classType == ClassType.MagicWandClass)
					{
						gameObject = lookObj.MageGeo;
						goto IL_184;
					}
				}
			}
			else if (classType <= ClassType.SpearClass)
			{
				if (classType == ClassType.DualBladesClass)
				{
					gameObject = lookObj.AssassinGeo;
					goto IL_184;
				}
				if (classType == ClassType.SpearClass)
				{
					gameObject = lookObj.SpearmanGeo;
					goto IL_184;
				}
			}
			else
			{
				if (classType == ClassType.BowClass)
				{
					gameObject = lookObj.ArcherGeo;
					goto IL_184;
				}
				if (classType == ClassType.SaberClass)
				{
					gameObject = lookObj.DuelistGeo;
					goto IL_184;
				}
			}
		}
		else if (classType <= ClassType.GunClass)
		{
			if (classType <= ClassType.BoxingGloveClass)
			{
				if (classType == ClassType.LadleClass)
				{
					gameObject = lookObj.ChefGeo;
					goto IL_184;
				}
				if (classType == ClassType.BoxingGloveClass)
				{
					gameObject = lookObj.BoxerGeo;
					goto IL_184;
				}
			}
			else
			{
				if (classType == ClassType.LanceClass)
				{
					gameObject = lookObj.LancerGeo;
					goto IL_184;
				}
				if (classType == ClassType.GunClass)
				{
					gameObject = lookObj.GunslingerGeo;
					goto IL_184;
				}
			}
		}
		else if (classType <= ClassType.CannonClass)
		{
			if (classType == ClassType.KatanaClass)
			{
				gameObject = lookObj.RoninGeo;
				lookObj.Animator.SetBool("HideForeheadHair", true);
				goto IL_184;
			}
			if (classType == ClassType.CannonClass)
			{
				gameObject = lookObj.PirateGeo;
				goto IL_184;
			}
		}
		else
		{
			if (classType == ClassType.LuteClass)
			{
				gameObject = lookObj.BardGeo;
				goto IL_184;
			}
			if (classType == ClassType.AstroClass)
			{
				gameObject = lookObj.AstroGeo;
				goto IL_184;
			}
		}
		gameObject = lookObj.KnightGeo;
		IL_184:
		gameObject.SetActive(true);
		lookObj.CurrentClassOutfit = gameObject;
	}

	// Token: 0x0600223A RID: 8762 RVA: 0x000124B1 File Offset: 0x000106B1
	private static void SetGeoColor(SkinnedMeshRenderer renderer, int shaderID, MaterialPropertyBlock matPropBlock, Color color)
	{
		renderer.GetPropertyBlock(matPropBlock);
		matPropBlock.SetColor(shaderID, color);
		renderer.SetPropertyBlock(matPropBlock);
	}

	// Token: 0x0600223B RID: 8763 RVA: 0x000A93F8 File Offset: 0x000A75F8
	public static void InitializeCharacterLook(CharacterData charData, LookController lookObj, bool generateRandomLook)
	{
		if (generateRandomLook)
		{
			CharacterCreator.GenerateRandomLook(charData);
		}
		MaterialPropertyBlock propertyBlock = lookObj.PropertyBlock;
		MaterialWeightObject materialWeightObject = LookLibrary.GetEyeLookData()[charData.EyeType];
		lookObj.LeftEyeGeo.sharedMaterial = materialWeightObject.Material;
		lookObj.RightEyeGeo.sharedMaterial = materialWeightObject.Material;
		lookObj.LeftEyeGeo.GetPropertyBlock(lookObj.PropertyBlock);
		lookObj.PropertyBlock.SetColor(ShaderID_RL._MainColor, materialWeightObject.Material.GetColor(ShaderID_RL._MainColor));
		lookObj.PropertyBlock.SetColor(ShaderID_RL._AlphaBlendColor, materialWeightObject.Material.GetColor(ShaderID_RL._AlphaBlendColor));
		lookObj.PropertyBlock.SetColor(ShaderID_RL._RimLightColor, materialWeightObject.Material.GetColor(ShaderID_RL._RimLightColor));
		lookObj.LeftEyeGeo.SetPropertyBlock(lookObj.PropertyBlock);
		lookObj.RightEyeGeo.SetPropertyBlock(lookObj.PropertyBlock);
		MaterialWeightObject materialWeightObject2 = LookLibrary.GetMouthLookData()[charData.MouthType];
		lookObj.MouthGeo.sharedMaterial = materialWeightObject2.Material;
		if (charData.ClassType == ClassType.DualBladesClass)
		{
			lookObj.MouthGeo.gameObject.SetActive(false);
		}
		else
		{
			lookObj.MouthGeo.gameObject.SetActive(true);
		}
		MaterialWeightObject materialWeightObject3 = LookLibrary.GetFacialHairLookData()[charData.FacialHairType];
		lookObj.HeadGeo.sharedMaterial = materialWeightObject3.Material;
		ColorWeightObject colorWeightObject = LookLibrary.GetSkinColorLookData()[charData.SkinColorType];
		LookCreator.SetGeoColor(lookObj.HeadGeo, ShaderID_RL._MainColor, lookObj.PropertyBlock, colorWeightObject.Color);
		MaterialWeightObject materialWeightObject4 = LookLibrary.GetHairLookData()[charData.HairType];
		lookObj.ChestHairGeo.sharedMaterial = materialWeightObject4.Material;
		lookObj.HelmetHairGeo.sharedMaterial = materialWeightObject4.Material;
		ColorWeightObject colorWeightObject2 = LookLibrary.GetHairColorLookData()[charData.HairColorType];
		LookCreator.SetGeoColor(lookObj.ChestHairGeo, ShaderID_RL._MainColor, lookObj.PropertyBlock, colorWeightObject2.Color);
		LookCreator.SetGeoColor(lookObj.HelmetHairGeo, ShaderID_RL._MainColor, lookObj.PropertyBlock, colorWeightObject2.Color);
		BodyWeightObject bodyWeightObject = LookLibrary.GetBodyLookData()[charData.BodyType];
		lookObj.Animator.SetFloat("BodyType", (float)bodyWeightObject.BodyTypeWeightParam);
		lookObj.HeadGeo.SetBlendShapeWeight(0, (float)bodyWeightObject.BlendShapeWeight);
	}

	// Token: 0x0600223C RID: 8764 RVA: 0x000A963C File Offset: 0x000A783C
	public static void InitializeHelmetLook(EquipmentType equipType, LookController lookObj)
	{
		MaterialGeoObject materialGeoObject = EquipmentLookLibrary.GetHelmetEquipmentLookData(equipType) ?? EquipmentLookLibrary.GetHelmetEquipmentLookData(EquipmentType.None);
		if (materialGeoObject != null)
		{
			if (lookObj.CustomHelmetMesh)
			{
				if (lookObj.IsPortraitModel)
				{
					UnityEngine.Object.Destroy(lookObj.CustomHelmetMesh);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(lookObj.CustomHelmetMesh);
				}
				lookObj.CustomHelmetMesh = null;
				lookObj.SetCustomMeshDirty();
			}
			lookObj.HelmetGeo.gameObject.SetActive(true);
			lookObj.HelmetHairGeo.gameObject.SetActive(true);
			if (materialGeoObject.CustomGeo)
			{
				lookObj.CustomHelmetMesh = UnityEngine.Object.Instantiate<GameObject>(materialGeoObject.CustomGeo, lookObj.VisualsGameObject.transform, false);
				lookObj.CustomHelmetMesh.name = materialGeoObject.CustomGeo.name;
				lookObj.CustomHelmetMesh.SetLayerRecursively(lookObj.HelmetGeo.gameObject.layer, false);
				SkinnedMeshRenderer component = lookObj.CustomHelmetMesh.transform.FindDeep("geoHelmet").gameObject.GetComponent<SkinnedMeshRenderer>();
				LookCreator.RebindSkinnedMeshRenderers(lookObj.HelmetGeo, component);
				lookObj.HelmetGeo.gameObject.SetActive(false);
				SkinnedMeshRenderer component2 = lookObj.CustomHelmetMesh.transform.FindDeep("geoHelmetHair").gameObject.GetComponent<SkinnedMeshRenderer>();
				LookCreator.RebindSkinnedMeshRenderers(lookObj.HelmetHairGeo, component2);
				lookObj.HelmetHairGeo.gameObject.SetActive(false);
				GameObject gameObject = lookObj.CustomHelmetMesh.transform.FindDeep("root").gameObject;
				if (lookObj.IsPortraitModel)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(gameObject);
				}
				component.sharedMaterial = materialGeoObject.Material;
				component2.sharedMaterial = lookObj.HelmetHairGeo.sharedMaterial;
				lookObj.HelmetHairGeo.GetPropertyBlock(lookObj.PropertyBlock);
				LookCreator.SetGeoColor(component2, ShaderID_RL._MainColor, lookObj.PropertyBlock, lookObj.PropertyBlock.GetColor(ShaderID_RL._MainColor));
			}
			else
			{
				lookObj.HelmetGeo.sharedMaterial = materialGeoObject.Material;
			}
			SkinnedMeshRenderer[] currentOutfitGeoArray = lookObj.CurrentOutfitGeoArray;
			for (int i = 0; i < currentOutfitGeoArray.Length; i++)
			{
				LookCreator.SetGeoColor(currentOutfitGeoArray[i], ShaderID_RL._HelmetColor, lookObj.PropertyBlock, materialGeoObject.Material.GetColor(ShaderID_RL._MainColor));
			}
			return;
		}
		Debug.Log("Could not find Helmet Equipment Look Library entry for " + equipType.ToString());
	}

	// Token: 0x0600223D RID: 8765 RVA: 0x000A988C File Offset: 0x000A7A8C
	public static void InitializeArmorLook(EquipmentType equipType, LookController lookObj)
	{
		MaterialGeoObject materialGeoObject = EquipmentLookLibrary.GetArmorEquipmentLookData(equipType) ?? EquipmentLookLibrary.GetArmorEquipmentLookData(EquipmentType.None);
		if (materialGeoObject != null)
		{
			if (lookObj.CustomArmorMesh)
			{
				if (lookObj.IsPortraitModel)
				{
					UnityEngine.Object.Destroy(lookObj.CustomArmorMesh);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(lookObj.CustomArmorMesh);
				}
				lookObj.CustomArmorMesh = null;
				lookObj.SetCustomMeshDirty();
			}
			lookObj.ChestGeo.gameObject.SetActive(true);
			lookObj.LeftShoulderGeo.gameObject.SetActive(true);
			lookObj.RightShoulderGeo.gameObject.SetActive(true);
			if (materialGeoObject.CustomGeo)
			{
				lookObj.CustomArmorMesh = UnityEngine.Object.Instantiate<GameObject>(materialGeoObject.CustomGeo, lookObj.VisualsGameObject.transform, false);
				lookObj.CustomArmorMesh.name = materialGeoObject.CustomGeo.name;
				lookObj.CustomArmorMesh.SetLayerRecursively(lookObj.ChestGeo.gameObject.layer, false);
				SkinnedMeshRenderer component = lookObj.CustomArmorMesh.transform.FindDeep("geoChest").gameObject.GetComponent<SkinnedMeshRenderer>();
				LookCreator.RebindSkinnedMeshRenderers(lookObj.ChestGeo, component);
				lookObj.ChestGeo.gameObject.SetActive(false);
				SkinnedMeshRenderer component2 = lookObj.CustomArmorMesh.transform.FindDeep("geoShoulder_l").gameObject.GetComponent<SkinnedMeshRenderer>();
				LookCreator.RebindSkinnedMeshRenderers(lookObj.LeftShoulderGeo, component2);
				lookObj.LeftShoulderGeo.gameObject.SetActive(false);
				SkinnedMeshRenderer component3 = lookObj.CustomArmorMesh.transform.FindDeep("geoShoulder_r").gameObject.GetComponent<SkinnedMeshRenderer>();
				LookCreator.RebindSkinnedMeshRenderers(lookObj.RightShoulderGeo, component3);
				lookObj.RightShoulderGeo.gameObject.SetActive(false);
				GameObject gameObject = lookObj.CustomArmorMesh.transform.FindDeep("root").gameObject;
				if (lookObj.IsPortraitModel)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(gameObject);
				}
				component.sharedMaterial = materialGeoObject.Material;
				component2.sharedMaterial = materialGeoObject.Material;
				component3.sharedMaterial = materialGeoObject.Material;
			}
			else
			{
				lookObj.ChestGeo.sharedMaterial = materialGeoObject.Material;
				lookObj.LeftShoulderGeo.sharedMaterial = materialGeoObject.Material;
				lookObj.RightShoulderGeo.sharedMaterial = materialGeoObject.Material;
			}
			lookObj.LeftHandClosedGeo.sharedMaterial = materialGeoObject.Material;
			lookObj.RightHandClosedGeo.sharedMaterial = materialGeoObject.Material;
			lookObj.LeftHandOpenGeo.sharedMaterial = materialGeoObject.Material;
			lookObj.RightHandOpenGeo.sharedMaterial = materialGeoObject.Material;
			SkinnedMeshRenderer[] currentOutfitGeoArray = lookObj.CurrentOutfitGeoArray;
			for (int i = 0; i < currentOutfitGeoArray.Length; i++)
			{
				LookCreator.SetGeoColor(currentOutfitGeoArray[i], ShaderID_RL._ArmorColor, lookObj.PropertyBlock, materialGeoObject.Material.GetColor(ShaderID_RL._MainColor));
			}
			return;
		}
		Debug.Log("Could not find Armor Equipment Look Library entry for " + equipType.ToString());
	}

	// Token: 0x0600223E RID: 8766 RVA: 0x000A9B68 File Offset: 0x000A7D68
	public static void InitializeCapeLook(EquipmentType equipType, LookController lookObj)
	{
		MaterialBlendWeightObject materialBlendWeightObject = EquipmentLookLibrary.GetCapeEquipmentLookData(equipType) ?? EquipmentLookLibrary.GetCapeEquipmentLookData(EquipmentType.None);
		if (materialBlendWeightObject != null)
		{
			lookObj.CapeGeo.sharedMaterial = materialBlendWeightObject.Material;
			lookObj.ScarfGeo.sharedMaterial = materialBlendWeightObject.Material;
			lookObj.CapeGeo.SetBlendShapeWeight(0, (float)materialBlendWeightObject.BlendWeight);
			SkinnedMeshRenderer[] currentOutfitGeoArray = lookObj.CurrentOutfitGeoArray;
			for (int i = 0; i < currentOutfitGeoArray.Length; i++)
			{
				LookCreator.SetGeoColor(currentOutfitGeoArray[i], ShaderID_RL._CapeColor, lookObj.PropertyBlock, materialBlendWeightObject.Material.GetColor(ShaderID_RL._MainColor));
			}
			return;
		}
		Debug.Log("Could not find Cape Equipment Look Library entry for " + equipType.ToString());
	}

	// Token: 0x0600223F RID: 8767 RVA: 0x000A9C14 File Offset: 0x000A7E14
	public static void InitializeWeaponLook(EquipmentType equipType, LookController lookObj, AbilityType weaponType, bool hasCantAttackTrait)
	{
		PlayerLookController playerLookController = lookObj as PlayerLookController;
		if (playerLookController)
		{
			playerLookController.ForceDisableCritBlinkEffect();
		}
		LookCreator.DisableAllWeaponGeo(lookObj);
		SkinnedMeshRenderer skinnedMeshRenderer = null;
		SkinnedMeshRenderer skinnedMeshRenderer2 = null;
		int num = 0;
		bool flag = false;
		MaterialGeoObject materialGeoObject;
		if (hasCantAttackTrait)
		{
			materialGeoObject = (EquipmentLookLibrary.GetWeaponEquipmentLookData(AbilityType.SwordWeapon, equipType) ?? EquipmentLookLibrary.GetWeaponEquipmentLookData(AbilityType.SwordWeapon, EquipmentType.None));
			skinnedMeshRenderer = lookObj.SwordGeo;
		}
		else
		{
			materialGeoObject = (EquipmentLookLibrary.GetWeaponEquipmentLookData(weaponType, equipType) ?? EquipmentLookLibrary.GetWeaponEquipmentLookData(weaponType, EquipmentType.None));
			if (weaponType <= AbilityType.AxeSpinnerWeapon)
			{
				if (weaponType <= AbilityType.SwordBeamWeapon)
				{
					if (weaponType == AbilityType.SwordWeapon || weaponType == AbilityType.SwordBeamWeapon)
					{
						skinnedMeshRenderer = lookObj.SwordGeo;
					}
				}
				else if (weaponType != AbilityType.SpearWeapon)
				{
					if (weaponType - AbilityType.AxeWeapon <= 1)
					{
						skinnedMeshRenderer = lookObj.AxeGeo;
					}
				}
				else
				{
					skinnedMeshRenderer = lookObj.SpearGeo;
				}
			}
			else
			{
				if (weaponType <= AbilityType.CannonWeapon)
				{
					switch (weaponType)
					{
					case AbilityType.BowWeapon:
					case AbilityType.GroundBowWeapon:
						skinnedMeshRenderer = lookObj.BowGeo;
						skinnedMeshRenderer2 = lookObj.ArrowGeo;
						flag = true;
						goto IL_27B;
					case AbilityType.KunaiWeapon:
						skinnedMeshRenderer = lookObj.KunaiGeo;
						goto IL_27B;
					case AbilityType.BolaWeapon:
					case (AbilityType)57:
					case (AbilityType)58:
					case (AbilityType)59:
					case (AbilityType)63:
					case (AbilityType)65:
					case (AbilityType)67:
					case (AbilityType)69:
						goto IL_27B;
					case AbilityType.KineticBowWeapon:
						goto IL_26B;
					case AbilityType.PistolWeapon:
					case AbilityType.DragonPistolWeapon:
						skinnedMeshRenderer = lookObj.PistolGeo;
						num = 2;
						goto IL_27B;
					case AbilityType.FryingPanWeapon:
						skinnedMeshRenderer = lookObj.FryingPanGeo;
						goto IL_27B;
					case AbilityType.SpoonsWeapon:
						skinnedMeshRenderer = lookObj.LadleGeo;
						goto IL_27B;
					case AbilityType.ChakramWeapon:
						skinnedMeshRenderer = lookObj.ChakramGeo;
						goto IL_27B;
					case AbilityType.TonfaWeapon:
						skinnedMeshRenderer = lookObj.TonfaLeftGeo;
						skinnedMeshRenderer2 = lookObj.TonfaRightGeo;
						num = 1;
						goto IL_27B;
					case AbilityType.DualBladesWeapon:
						skinnedMeshRenderer = lookObj.DualBladeLeftGeo;
						skinnedMeshRenderer2 = lookObj.DualBladeRightGeo;
						num = 1;
						goto IL_27B;
					case AbilityType.BoxingGloveWeapon:
						break;
					case AbilityType.MagicWandWeapon:
						skinnedMeshRenderer = lookObj.WandGeo;
						goto IL_27B;
					default:
						if (weaponType != AbilityType.CannonWeapon)
						{
							goto IL_27B;
						}
						skinnedMeshRenderer = lookObj.CannonGeo;
						goto IL_27B;
					}
				}
				else
				{
					if (weaponType == AbilityType.SaberWeapon)
					{
						skinnedMeshRenderer = lookObj.SaberGeo;
						goto IL_27B;
					}
					switch (weaponType)
					{
					case AbilityType.LanceWeapon:
						skinnedMeshRenderer = lookObj.LanceGeo;
						goto IL_27B;
					case (AbilityType)121:
					case (AbilityType)123:
					case (AbilityType)125:
						goto IL_27B;
					case AbilityType.ScytheWeapon:
						skinnedMeshRenderer = lookObj.LanceGeo;
						goto IL_27B;
					case AbilityType.KatanaWeapon:
						skinnedMeshRenderer = lookObj.KatanaGeo;
						goto IL_27B;
					case AbilityType.ExplosiveHandsWeapon:
						break;
					case AbilityType.LuteWeapon:
						goto IL_26B;
					case AbilityType.AstroWandWeapon:
						skinnedMeshRenderer = lookObj.AstroWandGeo;
						goto IL_27B;
					default:
						goto IL_27B;
					}
				}
				skinnedMeshRenderer = lookObj.BoxingGloveGeo;
				num = 1;
				lookObj.LeftHandClosedGeo.gameObject.SetActive(false);
				lookObj.LeftHandOpenGeo.gameObject.SetActive(false);
				lookObj.RightHandClosedGeo.gameObject.SetActive(false);
				lookObj.RightHandOpenGeo.gameObject.SetActive(false);
				goto IL_27B;
				IL_26B:
				skinnedMeshRenderer = lookObj.LuteGeo;
			}
		}
		IL_27B:
		if (materialGeoObject != null)
		{
			Material fabledMaterial = EquipmentLookLibrary.GetFabledMaterial(weaponType);
			if (fabledMaterial)
			{
				skinnedMeshRenderer.sharedMaterial = fabledMaterial;
			}
			else
			{
				skinnedMeshRenderer.sharedMaterial = materialGeoObject.Material;
			}
			skinnedMeshRenderer.gameObject.SetActive(true);
			if (skinnedMeshRenderer2)
			{
				skinnedMeshRenderer2.sharedMaterial = skinnedMeshRenderer.sharedMaterial;
				if (!flag)
				{
					skinnedMeshRenderer2.gameObject.SetActive(true);
				}
			}
			lookObj.CurrentWeaponGeo = skinnedMeshRenderer;
			lookObj.SecondaryWeaponGeo = skinnedMeshRenderer2;
		}
		else
		{
			num = 1;
			Debug.Log("Could not find Weapon Equipment Look Library entry for " + equipType.ToString() + " of Weapon Type: " + weaponType.ToString());
		}
		if (materialGeoObject == null)
		{
			if (lookObj.CurrentWeaponGeo)
			{
				lookObj.CurrentWeaponGeo.gameObject.SetActive(false);
			}
			if (lookObj.SecondaryWeaponGeo)
			{
				lookObj.SecondaryWeaponGeo.gameObject.SetActive(false);
			}
		}
		lookObj.Animator.SetFloat("Weaponless", (float)num);
		if (hasCantAttackTrait)
		{
			skinnedMeshRenderer.sharedMaterial = EquipmentLookLibrary.CantAttackMaterial;
		}
	}

	// Token: 0x06002240 RID: 8768 RVA: 0x000A9F98 File Offset: 0x000A8198
	public static void DisableAllWeaponGeo(LookController lookObj)
	{
		lookObj.SwordGeo.gameObject.SetActive(false);
		lookObj.SpearGeo.gameObject.SetActive(false);
		lookObj.LanceGeo.gameObject.SetActive(false);
		lookObj.WandGeo.gameObject.SetActive(false);
		lookObj.BowGeo.gameObject.SetActive(false);
		lookObj.ArrowGeo.gameObject.SetActive(false);
		lookObj.SaberGeo.gameObject.SetActive(false);
		lookObj.LadleGeo.gameObject.SetActive(false);
		lookObj.AxeGeo.gameObject.SetActive(false);
		lookObj.KunaiGeo.gameObject.SetActive(false);
		lookObj.ChakramGeo.gameObject.SetActive(false);
		lookObj.TonfaLeftGeo.gameObject.SetActive(false);
		lookObj.TonfaRightGeo.gameObject.SetActive(false);
		lookObj.DualBladeLeftGeo.gameObject.SetActive(false);
		lookObj.DualBladeRightGeo.gameObject.SetActive(false);
		lookObj.FryingPanGeo.gameObject.SetActive(false);
		lookObj.PistolGeo.gameObject.SetActive(false);
		lookObj.BoxingGloveGeo.gameObject.SetActive(false);
		lookObj.KatanaGeo.gameObject.SetActive(false);
		lookObj.CannonGeo.gameObject.SetActive(false);
		lookObj.LuteGeo.gameObject.SetActive(false);
		lookObj.AstroWandGeo.gameObject.SetActive(false);
		lookObj.LeftHandClosedGeo.gameObject.SetActive(true);
		lookObj.LeftHandOpenGeo.gameObject.SetActive(true);
		lookObj.RightHandClosedGeo.gameObject.SetActive(true);
		lookObj.RightHandOpenGeo.gameObject.SetActive(true);
		lookObj.CurrentWeaponGeo = null;
		lookObj.SecondaryWeaponGeo = null;
	}

	// Token: 0x06002241 RID: 8769 RVA: 0x000AA170 File Offset: 0x000A8370
	public static void DisableAllClassOutfitGeo(LookController lookObj)
	{
		lookObj.BarbarianGeo.SetActive(false);
		lookObj.KnightGeo.SetActive(false);
		lookObj.MageGeo.SetActive(false);
		lookObj.ArcherGeo.SetActive(false);
		lookObj.DuelistGeo.SetActive(false);
		lookObj.SpearmanGeo.SetActive(false);
		lookObj.ChefGeo.SetActive(false);
		lookObj.AssassinGeo.SetActive(false);
		lookObj.GunslingerGeo.SetActive(false);
		lookObj.LancerGeo.SetActive(false);
		lookObj.BoxerGeo.SetActive(false);
		lookObj.RoninGeo.SetActive(false);
		lookObj.PirateGeo.SetActive(false);
		lookObj.BardGeo.SetActive(false);
		lookObj.AstroGeo.SetActive(false);
	}

	// Token: 0x06002242 RID: 8770 RVA: 0x000AA234 File Offset: 0x000A8434
	public static void RebindSkinnedMeshRenderers(SkinnedMeshRenderer rebindFromRenderer, SkinnedMeshRenderer rebindToRenderer)
	{
		LookCreator.m_boneMapDict.Clear();
		foreach (Transform transform in rebindFromRenderer.bones)
		{
			LookCreator.m_boneMapDict.Add(transform.gameObject.name, transform);
		}
		Transform[] array = new Transform[rebindToRenderer.bones.Length];
		for (int j = 0; j < rebindToRenderer.bones.Length; j++)
		{
			GameObject gameObject = rebindToRenderer.bones[j].gameObject;
			if (!LookCreator.m_boneMapDict.TryGetValue(gameObject.name, out array[j]))
			{
				Debug.Log("Unable to map bone \"" + gameObject.name + "\" to target skeleton.");
				break;
			}
		}
		rebindToRenderer.rootBone = rebindFromRenderer.rootBone;
		rebindToRenderer.bones = array;
	}

	// Token: 0x04001EEB RID: 7915
	private static Dictionary<string, Transform> m_boneMapDict = new Dictionary<string, Transform>();
}
