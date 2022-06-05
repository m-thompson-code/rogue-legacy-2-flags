using System;
using MoreMountains.CorgiEngine;
using TMPro;
using UnityEngine;

// Token: 0x0200080B RID: 2059
public class HealingRoomPropController : DualChoicePropController, ILocalizable
{
	// Token: 0x06003F72 RID: 16242 RVA: 0x000FDC38 File Offset: 0x000FBE38
	protected override void Awake()
	{
		this.m_healthChangeArgs = new MaxHealthChangeEventArgs(0f, 0f);
		this.m_updateTooltips = new Action<MonoBehaviour, EventArgs>(this.UpdateTooltips);
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		base.Awake();
	}

	// Token: 0x06003F73 RID: 16243 RVA: 0x000FDC88 File Offset: 0x000FBE88
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerManaChange, this.m_updateTooltips);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerMaxHealthChange, this.m_updateTooltips);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerHealthChange, this.m_updateTooltips);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.RelicStatsChanged, this.m_updateTooltips);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06003F74 RID: 16244 RVA: 0x000230E9 File Offset: 0x000212E9
	protected override void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
		base.OnDisable();
		this.RemovePlayerEventListeners();
	}

	// Token: 0x06003F75 RID: 16245 RVA: 0x00023104 File Offset: 0x00021304
	private void RemovePlayerEventListeners()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerManaChange, this.m_updateTooltips);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerMaxHealthChange, this.m_updateTooltips);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHealthChange, this.m_updateTooltips);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.RelicStatsChanged, this.m_updateTooltips);
	}

	// Token: 0x06003F76 RID: 16246 RVA: 0x000FDCD4 File Offset: 0x000FBED4
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

	// Token: 0x06003F77 RID: 16247 RVA: 0x000FDD50 File Offset: 0x000FBF50
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

	// Token: 0x06003F78 RID: 16248 RVA: 0x000FDDFC File Offset: 0x000FBFFC
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

	// Token: 0x06003F79 RID: 16249 RVA: 0x000FDE84 File Offset: 0x000FC084
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

	// Token: 0x06003F7A RID: 16250 RVA: 0x000FDF34 File Offset: 0x000FC134
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

	// Token: 0x06003F7B RID: 16251 RVA: 0x000FE018 File Offset: 0x000FC218
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

	// Token: 0x06003F7C RID: 16252 RVA: 0x00023138 File Offset: 0x00021338
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

	// Token: 0x06003F7D RID: 16253 RVA: 0x000FE0EC File Offset: 0x000FC2EC
	public void RefreshText(object sender, EventArgs args)
	{
		base.LeftInfoTextBox.HeaderText.text = LocalizationManager.GetString("LOC_ID_HEALING_ROOM_TITLE_RESTORE_HEALTH_1", false, false);
		base.LeftInfoTextBox.DescriptionText.text = LocalizationManager.GetString("LOC_ID_HEALING_ROOM_DESCRIPTION_RESTORE_HEALTH_1", false, false);
		base.RightInfoTextBox.HeaderText.text = LocalizationManager.GetString("LOC_ID_HEALING_ROOM_TITLE_MAX_HEALTH_1", false, false);
		base.RightInfoTextBox.DescriptionText.text = LocalizationManager.GetString("LOC_ID_HEALING_ROOM_DESCRIPTION_MAX_HEALTH_1", false, false);
		this.UpdateTooltips(null, null);
	}

	// Token: 0x04003199 RID: 12697
	private MaxHealthChangeEventArgs m_healthChangeArgs;

	// Token: 0x0400319A RID: 12698
	private Action<MonoBehaviour, EventArgs> m_updateTooltips;

	// Token: 0x0400319B RID: 12699
	private Action<MonoBehaviour, EventArgs> m_refreshText;
}
