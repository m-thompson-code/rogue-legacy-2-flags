using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000B11 RID: 2833
public class CameraController : MonoBehaviour
{
	// Token: 0x17001CDB RID: 7387
	// (get) Token: 0x06005511 RID: 21777 RVA: 0x0002E283 File Offset: 0x0002C483
	public static CinemachineBrain CinemachineBrain
	{
		get
		{
			return CameraController.Instance.m_cinemachineBrain;
		}
	}

	// Token: 0x17001CDC RID: 7388
	// (get) Token: 0x06005512 RID: 21778 RVA: 0x0002E28F File Offset: 0x0002C48F
	// (set) Token: 0x06005513 RID: 21779 RVA: 0x0002E296 File Offset: 0x0002C496
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

	// Token: 0x17001CDD RID: 7389
	// (get) Token: 0x06005514 RID: 21780 RVA: 0x0002E29E File Offset: 0x0002C49E
	// (set) Token: 0x06005515 RID: 21781 RVA: 0x0002E2A6 File Offset: 0x0002C4A6
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

	// Token: 0x17001CDE RID: 7390
	// (get) Token: 0x06005516 RID: 21782 RVA: 0x0002E2AF File Offset: 0x0002C4AF
	public static Camera BackgroundPerspCam
	{
		get
		{
			return CameraController.Instance.m_backgroundPerspFarCam;
		}
	}

	// Token: 0x17001CDF RID: 7391
	// (get) Token: 0x06005517 RID: 21783 RVA: 0x0002E2BB File Offset: 0x0002C4BB
	public static Camera BackgroundOrthoCam
	{
		get
		{
			return CameraController.Instance.m_backgroundOrthoCam;
		}
	}

	// Token: 0x17001CE0 RID: 7392
	// (get) Token: 0x06005518 RID: 21784 RVA: 0x0002E2C7 File Offset: 0x0002C4C7
	public static Camera TraitMaskCam
	{
		get
		{
			return CameraController.Instance.m_traitMaskCamera;
		}
	}

	// Token: 0x17001CE1 RID: 7393
	// (get) Token: 0x06005519 RID: 21785 RVA: 0x0002E2D3 File Offset: 0x0002C4D3
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

	// Token: 0x17001CE2 RID: 7394
	// (get) Token: 0x0600551A RID: 21786 RVA: 0x0002E2DC File Offset: 0x0002C4DC
	public static Camera GameCamera
	{
		get
		{
			return CameraController.Instance.m_gameCam;
		}
	}

	// Token: 0x17001CE3 RID: 7395
	// (get) Token: 0x0600551B RID: 21787 RVA: 0x0002E2E8 File Offset: 0x0002C4E8
	public static Camera ForegroundOrthoCam
	{
		get
		{
			return CameraController.Instance.m_foregroundOrthoCam;
		}
	}

	// Token: 0x17001CE4 RID: 7396
	// (get) Token: 0x0600551C RID: 21788 RVA: 0x0002E2F4 File Offset: 0x0002C4F4
	public static Camera ForegroundPerspCam
	{
		get
		{
			return CameraController.Instance.m_foregroundPerspCam;
		}
	}

	// Token: 0x17001CE5 RID: 7397
	// (get) Token: 0x0600551D RID: 21789 RVA: 0x0002E300 File Offset: 0x0002C500
	public static SoloCameraController SoloCam
	{
		get
		{
			return CameraController.Instance.m_soloCam;
		}
	}

	// Token: 0x17001CE6 RID: 7398
	// (get) Token: 0x0600551E RID: 21790 RVA: 0x0002E30C File Offset: 0x0002C50C
	public static Camera UICamera
	{
		get
		{
			return CameraController.Instance.m_uiCamera;
		}
	}

	// Token: 0x17001CE7 RID: 7399
	// (get) Token: 0x0600551F RID: 21791 RVA: 0x0002E318 File Offset: 0x0002C518
	public static bool IsInstantiated
	{
		get
		{
			return CameraController.m_instance != null;
		}
	}

