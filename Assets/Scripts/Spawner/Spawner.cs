﻿using Unity.Entities;
using UnityEngine;

public struct Spawner : IComponentData
{
    public Entity redSpawnerPrefab;
    public Entity blueSpawnerPrefab;
    public Entity greenSpawnerPrefab;
    public Entity orangeSpawnerPrefab;
    public Entity yellowSpawnerPrefab;
    public Entity pinkSpawnerPrefab;
    public Entity purpleSpawnerPrefab;
    public Entity cyanSpawnerPrefab;
    public int redSpawnerPrefabChances;
    public int blueSpawnerPrefabChances;
    public int greenSpawnerPrefabChances;
    public int orangeSpawnerPrefabChances;
    public int yellowSpawnerPrefabChances;
    public int pinkSpawnerPrefabChances;
    public int purpleSpawnerPrefabChances;
    public int cyanSpawnerPrefabChances;
    public float timeBetweenSpawns;
    public float timeToNextSpawn;
    public int numToSpawn;
    public int numAlreadySpawned;
}
