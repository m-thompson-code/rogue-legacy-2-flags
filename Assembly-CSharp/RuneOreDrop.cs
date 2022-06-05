using System;
using TMPro;
using UnityEngine;

// Token: 0x0200047A RID: 1146
public class RuneOreDrop : BaseItemDrop
{
	// Token: 0x1700104C RID: 4172
	// (get) Token: 0x060029EB RID: 10731 RVA: 0x0008A8B5 File Offset: 0x00088AB5
	public override ItemDropType ItemDropType
	{
		get
		{
			return ItemDropType.RuneOre;
		}
	}

	// Token: 0x060029EC RID: 10732 RVA: 0x0008A8BC File Offset: 0x00088ABC
	protected override void Collect(GameObject collector)
	{
		int num = Economy_EV.GetItemDropValue(this.ItemDropType, false);
		if (base.ValueOverride > -1)
		{
			num = base.ValueOverride;
		}
		num = (int)((float)num * (1f + Economy_EV.GetRuneOreGainMod()));
		SaveManager.PlayerSaveData.RuneOreCollected += num;
		Vector2 absPos = collector.transform.position;
		absPos.x = base.transform.position.x;
		IMidpointObj component = collector.GetComponent<IMidpointObj>();
		if (component != null)
		{
			absPos = component.Midpoint;
			absPos.y += component.Midpoint.y - collector.transform.position.y;
		}
		int num2 = num;
		string text = num2.ToString() + " [RuneOre_Icon]";
		TextPopupManager.DisplayTextAtAbsPos(TextPopupType.RuneOreCollected, text, absPos, null, TextAlignmentOptions.Center);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.RuneOreChanged, this, null);
		base.ValueOverride = num;
		base.Collect(collector);
	}
}
