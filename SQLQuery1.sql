Create table Perfil(
IdPerfil	integer			identity(1,1),
Nome		nvarchar(50)	not null,
primary key(IdPerfil))

Create table Usuarios(
IdUsuario	integer			identity(1,1),
Nome		nvarchar(150)	not null,
Email		nvarchar(100)	not null unique,
Senha		nvarchar(50)	not null,
DataCriacao	datetime		not null,
[Status]	integer			not null,
IdPerfil	integer			not null,
primary key(IdUsuario),
foreign Key(IdPerfil) references Perfil(IdPerfil))