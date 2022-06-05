using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200068C RID: 1676
public class CameraController : MonoBehaviour
{
	// Token: 0x17001513 RID: 5395
	// (get) Token: 0x06003C92 RID: 15506 RVA: 0x000D14B3 File Offset: 0x000CF6B3
	public static CinemachineBrain CinemachineBrain
	{
		get
		{
			return CameraController.Instance.m_cinemachineBrain;
		}
	}

	// Token: 0x17001514 RID: 5396
	// (get) Token: 0x06003C93 RID: 15507 RVA: 0x000D14BF File Offset: 0x000CF6BF
	// (set) Token: 0x06003C94 RID: 15508 RVA: 0x000D14C6 File Offset: 0x000CF6C6
	private static CameraController Instance
	{
		get
		{
			return CameraController.m_instance;
		}
		set
		{
			CameraController.m_instance = value;
		}
	}

	// Token: 0x17001515 RID: 5397
	// (get) Token: 0x06003C95 RID: 15509 RVA: 0x000D14CE File Offset: 0x000CF6CE
	// (set) Token: 0x06003C96 RID: 15510 RVA: 0x000D14D6 File Offset: 0x000CF6D6
	public BiomeType CurrentBiomeType
	{
		get
		{
			return this.m_currentBiomeType;
		}
		private set
		{
			this.m_currentBiomeType = value;
		}
	}

	// Token: 0x17001516 RID: 5398
	// (get) Token: 0x06003C97 RID: 15511 RVA: 0x000D14DF File Offset: 0x000CF6DF
	public static Camera BackgroundPerspCam
	{
		get
		{
			return CameraController.Instance.m_backgroundPerspFarCam;
		}
	}

	// Token: 0x17001517 RID: 5399
	// (get) Token: 0x06003C98 RID: 15512 RVA: 0x000D14EB File Offset: 0x000CF6EB
	public static Camera BackgroundOrthoCam
	{
		get
		{
			return CameraController.Instance.m_backgroundOrthoCam;
		}
	}

	// Token: 0x17001518 RID: 5400
	// (get) Token: 0x06003C99 RID: 15513 RVA: 0x000D14F7 File Offset: 0x000CF6F7
	public static Camera TraitMaskCam
	{
		get
		{
			return CameraController.Instance.m_traitMaskCamera;
		}
	}

	// Token: 0x17001519 RID: 5401
	// (get) Token: 0x06003C9A RID: 15514 RVA: 0x000D1503 File Offset: 0x000CF703
	public static IEnumerable<Camera> Cameras
	{
		get
		{
			yield return CameraController.BackgroundPerspCam;
			yield return CameraController.BackgroundOrthoCam;
			yield return CameraController.ForegroundPerspCam;
			yield return CameraController.ForegroundOrthoCam;
			yield return CameraController.CharacterCamera;
			yield return CameraController.TraitMaskCam;
			yield break;
		}
	}

	// Token: 0x1700151A RID: 5402
	// (get) Token: 0x06003C9B RID: 15515 RVA: 0x000D150C File Offset: 0x000CF70C
	public static Camera GameCamera
	{
		get
		{
			return CameraController.Instance.m_gameCam;
		}
	}

	// Token: 0x1700151B RID: 5403
	// (get) Token: 0x06003C9C RID: 15516 RVA: 0x000D1518 File Offset: 0x000CF718
	public static Camera ForegroundOrthoCam
	{
		get
		{
			return CameraController.Instance.m_foregroundOrthoCam;
		}
	}

	// Token: 0x1700151C RID: 5404
	// (get) Token: 0x06003C9D RID: 15517 RVA: 0x000D1524 File Offset: 0x000CF724
	public static Camera ForegroundPerspCam
	{
		get
		{
			return CameraController.Instance.m_foregroundPerspCam;
		}
	}

	// Token: 0x1700151D RID: 5405
	// (get) Token: 0x06003C9E RID: 15518 RVA: 0x000D1530 File Offset: 0x000CF730
	public static SoloCameraController SoloCam
	{
		get
		{
			return CameraController.Instance.m_soloCam;
		}
	}

