using System;
using UnityEngine;

[Serializable]
public class SaveData 
{
    public bool[] FoundSpareParts = new bool[5];
    public bool[] foundDocumentsCollectable = new bool[15];
    public float posX, posY, posZ;
}
