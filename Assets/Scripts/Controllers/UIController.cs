﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the player's UI
/// </summary>
public class UIController : MonoBehaviour {
    //Assign the prefab in editor to spawn as minimap hex object
    public GameObject uiGridPrefab;
    public GameObject playerFigurePrefab;
    //uiGrid holds all UICells which relate to each hex on the map
    public HexGrid<UICell> uiGrid = new HexGrid<UICell>();
    private GameObject playerFigure;
    int[] figurePositionHex;
    LevelController levelController;

    public float uiScale = 0.02f; //the scale of the minimap compared to the world map.

    //Materials for hexes
    public Material defaultHexMaterial;
    public Material possibleMoveMat1;
    public Material possibleMoveMat2;
    public Material possibleMoveMat3;
    public Material highlightMaterial;

    void Start() {
        levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
        
        StartCoroutine(LoadTileCheck()); //Coroutine that waits for all cells to become ready before continuing.
    }

    //Helper coroutine for Start
    //Makes sure all tiles are completely loaded before scaling, repositioning, and setting visibility.
    //In absence of this fix, approx. 16 tiles and the player figure load before the rest, and are scaled down+set invisible while the rest remain at full size.
    IEnumerator LoadTileCheck()
    {
        spawnPlayerFigure();
        yield return new WaitUntil(() => levelController.cellsReady>=236); //wait until all cells are ready
        scaleandReposition();//properly scales down the UI grid.
        setVisibility(false);
    }


    /// <summary>
    /// Allow getting/setting for the UI grid using [q,r,h]
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <param name="h">height</param>
    public UICell this[int q, int r, int h] {
        get { return this.uiGrid[q, r, h]; }
        set { this.uiGrid[q, r, h] = value; }
    }

    /// <summary>
    /// Called from LevelController by each hex, instantiates an object at corresponding position
    /// </summary>
    /// <param name="cell"></param>
    public void addCellToUIMap(UICell cell) {
        //Create a new GameObject to represent a location in the world
        GameObject newHologramCell = (GameObject)Instantiate(uiGridPrefab, cell.centerPos, transform.rotation);
        cell.setGameObject(newHologramCell);
        newHologramCell.GetComponent<UICellObj>().parent = cell;
        newHologramCell.GetComponent<UICellObj>().q = cell.q;
        newHologramCell.GetComponent<UICellObj>().r = cell.r;
        newHologramCell.GetComponent<UICellObj>().h = cell.h;
        //Put the cell into the UIGrid
        uiGrid[cell.q, cell.r, cell.h] = cell;
        //Set the game object's parent transform for scaling/rotation purposes.
        newHologramCell.transform.SetParent(gameObject.transform);
    }

    //Show valid moves for the player's move command.
    public void ShowValidMoves(List<List<PathCell>> hexCells) {
        Material matToUse;
        for (int i = 0; i < hexCells.Count; i++) {
            foreach (PathCell coords in hexCells[i]) {
                if (i == 0) {
                    matToUse = possibleMoveMat1;
                } else if (i == 1) {
                    matToUse = possibleMoveMat2;
                } else {
                    matToUse = possibleMoveMat3;
                }
                uiGrid[coords.q, coords.r, coords.h].gameObject.GetComponent<Renderer>().material = matToUse;
            }
        }
    }

    /// <summary>
    /// Displays the top-down radius around an area.
    /// Currently used for ability ranges.
    /// Will likely abstract some of this at some point.
    /// </summary>
    public void ShowValidTopDownRadius(int q, int r, int h, int radius, bool includeOrigin = false, TargetingMaterial matParam = TargetingMaterial.COSTS_ONE) {
        Material matToUse;
        switch (matParam) {
            case TargetingMaterial.COSTS_ONE: matToUse = possibleMoveMat1; break;
            case TargetingMaterial.COSTS_TWO: matToUse = possibleMoveMat2; break;
            case TargetingMaterial.COSTS_THREE: matToUse = possibleMoveMat3; break;
            case TargetingMaterial.TARGETED_ZONE: matToUse = highlightMaterial; break;
            default: matToUse = possibleMoveMat1; break;
        }
        UICell[] topDown = uiGrid.TopDownRadius(q, r, h, radius, includeOrigin);
        foreach (UICell rangeTile in topDown) {
            rangeTile.gameObject.GetComponent<Renderer>().material = matToUse;
        }
    }

