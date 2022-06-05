using System;
using System.Collections;
using System.Reflection;
using System.Text;
using Unity.Cloud.UserReporting;
using Unity.Cloud.UserReporting.Client;
using Unity.Cloud.UserReporting.Plugin;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000D02 RID: 3330
public class UserReportingScript : MonoBehaviour
{
	// Token: 0x06005EDD RID: 24285 RVA: 0x00034479 File Offset: 0x00032679
	public UserReportingScript()
	{
		this.UserReportSubmitting = new UnityEvent();
		this.unityUserReportingUpdater = new UnityUserReportingUpdater();
	}

	// Token: 0x17001F26 RID: 7974
	// (get) Token: 0x06005EDE RID: 24286 RVA: 0x00034497 File Offset: 0x00032697
	// (set) Token: 0x06005EDF RID: 24287 RVA: 0x0003449F File Offset: 0x0003269F
	public UserReport CurrentUserReport { get; private set; }

	// Token: 0x17001F27 RID: 7975
	// (get) Token: 0x06005EE0 RID: 24288 RVA: 0x000344A8 File Offset: 0x000326A8
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

	// Token: 0x06005EE1 RID: 24289 RVA: 0x000344D3 File Offset: 0x000326D3
	public void CancelUserReport()
	{
		this.CurrentUserReport = null;
		this.ClearForm();
	}

	// Token: 0x06005EE2 RID: 24290 RVA: 0x000344E2 File Offset: 0x000326E2
	private IEnumerator ClearError()
	{
		yield return new WaitForSeconds(10f);
		this.isShowingError = false;
		yield break;
	}

	// Token: 0x06005EE3 RID: 24291 RVA: 0x000344F1 File Offset: 0x000326F1
	private void ClearForm()
	{
		this.SummaryInput.text = null;
		this.DescriptionInput.text = null;
	}

