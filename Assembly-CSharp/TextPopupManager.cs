using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000B5A RID: 2906
public class TextPopupManager : MonoBehaviour
{
	// Token: 0x0600585F RID: 22623 RVA: 0x00030074 File Offset: 0x0002E274
	private void Awake()
	{
		this.Initialize();
		SceneManager.sceneLoaded += this.OnSceneLoaded;
	}

	// Token: 0x06005860 RID: 22624 RVA: 0x0003008D File Offset: 0x0002E28D
	private void OnDestroy()
	{
		TextPopupManager.m_isInitialized = false;
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	// Token: 0x06005861 RID: 22625 RVA: 0x000300A6 File Offset: 0x0002E2A6
	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		if (TextPopupManager.IsInitialized)
		{
			TextPopupManager.DisableAllTextPopups();
		}
	}

	// Token: 0x06005862 RID: 22626 RVA: 0x0015137C File Offset: 0x0014F57C
	private void Initialize()
	{
		this.m_popupLockoutDurationTable = new Dictionary<TextPopupType, float>();
		this.m_textPopupDict = new Dictionary<TextPopupType, GenericPool_RL<TextPopupObj>>();
		foreach (KeyValuePair<TextPopupType, TextPopupLibrary.TextPopupEntry> keyValuePair in TextPopupLibrary.TextPopupDict)
		{
			TextPopupObj textPopupPrefab = keyValuePair.Value.TextPopupPrefab;
			if (this.m_textPopupDict.ContainsKey(keyValuePair.Key))
			{
				throw new Exception("Text Popup Type: " + keyValuePair.Key.ToString() + " already found in TextPopup Library.  Duplicates not allowed.");
			}
			GenericPool_RL<TextPopupObj> genericPool_RL = new GenericPool_RL<TextPopupObj>();
			genericPool_RL.Initialize(textPopupPrefab, keyValuePair.Value.PoolSize, false, true);
			this.m_textPopupDict.Add(keyValuePair.Key, genericPool_RL);
			this.m_popupLockoutDurationTable.Add(keyValuePair.Key, 0f);
		}
		TextPopupManager.m_isInitialized = true;
	}

	// Token: 0x06005863 RID: 22627 RVA: 0x0015147C File Offset: 0x0014F67C
	private TextPopupObj GetTextPopup(TextPopupType popupType)
	{
		GenericPool_RL<TextPopupObj> genericPool_RL = null;
		if (!this.m_textPopupDict.TryGetValue(popupType, out genericPool_RL))
		{
			throw new Exception("Text Popup Type: " + popupType.ToString() + " cannot be found in TextPopupManager.  Please make sure the prefab is added to the TextPopup Library prefab.");
		}
		return genericPool_RL.GetFreeObj();
	}

	// Token: 0x17001D89 RID: 7561
	// (get) Token: 0x06005864 RID: 22628 RVA: 0x000300B4 File Offset: 0x0002E2B4
	private static TextPopupManager Instance
	{
		get
		{
			if (TextPopupManager.m_textPopupManager == null)
			{
				TextPopupManager.m_textPopupManager = CDGHelper.FindStaticInstance<TextPopupManager>(false);
			}
			return TextPopupManager.m_textPopupManager;
		}
	}

	// Token: 0x17001D8A RID: 7562
	// (get) Token: 0x06005865 RID: 22629 RVA: 0x000300D3 File Offset: 0x0002E2D3
	public static bool IsInitialized
	{
		get
		{
			return TextPopupManager.m_isInitialized;
		}
	}

	// Token: 0x06005866 RID: 22630 RVA: 0x001514C4 File Offset: 0x0014F6C4
	public static TextPopupObj DisplayTextDefaultPos(TextPopupType popupType, string text, BaseCharacterController charController, bool attachToSource, bool queuePopup = true)
	{
		Vector2 spawnPos = charController.Midpoint;
		spawnPos.y += charController.CollisionBounds.height / 2f;
		spawnPos.y += 0.5f;
		return TextPopupManager.DisplayText(popupType, text, charController.gameObject, spawnPos, attachToSource, queuePopup);
	}

	// Token: 0x06005867 RID: 22631 RVA: 0x00151520 File Offset: 0x0014F720
	public static TextPopupObj DisplayText(TextPopupType popupType, string text, GameObject source, Vector2 spawnPos, bool attachToSource, bool queuePopup = true)
	{
		spawnPos.y += 0.5f;
		TextPopupObj textPopup = TextPopupManager.Instance.GetTextPopup(popupType);
		textPopup.gameObject.SetActive(true);
		textPopup.Source = source;
		if (!textPopup.Text.Equals(text))
		{
			textPopup.SetText(text, TextAlignmentOptions.Center);
		}
		if (queuePopup)
		{
			if (!TextPopupManager.Instance.m_popupYOffsetDict.ContainsKey(source))
			{
				TextPopupManager.Instance.m_popupYOffsetDict.Add(source, new Vector2(Time.time, 0.9f));
			}
			else
			{
				Vector2 vector = TextPopupManager.Instance.m_popupYOffsetDict[source];
				spawnPos.y += vector.y;
				vector.y += 0.9f;
				TextPopupManager.Instance.m_popupYOffsetDict[source] = vector;
			}
		}
		textPopup.transform.localPosition = new Vector3(spawnPos.x, spawnPos.y, textPopup.transform.localPosition.z);
		if (attachToSource)
		{
			textPopup.transform.SetParent(source.transform, true);
		}
		textPopup.Spawn();
		return textPopup;
	}

