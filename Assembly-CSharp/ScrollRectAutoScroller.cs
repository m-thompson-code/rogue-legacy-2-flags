using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002E6 RID: 742
public class ScrollRectAutoScroller : MonoBehaviour
{
	// Token: 0x06001D7C RID: 7548 RVA: 0x00060FBC File Offset: 0x0005F1BC
	private void Awake()
	{
		this.m_scrollBar = this.m_scrollRect.verticalScrollbar;
		this.m_contentRectTransform = this.m_scrollRect.content;
		this.m_viewportRectTransform = this.m_scrollRect.viewport;
	}

	// Token: 0x06001D7D RID: 7549 RVA: 0x00060FF1 File Offset: 0x0005F1F1
	public void ConfigureScrollBar(int arraySize)
	{
		this.m_arraySize = arraySize;
	}

	// Token: 0x06001D7E RID: 7550 RVA: 0x00060FFA File Offset: 0x0005F1FA
	public void SetSelectedIndex(int selectedEntryIndex)
	{
		this.AutoScroll(selectedEntryIndex);
	}

	// Token: 0x06001D7F RID: 7551 RVA: 0x00061004 File Offset: 0x0005F204
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

	// Token: 0x04001B6A RID: 7018
	[SerializeField]
	private ScrollRect m_scrollRect;

	// Token: 0x04001B6B RID: 7019
	private Scrollbar m_scrollBar;

	// Token: 0x04001B6C RID: 7020
	private RectTransform m_viewportRectTransform;

	// Token: 0x04001B6D RID: 7021
	private RectTransform m_contentRectTransform;

	// Token: 0x04001B6E RID: 7022
	private int m_arraySize;
}
