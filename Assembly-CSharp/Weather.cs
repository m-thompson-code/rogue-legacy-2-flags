using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

// Token: 0x0200056E RID: 1390
public class Weather : MonoBehaviour
{
	// Token: 0x17001271 RID: 4721
	// (get) Token: 0x060032FC RID: 13052 RVA: 0x000AC405 File Offset: 0x000AA605
	// (set) Token: 0x060032FD RID: 13053 RVA: 0x000AC40D File Offset: 0x000AA60D
	public List<BaseRoom> RoomList { get; private set; } = new List<BaseRoom>();

	// Token: 0x17001272 RID: 4722
	// (get) Token: 0x060032FE RID: 13054 RVA: 0x000AC416 File Offset: 0x000AA616
	// (set) Token: 0x060032FF RID: 13055 RVA: 0x000AC41E File Offset: 0x000AA61E
	public bool IsHeirloomWeather { get; set; }

	// Token: 0x06003300 RID: 13056 RVA: 0x000AC428 File Offset: 0x000AA628
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

	// Token: 0x06003301 RID: 13057 RVA: 0x000AC4B0 File Offset: 0x000AA6B0
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06003302 RID: 13058 RVA: 0x000AC4C0 File Offset: 0x000AA6C0
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

	// Token: 0x06003303 RID: 13059 RVA: 0x000AC594 File Offset: 0x000AA794
	private bool GetWeatherShouldBeActive(BaseRoom room)
	{
		bool result = false;
		if (this.RoomList.Contains(room))
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06003304 RID: 13060 RVA: 0x000AC5B4 File Offset: 0x000AA7B4
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

	// Token: 0x06003305 RID: 13061 RVA: 0x000AC61E File Offset: 0x000AA81E
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

	// Token: 0x06003306 RID: 13062 RVA: 0x000AC634 File Offset: 0x000AA834
	private void Update()
	{
		if (!this.m_followCamera)
		{
			return;
		}
		base.gameObject.transform.localPosition = CameraController.GameCamera.transform.localPosition;
	}

	// Token: 0x040027C5 RID: 10181
	private const int MAX_PARTICLE_SYSTEM_SEED_VALUE = 1000;

	// Token: 0x040027C6 RID: 10182
	private ParticleSystem[] m_particleSystems;

	// Token: 0x040027C7 RID: 10183
	private Coroutine m_initializationCoroutine;

	// Token: 0x040027C8 RID: 10184
	private bool m_followCamera = true;

	// Token: 0x040027C9 RID: 10185
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;
}
