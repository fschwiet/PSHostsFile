using System;
using System.Collections.Generic;

namespace KellermanSoftware.CompareNETObjectsTests.TestClasses
{
    public enum Level 
    {
        Company,
        Division,
        Department
    }

    [Serializable]
    public class Entity : IEntity
    {
        private List<Entity> _children = new List<Entity>();

        public string Description
        {
            get;
            set;
        }

        public Level EntityLevel
        {
            get;
            set;
        }

        public Entity Parent
        {
            get;
            set;
        }

        public List<Entity> Children
        {
            get { return _children; }
            set { _children = value; }
        }
    }
}
