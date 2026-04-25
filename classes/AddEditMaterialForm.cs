// AddEditMaterialForm.cs
using System;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.Repositories;

namespace WindowsFormsApp1
{
    public partial class AddEditMaterialForm : Form
    {
        private int materialId = -1;
        private MaterialRepository materialRepo;
        private SupplierRepository supplierRepo;

        public AddEditMaterialForm(string connString, int id = -1)
        {
            materialRepo = new MaterialRepository(connString);
            supplierRepo = new SupplierRepository();
            materialId = id;
            InitializeComponent();
            LoadSuppliers();
            if (id > 0)
            {
                this.Text = "Редактирование материала";
                LoadMaterialData(id);
            }
            else
            {
                this.Text = "Добавление материала";
            }
        }

        private void LoadSuppliers()
        {
            var suppliers = supplierRepo.GetAll();
            foreach (var s in suppliers)
                cmbArrival.Items.Add(s.Name);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtBatch.Text))
            {
                MessageBox.Show("Заполните обязательные поля: Название и Партия");
                return;
            }

            try
            {
                var material = new Material
                {
                    Name = txtName.Text,
                    BatchNumber = txtBatch.Text,
                    FullBatchMass = decimal.Parse(txtMass.Text.Replace(".", ",")),
                    Length = decimal.Parse(txtLength.Text.Replace(".", ",")),
                    Quantity = int.Parse(txtQuantity.Text),
                    ArrivalFrom = cmbArrival.Text.Trim(),
                    ConsumptionTo = txtConsumption.Text,
                    ArrivalDate = dtpArrivalDate.Checked && dtpArrivalDate.Value != DateTime.MinValue
                        ? dtpArrivalDate.Value
                        : (DateTime?)null
                };

                int savedMaterialId;
                if (materialId > 0)
                {
                    material.Id = materialId;
                    materialRepo.Update(material);
                    savedMaterialId = materialId;
                }
                else
                {
                    savedMaterialId = materialRepo.Insert(material);
                }

                if (!string.IsNullOrWhiteSpace(cmbArrival.Text))
                {
                    var supplier = supplierRepo.GetAll().FirstOrDefault(s => s.Name == cmbArrival.Text.Trim());
                    if (supplier == null)
                    {
                        var newSupplier = new Supplier { Name = cmbArrival.Text.Trim() };
                        supplierRepo.Insert(newSupplier);
                        supplier = supplierRepo.GetAll().FirstOrDefault(s => s.Name == cmbArrival.Text.Trim());
                    }

                    if (supplier != null)
                    {
                        var msRepo = new MaterialSupplierRepository();
                        msRepo.Insert(savedMaterialId, supplier.Id, material.Quantity, material.ArrivalDate);
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения:\n" + ex.Message);
            }
        }

        private void LoadMaterialData(int id)
        {
            try
            {
                var material = materialRepo.GetById(id);
                if (material != null)
                {
                    txtName.Text = material.Name;
                    txtBatch.Text = material.BatchNumber;
                    txtMass.Text = material.FullBatchMass.ToString();
                    txtLength.Text = material.Length.ToString();
                    txtQuantity.Text = material.Quantity.ToString();
                    cmbArrival.Text = material.ArrivalFrom ?? "";
                    txtConsumption.Text = material.ConsumptionTo ?? "";
                    if (material.ArrivalDate.HasValue && material.ArrivalDate.Value != DateTime.MinValue)
                    {
                        dtpArrivalDate.Checked = true;
                        dtpArrivalDate.Value = material.ArrivalDate.Value;
                    }
                    else
                    {
                        dtpArrivalDate.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных:\n" + ex.Message);
            }
        }
    }
}