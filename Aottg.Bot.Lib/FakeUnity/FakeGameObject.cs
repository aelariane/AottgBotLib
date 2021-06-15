using System.Collections.Generic;

namespace FakeUnity
{
    public sealed class FakeGameObject
    {
        private List<FakeComponent> _components;

        public IReadOnlyCollection<FakeComponent> Components => _components;

        public string Name { get; set; }

        internal FakeGameObject(FakePrefab source)
        {
            Name = source.Name;
            if (source.Components.Count > 0)
            {
                _components = new List<FakeComponent>(source.Components);
            }
            else
            {
                _components = new List<FakeComponent>();
            }
        }

        public T GetComponent<T>() where T : FakeComponent
        {
            foreach (var comp in _components)
            {
                if (comp.GetType() == typeof(T))
                {   
                    return comp as T;
                }
            }

            return null;
        }
    }
}