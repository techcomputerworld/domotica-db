## Aplicacion Domotica-db 

Aplicación en ASP.NET Core 2.2 creada con la opcion de vistas en MVC.

Para ponerla en marcha la descargáis en vuestro PC, yo la ando desarrollando en Visual Studio 2019. 

Se puede programar con Visual Studio Code y con Mac también y en Linux. 

### puesta en marcha

editar appsetting.json
{
  "ConnectionStrings": {
    //"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=aspnet-Domotica_db-B49DB4ED-9605-4641-8C42-5C12BF4CDE57;
    //"DefaultConnection": "Data Source=ubuntu\\ubuntu;Initial Catalog=sistema_facturacion;Integrated Security=True"
    Trusted_Connection=True;MultipleActiveResultSets=true"
    "DefaultConnection": "Server=192.168.1.216;userid=root;Password=tu clave;Database=domotica0;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "AllowedHosts": "*"
}

Configuráis la conexión con la base de datos y luego usáis las migraciones actualizando la base de datos, hay que crear base de datos y tener una vacia.

### Sobre MySQL

Hay que crear la tabla, tal cual en MySQL poniendo ese comando tal cual en SQL en MySQL, yo uso MySQL Workbench para esa tarea realmente se puede poner hasta en la consola.




DROP TABLE IF EXISTS `__EFMigrationsHistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` text NOT NULL,
  `ProductVersion` text NOT NULL,
  PRIMARY KEY (`MigrationId`(255))
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

En la consola de paquetes nuget ponéis:
Update-Database 
En la consola de comandos es: 
dotnet ef database update 

## Web del proyecto 

https://techcomputerworld.com

