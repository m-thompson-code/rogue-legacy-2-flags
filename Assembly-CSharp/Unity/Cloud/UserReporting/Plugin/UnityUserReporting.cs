using System;
using Unity.Cloud.UserReporting.Client;
using UnityEngine;

namespace Unity.Cloud.UserReporting.Plugin
{
	// Token: 0x02000D29 RID: 3369
	public static class UnityUserReporting
	{
		// Token: 0x17001F90 RID: 8080
		// (get) Token: 0x0600602C RID: 24620 RVA: 0x00035096 File Offset: 0x00033296
		// (set) Token: 0x0600602D RID: 24621 RVA: 0x000350A9 File Offset: 0x000332A9
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

		// Token: 0x0600602E RID: 24622 RVA: 0x000350B1 File Offset: 0x000332B1
		public static void Configure(string endpoint, string projectIdentifier, IUserReportingPlatform platform, UserReportingClientConfiguration configuration)
		{
			UnityUserReporting.CurrentClient = new UserReportingClient(endpoint, projectIdentifier, platform, configuration);
		}

		// Token: 0x0600602F RID: 24623 RVA: 0x000350C1 File Offset: 0x000332C1
		public static void Configure(string endpoint, string projectIdentifier, UserReportingClientConfiguration configuration)
		{
			UnityUserReporting.CurrentClient = new UserReportingClient(endpoint, projectIdentifier, UnityUserReporting.GetPlatform(), configuration);
		}

		// Token: 0x06006030 RID: 24624 RVA: 0x000350D5 File Offset: 0x000332D5
		public static void Configure(string projectIdentifier, UserReportingClientConfiguration configuration)
		{
			UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", projectIdentifier, UnityUserReporting.GetPlatform(), configuration);
		}

		// Token: 0x06006031 RID: 24625 RVA: 0x000350E8 File Offset: 0x000332E8
		public static void Configure(string projectIdentifier)
		{
			UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", projectIdentifier, UnityUserReporting.GetPlatform(), new UserReportingClientConfiguration());
		}

		// Token: 0x06006032 RID: 24626 RVA: 0x000350FF File Offset: 0x000332FF
		public static void Configure()
		{
			UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", Application.cloudProjectId, UnityUserReporting.GetPlatform(), new UserReportingClientConfiguration());
		}

		// Token: 0x06006033 RID: 24627 RVA: 0x0003511A File Offset: 0x0003331A
		public static void Configure(UserReportingClientConfiguration configuration)
		{
			UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", Application.cloudProjectId, UnityUserReporting.GetPlatform(), configuration);
		}

		// Token: 0x06006034 RID: 24628 RVA: 0x00035131 File Offset: 0x00033331
		public static void Configure(string projectIdentifier, IUserReportingPlatform platform, UserReportingClientConfiguration configuration)
		{
			UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", projectIdentifier, platform, configuration);
		}

		// Token: 0x06006035 RID: 24629 RVA: 0x00035140 File Offset: 0x00033340
		public static void Configure(IUserReportingPlatform platform, UserReportingClientConfiguration configuration)
		{
			UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", Application.cloudProjectId, platform, configuration);
		}

		// Token: 0x06006036 RID: 24630 RVA: 0x00035153 File Offset: 0x00033353
		public static void Configure(IUserReportingPlatform platform)
		{
			UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", Application.cloudProjectId, platform, new UserReportingClientConfiguration());
		}

		// Token: 0x06006037 RID: 24631 RVA: 0x0003516A File Offset: 0x0003336A
		private static IUserReportingPlatform GetPlatform()
		{
			return new UnityUserReportingPlatform();
		}

		// Token: 0x06006038 RID: 24632 RVA: 0x00035171 File Offset: 0x00033371
		public static void Use(UserReportingClient client)
		{
			if (client != null)
			{
				UnityUserReporting.CurrentClient = client;
			}
		}

		// Token: 0x04004EAB RID: 20139
		private static UserReportingClient currentClient;
	}
}
