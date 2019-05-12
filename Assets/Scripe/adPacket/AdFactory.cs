using UnityEngine;
using System.Collections;

public class AdFactory 
{
    public static AdInterface creatInterface() {
        return new AdInterface();
    }
}
