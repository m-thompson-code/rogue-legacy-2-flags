using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x02000EDB RID: 3803
	[AddComponentMenu("")]
	[RequireComponent(typeof(Image))]
	public class TouchJoystickExample : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IDragHandler
	{
		// Token: 0x17002407 RID: 9223
		// (get) Token: 0x06006E11 RID: 28177 RVA: 0x0003C7DC File Offset: 0x0003A9DC
		// (set) Token: 0x06006E12 RID: 28178 RVA: 0x0003C7E4 File Offset: 0x0003A9E4
		public Vector2 position { get; private set; }

		// Token: 0x06006E13 RID: 28179 RVA: 0x0003C7ED File Offset: 0x0003A9ED
		private void Start()
		{
			if (SystemInfo.deviceType == DeviceType.Handheld)
			{
				this.allowMouseControl = false;
			}
			this.StoreOrigValues();
		}

		// Token: 0x06006E14 RID: 28180 RVA: 0x001895F0 File Offset: 0x001877F0
		private void Update()
		{
			if ((float)Screen.width != this.origScreenResolution.x || (float)Screen.height != this.origScreenResolution.y || Screen.orientation != this.origScreenOrientation)
			{
				this.Restart();
				this.StoreOrigValues();
			}
		}

		// Token: 0x06006E15 RID: 28181 RVA: 0x0003C804 File Offset: 0x0003AA04
		private void Restart()
		{
			this.hasFinger = false;
			(base.transform as RectTransform).anchoredPosition = this.origAnchoredPosition;
			this.position = Vector2.zero;
		}

		// Token: 0x06006E16 RID: 28182 RVA: 0x0018963C File Offset: 0x0018783C
		private void StoreOrigValues()
		{
			this.origAnchoredPosition = (base.transform as RectTransform).anchoredPosition;
			this.origWorldPosition = base.transform.position;
			this.origScreenResolution = new Vector2((float)Screen.width, (float)Screen.height);
			this.origScreenOrientation = Screen.orientation;
		}

		// Token: 0x06006E17 RID: 28183 RVA: 0x00189694 File Offset: 0x00187894
		private void UpdateValue(Vector3 value)
		{
			Vector3 vector = this.origWorldPosition - value;
			vector.y = -vector.y;
			vector /= (float)this.radius;
			this.position = new Vector2(-vector.x, vector.y);
		}

		// Token: 0x06006E18 RID: 28184 RVA: 0x0003C82E File Offset: 0x0003AA2E
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			if (this.hasFinger)
			{
				return;
			}
			if (!this.allowMouseControl && TouchJoystickExample.IsMousePointerId(eventData.pointerId))
			{
				return;
			}
			this.hasFinger = true;
			this.lastFingerId = eventData.pointerId;
		}

		// Token: 0x06006E19 RID: 28185 RVA: 0x0003C862 File Offset: 0x0003AA62
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			if (eventData.pointerId != this.lastFingerId)
			{
				return;
			}
			if (!this.allowMouseControl && TouchJoystickExample.IsMousePointerId(eventData.pointerId))
			{
				return;
			}
			this.Restart();
		}

		// Token: 0x06006E1A RID: 28186 RVA: 0x001896E4 File Offset: 0x001878E4
		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			if (!this.hasFinger || eventData.pointerId != this.lastFingerId)
			{
				return;
			}
			Vector3 vector = new Vector3(eventData.position.x - this.origWorldPosition.x, eventData.position.y - this.origWorldPosition.y);
			vector = Vector3.ClampMagnitude(vector, (float)this.radius);
			Vector3 vector2 = this.origWorldPosition + vector;
			base.transform.position = vector2;
			this.UpdateValue(vector2);
		}

		// Token: 0x06006E1B RID: 28187 RVA: 0x0003C7BB File Offset: 0x0003A9BB
		private static bool IsMousePointerId(int id)
		{
			return id == -1 || id == -2 || id == -3;
		}

		// Token: 0x04005878 RID: 22648
		public bool allowMouseControl = true;

		// Token: 0x04005879 RID: 22649
		public int radius = 50;

		// Token: 0x0400587A RID: 22650
		private Vector2 origAnchoredPosition;

		// Token: 0x0400587B RID: 22651
		private Vector3 origWorldPosition;

		// Token: 0x0400587C RID: 22652
		private Vector2 origScreenResolution;

		// Token: 0x0400587D RID: 22653
		private ScreenOrientation origScreenOrientation;

		// Token: 0x0400587E RID: 22654
		[NonSerialized]
		private bool hasFinger;

		// Token: 0x0400587F RID: 22655
		[NonSerialized]
		private int lastFingerId;
	}
}
