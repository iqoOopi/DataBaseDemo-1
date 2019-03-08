using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataBaseDemo_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //user clicks on the form
        private void productsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Validate(); //controls on the form are validate
                this.productsBindingSource.EndEdit();//finalizes changes (positive)
                this.tableAdapterManager.UpdateAll(this.mMABooksDataSet);
                //update databse tables
            }
            catch (DBConcurrencyException)
            {
                MessageBox.Show("Another user changed data in the meantime, Try agian");
                this.productsTableAdapter.Fill(this.mMABooksDataSet.Products);

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error while saving data:" + ex.Message);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Other error while saving data: " + ex.Message);
            }
        } 

        private void Form1_Load(object sender, EventArgs e)
        {
            //products table filled from the database
            try
            {
                this.productsTableAdapter.Fill(this.mMABooksDataSet.Products);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error while saving data:" + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Other error while saving data: " + ex.Message);
            }


        }

        //error within the grid view
        private void productsDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            int row = e.RowIndex + 1;
            int col = e.ColumnIndex + 1;
            MessageBox.Show("Bad data in row"+row+" and column"+col);
        }

        private void CalculateInventory()
        {
            decimal total = 0;
            foreach(DataRow dr in mMABooksDataSet.Tables["Products"].Rows)
            {
                int qty = (int) dr["OnHandQuantity"];
                decimal price = (decimal) dr["UnitPrice"];
                total += price * qty;
            }

        }
    }
}
