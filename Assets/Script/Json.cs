using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Json : MonoBehaviour
{
    private Logic game;
    private Network net;
    
    public string mot { get; set; }
    public string dicolinkUrl { get; set; }

    public override string ToString()
    {
        return $"mot : {mot}";
    }
    public void test()
    {
        Debug.Log(mot);
    }

    void Start()
    {
        game = GetComponent<Logic>();
        net = GetComponent<Network>();
    }


}
