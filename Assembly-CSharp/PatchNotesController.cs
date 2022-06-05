using System;
using System.Collections;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x020003D1 RID: 977
public class PatchNotesController : MonoBehaviour
{
	// Token: 0x060023FE RID: 9214 RVA: 0x00075E44 File Offset: 0x00074044
	private void Awake()
	{
		this.m_scrollArrow.SetActive(false);
		this.m_raycastBlocker.SetActive(false);
		this.m_onCancelButtonPressed = new Action<InputActionEventData>(this.OnCancelButtonPressed);
	}

	// Token: 0x060023FF RID: 9215 RVA: 0x00075E70 File Offset: 0x00074070
	private void Start()
	{
	}

	// Token: 0x06002400 RID: 9216 RVA: 0x00075E74 File Offset: 0x00074074
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

	// Token: 0x06002401 RID: 9217 RVA: 0x00075F11 File Offset: 0x00074111
	private void OnCancelButtonPressed(InputActionEventData eventData)
	{
		this.m_mainWindow.SetEnablePatchNotes(false);
	}

	// Token: 0x06002402 RID: 9218 RVA: 0x00075F1F File Offset: 0x0007411F
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

	// Token: 0x06002403 RID: 9219 RVA: 0x00075F30 File Offset: 0x00074130
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

	// Token: 0x04001E88 RID: 7816
	[SerializeField]
	private TMP_Text m_text;

	// Token: 0x04001E89 RID: 7817
	[SerializeField]
	private GameObject m_scrollArrow;

	// Token: 0x04001E8A RID: 7818
	[SerializeField]
	private GameObject m_raycastBlocker;

	// Token: 0x04001E8B RID: 7819
	[SerializeField]
	private ScrollBarInput_RL m_scrollInput;

	// Token: 0x04001E8C RID: 7820
	[SerializeField]
	private MainMenuWindowController m_mainWindow;

	// Token: 0x04001E8D RID: 7821
	[SerializeField]
	private TextAsset m_testTextFile;

	// Token: 0x04001E8E RID: 7822
	private Action<InputActionEventData> m_onCancelButtonPressed;
}
