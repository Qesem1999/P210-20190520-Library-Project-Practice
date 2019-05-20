using Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library
{
    public partial class Orders : Form
    {
        public Orders()
        {
            InitializeComponent();
            FillOrders();
        }

        private void FillOrders()
        {
            using (LibraryDB db = new LibraryDB())
            {
                List<Models.Order> orders = db.Orders.ToList();

                dgvOrders.Rows.Clear();

                foreach (var o in orders)
                {
                    string authorFullname = "";
                    foreach (var item in o.Book.AuthorsBooks)
                    {
                        authorFullname += item.Author.Name + " " + item.Author.Surname;
                    }


                    dgvOrders.Rows.Add(
                        o.Id,
                        o.Client.Name + " " + o.Client.Surname,
                        authorFullname,
                        o.OrderDate.ToString("dd.MM.yyyy hh:mm:ss"),
                        o.ReturnDate.ToString("dd.MM.yyyy hh:mm:ss")
                        );


                    if (DateTime.Now > o.ReturnDate)
                    {
                        dgvOrders.Rows[dgvOrders.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightPink;
                    }

                }
            }
        }


        private void LateOrders()
        {

        }
    }
}
