using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Demos.GamepadTemplateUI
{
	// Token: 0x0200094D RID: 2381
	[RequireComponent(typeof(Image))]
	public class ControllerUIEffect : MonoBehaviour
	{
		// Token: 0x060050B4 RID: 20660 RVA: 0x0011D633 File Offset: 0x0011B833
		private void Awake()
		{
			this._image = base.GetComponent<Image>();
			this._origColor = this._image.color;
			this._color = this._origColor;
		}

		// Token: 0x060050B5 RID: 20661 RVA: 0x0011D660 File Offset: 0x0011B860
		public void Activate(float amount)
		{
			amount = Mathf.Clamp01(amount);
			if (this._isActive && amount == this._highlightAmount)
			{
				return;
			}
			this._highlightAmount = amount;
			this._color = Color.Lerp(this._origColor, this._highlightColor, this._highlightAmount);
			this._isActive = true;
			this.RedrawImage();
		}

		// Token: 0x060050B6 RID: 20662 RVA: 0x0011D6B8 File Offset: 0x0011B8B8
		public void Deactivate()
		{
			if (!this._isActive)
			{
				return;
			}
			this._color = this._origColor;
			this._highlightAmount = 0f;
			this._isActive = false;
			this.RedrawImage();
		}

		// Token: 0x060050B7 RID: 20663 RVA: 0x0011D6E7 File Offset: 0x0011B8E7
		private void RedrawImage()
		{
			this._image.color = this._color;
			this._image.enabled = this._isActive;
		}

		// Token: 0x040042FD RID: 17149
		[SerializeField]
		private Color _highlightColor = Color.white;

		// Token: 0x040042FE RID: 17150
		private Image _image;

		// Token: 0x040042FF RID: 17151
		private Color _color;

		// Token: 0x04004300 RID: 17152
		private Color _origColor;

		// Token: 0x04004301 RID: 17153
		private bool _isActive;

		// Token: 0x04004302 RID: 17154
		private float _highlightAmount;
	}
}
