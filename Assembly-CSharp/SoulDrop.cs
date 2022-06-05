using System;
using TMPro;
using UnityEngine;

// Token: 0x02000771 RID: 1905
public class SoulDrop : BaseItemDrop
{
	// Token: 0x17001582 RID: 5506
	// (get) Token: 0x060039F8 RID: 14840 RVA: 0x00006CB3 File Offset: 0x00004EB3
	public override ItemDropType ItemDropType
	{
		get
		{
			return ItemDropType.Soul;
		}
	}

	// Token: 0x060039F9 RID: 14841 RVA: 0x000EC5C4 File Offset: 0x000EA7C4
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

	// Token: 0x04002E2D RID: 11821
	public static int FakeSoulCounter_STATIC;

	// Token: 0x04002E2E RID: 11822
	private GoldChangedEventArgs m_goldChangedEventArgs;
}
