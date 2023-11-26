using System;
using System.IO;
using System.Text;
using Xamarin.Essentials;
using SkiaSharp;
using System.Reflection;
using Xamarin.Forms;

namespace GenerarPDF
{
    public class PDF
    {
        private string contenido { get; set; }
        private string selectedImagePath;

        public PDF(string content)
        {
            this.contenido = content;
        }

        private string GenerateRandomNumbers(int length)
        {
            Random random = new Random();
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                int randomNumber = random.Next(0, 10); // Genera un dígito aleatorio entre 0 y 9
                stringBuilder.Append(randomNumber);
            }

            return stringBuilder.ToString();
        }

        public async void GenerarPDF(byte[] DatoImagen)
        {

            string randomTicketName = "ticket_" + GenerateRandomNumbers(5) + ".pdf";
            var filePath = Path.Combine(FileSystem.CacheDirectory, randomTicketName);
            var listaCadenas = contenido.Split('\n');
            int num = listaCadenas.Length;
            float pointsPerInch = 72; // 72 puntos por pulgada
            float width = 8.5f * pointsPerInch; // 8.5 pulgadas en puntos
            float height = 11f * pointsPerInch; // 11 pulgadas en puntos

            using (var stream = new SKFileWStream(filePath))
            using (var document = SKDocument.CreatePdf(stream))
            using (var canvas = document.BeginPage(width, height))
            {
                // Establecer el fondo
                canvas.Clear(SKColors.White);


                // Load the image from the embedded resources
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "GenerarPDF.Resources.Images.logo.png";
                using (var resourceStream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (resourceStream != null)
                    {
                        using (var bitmap = SKBitmap.Decode(resourceStream))
                        {
                            // Convertir SKBitmap a SKImage
                            var image = SKImage.FromBitmap(bitmap);

                            // Obtener el tamaño del PDF
                            var pdfWidth = 400; // Ancho del PDF


                            // Calcular el ancho y alto de la imagen para ajustar al ancho del PDF y mantener la proporción
                            float imageWidth = pdfWidth / 4.00f;
                            float imageHeight = bitmap.Height * (imageWidth / bitmap.Width);

                            // Calcular la posición para colocar la imagen al principio del PDF
                            float imageX = (pdfWidth - imageWidth) / 10; // Centrar la imagen horizontalmente
                            float imageY = 0; // Margen de 50 unidades desde arriba del PDF

                            // Dibujar la imagen en el canvas
                            canvas.DrawImage(image, new SKRect(imageX, imageY, imageX + imageWidth, imageY + imageHeight));
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Image resource not found.");
                    }

                    // Establecer estilo de texto
                    using (var paint = new SKPaint())
                    {
                        paint.TextSize = 14;
                        paint.Color = SKColors.Black; // Color del texto
                                                      // Definir el color del fondo de las celdas
                        SKColor celdaBackgroundColor = SKColors.Coral;
                        SKColor celdaBackgroundColorB = SKColors.White;
                        SKColor celdaBackgroundColorc = SKColors.Gold;
                        SKColor celdaBorderColor = SKColors.Black;
                        // Configurar las coordenadas iniciales para la primera tabla
                        float x = 20;
                        float y = 20; // Puedes ajustar la posición vertical

                        // Datos de la tabla con celdas más anchas
                        string[][] encabezadoTablaOrden = new string[][]
                        {
                        new string[] { "                                        Orden de Admisión de Cotización "},

                        };

                        // Ajusta el ancho de las celdas
                        float cellWidtho = 450; // Cambia el valor a un ancho deseado


                        // Iterar sobre los datos de la  tabla y dibujar celdas
                        foreach (var fila in encabezadoTablaOrden)
                        {
                            x = 148; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidtho, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorc;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidtho - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidtho + 10; // Espacio entre celdas
                            }
                            x = 20;

                        }

                        x = 20;  // Reiniciar x para la siguiente fila
                        y += 35; // Ajusta la posición vertical para la tercera tabla

                        // Datos de la tabla con celdas más anchas
                        string[][] TablaFolio = new string[][]
                        {
                        new string[] { "Folio:"}

                        };


                        // Ajusta el ancho de las celdas
                        float cellWidthf = 170; // Cambia el valor a un ancho deseado


                        // Iterar sobre los datos de la  tabla y dibujar celdas
                        foreach (var fila in TablaFolio)
                        {
                            x = 428; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidthf, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorB;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidthf - 1, y + 30 - 1), paint);




                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidthf + 10; // Espacio entre celdas
                            }
                            x = 80;
                            y += 25; // Espacio entre filas
                        }


                        x = 20;  // Reiniciar x para la siguiente fila
                        y += 15; // Ajusta la posición vertical para la tercera tabla

                        // Datos de la tabla con celdas más anchas
                        string[][] encabezadoTablaDatosVehiculos = new string[][]
                        {
                        new string[] { "                                                             Datos del vehículo     "},

                        };

                        // Ajusta el ancho de las celdas
                        float cellWidthe = 578; // Cambia el valor a un ancho deseado


                        // Iterar sobre los datos de la  tabla y dibujar celdas
                        foreach (var fila in encabezadoTablaDatosVehiculos)
                        {
                            x = 20; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidthe, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColor;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidthe - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidthe + 10; // Espacio entre celdas
                            }
                            x = 20;
                            y += 25; // Espacio entre filas
                        }




                        x = 20;  // Reiniciar x para la siguiente fila
                        y += 15; // Ajusta la posición vertical para la tercera tabla


                        // Datos de la primera tabla
                        string[][] primeraTablaDatosVehiculos = new string[][]
                        {
                        new string[] { "Marca:", "Submarca:", "Modelo:", "Tipo:" },
                        new string[] { "------", "-----", "-----", "-----" },
                        };

                        float cellWidth1 = 137; // Cambia el valor a un ancho deseado