	// Token: 0x1700151E RID: 5406
	// (get) Token: 0x06003C9F RID: 15519 RVA: 0x000D153C File Offset: 0x000CF73C
	public static Camera UICamera
	{
		get
		{
			return CameraController.Instance.m_uiCamera;
		}
	}

	// Token: 0x1700151F RID: 5407
	// (get) Token: 0x06003CA0 RID: 15520 RVA: 0x000D1548 File Offset: 0x000CF748
	public static bool IsInstantiated
	{
		get
		{
			return CameraController.m_instance != null;
		}
	}

	// Token: 0x17001520 RID: 5408
	// (get) Token: 0x06003CA1 RID: 15521 RVA: 0x000D1555 File Offset: 0x000CF755
	// (set) Token: 0x06003CA2 RID: 15522 RVA: 0x000D155C File Offset: 0x000CF75C
	public static float ZoomLevel
	{
		get
		{
			return CameraController.m_zoomLevel;
		}
		private set
		{
			CameraController.m_zoomLevel = value;
		}
	}

	// Token: 0x17001521 RID: 5409
	// (get) Token: 0x06003CA3 RID: 15523 RVA: 0x000D1564 File Offset: 0x000CF764
	public static MobilePostProcessing ForegroundPostProcessing
	{
		get
		{
			return CameraController.Instance.m_foregroundPostProcessing;
		}
	}

	// Token: 0x17001522 RID: 5410
	// (get) Token: 0x06003CA4 RID: 15524 RVA: 0x000D1570 File Offset: 0x000CF770
	public static MobilePostProcessing BackgroundPostProcessing
	{
		get
		{
			return CameraController.Instance.m_backgroundPostProcessing;
		}
	}

	// Token: 0x17001523 RID: 5411
	// (get) Token: 0x06003CA5 RID: 15525 RVA: 0x000D157C File Offset: 0x000CF77C
	public static Camera CharacterCamera
	{
		get
		{
			return CameraController.Instance.m_characterCam;
		}
	}

	// Token: 0x17001524 RID: 5412
	// (get) Token: 0x06003CA6 RID: 15526 RVA: 0x000D1588 File Offset: 0x000CF788
	public static Vector2 ZPositionRange
	{
		get
		{
			return CameraController.Instance.m_zoomZPositionRange;
		}
	}

	// Token: 0x06003CA7 RID: 15527 RVA: 0x000D1594 File Offset: 0x000CF794
	private void Awake()
	{
		if (!(CameraController.Instance == null))
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		CameraController.Instance = this;
		if (this.m_defaultMaxZoomLevel < 1f)
		{
			throw new ArgumentOutOfRangeException("m_maxZoomLevel", string.Format("| {0} | Max Zoom Level must be greater than 1", this));
		}
		if (CameraController.ZPositionRange.x <= CameraController.ZPositionRange.y)
		{
			throw new ArgumentOutOfRangeException("m_zoomZPositionRange", string.Format("| {0} | Zoom Z Position Range's x value must be greater than its y value", this));
		}
		SceneManager.sceneLoaded += this.OnSceneLoaded;
		this.m_onBiomeChange = new Action<MonoBehaviour, EventArgs>(this.OnBiomeChange);
		this.m_onQualitySettingsChanged = new Action<MonoBehaviour, EventArgs>(this.OnQualitySettingsChanged);
	}

