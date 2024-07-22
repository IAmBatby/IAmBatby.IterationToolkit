using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IterationToolkit
{
    public class ObjectiveBehaviour : MonoBehaviour
    {
        public Objective Objective { get; private set; }

        public UnityEvent OnObjectiveInactive;
        public UnityEvent OnObjectiveActive;
        public UnityEvent OnObjectiveComplete;
        public UnityEvent OnObjectiveFailed;

        public void StartTrackingObjective(Objective objective)
        {
            if (objective != null)
                StopTrackingObjective();

            Objective = objective;
            Objective.onStateChanged.AddListener(OnObjectiveStateChanged);
        }

        public void StopTrackingObjective()
        {
            if (Objective != null)
            {
                Objective.onStateChanged.RemoveListener(OnObjectiveStateChanged);
                Objective = null;
            }
        }

        public void ChangeObjectiveState(string newObjectiveState)
        {
            if (Objective != null)
                if (Enum.TryParse(typeof(ObjectiveState), newObjectiveState, out object resultState))
                    Objective.ChangeObjectiveState((ObjectiveState)resultState);
        }

        private void OnObjectiveStateChanged(ObjectiveState objectiveState)
        {
            if (objectiveState == ObjectiveState.Inactive)
                OnObjectiveInactive.Invoke();
            else if (objectiveState == ObjectiveState.Active)
                OnObjectiveActive.Invoke();
            else if (objectiveState != ObjectiveState.Complete)
                OnObjectiveComplete.Invoke();
            else if (objectiveState == ObjectiveState.Failed)
                OnObjectiveFailed.Invoke();
        }
    }
}
