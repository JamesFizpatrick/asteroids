using System;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Asteroids;
using Asteroids.Handlers;
using Asteroids.Managers;
using UnityEngine;
using Random = System.Random;


public class AsteroidsManager : MonoBehaviour, IManager
{
    #region Fields

    public Action<int> OnInitialAsteroidsDestroyed;
    public Action OnAllAsteroidsDestroyed;
    
    private static AsteroidsManager instance;
    private Dictionary<AsteroidType, List<GameObject>> asteroidsPool = new Dictionary<AsteroidType, List<GameObject>>();

    private int startQuantity = 0;
    private int destroyedCount = 0;

    private SoundManager soundManager;
    
    #endregion


        
    #region Properties

    public static AsteroidsManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject managerGo = new GameObject("AsteroidsManager");
                AsteroidsManager manager = managerGo.AddComponent<AsteroidsManager>();
                instance = manager;
            }

            return instance;
        }
    }

    #endregion



    #region Public methods
    
    public void SpawnAsteroids(int quantity, Vector3 playerPosition, float safeRadius)
    {
        startQuantity = quantity;
        
        Random random = new Random();

        float directionX;
        float directionY;
        float positionX;
        float positionY;
        Vector3 direction;
        Vector3 position;

        int minX = (int)(playerPosition.x - safeRadius);
        int maxX = (int)(playerPosition.x + safeRadius);
        int minY = (int)(playerPosition.y - safeRadius);
        int maxY = (int)(playerPosition.y + safeRadius);

        for (int i = 0; i < quantity; i++)
        {
            positionX =
                random.GetRandomExclude(-Screen.width / 2, Screen.width / 2, minX, maxX);
            positionY =
                random.GetRandomExclude(-Screen.height / 2, Screen.height / 2, minY, maxY);
            position = new Vector3(positionX, positionY);

            directionX = positionX > 0 ? random.GetRandomFloat(-1f, 0f) : random.GetRandomFloat(0f, 1f);
            directionY = positionY > 0 ? random.GetRandomFloat(-1f, 0f) : random.GetRandomFloat(0f, 1f);
            direction = new Vector3(directionX, directionY);
            
            Asteroid asteroid = SpawnAsteroid(AsteroidType.Huge, position);
            asteroid.OverrideDirection(direction);
        }
    }


    public int GetActiveAsteroidsCount()
    {
        int count = 0;
        foreach (KeyValuePair<AsteroidType, List<GameObject>> pair in asteroidsPool)
        {
            count += pair.Value.Count(x => x.activeSelf);
        }

        return count;
    }
    
    
    public void Reset()
    {
        foreach (KeyValuePair<AsteroidType, List<GameObject>> pair in asteroidsPool)
        {
            foreach (GameObject asteroid in pair.Value)
            {
                Asteroid asteroidComponent = asteroid.GetComponent<Asteroid>();
                asteroidComponent.Destroyed -= Asteroid_Destroyed;
                Destroy(asteroid);
            }
        }

        destroyedCount = 0;
        asteroidsPool.Clear();
    }


    public void Initialize()
    {
        soundManager = ManagersHub.GetManager<SoundManager>();
    }

    
    public void Unload() { }
    
    #endregion



    #region Private methods

    private Asteroid SpawnAsteroid(AsteroidType type, Vector3 position)
    {
        Asteroid asteroid = TryReuseAsteroid(type);
        if (asteroid == null)
        {
            asteroid = SpawnNewAsteroid(type);
        }
        
        asteroid.transform.localPosition = position;
        return asteroid;
    }
    
    
    private Asteroid SpawnNewAsteroid(AsteroidType type)
    {
        Asteroid[] asteroids = ManagersHub.GetManager<DataManager>().PlayerPreset.Asteroids;
        List<Asteroid> selectedAsteroid = asteroids.Where(a => a.Type == type).ToList();

        Random random = new Random();
        int index = random.Next(0, selectedAsteroid.Count);
        
        GameObject asteroid = Instantiate(selectedAsteroid[index].gameObject, GameSceneReferences.MainCanvas.transform);

        if (asteroidsPool.ContainsKey(type))
        {
            List<GameObject> asteroidsList = asteroidsPool[type];
            asteroidsList.Add(asteroid);
            asteroidsPool[type] = asteroidsList;
        }
        else
        {
            asteroidsPool.Add(type, new List<GameObject>(){asteroid});
        }

        Asteroid asteroidComponent = asteroid.GetComponent<Asteroid>();
        asteroidComponent.Destroyed += Asteroid_Destroyed;
        
        return asteroidComponent;
    }

    
    private Asteroid TryReuseAsteroid(AsteroidType type)
    {
        if (!asteroidsPool.ContainsKey(type))
        {
            return null;
        }
        
        List<GameObject> spawnedAsteroids = asteroidsPool[type];
        GameObject reusableAsteroid = spawnedAsteroids.Find(asteroid => !asteroid.gameObject.activeSelf);

        if (reusableAsteroid == null)
        {
            return null;
        }
        
        reusableAsteroid.SetActive(true);
        return reusableAsteroid.GetComponent<Asteroid>();
    }
    
    #endregion



    #region Event handlers

    private void Asteroid_Destroyed(Asteroid asteroid)
    {
        asteroid.Destroyed -= Asteroid_Destroyed;
        
        soundManager.PlaySound(SoundType.Explosion);
        
        if (asteroid.Type == AsteroidType.Huge)
        {
            if (destroyedCount != startQuantity)
            {
                destroyedCount++;
                OnInitialAsteroidsDestroyed?.Invoke(destroyedCount);
            }
        }
        
        Vector3 direction = asteroid.CurrentDirection;
        AsteroidType nextType = asteroid.Type.Next();
        Vector3 parentLocalPosition = asteroid.transform.localPosition;
        
        if (nextType != AsteroidType.None)
        {
            Asteroid leftAsteroid = CreateChildAsteroid(nextType, parentLocalPosition);
            Asteroid rightAsteroid = CreateChildAsteroid(nextType, parentLocalPosition);
            
            (Vector3 leftVector, Vector3 rightVector) = direction.GetBreakVectors(30f);
            
            leftAsteroid.OverrideDirection(leftVector);
            rightAsteroid.OverrideDirection(rightVector);
        }
        else if (GetActiveAsteroidsCount() == 0)
        {
            OnAllAsteroidsDestroyed?.Invoke();
        }
    }


    private Asteroid CreateChildAsteroid(AsteroidType nextType, Vector3 position)
    {
        Asteroid asteroid = SpawnAsteroid(nextType, position);
        asteroid.Destroyed += Asteroid_Destroyed;

        return asteroid;
    }
    
    #endregion
}
