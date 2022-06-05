using System;
using System.Collections.Generic;
using Rewired;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001E0 RID: 480
public class CameraDebugController : WindowController
{
	// Token: 0x17000A66 RID: 2662
	// (get) Token: 0x060013E5 RID: 5093 RVA: 0x0003C523 File Offset: 0x0003A723
	public override WindowID ID
	{
		get
		{
			return WindowID.CameraDebug;
		}
	}

	// Token: 0x060013E6 RID: 5094 RVA: 0x0003C528 File Offset: 0x0003A728
	private void Start()
	{
		this.m_backgroundPersp.onValueChanged.AddListener(new UnityAction<bool>(this.OnBackgroundPerspToggled));
		this.m_backgroundOrtho.onValueChanged.AddListener(new UnityAction<bool>(this.OnBackgroundOrthoToggled));
		this.m_game.onValueChanged.AddListener(new UnityAction<bool>(this.OnGameToggled));
		this.m_foregroundOrtho.onValueChanged.AddListener(new UnityAction<bool>(this.OnForegroundOrthoToggled));
		this.m_foregroundPersp.onValueChanged.AddListener(new UnityAction<bool>(this.OnForegroundPerspToggled));
		this.m_character.onValueChanged.AddListener(new UnityAction<bool>(this.OnCharacterToggled));
		this.m_ui.onValueChanged.AddListener(new UnityAction<bool>(this.OnUIToggled));
		this.m_traitMask.onValueChanged.AddListener(new UnityAction<bool>(this.OnTraitMaskToggled));
		this.m_map.onValueChanged.AddListener(new UnityAction<bool>(this.OnMapToggled));
		this.m_letterbox.onValueChanged.AddListener(new UnityAction<bool>(this.OnLetterboxToggled));
		this.m_weather.onValueChanged.AddListener(new UnityAction<bool>(this.OnWeatherToggled));
		this.m_sky.onValueChanged.AddListener(new UnityAction<bool>(this.OnSkyToggled));
		this.m_enableProps.onValueChanged.AddListener(new UnityAction<bool>(this.OnPropsToggled));
		this.m_enableTerrain.onValueChanged.AddListener(new UnityAction<bool>(this.OnTerrainToggled));
		this.m_executeButton.onClick.AddListener(new UnityAction(this.OnExecuteButtonPressed));
		this.m_layerDropdown.onValueChanged.AddListener(new UnityAction<int>(this.OnDropdownChanged));
	}

	// Token: 0x060013E7 RID: 5095 RVA: 0x0003C6F5 File Offset: 0x0003A8F5
	protected override void OnOpen()
	{
		this.m_windowCanvas.gameObject.SetActive(true);
		this.m_backgroundPersp.Select();
	}

