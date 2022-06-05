using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020005F4 RID: 1524
public class CooldownIndicatorAbilityName : MonoBehaviour
{
	// Token: 0x06002EFB RID: 12027 RVA: 0x00019B77 File Offset: 0x00017D77
	private void Awake()
	{
		this.m_onChangeAbility = new Action<MonoBehaviour, EventArgs>(this.OnChangeAbility);
	}

	// Token: 0x06002EFC RID: 12028 RVA: 0x00019B8B File Offset: 0x00017D8B
	private void Start()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.ChangeAbility, this.m_onChangeAbility);
		this.m_text.text = "NONE";
	}

	// Token: 0x06002EFD RID: 12029 RVA: 0x00019BAA File Offset: 0x00017DAA
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.ChangeAbility, this.m_onChangeAbility);
	}

	// Token: 0x06002EFE RID: 12030 RVA: 0x000C8F64 File Offset: 0x000C7164
	private void OnChangeAbility(MonoBehaviour sender, EventArgs eventArgs)
	{
		ChangeAbilityEventArgs changeAbilityEventArgs = eventArgs as ChangeAbilityEventArgs;
		if (changeAbilityEventArgs != null && changeAbilityEventArgs.CastAbilityType == this.m_abilityCategory)
		{
			this.m_text.text = changeAbilityEventArgs.Ability.AbilityType.ToString();
		}
	}

	// Token: 0x04002661 RID: 9825
	[SerializeField]
	private CastAbilityType m_abilityCategory;

	// Token: 0x04002662 RID: 9826
	[SerializeField]
	private Text m_text;

	// Token: 0x04002663 RID: 9827
	private Action<MonoBehaviour, EventArgs> m_onChangeAbility;
}
