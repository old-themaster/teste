// Decompiled with JetBrains decompiler
// Type: YbxMgr.DevilTurnMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 23E379A8-7519-4E9F-8BEC-E46601E9640E
// Assembly location: C:\server\road\Game.Server.dll

using Bussiness;
using EntityDatabase.PlayerModels;
using EntityDatabase.ServerModels;
using Game.Base.Packets;
using Game.Server.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YbxMgr
{
  public static class DevilTurnMgr
  {
    public static List<DevilTreasItemLists> ListDevilTreasItemList;
    public static List<DevilTreasPointsLists> ListDevilTreasPointsList;
    public static List<DevilTreasrankRewardlists> ListDevilTreasrankRewardlist;
    public static List<DevilTreassArahtoBoxLists> ListDevilTreassArahtoBoxList;
    public static List<DevilTurnOpenBoxes> ListDevilTurnOpenBox;
    public static List<Ybx_93_DevilTurn_Rank> DevilTurn_Ranks = new List<Ybx_93_DevilTurn_Rank>();
    public static int JackBox = 0;

    public static bool InIt()
    {
      try
      {
        ServerData serverData = new ServerData();
        DevilTurnMgr.ListDevilTreasItemList = serverData.DevilTreasItemLists.ToList<DevilTreasItemLists>();
        DevilTurnMgr.ListDevilTreasPointsList = serverData.DevilTreasPointsLists.ToList<DevilTreasPointsLists>();
        DevilTurnMgr.ListDevilTreasrankRewardlist = serverData.DevilTreasrankRewardlists.ToList<DevilTreasrankRewardlists>();
        DevilTurnMgr.ListDevilTreassArahtoBoxList = serverData.DevilTreassArahtoBoxLists.ToList<DevilTreassArahtoBoxLists>();
        DevilTurnMgr.ListDevilTurnOpenBox = serverData.DevilTurnOpenBoxes.ToList<DevilTurnOpenBoxes>();
        GameBallMgr.Init();
        SealPetMgrLogic.Intit();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static int GetNeedMoneyToDice(int BoxId, int Index) => DevilTurnMgr.ListDevilTurnOpenBox.Where<DevilTurnOpenBoxes>((Func<DevilTurnOpenBoxes, bool>) (p => p.BoxId == BoxId && p.BoxIndex == Index)).FirstOrDefault<DevilTurnOpenBoxes>().NeedMoney;

    public static void GetData(GSPacketIn pkg)
    {
      DevilTurnMgr.JackBox = pkg.ReadInt();
      int num1 = pkg.ReadInt();
      for (int index = 0; index < num1; ++index)
      {
        string str1 = pkg.ReadString();
        int num2 = pkg.ReadInt();
        string str2 = pkg.ReadString();
        int ServerId = pkg.ReadInt();
        int UserId = pkg.ReadInt();
        Ybx_93_DevilTurn_Rank ybx93DevilTurnRank = DevilTurnMgr.DevilTurn_Ranks.Where<Ybx_93_DevilTurn_Rank>((Func<Ybx_93_DevilTurn_Rank, bool>) (p => p.UserID == UserId && p.ServerId == ServerId)).FirstOrDefault<Ybx_93_DevilTurn_Rank>();
        if (ybx93DevilTurnRank == null)
          DevilTurnMgr.DevilTurn_Ranks.Add(new Ybx_93_DevilTurn_Rank()
          {
            UserID = UserId,
            ServerId = ServerId,
            NickName = str1,
            Score = num2,
            ServerName = str2
          });
        else
          ybx93DevilTurnRank.Score = num2;
      }
    }

    public static DevilTreasItemLists GetItem()
    {
      int maxRound = ThreadSafeRandom.NextStatic(DevilTurnMgr.ListDevilTreasItemList.Select<DevilTreasItemLists, int>((Func<DevilTreasItemLists, int>) (s => s.Random)).Max());
      List<DevilTreasItemLists> list = DevilTurnMgr.ListDevilTreasItemList.Where<DevilTreasItemLists>((Func<DevilTreasItemLists, bool>) (s => s.Random >= maxRound)).ToList<DevilTreasItemLists>();
      Random random = new Random();
      return list[random.Next(0, list.Count - 1)];
    }
  }
}
