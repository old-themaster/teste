using SqlDataProvider.Data;

public class ActivityQuestUserData : DataObject
{
    private int _UserID;
    private int _questid;
    private int _condictionid;
    private int _condiction1;
    private int _condiction2;
    private int _condiction3;
    private int _condiction4;
    private bool _isfinished;
    private bool _iscompleted;

    public int UserID { get => _UserID; set { _UserID = value; IsDirty = true; } }

    public int QuestID { get => _questid; set { _questid = value; IsDirty = true; } }

    public int CondictionID { get => _condictionid; set { _condictionid = value; IsDirty = true; } }

    public int Condiction1 { get => _condiction1; set { _condiction1 = value; IsDirty = true; } }

    public int Condiction2 { get => _condiction2; set { _condiction2 = value; IsDirty = true; } }

    public int Condiction3 { get => _condiction3; set { _condiction3 = value; IsDirty = true; } }

    public int Condiction4 { get => _condiction4; set { _condiction4 = value; IsDirty = true; } }

    public bool IsFinished { get => _isfinished; set { _isfinished = value; IsDirty = true; } }

    public bool IsCompleted { get => _iscompleted; set { _iscompleted = value; IsDirty = true; } }

    public int GetIndex(int Index)
    {
        switch (Index)
        {
            case 2:
                return Condiction2;
            case 3:
                return Condiction3;
            case 4:
                return Condiction4;
            default:
                return Condiction1;
        }
    }

    public void SetIndex(int Index, int Value)
    {
        switch (Index)
        {
            case 2:
                Condiction2 = Value;
                break;
            case 3:
                Condiction3 = Value;
                break;
            case 4:
                Condiction4 = Value;
                break;

            default:
                Condiction1 = Value;
                break;
        }
    }

}
