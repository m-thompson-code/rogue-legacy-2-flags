using System;
using System.Collections;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x0200066A RID: 1642
public class PatchNotesController : MonoBehaviour
{
	// Token: 0x0600321C RID: 12828 RVA: 0x0001B82F File Offset: 0x00019A2F
	private void Awake()
	{
		this.m_scrollArrow.SetActive(false);
		this.m_raycastBlocker.SetActive(false);
		this.m_onCancelButtonPressed = new Action<InputActionEventData>(this.OnCancelButtonPressed);
	}

	// Token: 0x0600321D RID: 12829 RVA: 0x00002FCA File Offset: 0x000011CA
	private void Start()
	{
	}

	// Token: 0x0600321E RID: 12830 RVA: 0x000D5D4C File Offset: 0x000D3F4C
	public void SetEnablePatchNotes(bool enable)
	{
		this.m_scrollArrow.SetActive(enable);
		this.m_raycastBlocker.SetActive(enable);
		if (enable)
		{
			Rewired_RL.Player.AddInputEventDelegate(this.m_onCancelButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
			Rewired_RL.Player.AddInputEventDelegate(this.m_onCancelButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
			this.m_scrollInput.AssignButtonToScroll(Rewired_RL.WindowInputActionType.Window_Vertical);
			return;
		}
		Rewired_RL.Player.RemoveInputEventDelegate(this.m_onCancelButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		Rewired_RL.Player.RemoveInputEventDelegate(this.m_onCancelButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		this.m_scrollInput.RemoveButtonToScroll(Rewired_RL.WindowInputActionType.Window_Vertical);
	}

	// Token: 0x0600321F RID: 12831 RVA: 0x0001B85B File Offset: 0x00019A5B
	private void OnCancelButtonPressed(InputActionEventData eventData)
	{
		this.m_mainWindow.SetEnablePatchNotes(false);
	}

	// Token: 0x06003220 RID: 12832 RVA: 0x0001B869 File Offset: 0x00019A69
	private IEnumerator WWWRequestPatchNotes()
	{
		UnityWebRequest webRequest = UnityWebRequest.Get("URL");
		yield return webRequest.SendWebRequest();
		if (webRequest.error != null)
		{
			Debug.Log("Error .. " + webRequest.error);
		}
		else
		{
			string text = webRequest.downloadHandler.text;
			Debug.Log("Found ... ==>" + text + "<==");
			this.ParsePatchNoteText(text);
		}
		yield break;
	}

	// Token: 0x06003221 RID: 12833 RVA: 0x000D5DEC File Offset: 0x000D3FEC
	private void ParsePatchNoteText(string textToParse)
	{
		string text = "";
		foreach (string text2 in textToParse.Split(new string[]
		{
			"\r\n"
		}, StringSplitOptions.None))
		{
			if (text2 != null)
			{
				int num = text2.IndexOf("=");
				if (num != -1)
				{
					string text3 = text2.Substring(0, num).ToLower();
					string text4 = text2.Substring(num + 1);
					if (text3 != null)
					{
						if (text3 == "title")
						{
							text = text + "<size=120%>" + text4 + "</size>";
							goto IL_BA;
						}
						if (text3 == "bullet")
						{
							text = text + "• <indent=5%>" + text4 + "</indent>";
							goto IL_BA;
						}
					}
					text += text4;
				}
				else
				{
					text += text2;
				}
				IL_BA:
				text += "\n";
			}
		}
		this.m_text.text = text;
	}

	// Token: 0x040028C6 RID: 10438
	[SerializeField]
	private TMP_Text m_text;

	// Token: 0x040028C7 RID: 10439
	[SerializeField]
	private GameObject m_scrollArrow;

	// Token: 0x040028C8 RID: 10440
	[SerializeField]
	private GameObject m_raycastBlocker;

	// Token: 0x040028C9 RID: 10441
	[SerializeField]
	private ScrollBarInput_RL m_scrollInput;

	// Token: 0x040028CA RID: 10442
	[SerializeField]
	private MainMenuWindowController m_mainWindow;

	// Token: 0x040028CB RID: 10443
	[SerializeField]
	private TextAsset m_testTextFile;

	// Token: 0x040028CC RID: 10444
	private Action<InputActionEventData> m_onCancelButtonPressed;
}
