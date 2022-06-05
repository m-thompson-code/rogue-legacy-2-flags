using System;

// Token: 0x020004E0 RID: 1248
public class SingleRelicRoomPropController : RelicRoomPropController
{
	// Token: 0x06002EB5 RID: 11957 RVA: 0x0009F201 File Offset: 0x0009D401
	protected override void InitializePooledPropOnEnter()
	{
		base.InitializePooledPropOnEnter();
		base.RightInfoTextBox.gameObject.SetActive(false);
		base.RightIcon.gameObject.SetActive(false);
		base.RightIconTwin.gameObject.SetActive(false);
	}
}
