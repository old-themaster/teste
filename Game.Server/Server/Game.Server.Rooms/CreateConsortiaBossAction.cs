﻿using Game.Logic;
using Game.Server.Games;
using Game.Server.Managers;
using System.Collections.Generic;
namespace Game.Server.Rooms
{
    public class CreateConsortiaBossAction : IAction
	{
		private GamePlayer m_player;
		private string m_name;
		private string m_password;
		private eRoomType m_roomType;
		private byte m_timeType;
		private int m_bossLevel;
		public CreateConsortiaBossAction(GamePlayer player, eRoomType roomType, int bossLevel)
		{
			this.m_player = player;
			this.m_name = "Consortia Boss";
			this.m_password = "12dasSda44";
			this.m_roomType = roomType;
			this.m_timeType = 2;
			this.m_bossLevel = bossLevel;
		}
		public void Execute()
		{
			if (this.m_player.CurrentRoom != null)
			{
				this.m_player.CurrentRoom.RemovePlayerUnsafe(this.m_player);
			}
			if (!this.m_player.IsActive)
			{
				return;
			}
			BaseRoom[] rooms = RoomMgr.Rooms;
			BaseRoom baseRoom = null;
			for (int i = 0; i < rooms.Length; i++)
			{
				if (!rooms[i].IsUsing)
				{
					baseRoom = rooms[i];
					break;
				}
			}
			if (baseRoom != null)
			{
				RoomMgr.WaitingRoom.RemovePlayer(this.m_player);
				baseRoom.Start();
				baseRoom.HardLevel = eHardLevel.Normal;
				baseRoom.LevelLimits = (int)baseRoom.GetLevelLimit(this.m_player);
				baseRoom.isCrosszone = false;
				baseRoom.isOpenBoss = false;
				baseRoom.MapId = 1291;
				baseRoom.currentFloor = this.m_bossLevel;
				baseRoom.TimeMode = this.m_timeType;
				baseRoom.UpdateRoom(this.m_name, this.m_password, this.m_roomType, this.m_timeType, baseRoom.MapId);
				this.m_player.Out.SendRoomCreate(baseRoom);
				this.m_player.AllWorldDameBoss = ConsortiaBossMgr.GetConsortiaBossTotalDame(this.m_player.PlayerCharacter.ConsortiaID);
				if (baseRoom.AddPlayerUnsafe(this.m_player))
				{
					List<GamePlayer> players = baseRoom.GetPlayers();
					List<IGamePlayer> list = new List<IGamePlayer>();
					foreach (GamePlayer current in players)
					{
						if (current != null)
						{
							list.Add(current);
						}
					}
					BaseGame baseGame = GameMgr.StartPVEGame(baseRoom.RoomId, list, baseRoom.MapId, baseRoom.RoomType, baseRoom.GameType, (int)baseRoom.TimeMode, baseRoom.HardLevel, baseRoom.LevelLimits, baseRoom.currentFloor);
					if (baseGame != null)
					{
						baseRoom.IsPlaying = true;
						baseRoom.StartGame(baseGame);
						return;
					}
					baseRoom.IsPlaying = false;
					baseRoom.SendPlayerState();
				}
			}
		}
	}
}