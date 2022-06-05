using System;
using TMPro;
using UnityEngine;

// Token: 0x020007F9 RID: 2041
public class FPSCounter_RL : MonoBehaviour
{
	// Token: 0x170016EC RID: 5868
	// (get) Token: 0x060043CA RID: 17354 RVA: 0x000ED9AF File Offset: 0x000EBBAF
	private bool IsEnabled
	{
		get
		{
			return this.m_panel.activeInHierarchy;
		}
	}

	// Token: 0x060043CB RID: 17355 RVA: 0x000ED9BC File Offset: 0x000EBBBC
	private void Awake()
	{
		this.m_onToggleFPSCounter = new Action<MonoBehaviour, EventArgs>(this.OnToggleFPSCounter);
	}

	// Token: 0x060043CC RID: 17356 RVA: 0x000ED9D0 File Offset: 0x000EBBD0
	protected virtual void Start()
	{
		this.m_panel.SetActive(false);
		this._timeLeft = this.m_updateInterval;
		Messenger<DebugMessenger, DebugEvent>.AddListener(DebugEvent.ToggleFPSCounter, this.m_onToggleFPSCounter);
	}

	// Token: 0x060043CD RID: 17357 RVA: 0x000ED9F7 File Offset: 0x000EBBF7
	private void OnDestroy()
	{
		Messenger<DebugMessenger, DebugEvent>.RemoveListener(DebugEvent.ToggleFPSCounter, this.m_onToggleFPSCounter);
	}

	// Token: 0x060043CE RID: 17358 RVA: 0x000EDA08 File Offset: 0x000EBC08
	private void OnToggleFPSCounter(MonoBehaviour arg1, EventArgs arg2)
	{
		this.m_panel.SetActive(!this.m_panel.activeInHierarchy);
		if (this.m_panel.activeInHierarchy)
		{
			this.m_storedVSyncCount = QualitySettings.vSyncCount;
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = -1;
			Debug.Log("<color=yellow>Setting target FPS to -1 and vsync to 0 for accurate FPS monitoring.</color>");
			return;
		}
		QualitySettings.vSyncCount = this.m_storedVSyncCount;
		Application.targetFrameRate = 120;
		Debug.Log(string.Concat(new string[]
		{
			"<color=yellow>Setting target FPS to ",
			120.ToString(),
			" and vsync to ",
			this.m_storedVSyncCount.ToString(),
			".  FPS monitoring disabled.</color>"
		}));
	}

	// Token: 0x060043CF RID: 17359 RVA: 0x000EDAB4 File Offset: 0x000EBCB4
	protected virtual void Update()
	{
		if (!this.IsEnabled)
		{
			return;
		}
		this._framesDrawnInTheInterval += 1f;
		this._framesAccumulated += Time.timeScale / Time.deltaTime;
		this._timeLeft -= Time.deltaTime;
		if ((double)this._timeLeft <= 0.0)
		{
			this._currentFPS = (int)Mathf.Clamp(this._framesAccumulated / this._framesDrawnInTheInterval, 0f, 300f);
			if (this._currentFPS >= 0 && this._currentFPS <= 300)
			{
				this._text.text = FPSCounter_RL._stringsFrom00To300[this._currentFPS];
			}
			this._framesDrawnInTheInterval = 0f;
			this._framesAccumulated = 0f;
			this._timeLeft = this.m_updateInterval;
		}
	}

	// Token: 0x040039F5 RID: 14837
	[SerializeField]
	private GameObject m_panel;

	// Token: 0x040039F6 RID: 14838
	[SerializeField]
	private TMP_Text _text;

	// Token: 0x040039F7 RID: 14839
	[SerializeField]
	private float m_updateInterval = 0.3f;

	// Token: 0x040039F8 RID: 14840
	private int m_storedVSyncCount;

	// Token: 0x040039F9 RID: 14841
	protected float _framesAccumulated;

	// Token: 0x040039FA RID: 14842
	protected float _framesDrawnInTheInterval;

	// Token: 0x040039FB RID: 14843
	protected float _timeLeft;

