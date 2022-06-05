using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000373 RID: 883
public class CooldownIndicatorAbilityName : MonoBehaviour
{
	// Token: 0x06002126 RID: 8486 RVA: 0x0006818A File Offset: 0x0006638A
	private void Awake()
	{
		this.m_onChangeAbility = new Action<MonoBehaviour, EventArgs>(this.OnChangeAbility);
	}

	// Token: 0x06002127 RID: 8487 RVA: 0x0006819E File Offset: 0x0006639E
	private void Start()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.ChangeAbility, this.m_onChangeAbility);
		this.m_text.text = "NONE";
	}

	// Token: 0x06002128 RID: 8488 RVA: 0x000681BD File Offset: 0x000663BD
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.ChangeAbility, this.m_onChangeAbility);
	}

	// Token: 0x06002129 RID: 8489 RVA: 0x000681CC File Offset: 0x000663CC
	private void OnChangeAbility(MonoBehaviour sender, EventArgs eventArgs)
	{
		ChangeAbilityEventArgs changeAbilityEventArgs = eventArgs as ChangeAbilityEventArgs;
		if (changeAbilityEventArgs != null && changeAbilityEventArgs.CastAbilityType == this.m_abilityCategory)
		{
			this.m_text.text = changeAbilityEventArgs.Ability.AbilityType.ToString();
		}
	}

	// Token: 0x04001CA6 RID: 7334
	[SerializeField]
	private CastAbilityType m_abilityCategory;

	// Token: 0x04001CA7 RID: 7335
	[SerializeField]
	private Text m_text;

	// Token: 0x04001CA8 RID: 7336
	private Action<MonoBehaviour, EventArgs> m_onChangeAbility;
}
