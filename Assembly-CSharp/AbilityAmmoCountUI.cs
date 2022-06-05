using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001AC RID: 428
public class AbilityAmmoCountUI : MonoBehaviour
{
	// Token: 0x06001108 RID: 4360 RVA: 0x0003118B File Offset: 0x0002F38B
	private void Awake()
	{
		this.m_onAbilityCooldownOver = new Action<MonoBehaviour, EventArgs>(this.OnAbilityCooldownOver);
		this.m_onAbilityUsed = new Action<MonoBehaviour, EventArgs>(this.OnAbilityUsed);
	}

	// Token: 0x06001109 RID: 4361 RVA: 0x000311B1 File Offset: 0x0002F3B1
	private IEnumerator Start()
	{
		Debug.LogWarningFormat("{0}: ({1}) is waiting for Player to be instantiated and for Character Class Component to be initialised...", new object[]
		{
			Time.frameCount,
			this
		});
		this.m_ammoCountText.text = "-";
		while (!PlayerManager.IsInstantiated || !PlayerManager.GetPlayerController().CharacterClass.IsInitialized)
		{
			yield return null;
		}
		Debug.LogWarningFormat("{0}: ({1}) finished waiting for Player to be instantiated and for Character Class Component to be initialised.", new object[]
		{
			Time.frameCount,
			this
		});
		if (PlayerManager.GetPlayerController().CastAbility != null)
		{
			IAbility ability = PlayerManager.GetPlayerController().CastAbility.GetAbility(this.m_abilityType, false);
			if (ability != null)
			{
				if (ability.MaxAmmo > 0)
				{
					this.m_ammoCountText.text = ability.AbilityData.MaxAmmo.ToString();
				}
				else
				{
					this.m_ammoCountText.enabled = false;
				}
			}
			else
			{
				Debug.LogFormat("<color=red>|{0}| Failed to get Ability Type ({1})</color>", new object[]
				{
					this,
					this.m_abilityType
				});
				this.m_ammoCountText.text = "-";
			}
		}
		else
		{
			Debug.LogFormat("<color=red>|{0}| Character Abilities is null</color>", new object[]
			{
				this
			});
		}
		switch (this.m_abilityType)
		{
		case CastAbilityType.Weapon:
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerWeaponAbilityCast, this.m_onAbilityUsed);
			break;
		case CastAbilityType.Spell:
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerSpellAbilityCast, this.m_onAbilityUsed);
			break;
		case CastAbilityType.Talent:
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerTalentAbilityCast, this.m_onAbilityUsed);
			break;
		default:
			throw new ArgumentException("m_abilityType");
		}
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.AbilityCooldownOver, this.m_onAbilityCooldownOver);
		yield break;
	}

	// Token: 0x0600110A RID: 4362 RVA: 0x000311C0 File Offset: 0x0002F3C0
	private void OnDestroy()
	{
		switch (this.m_abilityType)
		{
		case CastAbilityType.Weapon:
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerWeaponAbilityCast, this.m_onAbilityUsed);
			break;
		case CastAbilityType.Spell:
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerSpellAbilityCast, this.m_onAbilityUsed);
			break;
		case CastAbilityType.Talent:
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerTalentAbilityCast, this.m_onAbilityUsed);
			break;
		default:
			throw new ArgumentException("m_abilityType");
		}
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.AbilityCooldownOver, this.m_onAbilityCooldownOver);
	}

	// Token: 0x0600110B RID: 4363 RVA: 0x00031230 File Offset: 0x0002F430
	private void OnAbilityCooldownOver(MonoBehaviour sender, EventArgs eventArgs)
	{
		AbilityCooldownOverEventArgs abilityCooldownOverEventArgs = eventArgs as AbilityCooldownOverEventArgs;
		if (abilityCooldownOverEventArgs == null)
		{
			Debug.LogFormat("<color=red>{0}: Unable to cast EventArgs as AbilityUsedEventArgs</color>", new object[]
			{
				Time.frameCount
			});
			return;
		}
		if (abilityCooldownOverEventArgs.Ability != null)
		{
			this.m_ammoCountText.text = abilityCooldownOverEventArgs.Ability.CurrentAmmo.ToString();
			return;
		}
		Debug.LogFormat("{0}: args.Ability is null", new object[]
		{
			Time.frameCount
		});
	}

	// Token: 0x0600110C RID: 4364 RVA: 0x000312AC File Offset: 0x0002F4AC
	private void OnAbilityUsed(MonoBehaviour sender, EventArgs eventArgs)
	{
		AbilityUsedEventArgs abilityUsedEventArgs = eventArgs as AbilityUsedEventArgs;
		if (abilityUsedEventArgs == null)
		{
			Debug.LogFormat("<color=red>{0}: Unable to cast EventArgs as AbilityUsedEventArgs</color>", new object[]
			{
				Time.frameCount
			});
			return;
		}
		if (abilityUsedEventArgs.Ability != null)
		{
			this.m_ammoCountText.text = abilityUsedEventArgs.Ability.CurrentAmmo.ToString();
			return;
		}
		Debug.LogFormat("{0}: args.Ability is null", new object[]
		{
			Time.frameCount
		});
	}

	// Token: 0x040011FB RID: 4603
	[SerializeField]
	private CastAbilityType m_abilityType;

	// Token: 0x040011FC RID: 4604
	[SerializeField]
	private Text m_ammoCountText;

	// Token: 0x040011FD RID: 4605
	private Action<MonoBehaviour, EventArgs> m_onAbilityUsed;

	// Token: 0x040011FE RID: 4606
	private Action<MonoBehaviour, EventArgs> m_onAbilityCooldownOver;
}
