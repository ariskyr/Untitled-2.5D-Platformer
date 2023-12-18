using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDataPersistence
{
    void LoadData(GameData data);
    void SaveData(GameData data);
    
}
