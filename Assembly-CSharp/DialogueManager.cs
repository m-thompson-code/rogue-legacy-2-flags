using System;
using System.Collections.Generic;
using System.Linq;
using RL_Windows;

// Token: 0x02000382 RID: 898
public class DialogueManager
{
	// Token: 0x17000D69 RID: 3433
	// (get) Token: 0x06001D2F RID: 7471 RVA: 0x0000F09D File Offset: 0x0000D29D
	public static DialogueManager Instance
	{
		get
		{
			if (DialogueManager.m_instance == null)
			{
				DialogueManager.m_instance = new DialogueManager();
				DialogueManager.m_instance.Initialize();
			}
			return DialogueManager.m_instance;
		}
	}

	// Token: 0x06001D30 RID: 7472 RVA: 0x0000F0BF File Offset: 0x0000D2BF
	private void Initialize()
	{
		this.m_dialogueObjLinkedList = new LinkedList<DialogueObj>();
		this.m_carriageParsedStrings = new List<string>();
	}

	// Token: 0x06001D31 RID: 7473 RVA: 0x0009BE48 File Offset: 0x0009A048
	private void ForceRefreshText_Internal()
	{
		if (!string.IsNullOrEmpty(DialogueManager.LastTextLocID))
		{
			DialogueObj dialogueObj = this.m_dialogueObjLinkedList.First<DialogueObj>();
			DialoguePortraitType portraitType = dialogueObj.PortraitType;
			float typewriterSpeed = dialogueObj.typewriterSpeed;
			DialogueWindowStyle windowStyle = dialogueObj.WindowStyle;
			NPCState npcstate = dialogueObj.NPCState;
			this.m_dialogueObjLinkedList.Clear();
			string @string = LocalizationManager.GetString(DialogueManager.LastSpeakerLocID, DialogueManager.LastIsFemaleState, false);
			string string2 = LocalizationManager.GetString(DialogueManager.LastTextLocID, DialogueManager.LastIsFemaleState, false);
			this.ParseCarriageReturns(string2);
			foreach (string text in this.m_carriageParsedStrings)
			{
				DialogueObj value = default(DialogueObj);
				value.speaker = @string;
				value.text = text;
				value.PortraitType = portraitType;
				value.typewriterSpeed = typewriterSpeed;
				value.WindowStyle = windowStyle;
				value.NPCState = npcstate;
				this.m_dialogueObjLinkedList.AddLast(value);
			}
		}
	}

	// Token: 0x06001D32 RID: 7474 RVA: 0x0009BF4C File Offset: 0x0009A14C
	private void AddDialogue_Internal(string speakerLocID, string textLocID, bool isFemale, DialogueWindowStyle windowStyle, DialoguePortraitType portraitType, NPCState npcState, float typewriteSpeed)
	{
		string @string = LocalizationManager.GetString(textLocID, isFemale, false);
		string string2 = LocalizationManager.GetString(speakerLocID, isFemale, false);
		this.ParseCarriageReturns(@string);
		this.m_lastSpeakerLocID = speakerLocID;
		this.m_lastTextLocID = textLocID;
		this.m_lastIsFemale = isFemale;
		foreach (string text in this.m_carriageParsedStrings)
		{
			DialogueObj value = default(DialogueObj);
			value.speaker = string2;
			value.text = text;
			value.PortraitType = portraitType;
			value.typewriterSpeed = typewriteSpeed;
			value.WindowStyle = windowStyle;
			value.NPCState = npcState;
			this.m_dialogueObjLinkedList.AddLast(value);
		}
	}

	// Token: 0x06001D33 RID: 7475 RVA: 0x0009C010 File Offset: 0x0009A210
	private void AddNonLocDialogue_Internal(string speakerTitle, string text, bool disableWordScramble, DialogueWindowStyle windowStyle, DialoguePortraitType portraitType, NPCState npcState, float typewriteSpeed)
	{
		text = text.Replace("</LB>", "\n");
		this.ParseCarriageReturns(text);
		this.m_lastSpeakerLocID = null;
		this.m_lastTextLocID = null;
		this.m_lastIsFemale = false;
		foreach (string text2 in this.m_carriageParsedStrings)
		{
			DialogueObj value = default(DialogueObj);
			string text3 = text2;
			value.speaker = speakerTitle;
			value.text = text3;
			value.PortraitType = portraitType;
			value.typewriterSpeed = typewriteSpeed;
			value.WindowStyle = windowStyle;
			value.NPCState = npcState;
			this.m_dialogueObjLinkedList.AddLast(value);
		}
	}

