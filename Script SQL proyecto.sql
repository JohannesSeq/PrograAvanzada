CREATE DATABASE PROYECTO_BANCO_LOS_PATITOS

GO

USE PROYECTO_BANCO_LOS_PATITOS

GO

CREATE TABLE COMERCIO (
    IdComercio int identity (1,1) not null,
    Identificacion varchar(30) not null,
    TipoIdentificacion int not null,
	Nombre varchar (200) not null,
	TipoDeComercio int not null,
	Telefono varchar (20) not null,
	CorreoElectronico varchar (200) not null,
	Direccion varchar (500) not null,
	FechaDeRegistro datetime not null,
	FechaDeModificacion datetime,
	Estado bit not null,
);

GO

CREATE TABLE CAJA (
    IdCaja int identity (1,1) not null,
    IdComercio int,
	Nombre varchar (100) not null,
	Descripcion varchar (100) not null,
	TelefonoSINPE varchar (10) not null,
	FechaDeRegistro datetime not null,
	FechaDeModificacion datetime,
	Estado bit not null,
);

GO

CREATE TABLE SINPE(
	IdSinpe int identity (1,1) not null,
	TelefonoOrigen varchar (10) not null,
	NombreOrigen varchar (200) not null,
	TelefonoDestinatario varchar (10) not null,
	NombreDestinatario varchar (200) not null,
	Monto Decimal (18,2) not null,
	Descripcion varchar (50) not null,
	FechaDeRegistro datetime not null,
	FechaDeModificacion datetime,
	Estado bit not null,
);

GO

CREATE TABLE BITACORA(
	IdEvento int identity (1,1) not null,
	TablaDeEvento varchar (20) not null,
	TipoDeEvento varchar (20) not null,
	FechaDeEvento datetime not null,
	DescripcionDeEvento varchar (max) not null,
	StackTrace varchar (max) not null,
	DatosAnteriores varchar (max),
	DatosPosteriores varchar (max),
);

GO

ALTER TABLE COMERCIO
ADD CONSTRAINT COMERCIO_PK PRIMARY KEY (IdComercio)
GO

ALTER TABLE CAJA
ADD CONSTRAINT CAJA_PK PRIMARY KEY (IdCaja)
GO

ALTER TABLE CAJA
ADD CONSTRAINT CAJA_CMERCIO_PK FOREIGN KEY (IdComercio)  REFERENCES COMERCIO (IdComercio)
GO

ALTER TABLE SINPE
ADD CONSTRAINT SINPE_PK PRIMARY KEY (IdSinpe)
GO

ALTER TABLE BITACORA
ADD CONSTRAINT BITACORA_PK PRIMARY KEY (IdEvento)
GO

CREATE TABLE CONFIGURACION_DE_COMERCIO(
	idConfiguracion int identity (1,1) not null,
	IdComercio int,
	TipoConfiguracion int not null,
	Comision int not null,
	FechaDeRegistro datetime not null,
	FechaDeModificacion datetime not null,
	Estado bit not null
);

GO

ALTER TABLE CONFIGURACION_DE_COMERCIO
ADD CONSTRAINT CMERCIO_CONF_PK FOREIGN KEY (IdComercio)  REFERENCES COMERCIO (IdComercio)
GO
