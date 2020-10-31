using Events;
using UnityEngine;

public class Switcher : MonoBehaviour
{
    public GameObject[] objectA;
    public GameObject[] objectB;

    private void OnEnable()
    {
        EventController.AddListener<SwitchEvent>(Switch);
    }
    
    private void OnDisable()
    {
        EventController.RemoveListener<SwitchEvent>(Switch);
    }

    public void Switch(SwitchEvent evt)
    {
        for (int i = 0; i < objectA.Length; i++)
        {
            objectA[i].SetActive(!objectA[i].activeSelf);
        }

        for (int i = 0; i < objectB.Length; i++)
        {
            objectB[i].SetActive(!objectB[i].activeSelf);
        }
    }

    public void Switch(bool switchToA)
    {
        for (int i = 0; i < objectA.Length; i++)
        {
            objectA[i].SetActive(switchToA);
        }

        for (int i = 0; i < objectB.Length; i++)
        {
            objectB[i].SetActive(!switchToA);
        }
    }

}