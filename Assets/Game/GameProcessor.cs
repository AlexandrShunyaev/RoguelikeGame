using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameProcessor : MonoBehaviour
{
    [Header("Map")]
    [SerializeField]
    public AbstractDungeonGenerator dungeonGenerator;

    [FormerlySerializedAs("_player")]
    [Header("Character")]
    [SerializeField]
    private GameObject player;
    
    [SerializeField]
    public GameObject enemyPrefab;

    [SerializeField]
    public int enemyCount;

    #region PrivateFields

    private List<BoundsInt> _rooms;
    private List<GameObject> _enemies = new();
    private int _amountOfAliveEnemies;

    #endregion

    #region UnityEvents

    private void Awake()
    {
        _amountOfAliveEnemies = enemyCount;
        StartGame();
    }

    #endregion

    #region PublicMethods

    public void Restart()
    {
        _rooms.Clear();
        //player.SetActive(false);
        _enemies.ForEach(Destroy);
        
        StartGame();
    }

    public void EpisodeEnd(GameObject agent)
    {
        if (_enemies.Contains(agent))
        {
            _enemies.Remove(agent);
            Destroy(agent);
        }

        if (_enemies.Count == 0)
        {
            NextEpisodeInTraining();
        }
    }
    
    public void NextEpisodeInTraining()
    { 
        _rooms.Clear();
        StartGame();
    }

    #endregion

    #region PrivateMethods

    private void StartGame()
    {
        GenerateDungeon();

        var playerRoom = GetPlayerRoom();
        // CreatePlayer(playerRoom);
        var enemyRooms = _rooms.Count > 1 ? _rooms.Except(new List<BoundsInt> { playerRoom }).ToList() : _rooms;
        CreateEnemies(enemyRooms);
        ReplaceCharacter(player, playerRoom);
        
        // place items...
        
        SetActiveForAllCharacters(true);
    }

    private void GenerateDungeon()
    {
        dungeonGenerator.GenerateDungeon();
        _rooms = dungeonGenerator.Rooms;
    }

    #region CharactersMethods
    
    private void SetActiveForAllCharacters(bool value)
    {
        player.SetActive(value);
        _enemies.ForEach(x => x.SetActive(value));
    }

    private BoundsInt GetPlayerRoom() => _rooms[Random.Range(0, _rooms.Count)];

    private void CreateEnemies(IReadOnlyList<BoundsInt> rooms)
    {
        for (var i = 0; i < enemyCount; i++)
        {
            CreateEnemy(rooms[Random.Range(0, rooms.Count)]);
        }
    }
    
    private void CreateEnemy(BoundsInt room)
    {
        var enemy = Instantiate(enemyPrefab, new Vector3(room.center.x, room.center.y), Quaternion.identity);
        enemy.SetActive(false);
        _enemies.Add(enemy);
    }

    private void ReplaceCharacters(BoundsInt playerRoom, IReadOnlyList<BoundsInt> anotherRooms)
    {
        ReplaceCharacter(player, playerRoom);
        foreach (var enemy in _enemies)
        {
            ReplaceCharacter(enemy, anotherRooms[Random.Range(0,anotherRooms.Count)]);
        }
    }

    private void ReplaceCharacter(GameObject character, BoundsInt room)
        => character.transform.position = new Vector3(room.center.x, room.center.y);
    
    #endregion


    #endregion
}
