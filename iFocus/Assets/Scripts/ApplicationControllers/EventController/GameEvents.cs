using System;

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

}