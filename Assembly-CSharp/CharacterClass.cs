using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200034B RID: 843
[RequireComponent(typeof(PlayerController))]
public class CharacterClass : MonoBehaviour
{
	// Token: 0x17000CE0 RID: 3296
	// (get) Token: 0x06001B21 RID: 6945 RVA: 0x0000E154 File Offset: 0x0000C354
	// (set) Token: 0x06001B22 RID: 6946 RVA: 0x0000E15C File Offset: 0x0000C35C
	public bool IsInitialized { get; private set; }

	// Token: 0x17000CE1 RID: 3297
	// (get) Token: 0x06001B23 RID: 6947 RVA: 0x0000E165 File Offset: 0x0000C365
	public bool OverrideSaveFileValues
	{
		get
		{
			return !SaveManager.IsRunning && !LineageWindowController.CharacterLoadedFromLineage;
		}
	}

	// Token: 0x17000CE2 RID: 3298
	// (get) Token: 0x06001B24 RID: 6948 RVA: 0x0000E17A File Offset: 0x0000C37A
	public ClassData ClassData
	{
		get
		{
			return this.m_classData;
		}
	}

	// Token: 0x17000CE3 RID: 3299
	// (get) Token: 0x06001B25 RID: 6949 RVA: 0x0000E182 File Offset: 0x0000C382
	// (set) Token: 0x06001B26 RID: 6950 RVA: 0x00094418 File Offset: 0x00092618
	public ClassType ClassType
	{
		get
		{
			return this.m_classType;
		}
		set
		{
			this.m_classType = value;
			if (Application.isPlaying)
			{
				this.m_classData = ClassLibrary.GetClassData(this.m_classType);
				LookCreator.InitializeClassLook(this.m_classType, this.m_playerController.LookController);
				this.m_playerController.InitializeAllMods(true, true);
			}
		}
	}

	// Token: 0x17000CE4 RID: 3300
	// (get) Token: 0x06001B27 RID: 6951 RVA: 0x0000E18A File Offset: 0x0000C38A
	// (set) Token: 0x06001B28 RID: 6952 RVA: 0x0000E192 File Offset: 0x0000C392
	public AbilityType WeaponAbilityType
	{
		get
		{
			return this.m_weaponAbilityType;
		}
		set
		{
			this.m_weaponAbilityType = value;
			if (Application.isPlaying)
			{
				this.SetAbility(CastAbilityType.Weapon, value, true);
			}
		}
	}

	// Token: 0x17000CE5 RID: 3301
	// (get) Token: 0x06001B29 RID: 6953 RVA: 0x0000E1AB File Offset: 0x0000C3AB
	// (set) Token: 0x06001B2A RID: 6954 RVA: 0x0000E1B3 File Offset: 0x0000C3B3
	public AbilityType SpellAbilityType
	{
		get
		{
			return this.m_spellAbilityType;
		}
		set
		{
			this.m_spellAbilityType = value;
			if (Application.isPlaying)
			{
				this.SetAbility(CastAbilityType.Spell, value, true);
			}
		}
	}

	// Token: 0x17000CE6 RID: 3302
	// (get) Token: 0x06001B2B RID: 6955 RVA: 0x0000E1CC File Offset: 0x0000C3CC
	// (set) Token: 0x06001B2C RID: 6956 RVA: 0x0000E1D4 File Offset: 0x0000C3D4
	public AbilityType TalentAbilityType
	{
		get
		{
			return this.m_talentAbilityType;
		}
		set
		{
			this.m_talentAbilityType = value;
			if (Application.isPlaying)
			{
				this.SetAbility(CastAbilityType.Talent, value, true);
			}
		}
	}

	// Token: 0x06001B2D RID: 6957 RVA: 0x0000E1ED File Offset: 0x0000C3ED
	protected virtual void Awake()
	{
		if (Application.isPlaying)
		{
			this.m_playerController = base.GetComponent<PlayerController>();
			if (!this.OverrideSaveFileValues)
			{
				this.LoadPlayerSaveData();
			}
			this.m_classData = ClassLibrary.GetClassData(this.m_classType);
			this.IsInitialized = true;
		}
	}

	// Token: 0x06001B2E RID: 6958 RVA: 0x00094468 File Offset: 0x00092668
	private void LoadPlayerSaveData()
	{
		CharacterData currentCharacter = SaveManager.PlayerSaveData.CurrentCharacter;
		this.m_classType = currentCharacter.ClassType;
		this.m_weaponAbilityType = currentCharacter.Weapon;
		this.m_spellAbilityType = currentCharacter.Spell;
		this.m_talentAbilityType = currentCharacter.Talent;
	}

