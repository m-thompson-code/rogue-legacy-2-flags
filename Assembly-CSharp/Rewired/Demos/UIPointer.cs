using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x02000EE3 RID: 3811
	[AddComponentMenu("")]
	[RequireComponent(typeof(RectTransform))]
	public sealed class UIPointer : UIBehaviour
	{
		// Token: 0x17002409 RID: 9225
		// (get) Token: 0x06006E4E RID: 28238 RVA: 0x0003CA08 File Offset: 0x0003AC08
		// (set) Token: 0x06006E4F RID: 28239 RVA: 0x0003CA10 File Offset: 0x0003AC10
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

		// Token: 0x06006E50 RID: 28240 RVA: 0x0018A9CC File Offset: 0x00188BCC
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

		// Token: 0x06006E51 RID: 28241 RVA: 0x0003CA31 File Offset: 0x0003AC31
		private void Update()
		{
			if (this._autoSort && base.transform.GetSiblingIndex() < base.transform.parent.childCount - 1)
			{
				base.transform.SetAsLastSibling();
			}
		}

		// Token: 0x06006E52 RID: 28242 RVA: 0x0003CA65 File Offset: 0x0003AC65
		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			this.GetDependencies();
		}

		// Token: 0x06006E53 RID: 28243 RVA: 0x0003CA73 File Offset: 0x0003AC73
		protected override void OnCanvasGroupChanged()
		{
			base.OnCanvasGroupChanged();
			this.GetDependencies();
		}

		// Token: 0x06006E54 RID: 28244 RVA: 0x0018AA24 File Offset: 0x00188C24
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

		// Token: 0x06006E55 RID: 28245 RVA: 0x0003CA81 File Offset: 0x0003AC81
		private void GetDependencies()
		{
			this._canvas = base.transform.root.GetComponentInChildren<Canvas>();
		}

		// Token: 0x040058AE RID: 22702
		[Tooltip("Should the hardware pointer be hidden?")]
		[SerializeField]
		private bool _hideHardwarePointer = true;

		// Token: 0x040058AF RID: 22703
		[Tooltip("Sets the pointer to the last sibling in the parent hierarchy. Do not enable this on multiple UIPointers under the same parent transform or they will constantly fight each other for dominance.")]
		[SerializeField]
		private bool _autoSort = true;

		// Token: 0x040058B0 RID: 22704
		private Canvas _canvas;
	}
}
