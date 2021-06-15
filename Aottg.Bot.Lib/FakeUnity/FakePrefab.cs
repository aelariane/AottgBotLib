using System;
using System.Collections.Generic;
using System.Text;

namespace FakeUnity
{
    public sealed class FakePrefab
    {
        public string Name { get; set; }

        private List<FakeComponent> _components;

        public IReadOnlyCollection<FakeComponent> Components { get; set; }

        public FakePrefab(string name, IEnumerable<FakeComponent> components)
        {
            Name = name;
            if(_components == null)
            {
                _components = new List<FakeComponent>();
            }
            else
            {
                _components = new List<FakeComponent>(components);
            }
        }

        public FakePrefab(string name) : this(name, null)
        {
        }
    }
}
