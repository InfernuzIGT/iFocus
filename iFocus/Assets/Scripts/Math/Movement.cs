using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pyros
{
    public class Movement : MonoBehaviour
    {
        public EasingFunction.Ease type;
        private EasingFunction.Function function;

        [SerializeField]
        private bool loop = false;

        [SerializeField]
        private bool rewindOnEnd = false;

        [SerializeField]
        private float elapsedTime = 0;

        [SerializeField]
        private float movementDuration = 0;

        [SerializeField]
        private Transform[] positions = null;
        private Vector3[] auxPositions = null;

        [Header("Clamp")]
        [SerializeField]
        private bool xAxis = false;

        [SerializeField]
        private bool yAxis = false;

        [SerializeField]
        private bool zAxis = false;

        [Space]

        public UnityEvent OnStart;
        public UnityEvent OnNewPosition;
        public UnityEvent OnEnd;
        private UnityAction action;

        private void OnValidate()
        {
            auxPositions = new Vector3[positions.Length];

            for (int i = 0; i < positions.Length; i++)
                auxPositions[i] = positions[i].position;
        }

        [ContextMenu("StartMoving")]
        public void StartMove()
        {
            StartCoroutine(Move(auxPositions, movementDuration));
        }

        IEnumerator Move(Vector3[] positions, float duration)
        {
            action += Messange;
            OnStart.AddListener(action);

            OnStart.Invoke();
            

            function = EasingFunction.GetEasingFunction(type);

            elapsedTime = 0f;

            float x, y, z, distancePercentage, durationPercentage;
            float totalDistance = GetTotalDistance(positions);

            do
            {
                OnStart.Invoke();

                transform.position = positions[0];

                for (int i = 0; i < positions.Length - 1; i++)
                {
                    x = positions[i].x;
                    y = positions[i].y;
                    z = positions[i].z;

                    distancePercentage = Vector3.Distance(positions[i], positions[i + 1]) / totalDistance;
                    durationPercentage = duration * distancePercentage;

                    while (elapsedTime < durationPercentage)
                    {
                        x = function(positions[i].x, positions[i + 1].x, (elapsedTime / durationPercentage));
                        y = function(positions[i].y, positions[i + 1].y, (elapsedTime / durationPercentage));
                        z = function(positions[i].z, positions[i + 1].z, (elapsedTime / durationPercentage));

                        elapsedTime += Time.deltaTime;

                        if (xAxis)
                            x = transform.position.x;
                        if (yAxis)
                            y = transform.position.y;
                        if (zAxis)
                            z = transform.position.z;

                        transform.position = new Vector3(x, y, z);

                        yield return null;
                    }

                    if (!xAxis && !yAxis && !zAxis)
                        transform.position = positions[i + 1];

                    elapsedTime = 0f;

                    if (i < positions.Length - 2)
                        OnNewPosition.Invoke();
                }

                if (!rewindOnEnd)
                    OnEnd.Invoke();
                else
                {
                    for (int i = positions.Length - 1 ; i > 0; i--)
                    {
                        x = positions[i].x;
                        y = positions[i].y;
                        z = positions[i].z;

                        distancePercentage = Vector3.Distance(positions[i], positions[i - 1]) / totalDistance;
                        durationPercentage = duration * distancePercentage;

                        while (elapsedTime < durationPercentage)
                        {
                            x = function(positions[i].x, positions[i - 1].x, (elapsedTime / durationPercentage));
                            y = function(positions[i].y, positions[i - 1].y, (elapsedTime / durationPercentage));
                            z = function(positions[i].z, positions[i - 1].z, (elapsedTime / durationPercentage));

                            elapsedTime += Time.deltaTime;

                            if (xAxis)
                                x = transform.position.x;
                            if (yAxis)
                                y = transform.position.y;
                            if (zAxis)
                                z = transform.position.z;

                            transform.position = new Vector3(x, y, z);

                            yield return null;
                        }

                        if (!xAxis && !yAxis && !zAxis)
                            transform.position = positions[i - 1];

                        elapsedTime = 0f;

                        if (i > positions.Length + 2)
                            OnNewPosition.Invoke();
                    }

                    OnEnd.Invoke();

                }
            }
            while (loop);
        }

        public void Messange()
        {
            Mathf.Abs(5f);
        }

        public float GetTotalDistance(Vector3[] positions)
        {
            float totalDistance = 0;

            for (int i = 0; i < positions.Length - 1; i++)
                totalDistance += Vector3.Distance(positions[i], positions[i + 1]);

            return totalDistance;
        }

    }
}
