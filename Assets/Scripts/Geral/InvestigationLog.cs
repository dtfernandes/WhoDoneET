using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class InvestigationLog
{
    [field:SerializeField]
    public List<LogItem> Items { get; private set; }

    public InvestigationLog()
    {
        Items = new List<LogItem> { };
    }

    public void AddItem(LogItem item)
    {

        //Check if the item is already in the log

        if(Items.Any(x=> x.GUID == item.GUID))
        {
            return;
        }


        Items.Add(item);
    }
}
