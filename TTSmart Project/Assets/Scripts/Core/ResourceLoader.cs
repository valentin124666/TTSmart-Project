using System;
using System.Reflection;
using Data;
using Settings;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core
{
    public static class ResourceLoader
    {
        private static Transform _parentlessPool;
        private static PrefabsConfig _prefabsConfig;

        public static void Init()
        {
            _parentlessPool = new GameObject().transform;
            _parentlessPool.name = "[ParentlessPool]";

            _prefabsConfig = MainApp.Instance.PrefabsConfig;
        }

        #region Instantiate

        public static TP Instantiate<TP, TV>(Transform parent, string locationSuffix, params object[] args)
            where TP : SimplePresenter<TP, TV> where TV : SimplePresenterView<TP, TV>
        {
            var view = Instantiate<TV>(typeof(TV).GetCustomAttribute<PrefabInfo>().Location, parent);

            if (view != null)
            {
                return (TP)view.Instantiate(args);
            }

            throw new NullReferenceException();
        }

        public static T Instantiate<T>(Enumerators.NamePrefabAddressable key, Transform parent) where T : Component
        {
            var prefab = Object.Instantiate(_prefabsConfig.GetPrefabForName(key), parent ? parent : _parentlessPool);
            return prefab.GetComponent<T>();
        }
        
        #endregion
    }
}