using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Interface should be implemented by all the clases that has elements wich may be selected with a raycast.
/// </summary>
public interface IRaySelectable
{
    bool IsSelected { get; set; }

    void Select();

    void Unselect();
}