	// Token: 0x060013E8 RID: 5096 RVA: 0x0003C713 File Offset: 0x0003A913
	protected override void OnClose()
	{
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x060013E9 RID: 5097 RVA: 0x0003C726 File Offset: 0x0003A926
	protected override void OnFocus()
	{
		base.RewiredPlayer.AddInputEventDelegate(new Action<InputActionEventData>(this.OnCancelPressed), UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x060013EA RID: 5098 RVA: 0x0003C746 File Offset: 0x0003A946
	private void OnCancelPressed(InputActionEventData obj)
	{
		WindowManager.SetWindowIsOpen(this.ID, false);
	}

	// Token: 0x060013EB RID: 5099 RVA: 0x0003C754 File Offset: 0x0003A954
	protected override void OnLostFocus()
	{
		base.RewiredPlayer.RemoveInputEventDelegate(new Action<InputActionEventData>(this.OnCancelPressed), UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x060013EC RID: 5100 RVA: 0x0003C774 File Offset: 0x0003A974
	private void OnSkyToggled(bool isOn)
	{
		if (isOn)
		{
			for (int i = 0; i < this.m_activeSkies.Count; i++)
			{
				this.m_activeSkies[i].SetActive(true);
			}
			this.m_activeSkies.Clear();
			return;
		}
		Sky[] array = UnityEngine.Object.FindObjectsOfType<Sky>();
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j].gameObject.activeInHierarchy)
			{
				this.m_activeSkies.Add(array[j].gameObject);
				array[j].gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060013ED RID: 5101 RVA: 0x0003C7FC File Offset: 0x0003A9FC
	private void OnWeatherToggled(bool isOn)
	{
		if (isOn)
		{
			for (int i = 0; i < this.m_activeWeather.Count; i++)
			{
				this.m_activeWeather[i].SetActive(true);
			}
			this.m_activeWeather.Clear();
			return;
		}
		Weather[] array = UnityEngine.Object.FindObjectsOfType<Weather>();
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j].gameObject.activeInHierarchy)
			{
				this.m_activeWeather.Add(array[j].gameObject);
				array[j].gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060013EE RID: 5102 RVA: 0x0003C884 File Offset: 0x0003AA84
	private void OnBackgroundPerspToggled(bool isOn)
	{
		CameraController.SetCameraEnabled(0, isOn);
	}

	// Token: 0x060013EF RID: 5103 RVA: 0x0003C88D File Offset: 0x0003AA8D
	private void OnBackgroundOrthoToggled(bool isOn)
	{
		CameraController.SetCameraEnabled(1, isOn);
	}

	// Token: 0x060013F0 RID: 5104 RVA: 0x0003C896 File Offset: 0x0003AA96
	private void OnGameToggled(bool isOn)
	{
		CameraController.SetCameraEnabled(2, isOn);
	}

	// Token: 0x060013F1 RID: 5105 RVA: 0x0003C89F File Offset: 0x0003AA9F
	private void OnForegroundOrthoToggled(bool isOn)
	{
		CameraController.SetCameraEnabled(3, isOn);
	}

	// Token: 0x060013F2 RID: 5106 RVA: 0x0003C8A8 File Offset: 0x0003AAA8
	private void OnForegroundPerspToggled(bool isOn)
	{
		CameraController.SetCameraEnabled(4, isOn);
	}

	// Token: 0x060013F3 RID: 5107 RVA: 0x0003C8B1 File Offset: 0x0003AAB1
	private void OnCharacterToggled(bool isOn)
	{
		CameraController.SetCameraEnabled(5, isOn);
	}

	// Token: 0x060013F4 RID: 5108 RVA: 0x0003C8BA File Offset: 0x0003AABA
	private void OnUIToggled(bool isOn)
	{
		CameraController.SetCameraEnabled(6, isOn);
	}

	// Token: 0x060013F5 RID: 5109 RVA: 0x0003C8C3 File Offset: 0x0003AAC3
	private void OnTraitMaskToggled(bool isOn)
	{
		CameraController.SetCameraEnabled(7, isOn);
	}

	// Token: 0x060013F6 RID: 5110 RVA: 0x0003C8CC File Offset: 0x0003AACC
	private void OnLetterboxToggled(bool isOn)
	{
	}

	// Token: 0x060013F7 RID: 5111 RVA: 0x0003C8CE File Offset: 0x0003AACE
	private void OnMapToggled(bool isOn)
	{
		if (MapController.IsInitialized && MapController.Camera != null)
		{
			MapController.Camera.enabled = isOn;
		}
	}

	// Token: 0x060013F8 RID: 5112 RVA: 0x0003C8EF File Offset: 0x0003AAEF
	private void OnDropdownChanged(int index)
	{
		this.m_selectedLayerIndex = index;
	}

	// Token: 0x060013F9 RID: 5113 RVA: 0x0003C8F8 File Offset: 0x0003AAF8
	private void OnExecuteButtonPressed()
	{
		CameraLayer cameraLayer = CameraLayer.None;
		switch (this.m_selectedLayerIndex)
		{
		case 0:
			cameraLayer = CameraLayer.Background_Far_PERSP;
			break;
		case 1:
			cameraLayer = CameraLayer.Background_Near_PERSP;
			break;
		case 2:
			cameraLayer = CameraLayer.Background_ORTHO;
			break;
		case 3:
			cameraLayer = CameraLayer.Game;
			break;
		case 4:
			cameraLayer = CameraLayer.Foreground_ORTHO;
			break;
		case 5:
			cameraLayer = CameraLayer.Foreground_PERSP;
			break;
		}
		if (cameraLayer != CameraLayer.None)
		{
			PropSpawnController[] array = UnityEngine.Object.FindObjectsOfType<PropSpawnController>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].PropInstance != null)
				{
					Renderer[] componentsInChildren = array[i].PropInstance.GetComponentsInChildren<Renderer>();
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						if (componentsInChildren[j].gameObject.layer == CameraLayerUtility.GetLayer(cameraLayer))
						{
							componentsInChildren[j].enabled = this.m_arePropsEnabled;
						}
					}
				}
			}
			Ferr2DT_PathTerrain[] array2 = UnityEngine.Object.FindObjectsOfType<Ferr2DT_PathTerrain>();
			for (int k = 0; k < array2.Length; k++)
			{
				if (array2[k].DisableMerging || array2[k].name.Contains("Deco"))
				{
					if (cameraLayer == CameraLayer.Game)
					{
						if (!LayerMask.LayerToName(array2[k].gameObject.layer).Contains("Collides"))
						{
							goto IL_150;
						}
					}
					else if (array2[k].gameObject.layer != CameraLayerUtility.GetLayer(cameraLayer))
					{
						goto IL_150;
					}
					Renderer componentInChildren = array2[k].GetComponentInChildren<Renderer>();
					if (componentInChildren != null)
					{
						componentInChildren.enabled = this.m_isTerrainEnabled;
					}
				}
				IL_150:;
			}
		}
	}

	// Token: 0x060013FA RID: 5114 RVA: 0x0003CA65 File Offset: 0x0003AC65
	private void OnTerrainToggled(bool isOn)
	{
		this.m_isTerrainEnabled = isOn;
	}

	// Token: 0x060013FB RID: 5115 RVA: 0x0003CA6E File Offset: 0x0003AC6E
	private void OnPropsToggled(bool isOn)
	{
		this.m_arePropsEnabled = isOn;
	}

	// Token: 0x040013B4 RID: 5044
	[SerializeField]
	private Toggle m_backgroundPersp;

	// Token: 0x040013B5 RID: 5045
	[SerializeField]
	private Toggle m_backgroundOrtho;

	// Token: 0x040013B6 RID: 5046
	[SerializeField]
	private Toggle m_game;

	// Token: 0x040013B7 RID: 5047
	[SerializeField]
	private Toggle m_foregroundOrtho;

	// Token: 0x040013B8 RID: 5048
	[SerializeField]
	private Toggle m_foregroundPersp;

	// Token: 0x040013B9 RID: 5049
	[SerializeField]
	private Toggle m_character;

	// Token: 0x040013BA RID: 5050
	[SerializeField]
	private Toggle m_ui;

	// Token: 0x040013BB RID: 5051
	[SerializeField]
	private Toggle m_traitMask;

	// Token: 0x040013BC RID: 5052
	[SerializeField]
	private Toggle m_map;

	// Token: 0x040013BD RID: 5053
	[SerializeField]
	private Toggle m_letterbox;

	// Token: 0x040013BE RID: 5054
	[SerializeField]
	private Toggle m_weather;

	// Token: 0x040013BF RID: 5055
	[SerializeField]
	private Toggle m_sky;

	// Token: 0x040013C0 RID: 5056
	[SerializeField]
	private TMP_Dropdown m_layerDropdown;

	// Token: 0x040013C1 RID: 5057
	[SerializeField]
	private Toggle m_enableProps;

	// Token: 0x040013C2 RID: 5058
	[SerializeField]
	private Toggle m_enableTerrain;

	// Token: 0x040013C3 RID: 5059
	[SerializeField]
	private Button m_executeButton;

	// Token: 0x040013C4 RID: 5060
	private List<GameObject> m_activeSkies = new List<GameObject>();

	// Token: 0x040013C5 RID: 5061
	private List<GameObject> m_activeWeather = new List<GameObject>();

	// Token: 0x040013C6 RID: 5062
	private bool m_isTerrainEnabled = true;

	// Token: 0x040013C7 RID: 5063
	private bool m_arePropsEnabled = true;

	// Token: 0x040013C8 RID: 5064
	private int m_selectedLayerIndex;
}
