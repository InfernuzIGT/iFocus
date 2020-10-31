using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsController : MonoBehaviour
{
    private delegate void Event(int a);
    Event myEvent;

    public void SendMesagge()
    {
        // TODO: Este metodo deberia encargarse de decirle a los objetos correspondientes que se ejecuto un evento determinado.
    }

    public void Subscribe()
    {
        // TODO: Este metodo es el que permite que un objeto se subscriba a los eventos, bien para ser llamado o para ser informado de la ejecucion de un evento.
    }

    public void StartEvent()
    {
        // TODO: Este metodo debe encargarse de decirle a los objetos que se ejecuto cierto evento. Para que estos hagan lo que les corresponde.
    }

}

