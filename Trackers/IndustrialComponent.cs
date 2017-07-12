using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autelia.Serialization;

namespace CloudCity
{
    [System.Serializable]
    public class IndustrialComponent : ComponentSnap {

        [Header("Tracker functionality setup")]
        [Space(5)]

        [Range(0, 1)]
        [Tooltip("Production multiplier, added to total multiplier of industrials")]
        public float productionMulti;

        [Tooltip("Level at which item is unlocked")]
        public int level = 0;

        [Range(0, 100)]
        [Tooltip("ecoTick recurring cost")]
        public int baseCost;

        public bool usable;

        [Tooltip("Material to change sphere to on grab")]
        public Material industrialMaterial;

        public IndustrialTracker linkedTracker;

        bool checkStop;
        Vector3 oldPosition;

        public delegate void StopCheck();
        public static StopCheck stopCheck;

        protected override void Start()
        {
            base.Start();

            checkStop = true;

            StartCoroutine(WaitForStop());
        }

        protected override void Ungrab()
        {
            base.Ungrab();

            checkStop = true;

            StartCoroutine(WaitForStop());
        }

        protected override void SetSphereMaterial()
        {
            sphereScript.SetSphereMaterial(industrialMaterial);
        
        }

        bool CheckStopped()
        // Returns if object moved since last check
        {
            if (transform.position != oldPosition)
            {
                oldPosition = transform.position;
                return false;
            }
            else return true;
        }

        void UnlinkIfMoving()
        // Method that is attached to delegate to check if stopped. If stopped, component is unlinked
        {
            if(!CheckStopped())
            {
                Unlink();
            }
        }

        void Link(IndustrialTracker tempTracker)
        // Saves industrial tracker link and informs tracker of bonus
        {
            Debug.Log("Tryna link");
            linkedTracker = tempTracker;
            Debug.Log("Linked tracker: " + linkedTracker);
            if (linkedTracker)
                linkedTracker.LinkComponent(this);
            else return;
            stopCheck += UnlinkIfMoving;

            StartCoroutine(StoppedCheck());
        }

        bool TryLink()
        {
            Debug.Log(U.FindNearestBuildings(transform.position, radius).Count);
            List<IndustrialTracker> surroundingIndustrials = U.ReturnIndustrialTrackers(U.FindNearestBuildings(transform.position, radius));
            Debug.Log(surroundingIndustrials.Count + "Industrials found");
            if (surroundingIndustrials.Count >= 1)
            {
                Link(surroundingIndustrials[0]);
                return true;
            }
            else return false;
        }

        void Unlink()
        // Unlinks component from industrial tracker and restarts linking process
        {
            stopCheck -= UnlinkIfMoving;
            linkedTracker.UnlinkComponent(this);

            checkStop = true;

            StartCoroutine(WaitForStop());
        }

        IEnumerator WaitForStop()
        // Checks position until stopped, links component once stopped.
        {
            yield return null;
            while(checkStop)
            {
                if(CheckStopped())
                {
                    if (TryLink())
                        checkStop = false;
                }
                yield return new WaitForSeconds(1f);
            }
        }

        static IEnumerator StoppedCheck()
        // Runs delegate for checking that components haven't moved.
        {
            while(true)
            {
                if(stopCheck != null)
                {
                    stopCheck();
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
}