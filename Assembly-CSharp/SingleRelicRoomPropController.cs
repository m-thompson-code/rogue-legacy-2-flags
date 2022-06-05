using System;

// Token: 0x02000827 RID: 2087
public class SingleRelicRoomPropController : RelicRoomPropController
{
	// Token: 0x06004069 RID: 16489 RVA: 0x00023901 File Offset: 0x00021B01
	protected override void InitializePooledPropOnEnter()
	{
		base.InitializePooledPropOnEnter();
		base.RightInfoTextBox.gameObject.SetActive(false);
		base.RightIcon.gameObject.SetActive(false);
		base.RightIconTwin.gameObject.SetActive(false);
	}
}
