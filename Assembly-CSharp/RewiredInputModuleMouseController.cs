using System;
using Rewired;
using Rewired.Integration.UnityUI;
using UnityEngine;

// Token: 0x020002BE RID: 702
public class RewiredInputModuleMouseController : MonoBehaviour
{
	// Token: 0x06001BEE RID: 7150 RVA: 0x0005A1AA File Offset: 0x000583AA
	private void Awake()
	{
		this.m_reInputModule = base.GetComponent<RewiredStandaloneInputModule>();
	}

	// Token: 0x06001BEF RID: 7151 RVA: 0x0005A1B8 File Offset: 0x000583B8
	private void OnEnable()
	{
		ReInput.controllers.RemoveLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(this.LastControllerChanged));
		ReInput.controllers.AddLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(this.LastControllerChanged));
	}

	// Token: 0x06001BF0 RID: 7152 RVA: 0x0005A1E6 File Offset: 0x000583E6
	private void LastControllerChanged(Controller controller)
	{
		if (controller.type == ControllerType.Joystick)
		{
			this.m_reInputModule.allowMouseInput = false;
			return;
		}
		this.m_reInputModule.allowMouseInput = true;
	}

	// Token: 0x0400197C RID: 6524
	private RewiredStandaloneInputModule m_reInputModule;
}
