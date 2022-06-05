using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000948 RID: 2376
	[AddComponentMenu("")]
	public class PressStartToJoinExample_Assigner : MonoBehaviour
	{
		// Token: 0x06005088 RID: 20616 RVA: 0x0011C5D4 File Offset: 0x0011A7D4
		public static Player GetRewiredPlayer(int gamePlayerId)
		{
			if (!ReInput.isReady)
			{
				return null;
			}
			if (PressStartToJoinExample_Assigner.instance == null)
			{
				Debug.LogError("Not initialized. Do you have a PressStartToJoinPlayerSelector in your scehe?");
				return null;
			}
			for (int i = 0; i < PressStartToJoinExample_Assigner.instance.playerMap.Count; i++)
			{
				if (PressStartToJoinExample_Assigner.instance.playerMap[i].gamePlayerId == gamePlayerId)
				{
					return ReInput.players.GetPlayer(PressStartToJoinExample_Assigner.instance.playerMap[i].rewiredPlayerId);
				}
			}
			return null;
		}

		// Token: 0x06005089 RID: 20617 RVA: 0x0011C656 File Offset: 0x0011A856
		private void Awake()
		{
			this.playerMap = new List<PressStartToJoinExample_Assigner.PlayerMap>();
			PressStartToJoinExample_Assigner.instance = this;
		}

		// Token: 0x0600508A RID: 20618 RVA: 0x0011C66C File Offset: 0x0011A86C
		private void Update()
		{
			for (int i = 0; i < ReInput.players.playerCount; i++)
			{
				if (ReInput.players.GetPlayer(i).GetButtonDown("JoinGame"))
				{
					this.AssignNextPlayer(i);
				}
			}
		}

		// Token: 0x0600508B RID: 20619 RVA: 0x0011C6AC File Offset: 0x0011A8AC
		private void AssignNextPlayer(int rewiredPlayerId)
		{
			if (this.playerMap.Count >= this.maxPlayers)
			{
				Debug.LogError("Max player limit already reached!");
				return;
			}
			int nextGamePlayerId = this.GetNextGamePlayerId();
			this.playerMap.Add(new PressStartToJoinExample_Assigner.PlayerMap(rewiredPlayerId, nextGamePlayerId));
			Player player = ReInput.players.GetPlayer(rewiredPlayerId);
			player.controllers.maps.SetMapsEnabled(false, "Assignment");
			player.controllers.maps.SetMapsEnabled(true, "Default");
			Debug.Log("Added Rewired Player id " + rewiredPlayerId.ToString() + " to game player " + nextGamePlayerId.ToString());
		}

		// Token: 0x0600508C RID: 20620 RVA: 0x0011C74C File Offset: 0x0011A94C
		private int GetNextGamePlayerId()
		{
			int num = this.gamePlayerIdCounter;
			this.gamePlayerIdCounter = num + 1;
			return num;
		}

		// Token: 0x040042D5 RID: 17109
		private static PressStartToJoinExample_Assigner instance;

		// Token: 0x040042D6 RID: 17110
		public int maxPlayers = 4;

		// Token: 0x040042D7 RID: 17111
		private List<PressStartToJoinExample_Assigner.PlayerMap> playerMap;

		// Token: 0x040042D8 RID: 17112
		private int gamePlayerIdCounter;

		// Token: 0x02000F0B RID: 3851
		private class PlayerMap
		{
			// Token: 0x06007016 RID: 28694 RVA: 0x0019ECD2 File Offset: 0x0019CED2
			public PlayerMap(int rewiredPlayerId, int gamePlayerId)
			{
				this.rewiredPlayerId = rewiredPlayerId;
				this.gamePlayerId = gamePlayerId;
			}

			// Token: 0x04005A45 RID: 23109
			public int rewiredPlayerId;

			// Token: 0x04005A46 RID: 23110
			public int gamePlayerId;
		}
	}
}
