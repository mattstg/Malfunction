using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public SceneObjectManager manager;

    public Transform planetFloor;
    public Transform sky;

    public float buildingSeperation = 1;
    public float cityXSize = 10;

    public Vector3 Shift(Vector3 origion, float xOffset)
    {
        return new Vector3(origion.x + xOffset, origion.y, 0);
    }

	public void Initialize()
    {
        float currentSeperation = 0;
        manager.SpawnObjectFromPool(BaseObject.Type.Building, Shift(planetFloor.position, currentSeperation));
        while(currentSeperation <= cityXSize)
        {
            manager.SpawnObjectFromPool(BaseObject.Type.Building, Shift(planetFloor.position, currentSeperation));
            manager.SpawnObjectFromPool(BaseObject.Type.Building, Shift(planetFloor.position, -currentSeperation));
            currentSeperation += buildingSeperation;
        }
        
    }

    public void StartGame()
    {

    }

    public void EndGame()
    {

    }
}
