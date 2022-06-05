using System;
using System.Collections.Generic;
using System.Linq;
using RL_Windows;

// Token: 0x020001EA RID: 490
public class DialogueManager
{
	// Token: 0x17000A6B RID: 2667
	// (get) Token: 0x0600141A RID: 5146 RVA: 0x0003D047 File Offset: 0x0003B247
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

	// Token: 0x0600141B RID: 5147 RVA: 0x0003D069 File Offset: 0x0003B269
	private void Initialize()
	{
		this.m_dialogueObjLinkedList = new LinkedList<DialogueObj>();
		this.m_carriageParsedStrings = new List<string>();
	}

	// Token: 0x0600141C RID: 5148 RVA: 0x0003D084 File Offset: 0x0003B284
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

	// Token: 0x0600141D RID: 5149 RVA: 0x0003D188 File Offset: 0x0003B388
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

	// Token: 0x0600141E RID: 5150 RVA: 0x0003D24C File Offset: 0x0003B44C
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

	// Token: 0x0600141F RID: 5151 RVA: 0x0003D310 File Offset: 0x0003B510
	private void ClearLinkedList()
	{
		this.m_dialogueObjLinkedList.Clear();
	}

	// Token: 0x06001420 RID: 5152 RVA: 0x0003D320 File Offset: 0x0003B520
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

	// Token: 0x17000A6C RID: 2668
	// (get) Token: 0x06001421 RID: 5153 RVA: 0x0003D391 File Offset: 0x0003B591
	public static NPCState OnCompleteNPCState
	{
		get
		{
			return DialogueManager.Instance.m_onCompleteNPCState;
		}
	}

	// Token: 0x17000A6D RID: 2669
	// (get) Token: 0x06001422 RID: 5154 RVA: 0x0003D39D File Offset: 0x0003B59D
	public static NPCController NPCController
	{
		get
		{
			return DialogueManager.Instance.m_npcController;
		}
	}

	// Token: 0x17000A6E RID: 2670
	// (get) Token: 0x06001423 RID: 5155 RVA: 0x0003D3A9 File Offset: 0x0003B5A9
	public static bool IsNPCDialogue
	{
		get
		{
			return DialogueManager.Instance.m_isNPCDialogue;
		}
	}

	// Token: 0x17000A6F RID: 2671
	// (get) Token: 0x06001424 RID: 5156 RVA: 0x0003D3B5 File Offset: 0x0003B5B5
	public static string LastSpeakerLocID
	{
		get
		{
			return DialogueManager.Instance.m_lastSpeakerLocID;
		}
	}

	// Token: 0x17000A70 RID: 2672
	// (get) Token: 0x06001425 RID: 5157 RVA: 0x0003D3C1 File Offset: 0x0003B5C1
	public static string LastTextLocID
	{
		get
		{
			return DialogueManager.Instance.m_lastTextLocID;
		}
	}

	// Token: 0x17000A71 RID: 2673
	// (get) Token: 0x06001426 RID: 5158 RVA: 0x0003D3CD File Offset: 0x0003B5CD
	public static bool LastIsFemaleState
	{
		get
		{
			return DialogueManager.Instance.m_lastIsFemale;
		}
	}

	// Token: 0x17000A72 RID: 2674
	// (get) Token: 0x06001427 RID: 5159 RVA: 0x0003D3D9 File Offset: 0x0003B5D9
	public static LinkedList<DialogueObj> DialogueObjLinkedList
	{
		get
		{
			return DialogueManager.Instance.m_dialogueObjLinkedList;
		}
	}

	// Token: 0x17000A73 RID: 2675
	// (get) Token: 0x06001428 RID: 5160 RVA: 0x0003D3E5 File Offset: 0x0003B5E5
	public static LinkedListNode<DialogueObj> GetFirstDialogueNode
	{
		get
		{
			return DialogueManager.Instance.m_dialogueObjLinkedList.First;
		}
	}

	// Token: 0x17000A74 RID: 2676
	// (get) Token: 0x06001429 RID: 5161 RVA: 0x0003D3F6 File Offset: 0x0003B5F6
	public static LinkedListNode<DialogueObj> GetLastDialogueNode
	{
		get
		{
			return DialogueManager.Instance.m_dialogueObjLinkedList.Last;
		}
	}

