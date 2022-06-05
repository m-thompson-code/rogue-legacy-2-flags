using System;
using TMPro;
using UnityEngine;

// Token: 0x02000470 RID: 1136
public class EquipmentOreDrop : BaseItemDrop
{
	// Token: 0x1700103E RID: 4158
	// (get) Token: 0x060029C5 RID: 10693 RVA: 0x00089EC6 File Offset: 0x000880C6
	public override ItemDropType ItemDropType
	{
		get
		{
			return ItemDropType.EquipmentOre;
		}
	}

	// Token: 0x060029C6 RID: 10694 RVA: 0x00089ECC File Offset: 0x000880CC
	protected override void Collect(GameObject collector)
	{
		int num = Economy_EV.GetItemDropValue(this.ItemDropType, false);
		if (base.ValueOverride > -1)
		{
			num = base.ValueOverride;
		}
		num = (int)((float)num * (1f + Economy_EV.GetOreGainMod()));
		SaveManager.PlayerSaveData.EquipmentOreCollected += num;
		Vector2 absPos = collector.transform.position;
		absPos.x = base.transform.position.x;
		IMidpointObj component = collector.GetComponent<IMidpointObj>();
		if (component != null)
		{
			absPos = component.Midpoint;
			absPos.y += component.Midpoint.y - collector.transform.position.y;
		}
		int num2 = num;
		string text = num2.ToString() + " [EquipmentOre_Icon]";
		TextPopupManager.DisplayTextAtAbsPos(TextPopupType.EquipmentOreCollected, text, absPos, null, TextAlignmentOptions.Center);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.EquipmentOreChanged, this, null);
		base.ValueOverride = num;
		base.Collect(collector);
	}
}
