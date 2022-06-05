using System;
using TMPro;
using UnityEngine;

// Token: 0x02000766 RID: 1894
public class EquipmentOreDrop : BaseItemDrop
{
	// Token: 0x17001573 RID: 5491
	// (get) Token: 0x060039CF RID: 14799 RVA: 0x0000452B File Offset: 0x0000272B
	public override ItemDropType ItemDropType
	{
		get
		{
			return ItemDropType.EquipmentOre;
		}
	}

	// Token: 0x060039D0 RID: 14800 RVA: 0x000EBC24 File Offset: 0x000E9E24
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
