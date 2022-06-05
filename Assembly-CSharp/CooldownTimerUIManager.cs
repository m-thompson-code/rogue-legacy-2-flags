using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000360 RID: 864
public class CooldownTimerUIManager : MonoBehaviour
{
	// Token: 0x06001C54 RID: 7252 RVA: 0x0000EA8D File Offset: 0x0000CC8D
	private void Awake()
	{
		this.m_onChangeAbility = new Action<MonoBehaviour, EventArgs>(this.OnChangeAbility);
	}

	// Token: 0x06001C55 RID: 7253 RVA: 0x0000EAA1 File Offset: 0x0000CCA1
	private IEnumerator Start()
	{
		Debug.LogWarningFormat("<color=blue>{0}: [{1}] is waiting for Player to be instantiated and for Character Class Component to be initialised...</color>", new object[]
		{
			Time.frameCount,
			this
		});
		yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		yield return new WaitUntil(() => PlayerManager.GetPlayerController().CharacterClass.IsInitialized);
		Debug.LogWarningFormat("<color=green>{0}: [{1}] finished waiting for Player to be instantiated and for Character Class Component to be initialised.</color>", new object[]
		{
			Time.frameCount,
			this
		});
		yield break;
	}

	// Token: 0x06001C56 RID: 7254 RVA: 0x0000EAB0 File Offset: 0x0000CCB0
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.ChangeAbility, this.m_onChangeAbility);
	}

	// Token: 0x06001C57 RID: 7255 RVA: 0x0000EABF File Offset: 0x0000CCBF
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.ChangeAbility, this.m_onChangeAbility);
	}

	// Token: 0x06001C58 RID: 7256 RVA: 0x0000EACE File Offset: 0x0000CCCE
	private void OnChangeAbility(MonoBehaviour sender, EventArgs eventArgs)
	{
		this.InjectCooldown();
	}

	// Token: 0x06001C59 RID: 7257 RVA: 0x00098EC8 File Offset: 0x000970C8
	public void InjectCooldown()
	{
		CastAbility_RL component = PlayerManager.GetPlayer().GetComponent<CastAbility_RL>();
		if (component == null)
		{
			throw new MissingComponentException("CastAbility_RL");
		}
		BaseAbility_RL ability = component.GetAbility(CastAbilityType.Weapon, false);
		if (ability == null)
		{
			Debug.LogWarningFormat("{0}: Unable to find Weapon Ability Type", new object[]
			{
				Time.frameCount
			});
		}
		this.m_attackTimer.Cooldown = ability;
		ability = component.GetAbility(CastAbilityType.Spell, false);
		if (ability == null)
		{
			Debug.LogWarningFormat("{0}: Unable to find Spell Ability Type", new object[]
			{
				Time.frameCount
			});
		}
		this.m_spellTimer.Cooldown = ability;
		ability = component.GetAbility(CastAbilityType.Talent, false);
		if (ability == null)
		{
			Debug.LogWarningFormat("<color=red>{0}: ({1}) Unable to find Talent Ability Type</color>", new object[]
			{
				Time.frameCount,
				this
			});
		}
		this.m_talentTimer.Cooldown = ability;
	}

	// Token: 0x040019E4 RID: 6628
	[SerializeField]
	private CooldownTimerUI m_attackTimer;

	// Token: 0x040019E5 RID: 6629
	[SerializeField]
	private CooldownTimerUI m_spellTimer;

	// Token: 0x040019E6 RID: 6630
	[SerializeField]
	private CooldownTimerUI m_talentTimer;

	// Token: 0x040019E7 RID: 6631
	private Action<MonoBehaviour, EventArgs> m_onChangeAbility;
}
