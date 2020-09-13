/*
 * 
 *  Riptide Studios 2020
 * 
 */

using System.Collections.Generic;
using UnityEngine;

public class RandomPlacer : MonoBehaviour {
    public int randomToPlace;
    public Identification[] placeableObjects;

    public List<SavableObjects> savableObjects = new List<SavableObjects>();

    SaveLoad saveLoad;

    Transform parent;

    public void Start() {
        saveLoad = GetComponent<SaveLoad>();

        parent = Instantiate(new GameObject("parent")).transform;
        if (saveLoad.Load() == false) {
            for (int i = 0; i < randomToPlace; i++) {
                int rand = Random.Range(0, placeableObjects.Length);    // picks a random object
                Vector3 randomPos = new Vector3(Random.Range(-50f, 50), 1, Random.Range(-50f, 50)); // picks a random position
                GameObject obj = Instantiate(placeableObjects[rand].prefab); // places the picked object
                obj.transform.position = randomPos; // sets the position of the object
                obj.transform.parent = parent;

                savableObjects.Add(new SavableObjects(placeableObjects[rand].id, obj.transform.position, obj.transform.rotation));

                saveLoad.Save();
            }
        }
    }

    public void Reinstantiate() {
        // Reinstantiates the objects 
        for(int i = 0; i < savableObjects.Count; i++) {
            for(int z = 0; z < placeableObjects.Length; z++) {
                if(savableObjects[i].id == placeableObjects[z].id) {  // Checking what the object is
                    GameObject obj = Instantiate(placeableObjects[z].prefab);
                    // Sets the position, rotation and parent of the objects loaded from the save file
                    obj.transform.position = savableObjects[i].ReturnPosition();
                    obj.transform.rotation = savableObjects[i].ReturnRotation();
                    obj.transform.parent = parent;
                }
            }
        }
    }

}

[System.Serializable]
public class SavableObjects {
    public string id;
    public float px, py, pz;
    // Serializable version of a Vector3
    public float rx, ry, rz, rw;
    // Serializable version of a Quaternion

    public SavableObjects(string id, Vector3 position, Quaternion rotation) {
        // Set all the class variables
        this.id = id;

        px = position.x;
        py = position.y;
        pz = position.z;

        rx = rotation.x;
        ry = rotation.y;
        rz = rotation.z;
        rw = rotation.w;
    }

    // Returns the position in a Vector3 from the serialized floats
    public Vector3 ReturnPosition() {
        Vector3 pos = new Vector3(px, py, pz);
        return pos;
    }

    // Returns the rotation in a Quaternion from the serialized floats
    public Quaternion ReturnRotation() {
        Quaternion rot = new Quaternion(rx, ry, rz, rw);
        return rot;
    }
}

[System.Serializable]
public class Identification {
    // Specifically for identifying the objects based on their serialized IDs
    public string id;
    public GameObject prefab;
}