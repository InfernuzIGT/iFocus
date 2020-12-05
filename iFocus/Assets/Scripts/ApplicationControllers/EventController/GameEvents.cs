using System;
using UnityEngine;

namespace Events
{
    public class GameEvent { }

    public class HightlightDataEvent : GameEvent
    {
        public int id;
        public Action action;
    }

    public class QuizAnswerEvent : GameEvent
    {
        public int id;
    }

    public class ChangeButtonStateEvent : GameEvent
    {
        // public ButtonState state;
        public int index;
    }

    public class SwitchHighlightPointEvent : GameEvent
    {
        public bool isLocked;
    }

    public class SwitchEvent : GameEvent { }

    public class ResetSimulationEvent : GameEvent { }

    public class ModeEndingEvent : GameEvent { }

    #region State Events

    public class StateEvent : GameEvent
    {
        public GameObject eventInvoker;
    }

    /// <summary>
    /// Initial state of the simulation. There isn't logic running.
    /// </summary>
    public class StateInitialEvent : StateEvent { }

    /// <summary>
    /// Executed after received food. The simulation is currently running. From this point, it can goes to Events: PauseSimple, PauseHP and Stop.
    /// </summary>
    public class StateRunningEvent : StateEvent { }

    /// <summary>
    /// Trigger when the simulation needs stop completly it's current state.
    /// </summary>
    public class StatePauseSimpleEvent : StateEvent { }

    /// <summary>
    /// Executed when the simulation arrived to the next highlight point.
    /// </summary>
    public class StatePauseHPEvent : StateEvent { }

    /// <summary>
    /// The end state of the simulation. This could be executed from Button End or doe the end of simulation.
    /// </summary>
    public class StateStopEvent : StateEvent { }

    /// <summary>
    /// Executed when a highlight point has been selected
    /// </summary>
    public class StateSelectHPEvent : StateEvent { }

    /// <summary>
    /// Executed when the Info Button has been pressed
    /// </summary>
    public class StateInfoEvent : StateEvent { }

    /// <summary>
    /// Executed when the Preview Video Image has been selected
    /// </summary>
    public class StateFullScreenEvent : StateEvent { }

    /// <summary>
    /// Executed when the Graph Button has been pressed
    /// </summary>
    public class StateGraphEvent : StateEvent { }

    #endregion

}