using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossCardJsonBean : MonoBehaviour
{
    public long id;
    public string value;
    public long random;

    private List<long> cardList = null;


    public List<long> getCardList() {
        if (cardList == null) {
            cardList = new List<long>();
            if (value != null && value.Length > 0) {
                string[] list =  value.Split(',');
                if (list.Length > 0) {
                    foreach (string str in list) {
                        cardList.Add(long.Parse(str));
                    }
                }
            }
        }
        return cardList;
    }
}
