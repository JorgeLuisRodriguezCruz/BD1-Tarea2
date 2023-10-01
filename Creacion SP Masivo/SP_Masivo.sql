CREATE PROCEDURE dbo.AcreditaVacaciones 
	@inFechaOperacion DATE
	, @OutResult INT OUTPUt
AS
BEGIN
SET NOCOUNT ON;

BEGIN TRY

	SET @OutResult = 0;    			-- por default, 0 indica que el proceso corrio exitosamente
	DECLARE 
		@idEmpleado INT
		, @SaldoActual FLOAT
		, @TextoEmail TEXT
		, @LastIdComprobante INT
		, @QEmpleadosCumplen INT
		, @IDTIPOMOVIMIENTO INT = 3   -- ALAMBRADO, es constante (En mayusculas)Para denotar que son constantes
		, @MONTOCREDITO INT =1 		  -- el monto del credito siempre sera 1
		, @EVENTACREDITATODOS INT = 7 -- Alamabrado, constante
		;
	
	-- tabla variable con los empleados que cumplen mes
	
	DECLARE @EmpleadosCumplen TABLE(
		Sec INT IDENTITY(1,)
		, IdEmpleado INT
		, SaldoActual FLOAT
		, textoemail TEXT
		);
		
	-- control de que el proceso no se este corriendo 2 veces
	
	IF EXISTS (SELECT 1 FROM dbo.EventLog EL WHERE (EL.IdEventType=@EVENTACREDITATODOS) AND (EL.EventDate=@inFechaOperacion))
	BEGIN
		SET @OutResult=500201     -- Proceso ya se corrio para el dia de operacion
		RETURN;
	END;
	
	INSERT @empleadosCumplen (
		IdEmpleado
		, SaldoActual
		, textoemail
	)
	SELECT e.id
		e.SaldoVacaciones
		, 'Felicidades, ha cumplido nuevo mes de trabajo, 1 dia mas vacaciones, nuevo saldo'+CONVERT(VARCHAR, dbo.FNSumaSaldo(e.SaldoVacaciones, 1))
	FROM dbo.Empleado E
	WHERE dbo.FNCumplemes(E.FechaContratacion, @inFechaOperacion)=1
	ORDER BY e.IdEmpleado;    -- En indices clustered, ordenar por la PK mejora los tiempos de actualizacion

	SET @QEmpleadosCumplen=@@ROWCOUNT;    -- variable del sistema que indica cantidad de rows afectados en ultima sentencia SQL
				
	BEGIN TRANSACTION tacreditaTodos;

	COMMIT TRANSACTION tacreditaTodos;
	
	SELECT @outResult AS outResultCode


END TRY
BEGIN CATCH

	IF @@TRANCOUNT>0 BEGIN
		ROLLBACK tacreditaTodos;
	END;
	
	INSERT dbErrors ()
	Select ....
	
	
	SET @OutResult = 50005;   -- error en BD
	SELECT @outResult AS outResultCode

END CATCH

SET NOCOUNT OFF;

END;