using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Former base class for objectives, now a subclass of Objective.
/// Other classes can inherit this to implement different objective functionality
/// </summary>
<<<<<<< HEAD
public class Goal : Objective
{
=======
public class Goal : MonoBehaviour {
    private bool addedSelf = false;

    //Has the player reached this goal?
    public bool reached = false;
    //Coords
    public int q, r, h;
    //UI Prefab
    public GameObject uiPrefab;

    /// <summary>
    /// Unity's start function
    /// </summary>
    void Start() {
        //get the goal's model height so we can properly determine the cell it is on
        float modelHeight = gameObject.GetComponent<Renderer>().bounds.size.y;
        //Get the hex index for this hex cell.  Pass in the transform.
        int[] thisHexIndex = HexConst.CoordToHexIndex(transform.position + new Vector3(0, -0.5f * modelHeight, 0));
        q = thisHexIndex[0];
        r = thisHexIndex[1];
        h = thisHexIndex[2];
    }

>>>>>>> danielsbranch/master
    /// <summary>
    /// Unity's update function
    /// </summary>
    void Update() {
        //Can't add self in the start() function because it is creation order dependant on hex cells
        if (!addedSelf) {
            //Save the grid game object
            levelController = GameObject.Find("LevelController").GetComponent("LevelController") as LevelController;
            //Tell the level controller to initialize this hex cell
            levelController.AddGoal(q, r, h, gameObject);
            levelController.AddDisplayedObject(q, r, h, this.uiPrefab);
            addedSelf = true;
        }
    }

    public virtual void Accomplished() {
<<<<<<< HEAD
        levelController[q, r, h].hasGoal = false;
=======
        //Clear the ui prefab
        GameObject.Find("LevelController").GetComponent<LevelController>()[q, r, h].displayedPrefabs = new List<GameObject>();
        GameObject.Find("UIController").GetComponent<UIController>()[q, r, h].forceGoalMaterial = false;
>>>>>>> danielsbranch/master
        gameObject.SetActive(false);

    }
}
