using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityManager  {

    public SpawnManager manager;

    public CitySlot[] citySlots;
    public int availableSlots { private set; get; }
    public bool SlotAvailable => availableSlots > 0;
    
    public static Vector3 Shift(Vector3 origion, float xOffset)
    {
        return new Vector3(origion.x + xOffset, origion.y, 0);
    }

    public CityManager(SpawnManager newManager) //float citySize, float buildingSeperation)
    {
        float totalSize = (manager = newManager).citySize * 2;
        int slots = Mathf.FloorToInt(totalSize / manager.buildingSeperation);
        citySlots = new CitySlot[slots];
        availableSlots = slots;

        for (int i = 0; i < slots; i++)
        {
            citySlots[i] = new CitySlot(i, Shift(manager.planetFloor.position, i * (manager.buildingSeperation) - (totalSize / 2)));
        }

    }

    public class CitySlot
    {
        public int index { private set; get; }
        public Vector3 pos { private set; get; }
        public BaseObject occupant { private set; get; }
        public bool isEmpty => occupant == null || occupant.IsActive == false;
        public BaseObject.Type type => (occupant == null) ? BaseObject.Type.Null : occupant.type;

        public CitySlot(int i, Vector3 p)
        {
            index = i;
            pos = p;
        }

        public void SetOccupant(BaseObject newOccupant)
        {
            if (isEmpty || occupant.IsActive == false)
                occupant = newOccupant;
            else
            {
                Debug.LogError("Trying to overwrite slot " + index);
            }
        }

        public void VoidOccupant()
        {
            if (!isEmpty)
                occupant = null;
        }
    }
    
    public CitySlot GetEmptySlot()
    {
        if (availableSlots > 0)
        {
            int maxAttempts = 50;

            int randomSlotID = Random.Range(0, citySlots.Length);
            while (!citySlots[randomSlotID].isEmpty && maxAttempts > 0)
            {
                randomSlotID = Random.Range(0, citySlots.Length);
                maxAttempts--;
            }

            if (maxAttempts == 0)
            {
                int i = -1;
                bool emptyFound = false;
                while (i < citySlots.Length - 1 && !emptyFound)
                {
                    i++;
                    emptyFound = citySlots[i].isEmpty;
                }

                if (citySlots[i].isEmpty)
                {
                    Debug.Log("Linear search found an empty...");
                    availableSlots--;
                    return citySlots[i]; 
                }
                else
                {
                    Debug.LogError("No available slots...");
                    return null;
                }
                
            }
            else
            {
                availableSlots--;
                return citySlots[randomSlotID];
            }
        }
        else
        {
            Debug.LogError("Not Enough Slots...");
            return new CitySlot(-1, Vector2.zero);
        }
    }

    public CitySlot GetRandomBuilding()
    {
        int maxAttempts = 20;
        int randomIndex = Random.Range(0, citySlots.Length);
        while(citySlots[randomIndex].type != BaseObject.Type.Building && maxAttempts > 0)
        {
            randomIndex = Random.Range(0, citySlots.Length);
            maxAttempts--;
        }
        if(maxAttempts == 0)
        {
            Debug.LogError("Max Attempts exceeded... maybe no slots?");
            return new CitySlot(-1, Vector2.zero);
        }

        return citySlots[randomIndex];
    }

    public void PushSlot(CitySlot citySlot)
    {
        if (citySlot.index != -1 && !citySlot.isEmpty)
        {
            Debug.Log("Slot pushed" + citySlot.index);
            citySlot.VoidOccupant();
            availableSlots++;
        }
        else
        {
            Debug.Log("Empty slot was pushed? " + citySlot.index);
        }
    }
    

}