                        // Iterar sobre los datos de la primera tabla y dibujar celdas
                        foreach (var fila in primeraTablaDatosVehiculos)
                        {
                            x = 20; // Reiniciar x para la siguiente fila
                            foreach (var valor in fila)
                            {


                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidth1, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorB;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth1 - 1, y + 30 - 1), paint);




                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidth1 + 10; // Espacio entre celdas
                            }
                            x = 20;
                            y += 25; // Espacio entre filas
                        }





                        // Reiniciar x y ajustar la posición vertical para la segunda tabla
                        x = 20;
                        y += 15; // Ajusta la posición vertical para la segunda tabla

                        // Datos de la segunda tabla
                        string[][] segundaTablaDatosVehiculos = new string[][]
                        {
                        new string[] { "Versión:", "Categoría", "Color", "Acabado" },
                        new string[] { "-----------", "-----------", "-----------", "-----------" },
                        };

                        float cellWidth2 = 137; // Cambia el valor a un ancho deseado

                        // Iterar sobre los datos de la segunda tabla y dibujar celdas
                        foreach (var fila in segundaTablaDatosVehiculos)
                        {
                            x = 20; // Reiniciar x para la siguiente fila
                            foreach (var valor in fila)
                            {
                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidth2, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorB;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth2 - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidth2 + 10; // Espacio entre celdas
                            }
                            x = 30; // Reiniciar x para la siguiente fila
                            y += 25; // Espacio entre filas
                        }




                        // Reiniciar x y ajustar la posición vertical para la segunda tabla
                        x = 20;
                        y += 15; // Ajusta la posición vertical para la otra tabla


                        // Datos de la tabla con celdas más anchas
                        string[][] encabezadoTablaServicio = new string[][]
                        {
                        new string[] { "                                                        Datos del Servicio      "},

                        };

                        // Ajusta el ancho de las celdas
                        float cellWidthes = 578; // Cambia el valor a un ancho deseado


                        // Iterar sobre los datos de la  tabla y dibujar celdas
                        foreach (var fila in encabezadoTablaServicio)
                        {
                            x = 20; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {

                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidthes, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColor;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidthes - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidthes + 10; // Espacio entre celdas
                            }
                            x = 20;
                            y += 25; // Espacio entre filas
                        }




                        // Reiniciar x y ajustar la posición vertical para la  tabla
                        x = 20;
                        y += 15; // Ajusta la posición vertical para la tabla

                        // Datos de la  tabla con celdas más anchas
                        string[][] primeraTablaDatosServicio = new string[][]
                        {
                        new string[] { "Tipo Servicio:", "Paquete:", "Prioridad:", "Tipo:" },
                        new string[] { " -------", "-----", "-----", "-----" },
                                            };

                        // Ajusta el ancho de las celdas
                        float cellWidth3 = 137; // Cambia el valor a un ancho deseado

                        // Iterar sobre los datos de la tabla y dibujar celdas
                        foreach (var fila in primeraTablaDatosServicio)
                        {
                            x = 20; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidth3, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorB;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth3 - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidth3 + 10; // Espacio entre celdas
                            }
                            x = 20;
                            y += 25; // Espacio entre filas
                        }


                        // Reiniciar x y ajustar la posición vertical para la tabla
                        x = 20;
                        y += 15; // Ajusta la posición vertical para la tabla

                        // Datos de la tabla con celdas más anchas
                        string[][] segundaTablaDatosServicio = new string[][]
                        {
                        new string[] { $"Dirección del Taller:" },
                        new string[] { "" },
                        };

                        // Ajusta el ancho y alto de las celdas
                        float cellWidth4 = 578; // Cambia el valor a un ancho deseado
                        float cellHeight = 75; // Altura de la celda

                        // Iterar sobre los datos de la tabla y dibujar celdas
                        foreach (var fila in segundaTablaDatosServicio)
                        {
                            x = 20; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Divide el valor en líneas si contiene saltos de línea
                                var lineas = valor.Split('\n');

                                // Calcula la altura de la celda en función del número de líneas
                                float celdaHeight = cellHeight * lineas.Length;

                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidth4, y + celdaHeight), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorB;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth4 - 1, y + celdaHeight - 1), paint);

                                // Dibujar cada línea en la celda
                                paint.Color = SKColors.Black;
                                foreach (var linea in lineas)
                                {
                                    canvas.DrawText(linea, x + 10, y + 20, paint);
                                    y += cellHeight; // Ajusta la posición vertical para la siguiente línea
                                }

                                x += cellWidth4 + 10; // Espacio entre celdas
                                y -= cellHeight * lineas.Length; // Ajusta la posición vertical para la siguiente celda
                            }

                            x = 20;
                            y += 25; // Espacio entre filas
                        }



                        // Reiniciar x y ajustar la posición vertical para la  tabla
                        x = 20;
                        y += 60; // Ajusta la posición vertical para la tabla

                        // Datos de la  tabla con celdas más anchas
                        string[][] terceraTablaDatosServicio = new string[][]
                        {
                        new string[] { "Fecha:", "Hora:" },
                        new string[] { " -------", "-----" },
                                            };

                        // Ajusta el ancho de las celdas
                        float cellWidth5 = 284; // Cambia el valor a un ancho deseado

                        // Iterar sobre los datos de la tabla y dibujar celdas
                        foreach (var fila in terceraTablaDatosServicio)
                        {
                            x = 20; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidth5, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorB;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth5 - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidth5 + 10; // Espacio entre celdas
                            }
                            x = 20;
                            y += 25; // Espacio entre filas
                        }


                        // Reiniciar x y ajustar la posición vertical para la segunda tabla
                        x = 20;
                        y += 15; // Ajusta la posición vertical para la otra tabla


                        // Datos de la tabla con celdas más anchas
                        string[][] encabezadoTablaUsuario = new string[][]
                        {
                        new string[] { "                                                           Datos del Usuario    "},

                        };

                        // Ajusta el ancho de las celdas
                        float cellWidtheu = 578; // Cambia el valor a un ancho deseado


                        // Iterar sobre los datos de la  tabla y dibujar celdas
                        foreach (var fila in encabezadoTablaUsuario)
                        {
                            x = 20; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {

                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidtheu, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColor;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidtheu - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidtheu + 10; // Espacio entre celdas
                            }
                            x = 20;
                            y += 25; // Espacio entre filas
                        }

                        // Reiniciar x y ajustar la posición vertical para la  tabla
                        x = 20;
                        y += 15; // Ajusta la posición vertical para la tabla

                        // Datos de la  tabla con celdas más anchas
                        string[][] primeraTablaDatosUsuarios = new string[][]
                        {
                        new string[] { "Nombre completo:", "Correo electronico" },
                        new string[] { " -------", "-----"},
                                            };

                        // Ajusta el ancho de las celdas
                        float cellWidth6 = 284; // Cambia el valor a un ancho deseado

                        // Iterar sobre los datos de la tabla y dibujar celdas
                        foreach (var fila in primeraTablaDatosUsuarios)
                        {
                            x = 20; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidth6, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorB;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth6 - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidth6 + 10; // Espacio entre celdas
                            }
                            x = 20;
                            y += 25; // Espacio entre filas
                        }

                        // Reiniciar x y ajustar la posición vertical para la  tabla
                        x = 20;
                        y += 20; // Ajusta la posición vertical para la tabla

                        // Datos de la  tabla con celdas más anchas
                        string[][] segundaTablaDatosUsuarios = new string[][]
                        {
                        new string[] { "Telefono:", "Estado","Municipio" },
                        new string[] { " -------", "-----","--------"},
                                            };

                        // Ajusta el ancho de las celdas
                        float cellWidth7 = 186; // Cambia el valor a un ancho deseado

                        // Iterar sobre los datos de la tabla y dibujar celdas
                        foreach (var fila in segundaTablaDatosUsuarios)
                        {
                            x = 20; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidth7, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorB;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth7 - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidth7 + 10; // Espacio entre celdas
                            }
                            x = 20;
                            y += 25; // Espacio entre filas
                        }


                        // Reiniciar x y ajustar la posición vertical para la  tabla
                        x = 20;
                        y += 18; // Ajusta la posición vertical para la tabla

                        // Datos de la  tabla con celdas más anchas
                        string[][] tablaPresupuesto = new string[][]
                        {
                        new string[] { "Presupuesto:" },
                        new string[] { " -------" },
                                            };

                        // Ajusta el ancho de las celdas
                        float cellWidth8 = 278; // Cambia el valor a un ancho deseado

                        // Iterar sobre los datos de la tabla y dibujar celdas
                        foreach (var fila in tablaPresupuesto)
                        {
                            x = 320; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidth8, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorB;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth8 - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidth5 + 10; // Espacio entre celdas
                            }
                            x = 20;
                            y += 25; // Espacio entre filas
                        }
                    }

                    // Finalizar el documento
                    document.EndPage();
                    // Comienza una nueva página en blanco
                    using (var canvas2 = document.BeginPage(width, height))
                    {
                        // Deja esta página en blanco, o dibuja cualquier cosa que desees en ella.
                        canvas.Clear(SKColors.White);

                        var assembly2 = Assembly.GetExecutingAssembly();

                        // Establecer estilo de texto
                        using (var paint = new SKPaint())
                        {
                            paint.TextSize = 14;
                            paint.Color = SKColors.Black; // Color del texto
                                                          // Definir el color del fondo de las celdas
                            SKColor celdaBackgroundColor = SKColors.Coral;
                            SKColor celdaBackgroundColorB = SKColors.White;
                            SKColor celdaBackgroundColorc = SKColors.Gold;
                            SKColor celdaBorderColor = SKColors.Black;
                            // Configurar las coordenadas iniciales para la primera tabla
                            float x = 20;
                            float y = 15; // Puedes ajustar la posición vertical

                            // Datos de la tabla con celdas más anchas
                            string[][] encabezadoTablaOrden2 = new string[][]
                            {
                                 new string[] { "                                                            Aréa Dañada del Vehículo  "},

                            };

                            // Ajusta el ancho de las celdas
                            float cellWidth14 = 578; // Cambia el valor a un ancho deseado


                            // Iterar sobre los datos de la  tabla y dibujar celdas
                            foreach (var fila in encabezadoTablaOrden2)
                            {
                                x = 20; // Reiniciar x para la siguiente fila

                                foreach (var valor in fila)
                                {
                                    // Dibujar borde de la celda con el color definido
                                    paint.Color = celdaBorderColor;
                                    canvas.DrawRect(new SKRect(x, y, x + cellWidth14, y + 30), paint);

                                    // Dibujar fondo de celda con color Rojo
                                    paint.Color = celdaBackgroundColorc;
                                    canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth14 - 1, y + 30 - 1), paint);

                                    // Dibujar valor (texto) en negro
                                    paint.Color = SKColors.Black;
                                    canvas.DrawText(valor, x + 10, y + 20, paint);
                                    x += cellWidth14 + 10; // Espacio entre celdas
                                }
                                x = 20;

                            }

                            x = 50;
                            y += 43;

                            for (int i = 0; i < 3; i++)
                            {
                                // Reiniciar x y ajustar la posición vertical para la tabla
                                 // Ajusta la posición vertical para la tabla

                                // Datos de la tabla con celdas más anchas
                                string[][] tablaImagen = new string[][]
                                {
                                    new string[] { "Nombre de la pieza: ", " Datos del golpe" },
                                    new string[] { "prueba.jpeg","Diametro Horizontal:\n Diametro Vertical:\n Profundidad: " },
                                };
                                // Ajusta el ancho y alto de las celdas
                                float cellWidth4 = 284; // Cambia el valor a un ancho deseado
                                float cellHeight = 57; // Altura de la celda

                                foreach (var fila in tablaImagen)
                                {
                                    x = 20; // Reiniciar x para la siguiente fila

                                    float maxCeldaHeight = 0; // Variable para rastrear la altura máxima de las celdas en esta fila

                                    foreach (var valor in fila)
                                    {
                                        // Divide el valor en líneas si contiene saltos de línea
                                        var lineas = valor.Split('\n');

                                        // Calcula la altura de la celda en función del número de líneas
                                        float celdaHeight = cellHeight * lineas.Length;

                                        // Actualiza la altura máxima de las celdas en esta fila
                                        maxCeldaHeight = Math.Max(maxCeldaHeight, celdaHeight);
                                    }

                                    foreach (var valor in fila)
                                    {
                                        // Divide el valor en líneas si contiene saltos de línea
                                        var lineas = valor.Split('\n');

                                        // Dibujar borde de la celda con el color definido
                                        paint.Color = celdaBorderColor;
                                        canvas.DrawRect(new SKRect(x, y, x + cellWidth4, y + maxCeldaHeight), paint);

                                        // Dibujar fondo de celda con color Rojo
                                        paint.Color = celdaBackgroundColorB;
                                        canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth4 - 1, y + maxCeldaHeight - 1), paint);

                                        // Ajusta las coordenadas y el tamaño según sea necesario
                                        float currentY = y;

                                        paint.Color = SKColors.Black;
                                        foreach (var linea in lineas)
                                        {
                                            if (linea.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) || linea.EndsWith(".png", StringComparison.OrdinalIgnoreCase) || linea.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                                            {

                                                if (DatoImagen != null)
                                                {
                                                    using (var imageStream = new MemoryStream(DatoImagen))
                                                    {
                                                        using (var bitmap = SKBitmap.Decode(imageStream))
                                                        {
                                                            var imageWidth = cellWidth4 - 20; // Ancho de la imagen igual al ancho de la celda menos un espacio para los márgenes
                                                            var imageHeight = maxCeldaHeight - 10; // Altura de la imagen igual a la altura de la celda menos un espacio para los márgenes

                                                            canvas.DrawBitmap(bitmap, new SKRect(x + 10, currentY + 5, x + 10 + imageWidth, currentY + 5 + imageHeight));
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                canvas.DrawText(linea, x + 10, currentY + 20, paint);
                                            }

                                            currentY += cellHeight; // Ajusta la posición vertical para la siguiente línea
                                        }

                                        x += cellWidth4 + 10; // Espacio entre celdas
                                    }

                                    x = 20;
                                    y += maxCeldaHeight + 0; // Espacio entre filas (se ajusta usando maxCeldaHeight para considerar la altura máxima de la celda)
                                }
                                x = 20;
                                y += 17;

                            }


  






                        }




                        // Finaliza la segunda página en blanco
                        document.EndPage();
                    }
                    document.Close();
                }

                // Abrir el archivo PDF generado
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filePath)
                });
            }
        }


        public async void GenerarPDFGolpeFuerte(byte[] DatoImagen)
        {

            string randomTicketName = "ticket_" + GenerateRandomNumbers(5) + ".pdf";
            var filePath = Path.Combine(FileSystem.CacheDirectory, randomTicketName);
            var listaCadenas = contenido.Split('\n');
            int num = listaCadenas.Length;
            float pointsPerInch = 72; // 72 puntos por pulgada
            float width = 8.5f * pointsPerInch; // 8.5 pulgadas en puntos
            float height = 11f * pointsPerInch; // 11 pulgadas en puntos

            using (var stream = new SKFileWStream(filePath))
            using (var document = SKDocument.CreatePdf(stream))
            using (var canvas = document.BeginPage(width, height))
            {
                // Establecer el fondo
                canvas.Clear(SKColors.White);


                // Load the image from the embedded resources
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "GenerarPDF.Resources.Images.logo.png";
                using (var resourceStream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (resourceStream != null)
                    {
                        using (var bitmap = SKBitmap.Decode(resourceStream))
                        {
                            // Convertir SKBitmap a SKImage
                            var image = SKImage.FromBitmap(bitmap);

                            // Obtener el tamaño del PDF
                            var pdfWidth = 400; // Ancho del PDF


                            // Calcular el ancho y alto de la imagen para ajustar al ancho del PDF y mantener la proporción
                            float imageWidth = pdfWidth / 4.00f;
                            float imageHeight = bitmap.Height * (imageWidth / bitmap.Width);

                            // Calcular la posición para colocar la imagen al principio del PDF
                            float imageX = (pdfWidth - imageWidth) / 10; // Centrar la imagen horizontalmente
                            float imageY = 0; // Margen de 50 unidades desde arriba del PDF

                            // Dibujar la imagen en el canvas
                            canvas.DrawImage(image, new SKRect(imageX, imageY, imageX + imageWidth, imageY + imageHeight));
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Image resource not found.");
                    }

                    // Establecer estilo de texto
                    using (var paint = new SKPaint())
                    {
                        paint.TextSize = 14;
                        paint.Color = SKColors.Black; // Color del texto
                                                      // Definir el color del fondo de las celdas
                        SKColor celdaBackgroundColor = SKColors.Coral;
                        SKColor celdaBackgroundColorB = SKColors.White;
                        SKColor celdaBackgroundColorc = SKColors.Gold;
                        SKColor celdaBorderColor = SKColors.Black;
                        // Configurar las coordenadas iniciales para la primera tabla
                        float x = 20;
                        float y = 20; // Puedes ajustar la posición vertical

                        // Datos de la tabla con celdas más anchas
                        string[][] encabezadoTablaOrden = new string[][]
                        {
                        new string[] { "                                        Orden de Admisión de Cotización "},

                        };

                        // Ajusta el ancho de las celdas
                        float cellWidtho = 450; // Cambia el valor a un ancho deseado


                        // Iterar sobre los datos de la  tabla y dibujar celdas
                        foreach (var fila in encabezadoTablaOrden)
                        {
                            x = 148; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidtho, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorc;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidtho - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidtho + 10; // Espacio entre celdas
                            }
                            x = 20;

                        }

                        x = 20;  // Reiniciar x para la siguiente fila
                        y += 35; // Ajusta la posición vertical para la tercera tabla

                        // Datos de la tabla con celdas más anchas
                        string[][] TablaFolio = new string[][]
                        {
                        new string[] { "Folio:"}

                        };


                        // Ajusta el ancho de las celdas
                        float cellWidthf = 170; // Cambia el valor a un ancho deseado


                        // Iterar sobre los datos de la  tabla y dibujar celdas
                        foreach (var fila in TablaFolio)
                        {
                            x = 428; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidthf, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorB;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidthf - 1, y + 30 - 1), paint);




                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidthf + 10; // Espacio entre celdas
                            }
                            x = 80;
                            y += 25; // Espacio entre filas
                        }


                        x = 20;  // Reiniciar x para la siguiente fila
                        y += 15; // Ajusta la posición vertical para la tercera tabla

                        // Datos de la tabla con celdas más anchas
                        string[][] encabezadoTablaDatosVehiculos = new string[][]
                        {
                        new string[] { "                                                             Datos del vehículo     "},

                        };

                        // Ajusta el ancho de las celdas
                        float cellWidthe = 578; // Cambia el valor a un ancho deseado


                        // Iterar sobre los datos de la  tabla y dibujar celdas
                        foreach (var fila in encabezadoTablaDatosVehiculos)
                        {
                            x = 20; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidthe, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColor;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidthe - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidthe + 10; // Espacio entre celdas
                            }
                            x = 20;
                            y += 25; // Espacio entre filas
                        }




                        x = 20;  // Reiniciar x para la siguiente fila
                        y += 15; // Ajusta la posición vertical para la tercera tabla


                        // Datos de la primera tabla
                        string[][] primeraTablaDatosVehiculos = new string[][]
                        {
                        new string[] { "Marca:", "Submarca:", "Modelo:", "Tipo:" },
                        new string[] { "------", "-----", "-----", "-----" },
                        };

                        float cellWidth1 = 137; // Cambia el valor a un ancho deseado


                        // Iterar sobre los datos de la primera tabla y dibujar celdas
                        foreach (var fila in primeraTablaDatosVehiculos)
                        {
                            x = 20; // Reiniciar x para la siguiente fila
                            foreach (var valor in fila)
                            {


                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidth1, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorB;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth1 - 1, y + 30 - 1), paint);




                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidth1 + 10; // Espacio entre celdas
                            }
                            x = 20;
                            y += 25; // Espacio entre filas
                        }





                        // Reiniciar x y ajustar la posición vertical para la segunda tabla
                        x = 20;
                        y += 15; // Ajusta la posición vertical para la segunda tabla

                        // Datos de la segunda tabla
                        string[][] segundaTablaDatosVehiculos = new string[][]
                        {
                        new string[] { "Versión:", "Categoría", "Color", "Acabado" },
                        new string[] { "-----------", "-----------", "-----------", "-----------" },
                        };

                        float cellWidth2 = 137; // Cambia el valor a un ancho deseado

                        // Iterar sobre los datos de la segunda tabla y dibujar celdas
                        foreach (var fila in segundaTablaDatosVehiculos)
                        {
                            x = 20; // Reiniciar x para la siguiente fila
                            foreach (var valor in fila)
                            {
                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidth2, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorB;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth2 - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidth2 + 10; // Espacio entre celdas
                            }
                            x = 30; // Reiniciar x para la siguiente fila
                            y += 25; // Espacio entre filas
                        }




                        // Reiniciar x y ajustar la posición vertical para la segunda tabla
                        x = 20;
                        y += 15; // Ajusta la posición vertical para la otra tabla


                        // Datos de la tabla con celdas más anchas
                        string[][] encabezadoTablaServicio = new string[][]
                        {
                        new string[] { "                                                        Datos del Servicio      "},

                        };

                        // Ajusta el ancho de las celdas
                        float cellWidthes = 578; // Cambia el valor a un ancho deseado


                        // Iterar sobre los datos de la  tabla y dibujar celdas
                        foreach (var fila in encabezadoTablaServicio)
                        {
                            x = 20; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {

                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidthes, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColor;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidthes - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidthes + 10; // Espacio entre celdas
                            }
                            x = 20;
                            y += 25; // Espacio entre filas
                        }




                        // Reiniciar x y ajustar la posición vertical para la  tabla
                        // Reiniciar x y ajustar la posición vertical para la  tabla
                        x = 20;
                        y += 15; // Ajusta la posición vertical para la tabla

                        // Datos de la  tabla con celdas más anchas
                        string[][] primeraTablaDatosServicio = new string[][]
                        {
                        new string[] { "Tipo Servicio:", "Prioridad:", "Tipo golpe:"},
                        new string[] { $"", $"", $""},
                                            };

                        // Ajusta el ancho de las celdas
                        float cellWidth3 = 186; // Cambia el valor a un ancho deseado

                        // Iterar sobre los datos de la tabla y dibujar celdas
                        foreach (var fila in primeraTablaDatosServicio)
                        {
                            x = 20; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidth3, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorB;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth3 - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidth3 + 10; // Espacio entre celdas
                            }
                            x = 20;
                            y += 25; // Espacio entre filas
                        }


                        // Reiniciar x y ajustar la posición vertical para la tabla
                        x = 20;
                        y += 15; // Ajusta la posición vertical para la tabla

                        // Datos de la tabla con celdas más anchas
                        string[][] segundaTablaDatosServicio = new string[][]
                        {
                        new string[] { $" Dirección del Taller:" },
                        new string[] { "" },
                        };

                        // Ajusta el ancho y alto de las celdas
                        float cellWidth4 = 578; // Cambia el valor a un ancho deseado
                        float cellHeight = 75; // Altura de la celda

                        // Iterar sobre los datos de la tabla y dibujar celdas
                        foreach (var fila in segundaTablaDatosServicio)
                        {
                            x = 20; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Divide el valor en líneas si contiene saltos de línea
                                var lineas = valor.Split('\n');

                                // Calcula la altura de la celda en función del número de líneas
                                float celdaHeight = cellHeight * lineas.Length;

                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidth4, y + celdaHeight), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorB;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth4 - 1, y + celdaHeight - 1), paint);

                                // Dibujar cada línea en la celda
                                paint.Color = SKColors.Black;
                                foreach (var linea in lineas)
                                {
                                    canvas.DrawText(linea, x + 10, y + 20, paint);
                                    y += cellHeight; // Ajusta la posición vertical para la siguiente línea
                                }

                                x += cellWidth4 + 10; // Espacio entre celdas
                                y -= cellHeight * lineas.Length; // Ajusta la posición vertical para la siguiente celda
                            }

                            x = 20;
                            y += 25; // Espacio entre filas
                        }



                        // Reiniciar x y ajustar la posición vertical para la  tabla
                        x = 20;
                        y += 60; // Ajusta la posición vertical para la tabla

                        // Datos de la  tabla con celdas más anchas
                        string[][] terceraTablaDatosServicio = new string[][]
                        {
                        new string[] { "Fecha:", "Hora:" },
                        new string[] { " -------", "-----" },
                                            };

                        // Ajusta el ancho de las celdas
                        float cellWidth5 = 284; // Cambia el valor a un ancho deseado

                        // Iterar sobre los datos de la tabla y dibujar celdas
                        foreach (var fila in terceraTablaDatosServicio)
                        {
                            x = 20; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidth5, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorB;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth5 - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidth5 + 10; // Espacio entre celdas
                            }
                            x = 20;
                            y += 25; // Espacio entre filas
                        }


                        // Reiniciar x y ajustar la posición vertical para la segunda tabla
                        x = 20;
                        y += 15; // Ajusta la posición vertical para la otra tabla


                        // Datos de la tabla con celdas más anchas
                        string[][] encabezadoTablaUsuario = new string[][]
                        {
                        new string[] { "                                                           Datos del Usuario    "},

                        };

                        // Ajusta el ancho de las celdas
                        float cellWidtheu = 578; // Cambia el valor a un ancho deseado


                        // Iterar sobre los datos de la  tabla y dibujar celdas
                        foreach (var fila in encabezadoTablaUsuario)
                        {
                            x = 20; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {

                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidtheu, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColor;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidtheu - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidtheu + 10; // Espacio entre celdas
                            }
                            x = 20;
                            y += 25; // Espacio entre filas
                        }

                        // Reiniciar x y ajustar la posición vertical para la  tabla
                        x = 20;
                        y += 15; // Ajusta la posición vertical para la tabla

                        // Datos de la  tabla con celdas más anchas
                        string[][] primeraTablaDatosUsuarios = new string[][]
                        {
                        new string[] { "Nombre completo:", "Correo electronico" },
                        new string[] { " -------", "-----"},
                                            };

                        // Ajusta el ancho de las celdas
                        float cellWidth6 = 284; // Cambia el valor a un ancho deseado

                        // Iterar sobre los datos de la tabla y dibujar celdas
                        foreach (var fila in primeraTablaDatosUsuarios)
                        {
                            x = 20; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidth6, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorB;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth6 - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidth6 + 10; // Espacio entre celdas
                            }
                            x = 20;
                            y += 25; // Espacio entre filas
                        }

                        // Reiniciar x y ajustar la posición vertical para la  tabla
                        x = 20;
                        y += 20; // Ajusta la posición vertical para la tabla

                        // Datos de la  tabla con celdas más anchas
                        string[][] segundaTablaDatosUsuarios = new string[][]
                        {
                        new string[] { "Telefono:", "Estado","Municipio" },
                        new string[] { " -------", "-----","--------"},
                                            };

                        // Ajusta el ancho de las celdas
                        float cellWidth7 = 186; // Cambia el valor a un ancho deseado

                        // Iterar sobre los datos de la tabla y dibujar celdas
                        foreach (var fila in segundaTablaDatosUsuarios)
                        {
                            x = 20; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidth7, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorB;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth7 - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidth7 + 10; // Espacio entre celdas
                            }
                            x = 20;
                            y += 25; // Espacio entre filas
                        }


                    }

                    // Finalizar el documento
                    document.EndPage();

                    using (var canvas2 = document.BeginPage(width, height))
                    {
                        // Deja esta página en blanco, o dibuja cualquier cosa que desees en ella.
                        canvas.Clear(SKColors.White);

                        var assembly2 = Assembly.GetExecutingAssembly();

                        // Establecer estilo de texto
                        using (var paint = new SKPaint())
                        {
                            paint.TextSize = 14;
                            paint.Color = SKColors.Black; // Color del texto
                                                          // Definir el color del fondo de las celdas
                            SKColor celdaBackgroundColor = SKColors.Coral;
                            SKColor celdaBackgroundColorB = SKColors.White;
                            SKColor celdaBackgroundColorc = SKColors.Gold;
                            SKColor celdaBorderColor = SKColors.Black;
                            // Configurar las coordenadas iniciales para la primera tabla
                            float x = 20;
                            float y = 15; // Puedes ajustar la posición vertical

                            // Datos de la tabla con celdas más anchas
                            string[][] encabezadoTablaOrden2 = new string[][]
                            {
                                 new string[] { "                                                            Imagenes del Vehículo  "},

                            };

                            // Ajusta el ancho de las celdas
                            float cellWidth14 = 578; // Cambia el valor a un ancho deseado


                            // Iterar sobre los datos de la  tabla y dibujar celdas
                            foreach (var fila in encabezadoTablaOrden2)
                            {
                                x = 20; // Reiniciar x para la siguiente fila

                                foreach (var valor in fila)
                                {
                                    // Dibujar borde de la celda con el color definido
                                    paint.Color = celdaBorderColor;
                                    canvas.DrawRect(new SKRect(x, y, x + cellWidth14, y + 30), paint);

                                    // Dibujar fondo de celda con color Rojo
                                    paint.Color = celdaBackgroundColorc;
                                    canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth14 - 1, y + 30 - 1), paint);

                                    // Dibujar valor (texto) en negro
                                    paint.Color = SKColors.Black;
                                    canvas.DrawText(valor, x + 10, y + 20, paint);
                                    x += cellWidth14 + 10; // Espacio entre celdas
                                }
                                x = 20;

                            }

                            x = 50;
                            y += 43;

                            // Reiniciar x y ajustar la posición vertical para la tabla
                            // Ajusta la posición vertical para la tabla

                            // Datos de la tabla con celdas más anchas
                            string[][] tablaImagen = new string[][]
                            {
                                    //new string[] { "Nombre de la pieza: ", " Datos del golpe" },
                                    new string[] { "prueba.jpeg"},
                            };
                            // Ajusta el ancho y alto de las celdas
                            float cellWidth4 = 284; // Cambia el valor a un ancho deseado
                            float cellHeight = 225; // Altura de la celda

                            foreach (var fila in tablaImagen)
                            {
                                x = 160; // Reiniciar x para la siguiente fila

                                float maxCeldaHeight = 0; // Variable para rastrear la altura máxima de las celdas en esta fila

                                foreach (var valor in fila)
                                {
                                    // Divide el valor en líneas si contiene saltos de línea
                                    var lineas = valor.Split('\n');

                                    // Calcula la altura de la celda en función del número de líneas
                                    float celdaHeight = cellHeight * lineas.Length;

                                    // Actualiza la altura máxima de las celdas en esta fila
                                    maxCeldaHeight = Math.Max(maxCeldaHeight, celdaHeight);
                                }

                                foreach (var valor in fila)
                                {
                                    // Divide el valor en líneas si contiene saltos de línea
                                    var lineas = valor.Split('\n');

                                    // Dibujar borde de la celda con el color definido
                                    paint.Color = celdaBorderColor;
                                    canvas.DrawRect(new SKRect(x, y, x + cellWidth4, y + maxCeldaHeight), paint);

                                    // Dibujar fondo de celda con color Rojo
                                    paint.Color = celdaBackgroundColorB;
                                    canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth4 - 1, y + maxCeldaHeight - 1), paint);

                                    // Ajusta las coordenadas y el tamaño según sea necesario
                                    float currentY = y;

                                    paint.Color = SKColors.Black;
                                    foreach (var linea in lineas)
                                    {
                                        if (linea.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) || linea.EndsWith(".png", StringComparison.OrdinalIgnoreCase) || linea.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                                        {

                                            if (DatoImagen != null)
                                            {
                                                using (var imageStream = new MemoryStream(DatoImagen))
                                                {
                                                    using (var bitmap = SKBitmap.Decode(imageStream))
                                                    {
                                                        var imageWidth = cellWidth4 - 20; // Ancho de la imagen igual al ancho de la celda menos un espacio para los márgenes
                                                        var imageHeight = maxCeldaHeight - 10; // Altura de la imagen igual a la altura de la celda menos un espacio para los márgenes

                                                        canvas.DrawBitmap(bitmap, new SKRect(x + 10, currentY + 5, x + 10 + imageWidth, currentY + 5 + imageHeight));
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            canvas.DrawText(linea, x + 10, currentY + 20, paint);
                                        }

                                        currentY += cellHeight; // Ajusta la posición vertical para la siguiente línea
                                    }

                                    x += cellWidth4 + 10; // Espacio entre celdas
                                }

                                x = 20;
                                y += maxCeldaHeight + 0; // Espacio entre filas (se ajusta usando maxCeldaHeight para considerar la altura máxima de la celda)
                            }
                            x = 20;
                            y += 17;









                        }




                        // Finaliza la segunda página en blanco
                        document.EndPage();
                    }

                    document.Close();
                }

                // Abrir el archivo PDF generado
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filePath)
                });
            }
        }


        public async void GenerarPDFSinDatos(byte[] DatoImagen)
        {

            string randomTicketName = "ticket_" + GenerateRandomNumbers(5) + ".pdf";
            var filePath = Path.Combine(FileSystem.CacheDirectory, randomTicketName);
            var listaCadenas = contenido.Split('\n');
            int num = listaCadenas.Length;
            float pointsPerInch = 72; // 72 puntos por pulgada
            float width = 8.5f * pointsPerInch; // 8.5 pulgadas en puntos
            float height = 11f * pointsPerInch; // 11 pulgadas en puntos

            using (var stream = new SKFileWStream(filePath))
            using (var document = SKDocument.CreatePdf(stream))
            using (var canvas = document.BeginPage(width, height))
            {
                // Establecer el fondo
                canvas.Clear(SKColors.White);


                // Load the image from the embedded resources
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "GenerarPDF.Resources.Images.logo.png";
                using (var resourceStream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (resourceStream != null)
                    {
                        using (var bitmap = SKBitmap.Decode(resourceStream))
                        {
                            // Convertir SKBitmap a SKImage
                            var image = SKImage.FromBitmap(bitmap);

                            // Obtener el tamaño del PDF
                            var pdfWidth = 400; // Ancho del PDF


                            // Calcular el ancho y alto de la imagen para ajustar al ancho del PDF y mantener la proporción
                            float imageWidth = pdfWidth / 4.00f;
                            float imageHeight = bitmap.Height * (imageWidth / bitmap.Width);

                            // Calcular la posición para colocar la imagen al principio del PDF
                            float imageX = (pdfWidth - imageWidth) / 10; // Centrar la imagen horizontalmente
                            float imageY = 0; // Margen de 50 unidades desde arriba del PDF

                            // Dibujar la imagen en el canvas
                            canvas.DrawImage(image, new SKRect(imageX, imageY, imageX + imageWidth, imageY + imageHeight));
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Image resource not found.");
                    }

                    // Establecer estilo de texto
                    using (var paint = new SKPaint())
                    {
                        paint.TextSize = 14;
                        paint.Color = SKColors.Black; // Color del texto
                                                      // Definir el color del fondo de las celdas
                        SKColor celdaBackgroundColor = SKColors.Coral;
                        SKColor celdaBackgroundColorB = SKColors.White;
                        SKColor celdaBackgroundColorc = SKColors.Gold;
                        SKColor celdaBorderColor = SKColors.Black;
                        // Configurar las coordenadas iniciales para la primera tabla
                        float x = 20;
                        float y = 20; // Puedes ajustar la posición vertical

                        // Datos de la tabla con celdas más anchas
                        string[][] encabezadoTablaOrden = new string[][]
                        {
                        new string[] { "                                        Orden de Admisión de Cotización "},

                        };

                        // Ajusta el ancho de las celdas
                        float cellWidtho = 450; // Cambia el valor a un ancho deseado


                        // Iterar sobre los datos de la  tabla y dibujar celdas
                        foreach (var fila in encabezadoTablaOrden)
                        {
                            x = 148; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidtho, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorc;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidtho - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidtho + 10; // Espacio entre celdas
                            }
                            x = 20;

                        }

                        x = 20;  // Reiniciar x para la siguiente fila
                        y += 35; // Ajusta la posición vertical para la tercera tabla

                        // Datos de la tabla con celdas más anchas
                        string[][] TablaFolio = new string[][]
                        {
                        new string[] { "Folio:"}

                        };


                        // Ajusta el ancho de las celdas
                        float cellWidthf = 170; // Cambia el valor a un ancho deseado


                        // Iterar sobre los datos de la  tabla y dibujar celdas
                        foreach (var fila in TablaFolio)
                        {
                            x = 428; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidthf, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorB;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidthf - 1, y + 30 - 1), paint);




                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidthf + 10; // Espacio entre celdas
                            }
                            x = 80;
                            y += 25; // Espacio entre filas
                        }


                        x = 20;  // Reiniciar x para la siguiente fila
                        y += 15; // Ajusta la posición vertical para la tercera tabla

                       

                        // Datos de la tabla con celdas más anchas
                        string[][] encabezadoTablaServicio = new string[][]
                        {
                        new string[] { "                                                        Datos del Servicio      "},

                        };

                        // Ajusta el ancho de las celdas
                        float cellWidthes = 578; // Cambia el valor a un ancho deseado


                        // Iterar sobre los datos de la  tabla y dibujar celdas
                        foreach (var fila in encabezadoTablaServicio)
                        {
                            x = 20; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {

                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidthes, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColor;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidthes - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidthes + 10; // Espacio entre celdas
                            }
                            x = 20;
                            y += 25; // Espacio entre filas
                        }




                        // Reiniciar x y ajustar la posición vertical para la  tabla
                        // Reiniciar x y ajustar la posición vertical para la  tabla
                        x = 20;
                        y += 15; // Ajusta la posición vertical para la tabla

                        // Datos de la  tabla con celdas más anchas
                        string[][] primeraTablaDatosServicio = new string[][]
                        {
                        new string[] { "Nombre del taller:", "Teléfono:", "Correo electrónico:"},
                        new string[] { $"", $"", $""},
                                            };

                        // Ajusta el ancho de las celdas
                        float cellWidth3 = 186; // Cambia el valor a un ancho deseado

                        // Iterar sobre los datos de la tabla y dibujar celdas
                        foreach (var fila in primeraTablaDatosServicio)
                        {
                            x = 20; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidth3, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorB;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth3 - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidth3 + 10; // Espacio entre celdas
                            }
                            x = 20;
                            y += 25; // Espacio entre filas
                        }


                        x = 20;
                        y += 15; // Ajusta la posición vertical para la tabla

                        // Datos de la  tabla con celdas más anchas
                        string[][] terceraTablaDatosServicio = new string[][]
                        {
                        new string[] { "Encargado:", "Fecha de la cita:", "Hora de la cita:"},
                        new string[] { $"", $"", $""},
                                            };


                        // Iterar sobre los datos de la tabla y dibujar celdas
                        foreach (var fila in terceraTablaDatosServicio)
                        {
                            x = 20; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidth3, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorB;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth3 - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidth3 + 10; // Espacio entre celdas
                            }
                            x = 20;
                            y += 25; // Espacio entre filas
                        }

                        // Reiniciar x y ajustar la posición vertical para la tabla
                        x = 20;
                        y += 15; // Ajusta la posición vertical para la tabla

                        // Datos de la tabla con celdas más anchas
                        string[][] segundaTablaDatosServicio = new string[][]
                        {
                        new string[] { $"Dirección del Taller:" },
                        new string[] { "" },
                        };

                        // Ajusta el ancho y alto de las celdas
                        float cellWidth4 = 578; // Cambia el valor a un ancho deseado
                        float cellHeight = 75; // Altura de la celda

                        // Iterar sobre los datos de la tabla y dibujar celdas
                        foreach (var fila in segundaTablaDatosServicio)
                        {
                            x = 20; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Divide el valor en líneas si contiene saltos de línea
                                var lineas = valor.Split('\n');

                                // Calcula la altura de la celda en función del número de líneas
                                float celdaHeight = cellHeight * lineas.Length;

                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidth4, y + celdaHeight), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorB;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth4 - 1, y + celdaHeight - 1), paint);

                                // Dibujar cada línea en la celda
                                paint.Color = SKColors.Black;
                                foreach (var linea in lineas)
                                {
                                    canvas.DrawText(linea, x + 10, y + 20, paint);
                                    y += cellHeight; // Ajusta la posición vertical para la siguiente línea
                                }

                                x += cellWidth4 + 10; // Espacio entre celdas
                                y -= cellHeight * lineas.Length; // Ajusta la posición vertical para la siguiente celda
                            }

                            x = 20;
                            y += 25; // Espacio entre filas
                        }

                        x = 20;
                        y += 65; // Ajusta la posición vertical para la otra tabla


                        // Datos de la tabla con celdas más anchas
                        string[][] encabezadoTablaUsuario = new string[][]
                        {
                        new string[] { "                                                           Datos del Usuario    "},

                        };

                        // Ajusta el ancho de las celdas
                        float cellWidtheu = 578; // Cambia el valor a un ancho deseado


                        // Iterar sobre los datos de la  tabla y dibujar celdas
                        foreach (var fila in encabezadoTablaUsuario)
                        {
                            x = 20; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {

                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidtheu, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColor;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidtheu - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidtheu + 10; // Espacio entre celdas
                            }
                            x = 20;
                            y += 25; // Espacio entre filas
                        }

                        // Reiniciar x y ajustar la posición vertical para la  tabla
                        x = 20;
                        y += 15; // Ajusta la posición vertical para la tabla

                        // Datos de la  tabla con celdas más anchas
                        string[][] primeraTablaDatosUsuarios = new string[][]
                        {
                        new string[] { "Nombre completo:", "Correo electronico" },
                        new string[] { " -------", "-----"},
                                            };

                        // Ajusta el ancho de las celdas
                        float cellWidth6 = 284; // Cambia el valor a un ancho deseado

                        // Iterar sobre los datos de la tabla y dibujar celdas
                        foreach (var fila in primeraTablaDatosUsuarios)
                        {
                            x = 20; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidth6, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorB;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth6 - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidth6 + 10; // Espacio entre celdas
                            }
                            x = 20;
                            y += 25; // Espacio entre filas
                        }

                        // Reiniciar x y ajustar la posición vertical para la  tabla
                        x = 20;
                        y += 20; // Ajusta la posición vertical para la tabla

                        // Datos de la  tabla con celdas más anchas
                        string[][] segundaTablaDatosUsuarios = new string[][]
                        {
                        new string[] { "Telefono:", "Estado","Municipio" },
                        new string[] { " -------", "-----","--------"},
                                            };

                        // Ajusta el ancho de las celdas
                        float cellWidth7 = 186; // Cambia el valor a un ancho deseado

                        // Iterar sobre los datos de la tabla y dibujar celdas
                        foreach (var fila in segundaTablaDatosUsuarios)
                        {
                            x = 20; // Reiniciar x para la siguiente fila

                            foreach (var valor in fila)
                            {
                                // Dibujar borde de la celda con el color definido
                                paint.Color = celdaBorderColor;
                                canvas.DrawRect(new SKRect(x, y, x + cellWidth7, y + 30), paint);

                                // Dibujar fondo de celda con color Rojo
                                paint.Color = celdaBackgroundColorB;
                                canvas.DrawRect(new SKRect(x + 1, y + 1, x + cellWidth7 - 1, y + 30 - 1), paint);

                                // Dibujar valor (texto) en negro
                                paint.Color = SKColors.Black;
                                canvas.DrawText(valor, x + 10, y + 20, paint);
                                x += cellWidth7 + 10; // Espacio entre celdas
                            }
                            x = 20;
                            y += 25; // Espacio entre filas
                        }






                    }

                    // Finalizar el documento
                    document.EndPage();

                    document.Close();
                }

                // Abrir el archivo PDF generado
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filePath)
                });
            }
        }


    }
}

