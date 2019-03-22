using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuke : Rocket {

    public override Type TypeToSpawnOnDeath => Type.NukeExplosion;

    public override void DeathFromExplosion()
    {
        manager.SpawnObjectFromPool(TypeToSpawnOnDeath, transform.position);
        isActive = false;
        gameObject.SetActive(false);
    }
}
