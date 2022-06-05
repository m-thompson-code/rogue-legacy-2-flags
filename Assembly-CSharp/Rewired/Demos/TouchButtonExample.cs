using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x02000EDA RID: 3802
	[AddComponentMenu("")]
	[RequireComponent(typeof(Image))]
	public class TouchButtonExample : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
	{
		// Token: 0x17002406 RID: 9222
		// (get) Token: 0x06006E09 RID: 28169 RVA: 0x0003C752 File Offset: 0x0003A952
		// (set) Token: 0x06006E0A RID: 28170 RVA: 0x0003C75A File Offset: 0x0003A95A
		public bool isPressed { get; private set; }

		// Token: 0x06006E0B RID: 28171 RVA: 0x0003C763 File Offset: 0x0003A963
		private void Awake()
		{
			if (SystemInfo.deviceType == DeviceType.Handheld)
			{
				this.allowMouseControl = false;
			}
		}

		// Token: 0x06006E0C RID: 28172 RVA: 0x0003C774 File Offset: 0x0003A974
		private void Restart()
		{
			this.isPressed = false;
		}

		// Token: 0x06006E0D RID: 28173 RVA: 0x0003C77D File Offset: 0x0003A97D
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			if (!this.allowMouseControl && TouchButtonExample.IsMousePointerId(eventData.pointerId))
			{
				return;
			}
			this.isPressed = true;
		}

		// Token: 0x06006E0E RID: 28174 RVA: 0x0003C79C File Offset: 0x0003A99C
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			if (!this.allowMouseControl && TouchButtonExample.IsMousePointerId(eventData.pointerId))
			{
				return;
			}
			this.isPressed = false;
		}

		// Token: 0x06006E0F RID: 28175 RVA: 0x0003C7BB File Offset: 0x0003A9BB
		private static bool IsMousePointerId(int id)
		{
			return id == -1 || id == -2 || id == -3;
		}

		// Token: 0x04005876 RID: 22646
		public bool allowMouseControl = true;
	}
}
