using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000CFA RID: 3322
public class ChatController : MonoBehaviour
{
	// Token: 0x06005EC2 RID: 24258 RVA: 0x000343B8 File Offset: 0x000325B8
	private void OnEnable()
	{
		this.TMP_ChatInput.onSubmit.AddListener(new UnityAction<string>(this.AddToChatOutput));
	}

	// Token: 0x06005EC3 RID: 24259 RVA: 0x000343D6 File Offset: 0x000325D6
	private void OnDisable()
	{
		this.TMP_ChatInput.onSubmit.RemoveListener(new UnityAction<string>(this.AddToChatOutput));
	}

	// Token: 0x06005EC4 RID: 24260 RVA: 0x00162DEC File Offset: 0x00160FEC
	private void AddToChatOutput(string newText)
	{
		this.TMP_ChatInput.text = string.Empty;
		DateTime now = DateTime.Now;
		TMP_Text tmp_ChatOutput = this.TMP_ChatOutput;
		tmp_ChatOutput.text = string.Concat(new string[]
		{
			tmp_ChatOutput.text,
			"[<#FFFF80>",
			now.Hour.ToString("d2"),
			":",
			now.Minute.ToString("d2"),
			":",
			now.Second.ToString("d2"),
			"</color>] ",
			newText,
			"\n"
		});
		this.TMP_ChatInput.ActivateInputField();
		this.ChatScrollbar.value = 0f;
	}

	// Token: 0x04004DCF RID: 19919
	public TMP_InputField TMP_ChatInput;

	// Token: 0x04004DD0 RID: 19920
	public TMP_Text TMP_ChatOutput;

	// Token: 0x04004DD1 RID: 19921
	public Scrollbar ChatScrollbar;
}