	// Token: 0x06005868 RID: 22632 RVA: 0x0015163C File Offset: 0x0014F83C
	public static TextPopupObj DisplayTextAtAbsPos(TextPopupType popupType, string text, Vector2 absPos, GameObject source = null, TextAlignmentOptions alignmentOptions = TextAlignmentOptions.Center)
	{
		absPos.y += 0.5f;
		TextPopupObj textPopup = TextPopupManager.Instance.GetTextPopup(popupType);
		textPopup.gameObject.SetActive(true);
		if (!textPopup.Text.Equals(text))
		{
			textPopup.SetText(text, alignmentOptions);
		}
		textPopup.transform.localPosition = new Vector3(0f, 0f, textPopup.transform.localPosition.z);
		if (source)
		{
			textPopup.transform.SetParent(source.transform, false);
			textPopup.gameObject.transform.localPosition = new Vector3(absPos.x, absPos.y, textPopup.transform.localPosition.z);
			float num = 1f / source.transform.lossyScale.x;
			textPopup.gameObject.transform.localScale = new Vector3(num, num, num);
		}
		else
		{
			textPopup.gameObject.transform.position = new Vector3(absPos.x, absPos.y, textPopup.transform.position.z);
		}
		textPopup.Spawn();
		return textPopup;
	}

	// Token: 0x06005869 RID: 22633 RVA: 0x00151768 File Offset: 0x0014F968
	public static TextPopupObj DisplayLocIDText(TextPopupType popupType, string locID, StringGenderType genderType, Vector2 position, float lockoutDuration = 0f)
	{
		position.y += 0.5f;
		if (TextPopupManager.Instance.m_popupLockoutDurationTable[popupType] <= Time.time)
		{
			TextPopupObj textPopup = TextPopupManager.Instance.GetTextPopup(popupType);
			textPopup.gameObject.SetActive(true);
			textPopup.SetLocIDText(locID, genderType);
			textPopup.gameObject.transform.position = new Vector3(position.x, position.y, textPopup.transform.position.z);
			textPopup.Spawn();
			TextPopupManager.Instance.m_popupLockoutDurationTable[popupType] = Time.time + lockoutDuration;
			return textPopup;
		}
		return null;
	}

	// Token: 0x0600586A RID: 22634 RVA: 0x00151810 File Offset: 0x0014FA10
	private void LateUpdate()
	{
		if (this.m_popupYOffsetDict.Count > 0)
		{
			this.m_offsetClearHelper.Clear();
			foreach (KeyValuePair<GameObject, Vector2> keyValuePair in this.m_popupYOffsetDict)
			{
				Vector2 value = keyValuePair.Value;
				if (Time.time > value.x + 0.1f)
				{
					this.m_offsetClearHelper.Add(keyValuePair.Key);
				}
			}
			foreach (GameObject key in this.m_offsetClearHelper)
			{
				this.m_popupYOffsetDict.Remove(key);
			}
		}
	}

	// Token: 0x0600586B RID: 22635 RVA: 0x001518F0 File Offset: 0x0014FAF0
	public static void DisableAllTextPopups()
	{
		foreach (KeyValuePair<TextPopupType, GenericPool_RL<TextPopupObj>> keyValuePair in TextPopupManager.Instance.m_textPopupDict)
		{
			keyValuePair.Value.DisableAll();
		}
	}

	// Token: 0x0600586C RID: 22636 RVA: 0x00151950 File Offset: 0x0014FB50
	public static void DestroyPools()
	{
		foreach (KeyValuePair<TextPopupType, GenericPool_RL<TextPopupObj>> keyValuePair in TextPopupManager.Instance.m_textPopupDict)
		{
			keyValuePair.Value.DestroyPool();
		}
		TextPopupManager.Instance.m_textPopupDict.Clear();
	}

	// Token: 0x04004131 RID: 16689
	private const float TEXT_POPUP_DELAY_INTERVAL = 0.15f;

	// Token: 0x04004132 RID: 16690
	private const float TEXT_POPUP_Y_OFFSET = 0.9f;

	// Token: 0x04004133 RID: 16691
	private const float TEXT_POPUP_Y_OFFSET_DURATION = 0.1f;

	// Token: 0x04004134 RID: 16692
	private const string TEXTPOPUPMANAGER_NAME = "TextPopupManager";

	// Token: 0x04004135 RID: 16693
	private const string RESOURCE_PATH = "Prefabs/Managers/TextPopupManager";

	// Token: 0x04004136 RID: 16694
	private Dictionary<TextPopupType, GenericPool_RL<TextPopupObj>> m_textPopupDict;

	// Token: 0x04004137 RID: 16695
	private Dictionary<TextPopupType, float> m_popupLockoutDurationTable;

	// Token: 0x04004138 RID: 16696
	private Dictionary<GameObject, Vector2> m_popupYOffsetDict = new Dictionary<GameObject, Vector2>();

	// Token: 0x04004139 RID: 16697
	private List<GameObject> m_offsetClearHelper = new List<GameObject>();

	// Token: 0x0400413A RID: 16698
	private static bool m_isInitialized;

	// Token: 0x0400413B RID: 16699
	private static TextPopupManager m_textPopupManager;
}
