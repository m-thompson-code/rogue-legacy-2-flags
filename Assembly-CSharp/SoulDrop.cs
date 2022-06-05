using System;
using TMPro;
using UnityEngine;

// Token: 0x0200047B RID: 1147
public class SoulDrop : BaseItemDrop
{
	// Token: 0x1700104D RID: 4173
	// (get) Token: 0x060029EE RID: 10734 RVA: 0x0008A9B1 File Offset: 0x00088BB1
	public override ItemDropType ItemDropType
	{
		get
		{
			return ItemDropType.Soul;
		}
	}

	// Token: 0x060029EF RID: 10735 RVA: 0x0008A9B8 File Offset: 0x00088BB8
	protected override void Collect(GameObject collector)
	{
		int num = Economy_EV.GetItemDropValue(this.ItemDropType, false);
		if (base.ValueOverride > -1)
		{
			num = base.ValueOverride;
		}
		SoulDrop.FakeSoulCounter_STATIC -= num;
		Vector2 absPos = collector.transform.position;
		absPos.x = base.transform.position.x;
		IMidpointObj component = collector.GetComponent<IMidpointObj>();
		if (component != null)
		{
			absPos = component.Midpoint;
			absPos.y += component.Midpoint.y - collector.transform.position.y;
		}
		int num2 = num;
		string text = num2.ToString() + " [Soul_Icon]";
		TextPopupManager.DisplayTextAtAbsPos(TextPopupType.SoulCollected, text, absPos, null, TextAlignmentOptions.Center);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.SoulChanged, this, null);
		base.ValueOverride = num;
		base.Collect(collector);
		if (!ItemDropManager.HasActiveItemDropsOfType(ItemDropType.Soul))
		{
			SoulDrop.FakeSoulCounter_STATIC = 0;
		}
	}

	// Token: 0x04002251 RID: 8785
	public static int FakeSoulCounter_STATIC;

	// Token: 0x04002252 RID: 8786
	private GoldChangedEventArgs m_goldChangedEventArgs;
}
