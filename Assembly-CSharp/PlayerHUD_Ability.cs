using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003DA RID: 986
public class PlayerHUD_Ability : MonoBehaviour
{
	// Token: 0x0600244C RID: 9292 RVA: 0x00078848 File Offset: 0x00076A48
	private void Awake()
	{
		this.m_valueStringBuilder = new StringBuilder();
		this.m_prevValueStringBuilder = new StringBuilder();
		this.m_onChangeAbilityHandler = new Action<MonoBehaviour, EventArgs>(this.OnChangeAbilityHandler);
		this.m_onAmmoChangeHandler = new Action<MonoBehaviour, EventArgs>(this.OnAmmoChangeHandler);
		this.m_onManaChangeHandler = new Action<MonoBehaviour, EventArgs>(this.OnManaChangeHandler);
		this.m_onUpdateAbilityDisarmStateHandler = new Action<MonoBehaviour, EventArgs>(this.OnUpdateAbilityDisarmStateHandler);
		this.m_beginCooldown = new Action<object, CooldownEventArgs>(this.BeginCooldown);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.ChangeAbility, this.m_onChangeAbilityHandler);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.UpdateAbilityHUD, this.m_onChangeAbilityHandler);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerAmmoChange, this.m_onAmmoChangeHandler);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerManaChange, this.m_onManaChangeHandler);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.UpdateAbilityDisarmState, this.m_onUpdateAbilityDisarmStateHandler);
	}

	// Token: 0x0600244D RID: 9293 RVA: 0x00078908 File Offset: 0x00076B08
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.ChangeAbility, this.m_onChangeAbilityHandler);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.UpdateAbilityHUD, this.m_onChangeAbilityHandler);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerAmmoChange, this.m_onAmmoChangeHandler);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerManaChange, this.m_onManaChangeHandler);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.UpdateAbilityDisarmState, this.m_onUpdateAbilityDisarmStateHandler);
	}

	// Token: 0x0600244E RID: 9294 RVA: 0x00078956 File Offset: 0x00076B56
	private void OnEnable()
	{
		this.m_abilityReadyGO.SetActive(false);
		this.InitializeAbilityReadyGO();
	}

	// Token: 0x0600244F RID: 9295 RVA: 0x0007896C File Offset: 0x00076B6C
	private void InitializeAbilityReadyGO()
	{
		if (this.m_assignedAbility)
		{
			if (this.m_assignedAbility.ActualCost <= 0 && this.m_assignedAbility.ActualCooldownTime <= 0f)
			{
				return;
			}
			PlayerController playerController = PlayerManager.GetPlayerController();
			if ((float)this.m_assignedAbility.ActualCost < playerController.CurrentMana && !this.m_assignedAbility.IsOnCooldown)
			{
				this.m_abilityReadyGO.SetActive(true);
			}
		}
	}

	// Token: 0x06002450 RID: 9296 RVA: 0x000789DA File Offset: 0x00076BDA
	private void OnChangeAbilityHandler(MonoBehaviour sender, EventArgs eventArgs)
	{
		this.UpdateAssignedAbility();
		this.InitializeAbilityReadyGO();
		this.UpdateDisarmedState();
	}

	// Token: 0x06002451 RID: 9297 RVA: 0x000789EE File Offset: 0x00076BEE
	private void OnUpdateAbilityDisarmStateHandler(MonoBehaviour sender, EventArgs args)
	{
		this.UpdateDisarmedState();
	}

	// Token: 0x06002452 RID: 9298 RVA: 0x000789F6 File Offset: 0x00076BF6
	private IEnumerator Start()
	{
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		this.m_animator = this.m_abilityReadyGO.GetComponent<Animator>();
		this.UpdateAssignedAbility();
		this.m_abilityReadyGO.SetActive(false);
		this.InitializeAbilityReadyGO();
		this.UpdateDisarmedState();
		yield break;
	}

	// Token: 0x06002453 RID: 9299 RVA: 0x00078A08 File Offset: 0x00076C08
	private void UpdateAssignedAbility()
	{
		if (!PlayerManager.IsInstantiated)
		{
			return;
		}
		if (this.m_assignedAbility)
		{
			this.m_assignedAbility.OnBeginCooldownRelay.RemoveListener(this.m_beginCooldown);
		}
		BaseAbility_RL ability = PlayerManager.GetPlayerController().CastAbility.GetAbility(this.m_castAbilityType, false);
		if (ability)
		{
			this.m_assignedAbility = ability;
			this.m_tempTitleText.text = AbilityLibrary.GetAbility(ability.AbilityType).AbilityData.Name;
			ability.OnBeginCooldownRelay.AddListener(this.m_beginCooldown, false);
			this.m_cooldownImage.enabled = false;
			this.m_cooldownText.gameObject.SetActive(false);
			this.m_cooldownIcon.sprite = IconLibrary.GetAbilityCooldownIcon(ability.CooldownRegenType, true);
			this.m_cooldownIcon.gameObject.SetActive(false);
			int actualCost = ability.ActualCost;
			if (actualCost > 0)
			{
				this.m_manaCostText.gameObject.SetActive(true);
				this.m_manaCostText.text = actualCost.ToString();
			}
			else
			{
				this.m_manaCostText.gameObject.SetActive(false);
			}
			Sprite sprite = null;
			switch (this.m_castAbilityType)
			{
			case CastAbilityType.Weapon:
				sprite = IconLibrary.GetAbilityIcon(ability.AbilityType, true);
				this.m_inputText.text = "[Attack]";
				break;
			case CastAbilityType.Spell:
				sprite = IconLibrary.GetAbilityIcon(ability.AbilityType, true);
				this.m_inputText.text = "[Spell]";
				break;
			case CastAbilityType.Talent:
				sprite = IconLibrary.GetAbilityIcon(ability.AbilityType, true);
				this.m_inputText.text = "[Talent]";
				break;
			}
			if (sprite)
			{
				this.m_tempTitleText.gameObject.SetActive(false);
				this.m_icon.gameObject.SetActive(true);
				this.m_icon.sprite = sprite;
			}
			else
			{
				this.m_tempTitleText.gameObject.SetActive(true);
				this.m_icon.gameObject.SetActive(false);
			}
			this.UpdateAmmo(ability);
			this.OnManaChangeHandler(null, null);
			this.BeginCooldown(null, null);
			return;
		}
		this.m_tempTitleText.text = "NULL";
	}

	// Token: 0x06002454 RID: 9300 RVA: 0x00078C20 File Offset: 0x00076E20
	private void OnAmmoChangeHandler(object sender, EventArgs args)
	{
		IAbility abilityObj = (args as PlayerAmmoChangeEventArgs).AbilityObj;
		if (abilityObj != null && abilityObj.CastAbilityType == this.m_castAbilityType)
		{
			this.UpdateAmmo(abilityObj);
		}
	}

	// Token: 0x06002455 RID: 9301 RVA: 0x00078C54 File Offset: 0x00076E54
	private void OnManaChangeHandler(object sender, EventArgs args)
	{
		if (this.m_assignedAbility && this.m_assignedAbility.ActualCost > 0)
		{
			if (PlayerManager.GetPlayerController().CurrentMana < (float)this.m_assignedAbility.ActualCost)
			{
				this.m_abilityReadyGO.SetActive(false);
				this.m_cooldownImage.enabled = true;
				return;
			}
			if (!this.m_abilityReadyGO.activeSelf)
			{
				this.m_abilityReadyGO.SetActive(true);
				this.m_animator.Play("Intro");
			}
			this.m_cooldownImage.enabled = false;
		}
	}

	// Token: 0x06002456 RID: 9302 RVA: 0x00078CE4 File Offset: 0x00076EE4
	private void UpdateAmmo(IAbility ability)
	{
		this.m_ammoText.gameObject.SetActive(false);
		if (ability.MaxAmmo > 0)
		{
			if (!this.m_abilityReadyGO.activeSelf)
			{
				this.m_abilityReadyGO.SetActive(true);
			}
			this.m_ammoText.gameObject.SetActive(true);
			this.m_ammoText.color = Color.green;
			this.m_ammoText.text = ability.CurrentAmmo.ToString();
			if (ability.CurrentAmmo == ability.MaxAmmo)
			{
				this.m_ammoText.color = Color.green;
				return;
			}
			if (ability.CurrentAmmo == 0)
			{
				this.m_ammoText.color = Color.red;
				this.m_abilityReadyGO.SetActive(false);
			}
		}
	}

	// Token: 0x06002457 RID: 9303 RVA: 0x00078DA4 File Offset: 0x00076FA4
	private void BeginCooldown(object sender, CooldownEventArgs eventArgs)
	{
		if (base.gameObject.activeInHierarchy)
		{
			base.StopAllCoroutines();
			base.StartCoroutine(this.CooldownCoroutine());
		}
	}

	// Token: 0x06002458 RID: 9304 RVA: 0x00078DC6 File Offset: 0x00076FC6
	private IEnumerator CooldownCoroutine()
	{
		if (this.m_assignedAbility.ActualCooldownTime > 0f && (this.m_assignedAbility.MaxAmmo == 0 || (this.m_assignedAbility.MaxAmmo != 0 && this.m_assignedAbility.CurrentAmmo == 0)))
		{
			this.m_manaCostText.gameObject.SetActive(false);
			if (this.m_assignedAbility.MaxAmmo > 0)
			{
				this.m_ammoText.gameObject.SetActive(false);
			}
			this.m_abilityReadyGO.SetActive(false);
			this.m_cooldownImage.enabled = true;
			if (!this.m_cooldownText.gameObject.activeSelf)
			{
				this.m_cooldownText.gameObject.SetActive(true);
			}
			this.m_cooldownIcon.gameObject.SetActive(true);
			this.m_trackedValue = 0;
			bool flag = this.m_assignedAbility.IsOnCooldown && (this.m_assignedAbility.MaxAmmo == 0 || (this.m_assignedAbility.MaxAmmo > 0 && this.m_assignedAbility.CurrentAmmo == 0));
			if (!flag)
			{
				this.UpdateTrackedValue();
			}
			while (flag)
			{
				if (this.UpdateTrackedValue())
				{
					if (this.m_cdChangeTween != null && this.m_cdChangeTween.isActiveAndEnabled)
					{
						this.m_cdChangeTween.StopTweenWithConditionChecks(false, this.m_cooldownIcon.gameObject.transform, null);
					}
					this.m_cooldownIcon.gameObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
					this.m_cdChangeTween = TweenManager.TweenTo(this.m_cooldownIcon.gameObject.transform, 0.25f, new EaseDelegate(Ease.Back.EaseOutLarge), new object[]
					{
						"localScale.x",
						1,
						"localScale.y",
						1,
						"localScale.z",
						1
					});
				}
				yield return null;
				flag = (this.m_assignedAbility.IsOnCooldown && (this.m_assignedAbility.MaxAmmo == 0 || (this.m_assignedAbility.MaxAmmo > 0 && this.m_assignedAbility.CurrentAmmo == 0)));
			}
			int actualCost = this.m_assignedAbility.ActualCost;
			if (actualCost > 0)
			{
				this.m_manaCostText.gameObject.SetActive(true);
				this.m_manaCostText.text = actualCost.ToString();
			}
			else
			{
				this.m_manaCostText.gameObject.SetActive(false);
			}
			if (this.m_assignedAbility.MaxAmmo > 0)
			{
				this.m_ammoText.gameObject.SetActive(true);
			}
			this.m_cooldownText.gameObject.SetActive(false);
			this.m_cooldownIcon.gameObject.SetActive(false);
			if (this.m_assignedAbility.MaxAmmo > 0 || this.m_assignedAbility.ActualCost <= 0 || (this.m_assignedAbility.ActualCost > 0 && (float)this.m_assignedAbility.ActualCost <= PlayerManager.GetPlayerController().CurrentMana))
			{
				this.m_cooldownImage.enabled = false;
				this.m_abilityReadyGO.SetActive(true);
				this.m_animator.Play("Intro");
			}
		}
		yield break;
	}

	// Token: 0x06002459 RID: 9305 RVA: 0x00078DD8 File Offset: 0x00076FD8
	private bool UpdateTrackedValue()
	{
		this.m_valueStringBuilder.Clear();
		if (this.m_assignedAbility.DecreaseCooldownOverTime)
		{
			this.m_trackedValue = (int)this.m_assignedAbility.CooldownTimer + 1;
		}
		else
		{
			this.m_trackedValue = (int)this.m_assignedAbility.CooldownTimer;
		}
		this.m_valueStringBuilder.Append(this.m_trackedValue);
		if (this.m_assignedAbility.DecreaseCooldownWhenHit && CDGHelper.IsPercent(this.m_assignedAbility.CooldownTimer))
		{
			float num = (this.m_assignedAbility.CooldownTimer - (float)this.m_trackedValue) * 100f;
			if (num > 0f)
			{
				this.m_valueStringBuilder.AppendFormat("<size=60%><voffset=20>.{0:D2}</voffset></size>", (int)num);
			}
		}
		bool result = false;
		if (this.m_assignedAbility.DisplayPausedAbilityCooldown)
		{
			if (!this.m_displayingNullCooldownString)
			{
				this.m_cooldownText.text = "--";
				this.m_displayingNullCooldownString = true;
				this.m_prevValueStringBuilder.Clear();
				result = true;
			}
		}
		else
		{
			this.m_displayingNullCooldownString = false;
			if (!this.m_prevValueStringBuilder.Equals(this.m_valueStringBuilder))
			{
				this.m_cooldownText.SetText(this.m_valueStringBuilder);
				this.m_prevValueStringBuilder.Clear();
				this.m_prevValueStringBuilder.Append(this.m_valueStringBuilder);
				result = true;
			}
		}
		return result;
	}

	// Token: 0x0600245A RID: 9306 RVA: 0x00078F1C File Offset: 0x0007711C
	private void UpdateDisarmedState()
	{
		if (!PlayerManager.IsInstantiated)
		{
			return;
		}
		if (this.m_assignedAbility)
		{
			if (PlayerManager.GetPlayerController().StatusEffectController.HasStatusEffect(StatusEffectType.Player_Disarmed) || (TraitManager.IsTraitActive(TraitType.CantAttack) && !this.m_assignedAbility.DealsNoDamage))
			{
				if (!this.m_disarmedGO.activeSelf)
				{
					this.m_disarmedGO.SetActive(true);
					this.m_disarmedGO.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
					TweenManager.TweenTo(this.m_disarmedGO.transform, 0.15f, new EaseDelegate(Ease.Back.EaseInLarge), new object[]
					{
						"localScale.x",
						1,
						"localScale.y",
						1
					});
					return;
				}
			}
			else
			{
				this.m_disarmedGO.SetActive(false);
			}
		}
	}

	// Token: 0x04001EDE RID: 7902
	[SerializeField]
	private TMP_Text m_tempTitleText;

	// Token: 0x04001EDF RID: 7903
	[SerializeField]
	private Image m_cooldownImage;

	// Token: 0x04001EE0 RID: 7904
	[SerializeField]
	private TMP_Text m_cooldownText;

	// Token: 0x04001EE1 RID: 7905
	[SerializeField]
	private CanvasGroup m_canvasGroup;

	// Token: 0x04001EE2 RID: 7906
	[SerializeField]
	private CastAbilityType m_castAbilityType;

	// Token: 0x04001EE3 RID: 7907
	[SerializeField]
	private TMP_Text m_ammoText;

	// Token: 0x04001EE4 RID: 7908
	[SerializeField]
	private Image m_icon;

	// Token: 0x04001EE5 RID: 7909
	[SerializeField]
	private TMP_Text m_inputText;

	// Token: 0x04001EE6 RID: 7910
	[SerializeField]
	private TMP_Text m_manaCostText;

	// Token: 0x04001EE7 RID: 7911
	[SerializeField]
	private GameObject m_abilityReadyGO;

	// Token: 0x04001EE8 RID: 7912
	[SerializeField]
	private Image m_cooldownIcon;

	// Token: 0x04001EE9 RID: 7913
	[SerializeField]
	private GameObject m_disarmedGO;

	// Token: 0x04001EEA RID: 7914
	private const string DECIMAL_COUNT_VALUE_STRING = "<size=60%><voffset=20>.{0:D2}</voffset></size>";

	// Token: 0x04001EEB RID: 7915
	private const string NULL_COOLDOWN_STRING = "--";

	// Token: 0x04001EEC RID: 7916
	private BaseAbility_RL m_assignedAbility;

	// Token: 0x04001EED RID: 7917
	private Animator m_animator;

	// Token: 0x04001EEE RID: 7918
	private Tween m_cdChangeTween;

	// Token: 0x04001EEF RID: 7919
	private int m_trackedValue;

	// Token: 0x04001EF0 RID: 7920
	private StringBuilder m_valueStringBuilder;

	// Token: 0x04001EF1 RID: 7921
	private StringBuilder m_prevValueStringBuilder;

	// Token: 0x04001EF2 RID: 7922
	private bool m_displayingNullCooldownString;

	// Token: 0x04001EF3 RID: 7923
	private Action<MonoBehaviour, EventArgs> m_onChangeAbilityHandler;

	// Token: 0x04001EF4 RID: 7924
	private Action<MonoBehaviour, EventArgs> m_onAmmoChangeHandler;

	// Token: 0x04001EF5 RID: 7925
	private Action<MonoBehaviour, EventArgs> m_onManaChangeHandler;

	// Token: 0x04001EF6 RID: 7926
	private Action<MonoBehaviour, EventArgs> m_onUpdateAbilityDisarmStateHandler;

	// Token: 0x04001EF7 RID: 7927
	private Action<object, CooldownEventArgs> m_beginCooldown;
}