	// Token: 0x06001D34 RID: 7476 RVA: 0x0000F0D7 File Offset: 0x0000D2D7
	private void ClearLinkedList()
	{
		this.m_dialogueObjLinkedList.Clear();
	}

	// Token: 0x06001D35 RID: 7477 RVA: 0x0009C0D4 File Offset: 0x0009A2D4
	private void ParseCarriageReturns(string text)
	{
		this.m_carriageParsedStrings.Clear();
		int length = "</CR>".Length;
		int num = 0;
		while (num != -1)
		{
			num = text.IndexOf("</CR>");
			if (num != -1)
			{
				string item = text.Substring(0, num);
				this.m_carriageParsedStrings.Add(item);
				text = text.Substring(num + length).TrimStart(Array.Empty<char>());
			}
		}
		this.m_carriageParsedStrings.Add(text);
	}

	// Token: 0x17000D6A RID: 3434
	// (get) Token: 0x06001D36 RID: 7478 RVA: 0x0000F0E4 File Offset: 0x0000D2E4
	public static NPCState OnCompleteNPCState
	{
		get
		{
			return DialogueManager.Instance.m_onCompleteNPCState;
		}
	}

	// Token: 0x17000D6B RID: 3435
	// (get) Token: 0x06001D37 RID: 7479 RVA: 0x0000F0F0 File Offset: 0x0000D2F0
	public static NPCController NPCController
	{
		get
		{
			return DialogueManager.Instance.m_npcController;
		}
	}

	// Token: 0x17000D6C RID: 3436
	// (get) Token: 0x06001D38 RID: 7480 RVA: 0x0000F0FC File Offset: 0x0000D2FC
	public static bool IsNPCDialogue
	{
		get
		{
			return DialogueManager.Instance.m_isNPCDialogue;
		}
	}

	// Token: 0x17000D6D RID: 3437
	// (get) Token: 0x06001D39 RID: 7481 RVA: 0x0000F108 File Offset: 0x0000D308
	public static string LastSpeakerLocID
	{
		get
		{
			return DialogueManager.Instance.m_lastSpeakerLocID;
		}
	}

	// Token: 0x17000D6E RID: 3438
	// (get) Token: 0x06001D3A RID: 7482 RVA: 0x0000F114 File Offset: 0x0000D314
	public static string LastTextLocID
	{
		get
		{
			return DialogueManager.Instance.m_lastTextLocID;
		}
	}

	// Token: 0x17000D6F RID: 3439
	// (get) Token: 0x06001D3B RID: 7483 RVA: 0x0000F120 File Offset: 0x0000D320
	public static bool LastIsFemaleState
	{
		get
		{
			return DialogueManager.Instance.m_lastIsFemale;
		}
	}

	// Token: 0x17000D70 RID: 3440
	// (get) Token: 0x06001D3C RID: 7484 RVA: 0x0000F12C File Offset: 0x0000D32C
	public static LinkedList<DialogueObj> DialogueObjLinkedList
	{
		get
		{
			return DialogueManager.Instance.m_dialogueObjLinkedList;
		}
	}

	// Token: 0x17000D71 RID: 3441
	// (get) Token: 0x06001D3D RID: 7485 RVA: 0x0000F138 File Offset: 0x0000D338
	public static LinkedListNode<DialogueObj> GetFirstDialogueNode
	{
		get
		{
			return DialogueManager.Instance.m_dialogueObjLinkedList.First;
		}
	}

	// Token: 0x17000D72 RID: 3442
	// (get) Token: 0x06001D3E RID: 7486 RVA: 0x0000F149 File Offset: 0x0000D349
	public static LinkedListNode<DialogueObj> GetLastDialogueNode
	{
		get
		{
			return DialogueManager.Instance.m_dialogueObjLinkedList.Last;
		}
	}

	// Token: 0x06001D3F RID: 7487 RVA: 0x0009C148 File Offset: 0x0009A348
	public static LinkedListNode<DialogueObj> GetDialogueNodeAtIndex(int index)
	{
		if (index >= DialogueManager.Instance.m_dialogueObjLinkedList.Count)
		{
			return null;
		}
		LinkedListNode<DialogueObj> linkedListNode = DialogueManager.GetFirstDialogueNode;
		for (int i = 0; i < index; i++)
		{
			linkedListNode = linkedListNode.Next;
		}
		return linkedListNode;
	}

