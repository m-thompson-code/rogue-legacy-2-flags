using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001D0 RID: 464
[RequireComponent(typeof(PlayerController))]
public class CharacterClass : MonoBehaviour
{
	// Token: 0x17000A10 RID: 2576
	// (get) Token: 0x060012A3 RID: 4771 RVA: 0x00036D87 File Offset: 0x00034F87
	// (set) Token: 0x060012A4 RID: 4772 RVA: 0x00036D8F File Offset: 0x00034F8F
	public bool IsInitialized { get; private set; }

	// Token: 0x17000A11 RID: 2577
	// (get) Token: 0x060012A5 RID: 4773 RVA: 0x00036D98 File Offset: 0x00034F98
	public bool OverrideSaveFileValues
	{
		get
		{
			return !SaveManager.IsRunning && !LineageWindowController.CharacterLoadedFromLineage;
		}
	}

	// Token: 0x17000A12 RID: 2578
	// (get) Token: 0x060012A6 RID: 4774 RVA: 0x00036DAD File Offset: 0x00034FAD
	public ClassData ClassData
	{
		get
		{
			return this.m_classData;
		}
	}

	// Token: 0x17000A13 RID: 2579
	// (get) Token: 0x060012A7 RID: 4775 RVA: 0x00036DB5 File Offset: 0x00034FB5
	// (set) Token: 0x060012A8 RID: 4776 RVA: 0x00036DC0 File Offset: 0x00034FC0
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

	// Token: 0x17000A14 RID: 2580
	// (get) Token: 0x060012A9 RID: 4777 RVA: 0x00036E0F File Offset: 0x0003500F
	// (set) Token: 0x060012AA RID: 4778 RVA: 0x00036E17 File Offset: 0x00035017
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

	// Token: 0x17000A15 RID: 2581
	// (get) Token: 0x060012AB RID: 4779 RVA: 0x00036E30 File Offset: 0x00035030
	// (set) Token: 0x060012AC RID: 4780 RVA: 0x00036E38 File Offset: 0x00035038
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

	// Token: 0x17000A16 RID: 2582
	// (get) Token: 0x060012AD RID: 4781 RVA: 0x00036E51 File Offset: 0x00035051
	// (set) Token: 0x060012AE RID: 4782 RVA: 0x00036E59 File Offset: 0x00035059
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

	// Token: 0x060012AF RID: 4783 RVA: 0x00036E72 File Offset: 0x00035072
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

	// Token: 0x060012B0 RID: 4784 RVA: 0x00036EB0 File Offset: 0x000350B0
	private void LoadPlayerSaveData()
	{
		CharacterData currentCharacter = SaveManager.PlayerSaveData.CurrentCharacter;
		this.m_classType = currentCharacter.ClassType;
		this.m_weaponAbilityType = currentCharacter.Weapon;
		this.m_spellAbilityType = currentCharacter.Spell;
		this.m_talentAbilityType = currentCharacter.Talent;
	}

	// Token: 0x060012B1 RID: 4785 RVA: 0x00036EF8 File Offset: 0x000350F8
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

	// Token: 0x060012B2 RID: 4786 RVA: 0x00036F07 File Offset: 0x00035107
	private void InitializeAbilities()
	{
		this.SetAbility(CastAbilityType.Weapon, this.WeaponAbilityType, true);
		this.SetAbility(CastAbilityType.Spell, this.SpellAbilityType, true);
		this.SetAbility(CastAbilityType.Talent, this.TalentAbilityType, true);
	}

	// Token: 0x060012B3 RID: 4787 RVA: 0x00036F34 File Offset: 0x00035134
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

	// Token: 0x060012B4 RID: 4788 RVA: 0x00036FA4 File Offset: 0x000351A4
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

	// Token: 0x060012B5 RID: 4789 RVA: 0x0003702C File Offset: 0x0003522C
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

	// Token: 0x04001309 RID: 4873
	[SerializeField]
	[ReadOnlyOnPlay]
	protected ClassType m_classType = ClassType.SwordClass;

	// Token: 0x0400130A RID: 4874
	[Space(10f)]
	[SerializeField]
	[ReadOnly]
	protected AbilityType m_weaponAbilityType;

	// Token: 0x0400130B RID: 4875
	[SerializeField]
	[ReadOnly]
	protected AbilityType m_spellAbilityType;

	// Token: 0x0400130C RID: 4876
	[SerializeField]
	[ReadOnly]
	protected AbilityType m_talentAbilityType;

	// Token: 0x0400130D RID: 4877
	[Space(10f)]
	[SerializeField]
	[ReadOnly]
	protected ClassData m_classData;

	// Token: 0x0400130E RID: 4878
	private PlayerController m_playerController;
}