	// Token: 0x06001B2F RID: 6959 RVA: 0x0000E228 File Offset: 0x0000C428
	private IEnumerator Start()
	{
		while (!this.m_playerController.IsInitialized)
		{
			yield return null;
		}
		if (this.m_classData != null)
		{
			this.InitializeAbilities();
		}
		yield break;
	}

	// Token: 0x06001B30 RID: 6960 RVA: 0x0000E237 File Offset: 0x0000C437
	private void InitializeAbilities()
	{
		this.SetAbility(CastAbilityType.Weapon, this.WeaponAbilityType, true);
		this.SetAbility(CastAbilityType.Spell, this.SpellAbilityType, true);
		this.SetAbility(CastAbilityType.Talent, this.TalentAbilityType, true);
	}

	// Token: 0x06001B31 RID: 6961 RVA: 0x000944B0 File Offset: 0x000926B0
	private AbilityType ChooseRandomAbility(CastAbilityType castAbilityType)
	{
		AbilityType[] array = null;
		switch (castAbilityType)
		{
		case CastAbilityType.Weapon:
			array = this.m_classData.WeaponData.WeaponAbilityArray;
			break;
		case CastAbilityType.Spell:
			array = this.m_classData.SpellData.SpellAbilityArray;
			break;
		case CastAbilityType.Talent:
			array = this.m_classData.TalentData.TalentAbilityArray;
			break;
		}
		int randomNumber = RNGManager.GetRandomNumber(RngID.Lineage, "CharacterClass.ChooseRandomAbility()", 0, array.Length);
		return array[randomNumber];
	}

	// Token: 0x06001B32 RID: 6962 RVA: 0x00094520 File Offset: 0x00092720
	public void SetAbility(CastAbilityType castAbilityType, AbilityType abilityType, bool destroyOldAbility = true)
	{
		if (abilityType == AbilityType.Random)
		{
			abilityType = this.ChooseRandomAbility(castAbilityType);
		}
		switch (castAbilityType)
		{
		case CastAbilityType.Weapon:
			this.m_weaponAbilityType = abilityType;
			break;
		case CastAbilityType.Spell:
			this.m_spellAbilityType = abilityType;
			break;
		case CastAbilityType.Talent:
			this.m_talentAbilityType = abilityType;
			break;
		}
		this.m_playerController.CastAbility.SetAbility(castAbilityType, abilityType, destroyOldAbility);
		if (this.m_playerController.LookController.IsClassLookInitialized)
		{
			this.m_playerController.LookController.InitializeEquipmentLook(SaveManager.PlayerSaveData.CurrentCharacter);
		}
	}

	// Token: 0x06001B33 RID: 6963 RVA: 0x000945A8 File Offset: 0x000927A8
	public void SetAbility(CastAbilityType castAbilityType, BaseAbility_RL ability, bool destroyOldAbility = false)
	{
		AbilityType abilityType = AbilityType.None;
		if (ability != null)
		{
			abilityType = ability.AbilityType;
		}
		switch (castAbilityType)
		{
		case CastAbilityType.Weapon:
			this.m_weaponAbilityType = abilityType;
			break;
		case CastAbilityType.Spell:
			this.m_spellAbilityType = abilityType;
			break;
		case CastAbilityType.Talent:
			this.m_talentAbilityType = abilityType;
			break;
		}
		this.m_playerController.CastAbility.SetAbility(castAbilityType, ability, destroyOldAbility);
		if (this.m_playerController.LookController.IsClassLookInitialized)
		{
			this.m_playerController.LookController.InitializeEquipmentLook(SaveManager.PlayerSaveData.CurrentCharacter);
		}
	}

	// Token: 0x0400193F RID: 6463
	[SerializeField]
	[ReadOnlyOnPlay]
	protected ClassType m_classType = ClassType.SwordClass;

	// Token: 0x04001940 RID: 6464
	[Space(10f)]
	[SerializeField]
	[ReadOnly]
	protected AbilityType m_weaponAbilityType;

	// Token: 0x04001941 RID: 6465
	[SerializeField]
	[ReadOnly]
	protected AbilityType m_spellAbilityType;

	// Token: 0x04001942 RID: 6466
	[SerializeField]
	[ReadOnly]
	protected AbilityType m_talentAbilityType;

	// Token: 0x04001943 RID: 6467
	[Space(10f)]
	[SerializeField]
	[ReadOnly]
	protected ClassData m_classData;

	// Token: 0x04001944 RID: 6468
	private PlayerController m_playerController;
}
