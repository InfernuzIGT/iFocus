using UnityEngine;

public enum BUTTONMASTER_STATE
{
    Food = 0,
    Play = 1,
    Pause = 2,
    Back = 3,
    Cancel = 4
}

[CreateAssetMenu(fileName = "New Button Master Asset", menuName = "ScriptableObjects/Button Master", order = 2)]
public class ButtonMasterSO : ScriptableObject
{
    [PreviewTexture(48), SerializeField] private Sprite iconFood;
    [PreviewTexture(48), SerializeField] private Sprite iconPlay;
    [PreviewTexture(48), SerializeField] private Sprite iconPause;
    [PreviewTexture(48), SerializeField] private Sprite iconBack;
    [PreviewTexture(48), SerializeField] private Sprite iconCancel;

    public Sprite GetIcon(BUTTONMASTER_STATE state)
    {
        switch (state)
        {
            case BUTTONMASTER_STATE.Food:
                return iconFood;

            case BUTTONMASTER_STATE.Play:
                return iconPlay;

            case BUTTONMASTER_STATE.Pause:
                return iconPause;

            case BUTTONMASTER_STATE.Back:
                return iconBack;

            case BUTTONMASTER_STATE.Cancel:
                return iconCancel;
        }

        Debug.LogError($"<color=red><b>[ERROR]</b></color> Can't get icon by state {state}");
        return null;
    }

}