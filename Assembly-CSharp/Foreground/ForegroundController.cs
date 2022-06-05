using System;
using System.Collections.Generic;
using UnityEngine;

namespace Foreground
{
	// Token: 0x02000896 RID: 2198
	public class ForegroundController : MonoBehaviour
	{
		// Token: 0x17001793 RID: 6035
		// (get) Token: 0x060047FD RID: 18429 RVA: 0x00103183 File Offset: 0x00101383
		// (set) Token: 0x060047FE RID: 18430 RVA: 0x0010318B File Offset: 0x0010138B
		public BaseRoom Room { get; private set; }

		// Token: 0x17001794 RID: 6036
		// (get) Token: 0x060047FF RID: 18431 RVA: 0x00103194 File Offset: 0x00101394
		// (set) Token: 0x06004800 RID: 18432 RVA: 0x0010319C File Offset: 0x0010139C
		public bool IsInitialized { get; private set; }

		// Token: 0x06004801 RID: 18433 RVA: 0x001031A5 File Offset: 0x001013A5
		private void Awake()
		{
			this.m_room.RoomMergedRelay.AddListener(new Action<object, EventArgs>(this.OnRoomMerged), false);
			this.m_room.RoomDestroyedRelay.AddListener(new Action<object, EventArgs>(this.OnRoomDestroyed), false);
		}

		// Token: 0x06004802 RID: 18434 RVA: 0x001031E4 File Offset: 0x001013E4
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

		// Token: 0x06004803 RID: 18435 RVA: 0x0010337C File Offset: 0x0010157C
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

		// Token: 0x06004804 RID: 18436 RVA: 0x001034E4 File Offset: 0x001016E4
		private ForegroundGroup GetRandomForegroundGroupInstance(ForegroundGroup[] potentialForegroundGroups)
		{
			int num = 0;
			if (potentialForegroundGroups.Length > 1)
			{
				num = RNGManager.GetRandomNumber(RngID.Prop, this.ToString(), 0, potentialForegroundGroups.Length);
			}
			return UnityEngine.Object.Instantiate<ForegroundGroup>(potentialForegroundGroups[num], base.transform, false);
		}

		// Token: 0x06004805 RID: 18437 RVA: 0x0010351C File Offset: 0x0010171C
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

		// Token: 0x06004806 RID: 18438 RVA: 0x00103564 File Offset: 0x00101764
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

		// Token: 0x06004807 RID: 18439 RVA: 0x001035E0 File Offset: 0x001017E0
		private void OnRoomDestroyed(object sender, EventArgs eventArgs)
		{
			this.m_room.RoomMergedRelay.RemoveListener(new Action<object, EventArgs>(this.OnRoomMerged));
			this.m_room.RoomDestroyedRelay.RemoveListener(new Action<object, EventArgs>(this.OnRoomDestroyed));
			this.IsInitialized = false;
		}

		// Token: 0x06004808 RID: 18440 RVA: 0x00103630 File Offset: 0x00101830
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

		// Token: 0x04003CDC RID: 15580
		[SerializeField]
		private bool m_lowerLeft;

		// Token: 0x04003CDD RID: 15581
		[SerializeField]
		private bool m_lowerRight;

		// Token: 0x04003CDE RID: 15582
		[SerializeField]
		private bool m_upperLeft;

		// Token: 0x04003CDF RID: 15583
		[SerializeField]
		private bool m_upperRight;

		// Token: 0x04003CE0 RID: 15584
		[SerializeField]
		private Room m_room;

		// Token: 0x04003CE1 RID: 15585
		[SerializeField]
		private CameraZoomController m_zoomController;

		// Token: 0x04003CE4 RID: 15588
		private List<ForegroundGroup> m_foregroundGroups = new List<ForegroundGroup>();
	}
}
