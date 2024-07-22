using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    [System.Serializable]
    public class Step
    {
        public int currentStep;
        public int MaxSteps;

        public ExtendedEvent onNextStep;


    }
}
