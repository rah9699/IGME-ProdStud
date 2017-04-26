namespace VRTK.Examples
{
    using UnityEngine;

    public class PlayerFigure : VRTK_InteractableObject
    {
		public GameObject holographicFigure;
		private LineRenderer moveLine;
		public GameObject controller;
		private LevelController levelController;
		public Material lineMaterial;

		public Player player;

		/// <summary>
		/// Called when the player grabs the object with a VR controller
		/// </summary>
		/// <param name="usingObject">The controller that grabbed</param>
		/// <returns></returns>
        public override void StartUsing(GameObject usingObject)
        {
            base.StartUsing(usingObject);
			//If there is not already a line, make one and set it up
			if (!controller) {
				moveLine = gameObject.AddComponent<LineRenderer> ();
				controller = usingObject;
				moveLine.widthMultiplier = 0.01f;
				moveLine.material = lineMaterial;
				//moveLine.useWorldSpace = true;
			}
        }

		protected override void Update(){
			if (controller != null) {
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

			RaycastHit hit;
			if (Physics.Raycast(scaledControllerPosition, new Vector3(0, -100, 0), out hit)) {
				//Get the UI hex cell the player is looking at
				player.VRHitObj = hit.transform.gameObject.GetComponent<UICellObj>() as UICellObj;
			}

			Vector3 newPosition = HexConst.HexToWorldCoord (player.VRHitObj.q, player.VRHitObj.r, player.VRHitObj.h);
			if (levelController[player.VRHitObj.q, player.VRHitObj.r, player.VRHitObj.h] != null) {
				drawLine (newPosition * 0.02f, Color.green);
			}
		}

    	public override void StopUsing(GameObject usingObject)
        {
            base.StopUsing(usingObject);
			controller = null;
			Destroy (moveLine);
			player.doVRClick = true;
        }

        protected void Start()
        {
			levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
			player = GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<Player> ();
        }

    }
}