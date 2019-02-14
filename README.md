# ORM
 University project on Object-Relational Mapping.
 # How do I use this outstanding ORM?

### How do I start?

Configure connection in your app.

    SSqlConnection conn = MSSqlConnection.GetInstance();
    ConnConfiguration conf = new ConnConfiguration("YourServerName", "YourDataBaseName");
    conn.setConfiguration(conf);
Create an instance of Manager class using the above connection as follows. Use your Manager class instance to map object.


    Manager mng = new Manager(conn);

### How do I make my class 'mappable'?
Add TableAttribute as an annotation above class declaration. (If the table in your database is represented by different name than the class name, you need to pass table's name into the constructor of the TableAttribute)

    [Table("TableNameOfMyClass")]
    class MyClass {...}
or

    [Table()]
    class myClass{...}

    
Add ColumnAttribute as an annotation above fields in class declaration, that should 
be as columns in the database table.

    [Table()]
    class myClass{
    
      [Column("myId")]
      protected int _id { get; set; }
      
      [Column()]
      protected int _name { get; set; }

    }

Add PrimaryKeyAttribute as an annotation above the field that is represented as primary key in the db's table.
    
    [Table()]
    class myClass{
      [Column("myId")]
      [PrimaryKey()]
      protected int _id { get; set; }
      // Further class declaration.
    }
    


### How do I determine relationships between tables?
**ONE-TO-ONE RELATIONSHIP**

To hold one-to-one relation between myClass and myClass2 you need to declare myClass2 field inside 
myClass with OneToOneAttribute as an annotation above it.

    [Table()]
    class myClass {
      [Column("myId")]
      [PrimaryKey()]
      protected int _id { get; set; }
      
      [Column("myColumnName")]
      [OneToOne()]
      protected myClass2 _myClass2 { get; set; }
      // Further class declaration.
    }
    
    
    [Table("tableNameInMyClass2")]
    class myClass2{
      [Column()]
      [PrimaryKey()]
      protected int _id { get; set; }
      // Further class declaration.
    }
  
Such classes are represented as following tables:
>  Table1:
>    Table name: myClass;
>    Column name : myId;
>    Column name: myColumnName;

> Table2:
>    Table name: tableNameInMyClass2;
>    Column name: _id;
  
**ONE-TO-MANY RELATIONSHIP**

To hold one-to-many relationship between myClass (one) and myClass2 (many), you need to declare 
a list of type myClass2 with an annotation OneToManyAttribute.

    [Table()]
    class myClass {
      [Column("myId")]
      [PrimaryKey()]
      protected int _id { get; set; }
      
      [OneToMany()]
      protected List<myClass2> _myClass2List { get; set; }
      // Further class declaration.
    }
    
    
    [Table()]
    class myClass2{
      [Column()]
      [PrimaryKey()]
      protected int primaryKey { get; set; }
      // Further class declaration.
    }  

Such classes are represented as following tables:
>    Table1:
>      Table name: myClass;
>      Column name: myId;

> Table2:
>      Table name: myClass2;
>      Column name: primaryKey;
>      Column name: myClassId;




