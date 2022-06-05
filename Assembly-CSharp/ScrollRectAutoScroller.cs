using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004F0 RID: 1264
public class ScrollRectAutoScroller : MonoBehaviour
{
	// Token: 0x060028C1 RID: 10433 RVA: 0x00016DD2 File Offset: 0x00014FD2
	private void Awake()
	{
		this.m_scrollBar = this.m_scrollRect.verticalScrollbar;
		this.m_contentRectTransform = this.m_scrollRect.content;
		this.m_viewportRectTransform = this.m_scrollRect.viewport;
	}

	// Token: 0x060028C2 RID: 10434 RVA: 0x00016E07 File Offset: 0x00015007
	public void ConfigureScrollBar(int arraySize)
	{
		this.m_arraySize = arraySize;
	}

	// Token: 0x060028C3 RID: 10435 RVA: 0x00016E10 File Offset: 0x00015010
	public void SetSelectedIndex(int selectedEntryIndex)
	{
		this.AutoScroll(selectedEntryIndex);
	}

	// Token: 0x060028C4 RID: 10436 RVA: 0x000BEC5C File Offset: 0x000BCE5C
	protected virtual void AutoScroll(int selectedEntryIndex)
	{
		float height = this.m_contentRectTransform.rect.height;
		float num = 1f - this.m_scrollBar.value;
		float height2 = this.m_viewportRectTransform.rect.height;
		if (height2 == 0f)
		{
			this.m_scrollBar.value = 1f;
			return;
		}
		float num2 = this.m_contentRectTransform.rect.height / (float)this.m_arraySize;
		float num3 = height2;
		float num4 = num3 - height2;
		if (num > 0f)
		{
			num3 = height2 + (height - height2) * num;
			num4 = num3 - height2;
		}
		float num5 = (float)(selectedEntryIndex + 1) * num2;
		float num6 = num5 - num2;
		float duration = 0.1f;
		if (num6 >= num4)
		{
			if (num5 > num3)
			{
				float num7 = 1f - (num5 - height2) / (height - height2);
				if (num7 >= 1f || num7 <= 0f)
				{
					this.m_scrollBar.value = Mathf.Clamp(num7, 0f, 1f);
					return;
				}
				TweenManager.TweenTo_UnscaledTime(this.m_scrollBar, duration, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
				{
					"value",
					Mathf.Clamp(num7, 0f, 1f)
				});
			}
			return;
		}
		float num8 = 1f - num6 / (height - height2);
		if (num8 >= 1f || num8 <= 0f)
		{
			this.m_scrollBar.value = Mathf.Clamp(num8, 0f, 1f);
			return;
		}
		TweenManager.TweenTo_UnscaledTime(this.m_scrollBar, duration, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
		{
			"value",
			Mathf.Clamp(num8, 0f, 1f)
		});
	}

	// Token: 0x040023B5 RID: 9141
	[SerializeField]
	private ScrollRect m_scrollRect;

	// Token: 0x040023B6 RID: 9142
	private Scrollbar m_scrollBar;

	// Token: 0x040023B7 RID: 9143
	private RectTransform m_viewportRectTransform;

	// Token: 0x040023B8 RID: 9144
	private RectTransform m_contentRectTransform;

	// Token: 0x040023B9 RID: 9145
	private int m_arraySize;
}
