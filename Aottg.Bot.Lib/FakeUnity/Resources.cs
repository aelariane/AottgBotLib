using System.Collections.Generic;

namespace FakeUnity
{
    public static class Resources
    {
        private static Dictionary<string, FakePrefab> _prefabs = new Dictionary<string, FakePrefab>();
        public static IReadOnlyCollection<string> PrefabNames => _prefabs.Keys;

        public static void AddPrefab(FakePrefab prefab)
        {
            if (_prefabs.ContainsKey(prefab.Name))
            {
                return;
            }

            _prefabs.Add(prefab.Name, prefab);
        }

        public static FakeGameObject Load(string prefabName)
        {
            if (!_prefabs.ContainsKey(prefabName))
            {
                return null;
            }

            FakePrefab prefab = _prefabs[prefabName];

            var result = new FakeGameObject(prefab);

            return result;
        }
    }
}