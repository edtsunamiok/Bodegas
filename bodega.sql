-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 17-07-2021 a las 00:08:34
-- Versión del servidor: 10.4.19-MariaDB
-- Versión de PHP: 8.0.7

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `bodega`
--

DELIMITER $$
--
-- Procedimientos
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `add_existenciasKardex` (IN `id_kardex_` INT, IN `cantidad_` INT, IN `precio_` DECIMAL(18,2), IN `id_producto_` INT)  BEGIN
       
        
         update `kardex`  set `Cantidad` = (Cantidad + cantidad_), 	`Precio` = case WHEN Precio = 0 then precio_ else ((Precio+ precio_)/2) end  	where `id_Kardex` = id_kardex_;
        
select @codigo := CONCAT('I', RIGHT(CONCAT('000000' , (select CAST(IfNULL(MAX(CAST(RIGHT(NumeroPedido, 6) AS int)), 0) + 1 AS char) from m_movimientos where `id_TipoMovimiento` = 1)), 6)) ;

insert m_movimientos(Concepto,Fecha, id_TipoMovimiento, NumeroPedido)
        values('Ingreso Nuevas Existenicas a inventario', NOW(),1,@codigo);
        
        SELECT @cod :=  max(id_movimiento) FROM `m_movimientos`  ;
        
        insert d_movimiento(Cantidad, id_Producto, Precio, Total,	id_movimiento)
        values(cantidad_,id_producto_,precio_, cantidad_*precio_,@cod );
        
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `add_NuevoMovimientoE_S` (IN `id_Producto_` INT, IN `id_Tipomovimiento_` INT, IN `Concepto_` VARCHAR(10000000), IN `Cantidad_` INT, IN `Precio_` DECIMAL(18,4))  BEGIN
       update `kardex`  set `Cantidad` = case when id_Tipomovimiento_ = 1 then  (Cantidad + Cantidad_) else (Cantidad - Cantidad_) END, 	`Precio` = case when id_Tipomovimiento_ = 1 then case WHEN Precio = 0 then precio_ else ((Precio+ precio_)/2) end ELSE Precio end 	where Id_Producto = id_Producto_;
       
        
        
select @codigo := CONCAT('I', RIGHT(CONCAT('000000' , (select CAST(IfNULL(MAX(CAST(RIGHT(NumeroPedido, 6) AS int)), 0) + 1 AS char) from m_movimientos where `id_TipoMovimiento` = 1)), 6)) ;
select @codigo2 := CONCAT('S', RIGHT(CONCAT('000000' , (select CAST(IfNULL(MAX(CAST(RIGHT(NumeroPedido, 6) AS int)), 0) + 1 AS char) from m_movimientos where `id_TipoMovimiento` = 2)), 6)) ;

insert m_movimientos(Concepto,Fecha, id_TipoMovimiento, NumeroPedido)
        values(Concepto_, NOW(),id_Tipomovimiento_,case WHEN id_Tipomovimiento_ =1 then @codigo else @codigo2 end);
        
        SELECT @cod :=  max(id_movimiento) FROM `m_movimientos`  ;
         SELECT @p :=  max(Precio) FROM kardex  where Id_Producto = id_Producto_;
        
        insert d_movimiento(Cantidad, id_Producto, Precio, Total,	id_movimiento)
        values(Cantidad_,id_Producto_,@p, Cantidad_*@p,@cod );
        

        
        
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `add_Nuevoproducto` (IN `nombre` VARCHAR(200), IN `Activo` BIT)  begin
    select @cod := CONCAT('sku', RIGHT(CONCAT('sku000000' , (select CAST(IfNULL(MAX(CAST(RIGHT(Codigo, 6) AS int)), 0) + 1 AS char) from Producto )), 6)) ;
    
    INSERT INTO Producto (Producto, Activo, Codigo) VALUES (nombre,Activo, @cod);
    
    
    select  @id := max(id_producto) from `Producto`;
    
    insert `kardex`(Cantidad,id_producto,Precio) values(0,@id,0.0);
    
    end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `allKardex` ()  BEGIN
