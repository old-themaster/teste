// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.PveInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class PveInfo
  {
        /*  public int GetPrice(int selectedLevel)
          {
              int num = 1;
              string[] strArray = string.IsNullOrEmpty(this.BossFightNeedMoney) ? new string[] { } : this.BossFightNeedMoney.Split('|');
              if ((uint)strArray.Length > 0U)
              {
                  switch (selectedLevel)
                  {
                      case 0:
                          num = int.Parse(strArray[0]);
                          break;
                      case 1:
                          num = int.Parse(strArray[1]);
                          break;
                      case 2:
                          num = int.Parse(strArray[2]);
                          break;
                      case 3:
                          num = int.Parse(strArray[3]);
                          break;
                      case 4:
                          num = int.Parse(strArray[4]);
                          break;
                  }
              }
              return num;
          }*/
        public int getPrice(int selectedLevel)
        {
            int num = 1;
            string[] strArray = string.IsNullOrEmpty(this.BossFightNeedMoney) ? new string[] { } : this.BossFightNeedMoney.Split('|');
            if ((uint)strArray.Length > 0U)
            {
                switch (selectedLevel)
                {
                    case 0:
                        num = int.Parse(strArray[0]);
                        break;
                    case 1:
                        num = int.Parse(strArray[1]);
                        break;
                    case 2:
                        num = int.Parse(strArray[2]);
                        break;
                    case 3:
                        num = int.Parse(strArray[3]);
                        break;
                    case 4:
                        num = int.Parse(strArray[4]);
                        break;
                }
            }

            return num;
        }

        public string AdviceTips { get; set; }

    public string BossFightNeedMoney { get; set; }

    public string Description { get; set; }

    public string EpicGameScript { get; set; }

    public string EpicTemplateIds { get; set; }

    public string HardGameScript { get; set; }

    public string HardTemplateIds { get; set; }

    public int ID { get; set; }

    public int LevelLimits { get; set; }

    public string Name { get; set; }

    public string NormalGameScript { get; set; }

    public string NormalTemplateIds { get; set; }

    public int Ordering { get; set; }

    public string Pic { get; set; }

    public string SimpleGameScript { get; set; }

    public string SimpleTemplateIds { get; set; }

    public string TerrorGameScript { get; set; }

    public string TerrorTemplateIds { get; set; }



    public int Type { get; set; }
  }
}
