// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.ConsortiaInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;
using System.Collections.Generic;

namespace SqlDataProvider.Data
{
  public class ConsortiaInfo : DataObject
  {
    private int _badgeID;
    private DateTime _buildDate;
    private int _celebCount;
    private int _consortiaID;
    private string _consortiaName;
    private int _count;
    private int _creatorID;
    private string _creatorName;
    private int _chairmanID;
    private string _chairmanName;
    private byte _chairmanTypeVIP;
    private int _chairmanVIPLevel;
    private DateTime _deductDate;
    private string _description;
    private int _extendAvailableNum;
    private int _fightPower;
    private int _honor;
    private string _ip;
    private bool _isExist;
    private int _level;
    private int _maxCount;
    private string _placard;
    private int _port;
    private int _repute;
    private int _riches;
    private Dictionary<string, RankingPersonInfo> m_rankList;
    public long MaxBlood;
    public long TotalAllMemberDame;

    public int AddDayHonor { get; set; }

    public int AddDayRiches { get; set; }

    public int AddWeekHonor { get; set; }

    public int AddWeekRiches { get; set; }

    public string BadgeBuyTime { get; set; }

    public int BadgeID
    {
      get => this._badgeID;
      set
      {
        this._badgeID = value;
        this._isDirty = true;
      }
    }

    public string BadgeName { get; set; }

    public int BadgeType { get; set; }

    public int bossState { get; set; }

    public DateTime BuildDate
    {
      get => this._buildDate;
      set
      {
        this._buildDate = value;
        this._isDirty = true;
      }
    }

    public int callBossLevel { get; set; }

    public int CelebCount
    {
      get => this._celebCount;
      set
      {
        this._celebCount = value;
        this._isDirty = true;
      }
    }

    public int ConsortiaID
    {
      get => this._consortiaID;
      set
      {
        this._consortiaID = value;
        this._isDirty = true;
      }
    }

    public string ConsortiaName
    {
      get => this._consortiaName;
      set
      {
        this._consortiaName = value;
        this._isDirty = true;
      }
    }

    public int Count
    {
      get => this._count;
      set
      {
        this._count = value;
        this._isDirty = true;
      }
    }

    public int CreatorID
    {
      get => this._creatorID;
      set
      {
        this._creatorID = value;
        this._isDirty = true;
      }
    }

    public string CreatorName
    {
      get => this._creatorName;
      set
      {
        this._creatorName = value;
        this._isDirty = true;
      }
    }

    public int ChairmanID
    {
      get => this._chairmanID;
      set
      {
        this._chairmanID = value;
        this._isDirty = true;
      }
    }

    public string ChairmanName
    {
      get => this._chairmanName;
      set
      {
        this._chairmanName = value;
        this._isDirty = true;
      }
    }

    public byte ChairmanTypeVIP
    {
      get => this._chairmanTypeVIP;
      set
      {
        this._chairmanTypeVIP = value;
        this._isDirty = true;
      }
    }

    public int ChairmanVIPLevel
    {
      get => this._chairmanVIPLevel;
      set
      {
        this._chairmanVIPLevel = value;
        this._isDirty = true;
      }
    }

    public DateTime DeductDate
    {
      get => this._deductDate;
      set
      {
        this._deductDate = value;
        this._isDirty = true;
      }
    }

    public string Description
    {
      get => this._description;
      set
      {
        this._description = value;
        this._isDirty = true;
      }
    }

    public DateTime endTime { get; set; }

    public int extendAvailableNum
    {
      get => this._extendAvailableNum;
      set
      {
        this._extendAvailableNum = value;
        this._isDirty = true;
      }
    }

    public int FightPower
    {
      get => this._fightPower;
      set
      {
        this._fightPower = value;
        this._isDirty = true;
      }
    }

    public int Honor
    {
      get => this._honor;
      set
      {
        this._honor = value;
        this._isDirty = true;
      }
    }

    public string IP
    {
      get => this._ip;
      set
      {
        this._ip = value;
        this._isDirty = true;
      }
    }

    public bool IsBossDie { get; set; }

    public bool IsExist
    {
      get => this._isExist;
      set
      {
        this._isExist = value;
        this._isDirty = true;
      }
    }

    public bool IsSendAward { get; set; }

    public bool IsVoting { get; set; }

    public int LastDayRiches { get; set; }

    public DateTime LastOpenBoss { get; set; }

    public int Level
    {
      get => this._level;
      set
      {
        this._level = value;
        this._isDirty = true;
      }
    }

    public int MaxCount
    {
      get => this._maxCount;
      set
      {
        this._maxCount = value;
        this._isDirty = true;
      }
    }

    public bool OpenApply { get; set; }

    public string Placard
    {
      get => this._placard;
      set
      {
        this._placard = value;
        this._isDirty = true;
      }
    }

    public int Port
    {
      get => this._port;
      set
      {
        this._port = value;
        this._isDirty = true;
      }
    }

    public Dictionary<string, RankingPersonInfo> RankList
    {
      get => this.m_rankList;
      set => this.m_rankList = value;
    }

    public int Repute
    {
      get => this._repute;
      set
      {
        this._repute = value;
        this._isDirty = true;
      }
    }

    public int Riches
    {
      get => this._riches;
      set
      {
        this._riches = value;
        this._isDirty = true;
      }
    }

    public bool SendToClient { get; set; }

    public int ShopLevel { get; set; }

    public int SkillLevel { get; set; }

    public int SmithLevel { get; set; }

    public int StoreLevel { get; set; }

    public int ValidDate { get; set; }

    public int VoteRemainDay { get; set; }

    public DateTime DateOpenTask { get; set; }
  }
}
