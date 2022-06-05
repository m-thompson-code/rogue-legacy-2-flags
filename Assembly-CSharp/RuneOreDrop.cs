using System;
using TMPro;
using UnityEngine;

// Token: 0x02000770 RID: 1904
public class RuneOreDrop : BaseItemDrop
{
	// Token: 0x17001581 RID: 5505
	// (get) Token: 0x060039F5 RID: 14837 RVA: 0x00004527 File Offset: 0x00002727
	public override ItemDropType ItemDropType
	{
		get
		{
			return ItemDropType.RuneOre;
		}
	}

	// Token: 0x060039F6 RID: 14838 RVA: 0x000EC4D4 File Offset: 0x000EA6D4
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
