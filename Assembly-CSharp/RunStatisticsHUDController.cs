using System;
using SceneManagement_RL;
using TMPro;
using UnityEngine;

// Token: 0x020002C4 RID: 708
public class RunStatisticsHUDController : MonoBehaviour
{
	// Token: 0x17000C93 RID: 3219
	// (get) Token: 0x06001C2A RID: 7210 RVA: 0x0005B338 File Offset: 0x00059538
	private bool IsRunning
	{
		get
		{
			return this.m_currentScene == SceneID.World;
		}
	}

	// Token: 0x06001C2B RID: 7211 RVA: 0x0005B343 File Offset: 0x00059543
	private void Awake()
	{
		this.m_onChestOpened = new Action<MonoBehaviour, EventArgs>(this.OnChestOpened);
		this.m_onSpecialItemDropped = new Action<MonoBehaviour, EventArgs>(this.OnSpecialItemDropped);
	}

	// Token: 0x06001C2C RID: 7212 RVA: 0x0005B36C File Offset: 0x0005956C
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

	// Token: 0x06001C2D RID: 7213 RVA: 0x0005B3D0 File Offset: 0x000595D0
	private void OnDestroy()
	{
		if (Application.isPlaying && this != null && this.m_isSubscribed)
		{
			this.UnsubscribeFromEvents();
			this.m_isSubscribed = false;
		}
	}

	// Token: 0x06001C2E RID: 7214 RVA: 0x0005B3F7 File Offset: 0x000595F7
	private void SubscribeToEvents()
	{
		this.m_isSubscribed = true;
		SceneLoader_RL.SceneLoadingEndRelay.AddListener(new Action<string>(this.OnSceneLoaded), false);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.ChestOpened, this.m_onChestOpened);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.SpecialItemsDropped, this.m_onSpecialItemDropped);
	}

	// Token: 0x06001C2F RID: 7215 RVA: 0x0005B432 File Offset: 0x00059632
	private void UnsubscribeFromEvents()
	{
		SceneLoader_RL.SceneLoadingEndRelay.RemoveListener(new Action<string>(this.OnSceneLoaded));
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.ChestOpened, this.m_onChestOpened);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.SpecialItemsDropped, this.m_onSpecialItemDropped);
	}

	// Token: 0x06001C30 RID: 7216 RVA: 0x0005B468 File Offset: 0x00059668
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

	// Token: 0x06001C31 RID: 7217 RVA: 0x0005B508 File Offset: 0x00059708
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

	// Token: 0x06001C32 RID: 7218 RVA: 0x0005B58E File Offset: 0x0005978E
	private void OnChestOpened(MonoBehaviour sender, EventArgs eventArgs)
	{
		this.m_chestCount++;
		this.m_chestCountText.text = string.Format("Chest Count = {0}", this.m_chestCount);
	}

	// Token: 0x06001C33 RID: 7219 RVA: 0x0005B5C0 File Offset: 0x000597C0
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

	// Token: 0x0400198E RID: 6542
	[SerializeField]
	private GameObject m_panel;

	// Token: 0x0400198F RID: 6543
	[SerializeField]
	private TextMeshProUGUI m_durationText;

	// Token: 0x04001990 RID: 6544
	[SerializeField]
	private TextMeshProUGUI m_expCountText;

	// Token: 0x04001991 RID: 6545
	[SerializeField]
	private TextMeshProUGUI m_chestCountText;

	// Token: 0x04001992 RID: 6546
	[SerializeField]
	private TextMeshProUGUI m_blueprintCountText;

	// Token: 0x04001993 RID: 6547
	[SerializeField]
	private TextMeshProUGUI m_runeCountText;

	// Token: 0x04001994 RID: 6548
	[SerializeField]
	private bool m_isEnabledInEditor = true;

	// Token: 0x04001995 RID: 6549
	private bool m_isSubscribed;

	// Token: 0x04001996 RID: 6550
	private bool m_isPaused;

	// Token: 0x04001997 RID: 6551
	private float m_elapsedTime;

	// Token: 0x04001998 RID: 6552
	private int m_chestCount;

	// Token: 0x04001999 RID: 6553
	private int m_blueprintCount;

	// Token: 0x0400199A RID: 6554
	private int m_runeCount;

	// Token: 0x0400199B RID: 6555
	private SceneID m_currentScene;

	// Token: 0x0400199C RID: 6556
	private Action<MonoBehaviour, EventArgs> m_onChestOpened;

	// Token: 0x0400199D RID: 6557
	private Action<MonoBehaviour, EventArgs> m_onSpecialItemDropped;
}
