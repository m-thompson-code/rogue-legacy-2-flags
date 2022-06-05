using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using SceneManagement_RL;
using TMPro;
using Unity.Cloud.UserReporting;
using Unity.Cloud.UserReporting.Client;
using Unity.Cloud.UserReporting.Plugin;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000829 RID: 2089
public class UserReportingScript_TMP : MonoBehaviour
{
	// Token: 0x06004513 RID: 17683 RVA: 0x000F5C77 File Offset: 0x000F3E77
	public UserReportingScript_TMP()
	{
		this.UserReportSubmitting = new UnityEvent();
		this.unityUserReportingUpdater = new UnityUserReportingUpdater();
	}

	// Token: 0x17001716 RID: 5910
	// (get) Token: 0x06004514 RID: 17684 RVA: 0x000F5C95 File Offset: 0x000F3E95
	// (set) Token: 0x06004515 RID: 17685 RVA: 0x000F5C9D File Offset: 0x000F3E9D
	public UserReport CurrentUserReport { get; private set; }

	// Token: 0x17001717 RID: 5911
	// (get) Token: 0x06004516 RID: 17686 RVA: 0x000F5CA6 File Offset: 0x000F3EA6
	public UserReportingState State
	{
		get
		{
			if (this.CurrentUserReport != null)
			{
				if (this.IsInSilentMode)
				{
					return UserReportingState.Idle;
				}
				if (this.isSubmitting)
				{
					return UserReportingState.SubmittingForm;
				}
				return UserReportingState.ShowingForm;
			}
			else
			{
				if (this.isCreatingUserReport)
				{
					return UserReportingState.CreatingUserReport;
				}
				return UserReportingState.Idle;
			}
		}
	}

	// Token: 0x06004517 RID: 17687 RVA: 0x000F5CD4 File Offset: 0x000F3ED4
	public void CancelUserReport()
	{
		if (this.UserNameInput != null)
		{
			SaveManager.ConfigData.UserReportName = this.UserNameInput.text;
		}
		if (this.EmailInput != null)
		{
			SaveManager.ConfigData.UserReportEmail = this.EmailInput.text;
			SaveManager.SaveConfigFile();
		}
		this.CurrentUserReport = null;
		this.isSubmitting = false;
		this.isCreatingUserReport = false;
		this.isShowingError = false;
		this.ClearForm();
	}

	// Token: 0x06004518 RID: 17688 RVA: 0x000F5D4E File Offset: 0x000F3F4E
	private IEnumerator ClearError()
	{
		yield return new WaitForSeconds(5f);
		this.isShowingError = false;
		yield break;
	}

	// Token: 0x06004519 RID: 17689 RVA: 0x000F5D5D File Offset: 0x000F3F5D
	private void ClearForm()
	{
		UnityUserReporting.CurrentClient.ClearScreenshots();
		this.SummaryInput.text = null;
		this.DescriptionInput.text = null;
		this.UserNameInput.text = null;
		this.EmailInput.text = null;
	}

	// Token: 0x0600451A RID: 17690 RVA: 0x000F5D9C File Offset: 0x000F3F9C
	public void CreateUserReport()
	{
		if (this.isCreatingUserReport)
		{
			return;
		}
		this.SubmissionCompleteButton.gameObject.SetActive(false);
		this.ProgressText.gameObject.SetActive(true);
		this.isCreatingUserReport = true;
		UnityUserReporting.CurrentClient.TakeScreenshot(1280, 720, delegate(UserReportScreenshot s)
		{
		});
		UnityUserReporting.CurrentClient.CreateUserReport(delegate(UserReport br)
		{
			if (string.IsNullOrEmpty(br.ProjectIdentifier))
			{
				Debug.LogWarning("The user report's project identifier is not set. Please setup cloud services using the Services tab or manually specify a project identifier when calling UnityUserReporting.Configure().");
			}
			if (this.UserNameInput != null)
			{
				this.UserNameInput.text = SaveManager.ConfigData.UserReportName;
			}
			if (this.EmailInput != null)
			{
				this.EmailInput.text = SaveManager.ConfigData.UserReportEmail;
			}
			string arg = "Unknown";
			string arg2 = "0.0";
			foreach (UserReportNamedValue userReportNamedValue in br.DeviceMetadata)
			{
				if (userReportNamedValue.Name == "Platform")
				{
					arg = userReportNamedValue.Value;
				}
				if (userReportNamedValue.Name == "Version")
				{
					arg2 = userReportNamedValue.Value;
				}
			}
			br.Dimensions.Add(new UserReportNamedValue("Platform.Version", string.Format("{0}.{1}", arg, arg2)));
			this.CurrentUserReport = br;
			this.isCreatingUserReport = false;
			this.SetThumbnail(br);
			if (this.IsInSilentMode)
			{
				this.SubmitUserReport();
			}
		});
	}

