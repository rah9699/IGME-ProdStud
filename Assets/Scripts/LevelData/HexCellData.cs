﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A data object that stores information about a hex cell.
/// Things like: location, real-world coordinates, what characters occupy the cell, the cell's gameobject, items on top of the cell, etc.
/// This object type will expand over time.
/// All HexCellData objects are built and populated at runtime.
/// </summary>
public class HexCellData : HexCell {
    //Reference to the cell's game object
    public GameObject hexCellObject;
    //Reference to displayed prefabs (things that will show up on the minimap)
    public List<GameObject> displayedPrefabs = new List<GameObject>();

    //Whether or not the cell contains a goal object and a reference if it does
    public bool hasGoal;
    public GameObject goal;

    //The list of items in this cell
    public List<GameObject> items=new List<GameObject>();

    //The list of props in this cell
    public List<GameObject> props = new List<GameObject>();

    //Whether or not the cell contains items or not.
    public bool HasItems {
        get { return items.Count>0; }
    }

    //Enemy presence is in PathCell. This is because of its greater relevance to pathing.

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <param name="h">height</param>
    /// <param name="hexCellObject">Hex GameObject</param>
	public HexCellData(int q, int r, int h, GameObject hexCellObject) : base(q, r, h) {
        this.hexCellObject = hexCellObject;
    }
}
