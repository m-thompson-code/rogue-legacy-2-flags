using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000822 RID: 2082
public class ChatController : MonoBehaviour
{
	// Token: 0x060044F0 RID: 17648 RVA: 0x000F5203 File Offset: 0x000F3403
	private void OnEnable()
	{
		this.TMP_ChatInput.onSubmit.AddListener(new UnityAction<string>(this.AddToChatOutput));
	}

	// Token: 0x060044F1 RID: 17649 RVA: 0x000F5221 File Offset: 0x000F3421
	private void OnDisable()
	{
		this.TMP_ChatInput.onSubmit.RemoveListener(new UnityAction<string>(this.AddToChatOutput));
	}

	// Token: 0x060044F2 RID: 17650 RVA: 0x000F5240 File Offset: 0x000F3440
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

	// Token: 0x04003AC7 RID: 15047
	public TMP_InputField TMP_ChatInput;

	// Token: 0x04003AC8 RID: 15048
	public TMP_Text TMP_ChatOutput;

	// Token: 0x04003AC9 RID: 15049
	public Scrollbar ChatScrollbar;
}
