using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

namespace RLAudio
{
	// Token: 0x02000E63 RID: 3683
	public class DualChoiceSpecialRoomAudioEventEmitterController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700213D RID: 8509
		// (get) Token: 0x060067EC RID: 26604 RVA: 0x0003974F File Offset: 0x0003794F
		public string Description
		{
			get
			{
				if (this.m_description == string.Empty)
				{
					this.m_description = this.ToString();
				}
				return this.m_description;
			}
		}

		// Token: 0x060067ED RID: 26605 RVA: 0x0017E654 File Offset: 0x0017C854
		private void Awake()
		{
			this.m_room = base.GetComponent<Room>();
			this.m_room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
			this.m_specialRoomController = base.GetComponent<BaseSpecialRoomController>();
			this.m_specialRoomController.RoomCompletedRelay.AddListener(new Action(this.OnRoomComplete), false);
			this.m_onLeftChoiceHover = new Action<GameObject>(this.OnLeftChoiceHover);
			this.m_onRightChoiceHover = new Action<GameObject>(this.OnRightChoiceHover);
		}

		// Token: 0x060067EE RID: 26606 RVA: 0x0017E6DC File Offset: 0x0017C8DC
		protected virtual void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
		{
			if (!this.m_dualChoiceController)
			{
				PropSpawnController[] propSpawnControllers = this.m_room.SpawnControllerManager.PropSpawnControllers;
				for (int i = 0; i < propSpawnControllers.Length; i++)
				{
					if (propSpawnControllers[i].ShouldSpawn && !(propSpawnControllers[i].PropInstance == null))
					{
						this.m_dualChoiceController = propSpawnControllers[i].PropInstance.gameObject.GetComponent<DualChoicePropController>();
						if (this.m_dualChoiceController)
						{
							Interactable[] componentsInChildren = propSpawnControllers[i].PropInstance.gameObject.GetComponentsInChildren<Interactable>();
							for (int j = 0; j < componentsInChildren.Length; j++)
							{
								if (!componentsInChildren[j].TriggerOnTouch)
								{
									componentsInChildren[j].m_triggerOnEnterEvent.AddListener(new UnityAction<GameObject>(this.OnChoiceMade));
								}
							}
							break;
						}
					}
				}
			}
			if (this.m_dualChoiceController)
			{
				this.m_dualChoiceController.LeftHoverRelay.AddListener(this.m_onLeftChoiceHover, false);
				this.m_dualChoiceController.RightHoverRelay.AddListener(this.m_onRightChoiceHover, false);
			}
			if (!this.m_isRoomComplete)
			{
				AudioManager.PlayOneShot(this, this.m_roomDiscoveredAudioEventPath, default(Vector3));
			}
		}

		// Token: 0x060067EF RID: 26607 RVA: 0x00039775 File Offset: 0x00037975
		private void OnDisable()
		{
			if (this.m_dualChoiceController)
			{
				this.m_dualChoiceController.LeftHoverRelay.RemoveListener(this.m_onLeftChoiceHover);
				this.m_dualChoiceController.RightHoverRelay.RemoveListener(this.m_onRightChoiceHover);
			}
		}

		// Token: 0x060067F0 RID: 26608 RVA: 0x0017E800 File Offset: 0x0017CA00
		private void OnChoiceMade(GameObject gameObj)
		{
			if (this.m_choiceMadeUnityEvent != null)
			{
				this.m_choiceMadeUnityEvent.Invoke();
			}
			string path = this.m_leftChosenAudioEventPath;
			if (this.m_currentChoice == RoomSide.Right)
			{
				path = this.m_rightChosenAudioEventPath;
			}
			this.PlayChoiceAudio(path, gameObj.transform.position);
		}

		// Token: 0x060067F1 RID: 26609 RVA: 0x000397B2 File Offset: 0x000379B2
		private void OnLeftChoiceHover(GameObject gameObj)
		{
			this.m_currentChoice = RoomSide.Left;
			this.m_currentChoiceGameObject = gameObj;
			this.PlayChoiceAudio(this.m_rightHoverAudioEventPath, gameObj.transform.position);
		}

		// Token: 0x060067F2 RID: 26610 RVA: 0x000397D9 File Offset: 0x000379D9
		private void OnRightChoiceHover(GameObject gameObj)
		{
			this.m_currentChoice = RoomSide.Right;
			this.m_currentChoiceGameObject = gameObj;
			this.PlayChoiceAudio(this.m_rightHoverAudioEventPath, gameObj.transform.position);
		}

		// Token: 0x060067F3 RID: 26611 RVA: 0x00039800 File Offset: 0x00037A00
		private void PlayChoiceAudio(string path, Vector3 position)
		{
			AudioManager.PlayOneShot(this, path, position);
		}

		// Token: 0x060067F4 RID: 26612 RVA: 0x0017E84C File Offset: 0x0017CA4C
		protected virtual void OnRoomComplete()
		{
			AudioManager.PlayOneShot(this, this.m_roomCompleteAudioEventPath, default(Vector3));
			this.m_isRoomComplete = true;
		}

		// Token: 0x0400545C RID: 21596
		[SerializeField]
		[EventRef]
		private string m_leftHoverAudioEventPath;

		// Token: 0x0400545D RID: 21597
		[SerializeField]
		[EventRef]
		private string m_rightHoverAudioEventPath;

		// Token: 0x0400545E RID: 21598
		[SerializeField]
		[EventRef]
		private string m_leftChosenAudioEventPath;

		// Token: 0x0400545F RID: 21599
		[SerializeField]
		[EventRef]
		private string m_rightChosenAudioEventPath;

		// Token: 0x04005460 RID: 21600
		[SerializeField]
		[EventRef]
		private string m_roomDiscoveredAudioEventPath;

		// Token: 0x04005461 RID: 21601
		[SerializeField]
		[EventRef]
		private string m_roomCompleteAudioEventPath;

		// Token: 0x04005462 RID: 21602
		[SerializeField]
		private UnityEvent m_choiceMadeUnityEvent;

		// Token: 0x04005463 RID: 21603
		private string m_description = string.Empty;

		// Token: 0x04005464 RID: 21604
		private RoomSide m_currentChoice = RoomSide.None;

		// Token: 0x04005465 RID: 21605
		private GameObject m_currentChoiceGameObject;

		// Token: 0x04005466 RID: 21606
		protected bool m_isRoomComplete;

		// Token: 0x04005467 RID: 21607
		private DualChoicePropController m_dualChoiceController;

		// Token: 0x04005468 RID: 21608
		private BaseSpecialRoomController m_specialRoomController;

		// Token: 0x04005469 RID: 21609
		private Room m_room;

		// Token: 0x0400546A RID: 21610
		private Action<GameObject> m_onLeftChoiceHover;

		// Token: 0x0400546B RID: 21611
		private Action<GameObject> m_onRightChoiceHover;
	}
}
