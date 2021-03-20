using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo : MonoBehaviour
{
    //Información a guardar
    public static bool haveGyroscope;


    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("conservar");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