    public void ClearCells() {
        foreach (UICell cell in uiGrid) {
            cell.gameObject.GetComponent<Renderer>().material = defaultHexMaterial;
        }
    }

    /// <summary>
    /// Create player figure if there isnt already one
    /// </summary>
    void spawnPlayerFigure() {
        if (!playerFigure) {
            playerFigure = (GameObject)Instantiate(playerFigurePrefab, transform.position, transform.rotation);
            playerFigure.transform.SetParent(gameObject.transform);
            //Subtract the player position
            figurePositionHex = HexConst.CoordToHexIndex(new Vector3(playerFigure.transform.position.x - transform.parent.position.x, playerFigure.transform.position.y - transform.parent.position.y, playerFigure.transform.position.z - transform.parent.position.z));
        } else {
            playerFigure.transform.position = this.transform.position;
        }
    }


    /// <summary>
    /// Called by a canvas button when minimap is open, Moves the player to the hex corresponding to the player figure
    /// </summary>
    public void doMove() {
        //Subtract the player position
        Vector3 scaledFigurePosition = new Vector3(playerFigure.transform.position.x - transform.parent.position.x, playerFigure.transform.position.y - transform.parent.position.y, playerFigure.transform.position.z - transform.parent.position.z);
        //Scale up the position from the miniature to full scale and reset the y axis
        scaledFigurePosition = scaledFigurePosition * 50;
        scaledFigurePosition.y = scaledFigurePosition.y - 50;
        //Convert the position into hex coordinates and move the player
        int[] figurePositionHex = HexConst.CoordToHexIndex(scaledFigurePosition);
        if (levelController.levelGrid.GetHex(figurePositionHex[0], figurePositionHex[1], figurePositionHex[2]) != null) {
            print("There is a Hex Here");
            Vector3 newPosition = HexConst.HexToWorldCoord(figurePositionHex[0], figurePositionHex[1], figurePositionHex[2]);
            GameObject.FindGameObjectWithTag("Player").transform.position = newPosition;
            ClearCells();
        } else {
            Debug.LogError("OH NO!! THERE IS NO HEX WHERE THE PLAYER FIGURE IS!!" + "q: " + figurePositionHex[0] + "r: " + figurePositionHex[1] + "h: " + figurePositionHex[2]);
        }
    }

    /// <summary>
    /// This is temporary to test different scales and positions of the minimap
    /// </summary>
    void Update() {
        //if (Input.GetKeyDown("up")) {
        //    //scaleandReposition (); //commented out because we only need to do this once at the beginning.
        //}
        //if (Input.GetKeyUp("b")) {
        //    setVisibility(true);
        //}
        //if (Input.GetKeyUp(KeyCode.Escape)) {
        //    setVisibility(false);
        //}
        //if (Input.GetKeyUp("v")) {
        //    doMove();
        //}
    }

    /// <summary>
    /// Rescale and move
    /// </summary>
    void scaleandReposition() {
        Debug.Log("Scaling down.");
        transform.localScale = transform.localScale * uiScale;
        transform.position = new Vector3(0, 1, 0);
        spawnPlayerFigure();
    }

    /// <summary>
    /// Enable/Disable the renderer this will eventually become on/off for the minimap
    /// </summary>
    public void setVisibility(bool visible) {
        if (visible) {
            foreach (MeshRenderer renderer in transform.GetComponentsInChildren<MeshRenderer>()) {
                renderer.enabled = true;
            }
            foreach (Collider col in transform.GetComponentsInChildren<Collider>()) {
                col.enabled = true;
            }

        } else {
            foreach (MeshRenderer renderer in transform.GetComponentsInChildren<MeshRenderer>()) {
                renderer.enabled = false;
            }
            foreach (Collider col in transform.GetComponentsInChildren<Collider>()) {
                col.enabled = false;
            }
        }
    }

}
