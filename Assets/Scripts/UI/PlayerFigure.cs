namespace VRTK.Examples
{
    using UnityEngine;

    public class PlayerFigure : VRTK_InteractableObject
    {
		public GameObject holographicFigure;
		private GameObject instantiatedHolographicFigure;
		private LineRenderer moveLine;
		public GameObject controller;
		private LevelController levelController;
		public Material lineMaterial;


		/// <summary>
		/// Called when the player grabs the object with a VR controller
		/// </summary>
		/// <param name="usingObject">The controller that grabbed</param>
		/// <returns></returns>
        public override void StartUsing(GameObject usingObject)
        {
            base.StartUsing(usingObject);
			//If there is not already a line, make one and set it up
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
			Vector3 scaledParentPosition = transform.parent.transform.position * 0.02f;
			moveLine.SetPosition (0, new Vector3(transform.localPosition.x+scaledParentPosition.x,transform.localPosition.y+scaledParentPosition.y+1,transform.localPosition.z+scaledParentPosition.z));
			moveLine.SetPosition(1, new Vector3(targetPos.x+transform.parent.position.x,targetPos.y+transform.parent.position.y,targetPos.z+transform.parent.position.z));
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
				drawLine (newPosition * 0.02f, Color.green);
				createDestinationObject (newPosition);
			} else {
				//Destroy (instantiatedHolographicFigure);
				drawLine (newPosition * 0.02f,Color.red);
			}
		}

		/// <summary>
		/// Instantiates a holographic figure of the player on an acceptable tile which can be used to move
		/// </summary>
		/// <param name="spawnPosition">The target position to spawn, should be a valid hex tile</param>
		/// <returns></returns>
		void createDestinationObject(Vector3 spawnPosition){
			Vector3 adjustedPosition = spawnPosition*0.02f;

			adjustedPosition = new Vector3 (adjustedPosition.x+transform.parent.position.x, adjustedPosition.y+transform.parent.position.y, adjustedPosition.z+transform.parent.position.z);

			if (instantiatedHolographicFigure != null) {
				instantiatedHolographicFigure.transform.position = adjustedPosition;
			} else {
				instantiatedHolographicFigure = (GameObject)Instantiate (holographicFigure, adjustedPosition, transform.rotation);
				instantiatedHolographicFigure.transform.parent = this.transform.parent;
				instantiatedHolographicFigure.transform.localScale = instantiatedHolographicFigure.transform.localScale * 0.02f;
				instantiatedHolographicFigure.GetComponent<PlayerFigureMover> ().playerFigure = this;
			}
		}


    public override void StopUsing(GameObject usingObject)
        {
            base.StopUsing(usingObject);
			controller = null;
			Destroy (moveLine);
        }

        protected void Start()
        {
			levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
        }

    }
}