	// Token: 0x17001CE8 RID: 7400
	// (get) Token: 0x06005520 RID: 21792 RVA: 0x0002E325 File Offset: 0x0002C525
	// (set) Token: 0x06005521 RID: 21793 RVA: 0x0002E32C File Offset: 0x0002C52C
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

	// Token: 0x17001CE9 RID: 7401
	// (get) Token: 0x06005522 RID: 21794 RVA: 0x0002E334 File Offset: 0x0002C534
	public static MobilePostProcessing ForegroundPostProcessing
	{
		get
		{
			return CameraController.Instance.m_foregroundPostProcessing;
		}
	}

	// Token: 0x17001CEA RID: 7402
	// (get) Token: 0x06005523 RID: 21795 RVA: 0x0002E340 File Offset: 0x0002C540
	public static MobilePostProcessing BackgroundPostProcessing
	{
		get
		{
			return CameraController.Instance.m_backgroundPostProcessing;
		}
	}

	// Token: 0x17001CEB RID: 7403
	// (get) Token: 0x06005524 RID: 21796 RVA: 0x0002E34C File Offset: 0x0002C54C
	public static Camera CharacterCamera
	{
		get
		{
			return CameraController.Instance.m_characterCam;
		}
	}

	// Token: 0x17001CEC RID: 7404
	// (get) Token: 0x06005525 RID: 21797 RVA: 0x0002E358 File Offset: 0x0002C558
	public static Vector2 ZPositionRange
	{
		get
		{
			return CameraController.Instance.m_zoomZPositionRange;
		}
	}

	// Token: 0x06005526 RID: 21798 RVA: 0x001419B4 File Offset: 0x0013FBB4
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

	// Token: 0x06005527 RID: 21799 RVA: 0x00141A68 File Offset: 0x0013FC68
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

	// Token: 0x06005528 RID: 21800 RVA: 0x00141B10 File Offset: 0x0013FD10
	private void OnBiomeChange(MonoBehaviour sender, EventArgs eventArgs)
	{
		BiomeEventArgs biomeEventArgs = eventArgs as BiomeEventArgs;
		if (biomeEventArgs != null && biomeEventArgs.Biome != this.CurrentBiomeType)
		{
			this.SetupCamera(biomeEventArgs.Biome, null);
		}
	}

	// Token: 0x06005529 RID: 21801 RVA: 0x0002E364 File Offset: 0x0002C564
	private void SetupCamera(BiomeType biome, BaseRoom room)
	{
		CameraController.SetCameraSettingsForBiome(biome, room);
		this.CurrentBiomeType = biome;
		CameraController.SetPostProcessingProfiles(biome, room);
	}

