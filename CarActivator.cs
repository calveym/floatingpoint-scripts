namespace VRTK.Examples
{
    using UnityEngine;

    public class CarActivator : VRTK_InteractableObject
    {
        public Material highlight;
        public GameObject carBody;
        public GameObject w1;
        public GameObject w2;
        public GameObject w3;
        public GameObject w4;
        public GameObject steeringWheel;
        public GameObject targetArea;
        public GameObject mainCamera;
        public GameObject carLeftController;
        public GameObject carRightController;
        public GameObject carCamera;
		public CarController carController;
		public Transform centerOfGravity;

        Material originalBody;
        Material originalWheel;
        Material originalSteeringWheel;
        GameObject[] wheels;
        bool hoverStatus;
        float hoverSeconds;

        public override void StartUsing(GameObject usingObject)
        {
            base.StartUsing(usingObject);
            carBody.GetComponent<Renderer>().material = highlight;
            steeringWheel.GetComponent<Renderer>().material = highlight;
            foreach (GameObject wheel in wheels)
            {
                wheel.GetComponent<Renderer>().material = highlight;
            }

            hoverStatus = true;
        }

        public override void StopUsing(GameObject usingObject)
        {
            base.StopUsing(usingObject);
            carBody.GetComponent<Renderer>().material = originalBody;
            steeringWheel.GetComponent<Renderer>().material = originalSteeringWheel;
            foreach (GameObject wheel in wheels)
            {
                wheel.GetComponent<Renderer>().material = originalWheel;
            }
            
            if(hoverSeconds > 1)
            {
                carController.enabled = true;
                carCamera.SetActive(true);
                carLeftController.SetActive(true);
                carRightController.SetActive(true);
                targetArea.SetActive(false);
                mainCamera.SetActive(false);
            }

            hoverStatus = false;
        }

        protected override void Start()
        {
            carCamera.SetActive(false);
            base.Start();
			gameObject.GetComponent<Rigidbody>().centerOfMass = centerOfGravity.localPosition;
            originalBody = carBody.GetComponent<Renderer>().material;
            originalSteeringWheel = steeringWheel.GetComponent<Renderer>().material;
            originalWheel = w1.GetComponent<Renderer>().material;
			carController.enabled = false;

            wheels = new GameObject[4] { w1, w2, w3, w4 };
        }

        protected override void Update()
        {
            if (hoverStatus)
            {
                hoverSeconds += Time.deltaTime;
            } else
            {
                hoverSeconds = 0;
            }
        }


    }
}