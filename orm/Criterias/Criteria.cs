using System;
using System.Collections.Generic;
using System.Text;

namespace orm.Criterias
{
    class Criteria
    {
        private string sqlOperator;
        protected string field;
        protected object value;
        public static List<Criteria> listOfCriteria = new List<Criteria>();


        public static List<Criteria> getListOfCriteria (){
            return listOfCriteria;
        }

        public Criteria(string sqlOperator, string field, object value)
        {
            this.sqlOperator = sqlOperator;
            this.field = field;
            this.value = value;
        }
        public Criteria(){}
        public static Criteria greaterThan(string fieldName, object value)
        {
            Criteria criteria=new Criteria(">", fieldName, value);
            listOfCriteria.Add(criteria);
            return criteria;
        }        
        public static Criteria lessThan(string fieldName, object value)
        {
            Criteria criteria=new Criteria("<", fieldName, value);
            listOfCriteria.Add(criteria);
            return criteria;
        }        
        public static Criteria equals(string fieldName, object value)
        {
            Criteria criteria=new Criteria("=", fieldName, value);
               listOfCriteria.Add(criteria);
            return criteria;
        }        
        public static Criteria notEquals(string fieldName, object value)
        {
            Criteria criteria=new Criteria("!=", fieldName, value);
            listOfCriteria.Add(criteria);
            return criteria;
        }      
        public string generateString()
        {
            if (value.GetType() == typeof(string)) {
                return field + sqlOperator + '"' + value + '"';
            }
            return field + sqlOperator + value.ToString();
        }
    }
}
