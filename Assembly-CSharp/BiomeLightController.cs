using System;
using UnityEngine;

// Token: 0x020001C1 RID: 449
public class BiomeLightController : MonoBehaviour
{
	// Token: 0x0600121C RID: 4636 RVA: 0x0003447F File Offset: 0x0003267F
	private void Awake()
	{
		this.m_onBiomeChange = new Action<MonoBehaviour, EventArgs>(this.OnBiomeChange);
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
	}

	// Token: 0x0600121D RID: 4637 RVA: 0x000344A5 File Offset: 0x000326A5
	private void Start()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.BiomeEnter, this.m_onBiomeChange);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x0600121E RID: 4638 RVA: 0x000344C0 File Offset: 0x000326C0
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.BiomeEnter, this.m_onBiomeChange);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x0600121F RID: 4639 RVA: 0x000344DC File Offset: 0x000326DC
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

	// Token: 0x06001220 RID: 4640 RVA: 0x0003454C File Offset: 0x0003274C
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

	// Token: 0x06001221 RID: 4641 RVA: 0x00034588 File Offset: 0x00032788
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

	// Token: 0x04001299 RID: 4761
	[SerializeField]
	private Light m_light;

	// Token: 0x0400129A RID: 4762
	[SerializeField]
	[ReadOnly]
	private BiomeType m_currentBiome;

	// Token: 0x0400129B RID: 4763
	private Action<MonoBehaviour, EventArgs> m_onBiomeChange;

	// Token: 0x0400129C RID: 4764
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;
}
