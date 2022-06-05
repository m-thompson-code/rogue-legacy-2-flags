using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x0200093E RID: 2366
	[AddComponentMenu("")]
	[RequireComponent(typeof(Image))]
	public class TouchButtonExample : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
	{
		// Token: 0x17001AC8 RID: 6856
		// (get) Token: 0x06005032 RID: 20530 RVA: 0x0011AB47 File Offset: 0x00118D47
		// (set) Token: 0x06005033 RID: 20531 RVA: 0x0011AB4F File Offset: 0x00118D4F
		public bool isPressed { get; private set; }

		// Token: 0x06005034 RID: 20532 RVA: 0x0011AB58 File Offset: 0x00118D58
		private void Awake()
		{
			if (SystemInfo.deviceType == DeviceType.Handheld)
			{
				this.allowMouseControl = false;
			}
		}

		// Token: 0x06005035 RID: 20533 RVA: 0x0011AB69 File Offset: 0x00118D69
		private void Restart()
		{
			this.isPressed = false;
		}

		// Token: 0x06005036 RID: 20534 RVA: 0x0011AB72 File Offset: 0x00118D72
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			if (!this.allowMouseControl && TouchButtonExample.IsMousePointerId(eventData.pointerId))
			{
				return;
			}
			this.isPressed = true;
		}

		// Token: 0x06005037 RID: 20535 RVA: 0x0011AB91 File Offset: 0x00118D91
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			if (!this.allowMouseControl && TouchButtonExample.IsMousePointerId(eventData.pointerId))
			{
				return;
			}
			this.isPressed = false;
		}

		// Token: 0x06005038 RID: 20536 RVA: 0x0011ABB0 File Offset: 0x00118DB0
		private static bool IsMousePointerId(int id)
		{
			return id == -1 || id == -2 || id == -3;
		}

		// Token: 0x04004296 RID: 17046
		public bool allowMouseControl = true;
	}
}
