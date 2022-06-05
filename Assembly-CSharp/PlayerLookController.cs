using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200048F RID: 1167
public class PlayerLookController : LookController
{
	// Token: 0x17000FE2 RID: 4066
	// (get) Token: 0x060025BA RID: 9658 RVA: 0x00014F86 File Offset: 0x00013186
	// (set) Token: 0x060025BB RID: 9659 RVA: 0x00014F8E File Offset: 0x0001318E
	public bool IsClassLookInitialized { get; private set; }

	// Token: 0x060025BC RID: 9660 RVA: 0x00014F97 File Offset: 0x00013197
	protected override void Awake()
	{
		base.Awake();
		this.m_onEquippedChanged = new Action<MonoBehaviour, EventArgs>(this.OnEquippedChanged);
	}

	// Token: 0x060025BD RID: 9661 RVA: 0x000B3888 File Offset: 0x000B1A88
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

	// Token: 0x060025BE RID: 9662 RVA: 0x00014FB1 File Offset: 0x000131B1
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.EquippedChanged, this.m_onEquippedChanged);
	}

	// Token: 0x060025BF RID: 9663 RVA: 0x00014FBF File Offset: 0x000131BF
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.EquippedChanged, this.m_onEquippedChanged);
	}

	// Token: 0x060025C0 RID: 9664 RVA: 0x00014FCD File Offset: 0x000131CD
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

	// Token: 0x060025C1 RID: 9665 RVA: 0x00014FDC File Offset: 0x000131DC
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

	// Token: 0x060025C2 RID: 9666 RVA: 0x000B38F0 File Offset: 0x000B1AF0
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

	// Token: 0x060025C3 RID: 9667 RVA: 0x0001500D File Offset: 0x0001320D
	private void SetMainColor(SkinnedMeshRenderer renderer, Color color)
	{
		renderer.GetPropertyBlock(base.PropertyBlock);
		base.PropertyBlock.SetColor(ShaderID_RL._MainColor, color);
		renderer.SetPropertyBlock(base.PropertyBlock);
	}

	// Token: 0x060025C4 RID: 9668 RVA: 0x000B39E0 File Offset: 0x000B1BE0
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

	// Token: 0x060025C5 RID: 9669 RVA: 0x000B3CD8 File Offset: 0x000B1ED8
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

	// Token: 0x060025C6 RID: 9670 RVA: 0x000B3D7C File Offset: 0x000B1F7C
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

	// Token: 0x060025C7 RID: 9671 RVA: 0x000B3E90 File Offset: 0x000B2090
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

	// Token: 0x060025C8 RID: 9672 RVA: 0x000B3FC0 File Offset: 0x000B21C0
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

	// Token: 0x060025C9 RID: 9673 RVA: 0x00015038 File Offset: 0x00013238
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

	// Token: 0x040020BB RID: 8379
	[SerializeField]
	private bool m_clampPlayerScale;

	// Token: 0x040020BC RID: 8380
	[SerializeField]
	private bool m_useCurrentScaleAsBase;

	// Token: 0x040020BD RID: 8381
	[SerializeField]
	private bool m_ignoreScaleTraits;

	// Token: 0x040020BE RID: 8382
	[SerializeField]
	private bool m_loadFromSaveFile;

	// Token: 0x040020BF RID: 8383
	private Vector3 m_storedBaseScale;

	// Token: 0x040020C0 RID: 8384
	private PlayerController m_playerController;

	// Token: 0x040020C1 RID: 8385
	private Action<MonoBehaviour, EventArgs> m_onEquippedChanged;

	// Token: 0x040020C3 RID: 8387
	private static MaterialPropertyBlock m_critMatPropertyBlock;

	// Token: 0x040020C4 RID: 8388
	private const float CRIT_PULSE_INTERVAL = 0.2f;

	// Token: 0x040020C5 RID: 8389
	private static Color m_critStartColor;

	// Token: 0x040020C6 RID: 8390
	private static Color m_critEndColor;

	// Token: 0x040020C7 RID: 8391
	private Coroutine m_critBlinkCoroutine;

	// Token: 0x040020C8 RID: 8392
	private bool m_critBlinkEnabled;

	// Token: 0x040020C9 RID: 8393
	private Color m_storedWeapon1Color;

	// Token: 0x040020CA RID: 8394
	private Color m_storedWeapon2Color;

	// Token: 0x040020CB RID: 8395
	private int m_critEffectTriggerBitMask;
}
