using System;
using System.Collections.Generic;
using System.Text;

namespace KellermanSoftware.CompareNETObjectsTests.TestClasses
{
    public interface IEntity
    {
        string Description
        {
            get;
            set;
        }

        Level EntityLevel
        {
            get;
            set;
        }

        Entity Parent
        {
            get;
            set;
        }

        List<Entity> Children
        {
            get;
            set;
        }
    }
}
