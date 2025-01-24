using Application.UsesCases.Facturas.GenerarFactura;
using Application.UsesCases.Servicios.ObtenerServicio;
using Application.UsesCases.Servicios.ObtenerServicios;
using Core.Entities;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Pkcs;

namespace Caja
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            cbMetodoPago.Items.Add("Transferencia");
            cbMetodoPago.Items.Add("Tarjeta");
            cbMetodoPago.Items.Add("Efectivo");

            // Opcional: Seleccionar un valor por defecto
            cbMetodoPago.SelectedIndex = 0;
            await CargarServicios();
        }

        private List<GenerarFacturaDetalleRequest> detallesFactura = new List<GenerarFacturaDetalleRequest>();

        private async Task CargarServicios()
        {
            string url = "https://localhost:7210/api/Servicio/todosServicios";
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var servicios = await response.Content.ReadFromJsonAsync<List<ObtenerServiciosResponse>>();

                        if (servicios != null)
                        {
                            cbServicios.DataSource = servicios;
                            cbServicios.DisplayMember = "Nombre"; // Propiedad que quieres mostrar
                            cbServicios.ValueMember = "Precio";           // Propiedad que usarás como valor
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error al obtener los servicios de la API.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error al cargar los servicios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAgregarServicio_Click(object sender, EventArgs e)
        {

            if (cbServicios.SelectedItem != null && int.TryParse(txtCantidadServicio.Text, out int cantidad) && cantidad > 0)
            {
                var servicioSeleccionado = (ObtenerServiciosResponse)cbServicios.SelectedItem;
                // Crear detalle del servicio
                var detalle = new GenerarFacturaDetalleRequest
                {
                    NombreServicio = servicioSeleccionado.Nombre,
                    Cantidad = cantidad,
                    Precio = servicioSeleccionado.Precio
                };

                detallesFactura.Add(detalle);
                ActualizarTabla();
            }
            else
            {
                MessageBox.Show("Selecciona un servicio y especifica una cantidad válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            txtCantidadServicio.Clear();
        }

        private void ActualizarTabla()
        {
            dgvServicios.DataSource = null; // Limpiar la fuente de datos
            dgvServicios.DataSource = detallesFactura; // Asignar la lista de detalles como fuente

            // Configurar las columnas visibles
            dgvServicios.Columns["FacturaId"].Visible = false; // Ocultar FacturaId si no es relevante para el usuario
            dgvServicios.Columns["NombreServicio"].HeaderText = "Servicio";
            dgvServicios.Columns["Cantidad"].HeaderText = "Cantidad";
            dgvServicios.Columns["Precio"].HeaderText = "Precio Unitario";
            dgvServicios.Columns["Precio"].DefaultCellStyle.Format = "C2"; // Mostrar como moneda
            dgvServicios.Columns["Total"].HeaderText = "Total";
            dgvServicios.Columns["Total"].DefaultCellStyle.Format = "C2"; // Formato de moneda

            ActualizarTotal();
        }

        private async Task GenerarFactura()
        {
            try
            {
                var facturaRequest = new GenerarFacturaRequest
                {
                    ClienteTelefono = txtTelefonoCliente.Text,
                    ClienteEmail = txtCorreoCliente.Text,
                    UsuarioTelefono = txtTelefonoEmpleado.Text,
                    UsuarioEmail = txtCorreoEmpleado.Text,
                    Metodo_Pago = cbMetodoPago?.SelectedItem?.ToString(),
                    Detalles = detallesFactura // Usar la lista de detalles
                };

                string url = "https://localhost:7210/api/Factura/generarFactura";

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsJsonAsync(url, facturaRequest);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Factura generada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Error al generar la factura.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarTotal()
        {
            // Calcula la suma total de los servicios
            decimal totalGeneral = detallesFactura.Sum(d => d.Cantidad * d.Precio);

            // Actualiza el label o textbox con el total formateado como moneda
            txtTotal.Text = $"{totalGeneral:C2}";
        }

        private void cbMetodoPago_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void btnFacturar_Click(object sender, EventArgs e)
        {

            await EnviarFacturaAsync();
        }

        private void GenerarFacturaPdf(GenerarFacturaRequest facturaRequest)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var pdfGenerator = new FacturaPdfGenerator();
            var pdfBytes = pdfGenerator.GenerarPdfFactura(facturaRequest);

            // Guardar el PDF en el disco
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Factura.pdf");
            File.WriteAllBytes(filePath, pdfBytes);

            MessageBox.Show($"Factura generada y guardada en: {filePath}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private async Task EnviarFacturaAsync()
        {
            try
            {
                // Crear la solicitud de factura con los datos del formulario
                var facturaRequest = new GenerarFacturaRequest
                {
                    UsuarioEmail = txtCorreoEmpleado.Text,
                    UsuarioTelefono = txtTelefonoEmpleado.Text,
                    ClienteEmail = txtCorreoCliente.Text,
                    ClienteTelefono = txtTelefonoCliente.Text,
                    Metodo_Pago = cbMetodoPago.SelectedItem.ToString(),
                    Detalles = detallesFactura // Lista de servicios seleccionados
                };

                string url = "https://localhost:7210/api/Factura/generar";

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsJsonAsync(url, facturaRequest);

                    if (response.IsSuccessStatusCode)
                    {
                        // Mostrar mensaje de éxito
                        MessageBox.Show("Factura generada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Generar el PDF de la factura
                        GenerarFacturaPdf(facturaRequest);

                        // Limpiar el formulario
                        LimpiarFormulario();
                    }
                    else
                    {
                        // Obtener mensaje de error del servidor
                        var errorContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Error al generar la factura: {errorContent}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al enviar la factura: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LimpiarFormulario()
        {
            txtTelefonoCliente.Clear();
            txtCorreoCliente.Clear();
            txtTelefonoEmpleado.Clear();
            txtCorreoEmpleado.Clear();
            cbMetodoPago.SelectedIndex = 0; // Seleccionar el primer método de pago
            detallesFactura.Clear(); // Limpiar la lista de servicios
            ActualizarTabla(); // Refrescar el DataGridView
        }

        private void GenerarReporteFactura(GenerarFacturaRequest generarFacturaRequest)
        {
            
        }
    }
}
