using System;
using UnityEngine;

// Token: 0x0200016A RID: 362
[RequireComponent(typeof(BaseAbility_RL))]
public class ReduceCDOnItemDropAbilityMod : MonoBehaviour
{
	// Token: 0x06000C88 RID: 3208 RVA: 0x00026C74 File Offset: 0x00024E74
	private void Awake()
	{
		this.m_ability = base.GetComponent<BaseAbility_RL>();
		this.m_onItemCollected = new Action<MonoBehaviour, EventArgs>(this.OnItemCollected);
	}

	// Token: 0x06000C89 RID: 3209 RVA: 0x00026C94 File Offset: 0x00024E94
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.ItemCollected, this.m_onItemCollected);
	}

	// Token: 0x06000C8A RID: 3210 RVA: 0x00026CA3 File Offset: 0x00024EA3
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.ItemCollected, this.m_onItemCollected);
	}

	// Token: 0x06000C8B RID: 3211 RVA: 0x00026CB4 File Offset: 0x00024EB4
	private void OnItemCollected(object sender, EventArgs args)
	{
		ItemCollectedEventArgs itemCollectedEventArgs = args as ItemCollectedEventArgs;
		bool flag = false;
		ItemDropType itemDropType = itemCollectedEventArgs.Item.ItemDropType;
		if (itemDropType <= ItemDropType.Coin)
		{
			if (itemDropType <= ItemDropType.Diamond)
			{
				if (itemDropType != ItemDropType.MoneyBag && itemDropType != ItemDropType.Diamond)
				{
					goto IL_CE;
				}
			}
			else if (itemDropType != ItemDropType.Crystal && itemDropType != ItemDropType.Coin)
			{
				goto IL_CE;
			}
			if (this.m_triggerOnCoins)
			{
				flag = true;
			}
		}
		else if (itemDropType <= ItemDropType.EquipmentOre)
		{
			if (itemDropType != ItemDropType.HealthDrop)
			{
				switch (itemDropType)
				{
				case ItemDropType.ManaDrop:
				case ItemDropType.MilkManaDrop:
					if (this.m_triggerOnManaDrops)
					{
						flag = true;
						goto IL_CE;
					}
					goto IL_CE;
				case (ItemDropType)62:
				case (ItemDropType)63:
				case (ItemDropType)64:
				case (ItemDropType)69:
					goto IL_CE;
				case ItemDropType.PizzaDrop:
				case ItemDropType.MushroomDrop:
				case ItemDropType.CandyDrop:
				case ItemDropType.CookieDrop:
					break;
				case ItemDropType.EquipmentOre:
					if (this.m_triggerOnEquipmentOre)
					{
						flag = true;
						goto IL_CE;
					}
					goto IL_CE;
				default:
					goto IL_CE;
				}
			}
			if (this.m_triggerOnHealthDrops)
			{
				flag = true;
			}
		}
		else if (itemDropType != ItemDropType.RuneOre)
		{
			if (itemDropType == ItemDropType.Soul)
			{
				if (this.m_triggerOnSoul)
				{
					flag = true;
				}
			}
		}
		else if (this.m_triggerOnRuneOre)
		{
			flag = true;
		}
		IL_CE:
		if (flag)
		{
			if (this.m_isFlat)
			{
				this.m_ability.ReduceFlatCooldown((int)this.m_cooldownReductionAmount);
				return;
			}
			this.m_ability.ReduceCooldown(this.m_ability.ActualCooldownTime * this.m_cooldownReductionAmount);
		}
	}

	// Token: 0x0400108E RID: 4238
	[SerializeField]
	private float m_cooldownReductionAmount;

	// Token: 0x0400108F RID: 4239
	[SerializeField]
	private bool m_isFlat;

	// Token: 0x04001090 RID: 4240
	[SerializeField]
	private bool m_triggerOnCoins;

	// Token: 0x04001091 RID: 4241
	[SerializeField]
	private bool m_triggerOnEquipmentOre;

	// Token: 0x04001092 RID: 4242
	[SerializeField]
	private bool m_triggerOnRuneOre;

	// Token: 0x04001093 RID: 4243
	[SerializeField]
	private bool m_triggerOnHealthDrops;

	// Token: 0x04001094 RID: 4244
	[SerializeField]
	private bool m_triggerOnManaDrops;

	// Token: 0x04001095 RID: 4245
	[SerializeField]
	private bool m_triggerOnSoul;

	// Token: 0x04001096 RID: 4246
	private BaseAbility_RL m_ability;

	// Token: 0x04001097 RID: 4247
	private Action<MonoBehaviour, EventArgs> m_onItemCollected;
}
