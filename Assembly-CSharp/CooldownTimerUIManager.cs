using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001DC RID: 476
public class CooldownTimerUIManager : MonoBehaviour
{
	// Token: 0x060013B3 RID: 5043 RVA: 0x0003BDFE File Offset: 0x00039FFE
	private void Awake()
	{
		this.m_onChangeAbility = new Action<MonoBehaviour, EventArgs>(this.OnChangeAbility);
	}

	// Token: 0x060013B4 RID: 5044 RVA: 0x0003BE12 File Offset: 0x0003A012
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

	// Token: 0x060013B5 RID: 5045 RVA: 0x0003BE21 File Offset: 0x0003A021
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.ChangeAbility, this.m_onChangeAbility);
	}

	// Token: 0x060013B6 RID: 5046 RVA: 0x0003BE30 File Offset: 0x0003A030
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.ChangeAbility, this.m_onChangeAbility);
	}

	// Token: 0x060013B7 RID: 5047 RVA: 0x0003BE3F File Offset: 0x0003A03F
	private void OnChangeAbility(MonoBehaviour sender, EventArgs eventArgs)
	{
		this.InjectCooldown();
	}

	// Token: 0x060013B8 RID: 5048 RVA: 0x0003BE48 File Offset: 0x0003A048
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

	// Token: 0x04001390 RID: 5008
	[SerializeField]
	private CooldownTimerUI m_attackTimer;

	// Token: 0x04001391 RID: 5009
	[SerializeField]
	private CooldownTimerUI m_spellTimer;

	// Token: 0x04001392 RID: 5010
	[SerializeField]
	private CooldownTimerUI m_talentTimer;

	// Token: 0x04001393 RID: 5011
	private Action<MonoBehaviour, EventArgs> m_onChangeAbility;
}