	// Token: 0x06005EE4 RID: 24292 RVA: 0x001631D4 File Offset: 0x001613D4
	public void CreateUserReport()
	{
		if (this.isCreatingUserReport)
		{
			return;
		}
		this.isCreatingUserReport = true;
		UnityUserReporting.CurrentClient.TakeScreenshot(2048, 2048, delegate(UserReportScreenshot s)
		{
		});
		UnityUserReporting.CurrentClient.TakeScreenshot(512, 512, delegate(UserReportScreenshot s)
		{
		});
		UnityUserReporting.CurrentClient.CreateUserReport(delegate(UserReport br)
		{
			if (string.IsNullOrEmpty(br.ProjectIdentifier))
			{
				Debug.LogWarning("The user report's project identifier is not set. Please setup cloud services using the Services tab or manually specify a project identifier when calling UnityUserReporting.Configure().");
			}
			br.Attachments.Add(new UserReportAttachment("Sample Attachment.txt", "SampleAttachment.txt", "text/plain", Encoding.UTF8.GetBytes("This is a sample attachment.")));
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

	// Token: 0x06005EE5 RID: 24293 RVA: 0x0003450B File Offset: 0x0003270B
	private UserReportingClientConfiguration GetConfiguration()
	{
		return new UserReportingClientConfiguration();
	}

	// Token: 0x06005EE6 RID: 24294 RVA: 0x00034512 File Offset: 0x00032712
	public bool IsSubmitting()
	{
		return this.isSubmitting;
	}

	// Token: 0x06005EE7 RID: 24295 RVA: 0x00163270 File Offset: 0x00161470
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

	// Token: 0x06005EE8 RID: 24296 RVA: 0x001632FC File Offset: 0x001614FC
	private void Start()
	{
		if (Application.isPlaying && UnityEngine.Object.FindObjectOfType<EventSystem>() == null)
		{
			GameObject gameObject = new GameObject("EventSystem");
			gameObject.AddComponent<EventSystem>();
			gameObject.AddComponent<StandaloneInputModule>();
		}
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
	}

	// Token: 0x06005EE9 RID: 24297 RVA: 0x001633FC File Offset: 0x001615FC
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
		if (this.CategoryDropdown != null)
		{
			string text = this.CategoryDropdown.options[this.CategoryDropdown.value].text;
			this.CurrentUserReport.Dimensions.Add(new UserReportNamedValue("Category", text));
			this.CurrentUserReport.Fields.Add(new UserReportNamedValue("Category", text));
		}
		if (this.DescriptionInput != null)
		{
			UserReportNamedValue item = default(UserReportNamedValue);
			item.Name = "Description";
			item.Value = this.DescriptionInput.text;
			this.CurrentUserReport.Fields.Add(item);
		}
		this.ClearForm();
		this.RaiseUserReportSubmitting();
		UnityUserReporting.CurrentClient.SendUserReport(this.CurrentUserReport, delegate(float uploadProgress, float downloadProgress)
		{
			if (this.ProgressText != null)
			{
				string text2 = string.Format("{0:P}", uploadProgress);
				this.ProgressText.text = text2;
			}
		}, delegate(bool success, UserReport br2)
		{
			if (!success)
			{
				this.isShowingError = true;
				base.StartCoroutine(this.ClearError());
			}
			this.CurrentUserReport = null;
			this.isSubmitting = false;
		});
	}

	// Token: 0x06005EEA RID: 24298 RVA: 0x00163524 File Offset: 0x00161724
	private void Update()
	{
		if (this.IsHotkeyEnabled && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.B))
		{
			this.CreateUserReport();
		}
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

	// Token: 0x06005EEB RID: 24299 RVA: 0x0003451A File Offset: 0x0003271A
	protected virtual void RaiseUserReportSubmitting()
	{
		if (this.UserReportSubmitting != null)
		{
			this.UserReportSubmitting.Invoke();
		}
	}

	// Token: 0x04004DE7 RID: 19943
	[Tooltip("The category dropdown.")]
	public Dropdown CategoryDropdown;

	// Token: 0x04004DE8 RID: 19944
	[Tooltip("The description input on the user report form.")]
	public InputField DescriptionInput;

	// Token: 0x04004DE9 RID: 19945
	[Tooltip("The UI shown when there's an error.")]
	public Canvas ErrorPopup;

	// Token: 0x04004DEA RID: 19946
	private bool isCreatingUserReport;

	// Token: 0x04004DEB RID: 19947
	[Tooltip("A value indicating whether the hotkey is enabled (Left Alt + Left Shift + B).")]
	public bool IsHotkeyEnabled;

	// Token: 0x04004DEC RID: 19948
	[Tooltip("A value indicating whether the prefab is in silent mode. Silent mode does not show the user report form.")]
	public bool IsInSilentMode;

	// Token: 0x04004DED RID: 19949
	[Tooltip("A value indicating whether the user report client reports metrics about itself.")]
	public bool IsSelfReporting;

	// Token: 0x04004DEE RID: 19950
	private bool isShowingError;

	// Token: 0x04004DEF RID: 19951
	private bool isSubmitting;

	// Token: 0x04004DF0 RID: 19952
	[Tooltip("The display text for the progress text.")]
	public Text ProgressText;

	// Token: 0x04004DF1 RID: 19953
	[Tooltip("A value indicating whether the user report client send events to analytics.")]
	public bool SendEventsToAnalytics;

	// Token: 0x04004DF2 RID: 19954
	[Tooltip("The UI shown while submitting.")]
	public Canvas SubmittingPopup;

	// Token: 0x04004DF3 RID: 19955
	[Tooltip("The summary input on the user report form.")]
	public InputField SummaryInput;

	// Token: 0x04004DF4 RID: 19956
	[Tooltip("The thumbnail viewer on the user report form.")]
	public Image ThumbnailViewer;

	// Token: 0x04004DF5 RID: 19957
	private UnityUserReportingUpdater unityUserReportingUpdater;

	// Token: 0x04004DF6 RID: 19958
	[Tooltip("The user report button used to create a user report.")]
	public Button UserReportButton;

	// Token: 0x04004DF7 RID: 19959
	[Tooltip("The UI for the user report form. Shown after a user report is created.")]
	public Canvas UserReportForm;

	// Token: 0x04004DF8 RID: 19960
	[Tooltip("The User Reporting platform. Different platforms have different features but may require certain Unity versions or target platforms. The Async platform adds async screenshotting and report creation, but requires Unity 2018.3 and above, the package manager version of Unity User Reporting, and a target platform that supports asynchronous GPU readback such as DirectX.")]
	public UserReportingPlatformType UserReportingPlatform;

	// Token: 0x04004DF9 RID: 19961
	[Tooltip("The event raised when a user report is submitting.")]
	public UnityEvent UserReportSubmitting;
}
