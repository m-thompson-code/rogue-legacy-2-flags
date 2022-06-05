using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x02000945 RID: 2373
	[AddComponentMenu("")]
	[RequireComponent(typeof(RectTransform))]
	public sealed class UIPointer : UIBehaviour
	{
		// Token: 0x17001ACB RID: 6859
		// (get) Token: 0x06005074 RID: 20596 RVA: 0x0011C1EA File Offset: 0x0011A3EA
		// (set) Token: 0x06005075 RID: 20597 RVA: 0x0011C1F2 File Offset: 0x0011A3F2
		public bool autoSort
		{
			get
			{
				return this._autoSort;
			}
			set
			{
				if (value == this._autoSort)
				{
					return;
				}
				this._autoSort = value;
				if (value)
				{
					base.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x06005076 RID: 20598 RVA: 0x0011C214 File Offset: 0x0011A414
		protected override void Awake()
		{
			base.Awake();
			Graphic[] componentsInChildren = base.GetComponentsInChildren<Graphic>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].raycastTarget = false;
			}
			if (this._hideHardwarePointer)
			{
				Cursor.visible = false;
			}
			if (this._autoSort)
			{
				base.transform.SetAsLastSibling();
			}
			this.GetDependencies();
		}

		// Token: 0x06005077 RID: 20599 RVA: 0x0011C26C File Offset: 0x0011A46C
		private void Update()
		{
			if (this._autoSort && base.transform.GetSiblingIndex() < base.transform.parent.childCount - 1)
			{
				base.transform.SetAsLastSibling();
			}
		}

		// Token: 0x06005078 RID: 20600 RVA: 0x0011C2A0 File Offset: 0x0011A4A0
		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			this.GetDependencies();
		}

		// Token: 0x06005079 RID: 20601 RVA: 0x0011C2AE File Offset: 0x0011A4AE
		protected override void OnCanvasGroupChanged()
		{
			base.OnCanvasGroupChanged();
			this.GetDependencies();
		}

		// Token: 0x0600507A RID: 20602 RVA: 0x0011C2BC File Offset: 0x0011A4BC
		public void OnScreenPositionChanged(Vector2 screenPosition)
		{
			if (this._canvas == null)
			{
				return;
			}
			Camera cam = null;
			RenderMode renderMode = this._canvas.renderMode;
			if (renderMode != RenderMode.ScreenSpaceOverlay && renderMode - RenderMode.ScreenSpaceCamera <= 1)
			{
				cam = this._canvas.worldCamera;
			}
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform.parent as RectTransform, screenPosition, cam, out vector);
			base.transform.localPosition = new Vector3(vector.x, vector.y, base.transform.localPosition.z);
		}

		// Token: 0x0600507B RID: 20603 RVA: 0x0011C341 File Offset: 0x0011A541
		private void GetDependencies()
		{
			this._canvas = base.transform.root.GetComponentInChildren<Canvas>();
		}

		// Token: 0x040042CB RID: 17099
		[Tooltip("Should the hardware pointer be hidden?")]
		[SerializeField]
		private bool _hideHardwarePointer = true;

		// Token: 0x040042CC RID: 17100
		[Tooltip("Sets the pointer to the last sibling in the parent hierarchy. Do not enable this on multiple UIPointers under the same parent transform or they will constantly fight each other for dominance.")]
		[SerializeField]
		private bool _autoSort = true;

		// Token: 0x040042CD RID: 17101
		private Canvas _canvas;
	}
}
