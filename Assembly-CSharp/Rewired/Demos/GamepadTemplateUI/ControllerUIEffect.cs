using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Demos.GamepadTemplateUI
{
	// Token: 0x02000EF3 RID: 3827
	[RequireComponent(typeof(Image))]
	public class ControllerUIEffect : MonoBehaviour
	{
		// Token: 0x06006EA1 RID: 28321 RVA: 0x0003CE48 File Offset: 0x0003B048
		private void Awake()
		{
			this._image = base.GetComponent<Image>();
			this._origColor = this._image.color;
			this._color = this._origColor;
		}

		// Token: 0x06006EA2 RID: 28322 RVA: 0x0018BC84 File Offset: 0x00189E84
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

		// Token: 0x06006EA3 RID: 28323 RVA: 0x0003CE73 File Offset: 0x0003B073
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

		// Token: 0x06006EA4 RID: 28324 RVA: 0x0003CEA2 File Offset: 0x0003B0A2
		private void RedrawImage()
		{
			this._image.color = this._color;
			this._image.enabled = this._isActive;
		}

		// Token: 0x040058FE RID: 22782
		[SerializeField]
		private Color _highlightColor = Color.white;

		// Token: 0x040058FF RID: 22783
		private Image _image;

		// Token: 0x04005900 RID: 22784
		private Color _color;

		// Token: 0x04005901 RID: 22785
		private Color _origColor;

		// Token: 0x04005902 RID: 22786
		private bool _isActive;

		// Token: 0x04005903 RID: 22787
		private float _highlightAmount;
	}
}
