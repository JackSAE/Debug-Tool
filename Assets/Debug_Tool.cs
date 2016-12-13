using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;


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

public class Debug_Tool : MonoBehaviour {

    
    int w = Screen.width, h = Screen.height;


    float deltaTime = 0.0f;

    public bool GUIEnabled = false;
    public bool GUIGameObjectShow = false;

    public string hitGameObjectName;
    MonoBehaviour[] scriptsOnTarget;
    public Transform hitGameObjectTransform;

    public string objectValues;
    public string scriptObjectName;
    public string gameObjectTransformEdit;
    public string gameObjectType;

    public Vector3 spawnPos;

    //getting Scripts from gameObjects
    public List<string> gameObjectList = new List<string>();
    public List<string> gamevariablesList = new List<string>();

    public GameObject[] gameObjectsToSpawn;

    FieldInfo[] myFieldInfo;
    Type classType;
    [SerializeField] string className;


    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

        //Toggle Debug Menu
        if (Input.GetKeyUp(KeyCode.BackQuote))
        {
            GUIEnabled = !GUIEnabled;
        }

        #region RayCasting 
        if (GUIEnabled && Input.GetMouseButton(0))
        {
            RaycastHit hitInfo = new RaycastHit();

            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                hitGameObjectName = hitInfo.transform.gameObject.name;
                hitGameObjectTransform = hitInfo.transform;
                gameObjectType = hitInfo.transform.gameObject.GetType().Name;
                //scriptObjectName = hitInfo.transform.
                GUIGameObjectShow = true;
                scriptsOnTarget = hitInfo.transform.gameObject.GetComponents<MonoBehaviour>();

            
            }
        }
        #endregion
    }

    #region  Turning a string to vector -> Used to move a gameObject around
    public static Vector3 StringToVector3(string sToVector)
    {
        // Remove the parentheses
        if (sToVector.StartsWith("(") && sToVector.EndsWith(")"))
        {
            sToVector = sToVector.Substring(1, sToVector.Length - 2);
        }

        // split the items
        string[] sToArray = sToVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sToArray[0]),
            float.Parse(sToArray[1]),
            float.Parse(sToArray[2]));

        return result; //Feels Good Man
    }
    #endregion


    void OnGUI()
    {
        //Checks if "~" has been pressed
        if(GUIEnabled)
        {

            GUIStyle style = new GUIStyle();

            #region FPS GUI 
            Rect rect = new Rect(0, 0, w , h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 4 / 100;
            style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(rect, text, style);
            #endregion

            //Checks if user has clicked on an object
            if(GUIGameObjectShow)
            {
                #region Getting Game Objects Name
                GUI.Label(new Rect(10, 20, 100, 20), hitGameObjectName);
                #endregion

                #region Get and Set GameObject Transform
                gameObjectTransformEdit = hitGameObjectTransform.position.ToString();

                gameObjectTransformEdit = GUI.TextField(new Rect(10, 40, 100, 50), hitGameObjectTransform.position.ToString());

                hitGameObjectTransform.position = StringToVector3(gameObjectTransformEdit);
                #endregion

                #region Getting Game Object Type
                GUI.Label(new Rect(10, 60, 200, 20), gameObjectType.ToString());
                #endregion

                #region Adds a game Objects Name + script(s) to a list - Names are currently unique
                foreach (MonoBehaviour mBScript in scriptsOnTarget)
                {
                    if (!gameObjectList.Contains( hitGameObjectName + mBScript.GetType().Name))
                    {
                        gameObjectList.Add(hitGameObjectName + mBScript.GetType().Name);

                        //Set type to classType
                        classType = mBScript.GetType();

                        //Get the names of the Variables
                        foreach ( var prop in classType.GetFields())
                            gamevariablesList.Add(hitGameObjectName + prop.Name);

                        #region Activator.CreateInstance Work in Progress
                        /*
                        //Get class Name
                        className = classType.Name;

                        //Get the name space of the class name - needed to create an instance
                        var nameSpace = classType.Namespace;

                        //Make an instance 
                        var myObj = Activator.CreateInstance(nameSpace, className);

                        foreach (var prop in classType.GetFields())
                        {
                            gameVariablesValuesList.Add(prop.Name + prop.GetValue(myObj));
                        }
                        */
                        #endregion

                    }
                    else
                    {
                        Debug.Log(hitGameObjectName + mBScript.name);
                        
                    }

                }
                #endregion



                /*
                ########################
                ## List of Scripts: -> numOfScripts
                ## (Foldout)>NameOfAllScripts in a DropDown
                ## (foldout>> Click on script all varaibles become a drop down
                ## All Variables that are public can have their values edited
                ########################
                */

                #region Spawning GameObjects
                if (gameObjectsToSpawn[0] != null)
                {
                    if (GUI.Button(new Rect(10, 160, 100, 20), "Spawn " + gameObjectsToSpawn[0].name))
                    {
                        Instantiate(gameObjectsToSpawn[0], Input.mousePosition, Quaternion.identity);
                    }
                }
                #endregion

            }

        }

    }

}
