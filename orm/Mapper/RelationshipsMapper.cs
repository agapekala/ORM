using System;
using System.Collections.Generic;
using System.Text;
using orm.Relationships;
using System.Reflection;
using orm.Attributes;

namespace orm.Mapper
{
    class RelationshipsMapper {

        // TO-DO: Implement findOneToManyRelationships and perhaps merge it with the below function.
        public List<IRelationship> findOneToOneRelationships(object o) {
            List<IRelationship> listOfRelationships = new List<IRelationship>();
            Type type = o.GetType();

            PropertyInfo[] props = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (PropertyInfo prp in props)
            {
                MethodInfo strGetter = prp.GetGetMethod(nonPublic: true);
                object[] att = prp.GetCustomAttributes(typeof(OneToOneAttribute), false);
                var val = strGetter.Invoke(o, null);

                if (att.Length != 0) {
                    object owner = o;
                    object owned = val;
                    OneToOneRelationship oneToOneRelationship = new OneToOneRelationship(owner, owned);
                    listOfRelationships.Add(oneToOneRelationship);
                }
            }
            return listOfRelationships;   
        }

    }
}
