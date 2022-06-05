using System;
using System.Collections.Generic;
using Rewired.Integration.UnityUI;
using UnityEngine;

// Token: 0x0200042F RID: 1071
public class MouseInputManager : MonoBehaviour
{
	// Token: 0x06002275 RID: 8821 RVA: 0x000126DD File Offset: 0x000108DD
	private void Awake()
	{
		this.m_onWindowOpenOrClose = new Action<MonoBehaviour, EventArgs>(this.OnWindowOpenOrClose);
	}

	// Token: 0x06002276 RID: 8822 RVA: 0x000AAC10 File Offset: 0x000A8E10
	private void OnWindowOpenOrClose(MonoBehaviour sender, EventArgs eventArgs)
	{
		if (this.m_openWindows == null)
		{
			this.m_openWindows = new List<WindowID>();
		}
		WindowStateChangeEventArgs windowStateChangeEventArgs = eventArgs as WindowStateChangeEventArgs;
		if (windowStateChangeEventArgs != null)
		{
			if (windowStateChangeEventArgs.IsOpen)
			{
				if (!this.m_openWindows.Contains(windowStateChangeEventArgs.Window))
				{
					this.m_openWindows.Add(windowStateChangeEventArgs.Window);
					this.m_inputModule.allowMouseInput = false;
					return;
				}
			}
			else
			{
				if (this.m_openWindows.Contains(windowStateChangeEventArgs.Window))
				{
					this.m_openWindows.Remove(windowStateChangeEventArgs.Window);
				}
				if (this.m_openWindows.Count == 0)
				{
					this.m_inputModule.allowMouseInput = true;
					return;
				}
			}
		}
		else
		{
			Debug.LogFormat("<color=red>{0}: Unable to cast eventArgs as UIWindowStateChangeEventArgs.</color>", new object[]
			{
				Time.frameCount
			});
		}
	}

	// Token: 0x04001F19 RID: 7961
	[SerializeField]
	private RewiredStandaloneInputModule m_inputModule;

	// Token: 0x04001F1A RID: 7962
	private List<WindowID> m_openWindows;

	// Token: 0x04001F1B RID: 7963
	private Action<MonoBehaviour, EventArgs> m_onWindowOpenOrClose;
}
