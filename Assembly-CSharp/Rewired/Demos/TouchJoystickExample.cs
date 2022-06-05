using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x0200093F RID: 2367
	[AddComponentMenu("")]
	[RequireComponent(typeof(Image))]
	public class TouchJoystickExample : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IDragHandler
	{
		// Token: 0x17001AC9 RID: 6857
		// (get) Token: 0x0600503A RID: 20538 RVA: 0x0011ABD1 File Offset: 0x00118DD1
		// (set) Token: 0x0600503B RID: 20539 RVA: 0x0011ABD9 File Offset: 0x00118DD9
		public Vector2 position { get; private set; }

		// Token: 0x0600503C RID: 20540 RVA: 0x0011ABE2 File Offset: 0x00118DE2
		private void Start()
		{
			if (SystemInfo.deviceType == DeviceType.Handheld)
			{
				this.allowMouseControl = false;
			}
			this.StoreOrigValues();
		}

		// Token: 0x0600503D RID: 20541 RVA: 0x0011ABFC File Offset: 0x00118DFC
		private void Update()
		{
			if ((float)Screen.width != this.origScreenResolution.x || (float)Screen.height != this.origScreenResolution.y || Screen.orientation != this.origScreenOrientation)
			{
				this.Restart();
				this.StoreOrigValues();
			}
		}

		// Token: 0x0600503E RID: 20542 RVA: 0x0011AC48 File Offset: 0x00118E48
		private void Restart()
		{
			this.hasFinger = false;
			(base.transform as RectTransform).anchoredPosition = this.origAnchoredPosition;
			this.position = Vector2.zero;
		}

		// Token: 0x0600503F RID: 20543 RVA: 0x0011AC74 File Offset: 0x00118E74
		private void StoreOrigValues()
		{
			this.origAnchoredPosition = (base.transform as RectTransform).anchoredPosition;
			this.origWorldPosition = base.transform.position;
			this.origScreenResolution = new Vector2((float)Screen.width, (float)Screen.height);
			this.origScreenOrientation = Screen.orientation;
		}

		// Token: 0x06005040 RID: 20544 RVA: 0x0011ACCC File Offset: 0x00118ECC
		private void UpdateValue(Vector3 value)
		{
			Vector3 vector = this.origWorldPosition - value;
			vector.y = -vector.y;
			vector /= (float)this.radius;
			this.position = new Vector2(-vector.x, vector.y);
		}

		// Token: 0x06005041 RID: 20545 RVA: 0x0011AD1A File Offset: 0x00118F1A
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

		// Token: 0x06005042 RID: 20546 RVA: 0x0011AD4E File Offset: 0x00118F4E
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

		// Token: 0x06005043 RID: 20547 RVA: 0x0011AD7C File Offset: 0x00118F7C
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

		// Token: 0x06005044 RID: 20548 RVA: 0x0011AE03 File Offset: 0x00119003
		private static bool IsMousePointerId(int id)
		{
			return id == -1 || id == -2 || id == -3;
		}

		// Token: 0x04004298 RID: 17048
		public bool allowMouseControl = true;

		// Token: 0x04004299 RID: 17049
		public int radius = 50;

		// Token: 0x0400429A RID: 17050
		private Vector2 origAnchoredPosition;

		// Token: 0x0400429B RID: 17051
		private Vector3 origWorldPosition;

		// Token: 0x0400429C RID: 17052
		private Vector2 origScreenResolution;

		// Token: 0x0400429D RID: 17053
		private ScreenOrientation origScreenOrientation;

		// Token: 0x0400429E RID: 17054
		[NonSerialized]
		private bool hasFinger;

		// Token: 0x0400429F RID: 17055
		[NonSerialized]
		private int lastFingerId;
	}
}
