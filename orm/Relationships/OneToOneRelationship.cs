using System;
using System.Collections.Generic;
using System.Text;

namespace orm.Relationships
{
    class OneToOneRelationship : IRelationship
    {
        private object _owner { get; set; }
        private object _owned { get; set; }

        public OneToOneRelationship(object owner, object owned) {
            _owned = owned;
            _owner = owner;
        }

        public object getOwned() {
            return _owned;
        }
    }
}
