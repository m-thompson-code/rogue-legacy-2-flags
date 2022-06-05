using System;
using System.Collections.Generic;
using UnityEngine;

namespace Foreground
{
	// Token: 0x02000DC3 RID: 3523
	public class ForegroundController : MonoBehaviour
	{
		// Token: 0x17002005 RID: 8197
		// (get) Token: 0x06006338 RID: 25400 RVA: 0x00036A71 File Offset: 0x00034C71
		// (set) Token: 0x06006339 RID: 25401 RVA: 0x00036A79 File Offset: 0x00034C79
		public BaseRoom Room { get; private set; }

		// Token: 0x17002006 RID: 8198
		// (get) Token: 0x0600633A RID: 25402 RVA: 0x00036A82 File Offset: 0x00034C82
		// (set) Token: 0x0600633B RID: 25403 RVA: 0x00036A8A File Offset: 0x00034C8A
		public bool IsInitialized { get; private set; }

		// Token: 0x0600633C RID: 25404 RVA: 0x00036A93 File Offset: 0x00034C93
		private void Awake()
		{
			this.m_room.RoomMergedRelay.AddListener(new Action<object, EventArgs>(this.OnRoomMerged), false);
			this.m_room.RoomDestroyedRelay.AddListener(new Action<object, EventArgs>(this.OnRoomDestroyed), false);
		}

		// Token: 0x0600633D RID: 25405 RVA: 0x00171D8C File Offset: 0x0016FF8C
		public void CreateForeground(ForegroundLocation foregroundLocation)
		{
			if (this.m_zoomController == null || foregroundLocation == ForegroundLocation.None)
			{
				return;
			}
			ForegroundGroup[] array = new ForegroundGroup[0];
			ForegroundLocation foregroundLocation2 = foregroundLocation;
			if (foregroundLocation == ForegroundLocation.LowerRightCorner)
			{
				foregroundLocation2 = ForegroundLocation.LowerLeftCorner;
			}
			else if (foregroundLocation == ForegroundLocation.UpperRightCorner)
			{
				foregroundLocation2 = ForegroundLocation.UpperLeftCorner;
			}
			if (foregroundLocation2 == ForegroundLocation.LowerLeftCorner && BiomeArtDataLibrary.GetArtData(this.m_room.AppearanceBiomeType).ForegroundData.Lower != null)
			{
				array = BiomeArtDataLibrary.GetArtData(this.m_room.AppearanceBiomeType).ForegroundData.Lower.GetForegrounds();
			}
			else if (BiomeArtDataLibrary.GetArtData(this.m_room.AppearanceBiomeType).ForegroundData.Upper != null)
			{
				array = BiomeArtDataLibrary.GetArtData(this.m_room.AppearanceBiomeType).ForegroundData.Upper.GetForegrounds();
			}
			if (this.m_zoomController.OverrideZoomLevel)
			{
				float zoomLevel = this.m_zoomController.ZoomLevel;
			}
			if (array.Length != 0)
			{
				ForegroundGroup randomForegroundGroupInstance = this.GetRandomForegroundGroupInstance(array);
				if (randomForegroundGroupInstance)
				{
					this.m_foregroundGroups.Add(randomForegroundGroupInstance);
					randomForegroundGroupInstance.transform.localPosition = this.GetLocalCornerCoordinates(foregroundLocation);
					for (int i = 0; i < randomForegroundGroupInstance.PropSpawnControllers.Length; i++)
					{
						if (foregroundLocation == ForegroundLocation.LowerRightCorner || foregroundLocation == ForegroundLocation.UpperRightCorner)
						{
							randomForegroundGroupInstance.PropSpawnControllers[i].Mirror();
							randomForegroundGroupInstance.PropSpawnControllers[i].transform.localPosition = RoomUtility.GetMirrorPosition(randomForegroundGroupInstance.PropSpawnControllers[i].transform);
						}
						randomForegroundGroupInstance.PropSpawnControllers[i].SetRoom(this.m_room);
						SpriteRenderer component = randomForegroundGroupInstance.PropSpawnControllers[i].GetComponent<SpriteRenderer>();
						if (component)
						{
							component.enabled = false;
						}
					}
				}
			}
		}

		// Token: 0x0600633E RID: 25406 RVA: 0x00171F24 File Offset: 0x00170124
		private Vector3 GetLocalCornerCoordinates(ForegroundLocation location)
		{
			if (location <= ForegroundLocation.LowerRightCorner)
			{
				if (location == ForegroundLocation.LowerLeftCorner)
				{
					return new Vector3(-0.5f * (float)this.m_room.Size.x * 32f, -0.5f * (float)this.m_room.Size.y * 18f, 0f);
				}
				if (location == ForegroundLocation.LowerRightCorner)
				{
					return new Vector3(0.5f * (float)this.m_room.Size.x * 32f, -0.5f * (float)this.m_room.Size.y * 18f, 0f);
				}
			}
			else
			{
				if (location == ForegroundLocation.UpperLeftCorner)
				{
					return new Vector3(-0.5f * (float)this.m_room.Size.x * 32f, 0.5f * (float)this.m_room.Size.y * 18f, 0f);
				}
				if (location == ForegroundLocation.UpperRightCorner)
				{
					return new Vector3(0.5f * (float)this.m_room.Size.x * 32f, 0.5f * (float)this.m_room.Size.y * 18f, 0f);
				}
			}
			throw new InvalidOperationException();
		}

