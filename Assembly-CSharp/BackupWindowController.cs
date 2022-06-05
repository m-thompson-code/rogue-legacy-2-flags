using System;
using System.Collections.Generic;
using Rewired;
using RL_Windows;
using UnityEngine;

// Token: 0x02000942 RID: 2370
public class BackupWindowController : WindowController
{
	// Token: 0x1700193E RID: 6462
	// (get) Token: 0x060047EF RID: 18415 RVA: 0x0002770C File Offset: 0x0002590C
	public override WindowID ID
	{
		get
		{
			return WindowID.Backup;
		}
	}

	// Token: 0x060047F0 RID: 18416 RVA: 0x00027710 File Offset: 0x00025910
	private void Awake()
	{
		this.m_onConfirmInputHandler = new Action<InputActionEventData>(this.OnConfirmInputHandler);
		this.m_onVerticalInputHandler = new Action<InputActionEventData>(this.OnVerticalInputHandler);
	}

	// Token: 0x060047F1 RID: 18417 RVA: 0x00116D74 File Offset: 0x00114F74
	public override void Initialize()
	{
		this.m_backupSlotArray = new BackupSlotEntry[6];
		for (int i = 0; i < this.m_backupSlotArray.Length - 1; i++)
		{
			BackupSlotEntry backupSlotEntry = UnityEngine.Object.Instantiate<BackupSlotEntry>(this.m_backupSlotPrefab, this.m_contentParentTransform);
			backupSlotEntry.name = "Backup Slot Entry " + i.ToString();
			BackupSlotEntry backupSlotEntry2 = backupSlotEntry;
			backupSlotEntry2.BackupSlotSelected = (BackupSlotSelectedHandler)Delegate.Combine(backupSlotEntry2.BackupSlotSelected, new BackupSlotSelectedHandler(this.UpdateSelectedBackupEntry));
			backupSlotEntry.Initialize(i, null, null, null, null, null);
			backupSlotEntry.gameObject.SetActive(false);
			this.m_backupSlotArray[i] = backupSlotEntry;
		}
		BackupSlotEntry backupSlotEntry3 = UnityEngine.Object.Instantiate<BackupSlotEntry>(this.m_backupSlotPrefab, this.m_contentParentTransform);
		backupSlotEntry3.name = "Change Profile Slot";
		BackupSlotEntry backupSlotEntry4 = backupSlotEntry3;
		backupSlotEntry4.BackupSlotSelected = (BackupSlotSelectedHandler)Delegate.Combine(backupSlotEntry4.BackupSlotSelected, new BackupSlotSelectedHandler(this.UpdateSelectedBackupEntry));
		backupSlotEntry3.InitializeAsChangeProfileSlotOption(this.m_backupSlotArray.Length - 1);
		this.m_backupSlotArray[this.m_backupSlotArray.Length - 1] = backupSlotEntry3;
		base.Initialize();
	}

