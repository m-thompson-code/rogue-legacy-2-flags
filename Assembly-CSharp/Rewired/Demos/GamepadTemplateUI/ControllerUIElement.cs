using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Demos.GamepadTemplateUI
{
	// Token: 0x02000EF4 RID: 3828
	[RequireComponent(typeof(Image))]
	public class ControllerUIElement : MonoBehaviour
	{
		// Token: 0x17002414 RID: 9236
		// (get) Token: 0x06006EA6 RID: 28326 RVA: 0x0003CED9 File Offset: 0x0003B0D9
		private bool hasEffects
		{
			get
			{
				return this._positiveUIEffect != null || this._negativeUIEffect != null;
			}
		}

		// Token: 0x06006EA7 RID: 28327 RVA: 0x0003CEF7 File Offset: 0x0003B0F7
		private void Awake()
		{
			this._image = base.GetComponent<Image>();
			this._origColor = this._image.color;
			this._color = this._origColor;
			this.ClearLabels();
		}

		// Token: 0x06006EA8 RID: 28328 RVA: 0x0018BCDC File Offset: 0x00189EDC
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

		// Token: 0x06006EA9 RID: 28329 RVA: 0x0018BDD0 File Offset: 0x00189FD0
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

		// Token: 0x06006EAA RID: 28330 RVA: 0x0018BE78 File Offset: 0x0018A078
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

		// Token: 0x06006EAB RID: 28331 RVA: 0x0018BF04 File Offset: 0x0018A104
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

		// Token: 0x06006EAC RID: 28332 RVA: 0x0003CF28 File Offset: 0x0003B128
		private void RedrawImage()
		{
			this._image.color = this._color;
		}

		// Token: 0x04005904 RID: 22788
		[SerializeField]
		private Color _highlightColor = Color.white;

		// Token: 0x04005905 RID: 22789
		[SerializeField]
		private ControllerUIEffect _positiveUIEffect;

		// Token: 0x04005906 RID: 22790
		[SerializeField]
		private ControllerUIEffect _negativeUIEffect;

		// Token: 0x04005907 RID: 22791
		[SerializeField]
		private Text _label;

		// Token: 0x04005908 RID: 22792
		[SerializeField]
		private Text _positiveLabel;

		// Token: 0x04005909 RID: 22793
		[SerializeField]
		private Text _negativeLabel;

		// Token: 0x0400590A RID: 22794
		[SerializeField]
		private ControllerUIElement[] _childElements = new ControllerUIElement[0];

		// Token: 0x0400590B RID: 22795
		private Image _image;

		// Token: 0x0400590C RID: 22796
		private Color _color;

		// Token: 0x0400590D RID: 22797
		private Color _origColor;

		// Token: 0x0400590E RID: 22798
		private bool _isActive;

		// Token: 0x0400590F RID: 22799
		private float _highlightAmount;
	}
}
