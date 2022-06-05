using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020006B2 RID: 1714
public class TextPopupManager : MonoBehaviour
{
	// Token: 0x06003F2B RID: 16171 RVA: 0x000E0FD0 File Offset: 0x000DF1D0
	private void Awake()
	{
		this.Initialize();
		SceneManager.sceneLoaded += this.OnSceneLoaded;
	}

	// Token: 0x06003F2C RID: 16172 RVA: 0x000E0FE9 File Offset: 0x000DF1E9
	private void OnDestroy()
	{
		TextPopupManager.m_isInitialized = false;
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	// Token: 0x06003F2D RID: 16173 RVA: 0x000E1002 File Offset: 0x000DF202
	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		if (TextPopupManager.IsInitialized)
		{
			TextPopupManager.DisableAllTextPopups();
		}
	}

	// Token: 0x06003F2E RID: 16174 RVA: 0x000E1010 File Offset: 0x000DF210
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

	// Token: 0x06003F2F RID: 16175 RVA: 0x000E1110 File Offset: 0x000DF310
	private TextPopupObj GetTextPopup(TextPopupType popupType)
	{
		GenericPool_RL<TextPopupObj> genericPool_RL = null;
		if (!this.m_textPopupDict.TryGetValue(popupType, out genericPool_RL))
		{
			throw new Exception("Text Popup Type: " + popupType.ToString() + " cannot be found in TextPopupManager.  Please make sure the prefab is added to the TextPopup Library prefab.");
		}
		return genericPool_RL.GetFreeObj();
	}

	// Token: 0x17001591 RID: 5521
	// (get) Token: 0x06003F30 RID: 16176 RVA: 0x000E1157 File Offset: 0x000DF357
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

	// Token: 0x17001592 RID: 5522
	// (get) Token: 0x06003F31 RID: 16177 RVA: 0x000E1176 File Offset: 0x000DF376
	public static bool IsInitialized
	{
		get
		{
			return TextPopupManager.m_isInitialized;
		}
	}

	// Token: 0x06003F32 RID: 16178 RVA: 0x000E1180 File Offset: 0x000DF380
	public static TextPopupObj DisplayTextDefaultPos(TextPopupType popupType, string text, BaseCharacterController charController, bool attachToSource, bool queuePopup = true)
	{
		Vector2 spawnPos = charController.Midpoint;
		spawnPos.y += charController.CollisionBounds.height / 2f;
		spawnPos.y += 0.5f;
		return TextPopupManager.DisplayText(popupType, text, charController.gameObject, spawnPos, attachToSource, queuePopup);
	}

	// Token: 0x06003F33 RID: 16179 RVA: 0x000E11DC File Offset: 0x000DF3DC
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

	// Token: 0x06003F34 RID: 16180 RVA: 0x000E12F8 File Offset: 0x000DF4F8
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

	// Token: 0x06003F35 RID: 16181 RVA: 0x000E1424 File Offset: 0x000DF624
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

	// Token: 0x06003F36 RID: 16182 RVA: 0x000E14CC File Offset: 0x000DF6CC
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

	// Token: 0x06003F37 RID: 16183 RVA: 0x000E15AC File Offset: 0x000DF7AC
	public static void DisableAllTextPopups()
	{
		foreach (KeyValuePair<TextPopupType, GenericPool_RL<TextPopupObj>> keyValuePair in TextPopupManager.Instance.m_textPopupDict)
		{
			keyValuePair.Value.DisableAll();
		}
	}

	// Token: 0x06003F38 RID: 16184 RVA: 0x000E160C File Offset: 0x000DF80C
	public static void DestroyPools()
	{
		foreach (KeyValuePair<TextPopupType, GenericPool_RL<TextPopupObj>> keyValuePair in TextPopupManager.Instance.m_textPopupDict)
		{
			keyValuePair.Value.DestroyPool();
		}
		TextPopupManager.Instance.m_textPopupDict.Clear();
	}

	// Token: 0x04002EE8 RID: 12008
	private const float TEXT_POPUP_DELAY_INTERVAL = 0.15f;

	// Token: 0x04002EE9 RID: 12009
	private const float TEXT_POPUP_Y_OFFSET = 0.9f;

	// Token: 0x04002EEA RID: 12010
	private const float TEXT_POPUP_Y_OFFSET_DURATION = 0.1f;

	// Token: 0x04002EEB RID: 12011
	private const string TEXTPOPUPMANAGER_NAME = "TextPopupManager";

	// Token: 0x04002EEC RID: 12012
	private const string RESOURCE_PATH = "Prefabs/Managers/TextPopupManager";

	// Token: 0x04002EED RID: 12013
	private Dictionary<TextPopupType, GenericPool_RL<TextPopupObj>> m_textPopupDict;

	// Token: 0x04002EEE RID: 12014
	private Dictionary<TextPopupType, float> m_popupLockoutDurationTable;

	// Token: 0x04002EEF RID: 12015
	private Dictionary<GameObject, Vector2> m_popupYOffsetDict = new Dictionary<GameObject, Vector2>();

	// Token: 0x04002EF0 RID: 12016
	private List<GameObject> m_offsetClearHelper = new List<GameObject>();

	// Token: 0x04002EF1 RID: 12017
	private static bool m_isInitialized;

	// Token: 0x04002EF2 RID: 12018
	private static TextPopupManager m_textPopupManager;
}
