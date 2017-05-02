﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A state-based object storing info about displaying individual hexes in the minimap
/// </summary>
public class UICell : HexCell {
    public GameObject gameObject;

<<<<<<< HEAD
	public AIController aiController = null;
    public LevelController levelController = null;
=======
    public bool forceGoalMaterial = false;
>>>>>>> danielsbranch/master

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <param name="h">height</param>
    public UICell(int q, int r, int h) : base(q, r, h) { }

    /// <summary>
    /// Set the height scaling of the UI cell object
    /// </summary>
    /// <param name="scale">scale</param>
    public void setModelScale(float scale) {
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, scale, gameObject.transform.localScale.z);
        gameObject.transform.position = gameObject.transform.position + new Vector3(0, -0.5f * gameObject.GetComponent<Renderer>().bounds.size.y, 0);
    }

    /// <summary>
    /// Store the GameObject this UI Cell represents
    /// </summary>
    /// <param name="cell">game object</param>
    public void setGameObject(GameObject cell) {
        gameObject = cell;
    }

<<<<<<< HEAD
	///<summary>
	/// Set the display status of the UICell and update minimap presence
	/// </summary>
	public void Display(bool display) {
		if (display) {
			MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer> ();
			Collider collider = this.gameObject.GetComponent<Collider> ();
			renderer.enabled = true;
			collider.enabled = true;
            aiController = GameObject.Find("AIController").GetComponent<AIController>();
            levelController = GameObject.Find("LevelController").GetComponent<LevelController>();

            GameObject thisMonster = aiController.GetEnemy(q, r, h);
            if (thisMonster != null)
            {
                gameObject.GetComponent<UICellObj>().AddMinimapObject(thisMonster,MinimapObjectType.ENEMY);
            }

            GameObject thisGoal = levelController.GetGoal(q, r, h);
            if (thisGoal != null)
            {
                gameObject.GetComponent<UICellObj>().AddMinimapObject(thisGoal, MinimapObjectType.GOAL);
            }

            List<GameObject> thisItems = levelController.GetItemPrefabs(q, r, h);
            if (thisItems != null)
            {
                gameObject.GetComponent<UICellObj>().AddMinimapObjects(thisItems, MinimapObjectType.ITEM);
            }

            List<GameObject> thisProps = levelController.GetProps(q, r, h);
            if (thisProps != null)
            {
                gameObject.GetComponent<UICellObj>().AddMinimapObjects(thisProps, MinimapObjectType.PROP);
            }

        } else {
			this.gameObject.GetComponent<MeshRenderer> ().enabled = false;
			this.gameObject.GetComponent<Collider> ().enabled = false;

			gameObject.GetComponent<UICellObj> ().HideContents();
		}
	}
=======
    ///<summary>
    /// Set the display status of the UICell
    /// </summary>
    public void Display(bool display) {
        if (display) {
            MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
            Collider collider = this.gameObject.GetComponent<Collider>();
            renderer.enabled = true;
            collider.enabled = true;
            gameObject.GetComponent<UICellObj>().AddMinimapObjects(GameObject.Find("UIController").GetComponent<UIController>().GetMinimapPrefabs(q, r, h));

        } else {
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.GetComponent<Collider>().enabled = false;

            gameObject.GetComponent<UICellObj>().RemoveMinimapObjects();
        }
    }

    //Only change material if 
    public void SetMaterial(Material mat, bool force) {
        if (!force && forceGoalMaterial) {
            gameObject.GetComponent<Renderer>().material = GameObject.Find("UIController").GetComponent<UIController>().objectiveMaterial;
        } else {
            gameObject.GetComponent<Renderer>().material = mat;
        }
        //if (!force && gameObject.GetComponent<UICellObj>().minimapObjects.Count >= 0 && !GameObject.Find("AIController").GetComponent<AIController>()[q, r, h].hasEnemy) {
        //    gameObject.GetComponent<Renderer>().material = GameObject.Find("UIController").GetComponent<UIController>().objectiveMaterial;
        //} else {
        //    gameObject.GetComponent<Renderer>().material = mat;
        //}
    }
>>>>>>> danielsbranch/master

}
