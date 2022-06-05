using System;
using System.Collections.Generic;
using Rewired.Integration.UnityUI;
using UnityEngine;

// Token: 0x02000268 RID: 616
public class MouseInputManager : MonoBehaviour
{
	// Token: 0x0600188C RID: 6284 RVA: 0x0004CE6F File Offset: 0x0004B06F
	private void Awake()
	{
		this.m_onWindowOpenOrClose = new Action<MonoBehaviour, EventArgs>(this.OnWindowOpenOrClose);
	}

	// Token: 0x0600188D RID: 6285 RVA: 0x0004CE84 File Offset: 0x0004B084
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

	// Token: 0x040017DA RID: 6106
	[SerializeField]
	private RewiredStandaloneInputModule m_inputModule;

	// Token: 0x040017DB RID: 6107
	private List<WindowID> m_openWindows;

	// Token: 0x040017DC RID: 6108
	private Action<MonoBehaviour, EventArgs> m_onWindowOpenOrClose;
}
