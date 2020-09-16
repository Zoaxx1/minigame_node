using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tale
{
    public class Rotations
    {
        private List<Vector3> _2directionLeft = new List<Vector3>() { new Vector3(0, 0, 45), new Vector3(0, 0, 135) };
        private List<Vector3> _2directionRight = new List<Vector3>() { new Vector3(0, 0, -45), new Vector3(0, 0, -135) };
        private List<Vector3> _2directionUp = new List<Vector3>() { new Vector3(0, 0, 45), new Vector3(0, 0, -45) };
        private List<Vector3> _2directionDown = new List<Vector3>() { new Vector3(0, 0, 135), new Vector3(0, 0, -135) };

        private List<Vector3> _4direction = new List<Vector3>() { new Vector3(0, 0, 45), new Vector3(0, 0, 135),
                                                                  new Vector3(0, 0, -135), new Vector3(0, 0, -45)};

        private List<Vector3> _6direction = new List<Vector3>() { new Vector3(0, 0, 0), new Vector3(0, 0, 45),
                                                                  new Vector3(0, 0, 135), new Vector3(0, 0, 180),
                                                                  new Vector3(0, 0, -135), new Vector3(0, 0, -45)};

        private List<Vector3> _8direction = new List<Vector3>() { new Vector3(0, 0, 0), new Vector3(0, 0, 45),
                                                                  new Vector3(0, 0, 90), new Vector3(0, 0, 135),
                                                                  new Vector3(0, 0, 180), new Vector3(0, 0, -135),
                                                                  new Vector3(0, 0, -90), new Vector3(0, 0, -45)};

        public List<Vector3> _2DLeft
        {
            get { return _2directionLeft; }
        }
        public List<Vector3> _2DRight
        {
            get { return _2directionRight; }
        }
        public List<Vector3> _2DUp
        {
            get { return _2directionUp; }
        }
        public List<Vector3> _2DDown
        {
            get { return _2directionDown; }
        }
        public List<Vector3> _4D
        {
            get { return _4direction; }
        }
        public List<Vector3> _6D
        {
            get { return _6direction; }
        }
        public List<Vector3> _8D
        {
            get { return _8direction; }
        }
    }

    public class MG5Node : MonoBehaviour
    {   
        public enum Directions
        {
            _2DirectionsLeft,
            _2DirectionsRight,
            _2DirectionsUp,
            _2DirectionsDown,
            _4Directions,
            _6Directions,
            _8Directions,
            Personalized
        }

        #region Properties

        private const float actionDetectionTolerance = .5f;

        [Tooltip("Select the Rotation you like")]
        [SerializeField]
        private Directions selectRotation;
        [Space (10)]
        [SerializeField]
        private List<Vector3> customeRotation;

        public Rotations allRotations;
        private List<Vector3> rotations;
        private Touch touch;
        private bool startRotation;
        private int i;

        #endregion

        #region MonoBehaviour Functions

        private void Start()
        {
            allRotations = new Rotations();
            rotations = new List<Vector3>();
            startRotation = false;

            SetRotations();
            StartRandomRotation();
        }

        private void Update()
        {
            if (startRotation)
            {
                PlayerInput();
            }                                
        }

#endregion

        #region Gameplay Functions
        private void PlayerInput()
        {
            if (Application.isEditor)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (ComparationFunction(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position))
                    {
                        RotateNode();
                    }
                }
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        if (ComparationFunction(Camera.main.ScreenToWorldPoint(touch.position) - transform.position))
                        {
                            RotateNode();
                        }
                    }
                }
            }
        }


        //Ants get the Rotation from the Nodes
        public Vector3 GetRotation()
        {
            return rotations[i];
        }

#endregion

        #region Support Functions

        public void StartRotation(bool value)
        {
            startRotation = value; 
        }

        public void SetRotations()
        {
            switch (selectRotation)
            {
                case Directions._2DirectionsLeft:
                    AddRotations(allRotations._2DLeft);
                    break;
                case Directions._2DirectionsRight:
                    AddRotations(allRotations._2DRight);
                    break;
                case Directions._2DirectionsUp:
                    AddRotations(allRotations._2DUp);
                    break;
                case Directions._2DirectionsDown:
                    AddRotations(allRotations._2DDown);
                    break;
                case Directions._4Directions:
                    AddRotations(allRotations._4D);
                    break;
                case Directions._6Directions:
                    AddRotations(allRotations._6D);
                    break;
                case Directions._8Directions:
                    AddRotations(allRotations._8D);
                    break;
                case Directions.Personalized:
                    AddRotations(customeRotation);
                    break;
            }
        }

        public void StartRandomRotation()
        {
            i = Random.Range(0, rotations.Count);

            transform.rotation = Quaternion.Euler(rotations[i]);
        }

        private void AddRotations(List<Vector3> addThisRotation)
        {
            foreach (Vector3 addRotation in addThisRotation)
            {
                rotations.Add(addRotation);
            }
        }

        private bool ComparationFunction(Vector3 comparation)
        {
            return comparation.x < actionDetectionTolerance && comparation.x > -actionDetectionTolerance && comparation.y < actionDetectionTolerance && comparation.y > -actionDetectionTolerance;
        }

        private void RotateNode()
        {
            i++;

            if (i >= rotations.Count)
            {
                i = 0;
            }

            transform.rotation = Quaternion.Euler(rotations[i]);
        }

#endregion
    }
}

