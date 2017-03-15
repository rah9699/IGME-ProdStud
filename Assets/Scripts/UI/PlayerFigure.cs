namespace VRTK.Examples
{
    using UnityEngine;

    public class PlayerFigure : VRTK_InteractableObject
    {
		public GameObject snappedFigure;
		private LineRenderer moveLine;
		private GameObject controller;
		private LevelController levelController;
		public Material lineMaterial;

        public override void StartUsing(GameObject usingObject)
        {
            base.StartUsing(usingObject);
			if (!moveLine) {
				moveLine = gameObject.AddComponent<LineRenderer> ();
				controller = usingObject;
				moveLine.widthMultiplier = 0.01f;
				moveLine.material = lineMaterial;
				//moveLine.useWorldSpace = true;
			}
        }

		protected override void Update(){
			if (controller) {
				convertControllerPosition ();
			}
			base.Update ();
		}

		/// <summary>
		/// Simply draws a line from the gameobject to whatever vector is given
		/// </summary>
		/// <param name="targetPos">endpoint of line</param>
		/// <param name="color">color of line</param>
		/// <returns></returns>
		void drawLine(Vector3 targetPos, Color color){
			moveLine.SetPosition (0, new Vector3(transform.position.x,transform.position.y,transform.position.z));
			moveLine.SetPosition(1, new Vector3(targetPos.x,targetPos.y + 1,targetPos.z));
			moveLine.material.color = color;

		}

		/// <summary>
		/// Scales the controller position to world UI coordinates ans snaps to a grid
		/// </summary>
		/// <param></param>
		/// <returns></returns>
		void convertControllerPosition(){
			Vector3 scaledControllerPosition = new Vector3 (controller.transform.parent.transform.localPosition.x,controller.transform.parent.transform.localPosition.y - 1,controller.transform.parent.transform.localPosition.z);
			scaledControllerPosition = scaledControllerPosition * 50;

			int[] figurePositionHex = HexConst.CoordToHexIndex (scaledControllerPosition);
			Vector3 newPosition = HexConst.HexToWorldCoord (figurePositionHex [0], figurePositionHex [1], figurePositionHex [2]);
			if (levelController.levelGrid.GetHex (figurePositionHex [0], figurePositionHex [1], figurePositionHex [2]) != null) {
				print ("There is a Hex Here");
				drawLine (newPosition * 0.02f, Color.green);
			} else {
				drawLine (newPosition * 0.02f,Color.red);
			}
		}

		public void doMove() {
			//Subtract the player position
			Vector3 scaledFigurePosition = new Vector3 (transform.position.x-transform.parent.position.x,transform.position.y-transform.parent.position.y,transform.position.z-transform.parent.position.z);
			//Scale up the position from the miniature to full scale and reset the y axis
			scaledFigurePosition = scaledFigurePosition * 50;
			scaledFigurePosition.y = scaledFigurePosition.y - 50;
			//Convert the position into hex coordinates and move the player
			int[] figurePositionHex = HexConst.CoordToHexIndex (scaledFigurePosition);
			if (levelController.levelGrid.GetHex(figurePositionHex[0], figurePositionHex[1], figurePositionHex[2]) != null) {
				print ("There is a Hex Here");
				Vector3 newPosition = HexConst.HexToWorldCoord(figurePositionHex[0], figurePositionHex[1], figurePositionHex[2]);
				GameObject.FindGameObjectWithTag ("Player").transform.position = newPosition;
			}else{
				Debug.LogError ("OH NO!! THERE IS NO HEX WHERE THE PLAYER FIGURE IS!!" + "q: "+figurePositionHex[0]+ "r: "+figurePositionHex[1]+ "h: "+figurePositionHex[2]);
			}
		}



        public override void StopUsing(GameObject usingObject)
        {
            base.StopUsing(usingObject);
        }

        protected void Start()
        {
			levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
        }

    }
}