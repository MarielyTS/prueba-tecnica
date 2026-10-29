-- Base de datos: PostgreSQL
-- Con docker-compose este script se ejecuta solo al crear el contenedor por primera vez.
-- Ejecución manual: crear la base "prueba_tecnica" y ejecutar este script sobre ella.

DROP TABLE IF EXISTS usuarios;

CREATE TABLE usuarios (
    id              SERIAL       PRIMARY KEY,
    username        VARCHAR(50)  NOT NULL UNIQUE,
    email           VARCHAR(100) NOT NULL UNIQUE,
    password        VARCHAR(255) NOT NULL,              -- hash BCrypt, nunca texto plano
    estado          BOOLEAN      NOT NULL DEFAULT TRUE, -- TRUE = activo, FALSE = inactivo
    fecha_creacion  TIMESTAMP    NOT NULL DEFAULT NOW()
);

-- Usuario de prueba: admin / password12345 (hash BCrypt generado con BCrypt.Net-Next)
INSERT INTO usuarios (username, email, password)
VALUES ('admin', 'admin@test.com', '$2a$11$B4jzOIW29diEKxvjg9IqQu245HPe06maEF5RIRomE6pDuNxyP8KPW');
