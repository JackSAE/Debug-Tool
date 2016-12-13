using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

public class Test : MonoBehaviour {

    public string test = "test";
    public int testInt = 1;
    public Type myTypetest;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        myTypetest = typeof(Test);
        //Debug.LogError(typeof(Test).Namespace);


    }
}
