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

// Token: 0x02000828 RID: 2088
public class UserReportingScript : MonoBehaviour
{
	// Token: 0x06004501 RID: 17665 RVA: 0x000F5600 File Offset: 0x000F3800
	public UserReportingScript()
	{
		this.UserReportSubmitting = new UnityEvent();
		this.unityUserReportingUpdater = new UnityUserReportingUpdater();
	}

	// Token: 0x17001714 RID: 5908
	// (get) Token: 0x06004502 RID: 17666 RVA: 0x000F561E File Offset: 0x000F381E
	// (set) Token: 0x06004503 RID: 17667 RVA: 0x000F5626 File Offset: 0x000F3826
	public UserReport CurrentUserReport { get; private set; }

	// Token: 0x17001715 RID: 5909
	// (get) Token: 0x06004504 RID: 17668 RVA: 0x000F562F File Offset: 0x000F382F
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

	// Token: 0x06004505 RID: 17669 RVA: 0x000F565A File Offset: 0x000F385A
	public void CancelUserReport()
	{
		this.CurrentUserReport = null;
		this.ClearForm();
	}

	// Token: 0x06004506 RID: 17670 RVA: 0x000F5669 File Offset: 0x000F3869
	private IEnumerator ClearError()
	{
		yield return new WaitForSeconds(10f);
		this.isShowingError = false;
		yield break;
	}

	// Token: 0x06004507 RID: 17671 RVA: 0x000F5678 File Offset: 0x000F3878
	private void ClearForm()
	{
		this.SummaryInput.text = null;
		this.DescriptionInput.text = null;
	}

	// Token: 0x06004508 RID: 17672 RVA: 0x000F5694 File Offset: 0x000F3894
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

	// Token: 0x06004509 RID: 17673 RVA: 0x000F572D File Offset: 0x000F392D
	private UserReportingClientConfiguration GetConfiguration()
	{
		return new UserReportingClientConfiguration();
	}

	// Token: 0x0600450A RID: 17674 RVA: 0x000F5734 File Offset: 0x000F3934
	public bool IsSubmitting()
	{
		return this.isSubmitting;
	}

	// Token: 0x0600450B RID: 17675 RVA: 0x000F573C File Offset: 0x000F393C
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

	// Token: 0x0600450C RID: 17676 RVA: 0x000F57C8 File Offset: 0x000F39C8
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

	// Token: 0x0600450D RID: 17677 RVA: 0x000F58C8 File Offset: 0x000F3AC8
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

	// Token: 0x0600450E RID: 17678 RVA: 0x000F59F0 File Offset: 0x000F3BF0
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

	// Token: 0x0600450F RID: 17679 RVA: 0x000F5AE9 File Offset: 0x000F3CE9
	protected virtual void RaiseUserReportSubmitting()
	{
		if (this.UserReportSubmitting != null)
		{
			this.UserReportSubmitting.Invoke();
		}
	}

	// Token: 0x04003AD8 RID: 15064
	[Tooltip("The category dropdown.")]
	public Dropdown CategoryDropdown;

	// Token: 0x04003AD9 RID: 15065
	[Tooltip("The description input on the user report form.")]
	public InputField DescriptionInput;

	// Token: 0x04003ADA RID: 15066
	[Tooltip("The UI shown when there's an error.")]
	public Canvas ErrorPopup;

	// Token: 0x04003ADB RID: 15067
	private bool isCreatingUserReport;

	// Token: 0x04003ADC RID: 15068
	[Tooltip("A value indicating whether the hotkey is enabled (Left Alt + Left Shift + B).")]
	public bool IsHotkeyEnabled;

	// Token: 0x04003ADD RID: 15069
	[Tooltip("A value indicating whether the prefab is in silent mode. Silent mode does not show the user report form.")]
	public bool IsInSilentMode;

	// Token: 0x04003ADE RID: 15070
	[Tooltip("A value indicating whether the user report client reports metrics about itself.")]
	public bool IsSelfReporting;

	// Token: 0x04003ADF RID: 15071
	private bool isShowingError;

	// Token: 0x04003AE0 RID: 15072
	private bool isSubmitting;

	// Token: 0x04003AE1 RID: 15073
	[Tooltip("The display text for the progress text.")]
	public Text ProgressText;

	// Token: 0x04003AE2 RID: 15074
	[Tooltip("A value indicating whether the user report client send events to analytics.")]
	public bool SendEventsToAnalytics;

	// Token: 0x04003AE3 RID: 15075
	[Tooltip("The UI shown while submitting.")]
	public Canvas SubmittingPopup;

	// Token: 0x04003AE4 RID: 15076
	[Tooltip("The summary input on the user report form.")]
	public InputField SummaryInput;

	// Token: 0x04003AE5 RID: 15077
	[Tooltip("The thumbnail viewer on the user report form.")]
	public Image ThumbnailViewer;

	// Token: 0x04003AE6 RID: 15078
	private UnityUserReportingUpdater unityUserReportingUpdater;

	// Token: 0x04003AE7 RID: 15079
	[Tooltip("The user report button used to create a user report.")]
	public Button UserReportButton;

	// Token: 0x04003AE8 RID: 15080
	[Tooltip("The UI for the user report form. Shown after a user report is created.")]
	public Canvas UserReportForm;

	// Token: 0x04003AE9 RID: 15081
	[Tooltip("The User Reporting platform. Different platforms have different features but may require certain Unity versions or target platforms. The Async platform adds async screenshotting and report creation, but requires Unity 2018.3 and above, the package manager version of Unity User Reporting, and a target platform that supports asynchronous GPU readback such as DirectX.")]
	public UserReportingPlatformType UserReportingPlatform;

	// Token: 0x04003AEA RID: 15082
	[Tooltip("The event raised when a user report is submitting.")]
	public UnityEvent UserReportSubmitting;
}
