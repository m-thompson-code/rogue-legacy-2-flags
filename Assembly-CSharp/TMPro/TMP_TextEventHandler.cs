using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TMPro
{
	// Token: 0x02000854 RID: 2132
	public class TMP_TextEventHandler : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x17001778 RID: 6008
		// (get) Token: 0x060046CD RID: 18125 RVA: 0x000FCF8C File Offset: 0x000FB18C
		// (set) Token: 0x060046CE RID: 18126 RVA: 0x000FCF94 File Offset: 0x000FB194
		public TMP_TextEventHandler.CharacterSelectionEvent onCharacterSelection
		{
			get
			{
				return this.m_OnCharacterSelection;
			}
			set
			{
				this.m_OnCharacterSelection = value;
			}
		}

		// Token: 0x17001779 RID: 6009
		// (get) Token: 0x060046CF RID: 18127 RVA: 0x000FCF9D File Offset: 0x000FB19D
		// (set) Token: 0x060046D0 RID: 18128 RVA: 0x000FCFA5 File Offset: 0x000FB1A5
		public TMP_TextEventHandler.SpriteSelectionEvent onSpriteSelection
		{
			get
			{
				return this.m_OnSpriteSelection;
			}
			set
			{
				this.m_OnSpriteSelection = value;
			}
		}

		// Token: 0x1700177A RID: 6010
		// (get) Token: 0x060046D1 RID: 18129 RVA: 0x000FCFAE File Offset: 0x000FB1AE
		// (set) Token: 0x060046D2 RID: 18130 RVA: 0x000FCFB6 File Offset: 0x000FB1B6
		public TMP_TextEventHandler.WordSelectionEvent onWordSelection
		{
			get
			{
				return this.m_OnWordSelection;
			}
			set
			{
				this.m_OnWordSelection = value;
			}
		}

		// Token: 0x1700177B RID: 6011
		// (get) Token: 0x060046D3 RID: 18131 RVA: 0x000FCFBF File Offset: 0x000FB1BF
		// (set) Token: 0x060046D4 RID: 18132 RVA: 0x000FCFC7 File Offset: 0x000FB1C7
		public TMP_TextEventHandler.LineSelectionEvent onLineSelection
		{
			get
			{
				return this.m_OnLineSelection;
			}
			set
			{
				this.m_OnLineSelection = value;
			}
		}

		// Token: 0x1700177C RID: 6012
		// (get) Token: 0x060046D5 RID: 18133 RVA: 0x000FCFD0 File Offset: 0x000FB1D0
		// (set) Token: 0x060046D6 RID: 18134 RVA: 0x000FCFD8 File Offset: 0x000FB1D8
		public TMP_TextEventHandler.LinkSelectionEvent onLinkSelection
		{
			get
			{
				return this.m_OnLinkSelection;
			}
			set
			{
				this.m_OnLinkSelection = value;
			}
		}

		// Token: 0x060046D7 RID: 18135 RVA: 0x000FCFE4 File Offset: 0x000FB1E4
		private void Awake()
		{
			this.m_TextComponent = base.gameObject.GetComponent<TMP_Text>();
			if (this.m_TextComponent.GetType() == typeof(TextMeshProUGUI))
			{
				this.m_Canvas = base.gameObject.GetComponentInParent<Canvas>();
				if (this.m_Canvas != null)
				{
					if (this.m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
					{
						this.m_Camera = null;
						return;
					}
					this.m_Camera = this.m_Canvas.worldCamera;
					return;
				}
			}
			else
			{
				this.m_Camera = Camera.main;
			}
		}

		// Token: 0x060046D8 RID: 18136 RVA: 0x000FD070 File Offset: 0x000FB270
		private void LateUpdate()
		{
			if (TMP_TextUtilities.IsIntersectingRectTransform(this.m_TextComponent.rectTransform, Input.mousePosition, this.m_Camera))
			{
				int num = TMP_TextUtilities.FindIntersectingCharacter(this.m_TextComponent, Input.mousePosition, this.m_Camera, true);
				if (num != -1 && num != this.m_lastCharIndex)
				{
					this.m_lastCharIndex = num;
					TMP_TextElementType elementType = this.m_TextComponent.textInfo.characterInfo[num].elementType;
					if (elementType == TMP_TextElementType.Character)
					{
						this.SendOnCharacterSelection(this.m_TextComponent.textInfo.characterInfo[num].character, num);
					}
					else if (elementType == TMP_TextElementType.Sprite)
					{
						this.SendOnSpriteSelection(this.m_TextComponent.textInfo.characterInfo[num].character, num);
					}
				}
				int num2 = TMP_TextUtilities.FindIntersectingWord(this.m_TextComponent, Input.mousePosition, this.m_Camera);
				if (num2 != -1 && num2 != this.m_lastWordIndex)
				{
					this.m_lastWordIndex = num2;
					TMP_WordInfo tmp_WordInfo = this.m_TextComponent.textInfo.wordInfo[num2];
					this.SendOnWordSelection(tmp_WordInfo.GetWord(), tmp_WordInfo.firstCharacterIndex, tmp_WordInfo.characterCount);
				}
				int num3 = TMP_TextUtilities.FindIntersectingLine(this.m_TextComponent, Input.mousePosition, this.m_Camera);
				if (num3 != -1 && num3 != this.m_lastLineIndex)
				{
					this.m_lastLineIndex = num3;
					TMP_LineInfo tmp_LineInfo = this.m_TextComponent.textInfo.lineInfo[num3];
					char[] array = new char[tmp_LineInfo.characterCount];
					int num4 = 0;
					while (num4 < tmp_LineInfo.characterCount && num4 < this.m_TextComponent.textInfo.characterInfo.Length)
					{
						array[num4] = this.m_TextComponent.textInfo.characterInfo[num4 + tmp_LineInfo.firstCharacterIndex].character;
						num4++;
					}
					string line = new string(array);
					this.SendOnLineSelection(line, tmp_LineInfo.firstCharacterIndex, tmp_LineInfo.characterCount);
				}
				int num5 = TMP_TextUtilities.FindIntersectingLink(this.m_TextComponent, Input.mousePosition, this.m_Camera);
				if (num5 != -1 && num5 != this.m_selectedLink)
				{
					this.m_selectedLink = num5;
					TMP_LinkInfo tmp_LinkInfo = this.m_TextComponent.textInfo.linkInfo[num5];
					this.SendOnLinkSelection(tmp_LinkInfo.GetLinkID(), tmp_LinkInfo.GetLinkText(), num5);
				}
			}
		}

		// Token: 0x060046D9 RID: 18137 RVA: 0x000FD2B6 File Offset: 0x000FB4B6
		public void OnPointerEnter(PointerEventData eventData)
		{
		}

		// Token: 0x060046DA RID: 18138 RVA: 0x000FD2B8 File Offset: 0x000FB4B8
		public void OnPointerExit(PointerEventData eventData)
		{
		}

		// Token: 0x060046DB RID: 18139 RVA: 0x000FD2BA File Offset: 0x000FB4BA
		private void SendOnCharacterSelection(char character, int characterIndex)
		{
			if (this.onCharacterSelection != null)
			{
				this.onCharacterSelection.Invoke(character, characterIndex);
			}
		}

		// Token: 0x060046DC RID: 18140 RVA: 0x000FD2D1 File Offset: 0x000FB4D1
		private void SendOnSpriteSelection(char character, int characterIndex)
		{
			if (this.onSpriteSelection != null)
			{
				this.onSpriteSelection.Invoke(character, characterIndex);
			}
		}

		// Token: 0x060046DD RID: 18141 RVA: 0x000FD2E8 File Offset: 0x000FB4E8
		private void SendOnWordSelection(string word, int charIndex, int length)
		{
			if (this.onWordSelection != null)
			{
				this.onWordSelection.Invoke(word, charIndex, length);
			}
		}

		// Token: 0x060046DE RID: 18142 RVA: 0x000FD300 File Offset: 0x000FB500
		private void SendOnLineSelection(string line, int charIndex, int length)
		{
			if (this.onLineSelection != null)
			{
				this.onLineSelection.Invoke(line, charIndex, length);
			}
		}

		// Token: 0x060046DF RID: 18143 RVA: 0x000FD318 File Offset: 0x000FB518
		private void SendOnLinkSelection(string linkID, string linkText, int linkIndex)
		{
			if (this.onLinkSelection != null)
			{
				this.onLinkSelection.Invoke(linkID, linkText, linkIndex);
			}
		}

		// Token: 0x04003BB1 RID: 15281
		[SerializeField]
		private TMP_TextEventHandler.CharacterSelectionEvent m_OnCharacterSelection = new TMP_TextEventHandler.CharacterSelectionEvent();

		// Token: 0x04003BB2 RID: 15282
		[SerializeField]
		private TMP_TextEventHandler.SpriteSelectionEvent m_OnSpriteSelection = new TMP_TextEventHandler.SpriteSelectionEvent();

		// Token: 0x04003BB3 RID: 15283
		[SerializeField]
		private TMP_TextEventHandler.WordSelectionEvent m_OnWordSelection = new TMP_TextEventHandler.WordSelectionEvent();

		// Token: 0x04003BB4 RID: 15284
		[SerializeField]
		private TMP_TextEventHandler.LineSelectionEvent m_OnLineSelection = new TMP_TextEventHandler.LineSelectionEvent();

		// Token: 0x04003BB5 RID: 15285
		[SerializeField]
		private TMP_TextEventHandler.LinkSelectionEvent m_OnLinkSelection = new TMP_TextEventHandler.LinkSelectionEvent();

		// Token: 0x04003BB6 RID: 15286
		private TMP_Text m_TextComponent;

		// Token: 0x04003BB7 RID: 15287
		private Camera m_Camera;

		// Token: 0x04003BB8 RID: 15288
		private Canvas m_Canvas;

		// Token: 0x04003BB9 RID: 15289
		private int m_selectedLink = -1;

		// Token: 0x04003BBA RID: 15290
		private int m_lastCharIndex = -1;

		// Token: 0x04003BBB RID: 15291
		private int m_lastWordIndex = -1;

		// Token: 0x04003BBC RID: 15292
		private int m_lastLineIndex = -1;

		// Token: 0x02000E6F RID: 3695
		[Serializable]
		public class CharacterSelectionEvent : UnityEvent<char, int>
		{
		}

		// Token: 0x02000E70 RID: 3696
		[Serializable]
		public class SpriteSelectionEvent : UnityEvent<char, int>
		{
		}

		// Token: 0x02000E71 RID: 3697
		[Serializable]
		public class WordSelectionEvent : UnityEvent<string, int, int>
		{
		}

		// Token: 0x02000E72 RID: 3698
		[Serializable]
		public class LineSelectionEvent : UnityEvent<string, int, int>
		{
		}

		// Token: 0x02000E73 RID: 3699
		[Serializable]
		public class LinkSelectionEvent : UnityEvent<string, string, int>
		{
		}
	}
}
