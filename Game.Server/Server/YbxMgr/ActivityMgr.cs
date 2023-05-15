// Decompiled with JetBrains decompiler
// Type: YbxMgr.ActivityMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 23E379A8-7519-4E9F-8BEC-E46601E9640E
// Assembly location: C:\server\road\Game.Server.dll

using Bussiness;
using EntityDatabase.ServerModels;
using Game.Base.Packets;
using Game.Server.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace YbxMgr
{
  public static class ActivityMgr
  {
    public static TrialEliteModels _TrialEliteModel;
    public static List<TrialEliteModel_Equip> _TrialEliteModel_Equip;
    public static List<TemplateAwayUses> listtemplateAwayUse;
    public static NapLanDaus napLanDau;
    public static List<NapLanDau_Qua> _NapLanDau_Qua;

    public static bool Init()
    {
      ServerData serverData = new ServerData();
      try
      {
        ActivityMgr._TrialEliteModel = serverData.TrialEliteModels.FirstOrDefault<TrialEliteModels>();
        ActivityMgr._TrialEliteModel_Equip = serverData.TrialEliteModel_Equip.ToList<TrialEliteModel_Equip>();
        ActivityMgr.listtemplateAwayUse = serverData.TemplateAwayUses.Where<TemplateAwayUses>((Expression<Func<TemplateAwayUses, bool>>) (p => p.Active)).ToList<TemplateAwayUses>();
        ActivityMgr.napLanDau = serverData.NapLanDaus.FirstOrDefault<NapLanDaus>();
        ActivityMgr._NapLanDau_Qua = serverData.NapLanDau_Qua.Where<NapLanDau_Qua>((Expression<Func<NapLanDau_Qua, bool>>) (p => p.IsActive)).ToList<NapLanDau_Qua>();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static void SendTrailElite(GamePlayer player, int type)
    {
      GSPacketIn packet = new GSPacketIn((short) 84, player.PlayerId);
      packet.WriteInt(14);
      packet.WriteInt(type);
      if (type == 2)
      {
        packet.WriteBoolean(ActivityMgr._TrialEliteModel.isOpen);
        packet.WriteInt(player.PlayerCharacter.TrialEliteModel.battleRank);
        packet.WriteInt(player.PlayerCharacter.TrialEliteModel.battleScore);
        packet.WriteInt(player.PlayerCharacter.TrialEliteModel.totalCount);
        packet.WriteInt(player.PlayerCharacter.TrialEliteModel.totalWin);
        packet.WriteInt(5);
        packet.WriteInt(1);
        packet.WriteInt(10);
        packet.WriteInt((int) Convert.ToInt16((ActivityMgr._TrialEliteModel.lastDays - DateTime.Now).TotalDays));
      }
      else
      {
        packet.WriteBoolean(ActivityMgr._TrialEliteModel.isOpen);
        packet.WriteInt((int) Convert.ToInt16((ActivityMgr._TrialEliteModel.lastDays - DateTime.Now).TotalDays));
      }
      player.Out.SendTCP(packet);
    }

    public static void SendvipMerryDiscount(GamePlayer player)
    {
      GSPacketIn packet = new GSPacketIn((short) 145, player.PlayerId);
      packet.WriteByte((byte) 169);
      packet.WriteInt(2);
      packet.WriteBoolean(false);
      player.Out.SendTCP(packet);
    }

    public static void SendBallGame(GamePlayer player)
    {
      if (!(Convert.ToDateTime(GameProperties.NewYearBeginDate) <= DateTime.Now) || !(DateTime.Now <= Convert.ToDateTime(GameProperties.NewYearEndDate)))
        return;
      byte val = 1;
      if (DateTime.Now == Convert.ToDateTime(GameProperties.NewYearEndDate).AddDays(-1.0))
        val = (byte) 2;
      GSPacketIn packet = new GSPacketIn((short) 885, player.PlayerId);
      packet.WriteByte((byte) 1);
      packet.WriteByte(val);
      player.Out.SendTCP(packet);
    }

    public static void SendFistRecharge(GamePlayer player)
    {
      GSPacketIn packet = new GSPacketIn((short) 150, player.PlayerId);
      packet.WriteBoolean(true);
      packet.WriteInt(ActivityMgr.napLanDau.RechargeMoneyTotal);
      packet.WriteInt(1);
      packet.WriteInt(1);
      packet.WriteString(ActivityMgr.napLanDau.EndDate.ToString("yyyy-MM-dd hh:ss:mm"));
      packet.WriteInt(ActivityMgr._NapLanDau_Qua.Count);
      foreach (NapLanDau_Qua napLanDauQua in ActivityMgr._NapLanDau_Qua)
      {
        packet.WriteInt(napLanDauQua.RewardItemID);
        packet.WriteInt(napLanDauQua.RewardItemCount);
        packet.WriteInt(napLanDauQua.RewardItemValid);
        packet.WriteBoolean(napLanDauQua.IsBind);
        packet.WriteInt(napLanDauQua.StrengthenLevel);
        packet.WriteInt(napLanDauQua.AttackCompose);
        packet.WriteInt(napLanDauQua.DefendCompose);
        packet.WriteInt(napLanDauQua.AgilityCompose);
        packet.WriteInt(napLanDauQua.LuckCompose);
      }
      player.Out.SendTCP(packet);
    }
  }
}
