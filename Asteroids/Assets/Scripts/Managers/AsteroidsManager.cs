using System.Collections.Generic;
using System.Linq;
using Asteroids.Managers;
using UnityEngine;
using Random = System.Random;


public class AsteroidsManager : MonoBehaviour, IManager
{
    
    #region Fields

    private static AsteroidsManager instance;

    private Dictionary<Asteroid.AsteroidType, GameObject> asteroidsPool =
        new Dictionary<Asteroid.AsteroidType, GameObject>();
    
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


    public void SpawnAsteroid(Asteroid.AsteroidType type)
    {
        Vector3 playerShipLocalPosition = ManagersHub.GetManager<GameManager>().GetPlayerShipLocalPosition();

        playerShipLocalPosition.x += 100f;
        playerShipLocalPosition.y += 100f;
        
        Asteroid[] asteroids = ManagersHub.GetManager<DataManager>().PlayerPreset.Asteroids;
        List<Asteroid> selectedAsteroid = asteroids.Where(a => a.Type == type).ToList();

        Random random = new Random();
        int index = random.Next(0, selectedAsteroid.Count);
        
        GameObject asteroid = Instantiate(selectedAsteroid[index].gameObject, GameSceneReferences.MainCanvas.transform);
        asteroid.transform.localPosition = playerShipLocalPosition;
    }
    
    
    public void Initialize() { }

    public void Unload() { }
}
