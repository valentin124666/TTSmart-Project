using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Core.Interfaces;
using Data.Characteristics;
using Managers.Interfaces;
using MazeComponent;
using UnityEngine;

namespace Managers
{
    public class GameDataManager : IService, IGameDataManager
    {
        private Dictionary<SimpleData, string> _data;

        private static string pathData
        {
            get
            {
#if UNITY_EDITOR
                return Path.Combine(Application.dataPath) + "/Data/Json";
#endif
                return Path.Combine(Application.persistentDataPath);
            }
        }

        public void Init()
        {
            _data = new Dictionary<SimpleData, string>();
            
            var mazeData = LoadJsonOrNull(typeof(MazeData));
            if (mazeData == null)
            {
                _data.Add(new MazeData() { width = 10, height = 10 }, typeof(MazeData).GetCustomAttribute<DataName>().Name);
            }
            else
            {
                _data.Add((MazeData)mazeData, typeof(MazeData).GetCustomAttribute<DataName>().Name);
            }
            
            var levelData = LoadJsonOrNull(typeof(LevelData));
            if (levelData == null)
            {
                _data.Add(new LevelData() , typeof(LevelData).GetCustomAttribute<DataName>().Name);
            }
            else
            {
                _data.Add((LevelData)levelData, typeof(LevelData).GetCustomAttribute<DataName>().Name);
            }

        }
        
        private void SaveJson(string json, string sectionName)
        {
            string pathToJson = pathData + sectionName;

            try
            {
                File.WriteAllText(pathToJson, json);
            }
            catch (Exception e)
            {
                Console.WriteLine($"[GameDataManager] - SaveJson: {e.Message}");
                throw;
            }
        }

        public void SaveDataClients()
        {
            if (_data == null) return;

            foreach (var item in _data)
            {
                var json = JsonUtility.ToJson(item.Key);
                SaveJson(json, item.Value);
            }
        }

        public T GetDataOfType<T>() where T : SimpleData
        {
            return (T)_data.Keys.First(data => data is T);
        }


        private static object LoadJsonOrNull(Type typeData)
        {
            var dataName = typeData.GetCustomAttribute<DataName>();

            if (dataName == null)
            {
                Debug.LogError($"[GameDataManager] - LoadJson: The declared class is missing an attribute 'DataName', class {typeof(Type)}");
                throw new NullReferenceException();
            }

            var pathToJson = pathData + dataName.Name;

            if (!File.Exists(pathToJson))
            {
                return null;
            }

            try
            {
                var json = File.ReadAllText(pathToJson);

                return JsonUtility.FromJson(json, typeData);
            }
            catch (Exception e)
            {
                Console.WriteLine($"[GameDataManager] - LoadJson: {e.Message}");
                throw;
            }
        }
    }

    [Serializable]
    public class SimpleData
    {
    }


    [Serializable]
    [DataName("MazeData")]
    public class MazeData : SimpleData
    {
        public int width;
        public int height;
    }

    [Serializable]
    [DataName("LevelData")]
    public class LevelData : SimpleData
    {
        public int width;
        public int height;

        public Maze.MazeGeneratorCell[] cells;
        public Vector2Int posPlayer;
        public int time;
        public int stepsTaken;
    }
}