	// Token: 0x06001D40 RID: 7488 RVA: 0x0000F15A File Offset: 0x0000D35A
	public static void StartNewNPCDialogue(NPCController npcController, NPCState onCompleteNPCState = NPCState.Idle)
	{
		DialogueManager.StartNewDialogue(npcController, onCompleteNPCState);
		DialogueManager.Instance.m_isNPCDialogue = true;
	}

	// Token: 0x06001D41 RID: 7489 RVA: 0x0000F16E File Offset: 0x0000D36E
	public static void StartNewDialogue(NPCController npcController = null, NPCState onCompleteNPCState = NPCState.Idle)
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.Dialogue))
		{
			WindowManager.LoadWindow(WindowID.Dialogue);
		}
		DialogueManager.Instance.m_npcController = npcController;
		DialogueManager.Instance.m_onCompleteNPCState = onCompleteNPCState;
		DialogueManager.Instance.ClearLinkedList();
		DialogueManager.Instance.m_isNPCDialogue = false;
	}

	// Token: 0x06001D42 RID: 7490 RVA: 0x0009C184 File Offset: 0x0009A384
	public static void UpdateNPCDialogueState()
	{
		if (DialogueManager.Instance.m_npcController)
		{
			NPCDialogueManager.MarkNPCAsSpoken(DialogueManager.Instance.m_npcController.NPCType, DialogueManager.Instance.m_isNPCDialogue, DialogueManager.Instance.m_npcController);
		}
		DialogueManager.Instance.m_npcController = null;
		DialogueManager.Instance.m_isNPCDialogue = false;
	}

	// Token: 0x06001D43 RID: 7491 RVA: 0x0000F1A9 File Offset: 0x0000D3A9
	public static void AddDialogue(string speakerLocID, string textLocID, bool isFemale, DialogueWindowStyle windowStyle = DialogueWindowStyle.HorizontalUpper, DialoguePortraitType portraitType = DialoguePortraitType.None, NPCState npcState = NPCState.None, float typewriterSpeed = 0.015f)
	{
		DialogueManager.Instance.AddDialogue_Internal(speakerLocID, textLocID, isFemale, windowStyle, portraitType, npcState, typewriterSpeed);
	}

	// Token: 0x06001D44 RID: 7492 RVA: 0x0000F1BF File Offset: 0x0000D3BF
	public static void AddNonLocDialogue(string speaker, string text, bool disableWordScramble = false, DialogueWindowStyle windowStyle = DialogueWindowStyle.HorizontalUpper, DialoguePortraitType portraitType = DialoguePortraitType.None, NPCState npcState = NPCState.None, float typeWriterSpeed = 0.015f)
	{
		DialogueManager.Instance.AddNonLocDialogue_Internal(speaker, text, disableWordScramble, windowStyle, portraitType, npcState, typeWriterSpeed);
	}

	// Token: 0x06001D45 RID: 7493 RVA: 0x0000F1D5 File Offset: 0x0000D3D5
	public static void AddDialogueCompleteEndHandler(Action action)
	{
		(WindowManager.GetWindowController(WindowID.Dialogue) as DialogueWindowController).AddEndHandler(action);
	}

	// Token: 0x06001D46 RID: 7494 RVA: 0x0000F1E8 File Offset: 0x0000D3E8
	public static void ForceRefreshText()
	{
		DialogueManager.Instance.ForceRefreshText_Internal();
	}

	// Token: 0x04001AA9 RID: 6825
	private static DialogueManager m_instance;

	// Token: 0x04001AAA RID: 6826
	private const string CARRIAGE_RETURN = "</CR>";

	// Token: 0x04001AAB RID: 6827
	private const string LINE_BREAK = "</LB>";

	// Token: 0x04001AAC RID: 6828
	private LinkedList<DialogueObj> m_dialogueObjLinkedList;

	// Token: 0x04001AAD RID: 6829
	private List<string> m_carriageParsedStrings;

	// Token: 0x04001AAE RID: 6830
	private NPCController m_npcController;

	// Token: 0x04001AAF RID: 6831
	private NPCState m_onCompleteNPCState;

	// Token: 0x04001AB0 RID: 6832
	private bool m_isNPCDialogue;

	// Token: 0x04001AB1 RID: 6833
	private string m_lastSpeakerLocID;

	// Token: 0x04001AB2 RID: 6834
	private string m_lastTextLocID;

	// Token: 0x04001AB3 RID: 6835
	private bool m_lastIsFemale;
}
