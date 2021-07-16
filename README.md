# Bodegas
Proceso básico para controlar entradas y salidas de Kardex. 

Requisitos previos:
Visual Studio 2019, 
MySQL Server
Instalación - Desarrollador
Clonar el proyecto en de Visual Studio, Ejecutar y esperar que baje los paquetes NutGet para poder hacer conexiones con la base de datos de MySql

Modificar el archivo appsettings.json, 
con sus datos de usuario, password y host. 
Por defecto el host es 'locahost', el usuario es 'earh' y el password '123456'. 
No dejar espacios después del símbolo '=', además los datos deben empezar desde la primera línea sin dejar saltos de líneas antes.
El host también puede ser la dirección IP de una máquina remota en red local. Ejemplo:

host=127.0.0.1
usuario=earh
password=123456
port=3306
database=bodega

No editar este archivo.
Ejecutar los scripts SQL en el servidor MySQL del host, tal como esta creado el archivo, sin cambiarle el orden. 

![image](https://user-images.githubusercontent.com/24233265/126014005-aec66b35-966d-46a4-ab45-d0f59631f617.png)
![image](https://user-images.githubusercontent.com/24233265/126014047-fd87abf0-7136-41e4-9941-ea1882929a34.png)
![image](https://user-images.githubusercontent.com/24233265/126014056-853f8296-6d91-4012-ab52-d26c28fc56f5.png)
![image](https://user-images.githubusercontent.com/24233265/126014108-e4fe3740-05f2-4128-b9ac-8c944f712a3b.png)
