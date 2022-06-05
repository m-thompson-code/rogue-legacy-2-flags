using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002AC RID: 684
public class PlayerLookController : LookController
{
	// Token: 0x17000C77 RID: 3191
	// (get) Token: 0x06001B4E RID: 6990 RVA: 0x0005720D File Offset: 0x0005540D
	// (set) Token: 0x06001B4F RID: 6991 RVA: 0x00057215 File Offset: 0x00055415
	public bool IsClassLookInitialized { get; private set; }

	// Token: 0x06001B50 RID: 6992 RVA: 0x0005721E File Offset: 0x0005541E
	protected override void Awake()
	{
		base.Awake();
		this.m_onEquippedChanged = new Action<MonoBehaviour, EventArgs>(this.OnEquippedChanged);
	}

	// Token: 0x06001B51 RID: 6993 RVA: 0x00057238 File Offset: 0x00055438
	public override void Initialize()
	{
		base.Initialize();
		this.m_playerController = base.GetComponent<PlayerController>();
		this.m_storedBaseScale = base.gameObject.transform.localScale;
		if (PlayerLookController.m_critMatPropertyBlock == null)
		{
			PlayerLookController.m_critMatPropertyBlock = new MaterialPropertyBlock();
			ColorUtility.TryParseHtmlString("#FFB000", out PlayerLookController.m_critStartColor);
			ColorUtility.TryParseHtmlString("#795C00", out PlayerLookController.m_critEndColor);
		}
	}

