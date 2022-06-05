using System;
using TMPro;
using UnityEngine;

// Token: 0x02000CBF RID: 3263
public class FPSCounter_RL : MonoBehaviour
{
	// Token: 0x17001EEA RID: 7914
	// (get) Token: 0x06005D53 RID: 23891 RVA: 0x000335B9 File Offset: 0x000317B9
	private bool IsEnabled
	{
		get
		{
			return this.m_panel.activeInHierarchy;
		}
	}

	// Token: 0x06005D54 RID: 23892 RVA: 0x000335C6 File Offset: 0x000317C6
	private void Awake()
	{
		this.m_onToggleFPSCounter = new Action<MonoBehaviour, EventArgs>(this.OnToggleFPSCounter);
	}

	// Token: 0x06005D55 RID: 23893 RVA: 0x000335DA File Offset: 0x000317DA
	protected virtual void Start()
	{
		this.m_panel.SetActive(false);
		this._timeLeft = this.m_updateInterval;
		Messenger<DebugMessenger, DebugEvent>.AddListener(DebugEvent.ToggleFPSCounter, this.m_onToggleFPSCounter);
	}

	// Token: 0x06005D56 RID: 23894 RVA: 0x00033601 File Offset: 0x00031801
	private void OnDestroy()
	{
		Messenger<DebugMessenger, DebugEvent>.RemoveListener(DebugEvent.ToggleFPSCounter, this.m_onToggleFPSCounter);
	}

	// Token: 0x06005D57 RID: 23895 RVA: 0x0015B82C File Offset: 0x00159A2C
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

	// Token: 0x06005D58 RID: 23896 RVA: 0x0015B8D8 File Offset: 0x00159AD8
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

	// Token: 0x04004CBA RID: 19642
	[SerializeField]
	private GameObject m_panel;

	// Token: 0x04004CBB RID: 19643
	[SerializeField]
	private TMP_Text _text;

	// Token: 0x04004CBC RID: 19644
	[SerializeField]
	private float m_updateInterval = 0.3f;

	// Token: 0x04004CBD RID: 19645
	private int m_storedVSyncCount;

	// Token: 0x04004CBE RID: 19646
	protected float _framesAccumulated;

	// Token: 0x04004CBF RID: 19647
	protected float _framesDrawnInTheInterval;

	// Token: 0x04004CC0 RID: 19648
	protected float _timeLeft;

	// Token: 0x04004CC1 RID: 19649
	protected int _currentFPS;

	// Token: 0x04004CC2 RID: 19650
	private Action<MonoBehaviour, EventArgs> m_onToggleFPSCounter;

	// Token: 0x04004CC3 RID: 19651
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
