using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flow
{
    public bool initialized;

    public virtual void Initialize(int progressNumber)
    {
        initialized = true;
    }
    // Update is called once per frame
    public virtual void Update (float dt)
    {
		

	}
    public virtual void EndFlow()
    { 
        initialized = false;
    }
    
    
}
