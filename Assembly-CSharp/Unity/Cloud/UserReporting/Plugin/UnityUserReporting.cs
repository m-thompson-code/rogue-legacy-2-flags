using System;
using Unity.Cloud.UserReporting.Client;
using UnityEngine;

namespace Unity.Cloud.UserReporting.Plugin
{
	// Token: 0x02000843 RID: 2115
	public static class UnityUserReporting
	{
		// Token: 0x17001762 RID: 5986
		// (get) Token: 0x060045F4 RID: 17908 RVA: 0x000F87D3 File Offset: 0x000F69D3
		// (set) Token: 0x060045F5 RID: 17909 RVA: 0x000F87E6 File Offset: 0x000F69E6
		public static UserReportingClient CurrentClient
		{
			get
			{
				if (UnityUserReporting.currentClient == null)
				{
					UnityUserReporting.Configure();
				}
				return UnityUserReporting.currentClient;
			}
			private set
			{
				UnityUserReporting.currentClient = value;
			}
		}

		// Token: 0x060045F6 RID: 17910 RVA: 0x000F87EE File Offset: 0x000F69EE
		public static void Configure(string endpoint, string projectIdentifier, IUserReportingPlatform platform, UserReportingClientConfiguration configuration)
		{
			UnityUserReporting.CurrentClient = new UserReportingClient(endpoint, projectIdentifier, platform, configuration);
		}

		// Token: 0x060045F7 RID: 17911 RVA: 0x000F87FE File Offset: 0x000F69FE
		public static void Configure(string endpoint, string projectIdentifier, UserReportingClientConfiguration configuration)
		{
			UnityUserReporting.CurrentClient = new UserReportingClient(endpoint, projectIdentifier, UnityUserReporting.GetPlatform(), configuration);
		}

		// Token: 0x060045F8 RID: 17912 RVA: 0x000F8812 File Offset: 0x000F6A12
		public static void Configure(string projectIdentifier, UserReportingClientConfiguration configuration)
		{
			UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", projectIdentifier, UnityUserReporting.GetPlatform(), configuration);
		}

		// Token: 0x060045F9 RID: 17913 RVA: 0x000F8825 File Offset: 0x000F6A25
		public static void Configure(string projectIdentifier)
		{
			UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", projectIdentifier, UnityUserReporting.GetPlatform(), new UserReportingClientConfiguration());
		}

		// Token: 0x060045FA RID: 17914 RVA: 0x000F883C File Offset: 0x000F6A3C
		public static void Configure()
		{
			UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", Application.cloudProjectId, UnityUserReporting.GetPlatform(), new UserReportingClientConfiguration());
		}

		// Token: 0x060045FB RID: 17915 RVA: 0x000F8857 File Offset: 0x000F6A57
		public static void Configure(UserReportingClientConfiguration configuration)
		{
			UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", Application.cloudProjectId, UnityUserReporting.GetPlatform(), configuration);
		}

		// Token: 0x060045FC RID: 17916 RVA: 0x000F886E File Offset: 0x000F6A6E
		public static void Configure(string projectIdentifier, IUserReportingPlatform platform, UserReportingClientConfiguration configuration)
		{
			UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", projectIdentifier, platform, configuration);
		}

		// Token: 0x060045FD RID: 17917 RVA: 0x000F887D File Offset: 0x000F6A7D
		public static void Configure(IUserReportingPlatform platform, UserReportingClientConfiguration configuration)
		{
			UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", Application.cloudProjectId, platform, configuration);
		}

		// Token: 0x060045FE RID: 17918 RVA: 0x000F8890 File Offset: 0x000F6A90
		public static void Configure(IUserReportingPlatform platform)
		{
			UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", Application.cloudProjectId, platform, new UserReportingClientConfiguration());
		}

		// Token: 0x060045FF RID: 17919 RVA: 0x000F88A7 File Offset: 0x000F6AA7
		private static IUserReportingPlatform GetPlatform()
		{
			return new UnityUserReportingPlatform();
		}

		// Token: 0x06004600 RID: 17920 RVA: 0x000F88AE File Offset: 0x000F6AAE
		public static void Use(UserReportingClient client)
		{
			if (client != null)
			{
				UnityUserReporting.CurrentClient = client;
			}
		}

		// Token: 0x04003B6A RID: 15210
		private static UserReportingClient currentClient;
	}
}
