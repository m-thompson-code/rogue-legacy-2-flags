using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TMPro
{
	// Token: 0x02000D52 RID: 3410
	public class TMP_TextEventHandler : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x17001FBC RID: 8124
		// (get) Token: 0x06006169 RID: 24937 RVA: 0x00035AC3 File Offset: 0x00033CC3
		// (set) Token: 0x0600616A RID: 24938 RVA: 0x00035ACB File Offset: 0x00033CCB
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

		// Token: 0x17001FBD RID: 8125
		// (get) Token: 0x0600616B RID: 24939 RVA: 0x00035AD4 File Offset: 0x00033CD4
		// (set) Token: 0x0600616C RID: 24940 RVA: 0x00035ADC File Offset: 0x00033CDC
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

		// Token: 0x17001FBE RID: 8126
		// (get) Token: 0x0600616D RID: 24941 RVA: 0x00035AE5 File Offset: 0x00033CE5
		// (set) Token: 0x0600616E RID: 24942 RVA: 0x00035AED File Offset: 0x00033CED
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

		// Token: 0x17001FBF RID: 8127
		// (get) Token: 0x0600616F RID: 24943 RVA: 0x00035AF6 File Offset: 0x00033CF6
		// (set) Token: 0x06006170 RID: 24944 RVA: 0x00035AFE File Offset: 0x00033CFE
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

		// Token: 0x17001FC0 RID: 8128
		// (get) Token: 0x06006171 RID: 24945 RVA: 0x00035B07 File Offset: 0x00033D07
		// (set) Token: 0x06006172 RID: 24946 RVA: 0x00035B0F File Offset: 0x00033D0F
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

		// Token: 0x06006173 RID: 24947 RVA: 0x00169CBC File Offset: 0x00167EBC
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

		// Token: 0x06006174 RID: 24948 RVA: 0x00169D48 File Offset: 0x00167F48
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

		// Token: 0x06006175 RID: 24949 RVA: 0x00002FCA File Offset: 0x000011CA
		public void OnPointerEnter(PointerEventData eventData)
		{
		}

		// Token: 0x06006176 RID: 24950 RVA: 0x00002FCA File Offset: 0x000011CA
		public void OnPointerExit(PointerEventData eventData)
		{
		}

		// Token: 0x06006177 RID: 24951 RVA: 0x00035B18 File Offset: 0x00033D18
		private void SendOnCharacterSelection(char character, int characterIndex)
		{
			if (this.onCharacterSelection != null)
			{
				this.onCharacterSelection.Invoke(character, characterIndex);
			}
		}

		// Token: 0x06006178 RID: 24952 RVA: 0x00035B2F File Offset: 0x00033D2F
		private void SendOnSpriteSelection(char character, int characterIndex)
		{
			if (this.onSpriteSelection != null)
			{
				this.onSpriteSelection.Invoke(character, characterIndex);
			}
		}

		// Token: 0x06006179 RID: 24953 RVA: 0x00035B46 File Offset: 0x00033D46
		private void SendOnWordSelection(string word, int charIndex, int length)
		{
			if (this.onWordSelection != null)
			{
				this.onWordSelection.Invoke(word, charIndex, length);
			}
		}

		// Token: 0x0600617A RID: 24954 RVA: 0x00035B5E File Offset: 0x00033D5E
		private void SendOnLineSelection(string line, int charIndex, int length)
		{
			if (this.onLineSelection != null)
			{
				this.onLineSelection.Invoke(line, charIndex, length);
			}
		}

		// Token: 0x0600617B RID: 24955 RVA: 0x00035B76 File Offset: 0x00033D76
		private void SendOnLinkSelection(string linkID, string linkText, int linkIndex)
		{
			if (this.onLinkSelection != null)
			{
				this.onLinkSelection.Invoke(linkID, linkText, linkIndex);
			}
		}

		// Token: 0x04004F2E RID: 20270
		[SerializeField]
		private TMP_TextEventHandler.CharacterSelectionEvent m_OnCharacterSelection = new TMP_TextEventHandler.CharacterSelectionEvent();

		// Token: 0x04004F2F RID: 20271
		[SerializeField]
		private TMP_TextEventHandler.SpriteSelectionEvent m_OnSpriteSelection = new TMP_TextEventHandler.SpriteSelectionEvent();

		// Token: 0x04004F30 RID: 20272
		[SerializeField]
		private TMP_TextEventHandler.WordSelectionEvent m_OnWordSelection = new TMP_TextEventHandler.WordSelectionEvent();

		// Token: 0x04004F31 RID: 20273
		[SerializeField]
		private TMP_TextEventHandler.LineSelectionEvent m_OnLineSelection = new TMP_TextEventHandler.LineSelectionEvent();

		// Token: 0x04004F32 RID: 20274
		[SerializeField]
		private TMP_TextEventHandler.LinkSelectionEvent m_OnLinkSelection = new TMP_TextEventHandler.LinkSelectionEvent();

		// Token: 0x04004F33 RID: 20275
		private TMP_Text m_TextComponent;

		// Token: 0x04004F34 RID: 20276
		private Camera m_Camera;

		// Token: 0x04004F35 RID: 20277
		private Canvas m_Canvas;

		// Token: 0x04004F36 RID: 20278
		private int m_selectedLink = -1;

		// Token: 0x04004F37 RID: 20279
		private int m_lastCharIndex = -1;

		// Token: 0x04004F38 RID: 20280
		private int m_lastWordIndex = -1;

		// Token: 0x04004F39 RID: 20281
		private int m_lastLineIndex = -1;

		// Token: 0x02000D53 RID: 3411
		[Serializable]
		public class CharacterSelectionEvent : UnityEvent<char, int>
		{
		}

		// Token: 0x02000D54 RID: 3412
		[Serializable]
		public class SpriteSelectionEvent : UnityEvent<char, int>
		{
		}

		// Token: 0x02000D55 RID: 3413
		[Serializable]
		public class WordSelectionEvent : UnityEvent<string, int, int>
		{
		}

		// Token: 0x02000D56 RID: 3414
		[Serializable]
		public class LineSelectionEvent : UnityEvent<string, int, int>
		{
		}

		// Token: 0x02000D57 RID: 3415
		[Serializable]
		public class LinkSelectionEvent : UnityEvent<string, string, int>
		{
		}
	}
}
