using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

// Token: 0x02000940 RID: 2368
public class Weather : MonoBehaviour
{
	// Token: 0x1700193A RID: 6458
	// (get) Token: 0x060047DD RID: 18397 RVA: 0x00027661 File Offset: 0x00025861
	// (set) Token: 0x060047DE RID: 18398 RVA: 0x00027669 File Offset: 0x00025869
	public List<BaseRoom> RoomList { get; private set; } = new List<BaseRoom>();

	// Token: 0x1700193B RID: 6459
	// (get) Token: 0x060047DF RID: 18399 RVA: 0x00027672 File Offset: 0x00025872
	// (set) Token: 0x060047E0 RID: 18400 RVA: 0x0002767A File Offset: 0x0002587A
	public bool IsHeirloomWeather { get; set; }

	// Token: 0x060047E1 RID: 18401 RVA: 0x00116ADC File Offset: 0x00114CDC
	private void Awake()
	{
		this.m_particleSystems = base.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < this.m_particleSystems.Length; i++)
		{
			this.m_particleSystems[i].Stop();
			this.m_particleSystems[i].Clear();
			this.m_particleSystems[i].randomSeed = (uint)UnityEngine.Random.Range(0, 1000);
		}
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		base.gameObject.SetActive(false);
	}

	// Token: 0x060047E2 RID: 18402 RVA: 0x00027683 File Offset: 0x00025883
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x060047E3 RID: 18403 RVA: 0x00116B64 File Offset: 0x00114D64
	private void OnPlayerEnterRoom(MonoBehaviour sender, EventArgs eventArgs)
	{
		RoomViaDoorEventArgs roomViaDoorEventArgs = eventArgs as RoomViaDoorEventArgs;
		if (roomViaDoorEventArgs != null)
		{
			if (this.GetWeatherShouldBeActive(roomViaDoorEventArgs.Room))
			{
				if (!base.gameObject.activeInHierarchy)
				{
					base.gameObject.SetActive(true);
				}
			}
			else if (base.gameObject.activeInHierarchy)
			{
				base.gameObject.SetActive(false);
			}
			if (base.gameObject.activeInHierarchy)
			{
				BaseRoom room = roomViaDoorEventArgs.Room;
				float x = room.Bounds.center.x;
				float y = room.Bounds.center.y;
				if (this.m_particleSystems != null)
				{
					if (this.m_initializationCoroutine != null)
					{
						base.StopCoroutine(this.m_initializationCoroutine);
						this.m_initializationCoroutine = null;
					}
					this.m_initializationCoroutine = base.StartCoroutine(this.InitializeParticleSystems(new Vector2(x, y)));
				}
			}
		}
	}

	// Token: 0x060047E4 RID: 18404 RVA: 0x00116C38 File Offset: 0x00114E38
	private bool GetWeatherShouldBeActive(BaseRoom room)
	{
		bool result = false;
		if (this.RoomList.Contains(room))
		{
			result = true;
		}
		return result;
	}

	// Token: 0x060047E5 RID: 18405 RVA: 0x00116C58 File Offset: 0x00114E58
	private void OnCameraActivated(ICinemachineCamera cinemachineCameraA, ICinemachineCamera cinemachineCameraB)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		Vector2 position = (cinemachineCameraA as CinemachineVirtualCamera).State.FinalPosition;
		if (this.m_particleSystems != null)
		{
			if (this.m_initializationCoroutine != null)
			{
				base.StopCoroutine(this.m_initializationCoroutine);
				this.m_initializationCoroutine = null;
			}
			this.m_initializationCoroutine = base.StartCoroutine(this.InitializeParticleSystems(position));
		}
	}

	// Token: 0x060047E6 RID: 18406 RVA: 0x00027691 File Offset: 0x00025891
	private IEnumerator InitializeParticleSystems(Vector2 position)
	{
		this.m_followCamera = false;
		if (this.m_particleSystems != null)
		{
			base.transform.position = position;
			for (int i = 0; i < this.m_particleSystems.Length; i++)
			{
				this.m_particleSystems[i].Clear();
				this.m_particleSystems[i].Simulate(10f);
				this.m_particleSystems[i].Play();
			}
		}
		yield return null;
		this.m_followCamera = true;
		yield break;
	}

	// Token: 0x060047E7 RID: 18407 RVA: 0x000276A7 File Offset: 0x000258A7
	private void Update()
	{
		if (!this.m_followCamera)
		{
			return;
		}
		base.gameObject.transform.localPosition = CameraController.GameCamera.transform.localPosition;
	}

	// Token: 0x040036F9 RID: 14073
	private const int MAX_PARTICLE_SYSTEM_SEED_VALUE = 1000;

	// Token: 0x040036FA RID: 14074
	private ParticleSystem[] m_particleSystems;

	// Token: 0x040036FB RID: 14075
	private Coroutine m_initializationCoroutine;

	// Token: 0x040036FC RID: 14076
	private bool m_followCamera = true;

	// Token: 0x040036FD RID: 14077
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;
}
