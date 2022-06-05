using System;
using UnityEngine;
using UnityEngine.Events;

namespace TMPro.Examples
{
	// Token: 0x02000D6C RID: 3436
	public class TMP_TextEventCheck : MonoBehaviour
	{
		// Token: 0x060061C1 RID: 25025 RVA: 0x0016BEA0 File Offset: 0x0016A0A0
		private void OnEnable()
		{
			if (this.TextEventHandler != null)
			{
				this.TextEventHandler.onCharacterSelection.AddListener(new UnityAction<char, int>(this.OnCharacterSelection));
				this.TextEventHandler.onSpriteSelection.AddListener(new UnityAction<char, int>(this.OnSpriteSelection));
				this.TextEventHandler.onWordSelection.AddListener(new UnityAction<string, int, int>(this.OnWordSelection));
				this.TextEventHandler.onLineSelection.AddListener(new UnityAction<string, int, int>(this.OnLineSelection));
				this.TextEventHandler.onLinkSelection.AddListener(new UnityAction<string, string, int>(this.OnLinkSelection));
			}
		}

		// Token: 0x060061C2 RID: 25026 RVA: 0x0016BF4C File Offset: 0x0016A14C
		private void OnDisable()
		{
			if (this.TextEventHandler != null)
			{
				this.TextEventHandler.onCharacterSelection.RemoveListener(new UnityAction<char, int>(this.OnCharacterSelection));
				this.TextEventHandler.onSpriteSelection.RemoveListener(new UnityAction<char, int>(this.OnSpriteSelection));
				this.TextEventHandler.onWordSelection.RemoveListener(new UnityAction<string, int, int>(this.OnWordSelection));
				this.TextEventHandler.onLineSelection.RemoveListener(new UnityAction<string, int, int>(this.OnLineSelection));
				this.TextEventHandler.onLinkSelection.RemoveListener(new UnityAction<string, string, int>(this.OnLinkSelection));
			}
		}

		// Token: 0x060061C3 RID: 25027 RVA: 0x00035DB2 File Offset: 0x00033FB2
		private void OnCharacterSelection(char c, int index)
		{
			Debug.Log(string.Concat(new string[]
			{
				"Character [",
				c.ToString(),
				"] at Index: ",
				index.ToString(),
				" has been selected."
			}));
		}

		// Token: 0x060061C4 RID: 25028 RVA: 0x00035DF0 File Offset: 0x00033FF0
		private void OnSpriteSelection(char c, int index)
		{
			Debug.Log(string.Concat(new string[]
			{
				"Sprite [",
				c.ToString(),
				"] at Index: ",
				index.ToString(),
				" has been selected."
			}));
		}

		// Token: 0x060061C5 RID: 25029 RVA: 0x0016BFF8 File Offset: 0x0016A1F8
		private void OnWordSelection(string word, int firstCharacterIndex, int length)
		{
			Debug.Log(string.Concat(new string[]
			{
				"Word [",
				word,
				"] with first character index of ",
				firstCharacterIndex.ToString(),
				" and length of ",
				length.ToString(),
				" has been selected."
			}));
		}

		// Token: 0x060061C6 RID: 25030 RVA: 0x0016C050 File Offset: 0x0016A250
		private void OnLineSelection(string lineText, int firstCharacterIndex, int length)
		{
			Debug.Log(string.Concat(new string[]
			{
				"Line [",
				lineText,
				"] with first character index of ",
				firstCharacterIndex.ToString(),
				" and length of ",
				length.ToString(),
				" has been selected."
			}));
		}

		// Token: 0x060061C7 RID: 25031 RVA: 0x0016C0A8 File Offset: 0x0016A2A8
		private void OnLinkSelection(string linkID, string linkText, int linkIndex)
		{
			Debug.Log(string.Concat(new string[]
			{
				"Link Index: ",
				linkIndex.ToString(),
				" with ID [",
				linkID,
				"] and Text \"",
				linkText,
				"\" has been selected."
			}));
		}

		// Token: 0x04004FB7 RID: 20407
		public TMP_TextEventHandler TextEventHandler;
	}
}
