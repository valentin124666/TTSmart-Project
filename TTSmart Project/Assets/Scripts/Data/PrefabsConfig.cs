using System;
using System.Linq;
using Settings;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [CreateAssetMenu(menuName = "Custom menu/Data/PrefabsConfig")]
    public class PrefabsConfig : ScriptableObject
    {
        [SerializeField] private Config[] configs;

        public GameObject GetPrefabForName(Enumerators.NamePrefabAddressable namePrefabAddressable)
        {
            return configs.First(config => config.Name == namePrefabAddressable).Prefab;
        }
    }

    [Serializable]
    public class Config
    {
        [SerializeField]
        private GameObject prefab;
        [FormerlySerializedAs("_name")] [SerializeField]
        private Enumerators.NamePrefabAddressable name;

        public GameObject Prefab => prefab;
        public Enumerators.NamePrefabAddressable Name => name;
    }
}