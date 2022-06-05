using System;
using Rewired;
using Rewired.Integration.UnityUI;
using UnityEngine;

// Token: 0x020004AE RID: 1198
public class RewiredInputModuleMouseController : MonoBehaviour
{
	// Token: 0x060026A4 RID: 9892 RVA: 0x0001596B File Offset: 0x00013B6B
	private void Awake()
	{
		this.m_reInputModule = base.GetComponent<RewiredStandaloneInputModule>();
	}

	// Token: 0x060026A5 RID: 9893 RVA: 0x00015979 File Offset: 0x00013B79
	private void OnEnable()
	{
		ReInput.controllers.RemoveLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(this.LastControllerChanged));
		ReInput.controllers.AddLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(this.LastControllerChanged));
	}

	// Token: 0x060026A6 RID: 9894 RVA: 0x000159A7 File Offset: 0x00013BA7
	private void LastControllerChanged(Controller controller)
	{
		if (controller.type == ControllerType.Joystick)
		{
			this.m_reInputModule.allowMouseInput = false;
			return;
		}
		this.m_reInputModule.allowMouseInput = true;
	}

	// Token: 0x0400216C RID: 8556
	private RewiredStandaloneInputModule m_reInputModule;
}
