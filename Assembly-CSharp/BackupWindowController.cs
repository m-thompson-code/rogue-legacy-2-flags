using System;
using System.Collections.Generic;
using Rewired;
using RL_Windows;
using UnityEngine;

// Token: 0x0200056F RID: 1391
public class BackupWindowController : WindowController
{
	// Token: 0x17001273 RID: 4723
	// (get) Token: 0x06003308 RID: 13064 RVA: 0x000AC682 File Offset: 0x000AA882
	public override WindowID ID
	{
		get
		{
			return WindowID.Backup;
		}
	}

	// Token: 0x06003309 RID: 13065 RVA: 0x000AC686 File Offset: 0x000AA886
	private void Awake()
	{
		this.m_onConfirmInputHandler = new Action<InputActionEventData>(this.OnConfirmInputHandler);
		this.m_onVerticalInputHandler = new Action<InputActionEventData>(this.OnVerticalInputHandler);
	}

	// Token: 0x0600330A RID: 13066 RVA: 0x000AC6AC File Offset: 0x000AA8AC
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

	// Token: 0x0600330B RID: 13067 RVA: 0x000AC7B0 File Offset: 0x000AA9B0
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

	// Token: 0x0600330C RID: 13068 RVA: 0x000AC8E4 File Offset: 0x000AAAE4
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

	// Token: 0x0600330D RID: 13069 RVA: 0x000AC943 File Offset: 0x000AAB43
	protected override void OnClose()
	{
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x0600330E RID: 13070 RVA: 0x000AC958 File Offset: 0x000AAB58
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

	// Token: 0x0600330F RID: 13071 RVA: 0x000ACA2A File Offset: 0x000AAC2A
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

	// Token: 0x06003310 RID: 13072 RVA: 0x000ACA54 File Offset: 0x000AAC54
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

	// Token: 0x06003311 RID: 13073 RVA: 0x000ACAA4 File Offset: 0x000AACA4
	protected override void OnFocus()
	{
		this.AddInputListeners();
	}

	// Token: 0x06003312 RID: 13074 RVA: 0x000ACAAC File Offset: 0x000AACAC
	protected override void OnLostFocus()
	{
		this.RemoveInputListeners();
	}

	// Token: 0x06003313 RID: 13075 RVA: 0x000ACAB4 File Offset: 0x000AACB4
	private void AddInputListeners()
	{
		base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Window_Vertical");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonRepeating, "Window_Vertical");
	}

	// Token: 0x06003314 RID: 13076 RVA: 0x000ACB0C File Offset: 0x000AAD0C
	private void RemoveInputListeners()
	{
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Window_Vertical");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonRepeating, "Window_Vertical");
	}

	// Token: 0x040027CC RID: 10188
	[SerializeField]
	private BackupSlotEntry m_backupSlotPrefab;

	// Token: 0x040027CD RID: 10189
	[SerializeField]
	private Transform m_contentParentTransform;

	// Token: 0x040027CE RID: 10190
	private BackupSlotEntry[] m_backupSlotArray;

	// Token: 0x040027CF RID: 10191
	private int m_currentSelectedIndex;

	// Token: 0x040027D0 RID: 10192
	private Action<InputActionEventData> m_onConfirmInputHandler;

	// Token: 0x040027D1 RID: 10193
	private Action<InputActionEventData> m_onVerticalInputHandler;
}