SELECT k.id_Kardex , p.Codigo, p.Producto, k.Cantidad, k.Precio,  k.Cantidad* k.Precio as Total, k.id_producto
FROM `kardex` as k inner join  producto as p on k.Id_Producto = p.id_producto 
order by p.Codigo;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `con_allMovimientos` ()  BEGIN



select  m.id_movimiento, m.fecha,m.NumeroPedido, m.Concepto, t.id_TipoMovimiento, t.NombreMovimiento ,
case when m.Estatus = 0 then 'Procesado' else 'Cancelado' end as Estatus
from m_movimientos as m 	
     inner join tipomovimiento as t on m.id_TipoMovimiento = t.id_TipoMovimiento
            order by m.fecha asc;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `con_OneKardex` (IN `id_kardex_` INT)  BEGIN
SELECT k.id_Kardex , p.Codigo, p.Producto, 0  as Cantidad, 0.0 as Precio,0.0 as Total, k.id_producto
FROM `kardex` as k inner join  producto as p on k.Id_Producto = p.id_producto 
where k.id_Kardex = id_kardex_
order by p.Codigo;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `con_OneMovimiento` (IN `id_movimiento_` INT)  BEGIN



select  m.id_movimiento, m.fecha,m.NumeroPedido, m.Concepto, d.id_movimientoDetalle, d.Cantidad, d.Precio, d.total, p.id_producto,p.Codigo, p.Producto, t.id_TipoMovimiento, t.NombreMovimiento ,
case WHEN estatus = 0 then 'Procesado' else 'Cancelado' end Estatus
from m_movimientos as m 
		inner join  d_movimiento as d on m.id_movimiento = d.id_movimiento
        inner join producto as p on d.id_producto = p.id_producto
         inner join tipomovimiento as t on m.id_TipoMovimiento = t.id_TipoMovimiento
         
         where m.id_movimiento = id_movimiento_
            order by m.fecha asc;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `con_Productos_Con_Existencias` ()  BEGIN

select  p.id_producto,CONCAT(CONCAT(CONCAT(p.Codigo," - "), p.Producto) , concat(" - Existencia ",k.Cantidad))as Codigo from producto as p 
inner join kardex as k on p.id_producto = k.Id_Producto
 /*where k.Cantidad > 0*/;


END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `con_TipoMovimiento` ()  BEGIN
SELECT id_TipoMovimiento, NombreMovimiento FROM tipomovimiento WHERE id_TipoMovimiento <>3;
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `upd_m_movimiento` (IN `id_movimiento_` INT)  BEGIN


select  @id_tipoMovimiento:= id_TipoMovimiento from  m_movimientos where id_movimiento = id_movimiento_;


select  @id_p:= Id_Producto from d_movimiento where id_movimiento = id_movimiento_;
select  @cant:= Cantidad from d_movimiento where id_movimiento = id_movimiento_;
select  @pre:= Precio from d_movimiento where id_movimiento = id_movimiento_;

update m_movimientos set  /*id_TipoMovimiento = 3 ,*/ Estatus = 1    where id_movimiento = id_movimiento_;






	update `kardex`  set `Cantidad` = case when @id_tipoMovimiento = 1 then   (Cantidad - @cant) else (Cantidad + @cant) end , 	`Precio` = case when @id_tipoMovimiento = 1 then case WHEN Precio = 0 then @pre else ((Precio*2)- @pre) end else  case WHEN Precio = 0 then @pre else ((Precio+ @pre)/2) END end 	where `Id_Producto` = @id_p;
    
    update kardex set Precio = 0 where Cantidad = 0;


END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `d_movimiento`
--

