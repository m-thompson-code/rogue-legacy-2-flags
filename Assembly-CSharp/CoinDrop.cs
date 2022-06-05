using System;
using TMPro;
using UnityEngine;

// Token: 0x0200046C RID: 1132
public class CoinDrop : BaseItemDrop
{
	// Token: 0x1700103A RID: 4154
	// (get) Token: 0x060029BC RID: 10684 RVA: 0x00089D0E File Offset: 0x00087F0E
	public override ItemDropType ItemDropType
	{
		get
		{
			return ItemDropType.Coin;
		}
	}

	// Token: 0x060029BD RID: 10685 RVA: 0x00089D14 File Offset: 0x00087F14
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

	// Token: 0x04002246 RID: 8774
	private GoldChangedEventArgs m_goldChangedEventArgs;
}
