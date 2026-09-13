# Sistema Gestor de Ventas e Inventario (Mini-POS)

**Estudiante:** Andrés Hernandez

**Descripción**
Prototipo de aplicación de consola que actúa como un Gestor de Ventas e Inventario básico. Permite registrar productos, consultar existencias, procesar ventas con cálculo automático de impuestos y descuentos, y visualizar un reporte de caja diario. Todo manejado en memoria mediante colecciones (Listas) y métodos estáticos, sin uso de bases de datos ni POO.

## Cómo ejecutar el proyecto

1. Clonar el repositorio.
2. Abrir una terminal en la carpeta raíz del proyecto.
3. Ejecutar el siguiente comando:
   `dotnet run`

## Ejemplo de Ticket Generado

```text
====================================================
REGISTRAR VENTA
====================================================

1. Jabón Johnson | Precio: $ 4.860,00 | Stock: 38

Seleccione el número del producto a vender (1-1): 1
Ingrese la cantidad a comprar:25
¿Aplica descuento de cliente frecuente (10%)? (S/N): s

====================================================
                TICKET DE VENTA
====================================================

Producto:             Jabón Johnson(x25)
 Subtotal:             $ 121.500,00
 Descuento (10%):     -$ 12.150,00
 IVA (19%):           +$ 20.776,50
---------------------------------------------------

TOTAL A PAGAR:        $ 130.126,50
====================================================

[OK] Venta efectuada con éxito. Stock actualizado: 13 unidades.

Presione una tecla para volver al menú principal.
```
