using System;
using SceneManagement_RL;
using TMPro;
using UnityEngine;

// Token: 0x020004B9 RID: 1209
public class RunStatisticsHUDController : MonoBehaviour
{
	// Token: 0x1700101C RID: 4124
	// (get) Token: 0x060026F4 RID: 9972 RVA: 0x00015D90 File Offset: 0x00013F90
	private bool IsRunning
	{
		get
		{
			return this.m_currentScene == SceneID.World;
		}
	}

	// Token: 0x060026F5 RID: 9973 RVA: 0x00015D9B File Offset: 0x00013F9B
	private void Awake()
	{
		this.m_onChestOpened = new Action<MonoBehaviour, EventArgs>(this.OnChestOpened);
		this.m_onSpecialItemDropped = new Action<MonoBehaviour, EventArgs>(this.OnSpecialItemDropped);
	}

	// Token: 0x060026F6 RID: 9974 RVA: 0x000B7C54 File Offset: 0x000B5E54
	private void Start()
	{
		if (Application.isEditor)
		{
			this.m_currentScene = SceneLoadingUtility.GetSceneID(SceneLoadingUtility.ActiveScene.name);
			if (this.m_isEnabledInEditor)
			{
				this.SubscribeToEvents();
			}
			if (!this.m_isEnabledInEditor || this.m_currentScene != SceneID.World)
			{
				this.m_panel.SetActive(false);
				return;
			}
		}
		else
		{
			this.m_panel.SetActive(false);
		}
	}

	// Token: 0x060026F7 RID: 9975 RVA: 0x00015DC1 File Offset: 0x00013FC1
	private void OnDestroy()
	{
		if (Application.isPlaying && this != null && this.m_isSubscribed)
		{
			this.UnsubscribeFromEvents();
			this.m_isSubscribed = false;
		}
	}

	// Token: 0x060026F8 RID: 9976 RVA: 0x00015DE8 File Offset: 0x00013FE8
	private void SubscribeToEvents()
	{
		this.m_isSubscribed = true;
		SceneLoader_RL.SceneLoadingEndRelay.AddListener(new Action<string>(this.OnSceneLoaded), false);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.ChestOpened, this.m_onChestOpened);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.SpecialItemsDropped, this.m_onSpecialItemDropped);
	}

	// Token: 0x060026F9 RID: 9977 RVA: 0x00015E23 File Offset: 0x00014023
	private void UnsubscribeFromEvents()
	{
		SceneLoader_RL.SceneLoadingEndRelay.RemoveListener(new Action<string>(this.OnSceneLoaded));
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.ChestOpened, this.m_onChestOpened);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.SpecialItemsDropped, this.m_onSpecialItemDropped);
	}

	// Token: 0x060026FA RID: 9978 RVA: 0x000B7CB8 File Offset: 0x000B5EB8
	private void OnSceneLoaded(string sceneName)
	{
		this.m_currentScene = SceneLoadingUtility.GetSceneID(sceneName);
		this.m_elapsedTime = 0f;
		this.m_chestCount = 0;
		this.m_blueprintCount = 0;
		this.m_runeCount = 0;
		this.m_durationText.text = "Duration = 0";
		this.m_chestCountText.text = "Chest Count = 0";
		this.m_blueprintCountText.text = "Blueprint Count = 0";
		this.m_runeCountText.text = "Rune Count = 0";
		this.m_expCountText.text = "EXP = 0";
		this.m_panel.SetActive(this.m_currentScene == SceneID.World);
	}

	// Token: 0x060026FB RID: 9979 RVA: 0x000B7D58 File Offset: 0x000B5F58
	private void OnSpecialItemDropped(MonoBehaviour sender, EventArgs eventArgs)
	{
		SpecialItemType specialItemType = (eventArgs as SpecialItemDroppedEventArgs).SpecialItemDrop.SpecialItemType;
		if (specialItemType == SpecialItemType.Blueprint)
		{
			this.m_blueprintCount++;
			this.m_blueprintCountText.text = string.Format("Blueprint Count = {0}", this.m_chestCount);
			return;
		}
		if (specialItemType != SpecialItemType.Rune)
		{
			return;
		}
		this.m_runeCount++;
		this.m_runeCountText.text = string.Format("Rune Count = {0}", this.m_chestCount);
	}

	// Token: 0x060026FC RID: 9980 RVA: 0x00015E56 File Offset: 0x00014056
	private void OnChestOpened(MonoBehaviour sender, EventArgs eventArgs)
	{
		this.m_chestCount++;
		this.m_chestCountText.text = string.Format("Chest Count = {0}", this.m_chestCount);
	}

	// Token: 0x060026FD RID: 9981 RVA: 0x000B7DE0 File Offset: 0x000B5FE0
	private void Update()
	{
		if (!this.IsRunning)
		{
			return;
		}
		this.m_isPaused = (Time.timeScale == 0f);
		if (!this.m_isPaused)
		{
			this.m_elapsedTime += Time.deltaTime;
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)this.m_elapsedTime);
			this.m_durationText.text = string.Format("Duration = {0}", timeSpan.ToString("mm\\:ss"));
			if (SaveManager.IsInitialized)
			{
				int runAccumulatedXP = SaveManager.PlayerSaveData.RunAccumulatedXP;
				this.m_expCountText.text = string.Format("EXP = {0}", runAccumulatedXP);
			}
		}
	}

	// Token: 0x04002195 RID: 8597
	[SerializeField]
	private GameObject m_panel;

	// Token: 0x04002196 RID: 8598
	[SerializeField]
	private TextMeshProUGUI m_durationText;

	// Token: 0x04002197 RID: 8599
	[SerializeField]
	private TextMeshProUGUI m_expCountText;

	// Token: 0x04002198 RID: 8600
	[SerializeField]
	private TextMeshProUGUI m_chestCountText;

	// Token: 0x04002199 RID: 8601
	[SerializeField]
	private TextMeshProUGUI m_blueprintCountText;

	// Token: 0x0400219A RID: 8602
	[SerializeField]
	private TextMeshProUGUI m_runeCountText;

	// Token: 0x0400219B RID: 8603
	[SerializeField]
	private bool m_isEnabledInEditor = true;

	// Token: 0x0400219C RID: 8604
	private bool m_isSubscribed;

	// Token: 0x0400219D RID: 8605
	private bool m_isPaused;

	// Token: 0x0400219E RID: 8606
	private float m_elapsedTime;

	// Token: 0x0400219F RID: 8607
	private int m_chestCount;

	// Token: 0x040021A0 RID: 8608
	private int m_blueprintCount;

	// Token: 0x040021A1 RID: 8609
	private int m_runeCount;

	// Token: 0x040021A2 RID: 8610
	private SceneID m_currentScene;

	// Token: 0x040021A3 RID: 8611
	private Action<MonoBehaviour, EventArgs> m_onChestOpened;

	// Token: 0x040021A4 RID: 8612
	private Action<MonoBehaviour, EventArgs> m_onSpecialItemDropped;
}
