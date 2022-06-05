using System;
using System.IO;
using System.Text;
using RL_Windows;
using UnityEngine;

// Token: 0x0200081A RID: 2074
public class LQAEntryOverride : MonoBehaviour
{
	// Token: 0x06003FFC RID: 16380 RVA: 0x00023560 File Offset: 0x00021760
	private void Awake()
	{
		LQAEntryOverride.InitializeDirectoryAndFile();
		this.m_closeConfirmWindow = new Action(this.CloseConfirmWindow);
	}

	// Token: 0x06003FFD RID: 16381 RVA: 0x001002E0 File Offset: 0x000FE4E0
	public static void InitializeDirectoryAndFile()
	{
		string text = Path.Combine(Application.persistentDataPath, "CustomData");
		if (!Directory.Exists(text))
		{
			try
			{
				Directory.CreateDirectory(text);
				Debug.Log("<color=green>Created CustomData directory</color>");
			}
			catch (Exception ex)
			{
				string str = "<color=red>CustomData directory does not exist and could not be created.</color> ";
				Exception ex2 = ex;
				Debug.Log(str + ((ex2 != null) ? ex2.ToString() : null));
				return;
			}
		}
		text = Path.Combine(text, StoreAPIManager.GetPlatformDirectoryName());
		if (!Directory.Exists(text))
		{
			try
			{
				Directory.CreateDirectory(text);
				Debug.Log("<color=green>Created CustomData/Platform subdirectory</color>");
			}
			catch (Exception ex3)
			{
				string str2 = "<color=red>CustomData/Platform subdirectory does not exist and could not be created.</color> ";
				Exception ex4 = ex3;
				Debug.Log(str2 + ((ex4 != null) ? ex4.ToString() : null));
				return;
			}
		}
		string path = Path.Combine(text, "LocIDTester_V2.txt");
		if (!File.Exists(path))
		{
			using (StreamWriter streamWriter = File.CreateText(path))
			{
				for (int i = 0; i < LQAEntryOverride.TESTER_INSTRUCTIONS.Length; i++)
				{
					streamWriter.WriteLine(LQAEntryOverride.TESTER_INSTRUCTIONS[i]);
				}
			}
		}
	}

	// Token: 0x06003FFE RID: 16382 RVA: 0x001003F8 File Offset: 0x000FE5F8
	private void LoadCustomFile()
	{
		this.m_dataLoadedSuccessfully = true;
		string path = Path.Combine(Path.Combine(Path.Combine(Application.persistentDataPath, "CustomData"), StoreAPIManager.GetPlatformDirectoryName()), "LocIDTester_V2.txt");
		try
		{
			byte[] bytes = File.ReadAllBytes(path);
			using (StringReader stringReader = new StringReader(Encoding.ASCII.GetString(bytes)))
			{
				string text;
				while ((text = stringReader.ReadLine()) != null)
				{
					int num = text.IndexOf("=");
					if (num != -1)
					{
						string text2 = text.Substring(0, num);
						string text3 = text.Substring(num + 1);
						if (!string.IsNullOrEmpty(text3) && text2 != null)
						{
							uint num2 = <PrivateImplementationDetails>.ComputeStringHash(text2);
							if (num2 <= 1864400925U)
							{
								if (num2 <= 631892357U)
								{
									if (num2 != 43064917U)
									{
										if (num2 != 226912077U)
										{
											if (num2 == 631892357U)
											{
												if (text2 == "ObjComplete_DisplayPlayer")
												{
													this.m_objCompleteDisplayPlayer = bool.Parse(text3.Trim());
												}
											}
										}
										else if (text2 == "Dialogue_WindowStyle")
										{
											this.m_dialogueWindowStyle = (DialogueWindowStyle)Enum.Parse(typeof(DialogueWindowStyle), text3.Trim());
										}
									}
									else if (text2 == "ObjComplete_SubTitleLocID")
									{
										this.m_objCompleteSubTitleLocID = text3.Trim();
									}
								}
								else if (num2 != 667651751U)
								{
									if (num2 != 1166447638U)
									{
										if (num2 == 1864400925U)
										{
											if (text2 == "Confirm_TitleLocID")
											{
												this.m_confirmTitleLocID = text3.Trim();
											}
										}
									}
									else if (text2 == "Dialogue_DisplayFemale")
									{
										this.m_dialogueIsFemale = bool.Parse(text3.Trim());
									}
								}
								else if (text2 == "Confirm_BodyLocID")
								{
									this.m_confirmBodyLocID = text3.Trim();
								}
							}
							else if (num2 <= 2279091609U)
							{
								if (num2 != 1975803461U)
								{
									if (num2 != 2245102601U)
									{
										if (num2 == 2279091609U)
										{
											if (text2 == "Confirm_Button2LocID")
											{
												this.m_button2LocID = text3.Trim();
											}
										}
									}
									else if (text2 == "Dialogue_TitleLocID")
									{
										this.m_dialogueTitleLocID = text3.Trim();
									}
								}
								else if (text2 == "ObjComplete_BodyLocID")
								{
									this.m_objCompleteBodyLocID = text3.Trim();
								}
							}
							else if (num2 != 2826962430U)
							{
								if (num2 != 3048169351U)
								{
									if (num2 == 3780585803U)
									{
										if (text2 == "Dialogue_BodyLocID")
										{
											this.m_dialogueBodyLocID = text3.Trim();
										}
									}
								}
								else if (text2 == "ObjComplete_TitleLocID")
								{
									this.m_objCompleteTitleLocID = text3.Trim();
								}
							}
							else if (text2 == "Confirm_Button1LocID")
							{
								this.m_button1LocID = text3.Trim();
							}
						}
					}
				}
				Debug.Log("<color=green>Config file loaded successfully.</color>");
			}
		}
		catch (Exception ex)
		{
			Debug.Log("Could not load config file. Error: " + ex.Message);
			this.m_dataLoadedSuccessfully = false;
		}
	}

