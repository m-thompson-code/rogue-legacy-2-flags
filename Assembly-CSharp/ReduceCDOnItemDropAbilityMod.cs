using System;
using UnityEngine;

// Token: 0x020002A0 RID: 672
[RequireComponent(typeof(BaseAbility_RL))]
public class ReduceCDOnItemDropAbilityMod : MonoBehaviour
{
	// Token: 0x0600139B RID: 5019 RVA: 0x00009FA2 File Offset: 0x000081A2
	private void Awake()
	{
		this.m_ability = base.GetComponent<BaseAbility_RL>();
		this.m_onItemCollected = new Action<MonoBehaviour, EventArgs>(this.OnItemCollected);
	}

	// Token: 0x0600139C RID: 5020 RVA: 0x00009FC2 File Offset: 0x000081C2
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.ItemCollected, this.m_onItemCollected);
	}

	// Token: 0x0600139D RID: 5021 RVA: 0x00009FD1 File Offset: 0x000081D1
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.ItemCollected, this.m_onItemCollected);
	}

	// Token: 0x0600139E RID: 5022 RVA: 0x00085E70 File Offset: 0x00084070
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

	// Token: 0x040015C2 RID: 5570
	[SerializeField]
	private float m_cooldownReductionAmount;

	// Token: 0x040015C3 RID: 5571
	[SerializeField]
	private bool m_isFlat;

	// Token: 0x040015C4 RID: 5572
	[SerializeField]
	private bool m_triggerOnCoins;

	// Token: 0x040015C5 RID: 5573
	[SerializeField]
	private bool m_triggerOnEquipmentOre;

	// Token: 0x040015C6 RID: 5574
	[SerializeField]
	private bool m_triggerOnRuneOre;

	// Token: 0x040015C7 RID: 5575
	[SerializeField]
	private bool m_triggerOnHealthDrops;

	// Token: 0x040015C8 RID: 5576
	[SerializeField]
	private bool m_triggerOnManaDrops;

	// Token: 0x040015C9 RID: 5577
	[SerializeField]
	private bool m_triggerOnSoul;

	// Token: 0x040015CA RID: 5578
	private BaseAbility_RL m_ability;

	// Token: 0x040015CB RID: 5579
	private Action<MonoBehaviour, EventArgs> m_onItemCollected;
}