	// Token: 0x06001B52 RID: 6994 RVA: 0x0005729E File Offset: 0x0005549E
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.EquippedChanged, this.m_onEquippedChanged);
	}

	// Token: 0x06001B53 RID: 6995 RVA: 0x000572AC File Offset: 0x000554AC
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.EquippedChanged, this.m_onEquippedChanged);
	}

	// Token: 0x06001B54 RID: 6996 RVA: 0x000572BA File Offset: 0x000554BA
	protected virtual IEnumerator Start()
	{
		if (this.m_loadFromSaveFile)
		{
			yield return new WaitUntil(() => this.m_playerController.IsInitialized && this.m_playerController.CharacterClass.IsInitialized);
			CharacterData currentCharacter = SaveManager.PlayerSaveData.CurrentCharacter;
			if (this.m_playerController.CharacterClass.OverrideSaveFileValues)
			{
				currentCharacter.ClassType = this.m_playerController.CharacterClass.ClassType;
				currentCharacter.Weapon = this.m_playerController.CastAbility.GetAbility(CastAbilityType.Weapon, false).AbilityType;
			}
			if (currentCharacter.Weapon == AbilityType.None)
			{
				BaseAbility_RL ability = this.m_playerController.CastAbility.GetAbility(CastAbilityType.Weapon, false);
				if (ability != null)
				{
					currentCharacter.Weapon = ability.AbilityType;
				}
			}
			this.InitializeLook(currentCharacter);
		}
		yield break;
	}

	// Token: 0x06001B55 RID: 6997 RVA: 0x000572C9 File Offset: 0x000554C9
	public override void InitializeLook(CharacterData charData)
	{
		base.InitializeLook(charData);
		this.InitializeScale(charData);
		this.InitializeTraitLook(charData);
		this.InitializeEquipmentLook(charData);
		base.UpdateCustomMeshArray();
		base.ApplyOutlineScale();
		this.IsClassLookInitialized = true;
	}

	// Token: 0x06001B56 RID: 6998 RVA: 0x000572FC File Offset: 0x000554FC
	public void InitializeScale(CharacterData charData)
	{
		Vector3 storedBaseScale = new Vector3(1.4f, 1.4f, 1f);
		if (this.m_useCurrentScaleAsBase)
		{
			storedBaseScale = this.m_storedBaseScale;
		}
		if (!this.m_ignoreScaleTraits)
		{
			if (charData.TraitOne == TraitType.YouAreLarge || charData.TraitTwo == TraitType.YouAreLarge)
			{
				storedBaseScale.x *= 1.5f;
				storedBaseScale.y *= 1.5f;
			}
			else if (charData.TraitOne == TraitType.YouAreSmall || charData.TraitTwo == TraitType.YouAreSmall)
			{
				storedBaseScale.x *= 0.55f;
				storedBaseScale.y *= 0.55f;
			}
		}
		if (this.m_clampPlayerScale)
		{
			storedBaseScale.x = Mathf.Clamp(storedBaseScale.x, 0.77f, 2.1f);
			storedBaseScale.y = Mathf.Clamp(storedBaseScale.y, 0.77f, 2.1f);
		}
		base.transform.localScale = storedBaseScale;
	}

	// Token: 0x06001B57 RID: 6999 RVA: 0x000573EC File Offset: 0x000555EC
	private void SetMainColor(SkinnedMeshRenderer renderer, Color color)
	{
		renderer.GetPropertyBlock(base.PropertyBlock);
		base.PropertyBlock.SetColor(ShaderID_RL._MainColor, color);
		renderer.SetPropertyBlock(base.PropertyBlock);
	}

	// Token: 0x06001B58 RID: 7000 RVA: 0x00057418 File Offset: 0x00055618
	public void InitializeTraitLook(CharacterData charData)
	{
		base.Animator.SetFloat("BoneStructureType", 0f);
		base.Animator.SetFloat("LimbType", 0f);
		if (charData.TraitOne == TraitType.PlayerKnockedLow || charData.TraitTwo == TraitType.PlayerKnockedLow)
		{
			base.Animator.SetFloat("BoneStructureType", 2f);
		}
		if (charData.TraitOne == TraitType.PlayerKnockedFar || charData.TraitTwo == TraitType.PlayerKnockedFar)
		{
			base.Animator.SetFloat("BoneStructureType", 1f);
		}
		if (charData.TraitOne == TraitType.EnemyKnockedLow || charData.TraitTwo == TraitType.EnemyKnockedLow)
		{
			base.Animator.SetFloat("LimbType", 1f);
		}
		if (charData.TraitOne == TraitType.EnemyKnockedFar || charData.TraitTwo == TraitType.EnemyKnockedFar)
		{
			base.Animator.SetFloat("LimbType", 2f);
		}
		if (charData.ClassType == ClassType.CannonClass)
		{
			if (base.RightEyeGeo.gameObject.activeSelf)
			{
				base.RightEyeGeo.gameObject.SetActive(false);
			}
		}
		else if (!base.RightEyeGeo.gameObject.activeSelf)
		{
			base.RightEyeGeo.gameObject.SetActive(true);
		}
		if (charData.TraitOne == TraitType.Vampire || charData.TraitTwo == TraitType.Vampire)
		{
			Color color = new Color(1f, 1f, 1f, 1f);
			this.SetMainColor(base.HeadGeo, color);
			Color color2 = new Color(0.15f, 0.17f, 0.23f, 1f);
			this.SetMainColor(base.HelmetHairGeo, color2);
			this.SetMainColor(base.ChestHairGeo, color2);
			Color color3 = new Color(1f, 0f, 0f, 1f);
			this.SetMainColor(base.LeftEyeGeo, color3);
			this.SetMainColor(base.RightEyeGeo, color3);
			base.MouthGeo.sharedMaterial = LookLibrary.VampireFangsMaterial;
		}
		if (charData.TraitOne == TraitType.YouAreBlue || charData.TraitTwo == TraitType.YouAreBlue)
		{
			Color color4 = new Color(0.27450982f, 0.6313726f, 1f);
			this.SetMainColor(base.HeadGeo, color4);
		}
		if (charData.TraitOne == TraitType.BounceTerrain || charData.TraitTwo == TraitType.BounceTerrain)
		{
			base.HeadGeo.sharedMaterial = LookLibrary.ClownHeadMaterial;
			Color color5 = new Color(1f, 1f, 1f, 1f);
			this.SetMainColor(base.HeadGeo, color5);
			Color color6 = new Color(1f, 0.17f, 0.23f, 1f);
			this.SetMainColor(base.HelmetHairGeo, color6);
			this.SetMainColor(base.ChestHairGeo, color6);
			base.LeftEyeGeo.sharedMaterial = LookLibrary.ClownEyesMaterial;
			base.RightEyeGeo.sharedMaterial = LookLibrary.ClownEyesMaterial;
			base.MouthGeo.sharedMaterial = LookLibrary.ClownMouthMaterial;
		}
	}

	// Token: 0x06001B59 RID: 7001 RVA: 0x00057710 File Offset: 0x00055910
	public void InitializeEquipmentLook(CharacterData charData)
	{
		LookCreator.InitializeHelmetLook(charData.HeadEquipmentType, this);
		LookCreator.InitializeArmorLook(charData.ChestEquipmentType, this);
		bool hasCantAttackTrait = charData.TraitOne == TraitType.CantAttack || charData.TraitTwo == TraitType.CantAttack;
		LookCreator.InitializeWeaponLook(charData.EdgeEquipmentType, this, charData.Weapon, hasCantAttackTrait);
		LookCreator.InitializeCapeLook(charData.CapeEquipmentType, this);
		base.UpdateCustomMeshArray();
		base.ApplyOutlineScale();
		if (this.m_playerController)
		{
			this.m_playerController.RecreateRendererArray();
			this.m_playerController.ResetRendererArrayColor();
			this.m_playerController.BlinkPulseEffect.ResetAllBlackFills();
		}
	}

	// Token: 0x06001B5A RID: 7002 RVA: 0x000577B4 File Offset: 0x000559B4
	private void OnEquippedChanged(MonoBehaviour sender, EventArgs args)
	{
		if (this.m_playerController == null)
		{
			if (PlayerManager.IsInstantiated)
			{
				this.m_playerController = PlayerManager.GetPlayerController();
			}
			if (this.m_playerController == null)
			{
				return;
			}
		}
		EquippedChangeEventArgs equippedChangeEventArgs = args as EquippedChangeEventArgs;
		switch (equippedChangeEventArgs.EquipmentCategoryType)
		{
		case EquipmentCategoryType.Weapon:
		{
			AbilityType weaponType = AbilityType.None;
			BaseAbility_RL ability = this.m_playerController.CastAbility.GetAbility(CastAbilityType.Weapon, false);
			if (ability != null)
			{
				weaponType = ability.AbilityType;
			}
			bool hasCantAttackTrait = SaveManager.PlayerSaveData.HasTrait(TraitType.CantAttack);
			LookCreator.InitializeWeaponLook(equippedChangeEventArgs.EquippedType, this, weaponType, hasCantAttackTrait);
			break;
		}
		case EquipmentCategoryType.Head:
			LookCreator.InitializeHelmetLook(equippedChangeEventArgs.EquippedType, this);
			break;
		case EquipmentCategoryType.Chest:
			LookCreator.InitializeArmorLook(equippedChangeEventArgs.EquippedType, this);
			break;
		case EquipmentCategoryType.Cape:
			LookCreator.InitializeCapeLook(equippedChangeEventArgs.EquippedType, this);
			break;
		}
		base.UpdateCustomMeshArray();
		base.ApplyOutlineScale();
		if (this.m_playerController)
		{
			this.m_playerController.RecreateRendererArray();
			this.m_playerController.ResetRendererArrayColor();
			this.m_playerController.BlinkPulseEffect.ResetAllBlackFills();
		}
	}

	// Token: 0x06001B5B RID: 7003 RVA: 0x000578C8 File Offset: 0x00055AC8
	public void SetCritBlinkEffectEnabled(bool enable, CritBlinkEffectTriggerType effectTriggerType)
	{
		if (enable)
		{
			this.m_critEffectTriggerBitMask |= (int)effectTriggerType;
			if (!this.m_critBlinkEnabled)
			{
				if (this.m_critBlinkCoroutine != null)
				{
					base.StopCoroutine(this.m_critBlinkCoroutine);
				}
				this.m_critBlinkCoroutine = null;
				this.m_critBlinkCoroutine = base.StartCoroutine(this.CritBlinkEffectCoroutine());
				this.m_critBlinkEnabled = true;
				return;
			}
		}
		else
		{
			this.m_critEffectTriggerBitMask &= (int)(~(int)effectTriggerType);
			if (this.m_critBlinkEnabled && this.m_critEffectTriggerBitMask == 0)
			{
				if (this.m_critBlinkCoroutine != null)
				{
					base.StopCoroutine(this.m_critBlinkCoroutine);
				}
				this.m_critBlinkCoroutine = null;
				SkinnedMeshRenderer currentWeaponGeo = this.m_playerController.LookController.CurrentWeaponGeo;
				if (currentWeaponGeo)
				{
					currentWeaponGeo.GetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
					PlayerLookController.m_critMatPropertyBlock.SetColor(ShaderID_RL._AddColor, this.m_storedWeapon1Color);
					currentWeaponGeo.SetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
				}
				SkinnedMeshRenderer secondaryWeaponGeo = this.m_playerController.LookController.SecondaryWeaponGeo;
				if (secondaryWeaponGeo)
				{
					secondaryWeaponGeo.GetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
					PlayerLookController.m_critMatPropertyBlock.SetColor(ShaderID_RL._AddColor, this.m_storedWeapon2Color);
					secondaryWeaponGeo.SetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
				}
				this.m_critBlinkEnabled = false;
			}
		}
	}

	// Token: 0x06001B5C RID: 7004 RVA: 0x000579F8 File Offset: 0x00055BF8
	public void ForceDisableCritBlinkEffect()
	{
		this.m_critEffectTriggerBitMask = 0;
		if (this.m_critBlinkCoroutine != null)
		{
			base.StopCoroutine(this.m_critBlinkCoroutine);
		}
		this.m_critBlinkCoroutine = null;
		if (this.m_critBlinkEnabled)
		{
			SkinnedMeshRenderer currentWeaponGeo = this.m_playerController.LookController.CurrentWeaponGeo;
			if (currentWeaponGeo)
			{
				currentWeaponGeo.GetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
				PlayerLookController.m_critMatPropertyBlock.SetColor(ShaderID_RL._AddColor, this.m_storedWeapon1Color);
				currentWeaponGeo.SetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
			}
			SkinnedMeshRenderer secondaryWeaponGeo = this.m_playerController.LookController.SecondaryWeaponGeo;
			if (secondaryWeaponGeo)
			{
				secondaryWeaponGeo.GetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
				PlayerLookController.m_critMatPropertyBlock.SetColor(ShaderID_RL._AddColor, this.m_storedWeapon2Color);
				secondaryWeaponGeo.SetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
			}
			this.m_critBlinkEnabled = false;
		}
	}

	// Token: 0x06001B5D RID: 7005 RVA: 0x00057AC1 File Offset: 0x00055CC1
	private IEnumerator CritBlinkEffectCoroutine()
	{
		SkinnedMeshRenderer weapon1Renderer = this.m_playerController.LookController.CurrentWeaponGeo;
		if (weapon1Renderer)
		{
			weapon1Renderer.GetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
			this.m_storedWeapon1Color = PlayerLookController.m_critMatPropertyBlock.GetColor(ShaderID_RL._AddColor);
		}
		SkinnedMeshRenderer weapon2Renderer = this.m_playerController.LookController.SecondaryWeaponGeo;
		if (weapon2Renderer)
		{
			weapon2Renderer.GetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
			this.m_storedWeapon2Color = PlayerLookController.m_critMatPropertyBlock.GetColor(ShaderID_RL._AddColor);
		}
		float intervalStartTime = Time.time;
		bool reverseTween = false;
		for (;;)
		{
			float num = (Time.time - intervalStartTime) / 0.2f;
			Color value;
			if (!reverseTween)
			{
				value = Color.Lerp(PlayerLookController.m_critStartColor, PlayerLookController.m_critEndColor, num);
			}
			else
			{
				value = Color.Lerp(PlayerLookController.m_critEndColor, PlayerLookController.m_critStartColor, num);
			}
			if (num >= 1f)
			{
				intervalStartTime = Time.time;
				reverseTween = !reverseTween;
			}
			if (weapon1Renderer)
			{
				weapon1Renderer.GetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
				PlayerLookController.m_critMatPropertyBlock.SetColor(ShaderID_RL._AddColor, value);
				weapon1Renderer.SetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
			}
			if (weapon2Renderer)
			{
				weapon2Renderer.GetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
				PlayerLookController.m_critMatPropertyBlock.SetColor(ShaderID_RL._AddColor, value);
				weapon2Renderer.SetPropertyBlock(PlayerLookController.m_critMatPropertyBlock);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04001906 RID: 6406
	[SerializeField]
	private bool m_clampPlayerScale;

	// Token: 0x04001907 RID: 6407
	[SerializeField]
	private bool m_useCurrentScaleAsBase;

	// Token: 0x04001908 RID: 6408
	[SerializeField]
	private bool m_ignoreScaleTraits;

	// Token: 0x04001909 RID: 6409
	[SerializeField]
	private bool m_loadFromSaveFile;

	// Token: 0x0400190A RID: 6410
	private Vector3 m_storedBaseScale;

	// Token: 0x0400190B RID: 6411
	private PlayerController m_playerController;

	// Token: 0x0400190C RID: 6412
	private Action<MonoBehaviour, EventArgs> m_onEquippedChanged;

	// Token: 0x0400190E RID: 6414
	private static MaterialPropertyBlock m_critMatPropertyBlock;

	// Token: 0x0400190F RID: 6415
	private const float CRIT_PULSE_INTERVAL = 0.2f;

	// Token: 0x04001910 RID: 6416
	private static Color m_critStartColor;

	// Token: 0x04001911 RID: 6417
	private static Color m_critEndColor;

	// Token: 0x04001912 RID: 6418
	private Coroutine m_critBlinkCoroutine;

	// Token: 0x04001913 RID: 6419
	private bool m_critBlinkEnabled;

	// Token: 0x04001914 RID: 6420
	private Color m_storedWeapon1Color;

	// Token: 0x04001915 RID: 6421
	private Color m_storedWeapon2Color;

	// Token: 0x04001916 RID: 6422
	private int m_critEffectTriggerBitMask;
}
