using System;
using System.Collections.Generic;
using System.Text;

namespace orm.Relationships
{
    class OneToManyRelationship : IRelationship
    {
        private object _owner { get; set; }
        private object _owned { get; set; }

        public OneToManyRelationship(object owner, object owned)
        {
            _owned = owned;
            _owner = owner;
        }

        public object getOwned()
        {
            return _owned;
        }


        public object getOwner() {
            return _owner;
        }

        public Type getOwnedType() {
            return _owned.GetType();
        }

    }

}
