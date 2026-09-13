using System;
using System.Collections.Generic;

namespace GestorVentasUnidad1
{
    class Program
    {
        static void Main(string[] args)
        {
            // ==========================================
            // 1. "BASES DE DATOS" EN MEMORIA (Listas)
            // ==========================================
            List<string> nombresProductos = new List<string>();
            List<decimal> preciosProductos = new List<decimal>();
            List<int> stockProductos = new List<int>();

            // ==========================================
            // 2. VARIABLES PARA EL REPORTE DE CAJA
            // ==========================================
            int ventasRealizadas = 0;
            decimal totalCaja = 0m;
            string productoMasVendido = "";
            int maxUnidadesVendidas = 0; 

            // ==========================================
            // 3. CICLO PRINCIPAL (MENÚ)
            // ==========================================
            int opcion = 0;

            do
            {
                // Limpieza de pantalla y muestra de encabezado.
                ImprimirEncabezado("SISTEMA GESTOR DE VENTAS E INVENTARIO (MINI-POS)");

                // Imprimimos las opciones del menú
                Console.WriteLine("1. Registrar nuevo producto en inventario");
                Console.WriteLine("2. Consultar inventario completo");
                Console.WriteLine("3. Registrar una venta");
                Console.WriteLine("4. Ver reporte de caja y estadísticas diarias");
                Console.WriteLine("5. Salir");
                Console.WriteLine("====================================================");

                // Pedimos la opción usando nuestro método seguro
                opcion = LeerEntero("Seleccione una opción (1-5): ", 1, 5);

                Console.Clear(); // Limpiamos la pantalla antes de ejecutar la opción

                // Evaluamos qué opción escogió el usuario
                switch (opcion)
                {
                    case 1:
                        ImprimirEncabezado("REGISTRAR NUEVO PRODUCTO");
                        
                        // 1. Pedir y validar el nombre
                        string nuevoNombre = "";
                        bool nombreEsValido = false;

                        while (!nombreEsValido)
                        {
                            Console.Write("Ingrese el nombre del producto: ");
                            nuevoNombre = Console.ReadLine() ??"";

                            // Validamos que no esté vacío (Trim() quita los espacios en blanco de los extremos)
                            if (string.IsNullOrWhiteSpace(nuevoNombre))
                            {
                                Console.WriteLine("[ERROR] El nombre no puede estar vacío.\n");
                            }
                            else
                            {
                                // Validar que no exista un producto con el mismo nombre
                                bool existe = false;
                                
                                // Recorremos la lista de nombres para buscar duplicados
                                foreach (string nombreGuardado in nombresProductos)
                                {
                                    // Comparamos los textos ignorando mayúsculas/minúsculas
                                    if (nombreGuardado.Equals(nuevoNombre, StringComparison.OrdinalIgnoreCase))
                                    {
                                        existe = true;
                                        break;
                                    }
                                }

                                if (existe)
                                {
                                    Console.WriteLine("[ERROR] Ya existe un producto con ese nombre. Intente con otro.\n");
                                }
                                else
                                {
                                    nombreEsValido = true; 
                                }
                            }
                        }

                        // 2. Pedir y validar el precio   
                        decimal precioProducto = LeerDecimal("Ingrese el precio unitario del producto: ", 0m);
                        
                        // 3. Pedir y validar el stock                        
                        int stockProducto = LeerEntero("Ingrese la cantidad de stock inicial del producto: ", 0, int.MaxValue);

                        // 4. Guardar los datos en nuestra "Base de datos"
                        nombresProductos.Add(nuevoNombre.Trim());
                        preciosProductos.Add(precioProducto);
                        stockProductos.Add(stockProducto);

                        Console.WriteLine("\n[OK] Producto registrado con éxito.");
                        break;

                    case 2:
                        ImprimirEncabezado("CONSULTAR INVENTARIO");
                        
                        // 1. Verificamos si hay productos registrados
                        if (nombresProductos.Count == 0)
                        {
                            Console.WriteLine("No hay productos registrados en el inventario aún.");
                        }
                        else
                        {
                            // 2. Imprimimos los encabezados de la tabla para que se vea ordenado
                            // -20, 10 y -10 se utiliza para alinear columnas a la izquierda o derecha
                            Console.WriteLine($"{"ID",-4} | {"NOMBRE DEL PRODUCTO",-20} | {"PRECIO",12} | {"STOCK",-10}");
                            Console.WriteLine("------------------------------------------------------------------");

                            // 3. Recorremos las listas con un for
                            for (int i = 0; i < nombresProductos.Count; i++)
                            {
                                // Extraemos los datos del producto actual
                                string nombre = nombresProductos[i];
                                decimal precio = preciosProductos[i];
                                int stock = stockProductos[i];

                                // 4. Formateamos la fila
                                // precio.ToString("C") formatea automáticamente como Moneda
                                string fila = $"{i + 1,-4} | {nombre,-20} | {precio,12:C} | {stock,-5}";

                                // 5. Validación de la alerta visual si el stock es menor a 5
                                if (stock < 5)
                                {
                                    fila += " [ALERTA: BAJO STOCK]";
                                }

                                Console.WriteLine(fila);
                            }
                        }
                        break;

                    case 3:
                        ImprimirEncabezado("REGISTRAR VENTA");
                        
                        // Si no hay productos, no podemos vender nada
                        if (nombresProductos.Count == 0)
                        {
                            Console.WriteLine("No hay productos registrados para vender.");
                            break; 
                        }

                        // 1. Mostrar lista de productos
                        for (int i = 0; i < nombresProductos.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {nombresProductos[i]} | Precio: {preciosProductos[i]:C} | Stock: {stockProductos[i]}");
                        }
                        Console.WriteLine();

                        // 2. Pedir el producto a comprar
                        int idElegido = LeerEntero($"Seleccione el número del producto a vender (1-{nombresProductos.Count}): ", 1, nombresProductos.Count);
                        int indice = idElegido - 1;

                        // 3. Pedir la cantidad validando contra el stock real
                        int cantidadComprar = 0;
                        bool cantidadValida = false;

                        while (!cantidadValida)
                        {
                            cantidadComprar = LeerEntero("Ingrese la cantidad a comprar: ", 1, int.MaxValue);

                            if (cantidadComprar > stockProductos[indice])
                            {
                                Console.WriteLine($"[ERROR] Stock insuficiente. Solo quedan {stockProductos[indice]} unidades en inventario.\n");
                            }
                            else
                            {
                                cantidadValida = true; // Si alcanza el stock, rompemos el ciclo
                            }
                        }

                        // 4. Preguntar por descuento
                        Console.Write("¿Aplica descuento de cliente frecuente (10%)? (S/N): ");
                        string respDescuento = Console.ReadLine() ?? "";
                        bool tieneDescuento = respDescuento.Trim().Equals("S", StringComparison.OrdinalIgnoreCase);

                        // 5. CÁLCULO DE FACTURA
                        decimal montoIva; 
                        decimal montoDescuento;
                        decimal totalPagar = CalcularFactura(preciosProductos[indice], cantidadComprar, tieneDescuento, out montoIva, out montoDescuento);
                        decimal subtotal = preciosProductos[indice] * cantidadComprar;

                        // 6. Actualizar inventario y caja
                        stockProductos[indice] -= cantidadComprar; // Restamos lo vendido
                        ventasRealizadas++;                        // Sumamos 1 a las ventas del día
                        totalCaja += totalPagar;                   // Acumulamos el dinero

                        // Evaluamos si es el producto más vendido de la sesión
                        if (cantidadComprar > maxUnidadesVendidas)
                        {
                            maxUnidadesVendidas = cantidadComprar;
                            productoMasVendido = nombresProductos[indice];
                        }

                        // 7. Imprimir el Ticket
                        Console.WriteLine("\n====================================================");
                        Console.WriteLine("                  TICKET DE VENTA");
                        Console.WriteLine("====================================================");
                        Console.WriteLine($" Producto:             {nombresProductos[indice]} (x{cantidadComprar})");
                        Console.WriteLine($" Subtotal:             {subtotal:C}");
                        Console.WriteLine($" Descuento (10%):     -{montoDescuento:C}");
                        Console.WriteLine($" IVA (19%):           +{montoIva:C}");
                        Console.WriteLine(" ---------------------------------------------------");
                        Console.WriteLine($" TOTAL A PAGAR:        {totalPagar:C}");
                        Console.WriteLine("====================================================");
                        Console.WriteLine($"[OK] Venta efectuada con éxito. Stock actualizado: {stockProductos[indice]} unidades.");
                        break;

                    case 4:
                        ImprimirEncabezado("REPORTE DE CAJA");
                        if (ventasRealizadas == 0)
                        {
                            Console.WriteLine("Aún no se han realizado ventas en esta sesión.");
                        }
                        else
                        {
                            // Calculamos el promedio
                            decimal promedioVentas = totalCaja / ventasRealizadas;

                            // Imprimimos las estadísticas
                            Console.WriteLine($" Total de ventas en la sesión:  {ventasRealizadas}");
                            Console.WriteLine($" Total ingresado a caja:        {totalCaja:C}");
                            Console.WriteLine($" Promedio de dinero por venta:  {promedioVentas:C}");
                            Console.WriteLine($" Producto más vendido:          {productoMasVendido} (con {maxUnidadesVendidas} unidades vendidas)");
                            Console.WriteLine("\n[OK] Reporte generado exitosamente.");
                        }
                        break;

                    case 5:
                        ImprimirEncabezado("SALIENDO DEL SISTEMA");
                        Console.WriteLine("¡Gracias por utilizar el Mini-POS! Hasta pronto.\n");
                        break;
                }

                if (opcion != 5)
                {
                    Console.WriteLine("\nPresione una tecla para volver al menú principal...");
                    Console.ReadKey();
                }

            } while (opcion != 5);
        }

