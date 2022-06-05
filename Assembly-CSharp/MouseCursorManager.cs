using System;
using UnityEngine;

// Token: 0x020006A1 RID: 1697
public class MouseCursorManager : MonoBehaviour
{
	// Token: 0x1700155C RID: 5468
	// (get) Token: 0x06003E08 RID: 15880 RVA: 0x000D95CF File Offset: 0x000D77CF
	private static MouseCursorManager Instance
	{
		get
		{
			return MouseCursorManager.m_cursorManager;
		}
	}

	// Token: 0x1700155D RID: 5469
	// (get) Token: 0x06003E09 RID: 15881 RVA: 0x000D95D6 File Offset: 0x000D77D6
	// (set) Token: 0x06003E0A RID: 15882 RVA: 0x000D95DD File Offset: 0x000D77DD
	public static bool IsInitialized { get; private set; }

	// Token: 0x1700155E RID: 5470
	// (get) Token: 0x06003E0B RID: 15883 RVA: 0x000D95E5 File Offset: 0x000D77E5
	public static bool IsCursorVisible
	{
		get
		{
			return MouseCursorManager.Instance.m_cursorVisible;
		}
	}

	// Token: 0x06003E0C RID: 15884 RVA: 0x000D95F1 File Offset: 0x000D77F1
	public static void SetCursorVisible(bool visible)
	{
		if (MouseCursorManager.Instance.m_cursorVisible != visible)
		{
			MouseCursorManager.Instance.m_cursorVisible = visible;
			Cursor.visible = visible;
		}
	}

	// Token: 0x06003E0D RID: 15885 RVA: 0x000D9611 File Offset: 0x000D7811
	public static void SetCursorType(CursorIconType cursorType)
	{
		Cursor.SetCursor(IconLibrary.GetCursorIconTexture(cursorType), Vector2.zero, CursorMode.Auto);
	}

	// Token: 0x06003E0E RID: 15886 RVA: 0x000D9624 File Offset: 0x000D7824
	private void Awake()
	{
		if (MouseCursorManager.m_cursorManager == null)
		{
			MouseCursorManager.m_cursorManager = this;
			this.Initialize();
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003E0F RID: 15887 RVA: 0x000D964B File Offset: 0x000D784B
	private void Initialize()
	{
		MouseCursorManager.SetCursorType(CursorIconType.Standard);
		MouseCursorManager.SetCursorVisible(false);
		MouseCursorManager.IsInitialized = true;
	}

	// Token: 0x04002E3E RID: 11838
	private bool m_cursorVisible = true;

	// Token: 0x04002E3F RID: 11839
	private static MouseCursorManager m_cursorManager;
}