	// Token: 0x06003CA8 RID: 15528 RVA: 0x000D1648 File Offset: 0x000CF848
	private void Start()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.BiomeEnter, this.m_onBiomeChange);
		Messenger<SceneMessenger, SceneEvent>.AddListener(SceneEvent.QualitySettingsChanged, this.m_onQualitySettingsChanged);
		foreach (Camera camera2 in from camera in CameraController.Cameras
		where !camera.orthographic
		select camera)
		{
			camera2.transparencySortMode = TransparencySortMode.Orthographic;
		}
		this.m_audioListener.localPosition = new Vector3(0f, 0f, (float)this.m_audioListenerOffset);
	}

	// Token: 0x06003CA9 RID: 15529 RVA: 0x000D16F0 File Offset: 0x000CF8F0
	private void OnBiomeChange(MonoBehaviour sender, EventArgs eventArgs)
	{
		BiomeEventArgs biomeEventArgs = eventArgs as BiomeEventArgs;
		if (biomeEventArgs != null && biomeEventArgs.Biome != this.CurrentBiomeType)
		{
			this.SetupCamera(biomeEventArgs.Biome, null);
		}
	}

	// Token: 0x06003CAA RID: 15530 RVA: 0x000D1722 File Offset: 0x000CF922
	private void SetupCamera(BiomeType biome, BaseRoom room)
	{
		CameraController.SetCameraSettingsForBiome(biome, room);
		this.CurrentBiomeType = biome;
		CameraController.SetPostProcessingProfiles(biome, room);
	}

	// Token: 0x06003CAB RID: 15531 RVA: 0x000D1739 File Offset: 0x000CF939
	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.BiomeEnter, this.m_onBiomeChange);
		Messenger<SceneMessenger, SceneEvent>.RemoveListener(SceneEvent.QualitySettingsChanged, this.m_onQualitySettingsChanged);
		CameraController.m_instance = null;
	}

	// Token: 0x06003CAC RID: 15532 RVA: 0x000D176B File Offset: 0x000CF96B
	public static void UpdateRoomCameraSettings(RoomViaDoorEventArgs roomArgs)
	{
		CameraController.Instance.OnPlayerEnterRoom(roomArgs);
	}

	// Token: 0x06003CAD RID: 15533 RVA: 0x000D1778 File Offset: 0x000CF978
	private void OnPlayerEnterRoom(RoomViaDoorEventArgs args)
	{
		if (args != null)
		{
			bool flag = false;
			if (args.Room is Room)
			{
				Room room = args.Room as Room;
				if (room.GridPointManager != null && room.GridPointManager.RoomMetaData != null)
				{
					SpecialRoomType specialRoomType = room.GridPointManager.RoomMetaData.SpecialRoomType;
					if (GameUtility.IsInLevelEditor)
					{
						flag = room.gameObject.GetComponent<HeirloomBiomeOverrideController>();
					}
					else
					{
						flag = (room.GridPointManager.IsTunnelDestination && specialRoomType == SpecialRoomType.Heirloom);
					}
				}
			}
			if (flag)
			{
				this.SetupCamera(BiomeType.Castle, args.Room);
			}
			else
			{
				BiomeType appearanceBiomeType = args.Room.AppearanceBiomeType;
				if (this.CurrentBiomeType != appearanceBiomeType || args.Room.BiomeArtDataOverride)
				{
					this.SetupCamera(appearanceBiomeType, args.Room);
					if (args.Room.BiomeArtDataOverride)
					{
						this.m_currentBiomeType = BiomeType.None;
					}
				}
			}
			CameraZoomController component = args.Room.gameObject.GetComponent<CameraZoomController>();
			float num = 1f;
			if (component)
			{
				num = CameraController.ClampZoomLevel(args.Room.CinemachineCamera.ConfinerSize, component.ZoomLevel);
			}
			CameraController.SetCameraZoomLevel(num);
			args.Room.CinemachineCamera.SetLensSize(CameraController.GetVirtualCameraLensSize(num));
			CameraOffsetOverrideController component2 = args.Room.gameObject.GetComponent<CameraOffsetOverrideController>();
			if (component2)
			{
				component2.ApplyOffset(args.Room.CinemachineCamera.VirtualCamera);
			}
			args.Room.CinemachineCamera.SetIsActiveCamera(true);
			args.Room.CinemachineCamera.VirtualCamera.InternalUpdateCameraState(Vector3.up, 1f);
			if (CameraController.CinemachineBrain.enabled)
			{
				CinemachineBrain.UpdateMethod updateMethod = CameraController.CinemachineBrain.m_UpdateMethod;
				CameraController.CinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.ManualUpdate;
				CameraController.CinemachineBrain.ManualUpdate();
				CameraController.CinemachineBrain.m_UpdateMethod = updateMethod;
			}
		}
	}

	// Token: 0x06003CAE RID: 15534 RVA: 0x000D195C File Offset: 0x000CFB5C
	private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
	{
		this.m_cinemachineBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
		CameraController.SoloCam.transform.localPosition = Vector3.zero;
		CameraController.GameCamera.orthographicSize = CameraController.GetVirtualCameraLensSize(1f);
		CameraController.SetCameraZoomLevel(1f);
		this.CurrentBiomeType = BiomeType.None;
		if (!GameUtility.SceneHasRooms)
		{
			using (IEnumerator<Camera> enumerator = CameraController.Cameras.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Camera camera = enumerator.Current;
					if (camera.gameObject.activeSelf)
					{
						camera.gameObject.SetActive(false);
					}
				}
				goto IL_D8;
			}
		}
		foreach (Camera camera2 in CameraController.Cameras)
		{
			if (!camera2.gameObject.activeSelf)
			{
				camera2.gameObject.SetActive(true);
			}
		}
		IL_D8:
		CameraController.StopKOEffect();
	}

	// Token: 0x06003CAF RID: 15535 RVA: 0x000D1A64 File Offset: 0x000CFC64
	private static void SetPostProcessingProfiles(BiomeType biome, BaseRoom room)
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
		CameraController.ForegroundPostProcessing.SetBaseProfile(biomeArtData.PostProcessingData.ForegroundProfile);
		CameraController.BackgroundPostProcessing.SetBaseProfile(biomeArtData.PostProcessingData.BackgroundProfile);
		CameraController.Instance.OnQualitySettingsChanged(null, null);
	}

	// Token: 0x06003CB0 RID: 15536 RVA: 0x000D1AC7 File Offset: 0x000CFCC7
	private void OnQualitySettingsChanged(object sender, EventArgs args)
	{
	}

	// Token: 0x06003CB1 RID: 15537 RVA: 0x000D1AC9 File Offset: 0x000CFCC9
	public static float GetVirtualCameraLensSize(float zoomLevel)
	{
		return zoomLevel * 9f;
	}

	// Token: 0x06003CB2 RID: 15538 RVA: 0x000D1AD4 File Offset: 0x000CFCD4
	public static float GetZoomLevel(Vector2 confinerSize)
	{
		float result = 1f;
		if (confinerSize.x > 1f && confinerSize.y > 1f)
		{
			result = CameraController.ClampZoomLevel(confinerSize, CameraController.m_instance.m_defaultMaxZoomLevel);
		}
		return result;
	}

	// Token: 0x06003CB3 RID: 15539 RVA: 0x000D1B14 File Offset: 0x000CFD14
	private static float ClampZoomLevel(Vector2 confinerSize, float zoomLevel)
	{
		if (confinerSize.x > 1f && confinerSize.y > 1f)
		{
			float b = Mathf.Min(confinerSize.x, confinerSize.y);
			zoomLevel = Mathf.Min(zoomLevel, b);
		}
		else
		{
			zoomLevel = 1f;
		}
		return zoomLevel;
	}

	// Token: 0x06003CB4 RID: 15540 RVA: 0x000D1B60 File Offset: 0x000CFD60
	public static void ToggleGreenScreenIsOn()
	{
		CameraController.m_isGreenScreenOn = !CameraController.m_isGreenScreenOn;
		if (CameraController.m_defaultCharacterCameraClearFlags == (CameraClearFlags)0)
		{
			CameraController.m_defaultCharacterCameraClearFlags = CameraController.CharacterCamera.clearFlags;
			CameraController.m_defaultCharacterCameraBackgroundColor = CameraController.CharacterCamera.backgroundColor;
		}
		CameraClearFlags clearFlags = CameraController.m_defaultCharacterCameraClearFlags;
		Color backgroundColor = CameraController.m_defaultCharacterCameraBackgroundColor;
		if (CameraController.m_isGreenScreenOn)
		{
			clearFlags = CameraClearFlags.Color;
			backgroundColor = CameraController.Instance.m_greenScreenColor;
		}
		CameraController.CharacterCamera.clearFlags = clearFlags;
		CameraController.CharacterCamera.backgroundColor = backgroundColor;
		foreach (Camera camera in CameraController.Cameras)
		{
			if (camera != CameraController.CharacterCamera)
			{
				camera.enabled = !CameraController.m_isGreenScreenOn;
			}
		}
		CameraController.UICamera.enabled = !CameraController.m_isGreenScreenOn;
		if (!CameraController.INCLUDE_ENEMIES_IN_GREEN_SCREEN)
		{
			EnemyController[] array = UnityEngine.Object.FindObjectsOfType<EnemyController>();
			for (int i = 0; i < array.Length; i++)
			{
				Renderer[] componentsInChildren = array[i].gameObject.GetComponentsInChildren<Renderer>(true);
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					componentsInChildren[j].enabled = !CameraController.m_isGreenScreenOn;
				}
			}
		}
		foreach (AddToGreenScreen addToGreenScreen in UnityEngine.Object.FindObjectsOfType<AddToGreenScreen>())
		{
			if (CameraController.m_isGreenScreenOn)
			{
				addToGreenScreen.gameObject.SetLayerRecursively(29, false);
			}
			else
			{
				addToGreenScreen.ResetLayers();
			}
		}
	}

	// Token: 0x06003CB5 RID: 15541 RVA: 0x000D1CD8 File Offset: 0x000CFED8
	public static void SetCameraSettingsForBiome(BiomeType biome, BaseRoom room)
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
		if (!biomeArtData)
		{
			Debug.LogFormat("<color=red>No Biome Art Data for Biome ({0})</color>", new object[]
			{
				biome
			});
			return;
		}
		if (biomeArtData.LightAndFogData != null)
		{
			AmbientLightAndFogBiomeArtData lightAndFogData = biomeArtData.LightAndFogData;
			RenderSettings.ambientLight = lightAndFogData.AmbientLightColor;
			RenderSettings.fog = lightAndFogData.Fog;
			RenderSettings.fogMode = lightAndFogData.FogMode;
			RenderSettings.fogColor = lightAndFogData.FogColor;
			RenderSettings.fogDensity = lightAndFogData.FogDensity;
			RenderSettings.fogStartDistance = lightAndFogData.FogStartDistance;
			RenderSettings.fogEndDistance = lightAndFogData.FogEndDistance;
			CameraController.m_fogDistance = new Vector2(lightAndFogData.FogStartDistance, lightAndFogData.FogEndDistance);
			RenderSettings.ambientSkyColor = lightAndFogData.AmbientLightColor;
			RenderSettings.ambientMode = lightAndFogData.AmbientMode;
			return;
		}
		Debug.LogFormat("<color=red>Light And Fog Data is null on Biome Art Data ({0})</color>", new object[]
		{
			biomeArtData.name
		});
	}

	// Token: 0x06003CB6 RID: 15542 RVA: 0x000D1DD0 File Offset: 0x000CFFD0
	public static void SetCameraEnabled(int cameraIndex, bool isEnabled)
	{
		Camera camera = null;
		switch (cameraIndex)
		{
		case 0:
			camera = CameraController.BackgroundPerspCam;
			break;
		case 1:
			camera = CameraController.BackgroundOrthoCam;
			break;
		case 2:
			camera = CameraController.GameCamera;
			break;
		case 3:
			camera = CameraController.ForegroundOrthoCam;
			break;
		case 4:
			camera = CameraController.ForegroundPerspCam;
			break;
		case 5:
			camera = CameraController.CharacterCamera;
			break;
		case 6:
			camera = CameraController.UICamera;
			break;
		case 7:
			camera = CameraController.TraitMaskCam;
			break;
		}
		camera.enabled = isEnabled;
	}

	// Token: 0x06003CB7 RID: 15543 RVA: 0x000D1E4C File Offset: 0x000D004C
	private static void SetCameraZoomLevel(float zoomLevel)
	{
		float zoomLevel2 = CameraController.ZoomLevel;
		CameraController.ZoomLevel = zoomLevel;
		float virtualCameraLensSize = CameraController.GetVirtualCameraLensSize(CameraController.ZoomLevel);
		foreach (Camera camera3 in from camera in CameraController.Cameras
		where camera.orthographic
		select camera)
		{
			camera3.orthographicSize = virtualCameraLensSize;
		}
		float t = 0f;
		if (CameraController.m_instance.m_defaultMaxZoomLevel > 1f)
		{
			t = (CameraController.ZoomLevel - 1f) / 9f;
		}
		foreach (Camera camera2 in from camera in CameraController.Cameras
		where !camera.orthographic
		select camera)
		{
			float z = Mathf.Lerp(CameraController.ZPositionRange.x, CameraController.ZPositionRange.y, t);
			camera2.transform.position = new Vector3(camera2.transform.position.x, camera2.transform.position.y, z);
		}
		if (GameUtility.SceneHasRooms)
		{
			CameraController.UpdateFogSettings();
		}
		if (CameraController.m_cameraZoomChangeEventArgs == null)
		{
			CameraController.m_cameraZoomChangeEventArgs = new CameraZoomChangeEventArgs(zoomLevel, zoomLevel2);
		}
		else
		{
			CameraController.m_cameraZoomChangeEventArgs.Initialize(zoomLevel, zoomLevel2);
		}
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.CameraZoomChange, CameraController.Instance, CameraController.m_cameraZoomChangeEventArgs);
	}

	// Token: 0x06003CB8 RID: 15544 RVA: 0x000D1FE4 File Offset: 0x000D01E4
	private static void UpdateFogSettings()
	{
		RenderSettings.fogStartDistance = CameraController.ZoomLevel * CameraController.m_fogDistance.x;
		RenderSettings.fogEndDistance = CameraController.ZoomLevel * CameraController.m_fogDistance.y;
	}

	// Token: 0x06003CB9 RID: 15545 RVA: 0x000D2010 File Offset: 0x000D0210
	public static void PlayKOEffect(float duration)
	{
		CameraController.Instance.PlayKOEffect_Internal(duration);
	}

	// Token: 0x06003CBA RID: 15546 RVA: 0x000D201D File Offset: 0x000D021D
	public static void StopKOEffect()
	{
		CameraController.Instance.StopKOEffect_Internal();
	}

	// Token: 0x06003CBB RID: 15547 RVA: 0x000D2029 File Offset: 0x000D0229
	private void PlayKOEffect_Internal(float duration)
	{
		if (CameraController.m_koCoroutine != null)
		{
			CameraController.Instance.StopCoroutine(CameraController.m_koCoroutine);
		}
		CameraController.m_koCoroutine = CameraController.Instance.StartCoroutine(CameraController.Instance.KOEffectCoroutine(duration));
	}

	// Token: 0x06003CBC RID: 15548 RVA: 0x000D205C File Offset: 0x000D025C
	private void StopKOEffect_Internal()
	{
		if (CameraController.m_koCoroutine != null)
		{
			CameraController.Instance.StopCoroutine(CameraController.m_koCoroutine);
		}
		CameraController.m_koCoroutine = null;
		if (this.m_koCanvas.gameObject.activeSelf)
		{
			this.m_koCanvas.gameObject.SetActive(false);
		}
	}

	// Token: 0x06003CBD RID: 15549 RVA: 0x000D20A8 File Offset: 0x000D02A8
	private IEnumerator KOEffectCoroutine(float duration)
	{
		this.m_koCanvas.gameObject.SetActive(true);
		duration += Time.unscaledTime;
		while (Time.unscaledTime < duration)
		{
			yield return null;
		}
		this.m_koCanvas.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06003CBE RID: 15550 RVA: 0x000D20BE File Offset: 0x000D02BE
	public static void LerpBGPostProcessToProfile(MobilePostProcessingProfile profile, float lerpSpeed)
	{
		CameraController.BackgroundPostProcessing.LerpToProfile(profile, lerpSpeed);
	}

	// Token: 0x06003CBF RID: 15551 RVA: 0x000D20CC File Offset: 0x000D02CC
	public static void LerpFGPostProcessToProfile(MobilePostProcessingProfile profile, float lerpSpeed)
	{
		if (TraitManager.IsTraitActive(TraitType.NoColor) || TraitManager.IsTraitActive(TraitType.Oversaturate) || TraitManager.IsTraitActive(TraitType.OldYellowTint))
		{
			return;
		}
		CameraController.ForegroundPostProcessing.LerpToProfile(profile, lerpSpeed);
	}

	// Token: 0x04002D7E RID: 11646
	[SerializeField]
	[Range(1f, 10f)]
	private float m_defaultMaxZoomLevel = 2f;

	// Token: 0x04002D7F RID: 11647
	[SerializeField]
	private Vector2 m_zoomZPositionRange = new Vector2(-28.5f, -56.5f);

	// Token: 0x04002D80 RID: 11648
	[SerializeField]
	private int m_audioListenerOffset;

	// Token: 0x04002D81 RID: 11649
	[SerializeField]
	private Camera m_backgroundPerspFarCam;

	// Token: 0x04002D82 RID: 11650
	[SerializeField]
	private Camera m_backgroundOrthoCam;

	// Token: 0x04002D83 RID: 11651
	[SerializeField]
	private Camera m_foregroundPerspCam;

	// Token: 0x04002D84 RID: 11652
	[SerializeField]
	private Camera m_foregroundOrthoCam;

	// Token: 0x04002D85 RID: 11653
	[SerializeField]
	private Camera m_gameCam;

	// Token: 0x04002D86 RID: 11654
	[SerializeField]
	private Camera m_characterCam;

	// Token: 0x04002D87 RID: 11655
	[SerializeField]
	private SoloCameraController m_soloCam;

	// Token: 0x04002D88 RID: 11656
	[SerializeField]
	private Camera m_uiCamera;

	// Token: 0x04002D89 RID: 11657
	[SerializeField]
	private Camera m_traitMaskCamera;

	// Token: 0x04002D8A RID: 11658
	[SerializeField]
	private CinemachineBrain m_cinemachineBrain;

	// Token: 0x04002D8B RID: 11659
	[SerializeField]
	private MobilePostProcessing m_foregroundPostProcessing;

	// Token: 0x04002D8C RID: 11660
	[SerializeField]
	private MobilePostProcessing m_backgroundPostProcessing;

	// Token: 0x04002D8D RID: 11661
	[SerializeField]
	private Canvas m_koCanvas;

	// Token: 0x04002D8E RID: 11662
	[SerializeField]
	private Transform m_audioListener;

	// Token: 0x04002D8F RID: 11663
	[SerializeField]
	private Color m_greenScreenColor = Color.green;

	// Token: 0x04002D90 RID: 11664
	public const float ABSOLUTE_MAX_ZOOM_LEVEL = 10f;

	// Token: 0x04002D91 RID: 11665
	public const float MIN_ZOOM_LEVEL = 1f;

	// Token: 0x04002D92 RID: 11666
	private static Vector2 MIN_ZOOM_ROOM_SIZE = new Vector2(2f, 2f);

	// Token: 0x04002D93 RID: 11667
	public const float DEFAULT_CAMERA_SIZE = 9f;

	// Token: 0x04002D94 RID: 11668
	public const float DEFAULT_FIELD_OF_VIEW = 35f;

	// Token: 0x04002D95 RID: 11669
	private static Vector3 DEFAULT_RENDER_TEXTURE_SCALE = new Vector3(64f, 32f, 1f);

	// Token: 0x04002D96 RID: 11670
	private static bool INCLUDE_ENEMIES_IN_GREEN_SCREEN = true;

	// Token: 0x04002D97 RID: 11671
	private static CameraController m_instance = null;

	// Token: 0x04002D98 RID: 11672
	private BiomeType m_currentBiomeType;

	// Token: 0x04002D99 RID: 11673
	private static float m_zoomLevel = 1f;

	// Token: 0x04002D9A RID: 11674
	private static CameraZoomChangeEventArgs m_cameraZoomChangeEventArgs = null;

	// Token: 0x04002D9B RID: 11675
	private static Vector2 m_fogDistance;

	// Token: 0x04002D9C RID: 11676
	private static bool m_isGreenScreenOn;

	// Token: 0x04002D9D RID: 11677
	private static CameraClearFlags m_defaultCharacterCameraClearFlags;

	// Token: 0x04002D9E RID: 11678
	private static Color m_defaultCharacterCameraBackgroundColor;

	// Token: 0x04002D9F RID: 11679
	private Action<MonoBehaviour, EventArgs> m_onBiomeChange;

	// Token: 0x04002DA0 RID: 11680
	private Action<MonoBehaviour, EventArgs> m_onQualitySettingsChanged;

	// Token: 0x04002DA1 RID: 11681
	private static Coroutine m_koCoroutine;
}
