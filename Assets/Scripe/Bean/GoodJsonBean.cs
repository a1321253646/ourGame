using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoodJsonBean 
{
    public long id;
    public string name;
    public long type;
    public long stacking;
    public long tabid;
    public long sortID;
    public string icon;
    public string describe;
    public string extra;
    List<long> bookList = new List<long>();
    public long getBookId() {
        string[] spilts =  extra.Split('，');
        foreach (string id in spilts) {
            bookList.Add(long.Parse(id));
        }
        if (bookList.Count == 1 && bookList[0] == 0) {
            return -1;
        }
        return getID(bookList);
    }
    private long getID(List<long> bookList) {
        int rangeRadomNum = Random.Range((int)0, (int)bookList.Count);
        return bookList[rangeRadomNum];
    }
}
