using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*
    Aim For Debug Tool:
###############################
--> Track FPS
--> Track Objects - object Transform - Object Name - Objects variables 
--> Type of Object
--> Spawning GameObjects 
--> Changing Variables 
--> GUI Interface - easily brought up with "~" key
--> Memory Usage 
##############################
*/


public class Debug_Tool_Reader : MonoBehaviour
{
    //simple struct to store the type and name of variables
    public struct Variable
    {
        public string name;
        public Type type;
    }

    public object getValue(string name)
    {
        return this.GetType().GetProperty(name).GetValue(this, null);
    }

    //Function to Get variables


    //Function to Set Variables 


}

public class Debug_Tool : MonoBehaviour {

    float deltaTime = 0.0f;
    public bool GUIEnabled = false;
    public bool GUIGameObjectShow = false;
    public string hitGameObject;
    public Transform hitGameObjectTransform;
    public string objectValues;
    public string scriptObjectName;

    public string gameObjectType;

    public Vector3 spawnPos;

    List<string> gameObjectList = new List<string>();

    public GameObject[] gameObjectsToSpawn;

    public List<Component> components;
    public bool cacheComponentsNow;

    Debug_Tool_Reader debug_Tool_Reader;

    int x, y = 10;
    public float width = 100;
    public float height = 20;

    void Start()
    {
      
    }
    void Awake()
    {
        if (!Application.isPlaying)
            CacheComponents();
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

        if (Input.GetKeyUp(KeyCode.BackQuote))
        {
            GUIEnabled = !GUIEnabled;
        }

        if(GUIEnabled && Input.GetMouseButton(0))
        {
            RaycastHit hitInfo = new RaycastHit();

            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                hitGameObject = hitInfo.transform.gameObject.name;
                hitGameObjectTransform = hitInfo.transform;
                gameObjectType = hitInfo.transform.gameObject.GetType().Name;
                //scriptObjectName = hitInfo.transform.
                GUIGameObjectShow = true;
            }
        }
    }

    void CacheComponents()
    {
        components = new List<Component>();
        foreach (var component in GetComponents<Component>())
        {
            if (component != this) components.Add(component);
        }
    }

    void OnGUI()
    {
        if(GUIEnabled)
        {

            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(0, 0, w , h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 4 / 100;
            style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(rect, text, style);

            if(GUIGameObjectShow)
            {
                GUI.Label(new Rect(10, 20, 100, 20), hitGameObject);
                GUI.Label(new Rect(10, 40, 100, 50), hitGameObjectTransform.position.ToString());

                // spawnPos = GUI.TextField(new Rect(10, 60, 100, 50),spawnPos.x.ToString());

                GUI.Label(new Rect(10, 60, 200, 20), gameObjectType.ToString());
                

                if (GUI.Button(new Rect(10, 160, 100, 20), "Spawn " + gameObjectsToSpawn[0].name))
                {
                    //Instantiate(gameObjectsToSpawn[0],new Vector3(), Quaternion.identity);
                }

            }

        }

    }

}
