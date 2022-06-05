using System;
using UnityEngine;

// Token: 0x02000334 RID: 820
public class BiomeLightController : MonoBehaviour
{
	// Token: 0x06001A7A RID: 6778 RVA: 0x0000D999 File Offset: 0x0000BB99
	private void Awake()
	{
		this.m_onBiomeChange = new Action<MonoBehaviour, EventArgs>(this.OnBiomeChange);
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
	}

	// Token: 0x06001A7B RID: 6779 RVA: 0x0000D9BF File Offset: 0x0000BBBF
	private void Start()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.BiomeEnter, this.m_onBiomeChange);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06001A7C RID: 6780 RVA: 0x0000D9DA File Offset: 0x0000BBDA
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.BiomeEnter, this.m_onBiomeChange);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06001A7D RID: 6781 RVA: 0x00091E50 File Offset: 0x00090050
	private void OnPlayerEnterRoom(MonoBehaviour sender, EventArgs eventArgs)
	{
		RoomViaDoorEventArgs roomViaDoorEventArgs = eventArgs as RoomViaDoorEventArgs;
		if (roomViaDoorEventArgs != null)
		{
			BiomeType appearanceBiomeType = roomViaDoorEventArgs.Room.AppearanceBiomeType;
			if (this.m_currentBiome != appearanceBiomeType || roomViaDoorEventArgs.Room.BiomeArtDataOverride)
			{
				this.UpdateLights(appearanceBiomeType, roomViaDoorEventArgs.Room);
				if (roomViaDoorEventArgs.Room.BiomeArtDataOverride)
				{
					this.m_currentBiome = BiomeType.None;
					return;
				}
			}
			return;
		}
		throw new InvalidCastException("Failed to cast eventArgs as RoomViaDoorEventArgs");
	}

	// Token: 0x06001A7E RID: 6782 RVA: 0x00091EC0 File Offset: 0x000900C0
	private void OnBiomeChange(MonoBehaviour sender, EventArgs eventArgs)
	{
		BiomeEventArgs biomeEventArgs = eventArgs as BiomeEventArgs;
		if (biomeEventArgs != null)
		{
			this.UpdateLights(biomeEventArgs.Biome, null);
			return;
		}
		Debug.LogFormat("<color=red>[{0}] Failed to cast eventArgs as BiomeChangeEventArgs</color>", new object[]
		{
			this
		});
	}

	// Token: 0x06001A7F RID: 6783 RVA: 0x00091EFC File Offset: 0x000900FC
	private void UpdateLights(BiomeType biome, BaseRoom room)
	{
		BiomeArtData biomeArtData = null;
		if (!room.IsNativeNull())
		{
			biomeArtData = room.BiomeArtDataOverride;
		}
		if (!biomeArtData)
		{
			biomeArtData = BiomeArtDataLibrary.GetArtData(biome);
		}
		if (biomeArtData)
		{
			if (biomeArtData.LightAndFogData != null)
			{
				Light lightMasterPrefab = biomeArtData.LightAndFogData.LightMasterPrefab;
				if (lightMasterPrefab)
				{
					this.m_currentBiome = biome;
					this.m_light.transform.rotation = lightMasterPrefab.transform.rotation;
					this.m_light.color = lightMasterPrefab.color;
					this.m_light.intensity = lightMasterPrefab.intensity;
					this.m_light.cullingMask = lightMasterPrefab.cullingMask;
					this.m_light.shadows = lightMasterPrefab.shadows;
					this.m_light.shadowStrength = lightMasterPrefab.shadowStrength;
					this.m_light.shadowNormalBias = lightMasterPrefab.shadowNormalBias;
					this.m_light.shadowBias = lightMasterPrefab.shadowBias;
					this.m_light.renderMode = lightMasterPrefab.renderMode;
					return;
				}
				Debug.LogFormat("<color=red>[{0}] Light Master Prefab field is null in Light And Fog Data on ({1})</color>", new object[]
				{
					this,
					biomeArtData
				});
				return;
			}
			else
			{
				Debug.LogFormat("<color=red>[{0}] Light And Fog Data is null on ({1})</color>", new object[]
				{
					this,
					biomeArtData
				});
			}
		}
	}

	// Token: 0x040018AF RID: 6319
	[SerializeField]
	private Light m_light;

	// Token: 0x040018B0 RID: 6320
	[SerializeField]
	[ReadOnly]
	private BiomeType m_currentBiome;

	// Token: 0x040018B1 RID: 6321
	private Action<MonoBehaviour, EventArgs> m_onBiomeChange;

	// Token: 0x040018B2 RID: 6322
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;
}
