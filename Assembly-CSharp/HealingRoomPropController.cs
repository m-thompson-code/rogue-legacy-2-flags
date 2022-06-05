using System;
using MoreMountains.CorgiEngine;
using TMPro;
using UnityEngine;

// Token: 0x020004D5 RID: 1237
public class HealingRoomPropController : DualChoicePropController, ILocalizable
{
	// Token: 0x06002E12 RID: 11794 RVA: 0x0009B688 File Offset: 0x00099888
	protected override void Awake()
	{
		this.m_healthChangeArgs = new MaxHealthChangeEventArgs(0f, 0f);
		this.m_updateTooltips = new Action<MonoBehaviour, EventArgs>(this.UpdateTooltips);
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		base.Awake();
	}

	// Token: 0x06002E13 RID: 11795 RVA: 0x0009B6D8 File Offset: 0x000998D8
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerManaChange, this.m_updateTooltips);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerMaxHealthChange, this.m_updateTooltips);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerHealthChange, this.m_updateTooltips);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.RelicStatsChanged, this.m_updateTooltips);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06002E14 RID: 11796 RVA: 0x0009B724 File Offset: 0x00099924
	protected override void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
		base.OnDisable();
		this.RemovePlayerEventListeners();
	}

	// Token: 0x06002E15 RID: 11797 RVA: 0x0009B73F File Offset: 0x0009993F
	private void RemovePlayerEventListeners()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerManaChange, this.m_updateTooltips);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerMaxHealthChange, this.m_updateTooltips);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHealthChange, this.m_updateTooltips);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.RelicStatsChanged, this.m_updateTooltips);
	}

	// Token: 0x06002E16 RID: 11798 RVA: 0x0009B774 File Offset: 0x00099974
	private void CalculateHealthAndManaRestored(PlayerController playerController, out float hpGain, out float mpGain)
	{
		float num = (float)playerController.ActualMaxHealth;
		float actualMagic = playerController.ActualMagic;
		hpGain = (float)Mathf.CeilToInt(num * 0f + actualMagic * 5f);
		if (TraitManager.IsTraitActive(TraitType.MegaHealth))
		{
			hpGain = 0f;
		}
		hpGain = Mathf.Clamp(hpGain, 0f, num);
		float num2 = (float)playerController.ActualMaxMana;
		mpGain = (float)Mathf.CeilToInt(num2 * 1f);
		mpGain = Mathf.Clamp(mpGain, 0f, num2);
	}

	// Token: 0x06002E17 RID: 11799 RVA: 0x0009B7F0 File Offset: 0x000999F0
	private void CalculateMaxHealthGain(PlayerController playerController, out float damageDealt, out float maxHealthGained)
	{
		damageDealt = (float)Mathf.Clamp(Mathf.Abs(Mathf.CeilToInt((float)playerController.ActualMaxHealth * -0.3f)), 1, int.MaxValue);
		playerController.DisableArmor = true;
		CharacterStates.MovementStates previousMovementState = playerController.PreviousMovementState;
		CharacterStates.MovementStates movementState = playerController.MovementState;
		playerController.MovementState = CharacterStates.MovementStates.Idle;
		CriticalStrikeType criticalStrikeType;
		float num;
		damageDealt = playerController.CalculateDamageTaken(playerController, out criticalStrikeType, out num, damageDealt, false, true);
		playerController.MovementState = previousMovementState;
		playerController.MovementState = movementState;
		playerController.DisableArmor = false;
		playerController.RelicMaxHealthMod += 0.15f;
		int actualMaxHealth = playerController.ActualMaxHealth;
		playerController.RelicMaxHealthMod -= 0.15f;
		maxHealthGained = (float)(actualMaxHealth - playerController.ActualMaxHealth);
	}

	// Token: 0x06002E18 RID: 11800 RVA: 0x0009B89C File Offset: 0x00099A9C
	private void UpdateTooltips(MonoBehaviour sender, EventArgs args)
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		float num;
		float num2;
		this.CalculateHealthAndManaRestored(playerController, out num, out num2);
		base.LeftInfoTextBox.SubHeaderText.text = string.Format(LocalizationManager.GetString("LOC_ID_HEALING_ROOM_SUBHEADER_RESTORE_HEALTH_1", false, false), (int)num, (int)num2);
		float num3;
		float num4;
		this.CalculateMaxHealthGain(playerController, out num3, out num4);
		base.RightInfoTextBox.SubHeaderText.text = string.Format(LocalizationManager.GetString("LOC_ID_HEALING_ROOM_SUBHEADER_MAX_HEALTH_1", false, false), (int)num3, (int)num4);
	}

	// Token: 0x06002E19 RID: 11801 RVA: 0x0009B924 File Offset: 0x00099B24
	protected override void InitializePooledPropOnEnter()
	{
		base.InitializePooledPropOnEnter();
		base.LeftIcon.sprite = IconLibrary.GetMiscIcon(MiscIconType.HealingRoom_RestoreHPMP);
		base.LeftInfoTextBox.HeaderText.text = LocalizationManager.GetString("LOC_ID_HEALING_ROOM_TITLE_RESTORE_HEALTH_1", false, false);
		base.LeftInfoTextBox.DescriptionText.text = LocalizationManager.GetString("LOC_ID_HEALING_ROOM_DESCRIPTION_RESTORE_HEALTH_1", false, false);
		base.RightIcon.sprite = IconLibrary.GetMiscIcon(MiscIconType.HealingRoom_IncreaseMaxHP);
		base.RightInfoTextBox.HeaderText.text = LocalizationManager.GetString("LOC_ID_HEALING_ROOM_TITLE_MAX_HEALTH_1", false, false);
		base.RightInfoTextBox.DescriptionText.text = LocalizationManager.GetString("LOC_ID_HEALING_ROOM_DESCRIPTION_MAX_HEALTH_1", false, false);
		this.UpdateTooltips(null, null);
	}

	// Token: 0x06002E1A RID: 11802 RVA: 0x0009B9D4 File Offset: 0x00099BD4
	public void RestoreHPMP()
	{
		this.RemovePlayerEventListeners();
		PlayerController playerController = PlayerManager.GetPlayerController();
		float num;
		float num2;
		this.CalculateHealthAndManaRestored(playerController, out num, out num2);
		playerController.SetHealth(num, true, true);
		playerController.SetMana(num2, true, true, false);
		Vector2 absPos = playerController.Midpoint;
		absPos.y += playerController.CollisionBounds.height / 2f;
		string str = string.Format(LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_HEALTH_RESTORE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), (int)num);
		string str2 = string.Format(LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_MANA_RESTORE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), (int)num2);
		TextPopupManager.DisplayTextAtAbsPos(TextPopupType.HPGained, str + "\n<color=#47FFFB>" + str2 + "</color>", absPos, null, TextAlignmentOptions.Center);
		this.PropComplete();
		this.DisableProp(true);
	}

	// Token: 0x06002E1B RID: 11803 RVA: 0x0009BAB8 File Offset: 0x00099CB8
	public void IncreaseMaxHealth()
	{
		this.RemovePlayerEventListeners();
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.CharacterHitResponse.StopInvincibleTime();
		float damageOverride = (float)Mathf.Clamp(Mathf.Abs(Mathf.CeilToInt((float)playerController.ActualMaxHealth * -0.3f)), 1, int.MaxValue);
		playerController.DisableArmor = true;
		playerController.CharacterHitResponse.StartHitResponse(playerController.gameObject, playerController, damageOverride, false, true);
		playerController.DisableArmor = false;
		if (!playerController.IsDead)
		{
			int actualMaxHealth = playerController.ActualMaxHealth;
			SaveManager.PlayerSaveData.TemporaryMaxHealthMods += 0.15f;
			playerController.InitializeHealthMods();
			int actualMaxHealth2 = playerController.ActualMaxHealth;
			this.m_healthChangeArgs.Initialise((float)actualMaxHealth2, (float)actualMaxHealth);
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerMaxHealthChange, this, this.m_healthChangeArgs);
			this.PropComplete();
			this.DisableProp(true);
			return;
		}
		this.PropComplete();
		this.DisableProp(false);
	}

	// Token: 0x06002E1C RID: 11804 RVA: 0x0009BB8C File Offset: 0x00099D8C
	protected override void DisableProp(bool firstTimeDisabled)
	{
		if (firstTimeDisabled)
		{
			base.Animator.SetBool("Used", true);
		}
		else
		{
			base.Animator.SetBool("InstantlyUsed", true);
		}
		base.DisableProp(firstTimeDisabled);
	}

	// Token: 0x06002E1D RID: 11805 RVA: 0x0009BBBC File Offset: 0x00099DBC
	public void RefreshText(object sender, EventArgs args)
	{
		base.LeftInfoTextBox.HeaderText.text = LocalizationManager.GetString("LOC_ID_HEALING_ROOM_TITLE_RESTORE_HEALTH_1", false, false);
		base.LeftInfoTextBox.DescriptionText.text = LocalizationManager.GetString("LOC_ID_HEALING_ROOM_DESCRIPTION_RESTORE_HEALTH_1", false, false);
		base.RightInfoTextBox.HeaderText.text = LocalizationManager.GetString("LOC_ID_HEALING_ROOM_TITLE_MAX_HEALTH_1", false, false);
		base.RightInfoTextBox.DescriptionText.text = LocalizationManager.GetString("LOC_ID_HEALING_ROOM_DESCRIPTION_MAX_HEALTH_1", false, false);
		this.UpdateTooltips(null, null);
	}

	// Token: 0x040024C7 RID: 9415
	private MaxHealthChangeEventArgs m_healthChangeArgs;

	// Token: 0x040024C8 RID: 9416
	private Action<MonoBehaviour, EventArgs> m_updateTooltips;

	// Token: 0x040024C9 RID: 9417
	private Action<MonoBehaviour, EventArgs> m_refreshText;
}