	// Token: 0x060047F2 RID: 18418 RVA: 0x00116E78 File Offset: 0x00115078
	private void InitializeBackupArray()
	{
		List<string> list = new List<string>(SaveManager.GetBackupPathes(SaveManager.CurrentProfile, SaveDataType.GameMode));
		int count = list.Count;
		List<string> list2 = new List<string>(SaveManager.GetBackupPathes(SaveManager.CurrentProfile, SaveDataType.Player));
		if (list2.Count < count)
		{
			count = list2.Count;
		}
		List<string> list3 = new List<string>(SaveManager.GetBackupPathes(SaveManager.CurrentProfile, SaveDataType.Equipment));
		if (list3.Count < count)
		{
			count = list3.Count;
		}
		List<string> list4 = new List<string>(SaveManager.GetBackupPathes(SaveManager.CurrentProfile, SaveDataType.Lineage));
		if (list4.Count < count)
		{
			count = list4.Count;
		}
		List<string> list5 = new List<string>(SaveManager.GetBackupPathes(SaveManager.CurrentProfile, SaveDataType.Stage));
		if (list5.Count < count)
		{
			count = list5.Count;
		}
		for (int i = 0; i < this.m_backupSlotArray.Length - 1; i++)
		{
			BackupSlotEntry backupSlotEntry = this.m_backupSlotArray[i];
			if (!backupSlotEntry.IsChangeProfileOption)
			{
				if (i < count)
				{
					backupSlotEntry.gameObject.SetActive(true);
					backupSlotEntry.Initialize(i, list[i], list2[i], list3[i], list4[i], list5[i]);
				}
				else
				{
					backupSlotEntry.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x060047F3 RID: 18419 RVA: 0x00116FAC File Offset: 0x001151AC
	protected override void OnOpen()
	{
		this.InitializeBackupArray();
		int num = -1;
		for (int i = 0; i < this.m_backupSlotArray.Length; i++)
		{
			if (this.m_backupSlotArray[i].gameObject.activeSelf)
			{
				num = i;
				break;
			}
		}
		this.m_backupSlotArray[num].OnSelect(null);
		this.m_windowCanvas.gameObject.SetActive(true);
	}

	// Token: 0x060047F4 RID: 18420 RVA: 0x0000EE94 File Offset: 0x0000D094
	protected override void OnClose()
	{
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x060047F5 RID: 18421 RVA: 0x0011700C File Offset: 0x0011520C
	private void OnVerticalInputHandler(InputActionEventData eventData)
	{
		if (this.m_backupSlotArray.Length <= 1)
		{
			return;
		}
		int currentSelectedIndex = this.m_currentSelectedIndex;
		int num = this.m_currentSelectedIndex;
		float num2 = eventData.GetAxis();
		if (num2 == 0f)
		{
			num2 = -eventData.GetAxisPrev();
		}
		if (num2 > 0f)
		{
			for (int i = 0; i < this.m_backupSlotArray.Length; i++)
			{
				num--;
				if (num < 0)
				{
					num = this.m_backupSlotArray.Length - 1;
				}
				if (this.m_backupSlotArray[num].gameObject.activeSelf)
				{
					break;
				}
			}
		}
		else
		{
			for (int j = 0; j < this.m_backupSlotArray.Length; j++)
			{
				num++;
				if (num >= this.m_backupSlotArray.Length)
				{
					num = 0;
				}
				if (this.m_backupSlotArray[num].gameObject.activeSelf)
				{
					break;
				}
			}
		}
		if (currentSelectedIndex != num)
		{
			this.m_backupSlotArray[num].OnSelect(null);
		}
	}

	// Token: 0x060047F6 RID: 18422 RVA: 0x00027736 File Offset: 0x00025936
	private void OnConfirmInputHandler(InputActionEventData eventData)
	{
		if (eventData.IsCurrentInputSource(ControllerType.Mouse))
		{
			return;
		}
		if (this.m_backupSlotArray.Length != 0)
		{
			this.m_backupSlotArray[this.m_currentSelectedIndex].ExecuteButton();
		}
	}

	// Token: 0x060047F7 RID: 18423 RVA: 0x001170E0 File Offset: 0x001152E0
	protected virtual void UpdateSelectedBackupEntry(BackupSlotEntry backupSlot)
	{
		if (backupSlot.Index == this.m_currentSelectedIndex)
		{
			return;
		}
		if (this.m_backupSlotArray[this.m_currentSelectedIndex] != null)
		{
			this.m_backupSlotArray[this.m_currentSelectedIndex].OnDeselect(null);
		}
		this.m_currentSelectedIndex = backupSlot.Index;
	}

	// Token: 0x060047F8 RID: 18424 RVA: 0x0002775E File Offset: 0x0002595E
	protected override void OnFocus()
	{
		this.AddInputListeners();
	}

	// Token: 0x060047F9 RID: 18425 RVA: 0x00027766 File Offset: 0x00025966
	protected override void OnLostFocus()
	{
		this.RemoveInputListeners();
	}

	// Token: 0x060047FA RID: 18426 RVA: 0x00117130 File Offset: 0x00115330
	private void AddInputListeners()
	{
		base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Window_Vertical");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonRepeating, "Window_Vertical");
	}

	// Token: 0x060047FB RID: 18427 RVA: 0x00117188 File Offset: 0x00115388
	private void RemoveInputListeners()
	{
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Window_Vertical");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonRepeating, "Window_Vertical");
	}

	// Token: 0x04003704 RID: 14084
	[SerializeField]
	private BackupSlotEntry m_backupSlotPrefab;

	// Token: 0x04003705 RID: 14085
	[SerializeField]
	private Transform m_contentParentTransform;

	// Token: 0x04003706 RID: 14086
	private BackupSlotEntry[] m_backupSlotArray;

	// Token: 0x04003707 RID: 14087
	private int m_currentSelectedIndex;

	// Token: 0x04003708 RID: 14088
	private Action<InputActionEventData> m_onConfirmInputHandler;

	// Token: 0x04003709 RID: 14089
	private Action<InputActionEventData> m_onVerticalInputHandler;
}
