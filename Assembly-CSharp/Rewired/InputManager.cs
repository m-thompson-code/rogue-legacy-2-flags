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
	// Token: 0x02000930 RID: 2352
	[AddComponentMenu("Rewired/Input Manager")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class InputManager : InputManager_Base
	{
		// Token: 0x06004E9A RID: 20122 RVA: 0x00113938 File Offset: 0x00111B38
		protected override void OnInitialized()
		{
			this.SubscribeEvents();
		}

		// Token: 0x06004E9B RID: 20123 RVA: 0x00113940 File Offset: 0x00111B40
		protected override void OnDeinitialized()
		{
			this.UnsubscribeEvents();
		}

		// Token: 0x06004E9C RID: 20124 RVA: 0x00113948 File Offset: 0x00111B48
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

		// Token: 0x06004E9D RID: 20125 RVA: 0x001139AE File Offset: 0x00111BAE
		protected override void CheckRecompile()
		{
		}

		// Token: 0x06004E9E RID: 20126 RVA: 0x001139B0 File Offset: 0x00111BB0
		protected override IExternalTools GetExternalTools()
		{
			return new ExternalTools();
		}

		// Token: 0x06004E9F RID: 20127 RVA: 0x001139B7 File Offset: 0x00111BB7
		private bool CheckDeviceName(string searchPattern, string deviceName, string deviceModel)
		{
			return Regex.IsMatch(deviceName, searchPattern, RegexOptions.IgnoreCase) || Regex.IsMatch(deviceModel, searchPattern, RegexOptions.IgnoreCase);
		}

		// Token: 0x06004EA0 RID: 20128 RVA: 0x001139CD File Offset: 0x00111BCD
		private void SubscribeEvents()
		{
			this.UnsubscribeEvents();
			SceneManager.sceneLoaded += this.OnSceneLoaded;
		}

		// Token: 0x06004EA1 RID: 20129 RVA: 0x001139E6 File Offset: 0x00111BE6
		private void UnsubscribeEvents()
		{
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
		}

		// Token: 0x06004EA2 RID: 20130 RVA: 0x001139F9 File Offset: 0x00111BF9
		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			base.OnSceneLoaded();
		}

		// Token: 0x04004215 RID: 16917
		private bool ignoreRecompile;
	}
}
