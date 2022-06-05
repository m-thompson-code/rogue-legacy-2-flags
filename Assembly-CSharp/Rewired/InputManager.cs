using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Rewired.Platforms;
using Rewired.Utils;
using Rewired.Utils.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rewired
{
	// Token: 0x02000EAF RID: 3759
	[AddComponentMenu("Rewired/Input Manager")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class InputManager : InputManager_Base
	{
		// Token: 0x06006BA1 RID: 27553 RVA: 0x0003B029 File Offset: 0x00039229
		protected override void OnInitialized()
		{
			this.SubscribeEvents();
		}

		// Token: 0x06006BA2 RID: 27554 RVA: 0x0003B031 File Offset: 0x00039231
		protected override void OnDeinitialized()
		{
			this.UnsubscribeEvents();
		}

		// Token: 0x06006BA3 RID: 27555 RVA: 0x00182958 File Offset: 0x00180B58
		protected override void DetectPlatform()
		{
			this.scriptingBackend = ScriptingBackend.Mono;
			this.scriptingAPILevel = ScriptingAPILevel.Net20;
			this.editorPlatform = EditorPlatform.None;
			this.platform = Rewired.Platforms.Platform.Unknown;
			this.webplayerPlatform = WebplayerPlatform.None;
			this.isEditor = false;
			if (SystemInfo.deviceName == null)
			{
				string empty = string.Empty;
			}
			if (SystemInfo.deviceModel == null)
			{
				string empty2 = string.Empty;
			}
			this.platform = Rewired.Platforms.Platform.Windows;
			this.scriptingBackend = ScriptingBackend.Mono;
			this.scriptingAPILevel = ScriptingAPILevel.Net46;
		}

		// Token: 0x06006BA4 RID: 27556 RVA: 0x00002FCA File Offset: 0x000011CA
		protected override void CheckRecompile()
		{
		}

		// Token: 0x06006BA5 RID: 27557 RVA: 0x0003B039 File Offset: 0x00039239
		protected override IExternalTools GetExternalTools()
		{
			return new ExternalTools();
		}

		// Token: 0x06006BA6 RID: 27558 RVA: 0x0003B040 File Offset: 0x00039240
		private bool CheckDeviceName(string searchPattern, string deviceName, string deviceModel)
		{
			return Regex.IsMatch(deviceName, searchPattern, RegexOptions.IgnoreCase) || Regex.IsMatch(deviceModel, searchPattern, RegexOptions.IgnoreCase);
		}

		// Token: 0x06006BA7 RID: 27559 RVA: 0x0003B056 File Offset: 0x00039256
		private void SubscribeEvents()
		{
			this.UnsubscribeEvents();
			SceneManager.sceneLoaded += this.OnSceneLoaded;
		}

		// Token: 0x06006BA8 RID: 27560 RVA: 0x0003B06F File Offset: 0x0003926F
		private void UnsubscribeEvents()
		{
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
		}

		// Token: 0x06006BA9 RID: 27561 RVA: 0x0003B082 File Offset: 0x00039282
		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			base.OnSceneLoaded();
		}

		// Token: 0x04005781 RID: 22401
		private bool ignoreRecompile;
	}
}