	// Token: 0x06003FFF RID: 16383 RVA: 0x001007AC File Offset: 0x000FE9AC
	public void DisplayCustomData()
	{
		this.m_dataLoadedSuccessfully = false;
		this.LoadCustomFile();
		if (this.m_dataLoadedSuccessfully)
		{
			switch (this.m_displayType)
			{
			case LQAEntryOverride.LQADisplayType.DialogueWindow:
				DialogueManager.StartNewDialogue(null, NPCState.Idle);
				DialogueManager.AddDialogue(this.m_dialogueTitleLocID, this.m_dialogueBodyLocID, this.m_dialogueIsFemale, this.m_dialogueWindowStyle, DialoguePortraitType.None, NPCState.None, 0.015f);
				WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
				return;
			case LQAEntryOverride.LQADisplayType.ConfirmWindow:
			{
				if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
				{
					WindowManager.LoadWindow(WindowID.ConfirmMenu);
				}
				ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
				confirmMenuWindowController.SetTitleText(this.m_confirmTitleLocID, true);
				confirmMenuWindowController.SetDescriptionText(this.m_confirmBodyLocID, true);
				confirmMenuWindowController.SetNumberOfButtons(2);
				confirmMenuWindowController.SetOnCancelAction(this.m_closeConfirmWindow);
				ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
				buttonAtIndex.SetButtonText(this.m_button1LocID, true);
				buttonAtIndex.SetOnClickAction(this.m_closeConfirmWindow);
				ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
				buttonAtIndex2.SetButtonText(this.m_button2LocID, true);
				buttonAtIndex2.SetOnClickAction(this.m_closeConfirmWindow);
				WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
				return;
			}
			case LQAEntryOverride.LQADisplayType.ObjectiveComplete:
			{
				LQAObjectiveCompleteHUDEventArgs eventArgs = new LQAObjectiveCompleteHUDEventArgs(5f, this.m_objCompleteTitleLocID, this.m_objCompleteSubTitleLocID, this.m_objCompleteBodyLocID, this.m_objCompleteDisplayPlayer);
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, eventArgs);
				break;
			}
			default:
				return;
			}
		}
	}

	// Token: 0x06004000 RID: 16384 RVA: 0x00013B7B File Offset: 0x00011D7B
	private void CloseConfirmWindow()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x040031FF RID: 12799
	private const string FILE_NAME = "LocIDTester_V2.txt";

	// Token: 0x04003200 RID: 12800
	private static readonly string[] TESTER_INSTRUCTIONS = new string[]
	{
		"// Change the lines below to test custom LocIDs.",
		"",
		"// Dialogue Window",
		"// WindowStyle can be: HorizontalUpper, HorizontalLower, VerticalLeft, or VerticalRight.",
		"Dialogue_TitleLocID=",
		"Dialogue_BodyLocID=",
		"Dialogue_DisplayFemale=false",
		"Dialogue_WindowStyle=HorizontalUpper",
		"",
		"// Confirm Window",
		"Confirm_TitleLocID=",
		"Confirm_BodyLocID=",
		"Confirm_Button1LocID=",
		"Confirm_Button2LocID=",
		"",
		"// Objective Complete Window",
		"ObjComplete_TitleLocID=",
		"ObjComplete_SubTitleLocID=",
		"ObjComplete_BodyLocID=",
		"ObjComplete_DisplayPlayer=false"
	};

	// Token: 0x04003201 RID: 12801
	[SerializeField]
	private LQAEntryOverride.LQADisplayType m_displayType;

	// Token: 0x04003202 RID: 12802
	private string m_dialogueTitleLocID;

	// Token: 0x04003203 RID: 12803
	private string m_dialogueBodyLocID;

	// Token: 0x04003204 RID: 12804
	private bool m_dialogueIsFemale;

	// Token: 0x04003205 RID: 12805
	private DialogueWindowStyle m_dialogueWindowStyle;

	// Token: 0x04003206 RID: 12806
	private string m_confirmTitleLocID;

	// Token: 0x04003207 RID: 12807
	private string m_confirmBodyLocID;

	// Token: 0x04003208 RID: 12808
	private string m_button1LocID;

	// Token: 0x04003209 RID: 12809
	private string m_button2LocID;

	// Token: 0x0400320A RID: 12810
	private string m_objCompleteTitleLocID;

	// Token: 0x0400320B RID: 12811
	private string m_objCompleteSubTitleLocID;

	// Token: 0x0400320C RID: 12812
	private string m_objCompleteBodyLocID;

	// Token: 0x0400320D RID: 12813
	private bool m_objCompleteDisplayPlayer;

	// Token: 0x0400320E RID: 12814
	private Action m_closeConfirmWindow;

	// Token: 0x0400320F RID: 12815
	private bool m_dataLoadedSuccessfully;

	// Token: 0x0200081B RID: 2075
	private enum LQADisplayType
	{
		// Token: 0x04003211 RID: 12817
		None,
		// Token: 0x04003212 RID: 12818
		DialogueWindow,
		// Token: 0x04003213 RID: 12819
		ConfirmWindow,
		// Token: 0x04003214 RID: 12820
		ObjectiveComplete
	}
}