	// Token: 0x0600142A RID: 5162 RVA: 0x0003D408 File Offset: 0x0003B608
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

	// Token: 0x0600142B RID: 5163 RVA: 0x0003D443 File Offset: 0x0003B643
	public static void StartNewNPCDialogue(NPCController npcController, NPCState onCompleteNPCState = NPCState.Idle)
	{
		DialogueManager.StartNewDialogue(npcController, onCompleteNPCState);
		DialogueManager.Instance.m_isNPCDialogue = true;
	}

	// Token: 0x0600142C RID: 5164 RVA: 0x0003D457 File Offset: 0x0003B657
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

	// Token: 0x0600142D RID: 5165 RVA: 0x0003D494 File Offset: 0x0003B694
	public static void UpdateNPCDialogueState()
	{
		if (DialogueManager.Instance.m_npcController)
		{
			NPCDialogueManager.MarkNPCAsSpoken(DialogueManager.Instance.m_npcController.NPCType, DialogueManager.Instance.m_isNPCDialogue, DialogueManager.Instance.m_npcController);
		}
		DialogueManager.Instance.m_npcController = null;
		DialogueManager.Instance.m_isNPCDialogue = false;
	}

	// Token: 0x0600142E RID: 5166 RVA: 0x0003D4F0 File Offset: 0x0003B6F0
	public static void AddDialogue(string speakerLocID, string textLocID, bool isFemale, DialogueWindowStyle windowStyle = DialogueWindowStyle.HorizontalUpper, DialoguePortraitType portraitType = DialoguePortraitType.None, NPCState npcState = NPCState.None, float typewriterSpeed = 0.015f)
	{
		DialogueManager.Instance.AddDialogue_Internal(speakerLocID, textLocID, isFemale, windowStyle, portraitType, npcState, typewriterSpeed);
	}

	// Token: 0x0600142F RID: 5167 RVA: 0x0003D506 File Offset: 0x0003B706
	public static void AddNonLocDialogue(string speaker, string text, bool disableWordScramble = false, DialogueWindowStyle windowStyle = DialogueWindowStyle.HorizontalUpper, DialoguePortraitType portraitType = DialoguePortraitType.None, NPCState npcState = NPCState.None, float typeWriterSpeed = 0.015f)
	{
		DialogueManager.Instance.AddNonLocDialogue_Internal(speaker, text, disableWordScramble, windowStyle, portraitType, npcState, typeWriterSpeed);
	}

	// Token: 0x06001430 RID: 5168 RVA: 0x0003D51C File Offset: 0x0003B71C
	public static void AddDialogueCompleteEndHandler(Action action)
	{
		(WindowManager.GetWindowController(WindowID.Dialogue) as DialogueWindowController).AddEndHandler(action);
	}

	// Token: 0x06001431 RID: 5169 RVA: 0x0003D52F File Offset: 0x0003B72F
	public static void ForceRefreshText()
	{
		DialogueManager.Instance.ForceRefreshText_Internal();
	}

	// Token: 0x04001409 RID: 5129
	private static DialogueManager m_instance;

	// Token: 0x0400140A RID: 5130
	private const string CARRIAGE_RETURN = "</CR>";

	// Token: 0x0400140B RID: 5131
	private const string LINE_BREAK = "</LB>";

	// Token: 0x0400140C RID: 5132
	private LinkedList<DialogueObj> m_dialogueObjLinkedList;

	// Token: 0x0400140D RID: 5133
	private List<string> m_carriageParsedStrings;

	// Token: 0x0400140E RID: 5134
	private NPCController m_npcController;

	// Token: 0x0400140F RID: 5135
	private NPCState m_onCompleteNPCState;

	// Token: 0x04001410 RID: 5136
	private bool m_isNPCDialogue;

	// Token: 0x04001411 RID: 5137
	private string m_lastSpeakerLocID;

	// Token: 0x04001412 RID: 5138
	private string m_lastTextLocID;

	// Token: 0x04001413 RID: 5139
	private bool m_lastIsFemale;
}
