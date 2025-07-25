USE [MedicalAppointmentManager]
GO
/****** Object:  User [luis_romero05]    Script Date: 21/7/2025 23:28:42 ******/
CREATE USER [luis_romero05] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [luisromero05]    Script Date: 21/7/2025 23:28:42 ******/
CREATE USER [luisromero05] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 21/7/2025 23:28:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[appointments]    Script Date: 21/7/2025 23:28:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[appointments](
	[apt_id] [int] IDENTITY(1,1) NOT NULL,
	[apt_code] [varchar](15) NOT NULL,
	[apt_patient_id] [int] NOT NULL,
	[apt_doctor_id] [int] NOT NULL,
	[apt_specialty_id] [int] NOT NULL,
	[apt_date] [date] NOT NULL,
	[apt_start_time] [time](7) NOT NULL,
	[apt_end_time] [time](7) NOT NULL,
	[apt_status] [varchar](20) NULL,
	[apt_notes] [text] NULL,
	[apt_is_active] [bit] NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[apt_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[apt_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[doctors]    Script Date: 21/7/2025 23:28:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[doctors](
	[doc_id] [int] IDENTITY(1,1) NOT NULL,
	[doc_license] [varchar](20) NOT NULL,
	[doc_code] [varchar](10) NOT NULL,
	[doc_name] [varchar](100) NOT NULL,
	[doc_email] [varchar](100) NOT NULL,
	[doc_password] [varchar](255) NOT NULL,
	[doc_phone] [varchar](20) NULL,
	[doc_specialty_id] [int] NOT NULL,
	[doc_is_active] [bit] NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[doc_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[doc_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[doc_email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[doc_license] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[patients]    Script Date: 21/7/2025 23:28:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[patients](
	[pat_id] [int] IDENTITY(1,1) NOT NULL,
	[pat_identity] [varchar](20) NOT NULL,
	[pat_code] [varchar](10) NOT NULL,
	[pat_name] [varchar](100) NOT NULL,
	[pat_email] [varchar](100) NOT NULL,
	[pat_password] [varchar](255) NOT NULL,
	[pat_phone] [varchar](20) NULL,
	[pat_address] [text] NULL,
	[pat_birthdate] [date] NULL,
	[pat_is_active] [bit] NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[pat_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[pat_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[pat_email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[pat_identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[specialties]    Script Date: 21/7/2025 23:28:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[specialties](
	[spc_id] [int] IDENTITY(1,1) NOT NULL,
	[spc_code] [varchar](10) NOT NULL,
	[spc_name] [varchar](50) NOT NULL,
	[spc_description] [text] NULL,
	[spc_is_active] [bit] NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[spc_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[spc_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[appointments] ADD  DEFAULT ('scheduled') FOR [apt_status]
GO
ALTER TABLE [dbo].[appointments] ADD  DEFAULT ((1)) FOR [apt_is_active]
GO
ALTER TABLE [dbo].[appointments] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[appointments] ADD  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[doctors] ADD  DEFAULT ((1)) FOR [doc_is_active]
GO
ALTER TABLE [dbo].[doctors] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[doctors] ADD  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[patients] ADD  DEFAULT ((1)) FOR [pat_is_active]
GO
ALTER TABLE [dbo].[patients] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[patients] ADD  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[specialties] ADD  DEFAULT ((1)) FOR [spc_is_active]
GO
ALTER TABLE [dbo].[specialties] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[specialties] ADD  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[appointments]  WITH CHECK ADD FOREIGN KEY([apt_doctor_id])
REFERENCES [dbo].[doctors] ([doc_id])
GO
ALTER TABLE [dbo].[appointments]  WITH CHECK ADD FOREIGN KEY([apt_patient_id])
REFERENCES [dbo].[patients] ([pat_id])
GO
ALTER TABLE [dbo].[appointments]  WITH CHECK ADD FOREIGN KEY([apt_specialty_id])
REFERENCES [dbo].[specialties] ([spc_id])
GO
ALTER TABLE [dbo].[doctors]  WITH CHECK ADD FOREIGN KEY([doc_specialty_id])
REFERENCES [dbo].[specialties] ([spc_id])
GO
ALTER TABLE [dbo].[appointments]  WITH CHECK ADD  CONSTRAINT [CHK_status] CHECK  (([apt_status]='no_show' OR [apt_status]='canceled' OR [apt_status]='completed' OR [apt_status]='scheduled'))
GO
ALTER TABLE [dbo].[appointments] CHECK CONSTRAINT [CHK_status]
GO
/****** Object:  StoredProcedure [dbo].[sp_appointment_cancel]    Script Date: 21/7/2025 23:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Cancelar cita
CREATE   PROCEDURE [dbo].[sp_appointment_cancel]
    @code VARCHAR(15),
    @notes TEXT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @now DATETIME = GETDATE();
    DECLARE @appointment_datetime DATETIME;

    SELECT @appointment_datetime = DATETIMEFROMPARTS(
        YEAR(apt_date), MONTH(apt_date), DAY(apt_date),
        DATEPART(HOUR, apt_start_time), DATEPART(MINUTE, apt_start_time), 0, 0
    )
    FROM appointments
    WHERE apt_code = @code AND apt_is_active = 1;

    IF @appointment_datetime IS NULL
    BEGIN
        RAISERROR('Cita no encontrada o ya cancelada.', 16, 1);
        RETURN;
    END

    IF DATEDIFF(HOUR, @now, @appointment_datetime) < 24
    BEGIN
        RAISERROR('No se puede cancelar con menos de 24 horas de anticipación.', 16, 1);
        RETURN;
    END

    UPDATE appointments
    SET 
        apt_status = 'canceled',
        apt_is_active = 0,
        apt_notes = ISNULL(@notes, apt_notes),
        updated_at = @now
    WHERE apt_code = @code;

    SELECT 'Cita cancelada exitosamente.' AS Result;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_appointment_create]    Script Date: 21/7/2025 23:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- PROCEDIMIENTOS ALMACENADOS: APPOINTMENTS
-- =============================================

-- Crear cita
CREATE   PROCEDURE [dbo].[sp_appointment_create]
    @patient_identifier VARCHAR(20),
    @doctor_identifier VARCHAR(20),
    @date DATE,
    @start_time TIME,
    @duration_minutes INT = 30,
    @notes TEXT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @patient_id INT;
    DECLARE @doctor_id INT;
    DECLARE @specialty_id INT;
    DECLARE @end_time TIME = DATEADD(MINUTE, @duration_minutes, CAST(@start_time AS DATETIME));
    DECLARE @appointment_code VARCHAR(15);
    
    -- Obtener ID del paciente
    SELECT @patient_id = pat_id 
    FROM patients 
    WHERE (pat_code = @patient_identifier OR pat_identity = @patient_identifier)
    AND pat_is_active = 1;
    
    IF @patient_id IS NULL
    BEGIN
        RAISERROR('Paciente no encontrado.', 16, 1);
        RETURN;
    END
    
    -- Obtener ID del médico
    SELECT 
        @doctor_id = doc_id,
        @specialty_id = doc_specialty_id
    FROM doctors 
    WHERE (doc_code = @doctor_identifier OR doc_license = @doctor_identifier)
    AND doc_is_active = 1;
    
    IF @doctor_id IS NULL
    BEGIN
        RAISERROR('Médico no encontrado.', 16, 1);
        RETURN;
    END
    
    -- Generar código de cita
    DECLARE @next_seq INT = ISNULL((
        SELECT MAX(CAST(RIGHT(apt_code, 3) AS INT)) 
        FROM appointments 
        WHERE apt_code LIKE 'APT-' + FORMAT(GETDATE(), 'yyyy') + '-%'
    ), 0) + 1;
    
    SET @appointment_code = 'APT-' + FORMAT(GETDATE(), 'yyyy') + '-' + RIGHT('000' + CAST(@next_seq AS VARCHAR(3)), 3);
    
    -- Validar fecha futura
    IF @date < CAST(GETDATE() AS DATE)
    BEGIN
        RAISERROR('No se pueden agendar citas en fechas pasadas.', 16, 1);
        RETURN;
    END
    
    -- Validar horario laboral
    IF CAST(@start_time AS TIME) < '08:00:00' OR CAST(@end_time AS TIME) > '17:00:00'
    BEGIN
        RAISERROR('El horario debe estar entre 8:00 AM y 5:00 PM.', 16, 1);
        RETURN;
    END
    
    -- Validar solapamiento de horarios
    IF EXISTS (
        SELECT 1 FROM appointments 
        WHERE apt_doctor_id = @doctor_id 
        AND apt_date = @date 
        AND apt_start_time < @end_time 
        AND apt_end_time > CAST(@start_time AS TIME)
        AND apt_is_active = 1
    )
    BEGIN
        RAISERROR('El médico ya tiene una cita en este horario.', 16, 1);
        RETURN;
    END
    
    INSERT INTO appointments (
        apt_code, apt_patient_id, apt_doctor_id, apt_specialty_id,
        apt_date, apt_start_time, apt_end_time, apt_notes
    )
    VALUES (
        @appointment_code, @patient_id, @doctor_id, @specialty_id,
        @date, @start_time, @end_time, @notes
    );
    
    SELECT 'Cita creada exitosamente.' AS Result, @appointment_code AS appointment_code;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_appointment_deactivate]    Script Date: 21/7/2025 23:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_appointment_deactivate]
    @code VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM appointments WHERE apt_code = @code AND apt_is_active = 1)
    BEGIN
        RAISERROR('Cita no encontrada o ya inactiva.', 16, 1);
        RETURN;
    END

    UPDATE appointments
    SET 
        apt_is_active = 0,
        updated_at = GETDATE()
    WHERE apt_code = @code;

    SELECT 'Cita desactivada exitosamente.' AS Result;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_appointment_get_by_code]    Script Date: 21/7/2025 23:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Procedimientos para Appointments (ejemplo)
CREATE   PROCEDURE [dbo].[sp_appointment_get_by_code]
    @code VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF NOT EXISTS (SELECT 1 FROM appointments WHERE apt_code = @code AND apt_is_active = 1)
    BEGIN
        RAISERROR('Cita no encontrada.', 16, 1);
        RETURN;
    END

    SELECT 
        apt_id AS Id,
        apt_code AS Code,
        apt_patient_id AS PatientId,
        apt_doctor_id AS DoctorId,
        apt_specialty_id AS SpecialtyId,
        apt_date AS Date,
        apt_start_time AS StartTime,
        apt_end_time AS EndTime,
        apt_status AS Status,
        apt_notes AS Notes,
        created_at AS CreatedAt,
        updated_at AS UpdatedAt
    FROM appointments
    WHERE apt_code = @code AND apt_is_active = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_appointment_search]    Script Date: 21/7/2025 23:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Búsqueda general de citas
CREATE   PROCEDURE [dbo].[sp_appointment_search]
    @search_term VARCHAR(100) = NULL,
    @date_from DATE = NULL,
    @date_to DATE = NULL,
    @status VARCHAR(20) = NULL,
    @page INT = 1,
    @page_size INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @offset INT = (@page - 1) * @page_size;
    
    SELECT 
        a.apt_id, a.apt_code, a.apt_date, 
        a.apt_start_time, a.apt_end_time, a.apt_status,
        p.pat_code AS patient_code, p.pat_name AS patient_name,
        d.doc_code AS doctor_code, d.doc_name AS doctor_name,
        s.spc_name AS specialty_name
    FROM appointments a
    JOIN patients p ON a.apt_patient_id = p.pat_id
    JOIN doctors d ON a.apt_doctor_id = d.doc_id
    JOIN specialties s ON a.apt_specialty_id = s.spc_id
    WHERE a.apt_is_active = 1
    AND (@date_from IS NULL OR a.apt_date >= @date_from)
    AND (@date_to IS NULL OR a.apt_date <= @date_to)
    AND (@status IS NULL OR a.apt_status = @status)
    AND (
        @search_term IS NULL
        OR a.apt_code LIKE '%' + @search_term + '%'
        OR p.pat_code LIKE '%' + @search_term + '%'
        OR p.pat_name LIKE '%' + @search_term + '%'
        OR d.doc_code LIKE '%' + @search_term + '%'
        OR d.doc_name LIKE '%' + @search_term + '%'
    )
    ORDER BY a.apt_date DESC, a.apt_start_time
    OFFSET @offset ROWS
    FETCH NEXT @page_size ROWS ONLY;
    
    SELECT COUNT(*) AS total_records
    FROM appointments a
    JOIN patients p ON a.apt_patient_id = p.pat_id
    JOIN doctors d ON a.apt_doctor_id = d.doc_id
    WHERE a.apt_is_active = 1
    AND (@date_from IS NULL OR a.apt_date >= @date_from)
    AND (@date_to IS NULL OR a.apt_date <= @date_to)
    AND (@status IS NULL OR a.apt_status = @status)
    AND (
        @search_term IS NULL
        OR a.apt_code LIKE '%' + @search_term + '%'
        OR p.pat_code LIKE '%' + @search_term + '%'
        OR p.pat_name LIKE '%' + @search_term + '%'
        OR d.doc_code LIKE '%' + @search_term + '%'
        OR d.doc_name LIKE '%' + @search_term + '%'
    );
END
GO
/****** Object:  StoredProcedure [dbo].[sp_appointment_update]    Script Date: 21/7/2025 23:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_appointment_update]
    @code VARCHAR(15),
    @notes TEXT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM appointments WHERE apt_code = @code AND apt_is_active = 1)
    BEGIN
        RAISERROR('Cita no encontrada o inactiva.', 16, 1);
        RETURN;
    END

    UPDATE appointments
    SET 
        apt_notes = @notes,
        updated_at = GETDATE()
    WHERE apt_code = @code;

    SELECT 'Cita actualizada exitosamente.' AS Result;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_appointment_update_status]    Script Date: 21/7/2025 23:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Actualizar estado de cita
CREATE   PROCEDURE [dbo].[sp_appointment_update_status]
    @code VARCHAR(15),
    @status VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF NOT EXISTS (SELECT 1 FROM appointments WHERE apt_code = @code AND apt_is_active = 1)
    BEGIN
        RAISERROR('Cita no encontrada.', 16, 1);
        RETURN;
    END

    IF @status NOT IN ('scheduled', 'completed', 'canceled', 'no_show')
    BEGIN
        RAISERROR('Estado no válido. Use: scheduled, completed, canceled o no_show.', 16, 1);
        RETURN;
    END

    UPDATE appointments
    SET 
        apt_status = @status,
        updated_at = GETDATE()
    WHERE apt_code = @code;

    SELECT 'Estado de cita actualizado exitosamente.' AS Result;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_doctor_create]    Script Date: 21/7/2025 23:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- PROCEDIMIENTOS ALMACENADOS: DOCTORS
-- =============================================

-- Crear médico
CREATE   PROCEDURE [dbo].[sp_doctor_create]
    @license VARCHAR(20),
    @code VARCHAR(10),
    @name VARCHAR(100),
    @email VARCHAR(100),
    @password VARCHAR(255),
    @phone VARCHAR(20) = NULL,
    @specialty_id INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Validar licencia única
    IF EXISTS (SELECT 1 FROM doctors WHERE doc_license = @license)
    BEGIN
        RAISERROR('El número de licencia ya está registrado.', 16, 1);
        RETURN;
    END

    -- Validar código único
    IF EXISTS (SELECT 1 FROM doctors WHERE doc_code = @code)
    BEGIN
        RAISERROR('El código de médico ya está en uso.', 16, 1);
        RETURN;
    END

    -- Validar email único
    IF EXISTS (SELECT 1 FROM doctors WHERE doc_email = @email)
    BEGIN
        RAISERROR('El correo electrónico ya está registrado.', 16, 1);
        RETURN;
    END

    -- Validar especialidad existente
    IF NOT EXISTS (SELECT 1 FROM specialties WHERE spc_id = @specialty_id AND spc_is_active = 1)
    BEGIN
        RAISERROR('Especialidad no válida.', 16, 1);
        RETURN;
    END

    INSERT INTO doctors (
        doc_license, doc_code, doc_name,
        doc_email, doc_password, doc_phone,
        doc_specialty_id
    )
    VALUES (
        @license, @code, @name,
        @email, @password, @phone,
        @specialty_id
    );

    SELECT 'Médico creado exitosamente.' AS Result;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_doctor_deactivate]    Script Date: 21/7/2025 23:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_doctor_deactivate]
    @code VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM doctors WHERE doc_code = @code AND doc_is_active = 1)
    BEGIN
        RAISERROR('Médico no encontrado o ya inactivo.', 16, 1);
        RETURN;
    END

    UPDATE doctors
    SET doc_is_active = 0,
        updated_at = GETDATE()
    WHERE doc_code = @code;

    SELECT 'Médico desactivado exitosamente.' AS Result;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_doctor_get_by_id]    Script Date: 21/7/2025 23:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- PROCEDIMIENTOS PARA OTRAS ENTIDADES (actualizados de forma similar)
-- =============================================

-- Procedimientos para Doctors (ejemplo)
CREATE   PROCEDURE [dbo].[sp_doctor_get_by_id]
    @identifier VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF NOT EXISTS (
        SELECT 1 FROM doctors 
        WHERE (doc_code = @identifier OR doc_license = @identifier)
        AND doc_is_active = 1
    )
    BEGIN
        RAISERROR('Médico no encontrado.', 16, 1);
        RETURN;
    END

    SELECT 
        doc_id AS Id,
        doc_license AS License,
        doc_code AS Code,
        doc_name AS Name,
        doc_email AS Email,
        doc_phone AS Phone,
        doc_specialty_id AS SpecialtyId,
        created_at AS CreatedAt,
        updated_at AS UpdatedAt
    FROM doctors
    WHERE (doc_code = @identifier OR doc_license = @identifier)
    AND doc_is_active = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_doctor_search]    Script Date: 21/7/2025 23:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Búsqueda general de médicos
CREATE   PROCEDURE [dbo].[sp_doctor_search]
    @search_term VARCHAR(100) = NULL,
    @specialty_id INT = NULL,
    @page INT = 1,
    @page_size INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @offset INT = (@page - 1) * @page_size;
    
    SELECT 
        doc_id, doc_license, doc_code, 
        doc_name, doc_email, doc_phone,
        s.spc_name AS specialty_name
    FROM doctors d
    JOIN specialties s ON d.doc_specialty_id = s.spc_id
    WHERE d.doc_is_active = 1
    AND (@specialty_id IS NULL OR d.doc_specialty_id = @specialty_id)
    AND (
        @search_term IS NULL
        OR doc_code LIKE '%' + @search_term + '%'
        OR doc_license LIKE '%' + @search_term + '%'
        OR doc_name LIKE '%' + @search_term + '%'
    )
    ORDER BY doc_name
    OFFSET @offset ROWS
    FETCH NEXT @page_size ROWS ONLY;
    
    SELECT COUNT(*) AS total_records
    FROM doctors d
    WHERE d.doc_is_active = 1
    AND (@specialty_id IS NULL OR d.doc_specialty_id = @specialty_id)
    AND (
        @search_term IS NULL
        OR doc_code LIKE '%' + @search_term + '%'
        OR doc_license LIKE '%' + @search_term + '%'
        OR doc_name LIKE '%' + @search_term + '%'
    );
END
GO
/****** Object:  StoredProcedure [dbo].[sp_doctor_update]    Script Date: 21/7/2025 23:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_doctor_update]
    @code VARCHAR(10),
    @name VARCHAR(100),
    @email VARCHAR(100),
    @phone VARCHAR(20) = NULL,
    @specialty_id INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM doctors WHERE doc_code = @code AND doc_is_active = 1)
    BEGIN
        RAISERROR('Médico no encontrado o inactivo.', 16, 1);
        RETURN;
    END

    UPDATE doctors
    SET 
        doc_name = @name,
        doc_email = @email,
        doc_phone = @phone,
        doc_specialty_id = @specialty_id,
        updated_at = GETDATE()
    WHERE doc_code = @code;

    SELECT 'Médico actualizado exitosamente.' AS Result;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_patient_create]    Script Date: 21/7/2025 23:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- sp_patient_create (incluye password)
CREATE   PROCEDURE [dbo].[sp_patient_create]
    @identity VARCHAR(20),
    @code VARCHAR(10),
    @name VARCHAR(100),
    @email VARCHAR(100),
    @password VARCHAR(255),
    @phone VARCHAR(20) = NULL,
    @address TEXT = NULL,
    @birthdate DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Validaciones...
    
    INSERT INTO patients (
        pat_identity, pat_code, pat_name, 
        pat_email, pat_password, pat_phone,
        pat_address, pat_birthdate
    )
    VALUES (
        @identity, @code, @name,
        @email, @password, @phone,
        @address, @birthdate
    );

    SELECT 'Paciente creado exitosamente.' AS Result;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_patient_deactivate]    Script Date: 21/7/2025 23:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Desactivar paciente (corregido)
CREATE   PROCEDURE [dbo].[sp_patient_deactivate]
    @code VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM patients WHERE pat_code = @code AND pat_is_active = 1)
    BEGIN
        RAISERROR('Paciente no encontrado o ya inactivo.', 16, 1);
        RETURN;
    END

    UPDATE patients
    SET pat_is_active = 0,
        updated_at = GETDATE()
    WHERE pat_code = @code;

    SELECT 'Paciente desactivado exitosamente.' AS Result;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_patient_get_by_id]    Script Date: 21/7/2025 23:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

    CREATE PROCEDURE [dbo].[sp_patient_get_by_id]
        @identifier VARCHAR(20)
    AS
    BEGIN
        SET NOCOUNT ON;
        
        IF NOT EXISTS (
            SELECT 1 FROM patients 
            WHERE (pat_code = @identifier OR pat_identity = @identifier)
            AND pat_is_active = 1
        )
        BEGIN
            RAISERROR('Paciente no encontrado.', 16, 1);
            RETURN;
        END

        SELECT 
            pat_id,
            pat_identity,
            pat_code,
            pat_name,
            pat_email,
            pat_phone,
            pat_address,
            pat_birthdate,
            pat_is_active,
            created_at,
            updated_at
        FROM patients
        WHERE (pat_code = @identifier OR pat_identity = @identifier)
        AND pat_is_active = 1;
    END
    
GO
/****** Object:  StoredProcedure [dbo].[sp_patient_get_total]    Script Date: 21/7/2025 23:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

    CREATE PROCEDURE [dbo].[sp_patient_get_total]
        @search_term VARCHAR(100) = NULL
    AS
    BEGIN
        SET NOCOUNT ON;
        
        SELECT COUNT(*) AS total
        FROM patients
        WHERE pat_is_active = 1
        AND (
            @search_term IS NULL
            OR pat_code LIKE '%' + @search_term + '%'
            OR pat_identity LIKE '%' + @search_term + '%'
            OR pat_name LIKE '%' + @search_term + '%'
        );
    END
    
GO
/****** Object:  StoredProcedure [dbo].[sp_patient_search]    Script Date: 21/7/2025 23:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

    CREATE PROCEDURE [dbo].[sp_patient_search]
        @search_term VARCHAR(100) = NULL,
        @page INT = 1,
        @page_size INT = 10
    AS
    BEGIN
        SET NOCOUNT ON;
        
        DECLARE @offset INT = (@page - 1) * @page_size;
        
        SELECT 
            pat_id,
            pat_identity,
            pat_code,
            pat_name,
            pat_email,
            pat_phone,
            pat_address,
            pat_birthdate,
            pat_is_active,
            created_at,
            updated_at
        FROM patients
        WHERE pat_is_active = 1
        AND (
            @search_term IS NULL
            OR pat_code LIKE '%' + @search_term + '%'
            OR pat_identity LIKE '%' + @search_term + '%'
            OR pat_name LIKE '%' + @search_term + '%'
        )
        ORDER BY pat_name
        OFFSET @offset ROWS
        FETCH NEXT @page_size ROWS ONLY;
    END
    
GO
/****** Object:  StoredProcedure [dbo].[sp_patient_update]    Script Date: 21/7/2025 23:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Actualizar paciente (corregido)
CREATE   PROCEDURE [dbo].[sp_patient_update]
    @code VARCHAR(10),
    @name VARCHAR(100),
    @email VARCHAR(100),
    @phone VARCHAR(20) = NULL,
    @address TEXT = NULL,
    @birthdate DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM patients WHERE pat_code = @code AND pat_is_active = 1)
    BEGIN
        RAISERROR('Paciente no encontrado o inactivo.', 16, 1);
        RETURN;
    END

    UPDATE patients
    SET 
        pat_name = @name,
        pat_email = @email,
        pat_phone = @phone,
        pat_address = @address,
        pat_birthdate = @birthdate,
        updated_at = GETDATE()
    WHERE pat_code = @code;

    SELECT 'Paciente actualizado exitosamente.' AS Result;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_specialty_create]    Script Date: 21/7/2025 23:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- PROCEDIMIENTOS ALMACENADOS: SPECIALTIES
-- =============================================

-- Crear especialidad
CREATE   PROCEDURE [dbo].[sp_specialty_create]
    @code VARCHAR(10),
    @name VARCHAR(50),
    @description TEXT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Validar código único
    IF EXISTS (SELECT 1 FROM specialties WHERE spc_code = @code)
    BEGIN
        RAISERROR('El código de especialidad ya está en uso.', 16, 1);
        RETURN;
    END

    -- Validar nombre único
    IF EXISTS (SELECT 1 FROM specialties WHERE spc_name = @name)
    BEGIN
        RAISERROR('El nombre de especialidad ya existe.', 16, 1);
        RETURN;
    END

    INSERT INTO specialties (spc_code, spc_name, spc_description)
    VALUES (@code, @name, @description);

    SELECT 'Especialidad creada exitosamente.' AS Result;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_specialty_deactivate]    Script Date: 21/7/2025 23:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- DESACTIVAR ESPECIALIDAD (DELETE LÓGICO)
-- =============================================
CREATE   PROCEDURE [dbo].[sp_specialty_deactivate]
    @code VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM specialties WHERE spc_code = @code AND spc_is_active = 1)
    BEGIN
        RAISERROR('Especialidad no encontrada o ya inactiva.', 16, 1);
        RETURN;
    END

    UPDATE specialties
    SET spc_is_active = 0,
        updated_at = GETDATE()
    WHERE spc_code = @code;

    SELECT 'Especialidad desactivada exitosamente.' AS Result;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_specialty_get_all]    Script Date: 21/7/2025 23:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Obtener todas las especialidades
CREATE   PROCEDURE [dbo].[sp_specialty_get_all]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT spc_id, spc_code, spc_name, spc_description 
    FROM specialties 
    WHERE spc_is_active = 1
    ORDER BY spc_name;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_specialty_get_by_code]    Script Date: 21/7/2025 23:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Obtener especialidad por código
CREATE   PROCEDURE [dbo].[sp_specialty_get_by_code]
    @code VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF NOT EXISTS (SELECT 1 FROM specialties WHERE spc_code = @code AND spc_is_active = 1)
    BEGIN
        RAISERROR('Especialidad no encontrada o inactiva.', 16, 1);
        RETURN;
    END

    SELECT spc_id, spc_code, spc_name, spc_description 
    FROM specialties 
    WHERE spc_code = @code AND spc_is_active = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_specialty_update]    Script Date: 21/7/2025 23:28:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- ACTUALIZAR ESPECIALIDAD
-- =============================================
CREATE   PROCEDURE [dbo].[sp_specialty_update]
    @code VARCHAR(10),
    @name VARCHAR(50),
    @description TEXT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM specialties WHERE spc_code = @code AND spc_is_active = 1)
    BEGIN
        RAISERROR('Especialidad no encontrada o inactiva.', 16, 1);
        RETURN;
    END

    UPDATE specialties
    SET spc_name = @name,
        spc_description = @description,
        updated_at = GETDATE()
    WHERE spc_code = @code;

    SELECT 'Especialidad actualizada exitosamente.' AS Result;
END;

GO