CREATE TABLE `d_movimiento` (
  `id_movimientoDetalle` int(11) NOT NULL,
  `id_producto` int(11) DEFAULT NULL,
  `Cantidad` int(11) DEFAULT NULL,
  `Precio` decimal(18,4) DEFAULT NULL,
  `total` decimal(18,4) DEFAULT NULL,
  `id_movimiento` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `d_movimiento`
--

INSERT INTO `d_movimiento` (`id_movimientoDetalle`, `id_producto`, `Cantidad`, `Precio`, `total`, `id_movimiento`) VALUES
(18, 15, 10, '0.0500', '0.5000', 19),
(19, 15, 10, '0.0000', '0.0000', 20),
(20, 15, 10, '0.0500', '0.5000', 21),
(21, 16, 10, '0.0500', '0.5000', 22),
(22, 16, 10, '0.0600', '0.6000', 23),
(23, 15, 10, '0.0200', '0.2000', 24),
(24, 17, 1, '100.0000', '100.0000', 25),
(25, 18, 20000, '0.2500', '5000.0000', 26),
(26, 18, 10, '13.3200', '133.2000', 27),
(27, 18, 50, '12.0000', '600.0000', 28),
(28, 18, 25, '1.1000', '27.5000', 29),
(29, 18, 25, '1.1000', '27.5000', 29),
(30, 18, 100, '1.0000', '100.0000', 30),
(31, 18, 50, '2.0000', '100.0000', 31),
(32, 18, 10, '3.0000', '10.0000', 32),
(33, 15, 10, '2.0000', '20.0000', 33);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `estatus`
--

CREATE TABLE `estatus` (
  `id_Estatus` int(11) NOT NULL,
  `NombreEstatus` varchar(100) DEFAULT NULL,
  `Activo` bit(1) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `kardex`
--

CREATE TABLE `kardex` (
  `id_Kardex` int(11) NOT NULL,
  `Id_Producto` int(11) DEFAULT NULL,
  `Cantidad` int(11) DEFAULT NULL,
  `Precio` decimal(18,4) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `kardex`
--

INSERT INTO `kardex` (`id_Kardex`, `Id_Producto`, `Cantidad`, `Precio`) VALUES
(5, 15, 0, '0.0000'),
(6, 16, 0, '0.0000'),
(7, 17, 0, '0.0000'),
(8, 18, 40, '1.0000');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `m_movimientos`
--

CREATE TABLE `m_movimientos` (
  `id_movimiento` int(11) NOT NULL,
  `fecha` datetime DEFAULT NULL,
  `NumeroPedido` varchar(1000) DEFAULT NULL,
  `Concepto` varchar(5000) DEFAULT NULL,
  `id_TipoMovimiento` int(11) DEFAULT NULL,
  `Estatus` bit(2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `m_movimientos`
--

INSERT INTO `m_movimientos` (`id_movimiento`, `fecha`, `NumeroPedido`, `Concepto`, `id_TipoMovimiento`, `Estatus`) VALUES
(19, '2021-07-15 09:16:23', 'I000001', 'Ingreso Nuevas Existenicas a inventario', 3, b'01'),
(20, '2021-07-15 09:17:36', 'I000002', 'Ingreso Nuevas Existenicas a inventario', 3, b'01'),
(21, '2021-07-15 09:18:43', 'I000003', 'Ingreso Nuevas Existenicas a inventario', 3, b'01'),
(22, '2021-07-15 09:19:49', 'I000004', 'Ingreso Nuevas Existenicas a inventario', 3, b'01'),
(23, '2021-07-15 09:20:02', 'I000005', 'Ingreso Nuevas Existenicas a inventario', 3, b'01'),
(24, '2021-07-15 09:30:08', 'I000006', 'Ingreso Nuevas Existenicas a inventario', 3, b'01'),
(25, '2021-07-15 10:44:47', 'I000007', 'Ingreso Nuevas Existenicas a inventario', 3, b'01'),
(26, '2021-07-15 10:45:16', 'I000008', 'Ingreso Nuevas Existenicas a inventario', 3, b'01'),
(27, '2021-07-16 12:42:07', 'I000001', 'Ingreso Nuevas Existenicas a inventario', 1, b'01'),
(28, '2021-07-16 12:47:42', 'I000002', 'Ingreso Nuevas Existenicas a inventario', 1, b'01'),
(29, '2021-07-16 15:35:14', 'S000001', 'venta', 2, b'01'),
(30, '2021-07-16 15:44:44', 'I000003', 'Ingreso Nuevas Existenicas a inventario', 1, b'00'),
(31, '2021-07-16 15:45:12', 'S000002', 'venta  de material  v2', 2, b'00'),
(32, '2021-07-16 15:50:31', 'S000003', 'venta  de material  v3', 2, b'00'),
(33, '2021-07-16 16:02:11', 'I000004', 'ingreso ', 1, b'01');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `producto`
--

CREATE TABLE `producto` (
  `id_producto` int(11) NOT NULL,
  `Producto` varchar(200) DEFAULT NULL,
  `Activo` bit(1) NOT NULL,
  `Codigo` varchar(500) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `producto`
--

INSERT INTO `producto` (`id_producto`, `Producto`, `Activo`, `Codigo`) VALUES
(15, 'Cemento 100kg', b'1', 'sku000001'),
(16, 'Ladrillos de 10x20 rojo', b'1', 'sku000002'),
(17, 'Caja Metalicas 6x6x18', b'1', 'sku000003'),
(18, 'Bolsas de cemento 100kg', b'1', 'sku000004');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipomovimiento`
--

CREATE TABLE `tipomovimiento` (
  `id_TipoMovimiento` int(11) NOT NULL,
  `NombreMovimiento` varchar(100) DEFAULT NULL,
  `Activo` bit(1) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `tipomovimiento`
--

INSERT INTO `tipomovimiento` (`id_TipoMovimiento`, `NombreMovimiento`, `Activo`) VALUES
(1, 'Ingreso por Inventario', b'1'),
(2, 'Salida por ventas', b'1'),
(3, 'Proceso Anulado', b'1');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `d_movimiento`
--
ALTER TABLE `d_movimiento`
  ADD PRIMARY KEY (`id_movimientoDetalle`);

--
-- Indices de la tabla `estatus`
--
ALTER TABLE `estatus`
  ADD PRIMARY KEY (`id_Estatus`);

--
-- Indices de la tabla `kardex`
--
ALTER TABLE `kardex`
  ADD PRIMARY KEY (`id_Kardex`);

--
-- Indices de la tabla `m_movimientos`
--
ALTER TABLE `m_movimientos`
  ADD PRIMARY KEY (`id_movimiento`);

--
-- Indices de la tabla `producto`
--
ALTER TABLE `producto`
  ADD PRIMARY KEY (`id_producto`);

--
-- Indices de la tabla `tipomovimiento`
--
ALTER TABLE `tipomovimiento`
  ADD PRIMARY KEY (`id_TipoMovimiento`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `d_movimiento`
--
ALTER TABLE `d_movimiento`
  MODIFY `id_movimientoDetalle` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=34;

--
-- AUTO_INCREMENT de la tabla `estatus`
--
ALTER TABLE `estatus`
  MODIFY `id_Estatus` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `kardex`
--
ALTER TABLE `kardex`
  MODIFY `id_Kardex` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `m_movimientos`
--
ALTER TABLE `m_movimientos`
  MODIFY `id_movimiento` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=34;

--
-- AUTO_INCREMENT de la tabla `producto`
--
ALTER TABLE `producto`
  MODIFY `id_producto` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- AUTO_INCREMENT de la tabla `tipomovimiento`
--
ALTER TABLE `tipomovimiento`
  MODIFY `id_TipoMovimiento` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
