// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.SubActiveConditionInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System.Collections.Generic;

namespace SqlDataProvider.Data
{
  public class SubActiveConditionInfo
  {
    public int GetValue(string index)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      if (!string.IsNullOrEmpty(this.Value))
      {
        string[] strArray = this.Value.Split('-');
        for (int index1 = 1; index1 < strArray.Length; index1 += 2)
        {
          string key = strArray[index1 - 1];
          if (!dictionary.ContainsKey(key))
            dictionary.Add(key, strArray[index1]);
          else
            dictionary[key] = strArray[index1];
        }
        if (dictionary.ContainsKey(index))
          return int.Parse(dictionary[index]);
      }
      return 0;
    }

    public int ActiveID { get; set; }

    public int AwardType { get; set; }

    public string AwardValue { get; set; }

    public int ConditionID { get; set; }

    public int ID { get; set; }

    public bool IsValid { get; set; }

    public int SubID { get; set; }

    public int Type { get; set; }

    public string Value { get; set; }
  }
}