		// Token: 0x0600633F RID: 25407 RVA: 0x0017208C File Offset: 0x0017028C
		private ForegroundGroup GetRandomForegroundGroupInstance(ForegroundGroup[] potentialForegroundGroups)
		{
			int num = 0;
			if (potentialForegroundGroups.Length > 1)
			{
				num = RNGManager.GetRandomNumber(RngID.Prop, this.ToString(), 0, potentialForegroundGroups.Length);
			}
			return UnityEngine.Object.Instantiate<ForegroundGroup>(potentialForegroundGroups[num], base.transform, false);
		}

		// Token: 0x06006340 RID: 25408 RVA: 0x001720C4 File Offset: 0x001702C4
		private void Mirror()
		{
			bool lowerLeft = this.m_lowerLeft;
			bool lowerRight = this.m_lowerRight;
			bool upperLeft = this.m_upperLeft;
			bool upperRight = this.m_upperRight;
			this.m_lowerLeft = lowerRight;
			this.m_lowerRight = lowerLeft;
			this.m_upperLeft = upperRight;
			this.m_upperRight = upperLeft;
		}

		// Token: 0x06006341 RID: 25409 RVA: 0x0017210C File Offset: 0x0017030C
		private void OnRoomMerged(object sender, EventArgs eventArgs)
		{
			foreach (ForegroundGroup foregroundGroup in this.m_foregroundGroups)
			{
				PropSpawnController[] propSpawnControllers = foregroundGroup.PropSpawnControllers;
				for (int i = 0; i < propSpawnControllers.Length; i++)
				{
					propSpawnControllers[i].gameObject.SetActive(false);
				}
				foregroundGroup.gameObject.SetActive(false);
			}
		}

		// Token: 0x06006342 RID: 25410 RVA: 0x00172188 File Offset: 0x00170388
		private void OnRoomDestroyed(object sender, EventArgs eventArgs)
		{
			this.m_room.RoomMergedRelay.RemoveListener(new Action<object, EventArgs>(this.OnRoomMerged));
			this.m_room.RoomDestroyedRelay.RemoveListener(new Action<object, EventArgs>(this.OnRoomDestroyed));
			this.IsInitialized = false;
		}

		// Token: 0x06006343 RID: 25411 RVA: 0x001721D8 File Offset: 0x001703D8
		public void Initialize()
		{
			if ((GameUtility.IsInLevelEditor && this.m_room != OnPlayManager.CurrentRoom) || BiomeArtDataLibrary.GetArtData(this.m_room.AppearanceBiomeType) == null)
			{
				return;
			}
			if (this.m_room.IsMirrored)
			{
				this.Mirror();
			}
			int num = BiomeArtDataLibrary.GetArtData(this.m_room.AppearanceBiomeType).ForegroundData.BottomSpawnOdds;
			if (this.m_lowerLeft && RNGManager.GetRandomNumber(RngID.Prop, "Spawn LL FG Element?", 0, 100) < num)
			{
				this.CreateForeground(ForegroundLocation.LowerLeftCorner);
			}
			if (this.m_lowerRight && RNGManager.GetRandomNumber(RngID.Prop, "Spawn LR FG Element?", 0, 100) < num)
			{
				this.CreateForeground(ForegroundLocation.LowerRightCorner);
			}
			num = BiomeArtDataLibrary.GetArtData(this.m_room.AppearanceBiomeType).ForegroundData.UpperSpawnOdds;
			if (this.m_upperLeft && RNGManager.GetRandomNumber(RngID.Prop, "Spawn UL FG Element?", 0, 100) < num)
			{
				this.CreateForeground(ForegroundLocation.UpperLeftCorner);
			}
			if (this.m_upperRight && RNGManager.GetRandomNumber(RngID.Prop, "Spawn UR FG Element?", 0, 100) < num)
			{
				this.CreateForeground(ForegroundLocation.UpperRightCorner);
			}
			this.IsInitialized = true;
		}

		// Token: 0x0400510A RID: 20746
		[SerializeField]
		private bool m_lowerLeft;

		// Token: 0x0400510B RID: 20747
		[SerializeField]
		private bool m_lowerRight;

		// Token: 0x0400510C RID: 20748
		[SerializeField]
		private bool m_upperLeft;

		// Token: 0x0400510D RID: 20749
		[SerializeField]
		private bool m_upperRight;

		// Token: 0x0400510E RID: 20750
		[SerializeField]
		private Room m_room;

		// Token: 0x0400510F RID: 20751
		[SerializeField]
		private CameraZoomController m_zoomController;

		// Token: 0x04005112 RID: 20754
		private List<ForegroundGroup> m_foregroundGroups = new List<ForegroundGroup>();
	}
}
