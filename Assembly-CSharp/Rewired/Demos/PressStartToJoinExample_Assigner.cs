using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000EE6 RID: 3814
	[AddComponentMenu("")]
	public class PressStartToJoinExample_Assigner : MonoBehaviour
	{
		// Token: 0x06006E62 RID: 28258 RVA: 0x0018AC88 File Offset: 0x00188E88
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

		// Token: 0x06006E63 RID: 28259 RVA: 0x0003CB30 File Offset: 0x0003AD30
		private void Awake()
		{
			this.playerMap = new List<PressStartToJoinExample_Assigner.PlayerMap>();
			PressStartToJoinExample_Assigner.instance = this;
		}

		// Token: 0x06006E64 RID: 28260 RVA: 0x0018AD0C File Offset: 0x00188F0C
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

		// Token: 0x06006E65 RID: 28261 RVA: 0x0018AD4C File Offset: 0x00188F4C
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

		// Token: 0x06006E66 RID: 28262 RVA: 0x0018ADEC File Offset: 0x00188FEC
		private int GetNextGamePlayerId()
		{
			int num = this.gamePlayerIdCounter;
			this.gamePlayerIdCounter = num + 1;
			return num;
		}

		// Token: 0x040058B8 RID: 22712
		private static PressStartToJoinExample_Assigner instance;

		// Token: 0x040058B9 RID: 22713
		public int maxPlayers = 4;

		// Token: 0x040058BA RID: 22714
		private List<PressStartToJoinExample_Assigner.PlayerMap> playerMap;

		// Token: 0x040058BB RID: 22715
		private int gamePlayerIdCounter;

		// Token: 0x02000EE7 RID: 3815
		private class PlayerMap
		{
			// Token: 0x06006E68 RID: 28264 RVA: 0x0003CB52 File Offset: 0x0003AD52
			public PlayerMap(int rewiredPlayerId, int gamePlayerId)
			{
				this.rewiredPlayerId = rewiredPlayerId;
				this.gamePlayerId = gamePlayerId;
			}

			// Token: 0x040058BC RID: 22716
			public int rewiredPlayerId;

			// Token: 0x040058BD RID: 22717
			public int gamePlayerId;
		}
	}
}