	// Token: 0x0600451B RID: 17691 RVA: 0x000F5E24 File Offset: 0x000F4024
	private UserReportingClientConfiguration GetConfiguration()
	{
		return new UserReportingClientConfiguration(0, MetricsGatheringMode.Disabled, 0, 0, 2);
	}

	// Token: 0x0600451C RID: 17692 RVA: 0x000F5E30 File Offset: 0x000F4030
	public bool IsSubmitting()
	{
		return this.isSubmitting;
	}

	// Token: 0x0600451D RID: 17693 RVA: 0x000F5E38 File Offset: 0x000F4038
	private void SetThumbnail(UserReport userReport)
	{
		if (userReport != null && this.ThumbnailViewer != null)
		{
			byte[] data = Convert.FromBase64String(userReport.Thumbnail.DataBase64);
			Texture2D texture2D = new Texture2D(1, 1);
			texture2D.LoadImage(data);
			this.ThumbnailViewer.sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
			this.ThumbnailViewer.preserveAspect = true;
		}
	}

	// Token: 0x17001718 RID: 5912
	// (get) Token: 0x0600451E RID: 17694 RVA: 0x000F5EC3 File Offset: 0x000F40C3
	// (set) Token: 0x0600451F RID: 17695 RVA: 0x000F5ECB File Offset: 0x000F40CB
	public bool IsInitialized { get; private set; }

	// Token: 0x06004520 RID: 17696 RVA: 0x000F5ED4 File Offset: 0x000F40D4
	private void Start()
	{
		bool flag = false;
		if (this.UserReportingPlatform == UserReportingPlatformType.Async)
		{
			Type type = Assembly.GetExecutingAssembly().GetType("Unity.Cloud.UserReporting.Plugin.Version2018_3.AsyncUnityUserReportingPlatform");
			if (type != null)
			{
				IUserReportingPlatform userReportingPlatform = Activator.CreateInstance(type) as IUserReportingPlatform;
				if (userReportingPlatform != null)
				{
					UnityUserReporting.Configure(userReportingPlatform, this.GetConfiguration());
					flag = true;
				}
			}
		}
		if (!flag)
		{
			UnityUserReporting.Configure(this.GetConfiguration());
		}
		string endpoint = string.Format("https://userreporting.cloud.unity3d.com/api/userreporting/projects/{0}/ping", UnityUserReporting.CurrentClient.ProjectIdentifier);
		UnityUserReporting.CurrentClient.Platform.Post(endpoint, "application/json", Encoding.UTF8.GetBytes("\"Ping\""), delegate(float upload, float download)
		{
		}, delegate(bool result, byte[] bytes)
		{
		});
		this.IsInitialized = true;
	}

