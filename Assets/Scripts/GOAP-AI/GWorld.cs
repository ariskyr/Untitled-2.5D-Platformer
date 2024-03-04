using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//different kind of singleton, since this doesn't derive from MonoBehaviour
public sealed class GWorld
{
    private static readonly GWorld instance = new GWorld();
    private static WorldStates world;
    //TODO: remove after
    private static Queue<GameObject> patients;
    private static Queue<GameObject> cubicles;

    static GWorld()
    {
        world = new WorldStates();

        //TODO: remove after
        patients = new Queue<GameObject>();
        cubicles = new Queue<GameObject>();
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("cubicle");
        foreach (GameObject c in cubes)
        {
            cubicles.Enqueue(c);
        }
        if (cubes.Length > 0)
        {
            world.ModifyState("freeCubicle", cubes.Length);
        }
    }

    private GWorld()
    {

    }

    //TODO: remove after
    public void AddPatient(GameObject p)
    {
        patients.Enqueue(p);
    }

    //TODO: remove after
    public GameObject RemovePatient()
    {
        if (patients.Count == 0) return null;
        return patients.Dequeue();
    }

    //TODO: remove after
    public void AddCubicle(GameObject c)
    {
        cubicles.Enqueue(c);
    }

    //TODO: remove after
    public GameObject RemoveCubicle()
    {
        if (cubicles.Count == 0) return null;
        return cubicles.Dequeue();
    }

    public static GWorld Instance
    {
        get { return instance; }
    }

    public WorldStates GetWorld()
    {
        return world;
    }
}
