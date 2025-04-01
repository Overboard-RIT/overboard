using System.Collections;
using UnityEngine;
using Rive;
using Rive.Components;

public class RiveAnimationCycler : MonoBehaviour
{
    public RiveWidget riveWidget;
    private int currentStateMachineIndex = 0;
    private StateMachine m_stateMachine;

    void Start()
    {
        m_stateMachine = riveWidget.Artboard?.StateMachine();

        if (riveWidget == null)
        {
            Debug.LogError("RiveWidget is not assigned!");
            return;
        }
        StartCoroutine(CycleAnimations());
    }

    private void Update()
    {
        m_stateMachine?.Advance(Time.deltaTime);
    }

    IEnumerator CycleAnimations()
    {
        while (true)
        {

            // Load and play the current state machine
            riveWidget.Load(riveWidget.File, riveWidget.Artboard.Name, m_stateMachine.Name);

            // Wait for 5 seconds
            yield return new WaitForSeconds(5f);

        }
    }
}