	// Token: 0x0600552A RID: 21802 RVA: 0x0002E37B File Offset: 0x0002C57B
	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.BiomeEnter, this.m_onBiomeChange);
		Messenger<SceneMessenger, SceneEvent>.RemoveListener(SceneEvent.QualitySettingsChanged, this.m_onQualitySettingsChanged);
		CameraController.m_instance = null;
	}

	// Token: 0x0600552B RID: 21803 RVA: 0x0002E3AD File Offset: 0x0002C5AD
	public static void UpdateRoomCameraSettings(RoomViaDoorEventArgs roomArgs)
	{
		CameraController.Instance.OnPlayerEnterRoom(roomArgs);
	}

	// Token: 0x0600552C RID: 21804 RVA: 0x00141B44 File Offset: 0x0013FD44
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

	// Token: 0x0600552D RID: 21805 RVA: 0x00141D28 File Offset: 0x0013FF28
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

	// Token: 0x0600552E RID: 21806 RVA: 0x00141E30 File Offset: 0x00140030
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

	// Token: 0x0600552F RID: 21807 RVA: 0x00002FCA File Offset: 0x000011CA
	private void OnQualitySettingsChanged(object sender, EventArgs args)
	{
	}

	// Token: 0x06005530 RID: 21808 RVA: 0x0002E3BA File Offset: 0x0002C5BA
	public static float GetVirtualCameraLensSize(float zoomLevel)
	{
		return zoomLevel * 9f;
	}

	// Token: 0x06005531 RID: 21809 RVA: 0x00141E94 File Offset: 0x00140094
	public static float GetZoomLevel(Vector2 confinerSize)
	{
		float result = 1f;
		if (confinerSize.x > 1f && confinerSize.y > 1f)
		{
			result = CameraController.ClampZoomLevel(confinerSize, CameraController.m_instance.m_defaultMaxZoomLevel);
		}
		return result;
	}

	// Token: 0x06005532 RID: 21810 RVA: 0x00141ED4 File Offset: 0x001400D4
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

	// Token: 0x06005533 RID: 21811 RVA: 0x00141F20 File Offset: 0x00140120
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

	// Token: 0x06005534 RID: 21812 RVA: 0x00142098 File Offset: 0x00140298
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

	// Token: 0x06005535 RID: 21813 RVA: 0x00142190 File Offset: 0x00140390
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

	// Token: 0x06005536 RID: 21814 RVA: 0x0014220C File Offset: 0x0014040C
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

	// Token: 0x06005537 RID: 21815 RVA: 0x0002E3C3 File Offset: 0x0002C5C3
	private static void UpdateFogSettings()
	{
		RenderSettings.fogStartDistance = CameraController.ZoomLevel * CameraController.m_fogDistance.x;
		RenderSettings.fogEndDistance = CameraController.ZoomLevel * CameraController.m_fogDistance.y;
	}

	// Token: 0x06005538 RID: 21816 RVA: 0x0002E3EF File Offset: 0x0002C5EF
	public static void PlayKOEffect(float duration)
	{
		CameraController.Instance.PlayKOEffect_Internal(duration);
	}

	// Token: 0x06005539 RID: 21817 RVA: 0x0002E3FC File Offset: 0x0002C5FC
	public static void StopKOEffect()
	{
		CameraController.Instance.StopKOEffect_Internal();
	}

	// Token: 0x0600553A RID: 21818 RVA: 0x0002E408 File Offset: 0x0002C608
	private void PlayKOEffect_Internal(float duration)
	{
		if (CameraController.m_koCoroutine != null)
		{
			CameraController.Instance.StopCoroutine(CameraController.m_koCoroutine);
		}
		CameraController.m_koCoroutine = CameraController.Instance.StartCoroutine(CameraController.Instance.KOEffectCoroutine(duration));
	}

	// Token: 0x0600553B RID: 21819 RVA: 0x001423A4 File Offset: 0x001405A4
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

	// Token: 0x0600553C RID: 21820 RVA: 0x0002E43A File Offset: 0x0002C63A
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

	// Token: 0x0600553D RID: 21821 RVA: 0x0002E450 File Offset: 0x0002C650
	public static void LerpBGPostProcessToProfile(MobilePostProcessingProfile profile, float lerpSpeed)
	{
		CameraController.BackgroundPostProcessing.LerpToProfile(profile, lerpSpeed);
	}

	// Token: 0x0600553E RID: 21822 RVA: 0x0002E45E File Offset: 0x0002C65E
	public static void LerpFGPostProcessToProfile(MobilePostProcessingProfile profile, float lerpSpeed)
	{
		if (TraitManager.IsTraitActive(TraitType.NoColor) || TraitManager.IsTraitActive(TraitType.Oversaturate) || TraitManager.IsTraitActive(TraitType.OldYellowTint))
		{
			return;
		}
		CameraController.ForegroundPostProcessing.LerpToProfile(profile, lerpSpeed);
	}

	// Token: 0x04003F3C RID: 16188
	[SerializeField]
	[Range(1f, 10f)]
	private float m_defaultMaxZoomLevel = 2f;

	// Token: 0x04003F3D RID: 16189
	[SerializeField]
	private Vector2 m_zoomZPositionRange = new Vector2(-28.5f, -56.5f);

	// Token: 0x04003F3E RID: 16190
	[SerializeField]
	private int m_audioListenerOffset;

	// Token: 0x04003F3F RID: 16191
	[SerializeField]
	private Camera m_backgroundPerspFarCam;

	// Token: 0x04003F40 RID: 16192
	[SerializeField]
	private Camera m_backgroundOrthoCam;

	// Token: 0x04003F41 RID: 16193
	[SerializeField]
	private Camera m_foregroundPerspCam;

	// Token: 0x04003F42 RID: 16194
	[SerializeField]
	private Camera m_foregroundOrthoCam;

	// Token: 0x04003F43 RID: 16195
	[SerializeField]
	private Camera m_gameCam;

	// Token: 0x04003F44 RID: 16196
	[SerializeField]
	private Camera m_characterCam;

	// Token: 0x04003F45 RID: 16197
	[SerializeField]
	private SoloCameraController m_soloCam;

	// Token: 0x04003F46 RID: 16198
	[SerializeField]
	private Camera m_uiCamera;

	// Token: 0x04003F47 RID: 16199
	[SerializeField]
	private Camera m_traitMaskCamera;

	// Token: 0x04003F48 RID: 16200
	[SerializeField]
	private CinemachineBrain m_cinemachineBrain;

	// Token: 0x04003F49 RID: 16201
	[SerializeField]
	private MobilePostProcessing m_foregroundPostProcessing;

	// Token: 0x04003F4A RID: 16202
	[SerializeField]
	private MobilePostProcessing m_backgroundPostProcessing;

	// Token: 0x04003F4B RID: 16203
	[SerializeField]
	private Canvas m_koCanvas;

	// Token: 0x04003F4C RID: 16204
	[SerializeField]
	private Transform m_audioListener;

	// Token: 0x04003F4D RID: 16205
	[SerializeField]
	private Color m_greenScreenColor = Color.green;

	// Token: 0x04003F4E RID: 16206
	public const float ABSOLUTE_MAX_ZOOM_LEVEL = 10f;

	// Token: 0x04003F4F RID: 16207
	public const float MIN_ZOOM_LEVEL = 1f;

	// Token: 0x04003F50 RID: 16208
	private static Vector2 MIN_ZOOM_ROOM_SIZE = new Vector2(2f, 2f);

	// Token: 0x04003F51 RID: 16209
	public const float DEFAULT_CAMERA_SIZE = 9f;

	// Token: 0x04003F52 RID: 16210
	public const float DEFAULT_FIELD_OF_VIEW = 35f;

	// Token: 0x04003F53 RID: 16211
	private static Vector3 DEFAULT_RENDER_TEXTURE_SCALE = new Vector3(64f, 32f, 1f);

	// Token: 0x04003F54 RID: 16212
	private static bool INCLUDE_ENEMIES_IN_GREEN_SCREEN = true;

	// Token: 0x04003F55 RID: 16213
	private static CameraController m_instance = null;

	// Token: 0x04003F56 RID: 16214
	private BiomeType m_currentBiomeType;

	// Token: 0x04003F57 RID: 16215
	private static float m_zoomLevel = 1f;

	// Token: 0x04003F58 RID: 16216
	private static CameraZoomChangeEventArgs m_cameraZoomChangeEventArgs = null;

	// Token: 0x04003F59 RID: 16217
	private static Vector2 m_fogDistance;

	// Token: 0x04003F5A RID: 16218
	private static bool m_isGreenScreenOn;

	// Token: 0x04003F5B RID: 16219
	private static CameraClearFlags m_defaultCharacterCameraClearFlags;

	// Token: 0x04003F5C RID: 16220
	private static Color m_defaultCharacterCameraBackgroundColor;

	// Token: 0x04003F5D RID: 16221
	private Action<MonoBehaviour, EventArgs> m_onBiomeChange;

	// Token: 0x04003F5E RID: 16222
	private Action<MonoBehaviour, EventArgs> m_onQualitySettingsChanged;

	// Token: 0x04003F5F RID: 16223
	private static Coroutine m_koCoroutine;
}
