using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000678 RID: 1656
public class PlayerHUD_Ability : MonoBehaviour
{
	// Token: 0x0600327C RID: 12924 RVA: 0x000D8930 File Offset: 0x000D6B30
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

	// Token: 0x0600327D RID: 12925 RVA: 0x000D89F0 File Offset: 0x000D6BF0
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.ChangeAbility, this.m_onChangeAbilityHandler);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.UpdateAbilityHUD, this.m_onChangeAbilityHandler);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerAmmoChange, this.m_onAmmoChangeHandler);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerManaChange, this.m_onManaChangeHandler);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.UpdateAbilityDisarmState, this.m_onUpdateAbilityDisarmStateHandler);
	}

	// Token: 0x0600327E RID: 12926 RVA: 0x0001BA87 File Offset: 0x00019C87
	private void OnEnable()
	{
		this.m_abilityReadyGO.SetActive(false);
		this.InitializeAbilityReadyGO();
	}

	// Token: 0x0600327F RID: 12927 RVA: 0x000D8A40 File Offset: 0x000D6C40
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

	// Token: 0x06003280 RID: 12928 RVA: 0x0001BA9B File Offset: 0x00019C9B
	private void OnChangeAbilityHandler(MonoBehaviour sender, EventArgs eventArgs)
	{
		this.UpdateAssignedAbility();
		this.InitializeAbilityReadyGO();
		this.UpdateDisarmedState();
	}

	// Token: 0x06003281 RID: 12929 RVA: 0x0001BAAF File Offset: 0x00019CAF
	private void OnUpdateAbilityDisarmStateHandler(MonoBehaviour sender, EventArgs args)
	{
		this.UpdateDisarmedState();
	}

	// Token: 0x06003282 RID: 12930 RVA: 0x0001BAB7 File Offset: 0x00019CB7
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

	// Token: 0x06003283 RID: 12931 RVA: 0x000D8AB0 File Offset: 0x000D6CB0
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

	// Token: 0x06003284 RID: 12932 RVA: 0x000D8CC8 File Offset: 0x000D6EC8
	private void OnAmmoChangeHandler(object sender, EventArgs args)
	{
		IAbility abilityObj = (args as PlayerAmmoChangeEventArgs).AbilityObj;
		if (abilityObj != null && abilityObj.CastAbilityType == this.m_castAbilityType)
		{
			this.UpdateAmmo(abilityObj);
		}
	}

	// Token: 0x06003285 RID: 12933 RVA: 0x000D8CFC File Offset: 0x000D6EFC
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

	// Token: 0x06003286 RID: 12934 RVA: 0x000D8D8C File Offset: 0x000D6F8C
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

	// Token: 0x06003287 RID: 12935 RVA: 0x0001BAC6 File Offset: 0x00019CC6
	private void BeginCooldown(object sender, CooldownEventArgs eventArgs)
	{
		if (base.gameObject.activeInHierarchy)
		{
			base.StopAllCoroutines();
			base.StartCoroutine(this.CooldownCoroutine());
		}
	}

	// Token: 0x06003288 RID: 12936 RVA: 0x0001BAE8 File Offset: 0x00019CE8
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

	// Token: 0x06003289 RID: 12937 RVA: 0x000D8E4C File Offset: 0x000D704C
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

	// Token: 0x0600328A RID: 12938 RVA: 0x000D8F90 File Offset: 0x000D7190
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

	// Token: 0x04002950 RID: 10576
	[SerializeField]
	private TMP_Text m_tempTitleText;

	// Token: 0x04002951 RID: 10577
	[SerializeField]
	private Image m_cooldownImage;

	// Token: 0x04002952 RID: 10578
	[SerializeField]
	private TMP_Text m_cooldownText;

	// Token: 0x04002953 RID: 10579
	[SerializeField]
	private CanvasGroup m_canvasGroup;

	// Token: 0x04002954 RID: 10580
	[SerializeField]
	private CastAbilityType m_castAbilityType;

	// Token: 0x04002955 RID: 10581
	[SerializeField]
	private TMP_Text m_ammoText;

	// Token: 0x04002956 RID: 10582
	[SerializeField]
	private Image m_icon;

	// Token: 0x04002957 RID: 10583
	[SerializeField]
	private TMP_Text m_inputText;

	// Token: 0x04002958 RID: 10584
	[SerializeField]
	private TMP_Text m_manaCostText;

	// Token: 0x04002959 RID: 10585
	[SerializeField]
	private GameObject m_abilityReadyGO;

	// Token: 0x0400295A RID: 10586
	[SerializeField]
	private Image m_cooldownIcon;

	// Token: 0x0400295B RID: 10587
	[SerializeField]
	private GameObject m_disarmedGO;

	// Token: 0x0400295C RID: 10588
	private const string DECIMAL_COUNT_VALUE_STRING = "<size=60%><voffset=20>.{0:D2}</voffset></size>";

	// Token: 0x0400295D RID: 10589
	private const string NULL_COOLDOWN_STRING = "--";

	// Token: 0x0400295E RID: 10590
	private BaseAbility_RL m_assignedAbility;

	// Token: 0x0400295F RID: 10591
	private Animator m_animator;

	// Token: 0x04002960 RID: 10592
	private Tween m_cdChangeTween;

	// Token: 0x04002961 RID: 10593
	private int m_trackedValue;

	// Token: 0x04002962 RID: 10594
	private StringBuilder m_valueStringBuilder;

	// Token: 0x04002963 RID: 10595
	private StringBuilder m_prevValueStringBuilder;

	// Token: 0x04002964 RID: 10596
	private bool m_displayingNullCooldownString;

	// Token: 0x04002965 RID: 10597
	private Action<MonoBehaviour, EventArgs> m_onChangeAbilityHandler;

	// Token: 0x04002966 RID: 10598
	private Action<MonoBehaviour, EventArgs> m_onAmmoChangeHandler;

	// Token: 0x04002967 RID: 10599
	private Action<MonoBehaviour, EventArgs> m_onManaChangeHandler;

	// Token: 0x04002968 RID: 10600
	private Action<MonoBehaviour, EventArgs> m_onUpdateAbilityDisarmStateHandler;

	// Token: 0x04002969 RID: 10601
	private Action<object, CooldownEventArgs> m_beginCooldown;
}
