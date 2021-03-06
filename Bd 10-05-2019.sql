USE [master]
GO
/****** Object:  Database [HorarioBD2]    Script Date: 10/05/2019 4:09:17 p. m. ******/
CREATE DATABASE [HorarioBD2]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'HorarioBD2', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLSERV2014\MSSQL\DATA\HorarioBD2.mdf' , SIZE = 8384KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'HorarioBD2_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLSERV2014\MSSQL\DATA\HorarioBD2_log.ldf' , SIZE = 1072KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [HorarioBD2] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HorarioBD2].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [HorarioBD2] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [HorarioBD2] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [HorarioBD2] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [HorarioBD2] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [HorarioBD2] SET ARITHABORT OFF 
GO
ALTER DATABASE [HorarioBD2] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [HorarioBD2] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [HorarioBD2] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [HorarioBD2] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [HorarioBD2] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [HorarioBD2] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [HorarioBD2] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [HorarioBD2] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [HorarioBD2] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [HorarioBD2] SET  ENABLE_BROKER 
GO
ALTER DATABASE [HorarioBD2] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [HorarioBD2] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [HorarioBD2] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [HorarioBD2] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [HorarioBD2] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [HorarioBD2] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [HorarioBD2] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [HorarioBD2] SET RECOVERY FULL 
GO
ALTER DATABASE [HorarioBD2] SET  MULTI_USER 
GO
ALTER DATABASE [HorarioBD2] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [HorarioBD2] SET DB_CHAINING OFF 
GO
ALTER DATABASE [HorarioBD2] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [HorarioBD2] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [HorarioBD2] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'HorarioBD2', N'ON'
GO
USE [HorarioBD2]
GO
/****** Object:  UserDefinedFunction [dbo].[diasDomingo]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[diasDomingo]
(
@FechaInicio datetime,
@FechaFin datetime

)
RETURNS INT
AS
BEGIN
 
IF @FechaInicio > @FechaFin RETURN 0
 
DECLARE @RangoFechas TABLE(fecha datetime)
DECLARE @Dia datetime
DECLARE @DiaSemana tinyint=7

 
SET @Dia = @FechaInicio
 
while @Dia <= @FechaFin
BEGIN
    INSERT INTO @RangoFechas
    SELECT @Dia
 
    SET @Dia = @Dia + 1
END
 
RETURN (
    SELECT SUM(CASE WHEN datepart(weekday, fecha) = @DiaSemana THEN 1 ELSE 0 END)
    FROM @RangoFechas
)
 
END










GO
/****** Object:  UserDefinedFunction [dbo].[DiasLaborados]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[DiasLaborados]
(
@FechaInicio datetime,
@FechaFin datetime,
@Dia1 bit,
@Dia2 bit,
@Dia3 bit,
@Dia4 bit,
@Dia5 bit

)
RETURNS INT
AS
BEGIN
 
DECLARE @Lunes tinyint
DECLARE @Martes tinyint
DECLARE @Miercoles tinyint
DECLARE @Jueves tinyint
DECLARE @Viernes tinyint


IF @FechaInicio > @FechaFin RETURN 0
 
	IF @Dia1 = 1

		SET @Lunes = 1
	ELSE 

		SET @Lunes = 0

	IF @Dia2 = 1

		SET @Martes = 2
	ELSE 
		SET @Martes = 0

	IF @Dia3 = 1

		SET @Miercoles = 3
	ELSE 
		SET @Miercoles = 0
	
	IF @Dia4 = 1

		SET @Jueves = 4
	ELSE 
		SET @Jueves = 0

	IF @Dia5 = 1

		SET @Viernes = 5
	ELSE 
		SET @Viernes = 0

DECLARE @RangoFechas TABLE(fecha datetime)
DECLARE @Dia datetime

 
SET @Dia = @FechaInicio
 
while @Dia <= @FechaFin
BEGIN
    INSERT INTO @RangoFechas
    SELECT @Dia
 
    SET @Dia = @Dia + 1
END
 
RETURN (
    SELECT SUM(CASE WHEN datepart(weekday, fecha) = @Lunes THEN 1 ELSE 0 END)+SUM(CASE WHEN datepart(weekday, fecha) = @Martes THEN 1 ELSE 0 END)+ SUM(CASE WHEN datepart(weekday, fecha) = @Miercoles THEN 1 ELSE 0 END)+
		SUM(CASE WHEN datepart(weekday, fecha) = @Jueves THEN 1 ELSE 0 END)+ SUM(CASE WHEN datepart(weekday, fecha) = @Viernes THEN 1 ELSE 0 END)
    FROM @RangoFechas
)
 
END



GO
/****** Object:  UserDefinedFunction [dbo].[diasSabado]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create FUNCTION [dbo].[diasSabado]
(
@FechaInicio datetime,
@FechaFin datetime

)
RETURNS INT
AS
BEGIN
 
IF @FechaInicio > @FechaFin RETURN 0
 
DECLARE @RangoFechas TABLE(fecha datetime)
DECLARE @Dia datetime
DECLARE @DiaSemana tinyint=6

 
SET @Dia = @FechaInicio
 
while @Dia <= @FechaFin
BEGIN
    INSERT INTO @RangoFechas
    SELECT @Dia
 
    SET @Dia = @Dia + 1
END
 
RETURN (
    SELECT SUM(CASE WHEN datepart(weekday, fecha) = @DiaSemana THEN 1 ELSE 0 END)
    FROM @RangoFechas
)
 
END










GO
/****** Object:  UserDefinedFunction [dbo].[finesSemana]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create FUNCTION [dbo].[finesSemana]
(
@FechaInicio datetime,
@FechaFin datetime

)
RETURNS INT
AS
BEGIN
 
IF @FechaInicio > @FechaFin RETURN 0
 
DECLARE @RangoFechas TABLE(fecha datetime)
DECLARE @Dia datetime
DECLARE @DiaSemana tinyint=7
DECLARE @DiaSemana1 tinyint=6
 
SET @Dia = @FechaInicio
 
while @Dia <= @FechaFin
BEGIN
    INSERT INTO @RangoFechas
    SELECT @Dia
 
    SET @Dia = @Dia + 1
END
 
RETURN (
    SELECT SUM(CASE WHEN datepart(weekday, fecha) = @DiaSemana THEN 1 ELSE 0 END)+SUM(CASE WHEN datepart(weekday, fecha) = @DiaSemana1 THEN 1 ELSE 0 END)
    FROM @RangoFechas
)
 
END










GO
/****** Object:  Table [dbo].[Ambiente]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Ambiente](
	[IdAmbiente] [int] IDENTITY(1,1) NOT NULL,
	[IdSede] [int] NOT NULL,
	[Piso] [int] NOT NULL,
	[Numero] [varchar](10) NOT NULL,
	[IdArea] [int] NOT NULL,
	[Pantalla] [bit] NOT NULL,
	[NumeroEquipos] [int] NULL,
	[NumeroSillas] [int] NULL,
	[NumeroMesas] [int] NULL,
 CONSTRAINT [PK_Ambiente] PRIMARY KEY CLUSTERED 
(
	[IdAmbiente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Area]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Area](
	[IdArea] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [int] NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Descripcion] [varchar](500) NULL,
 CONSTRAINT [PK_Area] PRIMARY KEY CLUSTERED 
(
	[IdArea] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Competencia]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Competencia](
	[IdCompetencia] [bigint] IDENTITY(1,1) NOT NULL,
	[Codigo] [bigint] NOT NULL,
	[Horas] [int] NOT NULL,
	[IdPrograma] [int] NOT NULL,
	[Nombre] [varchar](1000) NOT NULL,
 CONSTRAINT [PK_Competencia] PRIMARY KEY CLUSTERED 
(
	[IdCompetencia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Coordinacion]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Coordinacion](
	[IdCoordinacion] [int] IDENTITY(1,1) NOT NULL,
	[Nombre_Coordinacion] [varchar](50) NOT NULL,
	[Cedula] [varchar](50) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Apellido] [varchar](50) NOT NULL,
	[IdUsuario] [int] NULL,
	[Telefono] [varchar](20) NOT NULL,
	[IdArea] [int] NULL,
	[Correo] [varchar](50) NULL,
 CONSTRAINT [PK_Coordinacion] PRIMARY KEY CLUSTERED 
(
	[IdCoordinacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Departamento]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Departamento](
	[IdDepartamento] [int] NOT NULL,
	[NombreDepartamento] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Departamento] PRIMARY KEY CLUSTERED 
(
	[IdDepartamento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Ficha]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Ficha](
	[IdFicha] [int] IDENTITY(1,1) NOT NULL,
	[IdPrograma] [int] NOT NULL,
	[Ficha1] [varchar](15) NOT NULL,
	[NumAprendices] [int] NOT NULL,
	[Estado] [bit] NULL,
	[TipoFormacion] [varchar](50) NULL,
	[FechaInicio] [date] NOT NULL,
	[FechaFin] [date] NOT NULL,
	[Jornada] [varchar](20) NULL,
 CONSTRAINT [PK_Ficha] PRIMARY KEY CLUSTERED 
(
	[IdFicha] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Ficha_Ambiente]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Ficha_Ambiente](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdFicha] [int] NOT NULL,
	[IdAmbiente] [int] NOT NULL,
	[IdInstructor] [int] NOT NULL,
	[FechaInicio] [date] NOT NULL,
	[FechaFin] [date] NOT NULL,
	[HoraInicio] [time](7) NOT NULL,
	[HoraFin] [time](7) NOT NULL,
	[Color] [varchar](10) NOT NULL,
	[IdResultado] [int] NOT NULL,
	[Estado] [bit] NOT NULL,
	[Jornada] [int] NULL,
	[DiaSemana] [int] NULL,
	[ProgramacionPrincipal] [int] NULL,
	[Lunes] [bit] NULL,
	[Martes] [bit] NULL,
	[Miercoles] [bit] NULL,
	[Jueves] [bit] NULL,
	[Viernes] [bit] NULL,
	[Transversal] [bit] NULL,
	[NombreEmpresa] [varchar](50) NULL,
 CONSTRAINT [PK_FIcha_Ambiente] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Instructor]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Instructor](
	[IdInstructor] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Apellido] [varchar](50) NOT NULL,
	[IdUsuario] [int] NULL,
	[Cedula] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Estado] [bit] NOT NULL,
	[TipoContrato] [int] NOT NULL,
	[Telefono] [varchar](50) NULL,
	[IdArea] [int] NULL,
	[TipoInstructor] [int] NULL,
	[EnvioCorreo] [bit] NULL,
 CONSTRAINT [PK_Instructor] PRIMARY KEY CLUSTERED 
(
	[IdInstructor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Municipio]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Municipio](
	[IdMunicipio] [varchar](50) NOT NULL,
	[NombreMunicipio] [varchar](100) NOT NULL,
	[IdDepartamento] [int] NOT NULL,
 CONSTRAINT [PK_Municipio] PRIMARY KEY CLUSTERED 
(
	[IdMunicipio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PrestamoAmbiente]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PrestamoAmbiente](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdAmbiente] [int] NULL,
	[Recibio] [bit] NULL,
	[Entrego] [bit] NULL,
	[Fecha] [date] NULL,
	[HoraRecibio] [time](7) NULL,
	[HoraEntrego] [time](7) NULL,
	[Observacion] [varchar](200) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PrestamoLlaves]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PrestamoLlaves](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdFicha_Ambiente] [int] NULL,
	[Recibio] [bit] NULL,
	[Entrego] [bit] NULL,
	[Fecha] [date] NULL,
	[HoraRecibio] [time](7) NULL,
	[HoraEntrego] [time](7) NULL,
	[Observacion] [varchar](200) NULL,
	[HoraRecibio2] [time](7) NULL,
	[HoraEntrego2] [time](7) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Programa]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Programa](
	[IdPrograma] [int] IDENTITY(1,1) NOT NULL,
	[CodigoPrograma] [int] NOT NULL,
	[Nivel] [varchar](50) NOT NULL,
	[LineaTecnologica] [varchar](200) NULL,
	[Red_Tecnologica] [varchar](200) NULL,
	[Perfil_Instructor] [varchar](2500) NOT NULL,
	[Version_Programa] [float] NOT NULL,
	[IdArea] [int] NOT NULL,
	[NombrePrograma] [varchar](400) NOT NULL,
 CONSTRAINT [PK_Programa] PRIMARY KEY CLUSTERED 
(
	[IdPrograma] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Resultado_Aprendizaje]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Resultado_Aprendizaje](
	[IdResultado] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [int] NOT NULL,
	[Resultado] [varchar](1000) NOT NULL,
	[IdCompetencia] [bigint] NOT NULL,
 CONSTRAINT [PK_Resultado_Aprendizaje] PRIMARY KEY CLUSTERED 
(
	[IdResultado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sede]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sede](
	[IdSede] [int] IDENTITY(1,1) NOT NULL,
	[Nombre_Sede] [varchar](50) NOT NULL,
	[Direccion] [varchar](50) NOT NULL,
	[IdMunicipio] [varchar](50) NOT NULL,
	[Codigo] [int] NULL,
	[TipoSede] [int] NULL,
	[IdUsuario] [int] NULL,
 CONSTRAINT [PK_Sede] PRIMARY KEY CLUSTERED 
(
	[IdSede] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Solicitud]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Solicitud](
	[IdSolicitud] [int] IDENTITY(1,1) NOT NULL,
	[IdFicha_Ambiente] [int] NOT NULL,
	[IdCoordinacion] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Usuario](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[NombreUsuario] [varchar](50) NOT NULL,
	[Password] [varchar](30) NOT NULL,
	[TipoUsuario] [int] NOT NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Ambiente]  WITH CHECK ADD  CONSTRAINT [FK_Ambiente_Area] FOREIGN KEY([IdArea])
REFERENCES [dbo].[Area] ([IdArea])
GO
ALTER TABLE [dbo].[Ambiente] CHECK CONSTRAINT [FK_Ambiente_Area]
GO
ALTER TABLE [dbo].[Ambiente]  WITH CHECK ADD  CONSTRAINT [FK_Ambiente_Sede] FOREIGN KEY([IdSede])
REFERENCES [dbo].[Sede] ([IdSede])
GO
ALTER TABLE [dbo].[Ambiente] CHECK CONSTRAINT [FK_Ambiente_Sede]
GO
ALTER TABLE [dbo].[Competencia]  WITH CHECK ADD  CONSTRAINT [FK_Competencia_Programa] FOREIGN KEY([IdPrograma])
REFERENCES [dbo].[Programa] ([IdPrograma])
GO
ALTER TABLE [dbo].[Competencia] CHECK CONSTRAINT [FK_Competencia_Programa]
GO
ALTER TABLE [dbo].[Coordinacion]  WITH CHECK ADD  CONSTRAINT [FK_Coordinacion_Area] FOREIGN KEY([IdArea])
REFERENCES [dbo].[Area] ([IdArea])
GO
ALTER TABLE [dbo].[Coordinacion] CHECK CONSTRAINT [FK_Coordinacion_Area]
GO
ALTER TABLE [dbo].[Coordinacion]  WITH CHECK ADD  CONSTRAINT [FK_Coordinacion_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[Coordinacion] CHECK CONSTRAINT [FK_Coordinacion_Usuario]
GO
ALTER TABLE [dbo].[Ficha]  WITH CHECK ADD  CONSTRAINT [FK_Ficha_Programa] FOREIGN KEY([IdPrograma])
REFERENCES [dbo].[Programa] ([IdPrograma])
GO
ALTER TABLE [dbo].[Ficha] CHECK CONSTRAINT [FK_Ficha_Programa]
GO
ALTER TABLE [dbo].[Ficha_Ambiente]  WITH CHECK ADD  CONSTRAINT [FK_FIcha_Ambiente_Ambiente] FOREIGN KEY([IdAmbiente])
REFERENCES [dbo].[Ambiente] ([IdAmbiente])
GO
ALTER TABLE [dbo].[Ficha_Ambiente] CHECK CONSTRAINT [FK_FIcha_Ambiente_Ambiente]
GO
ALTER TABLE [dbo].[Ficha_Ambiente]  WITH CHECK ADD  CONSTRAINT [FK_FIcha_Ambiente_Ficha] FOREIGN KEY([IdFicha])
REFERENCES [dbo].[Ficha] ([IdFicha])
GO
ALTER TABLE [dbo].[Ficha_Ambiente] CHECK CONSTRAINT [FK_FIcha_Ambiente_Ficha]
GO
ALTER TABLE [dbo].[Ficha_Ambiente]  WITH CHECK ADD  CONSTRAINT [FK_FIcha_Ambiente_Instructor] FOREIGN KEY([IdInstructor])
REFERENCES [dbo].[Instructor] ([IdInstructor])
GO
ALTER TABLE [dbo].[Ficha_Ambiente] CHECK CONSTRAINT [FK_FIcha_Ambiente_Instructor]
GO
ALTER TABLE [dbo].[Ficha_Ambiente]  WITH CHECK ADD  CONSTRAINT [FK_FIcha_Ambiente_Resultado_Aprendizaje] FOREIGN KEY([IdResultado])
REFERENCES [dbo].[Resultado_Aprendizaje] ([IdResultado])
GO
ALTER TABLE [dbo].[Ficha_Ambiente] CHECK CONSTRAINT [FK_FIcha_Ambiente_Resultado_Aprendizaje]
GO
ALTER TABLE [dbo].[Instructor]  WITH CHECK ADD  CONSTRAINT [FK_Instructor_Area1] FOREIGN KEY([IdArea])
REFERENCES [dbo].[Area] ([IdArea])
GO
ALTER TABLE [dbo].[Instructor] CHECK CONSTRAINT [FK_Instructor_Area1]
GO
ALTER TABLE [dbo].[Instructor]  WITH CHECK ADD  CONSTRAINT [FK_Instructor_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[Instructor] CHECK CONSTRAINT [FK_Instructor_Usuario]
GO
ALTER TABLE [dbo].[Municipio]  WITH CHECK ADD  CONSTRAINT [FK_Municipio_Departamento] FOREIGN KEY([IdDepartamento])
REFERENCES [dbo].[Departamento] ([IdDepartamento])
GO
ALTER TABLE [dbo].[Municipio] CHECK CONSTRAINT [FK_Municipio_Departamento]
GO
ALTER TABLE [dbo].[Programa]  WITH CHECK ADD  CONSTRAINT [FK_Programa_Area] FOREIGN KEY([IdArea])
REFERENCES [dbo].[Area] ([IdArea])
GO
ALTER TABLE [dbo].[Programa] CHECK CONSTRAINT [FK_Programa_Area]
GO
ALTER TABLE [dbo].[Resultado_Aprendizaje]  WITH CHECK ADD  CONSTRAINT [FK_Resultado_Aprendizaje_Competencia] FOREIGN KEY([IdCompetencia])
REFERENCES [dbo].[Competencia] ([IdCompetencia])
GO
ALTER TABLE [dbo].[Resultado_Aprendizaje] CHECK CONSTRAINT [FK_Resultado_Aprendizaje_Competencia]
GO
ALTER TABLE [dbo].[Sede]  WITH CHECK ADD  CONSTRAINT [FK_Sede_Municipio] FOREIGN KEY([IdMunicipio])
REFERENCES [dbo].[Municipio] ([IdMunicipio])
GO
ALTER TABLE [dbo].[Sede] CHECK CONSTRAINT [FK_Sede_Municipio]
GO
ALTER TABLE [dbo].[Sede]  WITH CHECK ADD  CONSTRAINT [FK_Sede_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[Sede] CHECK CONSTRAINT [FK_Sede_Usuario]
GO
/****** Object:  StoredProcedure [dbo].[AmbientesDisponibles]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[AmbientesDisponibles]
(
 @fecha_ini date, @fecha_Fin date, @HoraInicio time,@HoraFin time, @IdArea int, @opc int, 
 @lunes bit, @martes bit, @miercoles bit, @jueves bit, @viernes bit
)

as
begin
if (@opc=1)
begin

		Select a.* from Ambiente a
		 join Sede s on a.IdSede = s.IdSede
		 where not exists 
			  (select f.IdAmbiente from Ficha_Ambiente f 
				 where f.IdAmbiente = a.IdAmbiente and ((f.FechaInicio between @fecha_ini and @fecha_Fin ) or
							(f.FechaFin between @fecha_ini and @fecha_Fin) or 
							(f.FechaInicio <=  @fecha_ini and f.FechaFin>= @fecha_Fin)) and
						    (f.Lunes =@lunes  or f.Martes =@martes  or f.Miercoles =@miercoles  or f.Jueves =@jueves or f.Viernes = @viernes)) 
		and((s.TipoSede =1))
		   
	
		
		order by a.IdSede

end

if (@opc=2)
begin

		Select a.* from Ambiente a
		 join Sede s on a.IdSede = s.IdSede
		 where not exists 
			  (select f.IdAmbiente from Ficha_Ambiente f 
				 where f.IdAmbiente = a.IdAmbiente and ((f.FechaInicio between @fecha_ini and @fecha_Fin ) or
							(f.FechaFin between @fecha_ini and @fecha_Fin) or 
							(f.FechaInicio <=  @fecha_ini and f.FechaFin>= @fecha_Fin)) and
							(f.Lunes =@lunes  or f.Martes =@martes  or f.Miercoles =@miercoles  or f.Jueves =@jueves or f.Viernes = @viernes))
		 and((s.TipoSede =1)) and a.IdArea = @IdArea
		   
	
		
		order by a.IdSede

end


if(@opc=3)
begin 


					
					Select * from Ambiente a 
					join Sede s on a.IdSede = s.IdSede
					where not exists 
						(select IdAmbiente from Ficha_Ambiente f 
							where f.IdAmbiente = a.IdAmbiente and ((f.FechaInicio between @fecha_ini and @fecha_Fin ) or
										(f.FechaFin between @fecha_ini and @fecha_Fin) or 
										(f.FechaInicio <=  @fecha_ini and f.FechaFin>= @fecha_Fin))and
										((f.HoraInicio >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and f.HoraInicio <= convert(time,@HoraFin) )or 
										(f.HoraFin >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and f.HoraFin <= convert(time,@HoraFin)) or
										(f.HoraInicio <= dateadd(MINUTE,1,convert(time,@HoraInicio)) and f.HoraFin>= convert(time,@HoraFin))) and
										(f.Lunes =@lunes  or f.Martes =@martes  or f.Miercoles =@miercoles  or f.Jueves =@jueves or f.Viernes = @viernes)) 
					and((s.TipoSede =1))

					order by a.IdSede
								
end 

if(@opc=4)
begin 


					Select * from Ambiente a
					join Sede s on a.IdSede = s.IdSede
					 where not exists 
						(select IdAmbiente from Ficha_Ambiente f 
							where f.IdAmbiente = a.IdAmbiente and ((f.FechaInicio between @fecha_ini and @fecha_Fin ) or
										(f.FechaFin between @fecha_ini and @fecha_Fin) or 
										(f.FechaInicio <=  @fecha_ini and f.FechaFin>= @fecha_Fin))and
										((f.HoraInicio >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and f.HoraInicio <= convert(time,@HoraFin) )or 
										(f.HoraFin >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and f.HoraFin <= convert(time,@HoraFin)) or
										(f.HoraInicio <= dateadd(MINUTE,1,convert(time,@HoraInicio)) and f.HoraFin>= convert(time,@HoraFin))) and
										(f.Lunes =@lunes  or f.Martes =@martes  or f.Miercoles =@miercoles  or f.Jueves =@jueves or f.Viernes = @viernes)) 
					and ((s.TipoSede =1)) and a.IdArea = @IdArea

					order by a.IdSede
								
end 




end









GO
/****** Object:  StoredProcedure [dbo].[AmbientesLibres]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[AmbientesLibres]
(
 @fecha_ini date, @fecha_Fin date, @HoraInicio time,@HoraFin time, 
 @lunes bit, @martes bit, @miercoles bit, @jueves bit, @viernes bit, @jornada int, @IdSede int  
)


--declare @fecha_ini as date = '2018-04-18'
--declare @fecha_Fin as date = '2018-04-19'
--declare @HoraInicio as time = '10:00'
--declare @HoraFin as time = '15:00'

as


begin

--Select a.*, ar.IdArea,ar.Nombre as Area,a.Numero as Ambiente, s.Nombre_Sede as NombreSede from Ambiente a
--join Sede s on a.IdSede = s.IdSede
--join Area ar on a.IdArea = ar.IdArea
--where a.IdSede =1 and not exists 
--	(select IdAmbiente from Ficha_Ambiente f 
--		where f.IdAmbiente = a.IdAmbiente and ((f.FechaInicio between @fecha_ini and @fecha_Fin ) or
--					(f.FechaFin between @fecha_ini and @fecha_Fin) or 
--					(f.FechaInicio <=  @fecha_ini and f.FechaFin>= @fecha_Fin))and
--					((f.HoraInicio >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and f.HoraInicio <= DATEADD(minute,-1,convert(time,@HoraFin)))or 
--					(f.HoraFin >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and f.HoraFin <= DATEADD(minute,-1,convert(time,@HoraFin))) or
--					(f.HoraInicio <= dateadd(MINUTE,1,convert(time,@HoraInicio)) and f.HoraFin>= DATEADD(minute,-1,convert(time,@HoraFin)))) and
--					(f.Lunes =@lunes  or f.Martes =@martes  or f.Miercoles =@miercoles  or f.Jueves =@jueves or f.Viernes = @viernes or f.Jornada=@jornada)) 
					

--order by a.IdSede
	--Select a.*, ar.IdArea,ar.Nombre as Area,a.Numero as Ambiente, s.Nombre_Sede as NombreSede from Ambiente a
	--				join Sede s on a.IdSede = s.IdSede
	--				join Area ar on a.IdArea = ar.IdArea
	--				where a.IdSede =1 and not exists 
	--					(select IdAmbiente from Ficha_Ambiente f 
	--						where f.IdAmbiente = a.IdAmbiente and ((f.FechaInicio between @fecha_ini and @fecha_Fin ) or
	--									(f.FechaFin between @fecha_ini and @fecha_Fin) or 
	--									(f.FechaInicio <=  @fecha_ini and f.FechaFin>= @fecha_Fin))and
	--									((f.HoraInicio >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and f.HoraInicio <= DATEADD(minute,-1,convert(time,@HoraFin)))or 
	--									(f.HoraFin >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and f.HoraFin <= DATEADD(minute,-1,convert(time,@HoraFin))) or
	--									(f.HoraInicio <= dateadd(MINUTE,1,convert(time,@HoraInicio)) and f.HoraFin>= DATEADD(minute,-1,convert(time,@HoraFin)))) and
	--									(f.Lunes =@lunes  or f.Martes =@martes  or f.Miercoles =@miercoles  or f.Jueves =@jueves or f.Viernes = @viernes or f.Jornada=@jornada)) 
	--									and not exists  (select IdAmbiente from PrestamoAmbiente p 
	--														where p.IdAmbiente = a.IdAmbiente and ((p.Fecha = @fecha_ini ) and
	--														((p.HoraRecibio >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and p.HoraRecibio <= DATEADD(minute,-1,convert(time,@HoraFin)))or 
	--														(p.HoraEntrego >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and p.HoraEntrego <= DATEADD(minute,-1,convert(time,@HoraFin))) or
	--														(p.HoraRecibio <= dateadd(MINUTE,1,convert(time,@HoraInicio)) and p.HoraEntrego>= DATEADD(minute,-1,convert(time,@HoraFin))))))
					
	IF @IdSede =0
	 BEGIN
	Select a.*, ar.IdArea,ar.Nombre as Area,a.Numero as Ambiente, s.Nombre_Sede as NombreSede from Ambiente a
	join Sede s on a.IdSede = s.IdSede
	join Area ar on a.IdArea = ar.IdArea
	where not exists 
		(select IdAmbiente from Ficha_Ambiente f 
			where f.IdAmbiente = a.IdAmbiente and ((f.FechaInicio between @fecha_ini and @fecha_Fin ) or
						(f.FechaFin between @fecha_ini and @fecha_Fin) or 
						(f.FechaInicio <=  @fecha_ini and f.FechaFin>= @fecha_Fin))and
						((f.HoraInicio >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and f.HoraInicio <= DATEADD(minute,-1,convert(time,@HoraFin)))or 
						(f.HoraFin >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and f.HoraFin <= DATEADD(minute,-1,convert(time,@HoraFin))) or
						(f.HoraInicio <= dateadd(MINUTE,1,convert(time,@HoraInicio)) and f.HoraFin>= DATEADD(minute,-1,convert(time,@HoraFin)))) and
						(f.Lunes =@lunes  or f.Martes =@martes  or f.Miercoles =@miercoles  or f.Jueves =@jueves or f.Viernes = @viernes or f.Jornada=@jornada)) order by s.Nombre_Sede 
END

ELSE
BEGIN
Select a.*, ar.IdArea,ar.Nombre as Area,a.Numero as Ambiente, s.Nombre_Sede as NombreSede from Ambiente a
	join Sede s on a.IdSede = s.IdSede
	join Area ar on a.IdArea = ar.IdArea
	where a.IdSede =@IdSede and not exists 
		(select IdAmbiente from Ficha_Ambiente f 
			where f.IdAmbiente = a.IdAmbiente and ((f.FechaInicio between @fecha_ini and @fecha_Fin ) or
						(f.FechaFin between @fecha_ini and @fecha_Fin) or 
						(f.FechaInicio <=  @fecha_ini and f.FechaFin>= @fecha_Fin))and
						((f.HoraInicio >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and f.HoraInicio <= DATEADD(minute,-1,convert(time,@HoraFin)))or 
						(f.HoraFin >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and f.HoraFin <= DATEADD(minute,-1,convert(time,@HoraFin))) or
						(f.HoraInicio <= dateadd(MINUTE,1,convert(time,@HoraInicio)) and f.HoraFin>= DATEADD(minute,-1,convert(time,@HoraFin)))) and
						(f.Lunes =@lunes  or f.Martes =@martes  or f.Miercoles =@miercoles  or f.Jueves =@jueves or f.Viernes = @viernes or f.Jornada=@jornada)) order by  s.Nombre_Sede 
end
END
GO
/****** Object:  StoredProcedure [dbo].[HorasInstructorCoordinacion]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[HorasInstructorCoordinacion]
( @fecha_ini date, @fecha_Fin date, @IdArea int)
AS

create table Instructor2
( IdInstructor int,
Nombre varchar(50),
Apellido varchar(50),
IdUsuario int,
Cedula varchar(50),
Email varchar(50),
Estado bit,
TipoContrato int,
Telefono varchar(50),
IdArea int,
TipoInstructor int,
EnvioCorreo bit,
HorasRango float,
Total float,
Jornada int,
)
	--------Consulta para determinar las horas trabajadas en un mes
		insert into Instructor2 (IdInstructor,Nombre,Apellido,IdUsuario,Cedula,Email,Estado,TipoContrato,Telefono, IdArea, TipoInstructor,EnvioCorreo,HorasRango,Total,Jornada)

			select i.*,t.HorasRango,t.Total,t.Jornada from 
					 (select f.*, ((cast(DATEDIFF(MINUTE,f.HoraInicio,f.HoraFin) as decimal)/60) * (dbo.DiasLaborados(convert(date,@fecha_ini),convert(date,@fecha_Fin),f.Lunes,f.Martes,f.Miercoles,f.Jueves,f.Viernes))) as  'HorasRango',
								  ((cast(DATEDIFF(MINUTE,f.HoraInicio,f.HoraFin) as decimal)/60) * (dbo.DiasLaborados(convert(date,f.FechaInicio),convert(date,f.FechaFin),f.Lunes,f.Martes,f.Miercoles,f.Jueves,f.Viernes))) as 'Total' from Ficha_Ambiente f 
										 where ( (f.FechaInicio between @fecha_ini and @fecha_Fin ) or
												 (f.FechaFin between @fecha_ini and @fecha_Fin) or 
										         (f.FechaInicio <=  @fecha_ini and f.FechaFin>= @fecha_Fin)) and
										          f.Jornada=1) t 
				 
		
				 
			join Instructor i on t.IdInstructor = i.IdInstructor
			where i.IdArea = @IdArea
	


		insert into Instructor2 (IdInstructor,Nombre,Apellido,IdUsuario,Cedula,Email,Estado,TipoContrato,Telefono, IdArea, TipoInstructor,EnvioCorreo,HorasRango,Total,Jornada)			
		  
		  select i.*,t.HorasRango,t.Total,t.Jornada from  
					(select f.*, (DATEDIFF( HH , f.HoraInicio , f.HoraFin ) * (dbo.diasSabado(convert(date,@fecha_ini),convert(date,@fecha_Fin)))) as 'HorasRango',
								 (DATEDIFF( HH , f.HoraInicio , f.HoraFin ) * (dbo.diasSabado(convert(date,f.FechaInicio),convert(date,f.FechaFin)))) as 'Total' from Ficha_Ambiente f 
										where ((f.FechaInicio between @fecha_ini and @fecha_Fin ) or
											   (f.FechaFin between @fecha_ini and @fecha_Fin) or 
											   (f.FechaInicio <=  @fecha_ini and f.FechaFin>= @fecha_Fin)) and 
												  f.Jornada=2) t
				 
		join Instructor i on t.IdInstructor = i.IdInstructor
		where i.IdArea = @IdArea

			insert into Instructor2 (IdInstructor,Nombre,Apellido,IdUsuario,Cedula,Email,Estado,TipoContrato,Telefono, IdArea, TipoInstructor,EnvioCorreo,HorasRango,Total,Jornada)
			
			select i.*,t.HorasRango,t.Total,t.Jornada  from  
					 (select f.*, (DATEDIFF( HH , f.HoraInicio , f.HoraFin ) * (dbo.diasDomingo(convert(date,@fecha_ini),convert(date,@fecha_Fin)))) as 'HorasRango',
								  (DATEDIFF( HH , f.HoraInicio , f.HoraFin ) * (dbo.diasDomingo(convert(date,f.FechaInicio),convert(date,f.FechaFin)))) as 'Total' from Ficha_Ambiente f 
										 where ( (f.FechaInicio between @fecha_ini and @fecha_Fin ) or
												 (f.FechaFin between @fecha_ini and @fecha_Fin) or 
												 (f.FechaInicio <=  @fecha_ini and f.FechaFin>= @fecha_Fin)) and
										            f.Jornada=3) t
				 
			join Instructor i on t.IdInstructor = i.IdInstructor
			where i.IdArea = @IdArea


		select IdInstructor,Nombre,Apellido,IdUsuario,Cedula,Email,Estado,TipoContrato,Telefono, IdArea, TipoInstructor,sum(HorasRango) as HorasRango, sum(Total) as Total from Instructor2 where Total is not null group by IdInstructor,Nombre,Apellido,IdUsuario,Cedula,Email,Estado,TipoContrato,Telefono, IdArea, TipoInstructor,EnvioCorreo, HorasRango,Total,Jornada order by Jornada asc

		drop table Instructor2




GO
/****** Object:  StoredProcedure [dbo].[HorasInstructorTodos]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[HorasInstructorTodos]
( @fecha_ini date, @fecha_Fin date)
AS

create table Instructor2
( IdInstructor int,
Nombre varchar(50),
Apellido varchar(50),
IdUsuario int,
Cedula varchar(50),
Email varchar(50),
Estado bit,
TipoContrato int,
Telefono varchar(50),
IdArea int,
TipoInstructor int,
EnvioCorreo bit,
HorasRango float,
Total float,
Jornada int,
)
	--------Consulta para determinar las horas trabajadas en un mes
		insert into Instructor2 (IdInstructor,Nombre,Apellido,IdUsuario,Cedula,Email,Estado,TipoContrato,Telefono, IdArea, TipoInstructor,EnvioCorreo,HorasRango,Total,Jornada)

			select i.*,t.HorasRango,t.Total,t.Jornada from 
					 (select f.*, ((cast(DATEDIFF(MINUTE,f.HoraInicio,f.HoraFin) as decimal)/60) * (dbo.DiasLaborados(convert(date,@fecha_ini),convert(date,@fecha_Fin),f.Lunes,f.Martes,f.Miercoles,f.Jueves,f.Viernes))) as  'HorasRango',
								  ((cast(DATEDIFF(MINUTE,f.HoraInicio,f.HoraFin) as decimal)/60) * (dbo.DiasLaborados(convert(date,f.FechaInicio),convert(date,f.FechaFin),f.Lunes,f.Martes,f.Miercoles,f.Jueves,f.Viernes))) as 'Total' from Ficha_Ambiente f 
										 where ( (f.FechaInicio between @fecha_ini and @fecha_Fin ) or
												 (f.FechaFin between @fecha_ini and @fecha_Fin) or 
										         (f.FechaInicio <=  @fecha_ini and f.FechaFin>= @fecha_Fin)) and
										          f.Jornada=1) t 
				 
		
				 
			join Instructor i on t.IdInstructor = i.IdInstructor
			
	


		insert into Instructor2 (IdInstructor,Nombre,Apellido,IdUsuario,Cedula,Email,Estado,TipoContrato,Telefono, IdArea, TipoInstructor,EnvioCorreo,HorasRango,Total,Jornada)			
		  
		  select i.*,t.HorasRango,t.Total,t.Jornada from  
					(select f.*, (DATEDIFF( HH , f.HoraInicio , f.HoraFin ) * (dbo.diasSabado(convert(date,@fecha_ini),convert(date,@fecha_Fin)))) as 'HorasRango',
								 (DATEDIFF( HH , f.HoraInicio , f.HoraFin ) * (dbo.diasSabado(convert(date,f.FechaInicio),convert(date,f.FechaFin)))) as 'Total' from Ficha_Ambiente f 
										where ((f.FechaInicio between @fecha_ini and @fecha_Fin ) or
											   (f.FechaFin between @fecha_ini and @fecha_Fin) or 
											   (f.FechaInicio <=  @fecha_ini and f.FechaFin>= @fecha_Fin)) and 
												  f.Jornada=2) t
				 
		join Instructor i on t.IdInstructor = i.IdInstructor
	

			insert into Instructor2 (IdInstructor,Nombre,Apellido,IdUsuario,Cedula,Email,Estado,TipoContrato,Telefono, IdArea, TipoInstructor,EnvioCorreo,HorasRango,Total,Jornada)
			
			select i.*,t.HorasRango,t.Total,t.Jornada  from  
					 (select f.*, (DATEDIFF( HH , f.HoraInicio , f.HoraFin ) * (dbo.diasDomingo(convert(date,@fecha_ini),convert(date,@fecha_Fin)))) as 'HorasRango',
								  (DATEDIFF( HH , f.HoraInicio , f.HoraFin ) * (dbo.diasDomingo(convert(date,f.FechaInicio),convert(date,f.FechaFin)))) as 'Total' from Ficha_Ambiente f 
										 where ( (f.FechaInicio between @fecha_ini and @fecha_Fin ) or
												 (f.FechaFin between @fecha_ini and @fecha_Fin) or 
												 (f.FechaInicio <=  @fecha_ini and f.FechaFin>= @fecha_Fin)) and
										            f.Jornada=3) t
				 
			join Instructor i on t.IdInstructor = i.IdInstructor



		select IdInstructor,Nombre,Apellido,IdUsuario,Cedula,Email,Estado,TipoContrato,Telefono, IdArea, TipoInstructor,sum(HorasRango) as HorasRango, sum(Total) as Total from Instructor2 where Total is not null group by IdInstructor,Nombre,Apellido,IdUsuario,Cedula,Email,Estado,TipoContrato,Telefono, IdArea, TipoInstructor,EnvioCorreo, HorasRango,Total,Jornada order by Jornada asc

		drop table Instructor2



GO
/****** Object:  StoredProcedure [dbo].[sp_ConsultarDiaTransversalFicha]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_ConsultarDiaTransversalFicha]
( @fecha_ini date, @fecha_Fin date,  @ficha int)
AS



	begin 

			

				select  * from Ficha_Ambiente f 
				where ( (f.FechaInicio between @fecha_ini and  @fecha_Fin ) or
						(f.FechaFin between @fecha_ini and  @fecha_Fin) or 
						(f.FechaInicio <= @fecha_ini and f.FechaFin>=  @fecha_Fin)) and f.IdFicha = @ficha
		

			
	end
















GO
/****** Object:  StoredProcedure [dbo].[sp_disponibilidadAmbiente]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_disponibilidadAmbiente]
( @fecha_ini date, @fecha_Fin date, @HoraInicio varchar(10),@HoraFin varchar(10), @idarea int, @idsede int, @lunes bit, @martes bit, 
@miercoles bit, @jueves bit, @viernes bit)
AS


create table Ambiente1
( IdAmbiente int,
IdSede int,
Piso int,
Numero varchar(10),
IdArea int,
Pantalla bit,
NumeroEquipos int,
NumeroSillas int,
NumeroMesas int
)

declare @vardate date = @fecha_ini


while @vardate <= @fecha_Fin
	begin 

		if ((datepart(dw,@vardate) < 6))
			begin

			

			insert into Ambiente1 (IdAmbiente,IdSede,Piso,Numero,IdArea,Pantalla,NumeroEquipos,NumeroSillas,NumeroMesas)
				select  i.* from Ficha_Ambiente f 
				join Ambiente i  on f.IdAmbiente = i.IdAmbiente 
				join Area a on i.IdArea = a.IdArea
				join Sede s on i.IdSede = s.IdSede
				where ( (f.FechaInicio between @vardate and @vardate ) or
						(f.FechaFin between @vardate and @vardate) or 
						(f.FechaInicio <=  @vardate and f.FechaFin>= @vardate))and
						((HoraInicio >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraInicio <= DATEADD(minute,-1,convert(time,@HoraFin)))or 
						(HoraFin >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraFin <= convert(time,@HoraFin)) or
						(f.HoraInicio <= dateadd(MINUTE,1,convert(time,@HoraInicio)) and f.HoraFin>= convert(time,@HoraFin)) 
						) and f.Jornada = 1 and i.IdArea = @idarea and s.IdSede = @idsede and (f.Lunes = @lunes or f.Martes = @martes or f.Miercoles = @miercoles
						 or f.Jueves = @jueves or f.Viernes = @viernes) 
			end

			set @vardate = DATEADD(DAY,1,@vardate)
	end

	select distinct * from Ambiente1

	drop table Ambiente1



GO
/****** Object:  StoredProcedure [dbo].[sp_disponibilidadAmbienteAreasAlternas]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_disponibilidadAmbienteAreasAlternas]
( @fecha_ini date, @fecha_Fin date, @HoraInicio varchar(10),@HoraFin varchar(10),@lunes bit, @martes bit, 
@miercoles bit, @jueves bit, @viernes bit, @IdSede int)
As
begin

 Select * from Ambiente a where not exists 
		(select IdAmbiente from Ficha_Ambiente f 
			where f.IdAmbiente = a.IdAmbiente and ((f.FechaInicio between @fecha_ini and @fecha_Fin ) or
						(f.FechaFin between @fecha_ini and @fecha_Fin) or 
						(f.FechaInicio <=  @fecha_ini and f.FechaFin>= @fecha_Fin))and
						((HoraInicio >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraInicio <= DATEADD(minute,-1,convert(time,@HoraFin)))or 
						(HoraFin >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraFin <= convert(time,@HoraFin)) or
						(f.HoraInicio <= dateadd(MINUTE,1,convert(time,@HoraInicio)) and f.HoraFin>= convert(time,@HoraFin)) 
						) and (f.Lunes = @lunes or f.Martes = @martes or f.Miercoles = @miercoles
						 or f.Jueves = @jueves or f.Viernes = @viernes)) and a.IdSede = @IdSede
												
												
end											
				



GO
/****** Object:  StoredProcedure [dbo].[sp_disponibilidadAmbienteSabado]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_disponibilidadAmbienteSabado]
( @fecha_ini date, @fecha_Fin date, @HoraInicio varchar(10),@HoraFin varchar(10), @jornada int)
AS


create table Ambiente1
( IdAmbiente int,
IdSede int,
Piso int,
Numero varchar(10),
IdArea int,
Pantalla bit,
NumeroEquipos int,
NumeroSillas int,
NumeroMesas int
)

declare @vardate date = @fecha_ini

while @vardate <= @fecha_Fin
	begin 

		if ((datepart(dw,@vardate) >= 6))
			begin

			

			insert into Ambiente1 (IdAmbiente,IdSede,Piso,Numero,IdArea,Pantalla,NumeroEquipos,NumeroSillas,NumeroMesas)
				select  i.* from Ficha_Ambiente f 
				join Ambiente i  on f.IdAmbiente = i.IdAmbiente
				join Area a on i.IdArea = a.IdArea 
				where ( (f.FechaInicio between @vardate and @vardate ) or
						(f.FechaFin between @vardate and @vardate) or 
						(f.FechaInicio <=  @vardate and f.FechaFin>= @vardate))and
						((HoraInicio >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraInicio <= DATEADD(minute,-1,convert(time,@HoraFin)))or 
						(HoraFin >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraFin <= convert(time,@HoraFin)) or
						(f.HoraInicio <= dateadd(MINUTE,1,convert(time,@HoraInicio)) and f.HoraFin>= convert(time,@HoraFin)) 
						) and f.Jornada = @jornada 
			end

			set @vardate = DATEADD(DAY,1,@vardate)
	end

	select distinct * from Ambiente1

	drop table Ambiente1

















GO
/****** Object:  StoredProcedure [dbo].[sp_disponibilidadFicha]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_disponibilidadFicha]
( @fecha_ini date, @fecha_Fin date, @HoraInicio varchar(10),@HoraFin varchar(10), @idarea int, @idprograma int, @lunes bit, @martes bit, 
@miercoles bit, @jueves bit, @viernes bit)
AS

create table Ficha2
( IdFicha int,
IdPrograma int,
Ficha1 varchar(15),
NumAprendices int,
Estado bit,
TipoFormacion varchar(50),
FechaInicio date,
FechaFin date,
Jornada varchar(20)
)

declare @vardate date = @fecha_ini

while @vardate <= @fecha_Fin
	begin 

		if ((datepart(dw,@vardate) < 6))
			begin

			



			insert into Ficha2 (IdFicha,IdPrograma,Ficha1,NumAprendices,Estado,TipoFormacion,FechaInicio,FechaFin,Jornada)
				select  i.* from Ficha_Ambiente f 
				join Ficha i  on f.IdFicha = i.IdFicha 
				join Programa p on i.IdPrograma = p.IdPrograma
				join Area a on p.IdArea = a.IdArea
				where ( (f.FechaInicio between @vardate and @vardate ) or
						(f.FechaFin between @vardate and @vardate) or 
						(f.FechaInicio <=  @vardate and f.FechaFin>= @vardate))and
						((HoraInicio >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraInicio <= DATEADD(minute,-1,convert(time,@HoraFin)))or 
						(HoraFin >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraFin <= convert(time,@HoraFin)) or
						(f.HoraInicio <=dateadd(MINUTE,1, convert(time,@HoraInicio)) and f.HoraFin>= convert(time,@HoraFin)) 
						) and f.Jornada = 1 and a.IdArea = @idarea and p.IdPrograma = @idprograma 
						--and (i.FechaInicio <=@fecha_ini and i.FechaFin>= @fecha_Fin) 
						and (f.Lunes = @lunes or f.Martes = @martes or f.Miercoles = @miercoles
						 or f.Jueves = @jueves or f.Viernes = @viernes)

			end

			set @vardate = DATEADD(DAY,1,@vardate)
	end

	select distinct * from Ficha2

	drop table Ficha2





GO
/****** Object:  StoredProcedure [dbo].[sp_disponibilidadFichaSabado]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_disponibilidadFichaSabado]
( @fecha_ini date, @fecha_Fin date, @HoraInicio varchar(10),@HoraFin varchar(10), @idarea int, @jornada int)
AS

create table Ficha2
( IdFicha int,
IdPrograma int,
Ficha1 varchar(15),
NumAprendices int,
Estado bit,
TipoFormacion varchar(50),
FechaInicio date,
FechaFin date,
Jornada varchar(20)
)

declare @vardate date = @fecha_ini

while @vardate <= @fecha_Fin
	begin 

		if ((datepart(dw,@vardate) >= 6))
			begin

			

			insert into Ficha2 (IdFicha,IdPrograma,Ficha1,NumAprendices,Estado,TipoFormacion,FechaInicio,FechaFin,Jornada)
				select  i.* from Ficha_Ambiente f 
				join Ficha i  on f.IdFicha = i.IdFicha 
				join Programa p on i.IdPrograma = p.IdPrograma
				join Area a on p.IdArea = a.IdArea
				where ( (f.FechaInicio between @vardate and @vardate ) or
						(f.FechaFin between @vardate and @vardate) or 
						(f.FechaInicio <=  @vardate and f.FechaFin>= @vardate))and
						((HoraInicio >= dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraInicio <= DATEADD(minute,-1,convert(time,@HoraFin)))or 
						(HoraFin >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraFin <= convert(time,@HoraFin)) or
						(f.HoraInicio <= dateadd(MINUTE,1,convert(time,@HoraInicio)) and f.HoraFin>= convert(time,@HoraFin)) 
						) and f.Jornada = @jornada and a.IdArea = @idarea and (i.FechaInicio <=@fecha_ini and i.FechaFin>= @fecha_Fin
						 )

			end

			set @vardate = DATEADD(DAY,1,@vardate)
	end

	select distinct * from Ficha2

	drop table Ficha2

















GO
/****** Object:  StoredProcedure [dbo].[sp_disponibilidadInstructor]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_disponibilidadInstructor]
( @fecha_ini date, @fecha_Fin date, @HoraInicio varchar(10),@HoraFin varchar(10), @idarea int, @lunes bit, @martes bit, 
@miercoles bit, @jueves bit, @viernes bit)
AS


create table Instructor1
( IdInstructor int,
Nombre varchar(50),
Apellido varchar(50),
IdUsuario int,
Cedula varchar(50),
Email varchar(50),
Estado bit,
TipoContrato int,
Telefono varchar(50),
IdArea int,
TipoInstructor int,
EnvioCorreo bit
)

declare @vardate date = @fecha_ini
declare @minuto time = '00:01:00'

while @vardate <= @fecha_Fin
	begin 

		if ((datepart(dw,@vardate) < 6))
			begin

			

			insert into Instructor1 (IdInstructor,Nombre,Apellido,IdUsuario,Cedula,Email,Estado,TipoContrato,Telefono, IdArea, TipoInstructor,EnvioCorreo)
				select  i.* from Ficha_Ambiente f 
				join Instructor i  on f.IdInstructor = i.IdInstructor 
		
				join Area a on i.IdArea = a.IdArea
				where ( (f.FechaInicio between @vardate and @vardate ) or
						(f.FechaFin between @vardate and @vardate) or 
						(f.FechaInicio <=  @vardate and f.FechaFin>= @vardate))and
						((HoraInicio >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraInicio <= DATEADD(minute,-1,convert(time,@HoraFin)))or 
						(HoraFin >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraFin <= convert(time,@HoraFin)) or
						(f.HoraInicio <= dateadd(MINUTE,1,convert(time,@HoraInicio)) and f.HoraFin>= convert(time,@HoraFin)) 
						) and f.Jornada = 1 and a.IdArea = @idarea and (f.Lunes = @lunes or f.Martes = @martes or f.Miercoles = @miercoles
						 or f.Jueves = @jueves or f.Viernes = @viernes) 
			end

			set @vardate = DATEADD(DAY,1,@vardate)
	end

	select distinct * from Instructor1

	drop table Instructor1











GO
/****** Object:  StoredProcedure [dbo].[sp_disponibilidadInstructorSabado]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_disponibilidadInstructorSabado]
( @fecha_ini date, @fecha_Fin date, @HoraInicio varchar(10),@HoraFin varchar(10), @idarea int, @jornada int)
AS


create table Instructor1
( IdInstructor int,
Nombre varchar(50),
Apellido varchar(50),
IdUsuario int,
Cedula varchar(50),
Email varchar(50),
Estado bit,
TipoContrato int,
Telefono varchar(50),
IdArea int,
TipoInstructor int,
EnvioCorreo bit
)

declare @vardate date = @fecha_ini

while @vardate <= @fecha_Fin
	begin 

		if ((datepart(dw,@vardate) >= 6))
			begin

			

			insert into Instructor1 (IdInstructor,Nombre,Apellido,IdUsuario,Cedula,Email,Estado,TipoContrato,Telefono, IdArea, TipoInstructor, EnvioCorreo)
				select  i.* from Ficha_Ambiente f 
				join Instructor i  on f.IdInstructor = i.IdInstructor
				
				join Area a on i.IdArea = a.IdArea
				where ( (f.FechaInicio between @vardate and @vardate ) or
						(f.FechaFin between @vardate and @vardate) or 
						(f.FechaInicio <=  @vardate and f.FechaFin>= @vardate))and
						((HoraInicio >= dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraInicio <= DATEADD(minute,-1,convert(time,@HoraFin)))or 
						(HoraFin >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraFin <= convert(time,@HoraFin)) or
						(f.HoraInicio <= dateadd(MINUTE,1,convert(time,@HoraInicio)) and f.HoraFin>= convert(time,@HoraFin)) 
						) and f.Jornada = @jornada  and a.IdArea = @idarea
			end

			set @vardate = DATEADD(DAY,1,@vardate)
	end

	select distinct * from Instructor1

	drop table Instructor1

















GO
/****** Object:  StoredProcedure [dbo].[sp_disponibilidadInstructorTransversal]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_disponibilidadInstructorTransversal]
( @fecha_ini date, @fecha_Fin date, @HoraInicio varchar(10),@HoraFin varchar(10), @diasemana int)
AS


create table Instructor1
( IdInstructor int,
Nombre varchar(50),
Apellido varchar(50),
IdUsuario int,
Cedula varchar(50),
Email varchar(50),
Estado bit,
TipoContrato int,
Telefono varchar(50),
IdArea int,
TipoInstructor int,
EnvioCorreo bit
)

declare @vardate date = @fecha_ini

while @vardate <= @fecha_Fin
	begin 

		if ((datepart(dw,@vardate) < 6))
			begin

			

			insert into Instructor1 (IdInstructor,Nombre,Apellido,IdUsuario,Cedula,Email,Estado,TipoContrato,Telefono, IdArea,TipoInstructor,EnvioCorreo)
				select  i.* from Ficha_Ambiente f 
				join Instructor i  on f.IdInstructor = i.IdInstructor 
				
				where ( (f.FechaInicio between @vardate and @vardate ) or
						(f.FechaFin between @vardate and @vardate) or 
						(f.FechaInicio <=  @vardate and f.FechaFin>= @vardate))and
						((HoraInicio >= dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraInicio <= DATEADD(minute,-1,convert(time,@HoraFin)))or 
						(HoraFin >= dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraFin <= convert(time,@HoraFin)) or
						(f.HoraInicio <= dateadd(MINUTE,1,convert(time,@HoraInicio)) and f.HoraFin>= convert(time,@HoraFin)) 
						) and f.Jornada = 1 and i.TipoInstructor = 2 and f.DiaSemana = @diasemana
			end

			set @vardate = DATEADD(DAY,1,@vardate)
	end

	select distinct * from Instructor1

	drop table Instructor1



	















GO
/****** Object:  StoredProcedure [dbo].[sp_disponibilidadInstructorTransversalDias]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_disponibilidadInstructorTransversalDias]
( @fecha_ini date, @fecha_Fin date, @HoraInicio varchar(10),@HoraFin varchar(10),@lunes bit, @martes bit, 
@miercoles bit, @jueves bit, @viernes bit)
AS


create table Instructor1
( IdInstructor int,
Nombre varchar(50),
Apellido varchar(50),
IdUsuario int,
Cedula varchar(50),
Email varchar(50),
Estado bit,
TipoContrato int,
Telefono varchar(50),
IdArea int,
TipoInstructor int,
EnvioCorreo bit
)

declare @vardate date = @fecha_ini

while @vardate <= @fecha_Fin
	begin 

		if ((datepart(dw,@vardate) < 6))
			begin

			

			insert into Instructor1 (IdInstructor,Nombre,Apellido,IdUsuario,Cedula,Email,Estado,TipoContrato,Telefono, IdArea,TipoInstructor,EnvioCorreo)
				select  i.* from Ficha_Ambiente f 
				join Instructor i  on f.IdInstructor = i.IdInstructor 
				
				where ( (f.FechaInicio between @vardate and @vardate ) or
						(f.FechaFin between @vardate and @vardate) or 
						(f.FechaInicio <=  @vardate and f.FechaFin>= @vardate))and
						((HoraInicio >= dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraInicio <= DATEADD(minute,-1,convert(time,@HoraFin)))or 
						(HoraFin >= dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraFin <= convert(time,@HoraFin)) or
						(f.HoraInicio <= dateadd(MINUTE,1,convert(time,@HoraInicio)) and f.HoraFin>= convert(time,@HoraFin)) 
						) and f.Jornada = 1 and i.TipoInstructor = 2  and (f.Lunes = @lunes or f.Martes = @martes or f.Miercoles = @miercoles
						 or f.Jueves = @jueves or f.Viernes = @viernes)
			end

			set @vardate = DATEADD(DAY,1,@vardate)
	end

	select distinct * from Instructor1

	drop table Instructor1



	















GO
/****** Object:  StoredProcedure [dbo].[sp_disponibilidadInstructorTransversalDomingo]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_disponibilidadInstructorTransversalDomingo]
( @fecha_ini date, @fecha_Fin date, @HoraInicio varchar(10),@HoraFin varchar(10))
AS


create table Instructor1
( IdInstructor int,
Nombre varchar(50),
Apellido varchar(50),
IdUsuario int,
Cedula varchar(50),
Email varchar(50),
Estado bit,
TipoContrato int,
Telefono varchar(50),
IdArea int,
TipoInstructor int,
EnvioCorreo bit
)

declare @vardate date = @fecha_ini

while @vardate <= @fecha_Fin
	begin 

		if ((datepart(dw,@vardate) = 7))
			begin

			

			insert into Instructor1 (IdInstructor,Nombre,Apellido,IdUsuario,Cedula,Email,Estado,TipoContrato,Telefono, IdArea,TipoInstructor,EnvioCorreo)
				select  i.* from Ficha_Ambiente f 
				join Instructor i  on f.IdInstructor = i.IdInstructor 
				
				where ( (f.FechaInicio between @vardate and @vardate ) or
						(f.FechaFin between @vardate and @vardate) or 
						(f.FechaInicio <=  @vardate and f.FechaFin>= @vardate))and
						((HoraInicio >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraInicio <= DATEADD(minute,-1,convert(time,@HoraFin)))or 
						(HoraFin >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraFin <= convert(time,@HoraFin)) or
						(f.HoraInicio <= dateadd(MINUTE,1,convert(time,@HoraInicio)) and f.HoraFin>= convert(time,@HoraFin)) 
						) and f.Jornada = 3 and i.TipoInstructor = 2 
			end

			set @vardate = DATEADD(DAY,1,@vardate)
	end

	select distinct * from Instructor1

	drop table Instructor1






GO
/****** Object:  StoredProcedure [dbo].[sp_disponibilidadInstructorTransversalSabado]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_disponibilidadInstructorTransversalSabado]
( @fecha_ini date, @fecha_Fin date, @HoraInicio varchar(10),@HoraFin varchar(10))
AS


create table Instructor1
( IdInstructor int,
Nombre varchar(50),
Apellido varchar(50),
IdUsuario int,
Cedula varchar(50),
Email varchar(50),
Estado bit,
TipoContrato int,
Telefono varchar(50),
IdArea int,
TipoInstructor int,
EnvioCorreo bit
)

declare @vardate date = @fecha_ini

while @vardate <= @fecha_Fin
	begin 

		if ((datepart(dw,@vardate) = 6))
			begin

			

			insert into Instructor1 (IdInstructor,Nombre,Apellido,IdUsuario,Cedula,Email,Estado,TipoContrato,Telefono, IdArea,TipoInstructor,EnvioCorreo)
				select  i.* from Ficha_Ambiente f 
				join Instructor i  on f.IdInstructor = i.IdInstructor 
				
				where ( (f.FechaInicio between @vardate and @vardate ) or
						(f.FechaFin between @vardate and @vardate) or 
						(f.FechaInicio <=  @vardate and f.FechaFin>= @vardate))and
						((HoraInicio >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraInicio <=DATEADD(minute,-1,convert(time,@HoraFin)))or 
						(HoraFin >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraFin <= convert(time,@HoraFin)) or
						(f.HoraInicio <= dateadd(MINUTE,1,convert(time,@HoraInicio)) and f.HoraFin>= convert(time,@HoraFin)) 
						) and f.Jornada = 2 and i.TipoInstructor = 2
			end

			set @vardate = DATEADD(DAY,1,@vardate)
	end

	select distinct * from Instructor1

	drop table Instructor1

















GO
/****** Object:  StoredProcedure [dbo].[sp_disponibilidadInstructorTransversalSinTranslape]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE proc [dbo].[sp_disponibilidadInstructorTransversalSinTranslape]
( @fecha_ini date, @fecha_Fin date, @HoraInicio varchar(10),@HoraFin varchar(10), @diasemana int, @lunes bit, @martes bit, 
@miercoles bit, @jueves bit, @viernes bit)
AS


create table Instructor1
( IdInstructor int,
Nombre varchar(50),
Apellido varchar(50),
IdUsuario int,
Cedula varchar(50),
Email varchar(50),
Estado bit,
TipoContrato int,
Telefono varchar(50),
IdArea int,
TipoInstructor int,
EnvioCorreo bit
)

declare @vardate date = @fecha_ini

while @vardate <= @fecha_Fin
	begin 

		if ((datepart(dw,@vardate) < 6))
			begin

			

			insert into Instructor1 (IdInstructor,Nombre,Apellido,IdUsuario,Cedula,Email,Estado,TipoContrato,Telefono, IdArea,TipoInstructor, EnvioCorreo)
				select  i.* from Ficha_Ambiente f 
				join Instructor i  on f.IdInstructor = i.IdInstructor 
				
				where ( (f.FechaInicio between @vardate and @vardate ) or
						(f.FechaFin between @vardate and @vardate) or 
						(f.FechaInicio <=  @vardate and f.FechaFin>= @vardate))and
						((HoraInicio >=convert(time, @HoraInicio) and HoraInicio <= DATEADD(minute,-1,convert(time,@HoraFin)))or 
						(HoraFin >=convert(time, @HoraInicio) and HoraFin <= convert(time,@HoraFin)) or
						(f.HoraInicio <= convert(time,@HoraInicio) and f.HoraFin>= convert(time,@HoraFin)) 
						) and f.Jornada = 1 and i.TipoInstructor = 2 and f.DiaSemana = @diasemana and (f.Lunes = @lunes or f.Martes = @martes or f.Miercoles = @miercoles
						 or f.Jueves = @jueves or f.Viernes = @viernes) 
			end

			set @vardate = DATEADD(DAY,1,@vardate)
	end

	select distinct * from Instructor1

	drop table Instructor1












GO
/****** Object:  StoredProcedure [dbo].[sp_HorasInstructor]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_HorasInstructor]
( @fecha_ini date, @fecha_Fin date, @idinstructor int)
AS

create table Instructor2
( IdInstructor int,
Nombre varchar(50),
Apellido varchar(50),
IdUsuario int,
Cedula varchar(50),
Email varchar(50),
Estado bit,
TipoContrato int,
Telefono varchar(50),
IdArea int,
TipoInstructor int,
EnvioCorreo bit,
HorasRango float,
Total float,
Jornada int,
)
	--------Consulta para determinar las horas trabajadas en un mes
		insert into Instructor2 (IdInstructor,Nombre,Apellido,IdUsuario,Cedula,Email,Estado,TipoContrato,Telefono, IdArea, TipoInstructor,EnvioCorreo,HorasRango,Total,Jornada)

			select i.*,t.HorasRango,t.Total,t.Jornada from 
					 (select f.*, ((cast(DATEDIFF(MINUTE,f.HoraInicio,f.HoraFin) as decimal)/60) * (dbo.DiasLaborados(convert(date,@fecha_ini),convert(date,@fecha_Fin),f.Lunes,f.Martes,f.Miercoles,f.Jueves,f.Viernes))) as  'HorasRango',
								  ((cast(DATEDIFF(MINUTE,f.HoraInicio,f.HoraFin) as decimal)/60) * (dbo.DiasLaborados(convert(date,f.FechaInicio),convert(date,f.FechaFin),f.Lunes,f.Martes,f.Miercoles,f.Jueves,f.Viernes))) as 'Total' from Ficha_Ambiente f 
										 where ( (f.FechaInicio between @fecha_ini and @fecha_Fin ) or
												 (f.FechaFin between @fecha_ini and @fecha_Fin) or 
										         (f.FechaInicio <=  @fecha_ini and f.FechaFin>= @fecha_Fin)) and
										          f.IdInstructor = @idinstructor and f.Jornada=1 ) t
				 
		
				 
			join Instructor i on t.IdInstructor = i.IdInstructor
	


		insert into Instructor2 (IdInstructor,Nombre,Apellido,IdUsuario,Cedula,Email,Estado,TipoContrato,Telefono, IdArea, TipoInstructor,EnvioCorreo,HorasRango,Total,Jornada)			
		  
		  select i.*,t.HorasRango,t.Total,t.Jornada from  
					(select f.*, (DATEDIFF( HH , f.HoraInicio , f.HoraFin ) * (dbo.diasSabado(convert(date,@fecha_ini),convert(date,@fecha_Fin)))) as 'HorasRango',
								 (DATEDIFF( HH , f.HoraInicio , f.HoraFin ) * (dbo.diasSabado(convert(date,f.FechaInicio),convert(date,f.FechaFin)))) as 'Total' from Ficha_Ambiente f 
										where ((f.FechaInicio between @fecha_ini and @fecha_Fin ) or
											   (f.FechaFin between @fecha_ini and @fecha_Fin) or 
											   (f.FechaInicio <=  @fecha_ini and f.FechaFin>= @fecha_Fin)) and 
												f.IdInstructor = @idinstructor and f.Jornada=2) t
				 
		join Instructor i on t.IdInstructor = i.IdInstructor


			insert into Instructor2 (IdInstructor,Nombre,Apellido,IdUsuario,Cedula,Email,Estado,TipoContrato,Telefono, IdArea, TipoInstructor,EnvioCorreo,HorasRango,Total,Jornada)
			
			select i.*,t.HorasRango,t.Total,t.Jornada  from  
					 (select f.*, (DATEDIFF( HH , f.HoraInicio , f.HoraFin ) * (dbo.diasDomingo(convert(date,@fecha_ini),convert(date,@fecha_Fin)))) as 'HorasRango',
								  (DATEDIFF( HH , f.HoraInicio , f.HoraFin ) * (dbo.diasDomingo(convert(date,f.FechaInicio),convert(date,f.FechaFin)))) as 'Total' from Ficha_Ambiente f 
										 where ( (f.FechaInicio between @fecha_ini and @fecha_Fin ) or
												 (f.FechaFin between @fecha_ini and @fecha_Fin) or 
												 (f.FechaInicio <=  @fecha_ini and f.FechaFin>= @fecha_Fin)) and
										          f.IdInstructor = @idinstructor and f.Jornada=3) t
				 
			join Instructor i on t.IdInstructor = i.IdInstructor
	


		select * from Instructor2 where Total is not null order by Jornada asc

		drop table Instructor2





GO
/****** Object:  StoredProcedure [dbo].[sp_ValidarProgramacion]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_ValidarProgramacion]
( @fecha_ini date, @fecha_Fin date, @HoraInicio varchar(10),@HoraFin varchar(10),@IdAmbiente int, @IdInstructor int, @IdFicha int, @jornada int, 
@lunes bit, @martes bit, @miercoles bit, @jueves bit, @viernes bit)
AS




declare @vardate date = @fecha_ini
declare @minuto time = '00:01:00'

while @vardate <= @fecha_Fin
	begin 

		if ((datepart(dw,@vardate) <= 7))
			begin

			
				select  f.* from Ficha_Ambiente f 
				
				where ( (f.FechaInicio between @vardate and @vardate ) or
						(f.FechaFin between @vardate and @vardate) or 
						(f.FechaInicio <=  @vardate and f.FechaFin>= @vardate))and
						((HoraInicio >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraInicio <= DATEADD(minute,-1,convert(time,@HoraFin)))or 
						(HoraFin >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraFin <= convert(time,@HoraFin)) or
						(f.HoraInicio <= dateadd(MINUTE,1,convert(time,@HoraInicio)) and f.HoraFin>= convert(time,@HoraFin)) 
						) and (f.IdAmbiente = @IdAmbiente or IdFicha = @IdFicha or IdInstructor = @IdInstructor ) and f.Jornada = @jornada and (f.Lunes = @lunes or f.Martes = @martes or f.Miercoles = @miercoles
						 or f.Jueves = @jueves or f.Viernes = @viernes) 
			end

			set @vardate = DATEADD(DAY,1,@vardate)
	end




GO
/****** Object:  StoredProcedure [dbo].[sp_ValidarProgramacionTransversal]    Script Date: 10/05/2019 4:09:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_ValidarProgramacionTransversal]
( @fecha_ini date, @fecha_Fin date, @HoraInicio varchar(10),@HoraFin varchar(10),@IdAmbiente int, @IdInstructor int, @IdFicha int, @lunes bit, @martes bit, @miercoles bit, @jueves bit, @viernes bit, @Opc int)
AS




declare @vardate date = @fecha_ini
declare @minuto time = '00:01:00'

while @vardate <= @fecha_Fin
	begin 

		if ((datepart(dw,@vardate) < 6))
			begin

			
				if	(@Opc = 1)

					begin 
						select  f.* from Ficha_Ambiente f 
				
						where ( (f.FechaInicio between @vardate and @vardate ) or
								(f.FechaFin between @vardate and @vardate) or 
								(f.FechaInicio <=  @vardate and f.FechaFin>= @vardate))and
								((HoraInicio >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraInicio <= dateadd(MINUTE,-1,convert(time,@HoraFin)))or 
								(HoraFin >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraFin <= dateadd(MINUTE,-1,convert(time,@HoraFin))) or
								(f.HoraInicio <= dateadd(MINUTE,1,convert(time,@HoraInicio)) and f.HoraFin>= dateadd(MINUTE,-1,convert(time,@HoraFin))) 
								) and (f.IdAmbiente = @IdAmbiente or IdFicha = @IdFicha or IdInstructor = @IdInstructor)  and (f.Lunes = @lunes or f.Martes = @martes or f.Miercoles = @miercoles
								 or f.Jueves = @jueves or f.Viernes = @viernes) and Jornada = 1 and f.Transversal = 1
				
						end 	

				if	(@Opc = 2)

					begin 
						select  f.* from Ficha_Ambiente f 
				
						where ( (f.FechaInicio between @vardate and @vardate ) or
								(f.FechaFin between @vardate and @vardate) or 
								(f.FechaInicio <=  @vardate and f.FechaFin>= @vardate))and
								((f.HoraInicio >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and f.HoraInicio <= dateadd(MINUTE,-1,convert(time,@HoraFin)))or 
								(f.HoraFin >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and f.HoraFin <= dateadd(MINUTE,-1,convert(time,@HoraFin))) or
								(f.HoraInicio <= dateadd(MINUTE,1,convert(time,@HoraInicio)) and f.HoraFin>= dateadd(MINUTE,-1,convert(time,@HoraFin))) 
								) and (f.IdAmbiente = @IdAmbiente or IdFicha = @IdFicha or IdInstructor = @IdInstructor) and Jornada = 2  and f.Transversal = 1
				
					end 	

				if	(@Opc = 3)

					begin 
						select  f.* from Ficha_Ambiente f 
				
						where ( (f.FechaInicio between @vardate and @vardate ) or
								(f.FechaFin between @vardate and @vardate) or 
								(f.FechaInicio <=  @vardate and f.FechaFin>= @vardate))and
								((f.HoraInicio >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraInicio <= dateadd(MINUTE,-1,convert(time,@HoraFin)))or 
								(f.HoraFin >=dateadd(MINUTE,1,convert(time, @HoraInicio)) and HoraFin <= dateadd(MINUTE,-1,convert(time,@HoraFin))) or
								(f.HoraInicio <= dateadd(MINUTE,1,convert(time,@HoraInicio)) and f.HoraFin>= dateadd(MINUTE,-1,convert(time,@HoraFin))) 
								) and (f.IdAmbiente = @IdAmbiente or IdFicha = @IdFicha or IdInstructor = @IdInstructor) and Jornada = 3  and f.Transversal = 1
				
					end 	
			end

			set @vardate = DATEADD(DAY,1,@vardate)
	end




GO
USE [master]
GO
ALTER DATABASE [HorarioBD2] SET  READ_WRITE 
GO
