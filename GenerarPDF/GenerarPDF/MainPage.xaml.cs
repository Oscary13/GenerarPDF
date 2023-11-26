using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GenerarPDF
{
    public partial class MainPage : ContentPage
    {

        private string selectedImagePath;
        public MainPage()
        {
            InitializeComponent();
        }

        private async void selectedImage_Clicked(object sender, EventArgs e)
        {
            // Abre el selector de archivos para elegir una imagen
            var result = await Xamarin.Essentials.FilePicker.PickAsync(new Xamarin.Essentials.PickOptions
            {
                FileTypes = Xamarin.Essentials.FilePickerFileType.Images,
                PickerTitle = "Seleccionar imagen"
            });

            // Si se selecciona una imagen, actualiza la ruta y la muestra
            if (result != null)
            {
                selectedImagePath = result.FullPath;
                DatoImagen = ConvertImageToByteArray(result.FullPath);
                selectedImage.Source = ConvertByteArrayToImage(DatoImagen);

                
            }
        }

        public byte[] DatoImagen { get; set; }

        private byte[] ConvertImageToByteArray(string imagePath)
        {
            byte[] imageArray = null;

            try
            {
                using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        long numBytes = new FileInfo(imagePath).Length;
                        imageArray = br.ReadBytes((int)numBytes);
                    }
                }
            }
            catch (Exception ex)
            {
                // Maneja cualquier excepción que pueda ocurrir durante la conversión
                Console.WriteLine("Error al convertir la imagen a bytes: " + ex.Message);
            }

            return imageArray;
        }


        public ImageSource ConvertByteArrayToImage(byte[] byteArray)
        {
            try
            {
                if (byteArray != null && byteArray.Length > 0)
                {
                    Stream stream = new MemoryStream(byteArray);
                    return ImageSource.FromStream(() => stream);
                }
            }
            catch (Exception ex)
            {
                // Maneja cualquier excepción que pueda ocurrir durante la conversión
                Console.WriteLine("Error al convertir bytes a imagen: " + ex.Message);
            }

            return null;
        }


        private void GenerarPDFButton_Clicked(object sender, EventArgs e)
        {
            string ordenFinal = Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + "_______________________________________________________________________________________________________________" + Environment.NewLine + "                            Auto Universe" + Environment.NewLine;
            ordenFinal += DateTime.Now.ToString().PadLeft(44) + Environment.NewLine;

            PDF pdf = new PDF(ordenFinal);

            // Pasa la ruta de la imagen a GenerarPDF
            //pdf.GenerarPDF(DatoImagen);

            //pdf.GenerarPDFGolpeFuerte(DatoImagen);

            pdf.GenerarPDFSinDatos(DatoImagen);
        }
    }
}