	// Token: 0x040039FC RID: 14844
	protected int _currentFPS;

	// Token: 0x040039FD RID: 14845
	private Action<MonoBehaviour, EventArgs> m_onToggleFPSCounter;

	// Token: 0x040039FE RID: 14846
	private static string[] _stringsFrom00To300 = new string[]
	{
		"00",
		"01",
		"02",
		"03",
		"04",
		"05",
		"06",
		"07",
		"08",
		"09",
		"10",
		"11",
		"12",
		"13",
		"14",
		"15",
		"16",
		"17",
		"18",
		"19",
		"20",
		"21",
		"22",
		"23",
		"24",
		"25",
		"26",
		"27",
		"28",
		"29",
		"30",
		"31",
		"32",
		"33",
		"34",
		"35",
		"36",
		"37",
		"38",
		"39",
		"40",
		"41",
		"42",
		"43",
		"44",
		"45",
		"46",
		"47",
		"48",
		"49",
		"50",
		"51",
		"52",
		"53",
		"54",
		"55",
		"56",
		"57",
		"58",
		"59",
		"60",
		"61",
		"62",
		"63",
		"64",
		"65",
		"66",
		"67",
		"68",
		"69",
		"70",
		"71",
		"72",
		"73",
		"74",
		"75",
		"76",
		"77",
		"78",
		"79",
		"80",
		"81",
		"82",
		"83",
		"84",
		"85",
		"86",
		"87",
		"88",
		"89",
		"90",
		"91",
		"92",
		"93",
		"94",
		"95",
		"96",
		"97",
		"98",
		"99",
		"100",
		"101",
		"102",
		"103",
		"104",
		"105",
		"106",
		"107",
		"108",
		"109",
		"110",
		"111",
		"112",
		"113",
		"114",
		"115",
		"116",
		"117",
		"118",
		"119",
		"120",
		"121",
		"122",
		"123",
		"124",
		"125",
		"126",
		"127",
		"128",
		"129",
		"130",
		"131",
		"132",
		"133",
		"134",
		"135",
		"136",
		"137",
		"138",
		"139",
		"140",
		"141",
		"142",
		"143",
		"144",
		"145",
		"146",
		"147",
		"148",
		"149",
		"150",
		"151",
		"152",
		"153",
		"154",
		"155",
		"156",
		"157",
		"158",
		"159",
		"160",
		"161",
		"162",
		"163",
		"164",
		"165",
		"166",
		"167",
		"168",
		"169",
		"170",
		"171",
		"172",
		"173",
		"174",
		"175",
		"176",
		"177",
		"178",
		"179",
		"180",
		"181",
		"182",
		"183",
		"184",
		"185",
		"186",
		"187",
		"188",
		"189",
		"190",
		"191",
		"192",
		"193",
		"194",
		"195",
		"196",
		"197",
		"198",
		"199",
		"200",
		"201",
		"202",
		"203",
		"204",
		"205",
		"206",
		"207",
		"208",
		"209",
		"210",
		"211",
		"212",
		"213",
		"214",
		"215",
		"216",
		"217",
		"218",
		"219",
		"220",
		"221",
		"222",
		"223",
		"224",
		"225",
		"226",
		"227",
		"228",
		"229",
		"230",
		"231",
		"232",
		"233",
		"234",
		"235",
		"236",
		"237",
		"238",
		"239",
		"240",
		"241",
		"242",
		"243",
		"244",
		"245",
		"246",
		"247",
		"248",
		"249",
		"250",
		"251",
		"252",
		"253",
		"254",
		"255",
		"256",
		"257",
		"258",
		"259",
		"260",
		"261",
		"262",
		"263",
		"264",
		"265",
		"266",
		"267",
		"268",
		"269",
		"270",
		"271",
		"272",
		"273",
		"274",
		"275",
		"276",
		"277",
		"278",
		"279",
		"280",
		"281",
		"282",
		"283",
		"284",
		"285",
		"286",
		"287",
		"288",
		"289",
		"290",
		"291",
		"292",
		"293",
		"294",
		"295",
		"296",
		"297",
		"298",
		"299",
		"300"
	};
}