        // =======================================================
        // MÉTODOS UTILITARIOS
        // =======================================================
        
        // Método lectura segura de enteros.
        static int LeerEntero(string mensaje, int min, int max)
        {
            int numeroValido = 0;
            bool esValido = false;

            while (!esValido)
            {
                
                Console.Write(mensaje);
                string entradaUsuario = Console.ReadLine() ??"";

                // tryparse: intenta convertir la entrada del usuario a un número entero, si no puede, devuelve false
                bool esNumero = int.TryParse(entradaUsuario, out numeroValido);

                if (!esNumero)
                {
                    // Si no es un número, mostramos un mensaje de error y volvemos a pedir la entrada
                    Console.WriteLine("[ERROR] Entrada no válida. Debe ingresar un número entero.\n");
                }
                else if (numeroValido < min || numeroValido > max)
                {
                    // Si es un número, pero no está en el rango permitido (ej. puso 6 en un menú del 1 al 5)
                    Console.WriteLine($"[ERROR] Opción fuera de rango. Ingrese un valor entre {min} y {max}.\n");
                }
                else
                {
                    // si es un número y está en el rango permitido, salimos del bucle
                    esValido = true; 
                }
            }
            
            return numeroValido;
        }

        // Método lectura segura de decimales.
        static decimal LeerDecimal(string mensaje, decimal min)
        {
            decimal numeroValido = 0;
            bool esValido = false;

            while (!esValido)
            {
                Console.Write(mensaje);
                string entradaUsuario = Console.ReadLine() ??"";

                // tryparse: intenta convertir la entrada del usuario a un número decimal, si no puede, devuelve false
                bool esNumero = decimal.TryParse(entradaUsuario, out numeroValido);

                if (!esNumero)
                {
                    // Si no es un número, mostramos un mensaje de error y volvemos a pedir la entrada
                    Console.WriteLine("[ERROR] Entrada no válida. Debe ingresar un número decimal.\n");
                }
                else if (numeroValido < min)
                {
                    // Si es un número, pero no está en el rango permitido (ej. puso -1 en un precio que debe ser >= 0)
                    Console.WriteLine($"[ERROR] Opción fuera de rango. Ingrese un valor mayor o igual a {min}.\n");
                }
                else
                {
                    // si es un número y está en el rango permitido, salimos del bucle
                    esValido = true; 
                }
            }
            
            return numeroValido;
        }

        //Logica Calculo de factura
        static decimal CalcularFactura(decimal precio, int cantidad, bool tieneDescuento, out decimal montoIva, out decimal montoDescuento)
        {
            // Calculamos el subtotal
            decimal subtotal = precio * cantidad;
            
            // Calculamos el descuento
            if (tieneDescuento)
            {
                montoDescuento = subtotal * 0.10m;
            }
            else
            {
                montoDescuento = 0m;
            }

            // Calculamos el IVA
            decimal baseGravable = subtotal - montoDescuento;
            
            montoIva = baseGravable * 0.19m;

            // Calculamos el total final a pagar
            decimal totalAPagar = baseGravable + montoIva;

            return totalAPagar;
        }

        // Método para imprimir un encabezado en la consola
        static void ImprimirEncabezado(string titulo)
        {
            // Limpia la consola para que no quede basura de ejecuciones anteriores
            Console.Clear();
            
            Console.WriteLine("====================================================");
            
            Console.WriteLine($"   {titulo}");
            
            Console.WriteLine("====================================================\n");
        }
    }
}