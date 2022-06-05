using System;
using UnityEngine;

// Token: 0x02000B3C RID: 2876
public class MouseCursorManager : MonoBehaviour
{
	// Token: 0x17001D44 RID: 7492
	// (get) Token: 0x060056FA RID: 22266 RVA: 0x0002F54D File Offset: 0x0002D74D
	private static MouseCursorManager Instance
	{
		get
		{
			return MouseCursorManager.m_cursorManager;
		}
	}

	// Token: 0x17001D45 RID: 7493
	// (get) Token: 0x060056FB RID: 22267 RVA: 0x0002F554 File Offset: 0x0002D754
	// (set) Token: 0x060056FC RID: 22268 RVA: 0x0002F55B File Offset: 0x0002D75B
	public static bool IsInitialized { get; private set; }

	// Token: 0x17001D46 RID: 7494
	// (get) Token: 0x060056FD RID: 22269 RVA: 0x0002F563 File Offset: 0x0002D763
	public static bool IsCursorVisible
	{
		get
		{
			return MouseCursorManager.Instance.m_cursorVisible;
		}
	}

	// Token: 0x060056FE RID: 22270 RVA: 0x0002F56F File Offset: 0x0002D76F
	public static void SetCursorVisible(bool visible)
	{
		if (MouseCursorManager.Instance.m_cursorVisible != visible)
		{
			MouseCursorManager.Instance.m_cursorVisible = visible;
			Cursor.visible = visible;
		}
	}

	// Token: 0x060056FF RID: 22271 RVA: 0x0002F58F File Offset: 0x0002D78F
	public static void SetCursorType(CursorIconType cursorType)
	{
		Cursor.SetCursor(IconLibrary.GetCursorIconTexture(cursorType), Vector2.zero, CursorMode.Auto);
	}

	// Token: 0x06005700 RID: 22272 RVA: 0x0002F5A2 File Offset: 0x0002D7A2
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

	// Token: 0x06005701 RID: 22273 RVA: 0x0002F5C9 File Offset: 0x0002D7C9
	private void Initialize()
	{
		MouseCursorManager.SetCursorType(CursorIconType.Standard);
		MouseCursorManager.SetCursorVisible(false);
		MouseCursorManager.IsInitialized = true;
	}

	// Token: 0x0400405B RID: 16475
	private bool m_cursorVisible = true;

	// Token: 0x0400405C RID: 16476
	private static MouseCursorManager m_cursorManager;
}
