using System;
using System.Collections.Generic;
using Rewired;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000376 RID: 886
public class CameraDebugController : WindowController
{
	// Token: 0x17000D62 RID: 3426
	// (get) Token: 0x06001CF0 RID: 7408 RVA: 0x0000EE72 File Offset: 0x0000D072
	public override WindowID ID
	{
		get
		{
			return WindowID.CameraDebug;
		}
	}

	// Token: 0x06001CF1 RID: 7409 RVA: 0x0009B488 File Offset: 0x00099688
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

	// Token: 0x06001CF2 RID: 7410 RVA: 0x0000EE76 File Offset: 0x0000D076
	protected override void OnOpen()
	{
		this.m_windowCanvas.gameObject.SetActive(true);
		this.m_backgroundPersp.Select();
	}

	// Token: 0x06001CF3 RID: 7411 RVA: 0x0000EE94 File Offset: 0x0000D094
	protected override void OnClose()
	{
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x06001CF4 RID: 7412 RVA: 0x0000EEA7 File Offset: 0x0000D0A7
	protected override void OnFocus()
	{
		base.RewiredPlayer.AddInputEventDelegate(new Action<InputActionEventData>(this.OnCancelPressed), UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x06001CF5 RID: 7413 RVA: 0x0000EEC7 File Offset: 0x0000D0C7
	private void OnCancelPressed(InputActionEventData obj)
	{
		WindowManager.SetWindowIsOpen(this.ID, false);
	}

	// Token: 0x06001CF6 RID: 7414 RVA: 0x0000EED5 File Offset: 0x0000D0D5
	protected override void OnLostFocus()
	{
		base.RewiredPlayer.RemoveInputEventDelegate(new Action<InputActionEventData>(this.OnCancelPressed), UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x06001CF7 RID: 7415 RVA: 0x0009B658 File Offset: 0x00099858
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

	// Token: 0x06001CF8 RID: 7416 RVA: 0x0009B6E0 File Offset: 0x000998E0
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

	// Token: 0x06001CF9 RID: 7417 RVA: 0x0000EEF5 File Offset: 0x0000D0F5
	private void OnBackgroundPerspToggled(bool isOn)
	{
		CameraController.SetCameraEnabled(0, isOn);
	}

	// Token: 0x06001CFA RID: 7418 RVA: 0x0000EEFE File Offset: 0x0000D0FE
	private void OnBackgroundOrthoToggled(bool isOn)
	{
		CameraController.SetCameraEnabled(1, isOn);
	}

	// Token: 0x06001CFB RID: 7419 RVA: 0x0000EF07 File Offset: 0x0000D107
	private void OnGameToggled(bool isOn)
	{
		CameraController.SetCameraEnabled(2, isOn);
	}

	// Token: 0x06001CFC RID: 7420 RVA: 0x0000EF10 File Offset: 0x0000D110
	private void OnForegroundOrthoToggled(bool isOn)
	{
		CameraController.SetCameraEnabled(3, isOn);
	}

	// Token: 0x06001CFD RID: 7421 RVA: 0x0000EF19 File Offset: 0x0000D119
	private void OnForegroundPerspToggled(bool isOn)
	{
		CameraController.SetCameraEnabled(4, isOn);
	}

	// Token: 0x06001CFE RID: 7422 RVA: 0x0000EF22 File Offset: 0x0000D122
	private void OnCharacterToggled(bool isOn)
	{
		CameraController.SetCameraEnabled(5, isOn);
	}

	// Token: 0x06001CFF RID: 7423 RVA: 0x0000EF2B File Offset: 0x0000D12B
	private void OnUIToggled(bool isOn)
	{
		CameraController.SetCameraEnabled(6, isOn);
	}

	// Token: 0x06001D00 RID: 7424 RVA: 0x0000EF34 File Offset: 0x0000D134
	private void OnTraitMaskToggled(bool isOn)
	{
		CameraController.SetCameraEnabled(7, isOn);
	}

	// Token: 0x06001D01 RID: 7425 RVA: 0x00002FCA File Offset: 0x000011CA
	private void OnLetterboxToggled(bool isOn)
	{
	}

	// Token: 0x06001D02 RID: 7426 RVA: 0x0000EF3D File Offset: 0x0000D13D
	private void OnMapToggled(bool isOn)
	{
		if (MapController.IsInitialized && MapController.Camera != null)
		{
			MapController.Camera.enabled = isOn;
		}
	}

	// Token: 0x06001D03 RID: 7427 RVA: 0x0000EF5E File Offset: 0x0000D15E
	private void OnDropdownChanged(int index)
	{
		this.m_selectedLayerIndex = index;
	}

	// Token: 0x06001D04 RID: 7428 RVA: 0x0009B768 File Offset: 0x00099968
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

	// Token: 0x06001D05 RID: 7429 RVA: 0x0000EF67 File Offset: 0x0000D167
	private void OnTerrainToggled(bool isOn)
	{
		this.m_isTerrainEnabled = isOn;
	}

	// Token: 0x06001D06 RID: 7430 RVA: 0x0000EF70 File Offset: 0x0000D170
	private void OnPropsToggled(bool isOn)
	{
		this.m_arePropsEnabled = isOn;
	}

	// Token: 0x04001A4E RID: 6734
	[SerializeField]
	private Toggle m_backgroundPersp;

	// Token: 0x04001A4F RID: 6735
	[SerializeField]
	private Toggle m_backgroundOrtho;

	// Token: 0x04001A50 RID: 6736
	[SerializeField]
	private Toggle m_game;

	// Token: 0x04001A51 RID: 6737
	[SerializeField]
	private Toggle m_foregroundOrtho;

	// Token: 0x04001A52 RID: 6738
	[SerializeField]
	private Toggle m_foregroundPersp;

	// Token: 0x04001A53 RID: 6739
	[SerializeField]
	private Toggle m_character;

	// Token: 0x04001A54 RID: 6740
	[SerializeField]
	private Toggle m_ui;

	// Token: 0x04001A55 RID: 6741
	[SerializeField]
	private Toggle m_traitMask;

	// Token: 0x04001A56 RID: 6742
	[SerializeField]
	private Toggle m_map;

	// Token: 0x04001A57 RID: 6743
	[SerializeField]
	private Toggle m_letterbox;

	// Token: 0x04001A58 RID: 6744
	[SerializeField]
	private Toggle m_weather;

	// Token: 0x04001A59 RID: 6745
	[SerializeField]
	private Toggle m_sky;

	// Token: 0x04001A5A RID: 6746
	[SerializeField]
	private TMP_Dropdown m_layerDropdown;

	// Token: 0x04001A5B RID: 6747
	[SerializeField]
	private Toggle m_enableProps;

	// Token: 0x04001A5C RID: 6748
	[SerializeField]
	private Toggle m_enableTerrain;

	// Token: 0x04001A5D RID: 6749
	[SerializeField]
	private Button m_executeButton;

	// Token: 0x04001A5E RID: 6750
	private List<GameObject> m_activeSkies = new List<GameObject>();

	// Token: 0x04001A5F RID: 6751
	private List<GameObject> m_activeWeather = new List<GameObject>();

	// Token: 0x04001A60 RID: 6752
	private bool m_isTerrainEnabled = true;

	// Token: 0x04001A61 RID: 6753
	private bool m_arePropsEnabled = true;

	// Token: 0x04001A62 RID: 6754
	private int m_selectedLayerIndex;
}
