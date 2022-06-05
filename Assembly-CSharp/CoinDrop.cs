using System;
using TMPro;
using UnityEngine;

// Token: 0x02000762 RID: 1890
public class CoinDrop : BaseItemDrop
{
	// Token: 0x1700156F RID: 5487
	// (get) Token: 0x060039C6 RID: 14790 RVA: 0x00017640 File Offset: 0x00015840
	public override ItemDropType ItemDropType
	{
		get
		{
			return ItemDropType.Coin;
		}
	}

	// Token: 0x060039C7 RID: 14791 RVA: 0x000EBA9C File Offset: 0x000E9C9C
	protected override void Collect(GameObject collector)
	{
		int num = Economy_EV.GetItemDropValue(this.ItemDropType, false);
		if (base.ValueOverride > -1)
		{
			num = base.ValueOverride;
		}
		if (num > 0)
		{
			float goldGainMod = Economy_EV.GetGoldGainMod();
			float architectGoldMod = NPC_EV.GetArchitectGoldMod(-1);
			num = Mathf.RoundToInt((float)num * (1f + goldGainMod) * architectGoldMod);
			num = Mathf.Clamp(num, 0, int.MaxValue);
			int goldCollected = SaveManager.PlayerSaveData.GoldCollected;
			SaveManager.PlayerSaveData.GoldCollected += num;
			if (this.m_goldChangedEventArgs == null)
			{
				this.m_goldChangedEventArgs = new GoldChangedEventArgs(goldCollected, SaveManager.PlayerSaveData.GoldCollected);
			}
			else
			{
				this.m_goldChangedEventArgs.Initialize(goldCollected, SaveManager.PlayerSaveData.GoldCollected);
			}
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.GoldChanged, this, this.m_goldChangedEventArgs);
			Vector2 absPos = collector.transform.position;
			absPos.x = base.transform.position.x;
			IMidpointObj component = collector.GetComponent<IMidpointObj>();
			if (component != null)
			{
				absPos = component.Midpoint;
				absPos.y += component.Midpoint.y - collector.transform.position.y;
			}
			string text = num.ToString() + " [Coin_Icon]";
			TextPopupManager.DisplayTextAtAbsPos(TextPopupType.GoldCollected, text, absPos, null, TextAlignmentOptions.Center);
			base.ValueOverride = num;
			base.Collect(collector);
			return;
		}
		throw new Exception("Gold Drop of type " + this.ItemDropType.ToString() + " is worth 0 gold.");
	}

	// Token: 0x04002E22 RID: 11810
	private GoldChangedEventArgs m_goldChangedEventArgs;
}
