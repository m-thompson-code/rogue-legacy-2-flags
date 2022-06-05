using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Demos.GamepadTemplateUI
{
	// Token: 0x0200094E RID: 2382
	[RequireComponent(typeof(Image))]
	public class ControllerUIElement : MonoBehaviour
	{
		// Token: 0x17001AD2 RID: 6866
		// (get) Token: 0x060050B9 RID: 20665 RVA: 0x0011D71E File Offset: 0x0011B91E
		private bool hasEffects
		{
			get
			{
				return this._positiveUIEffect != null || this._negativeUIEffect != null;
			}
		}

		// Token: 0x060050BA RID: 20666 RVA: 0x0011D73C File Offset: 0x0011B93C
		private void Awake()
		{
			this._image = base.GetComponent<Image>();
			this._origColor = this._image.color;
			this._color = this._origColor;
			this.ClearLabels();
		}

		// Token: 0x060050BB RID: 20667 RVA: 0x0011D770 File Offset: 0x0011B970
		public void Activate(float amount)
		{
			amount = Mathf.Clamp(amount, -1f, 1f);
			if (this.hasEffects)
			{
				if (amount < 0f && this._negativeUIEffect != null)
				{
					this._negativeUIEffect.Activate(Mathf.Abs(amount));
				}
				if (amount > 0f && this._positiveUIEffect != null)
				{
					this._positiveUIEffect.Activate(Mathf.Abs(amount));
				}
			}
			else
			{
				if (this._isActive && amount == this._highlightAmount)
				{
					return;
				}
				this._highlightAmount = amount;
				this._color = Color.Lerp(this._origColor, this._highlightColor, this._highlightAmount);
			}
			this._isActive = true;
			this.RedrawImage();
			if (this._childElements.Length != 0)
			{
				for (int i = 0; i < this._childElements.Length; i++)
				{
					if (!(this._childElements[i] == null))
					{
						this._childElements[i].Activate(amount);
					}
				}
			}
		}

		// Token: 0x060050BC RID: 20668 RVA: 0x0011D864 File Offset: 0x0011BA64
		public void Deactivate()
		{
			if (!this._isActive)
			{
				return;
			}
			this._color = this._origColor;
			this._highlightAmount = 0f;
			if (this._positiveUIEffect != null)
			{
				this._positiveUIEffect.Deactivate();
			}
			if (this._negativeUIEffect != null)
			{
				this._negativeUIEffect.Deactivate();
			}
			this._isActive = false;
			this.RedrawImage();
			if (this._childElements.Length != 0)
			{
				for (int i = 0; i < this._childElements.Length; i++)
				{
					if (!(this._childElements[i] == null))
					{
						this._childElements[i].Deactivate();
					}
				}
			}
		}

		// Token: 0x060050BD RID: 20669 RVA: 0x0011D90C File Offset: 0x0011BB0C
		public void SetLabel(string text, AxisRange labelType)
		{
			Text text2;
			switch (labelType)
			{
			case AxisRange.Full:
				text2 = this._label;
				break;
			case AxisRange.Positive:
				text2 = this._positiveLabel;
				break;
			case AxisRange.Negative:
				text2 = this._negativeLabel;
				break;
			default:
				text2 = null;
				break;
			}
			if (text2 != null)
			{
				text2.text = text;
			}
			if (this._childElements.Length != 0)
			{
				for (int i = 0; i < this._childElements.Length; i++)
				{
					if (!(this._childElements[i] == null))
					{
						this._childElements[i].SetLabel(text, labelType);
					}
				}
			}
		}

		// Token: 0x060050BE RID: 20670 RVA: 0x0011D998 File Offset: 0x0011BB98
		public void ClearLabels()
		{
			if (this._label != null)
			{
				this._label.text = string.Empty;
			}
			if (this._positiveLabel != null)
			{
				this._positiveLabel.text = string.Empty;
			}
			if (this._negativeLabel != null)
			{
				this._negativeLabel.text = string.Empty;
			}
			if (this._childElements.Length != 0)
			{
				for (int i = 0; i < this._childElements.Length; i++)
				{
					if (!(this._childElements[i] == null))
					{
						this._childElements[i].ClearLabels();
					}
				}
			}
		}

		// Token: 0x060050BF RID: 20671 RVA: 0x0011DA38 File Offset: 0x0011BC38
		private void RedrawImage()
		{
			this._image.color = this._color;
		}

		// Token: 0x04004303 RID: 17155
		[SerializeField]
		private Color _highlightColor = Color.white;

		// Token: 0x04004304 RID: 17156
		[SerializeField]
		private ControllerUIEffect _positiveUIEffect;

		// Token: 0x04004305 RID: 17157
		[SerializeField]
		private ControllerUIEffect _negativeUIEffect;

		// Token: 0x04004306 RID: 17158
		[SerializeField]
		private Text _label;

		// Token: 0x04004307 RID: 17159
		[SerializeField]
		private Text _positiveLabel;

		// Token: 0x04004308 RID: 17160
		[SerializeField]
		private Text _negativeLabel;

		// Token: 0x04004309 RID: 17161
		[SerializeField]
		private ControllerUIElement[] _childElements = new ControllerUIElement[0];

		// Token: 0x0400430A RID: 17162
		private Image _image;

		// Token: 0x0400430B RID: 17163
		private Color _color;

		// Token: 0x0400430C RID: 17164
		private Color _origColor;

		// Token: 0x0400430D RID: 17165
		private bool _isActive;

		// Token: 0x0400430E RID: 17166
		private float _highlightAmount;
	}
}