	// Token: 0x06004521 RID: 17697 RVA: 0x000F5FB0 File Offset: 0x000F41B0
	public void SubmitUserReport()
	{
		if (this.isSubmitting || this.CurrentUserReport == null)
		{
			return;
		}
		this.isSubmitting = true;
		if (this.SummaryInput != null)
		{
			this.CurrentUserReport.Summary = this.SummaryInput.text;
		}
		UserReportNamedValue item = default(UserReportNamedValue);
		item.Name = "Game Version";
		item.Value = System_EV.GetVersionString();
		this.CurrentUserReport.Fields.Add(item);
		if (this.UserNameInput != null)
		{
			UserReportNamedValue item2 = default(UserReportNamedValue);
			item2.Name = "User Name";
			item2.Value = this.UserNameInput.text;
			this.CurrentUserReport.Fields.Add(item2);
			SaveManager.ConfigData.UserReportName = this.UserNameInput.text;
		}
		if (this.EmailInput != null)
		{
			UserReportNamedValue item3 = default(UserReportNamedValue);
			item3.Name = "E-mail";
			item3.Value = this.EmailInput.text;
			this.CurrentUserReport.Fields.Add(item3);
			SaveManager.ConfigData.UserReportEmail = this.EmailInput.text;
			SaveManager.SaveConfigFile();
		}
		if (this.BugReportToggle != null && this.FeatureToggle != null)
		{
			if (this.BugReportToggle.isOn)
			{
				this.CurrentUserReport.Dimensions.Add(new UserReportNamedValue("ReportType", "Bug Report"));
				this.CurrentUserReport.Fields.Add(new UserReportNamedValue("ReportType", "Bug Report"));
			}
			else
			{
				this.CurrentUserReport.Dimensions.Add(new UserReportNamedValue("ReportType", "Feature Suggestion"));
				this.CurrentUserReport.Fields.Add(new UserReportNamedValue("ReportType", "Feature Suggestion"));
			}
		}
		if (this.ScreenshotToggle != null && !this.ScreenshotToggle.isOn)
		{
			UnityUserReporting.CurrentClient.ClearScreenshots();
			this.CurrentUserReport.Screenshots.Clear();
		}
		this.CurrentUserReport.Thumbnail = default(UserReportScreenshot);
		if (this.SaveFileToggle != null && this.SaveFileToggle.isOn)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			using (MemoryStream memoryStream = new MemoryStream())
			{
				binaryFormatter.Serialize(memoryStream, SaveManager.PlayerSaveData);
				this.CurrentUserReport.Attachments.Add(new UserReportAttachment("PlayerSave", "Player.rc2dat", "binary", memoryStream.ToArray()));
			}
			using (MemoryStream memoryStream2 = new MemoryStream())
			{
				binaryFormatter.Serialize(memoryStream2, SaveManager.EquipmentSaveData);
				this.CurrentUserReport.Attachments.Add(new UserReportAttachment("EquipmentSave", "Equipment.rc2dat", "binary", memoryStream2.ToArray()));
			}
			using (MemoryStream memoryStream3 = new MemoryStream())
			{
				binaryFormatter.Serialize(memoryStream3, SaveManager.StageSaveData);
				this.CurrentUserReport.Attachments.Add(new UserReportAttachment("StageSave", "Stage.rc2dat", "binary", memoryStream3.ToArray()));
			}
			using (MemoryStream memoryStream4 = new MemoryStream())
			{
				binaryFormatter.Serialize(memoryStream4, SaveManager.ModeSaveData);
				this.CurrentUserReport.Attachments.Add(new UserReportAttachment("GameModeSave", "GameMode.rc2dat", "binary", memoryStream4.ToArray()));
			}
			using (MemoryStream memoryStream5 = new MemoryStream())
			{
				binaryFormatter.Serialize(memoryStream5, SaveManager.LineageSaveData);
				this.CurrentUserReport.Attachments.Add(new UserReportAttachment("LineageSave", "Lineage.rc2dat", "binary", memoryStream5.ToArray()));
			}
		}
		if (this.DescriptionInput != null)
		{
			UserReportNamedValue item4 = default(UserReportNamedValue);
			item4.Name = "Description";
			item4.Value = this.DescriptionInput.text;
			this.CurrentUserReport.Fields.Add(item4);
		}
		UserReportNamedValue item5 = default(UserReportNamedValue);
		item5.Name = "Current Scene";
		item5.Value = SceneLoader_RL.CurrentScene;
		this.CurrentUserReport.Fields.Add(item5);
		UserReportNamedValue item6 = default(UserReportNamedValue);
		item6.Name = "Previous Scene";
		item6.Value = SceneLoader_RL.PreviousScene;
		this.CurrentUserReport.Fields.Add(item6);
		if (SceneManager.GetActiveScene().name == "World")
		{
			UserReportNamedValue item7 = default(UserReportNamedValue);
			item7.Name = "Current Seed";
			item7.Value = RNGSeedManager.GetSeedAsHex(RNGSeedManager.GetCurrentSeed("World")) + "-" + BurdenManager.GetBurdenLevel(BurdenType.RoomCount).ToString();
			this.CurrentUserReport.Fields.Add(item7);
			BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
			UserReportNamedValue item8 = default(UserReportNamedValue);
			item8.Name = "Biome";
			item8.Value = currentPlayerRoom.BiomeType.ToString();
			this.CurrentUserReport.Fields.Add(item8);
			UserReportNamedValue item9 = default(UserReportNamedValue);
			item9.Name = "Biome Controller Index";
			item9.Value = currentPlayerRoom.BiomeControllerIndex.ToString();
			this.CurrentUserReport.Fields.Add(item9);
			string[] array;
			if (currentPlayerRoom is Room)
			{
				string text = (currentPlayerRoom as Room).GridPointManager.RoomMetaData.ID.ToString();
				if ((currentPlayerRoom as Room).GridPointManager.IsRoomMirrored)
				{
					text += "M";
				}
				array = new string[]
				{
					text
				};
			}
			else
			{
				MergeRoom mergeRoom = currentPlayerRoom as MergeRoom;
				array = new string[mergeRoom.StandaloneGridPointManagers.Length];
				for (int i = 0; i < mergeRoom.StandaloneGridPointManagers.Length; i++)
				{
					array[i] = mergeRoom.StandaloneGridPointManagers[i].RoomMetaData.ID.ToString();
					if (mergeRoom.StandaloneGridPointManagers[i].IsRoomMirrored)
					{
						string[] array2 = array;
						int num = i;
						array2[num] += "M";
					}
				}
			}
			string text2 = string.Empty;
			for (int j = 0; j < array.Length; j++)
			{
				text2 += array[j].ToString();
				if (j < array.Length - 1)
				{
					text2 += ", ";
				}
			}
			UserReportNamedValue item10 = default(UserReportNamedValue);
			item10.Name = "Room ID(s)";
			item10.Value = text2;
			this.CurrentUserReport.Fields.Add(item10);
		}
		if (RNGSeedManager.PreviousMasterSeedOverrides != null)
		{
			UserReportNamedValue item11 = default(UserReportNamedValue);
			item11.Name = "Previous Seeds";
			string text3 = string.Empty;
			for (int k = RNGSeedManager.PreviousMasterSeedOverrides.Count - 1; k >= 0; k--)
			{
				text3 = text3 + RNGSeedManager.GetSeedAsHex(RNGSeedManager.PreviousMasterSeedOverrides.ElementAt(k)) + "-" + BurdenManager.GetBurdenLevel(BurdenType.RoomCount).ToString();
				if (k > 0)
				{
					text3 += ", ";
				}
			}
			item11.Value = text3;
			this.CurrentUserReport.Fields.Add(item11);
		}
		this.ClearForm();
		this.RaiseUserReportSubmitting();
		UnityUserReporting.CurrentClient.SendUserReport(this.CurrentUserReport, delegate(float uploadProgress, float downloadProgress)
		{
			if (this.ProgressText != null)
			{
				string text4 = string.Format("{0:P}", uploadProgress);
				this.ProgressText.text = text4;
			}
		}, delegate(bool success, UserReport br2)
		{
			if (!success)
			{
				if (this.m_errorMessageWaitCoroutine != null)
				{
					base.StopCoroutine(this.m_errorMessageWaitCoroutine);
				}
				this.m_errorMessageWaitCoroutine = base.StartCoroutine(this.ErrorMessageTimeout());
				this.isShowingError = true;
				return;
			}
			this.SubmissionCompleteButton.gameObject.SetActive(true);
			this.ProgressText.text = "COMPLETE";
		});
	}

	// Token: 0x06004522 RID: 17698 RVA: 0x000F6798 File Offset: 0x000F4998
	private void OnDisable()
	{
		if (this.m_errorMessageWaitCoroutine != null)
		{
			base.StopCoroutine(this.m_errorMessageWaitCoroutine);
			this.m_errorMessageWaitCoroutine = null;
		}
	}

	// Token: 0x06004523 RID: 17699 RVA: 0x000F67B5 File Offset: 0x000F49B5
	private IEnumerator ErrorMessageTimeout()
	{
		float timeStart = Time.unscaledTime;
		while (Time.unscaledTime - timeStart < 10f)
		{
			yield return null;
		}
		if (this.m_windowController == null)
		{
			this.m_windowController = base.GetComponentInParent<UserReportWindowController>();
		}
		this.m_errorMessageWaitCoroutine = null;
		this.m_windowController.CompleteUserReport();
		yield break;
	}

	// Token: 0x06004524 RID: 17700 RVA: 0x000F67C4 File Offset: 0x000F49C4
	private void Update()
	{
		UnityUserReporting.CurrentClient.IsSelfReporting = this.IsSelfReporting;
		UnityUserReporting.CurrentClient.SendEventsToAnalytics = this.SendEventsToAnalytics;
		if (this.UserReportButton != null)
		{
			this.UserReportButton.interactable = (this.State == UserReportingState.Idle);
		}
		if (this.UserReportForm != null)
		{
			this.UserReportForm.enabled = (this.State == UserReportingState.ShowingForm);
		}
		if (this.SubmittingPopup != null)
		{
			this.SubmittingPopup.enabled = (this.State == UserReportingState.SubmittingForm);
		}
		if (this.ErrorPopup != null)
		{
			this.ErrorPopup.enabled = this.isShowingError;
		}
		this.unityUserReportingUpdater.Reset();
		base.StartCoroutine(this.unityUserReportingUpdater);
	}

	// Token: 0x06004525 RID: 17701 RVA: 0x000F688E File Offset: 0x000F4A8E
	protected virtual void RaiseUserReportSubmitting()
	{
		if (this.UserReportSubmitting != null)
		{
			this.UserReportSubmitting.Invoke();
		}
	}

	// Token: 0x04003AEC RID: 15084
	[Tooltip("The username input on the user report form.")]
	public TMP_InputField UserNameInput;

	// Token: 0x04003AED RID: 15085
	[Tooltip("The email input on the user report form.")]
	public TMP_InputField EmailInput;

	// Token: 0x04003AEE RID: 15086
	[Tooltip("The description input on the user report form.")]
	public TMP_InputField DescriptionInput;

	// Token: 0x04003AEF RID: 15087
	[Tooltip("The UI shown when there's an error.")]
	public Canvas ErrorPopup;

	// Token: 0x04003AF0 RID: 15088
	private bool isCreatingUserReport;

	// Token: 0x04003AF1 RID: 15089
	[Tooltip("A value indicating whether the hotkey is enabled (Left Alt + Left Shift + B).")]
	public bool IsHotkeyEnabled;

	// Token: 0x04003AF2 RID: 15090
	[Tooltip("A value indicating whether the prefab is in silent mode. Silent mode does not show the user report form.")]
	public bool IsInSilentMode;

	// Token: 0x04003AF3 RID: 15091
	[Tooltip("A value indicating whether the user report client reports metrics about itself.")]
	public bool IsSelfReporting;

	// Token: 0x04003AF4 RID: 15092
	private bool isShowingError;

	// Token: 0x04003AF5 RID: 15093
	private bool isSubmitting;

	// Token: 0x04003AF6 RID: 15094
	[Tooltip("The display text for the progress text.")]
	public TMP_Text ProgressText;

	// Token: 0x04003AF7 RID: 15095
	[Tooltip("A value indicating whether the user report client send events to analytics.")]
	public bool SendEventsToAnalytics;

	// Token: 0x04003AF8 RID: 15096
	[Tooltip("The UI shown while submitting.")]
	public Canvas SubmittingPopup;

	// Token: 0x04003AF9 RID: 15097
	public Button SubmissionCompleteButton;

	// Token: 0x04003AFA RID: 15098
	[Tooltip("The summary input on the user report form.")]
	public TMP_InputField SummaryInput;

	// Token: 0x04003AFB RID: 15099
	public Toggle BugReportToggle;

	// Token: 0x04003AFC RID: 15100
	public Toggle FeatureToggle;

	// Token: 0x04003AFD RID: 15101
	public Toggle ScreenshotToggle;

	// Token: 0x04003AFE RID: 15102
	public Toggle SaveFileToggle;

	// Token: 0x04003AFF RID: 15103
	[Tooltip("The thumbnail viewer on the user report form.")]
	public Image ThumbnailViewer;

	// Token: 0x04003B00 RID: 15104
	private UnityUserReportingUpdater unityUserReportingUpdater;

	// Token: 0x04003B01 RID: 15105
	[Tooltip("The user report button used to create a user report.")]
	public Button UserReportButton;

	// Token: 0x04003B02 RID: 15106
	[Tooltip("The UI for the user report form. Shown after a user report is created.")]
	public Canvas UserReportForm;

	// Token: 0x04003B03 RID: 15107
	[Tooltip("The User Reporting platform. Different platforms have different features but may require certain Unity versions or target platforms. The Async platform adds async screenshotting and report creation, but requires Unity 2018.3 and above, the package manager version of Unity User Reporting, and a target platform that supports asynchronous GPU readback such as DirectX.")]
	public UserReportingPlatformType UserReportingPlatform;

	// Token: 0x04003B04 RID: 15108
	[Tooltip("The event raised when a user report is submitting.")]
	public UnityEvent UserReportSubmitting;

	// Token: 0x04003B05 RID: 15109
	private UserReportWindowController m_windowController;

	// Token: 0x04003B06 RID: 15110
	private Coroutine m_errorMessageWaitCoroutine;
}
