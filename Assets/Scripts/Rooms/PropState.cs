using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropState : MonoBehaviour
{
    public EProp eProp = EProp.None;

    public enum EProp
    {
        None,
        Light,
        Shafa,
        Zhanyifu,
        Huaping,
    }
}
