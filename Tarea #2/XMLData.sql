CREATE PROCEDURE [dbo].[ChargeXML]
	@xmlRoute varchar(200)

AS
	
DECLARE @Data xml
DECLARE @Comand NVARCHAR(500) = 'SELECT @Data =D FROM OPENROWSER (BULK ' + CHAR(39) + @xmlRoute + CHAR(39) + ', SINGLE_BLOB) AS Datos(D)'
DECLARE @Parameters NVARCHAR(500)

SET @Parameters = N'@Data xml OUTPUT' 


EXECUTE sp_executesql @Comand, @Parameters, @Datos OUTPUT -- se ejecuta el comado que se hizo de manera dinamica

DECLARE @hdoc int

EXEC sp_xml_preparedocument @hdoc OUTPUT, @Data 

INSERT INTO [dbo][Usuario]
			([Usuario]
			, [Clave])

SELECT * FROM OPENXML (@hdoc, 'RUTA DENTRO DEL CATALOGO', 1)
WITH (
	USUARIO varchar
	CLAVE varchar
	)


BEGIN
END