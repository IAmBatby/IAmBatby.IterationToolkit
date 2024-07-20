using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectiveState { Inactive, Active, Complete, Failed }

[CreateAssetMenu(fileName = "Objective", menuName = "IterationToolkit/Objective", order = 1)]
public class Objective : ScriptableObject
{

    public string objectiveName;
    public string objectiveDescription;
    public ObjectiveState CurrentObjectiveState { get; private set; } = ObjectiveState.Inactive;

    public ExtendedEvent<ObjectiveState> onStateChanged = new ExtendedEvent<ObjectiveState>();

    public static Objective InitializeObjective(Objective objectiveData)
    {
        Objective objective = objectiveData.Copy();

        return (objective);
    }

    public void ChangeObjectiveState(ObjectiveState newObjectiveState)
    {
        CurrentObjectiveState = newObjectiveState;

        switch (CurrentObjectiveState)
        {
            case ObjectiveState.Inactive:
                OnObjectiveInactive();
                break;
            case ObjectiveState.Active:
                OnObjectiveActive();
                break;
            case ObjectiveState.Complete:
                OnObjectiveComplete();
                break;
            case ObjectiveState.Failed:
                OnObjectiveFail();
                break;
        }

        onStateChanged.Invoke(CurrentObjectiveState);
    }

    protected virtual void OnObjectiveInactive()
    {

    }

    protected virtual void OnObjectiveActive()
    {

    }

    protected virtual void OnObjectiveComplete()
    {

    }

    protected virtual void OnObjectiveFail()
    {

    }

    public virtual string GetObjectiveDisplayStatus()
    {
        return (objectiveName + ": " + CurrentObjectiveState.ToString());
    }
}
